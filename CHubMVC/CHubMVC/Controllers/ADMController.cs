using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubMVC.Models;
using CHubCommon;
using CHubBLL;
using CHubModel;
using CHubModel.WebArg;
using System.Data;
using CHubDBEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;

namespace CHubMVC.Controllers
{
    public class ADMController : BaseController
    {
        public ActionResult QuickScr()
        {
            //ADMModels vm = new ADMModels();
            //APP_QUICK_SCREEN_BLL bll = new APP_QUICK_SCREEN_BLL();
            //vm.aqs = bll.GetAPPQUICKSCREENList();
            //return View(vm);
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetQUICK_SCREEN()
        {
            APP_QUICK_SCREEN_BLL qsBLL = new APP_QUICK_SCREEN_BLL();
            try
            {
                var result = qsBLL.GetQUICK_SCREEN();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ADM GetQUICK_SCREEN", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        /// <summary>
        /// 根据QUICK_SCREEN获取QUICK_DESC
        /// </summary>
        /// <param name="QUICK_SCREEN"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetQUICK_DESC(string QUICK_SCREEN)
        {
            APP_QUICK_SCREEN_BLL qsBLL = new APP_QUICK_SCREEN_BLL();
            try
            {

                var result = qsBLL.GetTableName(QUICK_SCREEN);
                return Json(new RequestResult(result.QUICK_DESC));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ADM GetQUICK_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetTableResult(string QUICK_SCREEN)
        {
            APP_QUICK_SCREEN_BLL qsBLL = new APP_QUICK_SCREEN_BLL();
            string TABLE_NAME = string.Empty; string PRIMARY_KEY = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                //根据QUICK_SCREEN获取TABLE_NAME
                var table = qsBLL.GetTableName(QUICK_SCREEN);
                TABLE_NAME = table.TABLE_NAME;//TABLE NAME

                PRIMARY_KEY = qsBLL.GetPRIMARY_KEY(TABLE_NAME);//主键

                //根据TABLE_NAME获取表里对应字段
                DataTable dtColumn = qsBLL.GetTableColum(TABLE_NAME);
                //Thead
                GetThead(sb, dtColumn);
                //根据TABLE_NAME获取表里对应数据
                DataTable dtDatas = qsBLL.GetTableDatas(TABLE_NAME);
                //Tbody
                GetTbody(sb, dtColumn, dtDatas, PRIMARY_KEY);
                //Tfoot
                GetTfoot(sb, dtColumn);

                return Json(new RequestResult(sb.ToString()));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ADM GetTableResult", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewLine(string QUICK_SCREEN)
        {
            APP_QUICK_SCREEN_BLL qsBLL = new APP_QUICK_SCREEN_BLL();
            StringBuilder sb = new StringBuilder();
            try
            {
                string TABLE_NAME = qsBLL.GetTableName(QUICK_SCREEN).TABLE_NAME;
                DataTable dtColumn = qsBLL.GetTableColum(TABLE_NAME);

                GetTrNewLine(sb, dtColumn);
                return Json(new RequestResult(sb.ToString()));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ADM NewLine", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateOrInsertQuickScreen(string str, string method, string pkval, string QUICK_SCREEN)
        {
            APP_QUICK_SCREEN_BLL qsBLL = new APP_QUICK_SCREEN_BLL();
            StringBuilder sb = new StringBuilder();
            string sql = string.Empty;
            string columnName = string.Empty; string columnValue = string.Empty; string PRIMARY_KEY = string.Empty; string columnType = string.Empty;
            try
            {
                JObject json = (JObject)JsonConvert.DeserializeObject(str);

                //获取TABLE_NAME
                string TABLE_NAME = qsBLL.GetTableName(QUICK_SCREEN).TABLE_NAME;
                //获取对应表所有字段
                DataTable dtColumn = qsBLL.GetTableColum(TABLE_NAME);

                PRIMARY_KEY = qsBLL.GetPRIMARY_KEY(TABLE_NAME);//主键

                if (method == "Update")//Update
                {
                    sb.Append("Update " + TABLE_NAME + " Set ");
                    foreach (DataRow drColumn in dtColumn.Rows)
                    {
                        string cv = string.Empty;
                        columnName = drColumn["COLUMN_NAME"].ToString();
                        columnType = drColumn["DATA_TYPE"].ToString();
                        columnValue = json[columnName].ToString();
                        if (columnValue.Split('\'').Count() > 1)//当数据有'（单引号）的时候
                        {
                            for (int i = 0; i < columnValue.Split('\'').Count(); i++)
                            {
                                if (i != columnValue.Split('\'').Count() - 1)
                                    cv += columnValue.Split('\'')[i] + "''";
                                else
                                    cv += columnValue.Split('\'')[i];                                
                            }
                            columnValue = cv;
                        }
                        if (columnType == "DATE")
                            sb.Append(" " + columnName + "=to_date('" + columnValue + "','yyyy/mm/dd hh24:mi:ss'),");
                        else
                            sb.Append(" " + columnName + "='" + columnValue + "',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" where " + PRIMARY_KEY + "='" + pkval + "'");
                    sql = sb.ToString();
                }
                else if (method == "Insert")//Insert
                {
                    sb.Append("Insert into " + TABLE_NAME + " (");
                    foreach (DataRow drColumn in dtColumn.Rows)
                    {
                        columnName = drColumn["COLUMN_NAME"].ToString();
                        sb.Append("" + columnName + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(") values (");
                    foreach (DataRow drColumn in dtColumn.Rows)
                    {
                        string cv = string.Empty;
                        columnName = drColumn["COLUMN_NAME"].ToString();
                        columnType = drColumn["DATA_TYPE"].ToString();
                        columnValue = json[columnName].ToString();
                        if (columnValue.Split('\'').Count() > 1)//当数据有'（单引号）的时候
                        {
                            for (int i = 0; i < columnValue.Split('\'').Count(); i++)
                            {
                                if (i != columnValue.Split('\'').Count() - 1)
                                    cv += columnValue.Split('\'')[i] + "''";
                                else
                                    cv += columnValue.Split('\'')[i];
                            }
                            columnValue = cv;
                        }                        
                        if (columnType == "DATE")
                            sb.Append(" to_date('" + columnValue + "','yyyy-mm-dd hh24:mi:ss'),");
                        else
                            sb.Append(" '" + columnValue + "',");                       
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    sql = sb.ToString();
                }
                qsBLL.UpdateOrInsertQuickScreen(sql);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ADM UpdateOrInsertQuickScreen", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }



        public static void GetThead(StringBuilder sb, DataTable dtColumn)
        {
            sb.Append(" <thead>");
            sb.Append("     <tr style='background-color: #f5f5f5;'>");
            sb.Append("         <th style='width:3%'>").Append("Operation").Append("</th>");
            foreach (DataRow dr in dtColumn.Rows)
            {
                sb.Append("         <th>").Append(dr["COLUMN_NAME"].ToString()).Append("</th>");
            }
            sb.Append("     </tr>");
            sb.Append(" </thead>");
        }

        public static void GetTbody(StringBuilder sb, DataTable dtColumn, DataTable dtDatas, string PRIMARY_KEY)
        {
            string columnName = string.Empty; string columnType = string.Empty;
            sb.Append(" <tbody>");
            foreach (DataRow drDatas in dtDatas.Rows)
            {
                sb.Append("     <tr>");
                sb.Append("         <td>").Append("<input type='button' class='btn btn-primary btn-sm btnSave' value='Save' data-primary_key='" + PRIMARY_KEY.ToLower() + "' data-" + PRIMARY_KEY.ToLower() + "='" + drDatas[PRIMARY_KEY].ToString() + "' />").Append("</td>");
                foreach (DataRow drColumn in dtColumn.Rows)
                {
                    columnName = drColumn["COLUMN_NAME"].ToString();
                    columnType = drColumn["DATA_TYPE"].ToString();
                    sb.Append("         <td>");
                    if (columnType == "DATE")
                        sb.Append("             <input type='text' class='form-control input-sm " + columnName + "' value=\"" + drDatas[columnName] + "\" onclick='ShowCalendar(this)' />");
                    else
                        sb.Append("             <input type='text' class='form-control input-sm " + columnName + "' value=\"" + drDatas[columnName] + "\" />");
                    sb.Append("         </td>");
                }
                sb.Append("     </tr>");
            }
            sb.Append(" </tbody>");
        }

        public static void GetTfoot(StringBuilder sb, DataTable dtColumn)
        {
            sb.Append(" <tfoot>");
            sb.Append("     <tr>");
            var rows = dtColumn.Rows.Count + 1;
            sb.Append("         <td colspan='" + rows + "'>");
            sb.Append("             <input type='button' class='btn btn-primary btn-sm' id='btnNew' value='New' />");
            sb.Append("         </td>");
            sb.Append("     </tr>");
            sb.Append(" </tfoot>");
        }

        public static void GetTrNewLine(StringBuilder sb, DataTable dtColumn)
        {
            string columnName = string.Empty; string columnType = string.Empty;
            sb.Append("     <tr>");
            sb.Append("         <td>").Append("<input type='button' class='btn btn-primary btn-sm btnSave' value='Save' data-primary_key='' />").Append("</td>");
            foreach (DataRow drColumn in dtColumn.Rows)
            {
                columnName = drColumn["COLUMN_NAME"].ToString();
                columnType = drColumn["DATA_TYPE"].ToString();
                sb.Append("         <td>");
                if (columnType == "DATE")
                    sb.Append("             <input type='text' class='form-control input-sm " + columnName + "' value=\"\" onclick='ShowCalendar(this)' />");
                else
                    sb.Append("             <input type='text' class='form-control input-sm " + columnName + "' value=\"\" />");
                sb.Append("         </td>");
            }
            sb.Append("     </tr>");
        }


        #region
        //[HttpPost]
        //public ActionResult GetTableDetail(string TABLE_NAME)
        //{
        //    APP_QUICK_SCREEN_BLL bll = new APP_QUICK_SCREEN_BLL();
        //    try
        //    {
        //        var columnList = bll.GetTableColum(TABLE_NAME);
        //        var tableDatas = bll.GetTableDatas(TABLE_NAME);
        //        string html = CreateTable(columnList, tableDatas);

        //        return Json(new RequestResult(html));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("ADM GetTableDetail");
        //        return Json(new RequestResult(false, ex.Message));
        //    }
        //}

        ///// <summary>
        ///// CreateTable
        ///// </summary>
        ///// <param name="tcl">Columns</param>
        ///// <param name="dt">Datas</param>
        ///// <returns></returns>
        //public static string CreateTable(List<TableColumnListArg> tcl, DataTable dt)
        //{
        //    string html = "";

        //    var count = tcl.Count();
        //    var width = Convert.ToDecimal(95 / count).ToString("0.0");

        //    html += "<table class='table table-striped table-hover table-bordered table-condensed' id='resultTableModal'>";

        //    #region thead
        //    html += "<thead>";
        //    html += "<tr>";
        //    foreach (var col in tcl)
        //    {
        //        html += "<th style='width:" + width + "%'>" + col.COLUMN_NAME + "</th>";
        //    }
        //    html += "<th style='width:5%'>Operation</th>";
        //    html += "</tr>";
        //    html += "</thead>";
        //    #endregion

        //    #region tbody
        //    html += "<tbody id='resultBodyModal'>";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        html += "<tr>";
        //        foreach (var tc in tcl)
        //        {
        //            if (tc.DATA_TYPE == "DATE")
        //                html += "<td>" + "<input type='text' class='form-control " + tc.COLUMN_NAME + "' value='" + dr[tc.COLUMN_NAME].ToString() + "' onclick='TimePick(this);' />" + "</td>";
        //            else
        //                html += "<td>" + "<input type='text' class='form-control " + tc.COLUMN_NAME + "' value='" + dr[tc.COLUMN_NAME].ToString() + "' />" + "</td>";
        //        }
        //        html += "<td>" + "<input type='button' class='btn btn-primary btn-sm btnSave' value='SAVE' />" + "</td>";
        //        html += "</tr>";
        //    }
        //    html += "</tbody>";
        //    #endregion

        //    html += "</table>";
        //    html += "<input type='button' class='btn btn-primary btn-sm' id='btnAdd' value='ADD NEW' />";
        //    return html;
        //}


        //public ActionResult SaveTableDetail(string TABLE_NAME, string STR)
        //{
        //    SaveDetail(TABLE_NAME, STR);

        //    return View();
        //}



        //public void SaveDetail(string TABLE_NAME, string STR)
        //{
        //    APP_QUICK_SCREEN_BLL bll = new APP_QUICK_SCREEN_BLL();

        //    var t = typeof(APP_PAGES);


        //    object model = null;
        //    switch (TABLE_NAME)
        //    {
        //        case "APP_PAGES":
        //            model = CreateClass<APP_PAGES>(STR);
        //            break;
        //        case "APP_NOTICE":
        //            model = CreateClass<APP_NOTICE>(STR);
        //            break;
        //        case "APP_USERS":
        //            model = CreateClass<APP_USERS>(STR);
        //            break;
        //        case "APP_WH":
        //            model = CreateClass<APP_WH>(STR);
        //            break;
        //        case "APP_WORKSPACE":
        //            model = CreateClass<APP_WORKSPACE>(STR);
        //            break;
        //        case "APP_WELCOME":
        //            model = CreateClass<APP_WELCOME>(STR);
        //            break;
        //        case "COUNTRY_CODES":
        //            model = CreateClass<COUNTRY_CODES>(STR);
        //            break;
        //        case "APP_CUST_ALIAS":
        //            model = CreateClass<APP_CUST_ALIAS>(STR);
        //            break;
        //    }

        //}


        //public T CreateClass<T>(string STR)
        //{
        //    var model = JsonConvert.DeserializeObject<T>(STR);
        //    return model;
        //}


        //public void Save<T>(T model)
        //{

        //}
        #endregion

    }
}
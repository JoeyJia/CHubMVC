using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Text;
using CHubBLL;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using CHubModel;

namespace CHubMVC.Controllers
{
    public class FARController : BaseController
    {
        // GET: FAR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyFar()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.myfar.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            ViewBag.PERIOD = DateTime.Now.Year;
            return View();
        }


        /// <summary>
        /// Search
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MyFarSearch(FAR_Arg arg)
        {
            FAR_BLL BLL = new FAR_BLL();
            try
            {
                var result = BLL.MyFarSearch(arg);
                var mainHmtl = MyFarSearchHtml(result);
                return Json(new RequestResult(mainHmtl));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MyFarSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult MyFarSave(FAR_HEADER far, string appUser)
        {
            FAR_BLL fBLL = new FAR_BLL();
            try
            {
                //update
                if (far.FAR_NO != 0)
                {
                    fBLL.MyFarUpdate(far);
                }
                else
                {
                    if (fBLL.IsExistMyFar(far))
                        return Json(new RequestResult(false, "Existed,Cannot Save!"));
                    else//Add
                    {
                        far.FAR_NO = Convert.ToDecimal(fBLL.GetFar_No());
                        far.REQ_BY = appUser;
                        fBLL.MyFarAdd(far);
                    }
                }
                return Json(new RequestResult(far.FAR_NO));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR MyFarSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetMyFarDetail(string FAR_NO)
        {
            FAR_BLL fBLL = new FAR_BLL();
            try
            {
                var header = fBLL.GetMyFarHeader(FAR_NO);
                var details = fBLL.GetMyFarDetail(FAR_NO);
                var detailHmtl = GetDetailHtml(details);
                return Json(new { Success = true, Header = header, DetailHmtl = detailHmtl });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR GetMyFarDetail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ConfirmMyFar(string FAR_NO, string APP_USER)
        {
            FAR_BLL fBLL = new FAR_BLL();
            try
            {
                if (fBLL.CheckSecurity("FAR_CONFIRM", APP_USER))
                {
                    fBLL.UpdateFarStatus(FAR_NO, "FIRM");
                    return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR ConfirmMyFar", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult DiscardMyFar(string FAR_NO, string APP_USER)
        {
            FAR_BLL fBLL = new FAR_BLL();
            try
            {
                fBLL.UpdateFarStatus(FAR_NO, "DISCARD");
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR ConfirmMyFar", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetDetailHtml(List<V_FAR_DETAIL> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.CUST_PARTNO).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCATION_CODE).Append("</td>");
                    if (!string.IsNullOrEmpty(item.PART_NO))
                        sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    else
                        sb.Append("     <td style='background-color:red;'>").Append(item.PART_NO).Append("</td>");
                    GetPART_STATUS(item.PART_STATUS, item.COLOR, sb);
                    sb.Append("     <td>").Append(item.M01).Append("</td>");
                    sb.Append("     <td>").Append(item.M02).Append("</td>");
                    sb.Append("     <td>").Append(item.M03).Append("</td>");
                    sb.Append("     <td>").Append(item.M04).Append("</td>");
                    sb.Append("     <td>").Append(item.M05).Append("</td>");
                    sb.Append("     <td>").Append(item.M06).Append("</td>");
                    sb.Append("     <td>").Append(item.M07).Append("</td>");
                    sb.Append("     <td>").Append(item.M08).Append("</td>");
                    sb.Append("     <td>").Append(item.M09).Append("</td>");
                    sb.Append("     <td>").Append(item.M10).Append("</td>");
                    sb.Append("     <td>").Append(item.M11).Append("</td>");
                    sb.Append("     <td>").Append(item.M12).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append("     <td>")
                        .Append("<input type='button' class='btn btn-primary btn-xs btnDelete' value='DELETE' data-cust_partno='" + item.CUST_PARTNO + "' data-far_no='" + item.FAR_NO + "' data-load_seq='" + item.LOAD_SEQ + "' />")
                        .Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public void GetPART_STATUS(string PART_STATUS, string Color, StringBuilder sb)
        {
            switch (Color)
            {
                case "R":
                    sb.Append("     <td style='color:red;'>").Append(PART_STATUS).Append("</td>");
                    break;
                case "G":
                    sb.Append("     <td style='color:green;'>").Append(PART_STATUS).Append("</td>");
                    break;
                case "Y":
                    sb.Append("     <td style='color:orange;'>").Append(PART_STATUS).Append("</td>");
                    break;
                default:
                    sb.Append("     <td>").Append(PART_STATUS).Append("</td>");
                    break;
            }
        }


        public string MyFarSearchHtml(List<V_FAR_HEADER> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.FAR_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PERIOD).Append("</td>");
                    GetFAR_STATUSColor(sb, item.FAR_STATUS);
                    sb.Append("     <td>").Append(item.CUSTOMER_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.SHORT_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.FAR_PROJECT).Append("</td>");
                    sb.Append("     <td>").Append(item.FAR_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.CREATE_DATE.ToString("yyyy-MM-dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.REQ_BY).Append("</td>");
                    sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-xs btnDetail' data-farno='" + item.FAR_NO + "' data-status='" + item.FAR_STATUS + "' value='Details' />");
                    //if (item.FAR_STATUS == "OPEN")
                    //{
                    //    sb.Append("<input type='button' class='btn btn-primary btn-xs btnConfirm' data-farno='" + item.FAR_NO + "' value='Confirm' />")
                    //      .Append("<input type='button' class='btn btn-primary btn-xs btnDiscard' data-farno='" + item.FAR_NO + "' value='Discard' />");
                    //}
                    //else
                    //{
                    //    sb.Append("<input type='button' class='btn btn-primary btn-xs btnConfirm' data-farno='" + item.FAR_NO + "' value='Confirm' disabled />")
                    //      .Append("<input type='button' class='btn btn-primary btn-xs btnDiscard' data-farno='" + item.FAR_NO + "' value='Discard' disabled />");
                    //}
                    sb.Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetFAR_STATUSColor(StringBuilder sb, string FAR_STATUS)
        {
            switch (FAR_STATUS)
            {
                case "OPEN":
                    sb.Append("     <td style='color:green;'>");
                    break;
                case "DISCARD":
                    sb.Append("     <td style='color:red;'>");
                    break;
                default:
                    sb.Append("     <td>");
                    break;
            }
            sb.Append(FAR_STATUS).Append("</td>");
            return sb.ToString();
        }

        /// <summary>
        /// FAR_STATUS
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFAR_STATUS()
        {
            FAR_BLL bll = new FAR_BLL();
            try
            {
                List<Item> items = new List<Item>();
                DataTable dt = bll.GetFAR_STATUS();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Item item = new Item();
                        item.Value = dr["FAR_STATUS"].ToString();
                        item.Text = dr["STATUS_DESC"].ToString();
                        items.Add(item);
                    }
                }
                return Json(new RequestResult(items));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetFAR_STATUS", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
        /// <summary>
        /// CUSTOMER_NO
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCUSTOMER_NO()
        {
            FAR_BLL bll = new FAR_BLL();
            try
            {
                List<Item> items = new List<Item>();
                DataTable dt = bll.GetCUSTOMER_NO();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Item item = new Item();
                        item.Value = dr["CUSTOMER_NO"].ToString();
                        item.Text = dr["SHORT_NAME"].ToString();
                        items.Add(item);
                    }
                }
                return Json(new RequestResult(items));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCUSTOMER_NO", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
        /// <summary>
        /// ADJ_TYPE
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetADJ_TYPE()
        {
            FAR_BLL bll = new FAR_BLL();
            try
            {
                List<Item> items = new List<Item>();
                DataTable dt = bll.GetADJ_TYPE();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Item item = new Item();
                        item.Value = dr["ADJ_TYPE"].ToString();
                        item.Text = dr["ADJ_DESC"].ToString();
                        items.Add(item);
                    }
                }
                return Json(new RequestResult(items));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetADJ_TYPE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
        /// <summary>
        /// PRIORITY_CODE
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPRIORITY_CODE()
        {
            FAR_BLL bll = new FAR_BLL();
            try
            {
                List<Item> items = new List<Item>();
                DataTable dt = bll.GetPRIORITY_CODE();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Item item = new Item();
                        item.Value = dr["PRIORITY_CODE"].ToString();
                        item.Text = dr["PRIORITY_DESC"].ToString();
                        items.Add(item);
                    }
                }
                return Json(new RequestResult(items));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPRIORITY_CODE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
        /// <summary>
        /// INV_STRATEGY_CODE
        /// </summary>
        /// <returns></returns>
        public ActionResult GetINV_STRATEGY_CODE()
        {
            FAR_BLL bll = new FAR_BLL();
            try
            {
                List<Item> items = new List<Item>();
                DataTable dt = bll.GetINV_STRATEGY_CODE();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Item item = new Item();
                        item.Value = dr["INV_STRATEGY_CODE"].ToString();
                        item.Text = dr["INV_STRATEGY_DESC"].ToString();
                        items.Add(item);
                    }
                }
                return Json(new RequestResult(items));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetINV_STRATEGY_CODE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.EXPFarMainTemplateName;
            return File(templateFolder + fileName, "application/ms-excel", fileName);
        }

        public ActionResult ExportFar(string FAR_NO)
        {
            FAR_BLL fBLL = new FAR_BLL();
            try
            {
                var result = fBLL.ExportFar(FAR_NO);
                string fileName = result.Split('~')[0] + ".xlsx";//文件名
                string sql = result.Split('~')[1];//sql语句
                DataTable dt = fBLL.RunSql(sql);
                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                string fullPath = basePath + fileName;
                NPOIExcelHelper npoi = new NPOIExcelHelper(fullPath);
                npoi.DataTableToExcel(dt, "sheet1");
                return File(fullPath, "application/ms-excel", fileName);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR ExportFar", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult FarUpload(string FAR_NO)
        {
            FAR_BLL fBLL = new FAR_BLL();
            StringBuilder sb = new StringBuilder();
            try
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataTable dt = ConvertToDataTable(hpf);
                    if (dt == null && dt.Rows.Count == 0)
                        return Json(new RequestResult(false, "No data in excel"));
                    List<FAR_DETAIL_STG> list = new List<FAR_DETAIL_STG>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        FAR_DETAIL_STG fd = new FAR_DETAIL_STG()
                        {
                            FAR_NO = Convert.ToDecimal(FAR_NO),
                            CUST_PARTNO = dr["ITEM_NO"].ToString(),
                            LOCATION_CODE = dr["LOCATION_CODE"].ToString(),
                            M01 = !string.IsNullOrEmpty(dr["M01"].ToString()) ? Convert.ToDecimal(dr["M01"].ToString()) : 0,
                            M02 = !string.IsNullOrEmpty(dr["M02"].ToString()) ? Convert.ToDecimal(dr["M02"].ToString()) : 0,
                            M03 = !string.IsNullOrEmpty(dr["M03"].ToString()) ? Convert.ToDecimal(dr["M03"].ToString()) : 0,
                            M04 = !string.IsNullOrEmpty(dr["M04"].ToString()) ? Convert.ToDecimal(dr["M04"].ToString()) : 0,
                            M05 = !string.IsNullOrEmpty(dr["M05"].ToString()) ? Convert.ToDecimal(dr["M05"].ToString()) : 0,
                            M06 = !string.IsNullOrEmpty(dr["M06"].ToString()) ? Convert.ToDecimal(dr["M06"].ToString()) : 0,
                            M07 = !string.IsNullOrEmpty(dr["M07"].ToString()) ? Convert.ToDecimal(dr["M07"].ToString()) : 0,
                            M08 = !string.IsNullOrEmpty(dr["M08"].ToString()) ? Convert.ToDecimal(dr["M08"].ToString()) : 0,
                            M09 = !string.IsNullOrEmpty(dr["M09"].ToString()) ? Convert.ToDecimal(dr["M09"].ToString()) : 0,
                            M10 = !string.IsNullOrEmpty(dr["M10"].ToString()) ? Convert.ToDecimal(dr["M10"].ToString()) : 0,
                            M11 = !string.IsNullOrEmpty(dr["M11"].ToString()) ? Convert.ToDecimal(dr["M11"].ToString()) : 0,
                            M12 = !string.IsNullOrEmpty(dr["M12"].ToString()) ? Convert.ToDecimal(dr["M12"].ToString()) : 0,
                            NOTE = dr["NOTE"].ToString()
                        };
                        list.Add(fd);
                    }
                    var LOAD_SEQ = fBLL.GetFAR_LOAD();
                    //Check
                    foreach (var item in list)
                    {
                        var result = fBLL.CheckCUST_PARTNO(item.CUST_PARTNO);
                        if (string.IsNullOrEmpty(result))
                            sb.Append(item.CUST_PARTNO).Append(",");
                    }
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        return Json(new RequestResult(false, "零件号：" + sb.ToString().TrimEnd(',') + "不存在!"));

                    //Add
                    foreach (var item in list)
                    {
                        fBLL.InsertFAR_DETAIL_STG(item, LOAD_SEQ);
                    }
                    var details = fBLL.GetMyFarDetail(FAR_NO);
                    string detailsHtml = GetDetailHtml(details);
                    return Json(new RequestResult(detailsHtml));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR FarUpload", ex);
                return Json(new RequestResult(false, ex.Message));
            }

            return View();
        }

        [HttpPost]
        public ActionResult DeleteFarDetail(string FAR_NO, string LOAD_SEQ, string CUST_PARTNO)
        {
            FAR_BLL fBLL = new FAR_BLL();
            try
            {
                fBLL.DeleteFarDetail(FAR_NO, LOAD_SEQ, CUST_PARTNO);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("FAR DeleteFarDetail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownLoad(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);
            string fullPath = folder.FullName + fileName;
            return File(fullPath, "application/ms-excel", fileName);
        }

        public DataTable ConvertToDataTable(HttpPostedFileBase hpf)
        {
            DataTable dt = new DataTable();
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string extension = Path.GetExtension(hpf.FileName);

            string fullPath = folder.FullName + tempGuid + extension;
            hpf.SaveAs(fullPath);

            NPOIExcelHelper npoi = new NPOIExcelHelper(fullPath);
            dt = npoi.ExcelToDataTable();
            System.IO.File.Delete(fullPath);

            return dt;
        }




        public class Item
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
    }
}
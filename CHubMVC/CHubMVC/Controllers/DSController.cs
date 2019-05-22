using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubModel;
using CHubCommon;
using CHubBLL;
using CHubDBEntity.UnmanagedModel;
using System.Text;
using CHubModel.ExtensionModel;
using CHubModel.WebArg;
using System.Data;
using System.IO;

namespace CHubMVC.Controllers
{
    public class DSController : BaseController
    {
        // GET: DS
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// /ds/dsoainq
        /// </summary>
        [Authorize]
        [HttpGet]
        public ActionResult DSOAINQ()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.dsoainq.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult DSOAINQSearch(string PART_NO, string COMPANY_CODE, string PO_NO, string OA_STATUS, string ORDER_DATE)
        {
            DSOAINQ_BLL doBLL = new DSOAINQ_BLL();
            try
            {
                var result = doBLL.DSOAINQSearch(PART_NO, COMPANY_CODE, PO_NO, OA_STATUS, ORDER_DATE);
                var mainHtml = GetDSOAINQHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DS DSOAINQSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetDSOAINQHtml(List<V_IHUB_OA_BASE> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append(GetStatusColor(item.OA_STATUS));
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.COMPANY_CODE).Append("</td>");
                    sb.Append("     <td>").Append(item.PO_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.ITEM_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_QTY).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_SHIPPED).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_DATE).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE_PLAN).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE).Append("</td>");
                    sb.Append("     <td>").Append(item.SUPP_SO).Append("</td>");
                    sb.Append("     <td>").Append(item.LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.SUPPLIER_ITEM).Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

        public string GetStatusColor(string STATUS)
        {
            string result = string.Empty;
            switch (STATUS)
            {
                case "Opening":
                    result = "      <td style='color:red;'>" + STATUS + "</td>";
                    break;
                case "Shipped":
                    result = "      <td style='color:green;'>" + STATUS + "</td>";
                    break;
                case "Closed":
                    result = "      <td style='color:green;'>" + STATUS + "</td>";
                    break;
                default:
                    result = "      <td>" + STATUS + "</td>";
                    break;
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        public ActionResult DSMAIN()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.dsmain.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult DSMAINSearch(DSMAINArg arg)
        {
            DSMAIN_BLL dmBLL = new DSMAIN_BLL();
            try
            {
                var result = dmBLL.DSMAINSearch(arg);
                var mainHtml = GetDSMAINHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DS DSMAIN", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult RunP_DS_STatus_REFRESH()
        {
            DSMAIN_BLL dmBLL = new DSMAIN_BLL();
            try
            {
                dmBLL.RunP_DS_STatus_REFRESH();
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DS RunP_DS_STatus_REFRESH", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult DSMAINMore(string PO_NO, string PART_NO)
        {
            DSMAIN_BLL dmBLL = new DSMAIN_BLL();
            try
            {
                var result = dmBLL.DSMAINMore(PO_NO, PART_NO);
                var mainHtml = GetDSMAINMoreHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DS DSMAINMore", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetDSMAINHtml(List<V_DS_ORDER_BASE> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append(GetStatusColor(item.STATUS_CODE));
                    sb.Append("     <td>").Append(item.CUSTOMER_PO_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PO_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.CATALOG_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_QTY).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_SHIPPED).Append("</td>");
                    sb.Append("     <td>").Append(item.ETA.HasValue ? item.ETA.Value.ToString("yyyy/MM/dd") : "")
                        .Append("       <div style='float:right;'>")
                        .Append("           <a class='MoreDetail' data-pono='" + item.PO_NO + "' data-partno='" + item.PART_NO + "'>More...").Append("</a>")
                        .Append("       </div>")
                        .Append("   </td>");
                    sb.Append("     <td>").Append(item.ETA_NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_RESERVED).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_PICKED).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_TO_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.WAREHOUSE).Append("</td>");
                    sb.Append("     <td>").Append(item.COMPANY_NAME_SHORT).Append("</td>");
                    sb.Append("     <td>").Append(item.SUPPLIER_ITEM).Append("</td>");
                    sb.Append("     <td>").Append(item.MOQ).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }
        public string GetDSMAINMoreHtml(List<V_IHUB_OA_BASE> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    //sb.Append(GetStatusColor(item.OA_STATUS));
                    sb.Append("     <td>").Append(item.SUPP_SO).Append("</td>");
                    sb.Append("     <td>").Append(item.LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.SUPPLIER_ITEM).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_QTY).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_SHIPPED).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_DATE.HasValue ? item.ORDER_DATE.Value.ToString("yyyy/MM/dd") : "").Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE_PLAN.HasValue ? item.SHIP_DATE_PLAN.Value.ToString("yyyy/MM/dd") : "").Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE.HasValue ? item.SHIP_DATE.Value.ToString("yyyy/MM/dd") : "").Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_WH).Append("</td>");
                    sb.Append("     <td>").Append(item.RECORD_DATE.HasValue ? item.RECORD_DATE.Value.ToString("yyyy/MM/dd") : "").Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

        public ActionResult IhubASN()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.ihubasn.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [HttpPost]
        public ActionResult IhubASNUpload()
        {
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
            if (hpf.ContentLength > 0)
            {
                DS_BLL dBLL = new DS_BLL();

                DataTable dt = GetDataFromExcel(hpf);
                if (dt == null && dt.Rows.Count == 0)
                    return Json(new RequestResult(false, "No data in Excel"));

                //每次导入获取序列号
                string LOAD_BATCH = dBLL.GetLOAD_BATCH();
                int lineNo = 0;
                //数据导入表 IHUB_ASN_STG
                foreach (DataRow dr in dt.Rows)
                {
                    IHUB_ASN_STG item = new IHUB_ASN_STG();
                    item.ASN_NO = dr["ASN_NO"].ToString();
                    item.LINE_NO = Convert.ToDecimal(dr["LINE_NO"].ToString());
                    item.COMPANY_CODE = dr["COMPANY_CODE"].ToString();
                    string ship_date = dr["SHIP_DATE"].ToString();
                    item.SHIP_DATE = Convert.ToDateTime(ship_date.Substring(0, 4) + "/" + ship_date.Substring(4, 2) + "/" + ship_date.Substring(6, 2));
                    item.PO_NO = dr["PO_NO"].ToString();
                    item.PO_LINE_NO = !string.IsNullOrEmpty(dr["PO_LINE_NO"].ToString()) ? Convert.ToDecimal(dr["PO_LINE_NO"].ToString()) : 0;
                    item.PO_REL_NO = !string.IsNullOrEmpty(dr["PO_REL_NO"].ToString()) ? Convert.ToDecimal(dr["PO_REL_NO"].ToString()) : 0;
                    item.PART_NO = dr["PART_NO"].ToString();
                    item.QTY_SHIPPED = !string.IsNullOrEmpty(dr["QTY_SHIPPED"].ToString()) ? Convert.ToDecimal(dr["QTY_SHIPPED"].ToString()) : 0;
                    item.COO = dr["COO"].ToString();
                    item.NOTE = dr["NOTE"].ToString();
                    item.LOAD_BATCH = Convert.ToDecimal(LOAD_BATCH);
                    item.LOAD_BY = Session[CHubConstValues.SessionUser].ToString();
                    dBLL.IhubASNUpload(item);
                    lineNo++;
                }
                //执行Function
                var result = dBLL.RunF_IHUB_ASN_LOAD_CHK(Convert.ToDecimal(LOAD_BATCH));
                //返回的不是OK，弹窗显示返回值，是OK，执行存过，弹窗提示导入行数
                if (result != "OK")
                    return Json(new RequestResult(false, result));
                else
                {
                    //返回值为OK，执行存过
                    dBLL.ExecP_IHUB_ASN_LOAD_POST(Convert.ToDecimal(LOAD_BATCH));
                    return Json(new RequestResult(lineNo));
                }
            }
            else
                return Json(new RequestResult(false, "Empty Excel"));
        }

        [HttpPost]
        public ActionResult IhubASNSearch(string COMPANY_CODE, string ASN_NO, int LOAD_DAY)
        {
            DS_BLL dBLL = new DS_BLL();
            try
            {
                var result = dBLL.IhubASNSearch(COMPANY_CODE, ASN_NO, LOAD_DAY);
                var mainHtml = GetIhubASNHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DS IhubASNSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
        [HttpPost]
        public ActionResult GetCOMPANY_NAME(string COMPANY_CODE)
        {
            DS_BLL dBLL = new DS_BLL();
            try
            {
                var result = dBLL.GetCOMPANY_NAME(COMPANY_CODE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DS GetCOMPANY_NAME", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.IhubASNTemplateName;
            return File(templateFolder + fileName, "application/ms-excel", fileName);
        }

        public DataTable GetDataFromExcel(HttpPostedFileBase hpf)
        {
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullPath = Path.Combine(folder.FullName, tempGuid + Path.GetExtension(hpf.FileName));
            hpf.SaveAs(fileFullPath);

            NPOIExcelHelper npoi = new NPOIExcelHelper(fileFullPath);
            DataTable dt = npoi.ExcelToDataTable();
            System.IO.File.Delete(fileFullPath);
            return dt;
        }

        public string GetIhubASNHtml(List<V_IHUB_ASN> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.ASN_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.PO_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_SHIPPED).Append("</td>");
                    sb.Append("     <td>").Append(item.COO).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.PLANNER).Append("</td>");
                    sb.Append("     <td>").Append(item.PO_LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PO_REL_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.LOAD_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.LOAD_BY).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

    }
}
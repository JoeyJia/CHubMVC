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
                    sb.Append(GetStatusColor(item.OA_STATUS));
                    sb.Append("     <td>").Append(item.SUPP_SO).Append("</td>");
                    sb.Append("     <td>").Append(item.LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.SUPPLIER_ITEM).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_QTY).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_SHIPPED).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_DATE).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE_PLAN).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_WH).Append("</td>");
                    sb.Append("     <td>").Append(item.RECORD_DATE).Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

    }
}
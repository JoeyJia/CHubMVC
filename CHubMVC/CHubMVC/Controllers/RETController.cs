using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel;
using CHubModel.WebArg;
using CHubBLL;
using System.Text;
using System.Data;
using System.IO;

namespace CHubMVC.Controllers
{
    public class RETController : BaseController
    {
        public ActionResult RetInv()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.retinv.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetCustomer_No(string AppUser)
        {
            RETINV_BLL riBLL = new RETINV_BLL();
            try
            {
                var result = riBLL.GetCustomer_No(AppUser);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetCustomer_No", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetInvSearch(RetInvArg arg)
        {
            RETINV_BLL riBLL = new RETINV_BLL();
            try
            {
                var result = riBLL.RetInvSearch(arg);
                var mainHtml = GetRetInvHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetInvSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetRetInvDetailModal(string INVOICE_ID)
        {
            RETINV_BLL riBLL = new RETINV_BLL();
            try
            {
                var result = riBLL.GetRetInvDetailModal(INVOICE_ID);
                var mainHtml = GetRetInvDetailHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetRetInvDetailModal", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public string GetRetInvHtml(List<V_RET_INV_RETURN_H> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.REFERENCE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.RET_REQ_NO == 0 ? "" : item.RET_REQ_NO.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.INVOICE_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.INVOICE_AMT == 0 ? "" : item.INVOICE_AMT.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.CHARGES == 0 ? "" : item.CHARGES.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.RECONCILE_STATUS == "Fully Reconciled" ? "<span style=\"color:green;\">" : "<span style=\"color:#ffa54f;\">").Append(item.RECONCILE_STATUS).Append("</span>").Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.INVOICE_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.CUSTOMER_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.CUST_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.WAREHOUSE).Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDetail\" value=\"Details\" />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetRetInvDetailHtml(List<V_RET_INV_RETURN_D> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.INVOICE_LINE_NO == 0 ? "" : item.INVOICE_LINE_NO.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PRINT_PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.DESCRIPTION).Append("</td>");
                    sb.Append("     <td>").Append(item.INVOICE_QTY == 0 ? "" : item.INVOICE_QTY.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.UNIT_PRICE_AMT == 0 ? "" : item.UNIT_PRICE_AMT.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.INVOICE_AMT == 0 ? "" : item.INVOICE_AMT.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.CHARGES == 0 ? "" : item.CHARGES.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.REMAINING_QTY).Append("</td>");
                    sb.Append("     <td>").Append(item.VAT_TIED).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

    }
}
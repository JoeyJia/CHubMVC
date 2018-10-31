using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubModel;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;
using System.Text;

namespace CHubMVC.Controllers
{
    public class AdhocController : BaseController
    {
        // GET: Adhoc
        public ActionResult CwsCust()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CwsCustSearch(string CUSTOMER_NO)
        {
            CWSCUST_BLL ccBLL = new CWSCUST_BLL();
            try
            {
                var result = ccBLL.CwsCustSearch(CUSTOMER_NO);
                var mainHtml = GetCwsCustHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Adhoc CwsCustSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CwsCustSave(APP_OECUSTOMER_MST arg)
        {
            CWSCUST_BLL ccBLL = new CWSCUST_BLL();
            try
            {
                ccBLL.CwsCustSave(arg);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Adhoc CwsCustSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetCwsCustHtml(List<APP_OECUSTOMER_MST> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm CUSTOMER_NO\" value=\"" + item.CUSTOMER_NO + "\" title=\"" + item.CUSTOMER_NO + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm CUST_NAME\" value=\"" + item.CUST_NAME + "\" title=\"" + item.CUST_NAME + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append(GetCWS_FLAG(item.CWS_FLAG)).Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm FLAG01\" value=\"" + item.FLAG01 + "\" title=\"" + item.FLAG01 + "\" />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm FLAG02\" value=\"" + item.FLAG02 + "\" title=\"" + item.FLAG02 + "\" />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm FLAG03\" value=\"" + item.FLAG03 + "\" title=\"" + item.FLAG03 + "\" />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm NOTE\" value=\"" + item.NOTE + "\" title=\"" + item.NOTE + "\" />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnSave\" value=\"SAVE\" data-customerno=\"" + item.CUSTOMER_NO + "\" />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetCWS_FLAG(string CWS_FLAG)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <select class=\"form-control input-sm CWS_FLAG\">");
            string[] YorN = { "Y", "N" };
            foreach (var i in YorN)
            {
                if (CWS_FLAG == i)
                    sb.Append("     <option value=\"" + i + "\" selected>").Append(i).Append("</option>");
                else
                    sb.Append("     <option value=\"" + i + "\">").Append(i).Append("</option>");
            }
            sb.Append(" </select>");
            return sb.ToString();
        }
    }
}
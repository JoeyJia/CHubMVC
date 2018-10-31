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


        [Authorize]
        [HttpPost]
        public ActionResult RunProc_P_RET_Match(string INVOICE_ID)
        {
            RETINV_BLL riBLL = new RETINV_BLL();
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            if (riBLL.CheckSecurity("RET_INV_REC", appUser))//权限控制
            {
                try
                {
                    //Run Proc
                    riBLL.RunProc_P_RET_Match(INVOICE_ID);
                    return Json(new RequestResult(true));
                }
                catch (Exception ex)
                {
                    return Json(new RequestResult(false, "Run error" + ex.Message));
                }
            }
            else
                return Json(new RequestResult(false, "You can not Operate"));
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
                    if (item.REMAINING_QTY > 0)
                        sb.Append(" <tr style=\"background-color: rgb(255, 165, 79);\">");
                    else
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


        public ActionResult RetMain()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.retmain.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetMainSearch(string CUSTOMER_NO, string RET_REQ_NO, string REQ_DATE)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.RetMainSearch(CUSTOMER_NO, RET_REQ_NO, REQ_DATE);
                var mainHtml = GetRetMainHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetMainSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetRetMainDetailModal(string RET_REQ_NO, string REQ_STATUS)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetRetMainDetailModal(RET_REQ_NO);
                var mainHtml = GetRetMainDetailHtml(result, REQ_STATUS);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetRetMainDetailModal", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult DeleteFromRET_REQ_D(string RET_REQ_NO, string LINE_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                rmBLL.DeleteFromRET_REQ_D(RET_REQ_NO, LINE_NO);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET DeleteFromRET_REQ_D", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveRET_REQ_D(List<RET_REQ_DArg> arg, string RET_REQ_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                string APP_USER = Session[CHubConstValues.SessionUser].ToString();
                if (rmBLL.CheckSecurity("RET_REQ_SAVE", APP_USER))
                {
                    if (arg != null && arg.Count() > 0)
                    {
                        foreach (var item in arg)
                        {
                            rmBLL.SaveRET_REQ_D(item, RET_REQ_NO);
                        }
                    }
                    return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "You can not operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET SaveRET_REQ_D", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CancelRET_REQ_H(string RET_REQ_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                rmBLL.UpdateREQ_STATUS("CANCELED", RET_REQ_NO);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET CancelRET_REQ_H", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubmitRET_REQ_H(List<RET_REQ_DArg> arg, string RET_REQ_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            bool isError = true;
            try
            {
                //检查所有行的PART_NO
                if (arg != null && arg.Count() > 0)
                {
                    foreach (var item in arg)
                    {
                        if (string.IsNullOrEmpty(item.PART_NO))
                        {
                            isError = false;
                            break;
                        }
                    }
                }
                //如果为空的话报错：零件号不存在！
                if (!isError)
                    return Json(new RequestResult(false, "零件号不存在！"));
                //检查通过之后 修改:RET_REQ_H 的字段 REQ_STATUS 为 SUBMITTED
                rmBLL.UpdateREQ_STATUS("SUBMITTED", RET_REQ_NO);
                return Json(new RequestResult(true));

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET SubmitRET_REQ_H", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ReVerify(List<RET_REQ_DArg> arg, string RET_REQ_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                //先执行SAVE操作
                if (arg != null && arg.Count() > 0)
                {
                    foreach (var item in arg)
                    {
                        rmBLL.SaveRET_REQ_D(item, RET_REQ_NO);
                    }
                }
                //然后执行Proc
                try
                {
                    rmBLL.ExecP_RET_Verify(RET_REQ_NO);
                    return Json(new RequestResult(true));
                }
                catch (Exception ex)
                {
                    return Json(new RequestResult(false, "Proc Error" + ex.Message));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET SubmitRET_REQ_H", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult DetailNewLine(string LINE_NO, string REQ_STATUS)
        {
            try
            {
                var mainHtml = GetRetMainDetailNewLineHtml(LINE_NO, REQ_STATUS);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET　DetailNewLine", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CallDetailFunction(string CUST_ITEM)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                string PART_NO = rmBLL.CallF_GOMS_PARTNO(CUST_ITEM);
                string DESCRIPTION = rmBLL.CallF_GOMS_desc(CUST_ITEM);
                string PART_GROUP = rmBLL.CallF_RET_PART_GROUP(CUST_ITEM);
                string SUPPLIER_CODE = rmBLL.CallF_RET_PART_supp(CUST_ITEM);
                var obj = new
                {
                    PART_NO = PART_NO,
                    DESCRIPTION = DESCRIPTION,
                    PART_GROUP = PART_GROUP,
                    SUPPLIER_CODE = SUPPLIER_CODE
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET CallDetailFunction", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetRetMainHtml(List<V_RET_REQ_H> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.RET_REQ_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.MOVEX_WH).Append("</td>");
                    sb.Append("     <td>").Append(item.CUST_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.CUSTOMER_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.RET_REQ_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.REQ_DATE.ToString("yyyy/MM/dd HH:mm:ss")).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.REQ_BY).Append("</td>");
                    sb.Append("     <td>").Append(GetRetMainColor(item.REQ_STATUS)).Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDetail\" value=\"Details\" />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }
        public string GetRetMainDetailHtml(List<V_RET_REQ_D> result, string REQ_STATUS)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtLINE_NO\" value=\"" + item.LINE_NO + "\" title=\"" + item.LINE_NO + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtCUST_ITEM\" value=\"" + item.CUST_ITEM + "\" title=\"" + item.CUST_ITEM + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY\" value=\"" + item.QTY + "\" title=\"" + item.QTY + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"" + item.PART_NO + "\" title =\"" + item.PART_NO + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtDESCRIPTION\" value=\"" + item.DESCRIPTION + "\" title=\"" + item.DESCRIPTION + "\" readonly />").Append("</td>");

                    if (Convert.ToDecimal(item.QTY) > item.QTY_APPROVED)
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY_APPROVED\" value=\"" + item.QTY_APPROVED + "\" title=\"" + item.QTY_APPROVED + "\" style=\"border-color:red\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY_APPROVED\" value=\"" + item.QTY_APPROVED + "\" title=\"" + item.QTY_APPROVED + "\" />").Append("</td>");

                    if (string.IsNullOrEmpty(item.REJECT_REASON))
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtREJECT_REASON\" value=\"" + item.REJECT_REASON + "\" title=\"" + item.REJECT_REASON + "\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtREJECT_REASON\" value=\"" + item.REJECT_REASON + "\" title=\"" + item.REJECT_REASON + "\" style=\"color:red;\" />").Append("</td>");

                    sb.Append("     <td>").Append(GetPART_GROUP(item.PART_GROUP)).Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtSUPPLIER_CODE\" value=\"" + item.SUPPLIER_CODE + "\" title=\"" + item.SUPPLIER_CODE + "\" />").Append("</td>");
                    string btnStr = REQ_STATUS == "NEW" ? "<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"DELETE\" data-lineno=\"" + item.LINE_NO + "\" />" : "<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"DELETE\" data-lineno=\"" + item.LINE_NO + "\" disabled />";
                    sb.Append("     <td>").Append(btnStr).Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }
        public string GetRetMainDetailNewLineHtml(string LINE_NO, string REQ_STATUS)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <tr>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtLINE_NO\" value=\"" + LINE_NO + "\" readonly />").Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtCUST_ITEM\" value=\"\" />").Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY\" value=\"\" />").Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"\" />").Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtDESCRIPTION\" value=\"\" />").Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY_APPROVED\" value=\"0\" />").Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtREJECT_REASON\" value=\"\" />").Append("</td>");
            sb.Append("     <td>").Append(GetPART_GROUP("")).Append("</td>");
            sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtSUPPLIER_CODE\" value=\"\" />").Append("</td>");
            string btnStr = REQ_STATUS == "NEW" ? "<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"DELETE\" data-lineno=\"" + LINE_NO + "\" />" : "<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"DELETE\" data-lineno=\"" + LINE_NO + "\" disabled />";
            sb.Append("     <td>").Append(btnStr).Append("</td>");
            sb.Append(" </tr>");
            return sb.ToString();
        }

        public string GetRetMainColor(string REQ_STATUS)
        {
            string sb = string.Empty;
            switch (REQ_STATUS)
            {
                case "CANCELED":
                    sb = "<span style=\"color:red;\">" + REQ_STATUS + "</span>";
                    break;
                case "APPROVED":
                    sb = "<span style=\"color:green;\">" + REQ_STATUS + "</span>";
                    break;
                case "SUBMITTED":
                    sb = "<span style=\"color:blue;\">" + REQ_STATUS + "</span>";
                    break;
                default:
                    sb = "<span>" + REQ_STATUS + "</span>";
                    break;
            }
            return sb.ToString();
        }
        public string GetPART_GROUP(string PART_GROUP)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<select class=\"form-control input-sm txtPART_GROUP\">");
            sb.Append(" <option value=\"\">").Append("</option>");

            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            var result = rmBLL.GetPART_GROUP();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    if (item.PART_GROUP == PART_GROUP)
                        sb.Append("<option value=\"" + item.PART_GROUP + "\" selected>").Append(item.PART_GROUP).Append("</option>");
                    else
                        sb.Append("<option value=\"" + item.PART_GROUP + "\">").Append(item.PART_GROUP).Append("</option>");
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

    }
}
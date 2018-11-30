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

        [Authorize]
        [HttpPost]
        public ActionResult RetInvCheckSecurity(string SECURE_ID, string APP_USER)
        {
            RETINV_BLL rBLL = new RETINV_BLL();
            try
            {
                return Json(new RequestResult(rBLL.CheckSecurity(SECURE_ID, APP_USER)));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetInvCheckSecurity", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RunProc_P_RET_INV_CLOSE(string INVOICE_ID)
        {
            RETINV_BLL rBLL = new RETINV_BLL();
            try
            {
                rBLL.RunProc_P_RET_INV_CLOSE(INVOICE_ID);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RunProc_P_RET_INV_CLOSE", ex);
                return Json(new RequestResult(false, "Run Error" + ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetInvDownload(string INVOICE_ID)
        {
            RETINV_BLL rBLL = new RETINV_BLL();
            try
            {
                var getSql = rBLL.RetInvGetSql(INVOICE_ID);
                string fileName = getSql.Split('~')[0] + ".xlsx";//文件名
                string sql = getSql.Split('~')[1];//sql语句
                DataTable dt = rBLL.RunRetInvSql(sql);//table
                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                string fullName = basePath + fileName;
                NPOIExcelHelper npoi = new NPOIExcelHelper(fullName);
                npoi.DataTableToExcel(dt, "Sheet1");
                return Json(new RequestResult(fileName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetInvDownload", ex);
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
                    sb.Append("     <td>").Append(item.INVOICE_AMT).Append("</td>");//== 0 ? "" : item.INVOICE_AMT.ToString()
                    sb.Append("     <td>").Append(item.CHARGES == 0 ? "" : item.CHARGES.ToString()).Append("</td>");
                    sb.Append("     <td>").Append(GetRECONCILE_STATUS(item.RECONCILE_STATUS)).Append("</td>");
                    //sb.Append("     <td>").Append(item.RECONCILE_STATUS == "Fully Reconciled" ? "<span style=\"color:green;\">" : "<span style=\"color:#ffa54f;\">").Append(item.RECONCILE_STATUS).Append("</span>").Append("</td>");
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

        public string GetRECONCILE_STATUS(string RECONCILE_STATUS)
        {
            StringBuilder sb = new StringBuilder();
            switch (RECONCILE_STATUS)
            {
                case "Fully Reconciled":
                    sb.Append("<span style=\"color:green;\">").Append(RECONCILE_STATUS).Append("</span>");
                    break;
                case "Closed":
                    sb.Append("<span style=\"color:blue;\">").Append(RECONCILE_STATUS).Append("</span>");
                    break;
                default:
                    sb.Append("<span style=\"color:#ffa54f;\">").Append(RECONCILE_STATUS).Append("</span>");
                    break;
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
                    sb.Append("     <td>").Append(item.INVOICE_AMT).Append("</td>");//== 0 ? "" : item.INVOICE_AMT.ToString()
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
        public ActionResult GetRetMainDetailModal(string RET_REQ_NO, string REQ_STATUS, string APP_USER)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetRetMainDetailModal(RET_REQ_NO);
                List<string> REJECT_REASON = result.Select(r => r.REJECT_REASON).Distinct().ToList();
                var mainHtml = GetRetMainDetailHtml(result, REQ_STATUS, APP_USER);
                return Json(new { Success = true, Data = mainHtml, REJECT_REASON = REJECT_REASON, Msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetRetMainDetailModal", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetRetMainDetailByRejectReason(string RET_REQ_NO, string REQ_STATUS, string APP_USER, string REJECT_REASON)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetRetMainDetailByRejectReason(RET_REQ_NO, REJECT_REASON);
                var mainHtml=GetRetMainDetailHtml(result, REQ_STATUS, APP_USER);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetRetMainDetailByRejectReason", ex);
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

                    //新加  需要再执行Re-Verify操作
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

        [Authorize]
        [HttpPost]
        public ActionResult RetMainDownload(string RET_REQ_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var getSql = rmBLL.RetMainGetSql(RET_REQ_NO);
                string fileName = getSql.Split('~')[0] + ".xlsx";//文件名
                string sql = getSql.Split('~')[1];//sql语句
                DataTable dt = rmBLL.RunRetMainSql(sql);
                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                string fullPath = basePath + fileName;
                NPOIExcelHelper npoi = new NPOIExcelHelper(fullPath);
                npoi.DataTableToExcel(dt, "Sheet1");
                return Json(new RequestResult(fileName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetMainDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetMainCheckAndSave(List<RET_REQ_DArg> arg, string RET_REQ_NO, string APP_USER)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                //check security
                if (rmBLL.CheckSecurity("RET_REQ_APP", APP_USER))
                {
                    //保存
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
                    return Json(new RequestResult(false));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetMainCheckAndSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeRET_REQ_H_Status(string RET_REQ_NO)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                rmBLL.ChangeRET_REQ_H_Status(RET_REQ_NO);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET ChangeRET_REQ_H_Status", ex);
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
                    sb.Append("     <td>").Append(item.RETURN_TYPE).Append("</td>");
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
        public string GetRetMainDetailHtml(List<V_RET_REQ_D> result, string REQ_STATUS, string APP_USER)
        {
            string read = string.Empty; string disable = string.Empty;
            //
            if (REQ_STATUS == "APPROVED" || REQ_STATUS == "CANCELED")
            {
                read = "readonly";
                disable = "disabled";
            }
            else if (!(new RETMAIN_BLL().CheckSecurity("RET_REQ_APP", APP_USER)))
            {
                read = "readonly";
                disable = "disabled";
            }

            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtLINE_NO\" value=\"" + item.LINE_NO + "\" title=\"" + item.LINE_NO + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtCUST_ITEM\" value=\"" + item.CUST_ITEM + "\" title=\"" + item.CUST_ITEM + "\" readonly />").Append("</td>");

                    if (item.REQ_STATUS == "NEW")
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY\" value=\"" + item.QTY + "\" title=\"" + item.QTY + "\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY\" value=\"" + item.QTY + "\" title=\"" + item.QTY + "\" readonly />").Append("</td>");

                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"" + item.PART_NO + "\" title =\"" + item.PART_NO + "\" readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtDESCRIPTION\" value=\"" + item.DESCRIPTION + "\" title=\"" + item.DESCRIPTION + "\" readonly />").Append("</td>");

                    if (Convert.ToDecimal(item.QTY) > item.QTY_APPROVED)
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY_APPROVED\" value=\"" + item.QTY_APPROVED + "\" title=\"" + item.QTY_APPROVED + "\" " + read + " style=\"border-color:red\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtQTY_APPROVED\" value=\"" + item.QTY_APPROVED + "\" title=\"" + item.QTY_APPROVED + "\" " + read + " />").Append("</td>");

                    if (string.IsNullOrEmpty(item.REJECT_REASON))
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtREJECT_REASON\" value=\"" + item.REJECT_REASON + "\" title=\"" + item.REJECT_REASON + "\" " + read + " />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtREJECT_REASON\" value=\"" + item.REJECT_REASON + "\" title=\"" + item.REJECT_REASON + "\" " + read + " style=\"color:red;\" />").Append("</td>");

                    sb.Append("     <td>").Append(GetPART_GROUP(item.PART_GROUP,disable)).Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm txtSUPPLIER_CODE\" value=\"" + item.SUPPLIER_CODE + "\" title=\"" + item.SUPPLIER_CODE + "\" " + read + " />").Append("</td>");
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
            sb.Append("     <td>").Append(GetPART_GROUP("", "")).Append("</td>");
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
        public string GetPART_GROUP(string PART_GROUP,string disable)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<select class=\"form-control input-sm txtPART_GROUP\" " + disable + ">");
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


        public ActionResult DownLoad(string fileName)
        {
            string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
            string fullPath = basePath + fileName;
            return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullPath));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetLOAD_TYPE(string appUser)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetLOAD_TYPEs(appUser);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetLOAD_TYPE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetLOAD_DESC(string LOAD_TYPE)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetLOAD_DESC(LOAD_TYPE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetLOAD_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetRETURN_TYPE()
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetRETURN_TYPEs().Select(a => a.RETURN_TYPE).ToList();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetRETURN_TYPE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetTYPE_DESC(string RETURN_TYPE)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                var result = rmBLL.GetRETURN_TYPEs().Where(a => a.RETURN_TYPE == RETURN_TYPE).First().TYPE_DESC;
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET GetTYPE_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetMainTemplateDownload(string LOAD_TYPE)
        {
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            try
            {
                IHUB_LOAD_TYPE ilt = rmBLL.GetIHUB_LOAD_TYPE(LOAD_TYPE);
                string fullHref = string.Empty;
                string fileName = ilt.LOAD_TEMPLATE;

                if (ilt.LOAD_FMT == "XLS")
                    fullHref = @"/ret/TemplateDownload?fileName=" + fileName;
                else
                    fullHref = @"/ret/TemplateDownloadTXT?fileName=" + fileName;

                return Json(new RequestResult(fullHref));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetMainTemplateDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult TemplateDownload(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fullPath = folder.FullName + fileName;

            return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullPath));
        }


        public void TemplateDownloadTXT(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);
            string fullPath = folder.FullName + fileName;

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(fullPath), System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }



        [Authorize]
        [HttpPost]
        public ActionResult RetMainTemplateUpload(string LOAD_TYPE, string INPUT1, string INPUT2, string INPUT3)
        {
            HttpPostedFileBase hpf = Request.Files["retmainloadInput"];
            RETMAIN_BLL rmBLL = new RETMAIN_BLL();
            DL_BLL dBLL = new DL_BLL();
            IHUB_LOAD_TYPE ilt = new IHUB_LOAD_TYPE();
            try
            {
                ilt = rmBLL.GetIHUB_LOAD_TYPE(LOAD_TYPE);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            bool hasTitle = true;
            string fileName = hpf.FileName;
            string extension = Path.GetExtension(fileName);//后缀名

            //导入的时候要匹配当前LOAD_TYPE的LOAD_FMT文件后缀格式
            string load_fmt = ilt.LOAD_FMT;
            if (!extension.Contains(load_fmt.ToLower()))
                return Json(new RequestResult(false, "Format does not match"));

            decimal first_row = ilt.FIRST_ROW;//从第几行开始导入
            string delimiter = ilt.DELIMITER;//csv文件用，分隔符

            //获取集合
            List<IHUB_LOAD_BASE> ilbs = GetLists(hpf, extension, first_row, delimiter, ref hasTitle);

            string LOAD_BATCH = dBLL.GetLOAD_BATCH();
            string LOAD_BY = Session[CHubConstValues.SessionUser].ToString();
            int Count = 0;
            bool load = true;

            //新增
            if (ilbs != null && ilbs.Count > 0)
            {
                int firstStart;
                decimal lineNo = first_row - 1;
                if (extension == ".xls" || extension == ".xlsx")
                {
                    if (hasTitle)
                        firstStart = (int)(first_row - 2);
                    else
                        firstStart = (int)(first_row - 1);
                }
                else
                    firstStart = (int)(first_row - 1);

                for (int i = firstStart; i < ilbs.Count; i++)
                {
                    Count++;
                    lineNo++;
                    string LOAD_LINE_NO = lineNo.ToString();
                    try
                    {
                        dBLL.AddIHUB_LOAD_BASE(ilbs[i], LOAD_BATCH, LOAD_TYPE, LOAD_BY, LOAD_LINE_NO, INPUT1, INPUT2, INPUT3);
                    }
                    catch (Exception ex)
                    {
                        load = false;
                        break;
                    }
                }
            }
            else
                return Json(new RequestResult(false, "No data"));

            if (!load)
                return Json(new RequestResult(false, "fail to load"));

            //执行存过，监测
            try
            {
                dBLL.ExecP_IHUB_LOAD_POST(Convert.ToDecimal(LOAD_BATCH), LOAD_TYPE);
            }
            catch (Exception ex)
            {
                return Json(new RequestResult(false, ex.Message));
            }
            return Json(new RequestResult(true, "成功导入数据:" + Count + "行"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hpf">上传文件</param>
        /// <param name="extension">后缀名</param>
        /// <param name="first_row">从第几行开始导入</param>
        /// <param name="delimiter">csv文件用，分隔符</param>
        /// <returns></returns>
        public List<IHUB_LOAD_BASE> GetLists(HttpPostedFileBase hpf, string extension, decimal first_row, string delimiter, ref bool hasTitle)
        {
            List<IHUB_LOAD_BASE> ilbs = new List<IHUB_LOAD_BASE>();

            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = string.Empty;
            if (extension == ".xls" || extension == ".xlsx")
                fileFullName = folder.FullName + tempGuid + ".xlsx";//excel
            else
                fileFullName = folder.FullName + tempGuid + ".csv";//csv
            hpf.SaveAs(fileFullName);

            try
            {
                if (extension == ".xls" || extension == ".xlsx")//excel
                {
                    ilbs = EXCELToList(fileFullName, first_row, ref hasTitle);
                }
                else//csv
                {
                    ilbs = CSVToList(fileFullName, delimiter);
                }
                System.IO.File.Delete(fileFullName);
            }
            catch (Exception ex)
            {
                return ilbs;
            }

            return ilbs;
        }

        /// <summary>
        /// EXCEL To List
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="first_row"></param>
        /// <param name="hasTitle"></param>
        /// <returns></returns>
        public List<IHUB_LOAD_BASE> EXCELToList(string fileName, decimal first_row, ref bool hasTitle)
        {
            List<IHUB_LOAD_BASE> ilbs = new List<IHUB_LOAD_BASE>();

            DataTable dt = new DataTable();
            NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fileName);
            if (first_row == 1)
                dt = npoiHelper.ExcelToDataTable(false);
            else
                dt = npoiHelper.ExcelToDataTable();

            string title = dt.Columns[0].Caption;
            if (title == "0")
                hasTitle = false;

            if (dt == null && dt.Rows.Count == 0)
                return ilbs;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List<string> datas = new List<string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    datas.Add(dt.Rows[i][j].ToString());
                }

                if (datas.Count > 24)
                    datas = datas.Take(24).ToList();

                if (datas.Count < 24)
                {
                    for (int a = datas.Count; a < 24; a++)
                    {
                        datas.Add("");
                    }
                }
                IHUB_LOAD_BASE ilb = new IHUB_LOAD_BASE()
                {
                    T01 = datas[0],
                    T02 = datas[1],
                    T03 = datas[2],
                    T04 = datas[3],
                    T05 = datas[4],
                    T06 = datas[5],
                    T07 = datas[6],
                    T08 = datas[7],
                    T09 = datas[8],
                    T10 = datas[9],
                    T11 = datas[10],
                    T12 = datas[11],
                    T13 = datas[12],
                    T14 = datas[13],
                    T15 = datas[14],
                    T16 = datas[15],
                    T17 = datas[16],
                    T18 = datas[17],
                    T19 = datas[18],
                    T20 = datas[19],
                    T21 = datas[20],
                    T22 = datas[21],
                    T23 = datas[22],
                    T24 = datas[23]
                };
                ilbs.Add(ilb);
            }
            return ilbs;
        }


        /// <summary>
        /// CSV To List
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public List<IHUB_LOAD_BASE> CSVToList(string fileName, string delimiter)
        {
            List<IHUB_LOAD_BASE> ilbs = new List<IHUB_LOAD_BASE>();

            string str = string.Empty;
            //FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            //StreamReader sr = new StreamReader(fs, Encoding.Default);

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                while ((str = sr.ReadLine()) != null)
                {
                    List<string> data = new List<string>();
                    List<string> strs = str.Split(delimiter.ToCharArray()).ToList();

                    var count = strs.Count;
                    if (count > 24)
                        strs = strs.Take(24).ToList();

                    for (int i = 0; i < strs.Count; i++)
                    {
                        data.Add(strs[i]);
                    }
                    if (count < 24)
                    {
                        for (int i = count; i < 24; i++)
                        {
                            data.Add("");
                        }
                    }
                    #region
                    IHUB_LOAD_BASE item = new IHUB_LOAD_BASE()
                    {
                        T01 = data[0],
                        T02 = data[1],
                        T03 = data[2],
                        T04 = data[3],
                        T05 = data[4],
                        T06 = data[5],
                        T07 = data[6],
                        T08 = data[7],
                        T09 = data[8],
                        T10 = data[9],
                        T11 = data[10],
                        T12 = data[11],
                        T13 = data[12],
                        T14 = data[13],
                        T15 = data[14],
                        T16 = data[15],
                        T17 = data[16],
                        T18 = data[17],
                        T19 = data[18],
                        T20 = data[19],
                        T21 = data[20],
                        T22 = data[21],
                        T23 = data[22],
                        T24 = data[23]
                    };
                    #endregion
                    ilbs.Add(item);
                }
            }

            return ilbs;
        }



        public ActionResult RetRestrict()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.retrestrict.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetRestrictSearch(string PART_NO)
        {
            RETRESTRICT_BLL rrBLL = new RETRESTRICT_BLL();
            try
            {
                var result = rrBLL.RetRestrictSearch(PART_NO);
                var mainHtml = GetRetRestrictHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetRestrictSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetRestrictSave(List<RetRestrictArg> arg)
        {
            RETRESTRICT_BLL rrBLL = new RETRESTRICT_BLL();
            try
            {
                if (arg != null && arg.Count() > 0)
                {
                    foreach (var item in arg)
                    {
                        rrBLL.RetRestrictSave(item);
                    }
                    return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "NO DATA"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetRestrictSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetRetRestrictHtml(List<V_RET_PART_RESTRICT> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm PART_NO' value='" + item.PART_NO + "' title='" + item.PART_NO + "' readonly />").Append("</td>");
                    sb.Append("     <td>").Append(GetRETURN_RESTRICT(item.RETURN_RESTRICT)).Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm RETURN_MOQ' value='" + item.RETURN_MOQ + "' title='" + item.RETURN_MOQ + "' />").Append("</td>");
                    sb.Append("     <td>").Append(item.RECORD_DATE).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }


        public string GetRETURN_RESTRICT(string RETURN_RESTRICT)
        {
            StringBuilder sb = new StringBuilder();
            string[] str = new string[] { "Y", "N" };
            sb.Append(" <select class='form-control input-sm RETURN_RESTRICT'>");
            foreach (var s in str)
            {
                if (s == RETURN_RESTRICT)
                    sb.Append("     <option value='" + s + "' selected>").Append(s).Append("</option>");
                else
                    sb.Append("     <option value='" + s + "'>").Append(s).Append("</option>");
            }

            sb.Append(" </select>");
            return sb.ToString();
        }

        [Authorize]
        [HttpPost]
        public ActionResult RetRestrictDownload()
        {
            RETRESTRICT_BLL rrBLL = new RETRESTRICT_BLL();
            try
            {
                string getSql = rrBLL.GetSql();
                string fileName = getSql.Split('~')[0] + ".xlsx";//文件名
                string sql = getSql.Split('~')[1];//sql
                DataTable dt = rrBLL.GetDataTableBySql(sql);
                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                string fullPath = basePath + fileName;
                NPOIExcelHelper npoi = new NPOIExcelHelper(fullPath);
                npoi.DataTableToExcel(dt, "Sheet1");
                return Json(new RequestResult(fileName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RET RetRestrictDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
    }
}
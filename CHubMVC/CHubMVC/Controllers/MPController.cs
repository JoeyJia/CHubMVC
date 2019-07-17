using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubModel;
using CHubDBEntity.UnmanagedModel;
using CHubBLL;
using CHubCommon;
using CHubMVC.Models;
using System.IO;
using System.Data;

namespace CHubMVC.Controllers
{
    public class MPController : BaseController
    {
        // GET: MP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MP_CUSTBANK()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mp_custbank.ToString(), this.Request.Url.AbsoluteUri);
            MPModels vm = new MPModels();
            ViewBag.AppUser = appUser;
            return View(vm);
        }

        [HttpPost]
        public ActionResult MP_CUSTBANK(MPModels vm)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                var result = mpBLL.MP_CUSTBANKSearch(vm.SearchCondition);
                vm.CBCollection = result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_CUSTBANK", ex);
                ViewBag.ErrorMsg = ex.Message;
            }
            ViewBag.AppUser = Session[CHubConstValues.SessionUser].ToString();
            return View(vm);
        }

        [HttpPost]
        public ActionResult MP_CUSTBANKSave(V_E_CUST_BANKING item, string appUser)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                //权限检查
                if (mpBLL.CheckSecurity("BANKING_MST_SAVE", appUser))
                {
                    mpBLL.MP_CUSTBANKSave(item, appUser);
                    var result = mpBLL.MP_CUSTBANK(item);
                    return Json(new RequestResult(result));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_CUSTBANKSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetManualADJTransType(string App_User = null, string TRANS_TYPE = null)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                if (mpBLL.CheckSecurity("BALANCE_ADJ", App_User))
                {
                    var result = mpBLL.GetManualADJTransType(App_User, TRANS_TYPE);
                    return Json(new RequestResult(result));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetManualADJTransType", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ManualADJOrderNoCheckAndAmt(string CUSTOMER_NO, string BILL_TO_LOCATION, string ORDER_NO)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                var msg = mpBLL.CheckOrderNo(CUSTOMER_NO, BILL_TO_LOCATION, ORDER_NO);
                var data = mpBLL.GetAmtByOrderNo(ORDER_NO);
                var brief = mpBLL.CallF_GOMS_ORD_BRIEF(ORDER_NO);
                return Json(new { Success = true, Msg = msg, Data = data, Brief = brief });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP ManualADJOrderNoCheckAndAmt", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ManualADJcheckDOC_NO(string DOC_NO)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                if (mpBLL.ManualADJcheckDOC_NO(DOC_NO))
                    return Json(new RequestResult(false, "Document No 已存在。"));
                else
                    return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP ManualADJcheckDOC_NO", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult RunP_BANK_TRANS_NEW(E_BANKING_TRANS item)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                mpBLL.RunP_BANK_TRANS_NEW(item);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP RunP_BANK_TRANS_NEW", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult TransHistoryGetCUSTBANK(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                V_E_CUST_BANKING item = new V_E_CUST_BANKING();
                item.CUSTOMER_NO = CUSTOMER_NO;
                item.BILL_TO_LOCATION = Convert.ToDecimal(BILL_TO_LOCATION);
                item.CURRENCY_CODE = CURRENCY_CODE;
                var custBank = mpBLL.MP_CUSTBANK(item);
                var transType = mpBLL.GetTransType();
                return Json(new { Success = true, custBank = custBank, transType = transType });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP TransHistoryGetCUSTBANK", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult TransHistoryGetTrans_TYPE(string TRANS_TYPE)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                var result = mpBLL.TransHistoryGetTrans_TYPE(TRANS_TYPE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP TransHistoryGetTrans_TYPE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult TransHistoryQuery(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE, string TRANS_TYPE, string TRANS_DATE, int PageIndex)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                int PageSize = 50;
                bool showMore = true;
                var result = mpBLL.TransHistoryQuery(CUSTOMER_NO, BILL_TO_LOCATION, CURRENCY_CODE, TRANS_TYPE, TRANS_DATE);
                if ((PageIndex * PageSize) >= result.Count())
                    showMore = false;
                result = result.Skip<V_E_BANKING_TRANS>((PageIndex - 1) * PageSize).Take<V_E_BANKING_TRANS>(PageSize).ToList();
                var mainHtml = TransHistoryQueryHtml(result);
                return Json(new RequestResult(true, showMore ? "" : "End", mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP TransHistoryQuery", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult TransHistoryDownload(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE, string TRANS_TYPE, string TRANS_DATE)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                DataTable dt = mpBLL.TransHistoryDownload(CUSTOMER_NO, BILL_TO_LOCATION, CURRENCY_CODE, TRANS_TYPE, TRANS_DATE);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                    string fullPath = basePath + fileName;
                    NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fullPath);
                    npoiHelper.DataTableToExcel(dt, "Sheet1");
                    return Json(new RequestResult(fileName));
                }
                else
                    return Json(new RequestResult(false, "No data"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP TransHistoryDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public ActionResult Download(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);
            string fullPath = folder.FullName + fileName;
            return File(fullPath, "application/ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult LoadCheck(string AppUser)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();
            try
            {
                if (mpBLL.CheckSecurity("BANK_RCPT_LOAD", AppUser))
                    return Json(new RequestResult(true));
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP LoadCheck", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult BankUpload(string AppUser)
        {
            MP_CUSTBANK_BLL mpBLL = new MP_CUSTBANK_BLL();

            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
            try
            {
                DataTable dt = GetDataFromExcel(hpf);
                if (dt == null && dt.Rows.Count == 0)
                    return Json(new RequestResult(false, "No data"));

                //list = ClassConvert.DataTableToList<E_BANKING_TRANS_LOAD>(dt);

                decimal LOAD_BATCH = mpBLL.GetLOAD_BATCH();
                decimal LINE_NO = 0;
                //load
                foreach (DataRow dr in dt.Rows)
                {
                    LINE_NO++;
                    E_BANKING_TRANS_LOAD item = new E_BANKING_TRANS_LOAD();
                    item.TRANS_TYPE = dr["TRANS_TYPE"].ToString();
                    item.CURRENCY_CODE = dr["CURRENCY_CODE"].ToString();
                    item.BANK_PAYER = dr["BANK_PAYER"].ToString();
                    item.TRANS_AMT = !string.IsNullOrEmpty(dr["TRANS_AMT"].ToString()) ? Convert.ToDecimal(dr["TRANS_AMT"].ToString()) : 0;
                    item.TRANS_DOC_NO = dr["TRANS_DOC_NO"].ToString();
                    var date = dr["TRANS_DATE(YYYYMMDD)"].ToString().Substring(0, 4) + "/" + dr["TRANS_DATE(YYYYMMDD)"].ToString().Substring(4, 2) + "/" + dr["TRANS_DATE(YYYYMMDD)"].ToString().Substring(6, 2);
                    item.TRANS_DATE = !string.IsNullOrEmpty(dr["TRANS_DATE(YYYYMMDD)"].ToString()) ? Convert.ToDateTime(date) : DateTime.Now;
                    item.TRANS_BRIEF = dr["TRANS_BRIEF"].ToString();
                    item.NOTE = dr["NOTE"].ToString();
                    mpBLL.LoadData(item, AppUser, LOAD_BATCH, LINE_NO);
                }
                //Run Proc
                try
                {
                    mpBLL.RunP_BANK_TRANS_LOAD_POST(LOAD_BATCH);
                }
                catch (Exception ex)
                {
                    return Json(new RequestResult(false, "执行存过失败"));
                }

                //Download
                DataTable downDT = mpBLL.BankReceiptDownload(LOAD_BATCH);
                string fileName = "Bank_Receipt_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                string folderName = Server.MapPath(CHubConstValues.ChubTempFolder);
                FileInfo folder = new FileInfo(folderName);
                if (!Directory.Exists(folder.FullName))
                    Directory.CreateDirectory(folder.FullName);

                string fullName = folder.FullName + fileName;
                NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fullName);
                npoiHelper.DataTableToExcel(downDT, "Sheet1");

                return Json(new RequestResult(fileName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP BankUpload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public DataTable GetDataFromExcel(HttpPostedFileBase hpf)
        {
            DataTable dt = new DataTable();
            string tempName = Guid.NewGuid().ToString();
            string folderFile = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderFile);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fullName = folder.FullName + tempName + ".xls";
            hpf.SaveAs(fullName);

            NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fullName);
            dt = npoiHelper.ExcelToDataTable();
            System.IO.File.Delete(fullName);
            return dt;
        }

        public ActionResult DownloadTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            FileInfo folder = new FileInfo(templateFolder);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);
            string fileName = CHubConstValues.BankReceiptTemplateName;
            string fullPath = folder.FullName + fileName;
            return File(fullPath, "application/ms-excel", fileName);
        }

        public string TransHistoryQueryHtml(List<V_E_BANKING_TRANS> list)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.TRANS_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_TYPE).Append("</td>");
                    if(item.TRANS_AMT>=0)
                        sb.Append("     <td style='color:green;'>").Append(item.TRANS_AMT).Append("</td>");
                    else
                        sb.Append("     <td style='color:orange;'>").Append(item.TRANS_AMT).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_DOC_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_BRIEF).Append("</td>");
                    sb.Append("     <td>").Append(item.APP_USER).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    if(item.BALANCE_AFT_TRANS>=0)
                        sb.Append("     <td style='color:green;'>").Append(item.BALANCE_AFT_TRANS).Append("</td>");
                    else
                        sb.Append("     <td style='color:orange;'>").Append(item.BALANCE_AFT_TRANS).Append("</td>");

                    sb.Append("     <td>").Append(item.ORDER_NO).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }
    }
}
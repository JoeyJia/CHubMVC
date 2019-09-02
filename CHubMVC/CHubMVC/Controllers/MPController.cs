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
using System.Text;
using CHubModel.WebArg;

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
                    if (string.IsNullOrEmpty(dr["TRANS_TYPE"].ToString()) && string.IsNullOrEmpty(dr["CURRENCY_CODE"].ToString()))
                        break;

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
                    if (item.TRANS_AMT >= 0)
                        sb.Append("     <td style='color:green;'>").Append(item.TRANS_AMT).Append("</td>");
                    else
                        sb.Append("     <td style='color:orange;'>").Append(item.TRANS_AMT).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_DOC_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.TRANS_BRIEF).Append("</td>");
                    sb.Append("     <td>").Append(item.APP_USER).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    if (item.BALANCE_AFT_TRANS >= 0)
                        sb.Append("     <td style='color:green;'>").Append(item.BALANCE_AFT_TRANS).Append("</td>");
                    else
                        sb.Append("     <td style='color:orange;'>").Append(item.BALANCE_AFT_TRANS).Append("</td>");

                    sb.Append("     <td>").Append(item.ORDER_NO).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        [Authorize]
        public ActionResult MP_ADDRMAP()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mp_addrmap.ToString(), this.Request.Url.AbsoluteUri);
            MPModels vm = new MPModels();
            vm.addrSearchCondition = new V_E_ADDR_MST();
            vm.addrSearchCondition.LastDays = "7";
            vm.appUser = appUser;
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult MP_ADDRMAP(MPModels vm)
        {
            MP_ADDRMAP_BLL amBLL = new MP_ADDRMAP_BLL();
            vm.addrCollection = amBLL.GetV_E_ADDR_MST(vm.addrSearchCondition);
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetMP_ADDRMAP_Detail(string ADDR_TOKEN, string TO_SYSTEM, string ABBR, string DEST_LOCATION)
        {
            MP_ADDRMAP_BLL mpBLL = new MP_ADDRMAP_BLL();
            try
            {
                var ca = mpBLL.GetCustomerAddress(ADDR_TOKEN, TO_SYSTEM, ABBR, DEST_LOCATION).First();
                var ga = mpBLL.GetGomsAddress(TO_SYSTEM, ABBR, DEST_LOCATION).FirstOrDefault();
                return Json(new { Success = true, ca = ca, ga = ga });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetMP_ADDRMAP_Detail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetGomsAddressByKeyWord(string keyWord, string TO_SYSTEM, string ABBR)
        {
            MP_ADDRMAP_BLL mpBLL = new MP_ADDRMAP_BLL();
            try
            {
                var result = mpBLL.GetGomsAddressByKeyWord(keyWord, TO_SYSTEM, ABBR);
                var mainHtml = GomsAddressHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetGomsAddressByKeyWord", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MP_ADDRMAP_Confirm(string ADDR_TOKEN, string SYSID, string ABBREVIATION, string DEST_LOCATION, string APP_USER)
        {
            MP_ADDRMAP_BLL mpBLL = new MP_ADDRMAP_BLL();
            try
            {
                //更新表E_ADDR_MST字段dest_location, updated_by, record_date
                mpBLL.UpdateE_ADDR_MST(ADDR_TOKEN, DEST_LOCATION, APP_USER);
                //刷新GOMS Address (Mapped) 区域的数据
                var ga = mpBLL.RefreshGomsAddress(ADDR_TOKEN, SYSID, ABBREVIATION, DEST_LOCATION, APP_USER).FirstOrDefault();
                return Json(new RequestResult(ga));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_ADDRMAP_Confirm", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GomsAddressHtml(List<G_ADDR_SPL> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type='radio' name='Radio' data-sysid='" + item.SYSID + "' data-abbr='" + item.ABBREVIATION + "' data-destLocation='" + item.DEST_LOCATION + "' />").Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_ADDR_1).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_ADDR_2).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_ADDR_3).Append("</td>");
                    sb.Append("     <td>").Append(item.DEST_CONTACT).Append("</td>");
                    sb.Append("     <td>").Append(item.DEST_PHONE).Append("</td>");
                    sb.Append("     <td>").Append(item.ABBREVIATION).Append("</td>");
                    var time = item.RECORD_DATE_OSDL.HasValue ? item.RECORD_DATE_OSDL.Value.ToString("yyyy/MM/dd") : "";
                    sb.Append("     <td>").Append(time).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }


        [Authorize]
        [HttpGet]
        public ActionResult MP_MAIN()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mp_main.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        /// <summary>
        /// 获取WAREHOUSE下拉选择
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetWarehouseList()
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var result= mpBLL.GetWarehouseList();
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        list.Add(new SelectListItem() { Value = item.WAREHOUSE, Text = item.WAREHOUSE + " (" + item.WAREHOUSE_DESC + ")" });
                    }
                }
                return Json(new RequestResult(list));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetWarehouseList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// 获取ORDER_STATUS下拉选择
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetOrderStatusList()
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var result = mpBLL.GetOrderStatusList();
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        list.Add(new SelectListItem() { Value = item.ORDER_STATUS, Text = item.ORDER_STATUS + " (" + item.STATUS_DESC + ")" });
                    }
                }
                return Json(new RequestResult(list));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetOrderStatusList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// 获取SHIP_METHOD下拉选择
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetShipMethodList()
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var result = mpBLL.GetShipMethodList();
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        list.Add(new SelectListItem() { Value = item.SHIP_METHOD, Text = item.SHIP_METHOD + " (" + item.SHIP_METHOD_DESC + ")" });

                    }
                } 
                return Json(new RequestResult(list));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetShipMethodList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// 联动WAREHOUSE，获取ORDER_TYPE下拉选择
        /// </summary>
        /// <param name="WAREHOUSE"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetOrderTypeList(string WAREHOUSE)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var result = mpBLL.GetOrderTypeList();
                result = result.Where(i => i.WAREHOUSE == WAREHOUSE).ToList();
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        list.Add(new SelectListItem() { Value = item.ORDER_TYPE, Text = item.ORDER_TYPE + " (" + item.ORDER_TYPE_DESC + ")" });
                    }
                }
                string WAREHOUSE_DESC = "";
                var wh = mpBLL.GetWarehouseList().Where(i => i.WAREHOUSE == WAREHOUSE).FirstOrDefault();
                if (wh != null)
                    WAREHOUSE_DESC = wh.WAREHOUSE_DESC;
                return Json(new RequestResult(true, WAREHOUSE_DESC, list));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetOrderTypeList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetOrderTypeDesc(string WAREHOUSE,string ORDER_TYPE)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                string ORDER_TYPE_DESC = "";
                var ot = mpBLL.GetOrderTypeList().Where(i => i.WAREHOUSE == WAREHOUSE && i.ORDER_TYPE == ORDER_TYPE).FirstOrDefault();
                if (ot != null)
                    ORDER_TYPE_DESC = ot.ORDER_TYPE_DESC;
                return Json(new RequestResult(ORDER_TYPE_DESC));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetOrderTypeDesc", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetOrderStatusDesc(string ORDER_STATUS)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                var STATUS_DESC = "";
                var os = mpBLL.GetOrderStatusList().Where(i => i.ORDER_STATUS == ORDER_STATUS).FirstOrDefault();
                if (os != null)
                    STATUS_DESC = os.STATUS_DESC;
                return Json(new RequestResult(STATUS_DESC));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetOrderStatusDesc", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetShipMethodDesc(string SHIP_METHOD)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                string SHIP_METHOD_DESC = "";
                var sm = mpBLL.GetShipMethodList().Where(i => i.SHIP_METHOD == SHIP_METHOD).FirstOrDefault();
                if (sm != null)
                    SHIP_METHOD_DESC = sm.SHIP_METHOD_DESC;
                return Json(new RequestResult(SHIP_METHOD_DESC));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetShipMethodDesc", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// MP_MAIN 查询
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult MP_MAINSearch(MPMainArg arg)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                var result = mpBLL.MP_MAINSearch(arg);
                var mainHtml = GetMP_MAINHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_MAINSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetMPMainInfo(string SO_NO)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                MPMainArg arg = new MPMainArg();
                arg.SO_NO = SO_NO;
                var result = mpBLL.MP_MAINSearch(arg).First();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP GetMPMainInfo", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MP_MAINDetail(string SO_NO)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                var result = mpBLL.MP_MAINDetail(SO_NO);
                var mainHmtl = GetMP_MAINDetailHtml(result);
                return Json(new RequestResult(mainHmtl));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_MAINDetail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MP_MAINCheck(string SO_NO,string APP_USER)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                if (mpBLL.CheckSecurity("MP_ORDER_OPT", APP_USER))
                {
                    var result = mpBLL.RunF_CHECK_IN_GOMS(SO_NO);
                    if (result == "1")
                        return Json(new RequestResult(false, "订单已经在GOMS 中存在!"));
                    else
                        return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "You cannot Operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_MAINCheck", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MP_MAINConfirm(string SO_NO, string Code, string Reason)
        {
            MP_MAIN_BLL mpBLL = new MP_MAIN_BLL();
            try
            {
                mpBLL.MP_MAINConfirm(SO_NO, Code, Reason);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_MAINConfirm", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MP_MAINMapping(string ADDR_TOKEN)
        {
            MP_MAIN_BLL mmBLL = new MP_MAIN_BLL();
            MP_ADDRMAP_BLL maBLL = new MP_ADDRMAP_BLL();
            try
            {
                var ca = maBLL.GetCustomerAddress(ADDR_TOKEN).First();
                var ga = maBLL.GetGomsAddress(ca.TO_SYSTEM, ca.ABBR, ca.DEST_LOCATION.ToString()).FirstOrDefault();
                return Json(new { Success = true, ca = ca, ga = ga });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MP MP_MAINMapping", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetMP_MAINHtml(List<V_E_SO_HEADER> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td style='color:" + GetColor(item.STATUS_COLOR) + ";'>").Append(item.ORDER_STATUS).Append("</td>");
                    sb.Append("     <td style='color:" + GetColor(item.GOMS_COLOR) + ";'>").Append(item.GOMS_STATUS).Append("</td>");
                    sb.Append("     <td>").Append("<a href='javascript:void(0);' class='MAP_STATUS' data-addrtoken='" + item.ADDR_TOKEN + "' style='color:" + GetColor(item.MAP_COLOR) + ";'>" + item.MAP_STATUS + "</a>").Append("</td>");
                    sb.Append("     <td>").Append(item.WAREHOUSE).Append("</td>");
                    sb.Append("     <td>").Append("<a href='javascript:void(0);' class='SO_NO' data-sono='" + item.SO_NO + "'>" + item.SO_NO + "</a>").Append("</td>");
                    sb.Append("     <td>").Append("<a href='javascript:void(0);' class='SHIP_NAME' data-sono='" + item.SO_NO + "'>" + item.SHIP_NAME + "</a>").Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_TYPE).Append("</td>");
                    sb.Append("     <td>").Append(item.DUE_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.TOTAL_AMT).Append("</td>");
                    sb.Append("     <td>").Append(item.ORDER_NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.CUSTOMER_NO).Append("</td>");
                    sb.Append("     <td>").Append("<a href='javascript:void(0);' class='PAY_METHOD' data-sono='" + item.SO_NO + "'>" + item.PAY_METHOD + "</a>").Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_METHOD).Append("</td>");
                    sb.Append("     <td>").Append("<a href='javascript:void(0);' class='FP_TYPE' data-sono='" + item.SO_NO + "'>" + item.FP_TYPE + "</a>").Append("</td>");
                    sb.Append("     <td>").Append(item.CREATE_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>")
                        .Append("<input type='button' class='btn btn-primary btn-xs btnDetail' value='LINES' data-sono='" + item.SO_NO + "' />");
                    if (item.ORDER_STATUS == "NEW" || item.ORDER_STATUS == "ONHOLD" || item.ORDER_STATUS == "REJECTED")
                        sb.Append("<input type='button' class='btn btn-primary btn-xs btnCancel' value='Cancel' data-sono='" + item.SO_NO + "' />");
                    else
                        sb.Append("<input type='button' class='btn btn-primary btn-xs btnCancel' value='Cancel' data-sono='" + item.SO_NO + "' disabled />");
                    if (item.ORDER_STATUS == "NEW" || item.ORDER_STATUS == "REJECTED")
                        sb.Append("<input type='button' class='btn btn-primary btn-xs btnHold' value='Hold' data-sono='" + item.SO_NO + "' />");
                    else
                        sb.Append("<input type='button' class='btn btn-primary btn-xs btnHold' value='Hold' data-sono='" + item.SO_NO + "' disabled />");
                    sb.Append("     </td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetMP_MAINDetailHtml(List<V_E_SO_DETAIL> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.UNIT_PRICE).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY).Append("</td>");
                    sb.Append("     <td>").Append(item.PRINT_PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.MOVEX_PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.CUSTOMER_PARTNO).Append("</td>");
                    sb.Append("     <td>").Append(item.LINE_NOTE).Append("</td>");
                    sb.Append("     <td>").Append(item.LIST_PRICE).Append("</td>");
                    sb.Append("     <td>").Append(item.CREATE_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetColor(string Color)
        {
            string result = "";
            switch (Color)
            {
                case "R":
                    result = "red";
                    break;
                case "G":
                    result = "green";
                    break;
                case "B":
                    result = "blue";
                    break;
                case "Y":
                    result = "orange";
                    break;
            }
            return result;
        }
    }
}
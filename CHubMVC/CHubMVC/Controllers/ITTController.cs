using CHubBLL;
using CHubCommon;
using CHubDBEntity;
using CHubModel;
using CHubMVC.Validations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
using CHubModel.ExtensionModel;
using CHubDBEntity.UnmanagedModel;

namespace CHubMVC.Controllers
{
    public class ITTController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.wbentry.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchWillBill(string willBillNo,string invoiceNo)
        {
            V_ITT_SHIPPING_SMRY_BLL ittBLL = new V_ITT_SHIPPING_SMRY_BLL();
            List<V_ITT_SHIPPING_SMRY> result = ittBLL.GetWillBillList(willBillNo,invoiceNo);
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTranLoadList(string willBillNo)
        {
            ITT_TRAN_LOAD_BLL tranBLL = new ITT_TRAN_LOAD_BLL();
            List<ITT_TRAN_LOAD> result = tranBLL.GetTranLoadList(willBillNo);
            foreach (var item in result)
            {
                if (item.LOAD_DATE == null)
                    item.LOAD_DATE = DateTime.Now;
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTranTypeList()
        {
            ITT_TRAN_TYPE_BLL typeBLL = new ITT_TRAN_TYPE_BLL();
            List<ITT_TRAN_TYPE> result = typeBLL.GetTranType();
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult PreFillTranLoad(string wayBillNo)
        {
            try
            {
                TranLoadPreFill result = PreFillTranLoadAction(wayBillNo);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Pre Fill TranLoad", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult PrefillByInovice(string invoiceNo)
        {
            try
            {
                V_SHIPPING_ALL_BASE_BLL saBLL = new V_SHIPPING_ALL_BASE_BLL();
                V_SHIPPING_ALL_BASE sa = saBLL.GetFirstBaseInfoByInvoice(invoiceNo);
                TranLoadPreFill result = new TranLoadPreFill();
                if (sa != null)
                {
                    result.InvoiceNo = sa.INVOICE_NO;
                    result.TranType = sa.TRAN_TYPE;
                    result.FromSystem = sa.FROM_SYSTEM;
                    result.Msg = "";
                    result.BackColor = "";

                    //ITT_TRAN_LOAD_BLL tlBLL = new ITT_TRAN_LOAD_BLL();
                    //if (tlBLL.ExistInvoiceNo(result.InvoiceNo))
                    //{
                    //    result.Msg = "Invoice No is exist";
                    //    result.BackColor = CHubConstValues.ErrorColor;
                    //}
                }
                else
                {
                    result.Msg = "Invalid invoice No";
                    result.BackColor = CHubConstValues.ErrorColor;
                }
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                return Json(new RequestResult(false, ex.Message));
            }
        }
          

        [HttpPost]
        [Authorize]
        public ActionResult SaveTranLoad(ITT_TRAN_LOAD model)
        {
            try
            {
                if(model.WILL_BILL_NO==null||model.FROM_SYSTEM==null)
                    return Json(new RequestResult(false, "WillBillNo or FromSystem can't be empty"));

                string msg = SaveTranLoadAction(model);
                if(!string.IsNullOrEmpty(msg))
                     return Json(new RequestResult(false, msg));
                return Json(new RequestResult(true,null,model.LOAD_BATCH_TOKEN));
            }
            catch (Exception ex)
            {
                //this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //return Content(ex.Message);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteTranLoad(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return Json(new RequestResult(false, "token is empty"));
                ITT_TRAN_LOAD_BLL tranBLL = new ITT_TRAN_LOAD_BLL();
                tranBLL.Delete(token);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadTranLoadFile()
        {
            try
            {
                HttpPostedFileBase fb = Request.Files[0];
                string tempGuid = Guid.NewGuid().ToString();
                string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                FileInfo folder = new FileInfo(folderPath);
                if (!Directory.Exists(folder.FullName))
                    Directory.CreateDirectory(folder.FullName);
                //fb.filename - to get short file name parse string
                 string errorLogName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + tempGuid + ".txt";
                string errorLogWebName = "/temp/" + errorLogName;
                string errorLogFullName = folder.FullName + errorLogName;
                TxtLog txtLog = new TxtLog();
                StringBuilder errorMsg = new StringBuilder();
                errorMsg.AppendLine(string.Format("Current User:{0}", Session[CHubConstValues.SessionUser].ToString()));

                string fileFullName = folder.FullName + tempGuid + ".xlsx";
                fb.SaveAs(fileFullName);

                NPOIExcelHelper excelHelper = new NPOIExcelHelper(fileFullName);
                DataTable dt = excelHelper.ExcelToDataTable();

                //LogHelper.WriteLog("DT data:"+dt.Rows[0][3].ToString() +"|"+ dt.Rows[0][4].ToString() + "|" + dt.Rows[0][5].ToString());
                //Delete temp file
                System.IO.File.Delete(fileFullName);

                if (dt == null || dt.Rows.Count == 0)
                    return Json(new RequestResult(false, "No data in excel"));

                //DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
                List<ITT_TRAN_LOAD> modelList = ClassConvert.ConvertDT2List<ITT_TRAN_LOAD>(dt);

                if (modelList == null || modelList.Count == 0)
                    return Json(new RequestResult(false, "wrong excel struct"));

                int successCount = 0;
                int failCount = 0;
                foreach (var item in modelList)
                {
                    if (item.INVOICE_NO!=null&&item.INVOICE_NO.Contains("/"))
                    {
                        string[] invoiceArray = item.INVOICE_NO.Split('/');
                        foreach (var inNo in invoiceArray)
                        {
                            ITT_TRAN_LOAD model = new ITT_TRAN_LOAD();
                            ClassConvert.DrawObj(item, model);
                            model.INVOICE_NO = inNo.Trim();

                            string msgInside = SaveTranLoadAction(model);
                            if (string.IsNullOrEmpty(msgInside))
                            {
                                successCount++;
                                LogHelper.WriteLog(string.Format("willBillNo:{0},Data:{1}", item.WILL_BILL_NO, JsonConvert.SerializeObject(item)));
                            }
                            else
                            {
                                failCount++;
                                LogHelper.WriteErrorLog(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msgInside, JsonConvert.SerializeObject(item)));
                                errorMsg.AppendLine(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msgInside, JsonConvert.SerializeObject(item)));
                                //txtLog.log(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msgInside, JsonConvert.SerializeObject(item)), errorLogFullName);
                            }
                        }
                    }
                    else
                    {
                        //If no invoce no , do a prefill ation
                        if (string.IsNullOrEmpty(item.INVOICE_NO))
                        {
                            TranLoadPreFill result = PreFillTranLoadAction(item.WILL_BILL_NO);
                            if (!string.IsNullOrEmpty(result.Msg))
                            {
                                failCount++;
                                LogHelper.WriteErrorLog(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, result.Msg, JsonConvert.SerializeObject(item)));
                                errorMsg.AppendLine(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, result.Msg, JsonConvert.SerializeObject(item)));
                                continue;
                            }
                            else
                            {
                                //prefill action
                                item.INVOICE_NO = result.InvoiceNo;
                                item.TRAN_TYPE = result.TranType;
                                item.FROM_SYSTEM = result.FromSystem;
                            }
                        }

                        string msg = SaveTranLoadAction(item);
                        if (string.IsNullOrEmpty(msg))
                        {
                            successCount++;
                            LogHelper.WriteLog(string.Format("willBillNo:{0},Data:{1}", item.WILL_BILL_NO, JsonConvert.SerializeObject(item)));
                        }
                        else
                        {
                            failCount++;
                            LogHelper.WriteErrorLog(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msg, JsonConvert.SerializeObject(item)));
                            errorMsg.AppendLine(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msg, JsonConvert.SerializeObject(item)));
                        }
                    }
                }

                bool success = true;
                if (failCount > 0)
                {
                    txtLog.log(errorMsg.ToString(), errorLogFullName);
                    success = false;
                }
                return Json(new RequestResult(success, string.Format("Total Lines:{0}, Success items:{1}, Fail items:{2}", modelList.Count, successCount, failCount), errorLogWebName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("UploadTranLoadFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadTranLoadTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.TranLoadExcelTemplateName;

            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        //Custom process part
        [HttpPost]
        [Authorize]
        public ActionResult GetCustList(string willBillNo)
        {
            ITT_CUST_LOAD_BLL custBLL = new ITT_CUST_LOAD_BLL();
            List<ITT_CUST_LOAD> result = custBLL.GetCustList(willBillNo);
            foreach (var item in result)
            {
                if (item.LOAD_DATE == null)
                    item.LOAD_DATE = DateTime.Now;
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTCGroupTypeList()
        {
            TC_PART_CATEGORY_BLL tcCateBLL = new TC_PART_CATEGORY_BLL();
            List<string> result = tcCateBLL.GetTCGroupList();
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetITTSysIDList()
        {
            M_SYSTEM_BLL msBLL = new M_SYSTEM_BLL();
            List<string> result = msBLL.GetITTSysIDList();
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveCustLoad(ITT_CUST_LOAD model)
        {
            try
            {
                if (model.WILL_BILL_NO == null)
                    return Json(new RequestResult(false, "WillBillNo can't be empty"));

                ITT_CUST_LOAD_BLL ctBLL = new ITT_CUST_LOAD_BLL();
                if (ctBLL.ExistCustLoad(model.WILL_BILL_NO, model.TC_GROUP))
                {
                    ITT_CUST_LOAD temp = ctBLL.GetCustLoadbyConstraint(model.WILL_BILL_NO, model.TC_GROUP);
                    if (model.LOAD_BATCH_TOKEN != temp.LOAD_BATCH_TOKEN)
                        return Json(new RequestResult(false, "Exist same wayBillNO and TC Group Items"));
                }

                string msg = SaveCustLoadAction(model);
                if (!string.IsNullOrEmpty(msg))
                    return Json(new RequestResult(false, msg));

                return Json(new RequestResult(true,null,model.LOAD_BATCH_TOKEN));
            }
            catch (Exception ex)
            {
                //this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //return Content(ex.Message);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteCustLoad(string token)
        {
            try
            {
                if(string.IsNullOrEmpty(token))
                    return Json(new RequestResult(false,"token is empty"));
                ITT_CUST_LOAD_BLL custBLL = new ITT_CUST_LOAD_BLL();
                custBLL.Delete(token);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadCustLoadFile()
        {
            try
            {
                HttpPostedFileBase fb = Request.Files[0];
                string tempGuid = Guid.NewGuid().ToString();
                string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                FileInfo folder = new FileInfo(folderPath);
                if (!Directory.Exists(folder.FullName))
                    Directory.CreateDirectory(folder.FullName);

                //fb.filename - to get short file name parse string
                string errorLogName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + tempGuid + ".txt";
                string errorLogWebName = "/temp/" + errorLogName;
                string errorLogFullName = folder.FullName + errorLogName;
                TxtLog txtLog = new TxtLog();
                StringBuilder errorMsg = new StringBuilder();
                errorMsg.AppendLine(string.Format("Current User:{0}", Session[CHubConstValues.SessionUser].ToString()));

                string fileFullName = folder.FullName + tempGuid + ".xlsx";
                fb.SaveAs(fileFullName);

                NPOIExcelHelper excelHelper = new NPOIExcelHelper(fileFullName);
                DataTable dt = excelHelper.ExcelToDataTable();
                //Delete temp file
                System.IO.File.Delete(fileFullName);

                if (dt == null || dt.Rows.Count == 0)
                    return Content("No data in excel");

                //DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
                List<ITT_CUST_LOAD> modelList = ClassConvert.ConvertDT2List<ITT_CUST_LOAD>(dt);

                if (modelList == null || modelList.Count == 0)
                    return Content("wrong excel strut");

                int successCount = 0;
                int failCount = 0;
                foreach (var item in modelList)
                {
                    string msg = SaveCustLoadAction(item);
                    if (string.IsNullOrEmpty(msg))
                    {
                        successCount++;
                        LogHelper.WriteLog(string.Format("willBillNo:{0},data:{1}", item.WILL_BILL_NO,JsonConvert.SerializeObject(item)));
                    }
                    else
                    {
                        failCount++;
                        LogHelper.WriteErrorLog(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msg, JsonConvert.SerializeObject(item)));
                        errorMsg.AppendLine(string.Format("willBillNo:{0},message:{1},data:{2}", item.WILL_BILL_NO, msg, JsonConvert.SerializeObject(item)));
                    }
                }

                bool success = true;
                if (failCount > 0)
                {
                    txtLog.log(errorMsg.ToString(), errorLogFullName);
                    success = false;
                }
                return Json(new RequestResult(success, string.Format("Total Lines:{0}, Success items:{1}, Fail items:{2}", modelList.Count, successCount, failCount), errorLogWebName));
                //return Content(string.Format("Total Lines:{0}, Success items:{1}, Fail items:{2}", modelList.Count, successCount, failCount));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("save Cust load", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadCustLoadTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.CustLoadExcelTemplateName;

            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetArrivalDateFromOutDate(DateTime outDate)
        {
            M_CALENDAR_BLL calBLL = new M_CALENDAR_BLL();
            DateTime arrDate = calBLL.GetArrivalDateFromOutDate(outDate).Value;
            return Json(arrDate);
        }

        //easy query part
        [Authorize]
        public ActionResult EasyQuery()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.ittezinq.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult FuzzyQueryParNo(string fuzzypartNo)
        {
            G_PART_DESCRIPTION_BLL gpBLL = new G_PART_DESCRIPTION_BLL();
            return Json(gpBLL.fuzzyqueryByPartNo(fuzzypartNo));
        }

        [HttpPost]
        [Authorize]
        public ActionResult InitAllPageData(string partNo)
        {

            return Json(new RequestResult(true));
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddEasyQueryLog(string partNo)
        {
            try
            {
                ITT_EASY_QUERY_LOG_BLL eqLogBLL = new ITT_EASY_QUERY_LOG_BLL();
                ITT_EASY_QUERY_LOG model = new ITT_EASY_QUERY_LOG();
                model.PART_NO = partNo;
                model.QUERY_DATE = DateTime.Now;
                model.USER_ID = Session[CHubConstValues.SessionUser].ToString();
                decimal logNo = eqLogBLL.Add(model);

                return Json(new RequestResult(logNo));
            }
            catch (Exception ex)
            {
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetWatchingOutList(decimal token)
        {
            try
            {
                ITT_EASY_WATCHING_BLL wBLL = new ITT_EASY_WATCHING_BLL();
                List<ITT_EASY_WATCHING> result = wBLL.GetWatchingList(token);
                foreach (var item in result)
                {
                    item.COLOR = ValueConvert.GetColorFullName(item.COLOR);
                }

                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getWatchingOutList", ex);
                return Json(new RequestResult(false,ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetMDMData(string partNo)
        {
            try
            {
                V_TC_MDM_ALL_BLL mdmBLL = new V_TC_MDM_ALL_BLL();
                V_TC_MDM_ALL model = mdmBLL.GetSpecifyMDM(partNo);
                return Json(new RequestResult(model));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("getMDMData", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetInvSnapShotData(string partNo)
        {
            try
            {
                
                using (CHubEntities db = new CHubEntities())
                {
                    V_INV_PDC_BLL pdcBLL = new V_INV_PDC_BLL(db);
                    List<V_INV_PDC> pdcList = pdcBLL.GetPDCData(partNo);
                    

                    V_INV_RDC_BLL rdcBLL = new V_INV_RDC_BLL(db);
                    List<V_INV_RDC> rdcList = rdcBLL.GetRDCData(partNo);


                    M_INV_BLL miBLL = new M_INV_BLL();
                    List<M_INV> interPDCList = miBLL.GetInterPDCData(partNo);
                    

                    var obj = new
                    {
                        pdcList = pdcList,
                        rdcList = rdcList,
                        interPDCList = interPDCList
                    };

                    return Json(new RequestResult(obj));
                }    
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetSnapShot", ex);
                return Json(new RequestResult(false,ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetOpenSnapShotData(string partNo)
        {
            try
            {
                using (CHubEntities db = new CHubEntities())
                {
                    List<OpeningQtySnapshot> openPDCList = new List<OpeningQtySnapshot>();
                    
                    List<OpeningQtySnapshot> openRDCList = new List<OpeningQtySnapshot>();
                    

                    //PDC part
                    V_OPEN_QTY_SO_PDC_BLL soPDCBLL = new V_OPEN_QTY_SO_PDC_BLL(db);
                    List<V_OPEN_QTY_SO_PDC> soPDCList = soPDCBLL.GetOpenPDCData(partNo);

                    V_OPEN_QTY_PO_PDC_BLL poPDCBLL = new V_OPEN_QTY_PO_PDC_BLL(db);
                    List<V_OPEN_QTY_PO_PDC> poPDCList = poPDCBLL.GetOpenPDCData(partNo);

                    V_OPEN_QTY_ASN_PDC_BLL asnPDCBLL = new V_OPEN_QTY_ASN_PDC_BLL();
                    List<V_OPEN_QTY_ASN_PDC> asnPDCList = asnPDCBLL.GetOpenPDCData(partNo);

                    //do union operation
                    foreach (var item in poPDCList)
                    {
                        if (!soPDCList.Any(a => a.WAREHOUSE == item.WAREHOUSE))
                        {
                            soPDCList.Add(new V_OPEN_QTY_SO_PDC { WAREHOUSE = item.WAREHOUSE,WH_ALIAS=item.WH_ALIAS});
                        }
                    }

                    foreach (var item in soPDCList)
                    {
                        OpeningQtySnapshot openSS = new OpeningQtySnapshot();
                        openSS.WHAlias = item.WH_ALIAS;
                        openSS.BackOrderedQty = item.QTY_BACKORDERED;
                        openSS.OpeningQty = item.QTY_OPENING;
                        openSS.ReservedQty = item.QTY_RESERVED;
                        openSS.InPickingQty = item.QTY_IN_PICKING;

                        V_OPEN_QTY_PO_PDC poData = poPDCList.FirstOrDefault(a => a.WAREHOUSE == item.WAREHOUSE);
                        V_OPEN_QTY_ASN_PDC asnData = asnPDCList.FirstOrDefault(a => a.WAREHOUSE == item.WAREHOUSE);
                        if (poData != null)
                        {
                            openSS.RemainingQty = poData.REMAINING_QTY.Value;
                            openSS.LatestETA = poData.LATEST_ETA.Value.ToString("yyyy-MM-dd");
                            //intransit will have logic later
                            openSS.InTransit = poData.REMAINING_QTY.Value;
                        }
                        if (asnData != null)
                        {
                            //Get the min value
                            openSS.InTransit = openSS.InTransit < asnData.OPEN_QTY ? openSS.InTransit : asnData.OPEN_QTY.Value;
                        }
                        openPDCList.Add(openSS);
                    }
                   

                    //RDC part
                    V_OPEN_QTY_SO_RDC_BLL soRDCBLL = new V_OPEN_QTY_SO_RDC_BLL(db);
                    List<V_OPEN_QTY_SO_RDC> soRDCList = soRDCBLL.GetOpenRDCData(partNo);

                    V_OPEN_QTY_PO_RDC_BLL poRDCBLL = new V_OPEN_QTY_PO_RDC_BLL(db);
                    List<V_OPEN_QTY_PO_RDC> poRDCList = poRDCBLL.GetOpenRDCData(partNo);

                    V_OPEN_QTY_ASN_RDC_BLL asnRDCBLL = new V_OPEN_QTY_ASN_RDC_BLL();
                    List<V_OPEN_QTY_ASN_RDC> asnRDCList = asnRDCBLL.GetOpenRDCData(partNo);

                    //do union operation
                    foreach (var item in poRDCList)
                    {
                        if (!soRDCList.Any(a => a.WAREHOUSE == item.WAREHOUSE))
                        {
                            soRDCList.Add(new V_OPEN_QTY_SO_RDC { WAREHOUSE = item.WAREHOUSE, WH_ALIAS = item.WH_ALIAS });
                        }
                    }

                    foreach (var item in soRDCList)
                    {
                        OpeningQtySnapshot openSS = new OpeningQtySnapshot();
                        openSS.WHAlias = item.WH_ALIAS;
                        openSS.BackOrderedQty = item.QTY_BACKORDERED;
                        openSS.OpeningQty = item.QTY_OPENING;
                        openSS.ReservedQty = item.QTY_RESERVED;
                        openSS.InPickingQty = item.QTY_IN_PICKING;

                        V_OPEN_QTY_PO_RDC poData = poRDCList.FirstOrDefault(a => a.WAREHOUSE == item.WAREHOUSE);
                        V_OPEN_QTY_ASN_RDC asnData = asnRDCList.FirstOrDefault(a => a.WAREHOUSE == item.WAREHOUSE);
                        if (poData != null)
                        {
                            openSS.RemainingQty = poData.REMAINING_QTY.Value;
                            openSS.LatestETA = poData.LATEST_ETA.Value.ToString("yyyy-MM-dd");
                            //intransit will have logic later
                            openSS.InTransit = poData.REMAINING_QTY.Value;
                        }
                        if (asnData != null)
                        {
                            //Get the min value
                            openSS.InTransit = openSS.InTransit < asnData.OPEN_QTY ? openSS.InTransit : asnData.OPEN_QTY.Value;
                        }
                        openRDCList.Add(openSS);
                    }


                    var obj = new
                    {
                        pdcList = openPDCList,
                        rdcList = openRDCList
                    };

                    return Json(new RequestResult(obj));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetOpenSnapShotData", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult GetShipmentData(string partNo)
        {
            try
            {
                V_ITT_SHIPPING_ALLIN1_BLL shipBLL = new V_ITT_SHIPPING_ALLIN1_BLL();
                List<V_ITT_SHIPPING_ALLIN1> result = shipBLL.GetShipmentData(partNo);
                if (result != null)
                {
                    string icoPath = Server.MapPath(CHubConstValues.ChubIcoFolder);
                    foreach (var item in result)
                    {
                        if (item.S10_DISPLAY != null)
                            item.COLOR_10 = ValueConvert.GetColorFullName(item.COLOR_10);
                        if (item.S30_DISPLAY != null)
                            item.COLOR_30 = ValueConvert.GetColorFullName(item.COLOR_30);
                        if (item.S50_DISPLAY != null)
                            item.COLOR_50 = ValueConvert.GetColorFullName(item.COLOR_50);
                    }
                }
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetShipmentData", ex);
                return Json(new RequestResult(false, ex.Message));
            }

            
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetPOLevel1Data(string partNo)
        {
            try
            {
                ITT_PO_BLL poBLL = new ITT_PO_BLL();
                List<ITT_PO_LEVEL_1> result = poBLL.GetLevel1Data(partNo);
                
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPOLevel1Data", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetPOLevel2Data(string partNo,string poNo)
        {
            try
            {
                ITT_PO_BLL poBLL = new ITT_PO_BLL();
                List<ITT_PO_LEVEL_2> result = poBLL.GetLevel2Data(partNo,poNo);

                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPOLevel2Data", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        #region private function part

        private string SaveTranLoadAction(ITT_TRAN_LOAD model)
        {
            try
            {
                ITT_TRAN_LOAD_VALIDATION validation = new ITT_TRAN_LOAD_VALIDATION(model);
                string msg = validation.ValidationAction();
                if (!string.IsNullOrEmpty(msg))
                {
                    return msg;
                }

                model.LOADED_BY = Session[CHubConstValues.SessionUser].ToString();
                model.LOAD_DATE = DateTime.Now;
                ITT_TRAN_LOAD_BLL tranBLL = new ITT_TRAN_LOAD_BLL();
                tranBLL.Save(model);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Fail Saved:{0}", ex.Message);
            }
        }

        public TranLoadPreFill PreFillTranLoadAction(string wayBillNo)
        {
            V_SHIPPING_ALL_BASE_BLL saBLL = new V_SHIPPING_ALL_BASE_BLL();
            V_SHIPPING_ALL_BASE sa = saBLL.GetFirstBaseInfo(wayBillNo);
            TranLoadPreFill result = new TranLoadPreFill();
            if (sa != null)
            {
                result.InvoiceNo = sa.INVOICE_NO;
                result.TranType = sa.TRAN_TYPE;
                result.FromSystem = sa.FROM_SYSTEM;
                result.Msg = "";
                result.BackColor = "";

                //if (!string.IsNullOrEmpty(result.InvoiceNo))
                //{
                //    ITT_TRAN_LOAD_BLL tlBLL = new ITT_TRAN_LOAD_BLL();
                //    if (tlBLL.ExistInvoiceNo(result.InvoiceNo))
                //    {
                //        result.Msg = "Invoice No is exist";
                //        result.BackColor = CHubConstValues.ErrorColor;
                //    }
                //}
            }
            else
            {
                result.Msg = "Can't Find Invoice No from WayBillNo";
                result.BackColor = "";
            }
            return result;
        }


        private string SaveCustLoadAction(ITT_CUST_LOAD model)
        {
            try
            {
                ITT_CUST_LOAD_VALICATION validation = new ITT_CUST_LOAD_VALICATION(model);
                string msg = validation.ValidationAction();
                if (!string.IsNullOrEmpty(msg))
                {
                    return msg;
                }

                if (model.BND_OUT_DATE != null && model.NBND_ARRIVAL_DATE == null)
                {
                    M_CALENDAR_BLL calBLL = new M_CALENDAR_BLL();
                    model.NBND_ARRIVAL_DATE = calBLL.GetArrivalDateFromOutDate(model.BND_OUT_DATE.Value);
                }

                model.LOADED_BY = Session[CHubConstValues.SessionUser].ToString();
                model.LOAD_DATE = DateTime.Now;
                ITT_CUST_LOAD_BLL tranBLL = new ITT_CUST_LOAD_BLL();
                tranBLL.Save(model);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Fail Saved:{0}", ex.Message);
            }
        }

        #endregion



    }
}
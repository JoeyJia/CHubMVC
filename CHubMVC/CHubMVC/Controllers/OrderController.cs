using CHubBLL;
using CHubCommon;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubModel;
using CHubModel.ExtensionModel;
using CHubMVC.Validations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static CHubCommon.CHubEnum;
using CHubMVC.Models;
using System.IO;
using CHubBLL.OtherProcess;

namespace CHubMVC.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        [Authorize]
        public ActionResult Index(string seq)
        {
            //if (Session[CHubConstValues.SessionUser] == null)
            //    //Session[CHubConstValues.SessionUser] = "lg166";// For test using
            //   return RedirectToAction("Login", "Account");

            ViewBag.AppUser = Session[CHubConstValues.SessionUser].ToString();
            ViewBag.seq = seq;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Init()
        {
            //if (Session[CHubConstValues.SessionUser] == null)
            //    return RedirectToAction("Login", "Account");

            using (CHubEntities db = new CHubEntities())
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                //add recent page data
                APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL(db);
                rpBLL.Add(appUser, PageNameEnum.qkord.ToString(), this.Request.Url.AbsoluteUri);

                APP_CUST_ALIAS_BLL acaBLL = new APP_CUST_ALIAS_BLL(db);
                List<ExAppCustAlias> acaList = acaBLL.GetAppCustAliasByAppUser(appUser);

                APP_ORDER_TYPE_BLL aotBLL = new APP_ORDER_TYPE_BLL(db);
                List<APP_ORDER_TYPE> aotList = aotBLL.GetValidOrderType();
                foreach (var item in aotList)
                {
                    item.TS_OR_HEADER = null;
                }

                var obj = new
                {
                    custAlias = acaList,
                    orderType = aotList,
                    defaultOrderType = CHubConstValues.DefaultOrderType
                };

                var res = JsonConvert.SerializeObject(obj);

                return Json(obj);
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult InitOrder(decimal orderSeq)
        {
            //if (Session[CHubConstValues.SessionUser] == null)
            //    return RedirectToAction("Login", "Account");

            using (CHubEntities db = new CHubEntities())
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                ExVAliasAddr priAddr = null;
                ExVAliasAddr AltAddr = null;
                List<OrderLineItem> olReslut = new List<OrderLineItem>();
                bool isSaved = false;
                bool splInd = true;
                string poNum = "";
                string dueDate = "";
                string note = "";

                V_ALIAS_ADDR_DFLT_BLL dfltBLL = new V_ALIAS_ADDR_DFLT_BLL(db);
                V_ALIAS_ADDR_SPL_BLL splBLL = new V_ALIAS_ADDR_SPL_BLL(db);

                TS_OR_HEADER_BLL hBLL = new TS_OR_HEADER_BLL(db);
                List<TS_OR_HEADER> hList = hBLL.GetHeadersBySeq(orderSeq);
                if (hList != null && hList.Count > 0)
                {
                    isSaved = true;
                    //Get primary addr and Alt addr
                    foreach (var item in hList)
                    {
                        //special ship
                        if (item.SPL_IND == CHubConstValues.IndY)
                        {
                            splInd = true;
                            if (item.SHIPFROM_SEQ == 0)
                            {
                                poNum = item.CUSTOMER_PO_NO;
                                dueDate = item.DUE_DATE.ToString("yyyy-MM-dd");
                                note = item.ORDER_NOTES;
                                priAddr = splBLL.GetSpecifyAliasAddrSPL(item.ALIAS_NAME, item.TO_SYSTEM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION, item.DEST_LOCATION);
                            }
                            if (item.SHIPFROM_SEQ == 1)
                                AltAddr = splBLL.GetSpecifyAliasAddrSPL(item.ALIAS_NAME, item.TO_SYSTEM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION, item.DEST_LOCATION);
                        }
                        else
                        {
                            splInd = false;
                            if (item.SHIPFROM_SEQ == 0)
                            {
                                poNum = item.CUSTOMER_PO_NO;
                                dueDate = item.DUE_DATE.ToString("yyyy-MM-dd");
                                note = item.ORDER_NOTES;
                                priAddr = dfltBLL.GetSpecifyAliasAddrDFLT(item.ALIAS_NAME, item.TO_SYSTEM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION);
                            }
                            if (item.SHIPFROM_SEQ == 1)
                                AltAddr = dfltBLL.GetSpecifyAliasAddrDFLT(item.ALIAS_NAME, item.TO_SYSTEM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION);
                        }
                    }

                    TS_OR_DETAIL_BLL dBLL = new TS_OR_DETAIL_BLL(db);
                    List<TS_OR_DETAIL> dList = dBLL.GetDetailsBySeq(orderSeq);

                    //change detail ot orderLine
                    OrderLineCheckArg arg = new OrderLineCheckArg();
                    arg.primarySysID = priAddr.SysID;
                    arg.primaryWareHouse = priAddr.WareHouse;
                    arg.customerNo = priAddr.CustomerNo;
                    if (AltAddr != null)
                    {
                        arg.altSysID = AltAddr.SysID;
                        arg.altWareHosue = AltAddr.WareHouse;
                    }
                    foreach (var item in dList)
                    {
                        arg.olItem = new OrderLineItem();
                        arg.olItem.CustomerPartNo = item.CUSTOMER_PART_NO;
                        arg.olItem.Qty = item.BUY_QTY;
                        arg.olItem.OrderLineNo = item.ORDER_LINE_NO;
                        CheckOrderLineItemAction(arg);
                        olReslut.Add(arg.olItem);
                    }
                }
                else
                {
                    TS_OR_HEADER_STAGE_BLL hsBLL = new TS_OR_HEADER_STAGE_BLL(db);
                    List<TS_OR_HEADER_STAGE> hsList = hsBLL.GetHeaderStageBySeq(orderSeq);

                    if (hsList != null && hsList.Count > 0)
                    {
                        isSaved = false;
                        //Get primary addr and Alt addr stage
                        foreach (var item in hsList)
                        {
                            //special ship
                            if (item.SPL_IND == CHubConstValues.IndY)
                            {
                                splInd = true;
                                if (item.SHIPFROM_SEQ == 0)
                                {
                                    poNum = item.CUSTOMER_PO_NO;
                                    dueDate = item.DUE_DATE.ToString("yyyy-MM-dd");
                                    note = item.ORDER_NOTES;
                                    priAddr = splBLL.GetSpecifyAliasAddrSPL(item.ALIAS_NAME, item.TO_SYSTEM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION, item.DEST_LOCATION);
                                }
                            }
                            else
                            {
                                splInd = false;
                                if (item.SHIPFROM_SEQ == 0)
                                {
                                    poNum = item.CUSTOMER_PO_NO;
                                    dueDate = item.DUE_DATE.ToString("yyyy-MM-dd");
                                    note = item.ORDER_NOTES;
                                    priAddr = dfltBLL.GetSpecifyAliasAddrDFLT(item.ALIAS_NAME, item.TO_SYSTEM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION);
                                }
                            }
                        }


                        TS_OR_DETAIL_STAGE_BLL dsBLL = new TS_OR_DETAIL_STAGE_BLL(db);
                        List<TS_OR_DETAIL_STAGE> dsList = dsBLL.GetDetailsStageByOrderSeq(orderSeq);

                        //change detail ot orderLine
                        OrderLineCheckArg arg = new OrderLineCheckArg();
                        arg.primarySysID = priAddr.SysID;
                        arg.primaryWareHouse = priAddr.WareHouse;
                        arg.customerNo = priAddr.CustomerNo;
                        arg.altSysID = null;
                        arg.altWareHosue = null;

                        foreach (var item in dsList)
                        {
                            arg.olItem = new OrderLineItem();
                            arg.olItem.CustomerPartNo = item.CUSTOMER_PART_NO;
                            arg.olItem.Qty = item.BUY_QTY;
                            arg.olItem.OrderLineNo = item.ORDER_LINE_NO;
                            CheckOrderLineItemAction(arg);
                            olReslut.Add(arg.olItem);
                        }
                    }
                    else
                    {
                        this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Content("Wrong Order Seq");
                    }
                }

                var obj = new
                {
                    priAddr = priAddr,
                    altAddr = AltAddr,
                    orderLines = olReslut,
                    isSaved = isSaved,
                    splInd = splInd,
                    poNum = poNum,
                    dueDate = dueDate,
                    note = note
                };

                return Json(obj);
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchAddrs(string shipName, string addr, string aliasName, bool isSpecialShip, string destName, long? destLocation)
        {
            try
            {
                List<ExVAliasAddr> list = new List<ExVAliasAddr>();
                CHubEntities db = new CHubEntities();
                if (isSpecialShip)
                {
                    V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                    list = bll.GetAliasAddrSPL(shipName.Trim(), addr.Trim(), destName.Trim(), destLocation, aliasName);
                }
                else
                {
                    V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                    list = bll.GetAliasAddrDFLT(aliasName);
                }

                //Get from parameter table***
                if (list.Count > 30)
                {
                    return Json(new RequestResult(false, string.Format("Result has {0} items, Make Condition more strict", list.Count.ToString())));
                }
                if (list.Count == 0)
                {
                    return Json(new RequestResult(false, "No result"));
                }

                return Json(new RequestResult(list));
            }
            catch (Exception ee)
            {
                return Json(new RequestResult(false, ee.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetStrictAddrs(string shipName, string addr, string aliasName, bool isSpecialShip)
        {
            try
            {
                List<ExVAliasAddr> list = new List<ExVAliasAddr>();
                CHubEntities db = new CHubEntities();
                if (isSpecialShip)
                {
                    V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                    list = bll.GetStrictAliasAddrSPL(shipName.Trim(), addr.Trim(), aliasName);
                }
                else
                {
                    V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                    list = bll.GetStrictAliasAddrDFLT(shipName.Trim(), addr.Trim(), aliasName);
                }
                return Json(list);
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ee.Message);
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult SaveDraft(OrderSaveArg arg)
        {
            try
            {
                if (arg.headInfo == null)
                    return Content("fail");

                string appUser = Session[CHubConstValues.SessionUser].ToString();

                CHubEntities db = new CHubEntities();
                TS_OR_HEADER_STAGE_BLL bll = new TS_OR_HEADER_STAGE_BLL(db);

                //Header part
                TS_OR_HEADER_STAGE orHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.headInfo, arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, arg.isSpecialShip, appUser);
                TS_OR_HEADER_STAGE altORHeaderStage = null;
                if (arg.altHeadInfo != null)
                {
                    altORHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.altHeadInfo, arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, arg.isSpecialShip, appUser, true);
                }

                //Detail Part
                List<TS_OR_DETAIL_STAGE> dStageList = null;
                if (arg.olList != null && arg.olList.Count > 0)
                {
                    dStageList = new List<TS_OR_DETAIL_STAGE>();
                    foreach (var item in arg.olList)
                    {
                        //ignore wrong lines
                        if (string.IsNullOrEmpty(item.PartNo) || string.IsNullOrEmpty(item.PriAVLCheckColor))
                            continue;
                        TS_OR_DETAIL_STAGE dStage = ManualClassConvert.ConvertOLItem2DetailStage(item, arg.seq, appUser);
                        dStageList.Add(dStage);
                    }
                }

                decimal seq = 0;
                if (string.IsNullOrEmpty(arg.seq))
                    seq = bll.AddHeadersWithDetailsStage(orHeaderStage, altORHeaderStage, dStageList, arg.dueDate);
                else
                    seq = bll.UpdateHeadersWithDetailsStage(orHeaderStage, altORHeaderStage, dStageList, arg.dueDate);

                if (seq != 0.00M)
                    return Content(seq.ToString());
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("Fail to save draft");
                }
            }
            catch (Exception ee)
            {
                //log ee
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ee.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveOrder(OrderSaveArg arg)
        {
            try
            {
                if (arg.headInfo == null)
                    return Content("fail");

                string appUser = Session[CHubConstValues.SessionUser].ToString();

                CHubEntities db = new CHubEntities();
                TS_OR_HEADER_BLL bll = new TS_OR_HEADER_BLL(db);

                //Header part
                TS_OR_HEADER orHeader = ManualClassConvert.ConvertExAliaAddr2Header(arg.headInfo, arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, arg.isSpecialShip, appUser);
                TS_OR_HEADER altORHeader = null;
                if (arg.altHeadInfo != null)
                {
                    altORHeader = ManualClassConvert.ConvertExAliaAddr2Header(arg.altHeadInfo, arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, arg.isSpecialShip, appUser, true);
                }

                //Detail part
                List<TS_OR_DETAIL> detailList = null;
                if (arg.olList != null && arg.olList.Count > 0)
                {
                    detailList = new List<TS_OR_DETAIL>();
                    foreach (var item in arg.olList)
                    {
                        //ignore wrong lines
                        if (string.IsNullOrEmpty(item.PartNo) || string.IsNullOrEmpty(item.PriAVLCheckColor))
                            continue;
                        TS_OR_DETAIL detail = ManualClassConvert.ConvertOLItem2Detail(item, arg.seq, appUser);
                        detailList.Add(detail);
                    }
                }

                decimal seq = 0;
                if (string.IsNullOrEmpty(arg.seq))
                    seq = bll.AddHeadersWithDetails(orHeader, altORHeader, detailList, arg.dueDate);
                else
                    seq = bll.UpdateHeadersWithDetails(orHeader, altORHeader, detailList, arg.dueDate);

                if (seq != 0.00M)
                    return Content(seq.ToString());
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("Fail to save order");
                }
            }
            catch (Exception ee)
            {
                //log ee
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ee.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CheckOrderLineItem(OrderLineCheckArg olArg)
        {
            try
            {
                string msg = CheckOrderLineItemAction(olArg);
                if (string.IsNullOrEmpty(msg))
                    return Json(olArg.olItem);
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content(msg);
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ee.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult BatchCheckOrderLines(OrderLineBatchCheckArg arg)
        {
            try
            {
                List<OrderLineItem> olList = new List<OrderLineItem>();

                OrderLineCheckArg olArg = new OrderLineCheckArg
                {
                    primarySysID = arg.primarySysID,
                    primaryWareHouse = arg.primaryWareHouse,
                    altSysID = arg.altSysID,
                    altWareHosue = arg.altWareHosue,
                    customerNo = arg.customerNo
                };
                foreach (var item in arg.olItemList)
                {
                    olArg.olItem = item;
                    CheckOrderLineItemAction(olArg);
                    olList.Add(olArg.olItem);
                }
                return Json(olList);
            }
            catch (Exception ex)
            {
                //log
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ex.Message);
            }
        }

        [Authorize]
        public ActionResult DownLoadOrder(decimal? orderSeq, decimal shipFrom = 0)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (orderSeq == null || orderSeq == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("No order seq data");
                }

                CHubEntities db = new CHubEntities();
                V_O_DOWNLOAD_HDR_BLL hBLL = new V_O_DOWNLOAD_HDR_BLL(db);
                V_O_DOWNLOAD_HDR hData = hBLL.GetSpecfyHDRData(orderSeq.Value, shipFrom);
                if (hData == null)
                    throw new Exception("No Header Data");

                V_O_DOWNLOAD_DTL_BLL dBLL = new V_O_DOWNLOAD_DTL_BLL(db);
                List<V_O_DOWNLOAD_DTL> dList = dBLL.GetDTLList(orderSeq.Value, shipFrom);

                if (dList == null || dList.Count == 0)
                    throw new Exception("No Lines Data");


                sb.AppendLine(hData.TXT);
                foreach (var item in dList)
                {
                    sb.AppendLine(item.TXT);
                }

                byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

                return File(buffer, "text/csv", hData.FILE_NAME);
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        #region For draft function part
        [Authorize]
        public ActionResult Draft()
        {
            //if (Session[CHubConstValues.SessionUser] == null)
            //    return RedirectToAction("Login", "Account");

            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, PageNameEnum.orddft.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [Authorize]
        public ActionResult InitDraft()
        {
            //if (Session[CHubConstValues.SessionUser] == null)
            //    return RedirectToAction("Login", "Account");

            string appUser = Session[CHubConstValues.SessionUser].ToString();

            CHubEntities db = new CHubEntities();
            TS_OR_HEADER_STAGE_BLL hStageBLL = new TS_OR_HEADER_STAGE_BLL();
            List<TS_OR_HEADER_STAGE> hStageList = hStageBLL.GetHeaderStageByUser(appUser);

            return Json(hStageList);
        }


        [HttpPost]
        [Authorize]
        public ActionResult DeleteDraft(decimal orderSeq, decimal shipFrom)
        {
            try
            {
                CHubEntities db = new CHubEntities();
                TS_OR_HEADER_STAGE_BLL hStageBLL = new TS_OR_HEADER_STAGE_BLL();
                hStageBLL.DeleteDraft(orderSeq, shipFrom);

                return Content("Success");
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ee.Message);
            }
        }

        #endregion

        #region  For Order List View

        [Authorize]
        public ActionResult Query()
        {
            //if (Session[CHubConstValues.SessionUser] == null)
            //    return RedirectToAction("Login", "Account");

            //add recent history
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, PageNameEnum.ordinq.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult QueryAction(decimal? orderSeq, string custAlias, string poNum, int currentPage, int pageSize)
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            int totalCount = 0;
            CHubEntities db = new CHubEntities();
            TS_OR_HEADER_BLL hBLL = new TS_OR_HEADER_BLL(db);
            List<TS_OR_HEADER> result = hBLL.GetHeaders(orderSeq, custAlias, poNum, currentPage, pageSize, out totalCount);

            var obj = new
            {
                result = result,
                totalCount = totalCount
            };
            return Json(obj);
        }

        #endregion


        [HttpGet]
        [Authorize]
        public ActionResult OrdInq()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, PageNameEnum.ordinq.ToString(), this.Request.Url.AbsoluteUri);
            OrdInqModels vm = new OrdInqModels();
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult OrdInq(OrdInqModels vm)
        {
            ORDINQ_BLL oiBLL = new ORDINQ_BLL();
            try
            {
                var result = oiBLL.GetORDER_HList(vm.CUSTOMER_PO_NO, vm.ORDER_NO);
                vm.titleList = result.Select(i => i.TITLE).ToList();
                vm.OrderList = new List<OrderList>();
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        OrderList oList = new OrderList();
                        oList.Order_H = item;
                        var ship_location = oiBLL.GetSHIPPING_LOCAL(item.LOAD_FROM, item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.SHIP_TO_LOCATION, item.DEST_LOCATION).FirstOrDefault();
                        if (ship_location != null)
                        {
                            oList.LOCAL_SHIP_TO_NAME = ship_location.LOCAL_SHIP_TO_NAME;
                            oList.LOCAL_SHIP_TO_ADDR_1 = ship_location.LOCAL_SHIP_TO_ADDR_1;
                            oList.LOCAL_SHIP_TO_ADDR_2 = ship_location.LOCAL_SHIP_TO_ADDR_2;
                            oList.LOCAL_SHIP_TO_ADDR_3 = ship_location.LOCAL_SHIP_TO_ADDR_3;
                            oList.LOCAL_SHIP_TO_CITY = ship_location.LOCAL_SHIP_TO_CITY;
                        }
                        oList.Order_DList = oiBLL.GetORDER_DList(item.LOAD_FROM, item.ORDER_NO);
                        vm.OrderList.Add(oList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ORDER ORDINQ", ex);
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(vm);
        }


        [Authorize]
        [HttpPost]
        public ActionResult PrintIhubOA(string LOAD_FROM,string ORDER_NO)
        {
            ORDINQ_BLL oBLL = new ORDINQ_BLL();
            try
            {
                var appUser = Session[CHubConstValues.SessionUser].ToString();
                //权限检测
                if (oBLL.CheckPrintSecurity("PRINT_IHUB_OA", appUser))
                {
                    //生成pdf
                    var header = oBLL.SearchV_OA_H_PRINT(LOAD_FROM, ORDER_NO);
                    if (header != null)
                    {
                        //根据OA_TYPE获取表头
                        var title = oBLL.SearchOA_TYPE_MST(header.OA_TYPE);
                        //获取明细
                        var details = oBLL.SearchV_OA_D_PRINT(header.LOAD_FROM, header.ORDER_NO);
                        
                        string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                        IHUBOAPrintBLL printBLL = new IHUBOAPrintBLL(basePath);
                        string fileName = printBLL.BuildIhubOAPrintFile(title,header,details);
                        string webPath = "/temp/" + fileName;
                        return Json(new RequestResult(webPath));
                    }
                    return Json(new RequestResult(false, "No Data"));
                }
                else
                    return Json(new RequestResult(false, "You cannot Operate!"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ORDER PrintIhubOA", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult GetOrder_R(string LOAD_FROM, string ORDER_NO, string LINE_NO)
        {
            ORDINQ_BLL oiBLL = new ORDINQ_BLL();
            try
            {
                var result = oiBLL.GetORDER_RList(LOAD_FROM, ORDER_NO, LINE_NO);
                var mainHtml = GetOrder_RHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ORDER GetOrder_R", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTrackList(string LOAD_FROM, string ORDER_NO)
        {
            ORDINQ_BLL oiBLL = new ORDINQ_BLL();
            try
            {
                var result = oiBLL.GetTrackList(LOAD_FROM, ORDER_NO);
                var mainHtml = GetTrackHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ORDER GetTrackList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public string GetOrder_RHtml(List<V_GOMS_ORDER_R> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    string fontColor = string.Empty;
                    switch (item.COLOR)
                    {
                        case "G":
                            fontColor = "green";
                            break;
                        case "Y":
                            fontColor = "orange";
                            break;
                        case "R":
                            fontColor = "red";
                            break;
                        case "B":
                            fontColor = "blue";
                            break;
                        default:
                            fontColor = "";
                            break;
                    }

                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.REL_NO).Append("</td>");
                    sb.Append("     <td style='color:" + fontColor + "'>").Append(item.STATUS_DESC).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.REVISED_QTY_DUE).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_RESERVED).Append("</td>");
                    sb.Append("     <td>").Append(item.QTY_PICKED).Append("</td>");
                    sb.Append("     <td style='color:" + fontColor + "'>").Append(item.QTY_SHIPPED).Append("</td>");
                    sb.Append("     <td>").Append(item.PROMISE_DATE.HasValue ? item.PROMISE_DATE.Value.ToString("yyyy/MM/dd") : "").Append("</td>");
                    sb.Append("     <td>").Append(item.PROMISE_MSG).Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }
        public string GetTrackHtml(List<V_SHIP_TRACK_PRO> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count() > 0)
            {
                var num = 0;
                foreach (var item in list)
                {
                    num++;
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(num).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.GPS_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.CARRIER_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.CARRIER_PRO_NO).Append("</td>");
                    if (!string.IsNullOrEmpty(item.CARRIER_PRO_NO))
                        sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-sm btnViewTrack' data-track_num='" + item.CARRIER_PRO_NO + "' value='查看物流' />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }


        #region *** private function ***
        private string GetPartNoFromCustPartNo(string custPartNo, string customerNo)
        {
            if (string.IsNullOrEmpty(custPartNo))
                return string.Empty;
            using (CHubEntities db = new CHubEntities())
            {
                G_CATALOG_CUSTOMER_PART_BLL custBLL = new G_CATALOG_CUSTOMER_PART_BLL(db);
                string PartNo = custBLL.GetPartNoFromCustPartNo(custPartNo, customerNo);
                if (string.IsNullOrEmpty(PartNo))
                {
                    G_PART_DESCRIPTION_BLL partBLL = new G_PART_DESCRIPTION_BLL(db);
                    if (partBLL.IsPartNoExist(custPartNo))
                        return custPartNo;
                    else
                    {
                        return string.Empty;
                    }

                }
                return PartNo;
            }
        }

        public string CheckOrderLineItemAction(OrderLineCheckArg olArg)
        {
            //reset 
            olArg.olItem.PartNo = string.Empty;
            olArg.olItem.AltAVLCheckColor = string.Empty;
            olArg.olItem.PriAVLCheckColor = string.Empty;
            olArg.olItem.PartNoPlaceHolder = string.Empty;
            olArg.olItem.PriAVLCheck = null;
            olArg.olItem.AltAVLCheck = null;
            olArg.olItem.DescCN = string.Empty;
            olArg.olItem.Description = string.Empty;
            olArg.olItem.ItemBackColor = string.Empty;
            olArg.olItem.WarningMsg = string.Empty;
            olArg.olItem.WarningColor = string.Empty;

            olArg.olItem.LastCheckNo = olArg.olItem.CustomerPartNo;
            olArg.olItem.LastQty = olArg.olItem.Qty;

            string msg = string.Empty;

            olArg.olItem.PartNo = GetPartNoFromCustPartNo(olArg.olItem.CustomerPartNo, olArg.customerNo);


            olArg.olItem.PartNoPlaceHolder = string.Empty;
            if (string.IsNullOrEmpty(olArg.olItem.PartNo))
            {
                olArg.olItem.PartNoPlaceHolder = "Can't find Part NO";
                olArg.olItem.ItemBackColor = CHubConstValues.ErrorColor;
                return null;
            }
            else
            {
                CHubEntities db = new CHubEntities();
                G_PART_DESCRIPTION_BLL pDescBLL = new G_PART_DESCRIPTION_BLL(db);

                //Get description
                G_PART_DESCRIPTION pDesc = pDescBLL.GetPartDescription(olArg.olItem.PartNo);
                olArg.olItem.Description = pDesc.DESCRIPTION;
                olArg.olItem.DescCN = pDesc.DESC_CN;

                //check inactive status
                if (pDesc.PART_STATUS == PartStatusEnum.I.ToString())
                {
                    olArg.olItem.WarningMsg = string.Format("SSC:{0},", pDesc.CURRENT_SALES_STATUS_CODE);
                    //olArg.olItem.WarningColor = CHubConstValues.WarningColor;

                }

                string usingSysID = olArg.primarySysID;
                //Do AVL check
                if (olArg.olItem.Qty > 0)
                {
                    if (string.IsNullOrEmpty(olArg.primarySysID) || string.IsNullOrEmpty(olArg.primaryWareHouse))
                    {
                        msg = "No Primary SysID and WareHouse information";
                    }


                    G_NETAVL_BLL netBLL = new G_NETAVL_BLL(db);
                    //Primary AVL check
                    decimal priNet = netBLL.GetSpecifyNETAVL(olArg.primarySysID, olArg.olItem.PartNo, olArg.primaryWareHouse);
                    if (priNet == 0)
                        olArg.olItem.PriAVLCheckColor = CHubConstValues.NoStockColor;
                    else if (priNet >= olArg.olItem.Qty)
                        olArg.olItem.PriAVLCheckColor = CHubConstValues.SatisfyStockColor;
                    else
                        olArg.olItem.PriAVLCheckColor = CHubConstValues.PartialStockColor;
                    olArg.olItem.PriAVLCheck = priNet;

                    //if primary is no enough do  Alt AVL check
                    if (priNet < olArg.olItem.Qty)
                    {
                        if (!(string.IsNullOrEmpty(olArg.altSysID) || string.IsNullOrEmpty(olArg.altWareHosue)))
                        {
                            decimal altNet = netBLL.GetSpecifyNETAVL(olArg.altSysID, olArg.olItem.PartNo, olArg.altWareHosue);
                            if (altNet == 0)
                                olArg.olItem.AltAVLCheckColor = CHubConstValues.NoStockColor;
                            else if (altNet >= olArg.olItem.Qty)
                            {
                                olArg.olItem.AltAVLCheckColor = CHubConstValues.SatisfyStockColor;
                                usingSysID = olArg.altSysID;
                            }
                            else
                                olArg.olItem.AltAVLCheckColor = CHubConstValues.PartialStockColor;
                            olArg.olItem.AltAVLCheck = altNet;
                        }
                    }
                }

                //Do G_OESALES_CATALOG_VALIDATION
                G_OESALES_CATALOG_VALIDATION oeSaleValidation = new G_OESALES_CATALOG_VALIDATION(usingSysID, olArg.olItem.PartNo, olArg.olItem.Qty);
                olArg.olItem.WarningMsg += oeSaleValidation.ValidationAction();
                if (!string.IsNullOrEmpty(olArg.olItem.WarningMsg))
                    olArg.olItem.WarningColor = CHubConstValues.WarningColor;
            }

            return msg;
        }

        #endregion

        //string appUser = Session[CHubConstValues.SessionUser].ToString();
        //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
        //rpBLL.Add(appUser, CHubEnum.PageNameEnum.wbprt.ToString(), this.Request.Url.AbsoluteUri);

        [Authorize]
        public ActionResult XCECWB()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.xcecwb.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="CUST_ORDER_NO"></param>
        /// <param name="CUST_NAME"></param>
        /// <param name="CREATE_DATE"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SearchXcecWB(string CUST_ORDER_NO, string CUST_NAME, string CREATE_DATE, string PROCESS_STATUS, int PageIndex, int PageSize)
        {
            V_XCEC_ORDER_HDR_BASE_BLL bll = new V_XCEC_ORDER_HDR_BASE_BLL();
            try
            {
                string comp = string.Empty;
                var result = bll.SearchXcecWB(CUST_ORDER_NO, CUST_NAME, CREATE_DATE, PROCESS_STATUS);
                if (result.Count() <= PageSize * PageIndex)
                    comp = "complete";
                //分页
                result = result.OrderByDescending(r => r.CREATE_DATE).Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
                var mainHtml = GetXcecWBHtml(result);
                return Json(new RequestResult(true, comp, mainHtml));
                //if (!string.IsNullOrEmpty(mainHtml))
                //    return Json(new RequestResult(true, comp, mainHtml));
                //else
                //    return Json(new RequestResult(true, comp, "Empty"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order SearchXcecWB", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Details
        /// </summary>
        /// <param name="WAREHOUSE"></param>
        /// <param name="IHUB_ORDER_NO"></param>
        /// <param name="CUST_ORDER_NO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetXcecWBDetails(string WAREHOUSE, string IHUB_ORDER_NO, string CUST_ORDER_NO)
        {
            V_XCEC_ORDER_HDR_BASE_BLL bll = new V_XCEC_ORDER_HDR_BASE_BLL();
            string headerHtml = string.Empty;
            string linesHtml = string.Empty;

            try
            {
                #region order header
                var Headerresult = bll.SearchXcecWBDetail(WAREHOUSE, IHUB_ORDER_NO).First();
                headerHtml = GetXcecWBDetailHtml(Headerresult);
                #endregion

                #region order lines
                var Linesresult = bll.GetLinesDetail(CUST_ORDER_NO);
                linesHtml = GetLinesDetailHtml(Linesresult);
                #endregion

                return Json(new RequestResult(true, headerHtml, linesHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetXcecWBDetails", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// ReProcess
        /// </summary>
        /// <param name="WAREHOUSE"></param>
        /// <param name="IHUB_ORDER_NO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReProcessXcecWB(string WAREHOUSE, string IHUB_ORDER_NO)
        {
            V_XCEC_ORDER_HDR_BASE_BLL bll = new V_XCEC_ORDER_HDR_BASE_BLL();
            try
            {
                //找到对应数据
                var result = bll.SearchXcecWBDetail(WAREHOUSE, IHUB_ORDER_NO).First();
                //将PROCESS_STATUS E/Q -> Q
                if (result.PROCESS_STATUS == "E")
                    bll.UpdateProcessStatus(result);
                //执行Proc
                bll.ExecProc();
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order ReProcessXcecWB", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Xcec_Addr_All
        /// </summary>
        /// <param name="WAREHOUSE"></param>
        /// <param name="ADDR_NAME"></param>
        /// <param name="ADDR_1"></param>
        /// <param name="ADDR_2"></param>
        /// <param name="ADDR_3"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchXcecAddrAll(string WAREHOUSE, string ADDR_NAME, string ADDR_1, string ADDR_2, string ADDR_3)
        {
            V_XCEC_ADDR_ALL_BLL bll = new V_XCEC_ADDR_ALL_BLL();
            try
            {
                var result = bll.SearchXcecAddrAll(WAREHOUSE, ADDR_NAME, ADDR_1, ADDR_2, ADDR_3);
                if (result.Count() > 50)
                    return Json(new RequestResult(false, "Result more than 50,Please make condition more strict!"));

                var mainHtml = GetXcecAddrAll(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order SearchXcecAddrAll", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Confirm to Match
        /// </summary>
        /// <param name="WAREHOUSE"></param>
        /// <param name="DEST_LOCATION"></param>
        /// <param name="XCEC_ADDR_SEQ"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmToMatch(string WAREHOUSE, string DEST_LOCATION, string XCEC_ADDR_SEQ)
        {
            V_XCEC_ADDR_ALL_BLL bll = new V_XCEC_ADDR_ALL_BLL();
            try
            {
                string User = Session[CHubConstValues.SessionUser].ToString();
                //权限
                if (!bll.SecureCheck(User))
                    return Json(new RequestResult(false, "You are not the Signer"));

                //查询出选中的数据
                var result = bll.GetXcecAddrAll(WAREHOUSE, DEST_LOCATION).First();

                //更新XCEC_INT.XCEC_ADDR_MST（KEY: XCEC_ADDR_SEQ)
                bll.UpdateXcecAddrAll(result, XCEC_ADDR_SEQ, User);

                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order ConfirmToMatch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Header Html
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetXcecWBHtml(List<V_XCEC_ORDER_HDR_BASE> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    //style="background-color:blue;"
                    sb.Append("     <tr style=\"background-color:" + GetTrColor(item.PROCESS_STATUS) + ";\">");
                    sb.Append("         <td title=\"" + item.PROCESS_STATUS + "\">").Append(item.PROCESS_STATUS).Append("</td>");
                    sb.Append("         <td title=\"" + item.SHIP_WH + "\">").Append(item.SHIP_WH).Append("</td>");
                    sb.Append("         <td title=\"" + item.CUST_ORDER_NO + "\">").Append(item.CUST_ORDER_NO).Append("</td>");
                    sb.Append("         <td title=\"" + item.CUST_NAME + "\">").Append(item.CUST_NAME).Append("</td>");
                    sb.Append("         <td title=\"" + item.ORDER_TYPE + "\">").Append(item.ORDER_TYPE).Append("</td>");
                    sb.Append("         <td title=\"" + item.DUE_DATE.ToString("yyyy-MM-dd") + "\">").Append(item.DUE_DATE.ToString("yyyy-MM-dd")).Append("</td>");
                    sb.Append("         <td title=\"" + item.KITS_NO + "\">").Append(item.KITS_NO).Append("</td>");
                    sb.Append("         <td title=\"" + item.DEALER_PO_NO + "\">").Append(item.DEALER_PO_NO).Append("</td>");
                    sb.Append("         <td title=\"" + item.NOTE + "\">").Append(item.NOTE).Append("</td>");
                    sb.Append("         <td title=\"" + item.IHUB_ORDER_NO + "\">").Append(item.IHUB_ORDER_NO).Append("</td>");
                    sb.Append("         <td title=\"" + item.CREATE_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\">").Append(item.CREATE_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss")).Append("</td>");
                    //button: PROCESS_STATUS E/Q REPROCESS 可用；C DOWNLOAD 可用
                    string reprocess = string.Empty; string download = string.Empty;
                    if (!(item.PROCESS_STATUS == "E" || item.PROCESS_STATUS == "Q"))
                        reprocess = "disabled=\"disabled\"";
                    if (!(item.PROCESS_STATUS == "C"))
                        download = "disabled=\"disabled\"";
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm DetailBtn\" value=\"更多..\" data-warehouse=\"" + item.WAREHOUSE + "\" data-ihuborderno=\"" + item.IHUB_ORDER_NO + "\" data-custorderno=\"" + item.CUST_ORDER_NO + "\" data-status=\"" + item.PROCESS_STATUS + "\" />")
                                              .Append("&nbsp;&nbsp;")
                                              .Append("<input type=\"button\" class=\"btn btn-primary btn-sm DownloadBtn\" value=\"下载\" data-orderseqno=\"" + item.ORDER_SEQ_NO + "\" " + download + " />")
                                              .Append("&nbsp;&nbsp;")
                                              .Append("<input type=\"button\" class=\"btn btn-primary btn-sm ReProcessBtn\" value=\"处理\" data-warehouse=\"" + item.WAREHOUSE + "\" data-ihuborderno=\"" + item.IHUB_ORDER_NO + "\" " + reprocess + " />")
                      .Append("         </td>");
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Header Detail Html
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetXcecWBDetailHtml(V_XCEC_ORDER_HDR_BASE result)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("     <tr>");
            sb.Append("         <td style=\"background-color:grey\">").Append("Customer PO:").Append("</td>");
            sb.Append("         <td>").Append(result.CUST_ORDER_NO).Append("</td>");
            sb.Append("         <td style=\"background-color:grey\">").Append("Order TYPE:").Append("</td>");
            sb.Append("         <td>").Append(result.ORDER_TYPE).Append("</td>");
            sb.Append("         <td style=\"background-color:grey\">").Append("DUE DATE:").Append("</td>");
            sb.Append("         <td>").Append(result.DUE_DATE.ToString("yyyy-MM-dd")).Append("</td>");
            sb.Append("         <td style=\"background-color:grey\">").Append("KITS NO:").Append("</td>");
            sb.Append("         <td>").Append(result.KITS_NO).Append("</td>");
            sb.Append("         <td style=\"background-color:grey\">").Append("SHIP FROM:").Append("</td>");
            sb.Append("         <td>").Append(result.SHIP_WH).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"background-color:grey\">").Append("CUSTOMER NAME:").Append("</td>");
            sb.Append("         <td colspan=\"9\">").Append(result.CUST_NAME).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"background-color:grey\">").Append("ADDRESS:").Append("</td>");
            sb.Append("         <td colspan=\"9\">").Append(result.XCEC_ADDR).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"background-color:grey\">").Append("GOMS ADDR:").Append("</td>");
            sb.Append("         <td colspan=\"9\">").Append(result.GOMS_ADDR_MATCHED).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td style=\"background-color:grey\">").Append("ERROR MSG:").Append("</td>");
            if (!string.IsNullOrEmpty(result.LAST_ERROR_MSG))
                sb.Append("         <td colspan=\"9\" style=\"background-color:#ff6666\">").Append(result.LAST_ERROR_MSG).Append("</td>");
            else
                sb.Append("         <td colspan=\"9\">").Append(result.LAST_ERROR_MSG).Append("</td>");
            sb.Append("     </tr>");
            sb.Append("     <tr>");
            sb.Append("         <td  colspan=\"10\" style=\"text-align:right;\">").Append("<input type=\"button\" class=\"btn btn-primary btn-sm\" id=\"RematchBtn\" data-warehouse=\"" + result.WAREHOUSE + "\" data-xcecaddrseq=\"" + result.XCEC_ADDR_SEQ + "\" value=\"GOMS ADDR REMATCH\" />").Append("</td>");
            sb.Append("     </tr>");

            return sb.ToString();
        }

        /// <summary>
        /// Lines Detai Html
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetLinesDetailHtml(List<V_XCEC_ORDER_LN_BASE> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append(item.ORDER_LINE_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.CUST_PART_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.QTY).Append("</td>");
                    sb.Append("         <td>").Append(item.DESCRIPTION).Append("</td>");
                    sb.Append("         <td>").Append(item.DESC_CN).Append("</td>");
                    sb.Append("         <td>").Append(item.DUE_DATE.ToString("yyyy-MM-dd")).Append("</td>");
                    sb.Append("     </tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Addr All Html
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetXcecAddrAll(List<V_XCEC_ADDR_ALL> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append("<input type=\"radio\" name=\"radio\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.CUSTOMER_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.DEST_LOCATION).Append("</td>");
                    sb.Append("         <td>").Append(item.CONTACT).Append("</td>");
                    sb.Append("         <td>").Append(item.TEL).Append("</td>");
                    sb.Append("         <td>").Append(item.ADDR_NAME).Append("</td>");
                    sb.Append("         <td>").Append(item.ADDR_1).Append("</td>");
                    sb.Append("         <td>").Append(item.ADDR_2).Append("</td>");
                    sb.Append("         <td>").Append(item.ADDR_3).Append("</td>");
                    sb.Append("</tr>");
                }

                sb.Append("     <tr>");
                sb.Append("         <td colspan=\"9\" style=\"text-align:right;\">").Append("<input type=\"button\" class=\"btn btn-primary btn-sm ConfirmBtn\" value=\"Comfirm to Match\" style=\"height: 50px;\" />").Append("</td>");
                sb.Append("     </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 每行颜色
        /// </summary>
        /// <param name="PROCESS_STATUS">Q - 灰色；E - 红色 ；C-蓝色； G： 绿色：其他 白色</param>
        /// <returns></returns>
        private static string GetTrColor(string PROCESS_STATUS)
        {
            string color = string.Empty;
            switch (PROCESS_STATUS)
            {
                case "Q":
                    color = "#cccccc";
                    break;
                case "E":
                    color = "#ff6666";
                    break;
                case "C":
                    color = "#99ccff";
                    break;
                case "G":
                    color = "#ccff99";
                    break;
            }
            return color;
        }



        public ActionResult AdrMap()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.adrmap.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="GOMS_ADDR"></param>
        /// <param name="CREATE_DATE"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchAdrMap(string GOMS_ADDR, string CREATE_DATE)
        {
            JD_ADDR_CONVERT_BLL bll = new JD_ADDR_CONVERT_BLL();
            try
            {
                var result = bll.SearchAdrMap(GOMS_ADDR, CREATE_DATE);
                var mainHtml = GetAdrMapHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order SearchAdrMap", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="JID"></param>
        /// <param name="CONVERTED_ADDR"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAdrMap(string JID, string CONVERTED_ADDR, string TERRITORY)
        {
            JD_ADDR_CONVERT_BLL bll = new JD_ADDR_CONVERT_BLL();
            try
            {
                var result = bll.SearchAdrMap(JID);
                result.CONVERTED_ADDR = CONVERTED_ADDR;
                result.TERRITORY = TERRITORY;
                result.UPDATED_BY = Session[CHubConstValues.SessionUser].ToString();
                result.RECORD_DATE = DateTime.Now;
                bll.UpdateAdrMap(result);
                return Json(new RequestResult(true, "Success"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order SaveAdrMap", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult CheckSecurityOfAMSave()
        {
            JD_ADDR_CONVERT_BLL bll = new JD_ADDR_CONVERT_BLL();
            try
            {
                string SECURE_ID = "JD_ADDR_CVT";
                string APP_USER = Session[CHubConstValues.SessionUser].ToString();
                if (bll.CheckSecurityOfAMSave(SECURE_ID, APP_USER))
                    return Json(new RequestResult(true));
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("order CheckSecurityOfAMSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public static string GetAdrMapHtml(List<JD_ADDR_CONVERT> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>").Append(item.JID).Append("</td>");
                    sb.Append("<td title=\"" + item.GOMS_ADDR + "\">").Append(item.GOMS_ADDR).Append("</td>");
                    sb.Append("<td title=\"" + item.TERRITORY + "\">").Append("<span class=\"txtTERRITORY\">").Append(item.TERRITORY).Append("</span>")
                        .Append("<div style=\"float:right\">")
                                .Append("<span class=\"glyphicon glyphicon-th-list txtArea\" style=\"cursor:pointer;\"></span>&nbsp;")
                                .Append("<span class=\"glyphicon glyphicon-remove txtRemove\" title=\"Remove\" style=\"cursor: pointer;\"></span>")
                         .Append("</div>")
                       .Append("</td>");
                    sb.Append("<td title=\"" + item.CONVERTED_ADDR + "\">").Append("<input type=\"text\" class=\"form-control input-sm CONV_ADDR\" value=\"" + item.CONVERTED_ADDR + "\" title=\"" + item.CONVERTED_ADDR + "\" style=\"width:100%;\" />").Append("</td>");
                    sb.Append("<td>").Append(item.CREATE_DATE.HasValue ? item.CREATE_DATE.Value.ToString("yyyy-MM-dd") : "").Append("</td>");
                    sb.Append("<td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm saveBtn\" value=\"SAVE\" data-id=\"" + item.JID + "\" />").Append("</td>");
                }
            }
            return sb.ToString();
        }


        [HttpPost]
        public ActionResult GetArea()
        {
            JD_ADDR_CONVERT_BLL acBLL = new JD_ADDR_CONVERT_BLL();
            List<Areas> areaList = new List<Areas>();
            try
            {
                var result = acBLL.GetArea("1");
                //var province = acBLL.GetArea("1").Select(a => a.PROVINCE).ToList();
                var province = result.Select(p => p.PROVINCE).Distinct().ToList();
                foreach (var p in province)//省
                {
                    Areas areaPro = new Areas();
                    areaPro.text = p;
                    areaPro.value = p;
                    areaPro.nodes = new List<Areas>();
                    //var city = acBLL.GetArea("2", p).Select(a=>a.CITY).ToList();
                    var city = result.Where(a => a.PROVINCE == p).Select(b => b.CITY).Distinct().ToList();
                    foreach (var c in city)//市
                    {
                        Areas areaCity = new Areas();
                        areaCity.text = c;
                        areaCity.value = p + c;
                        areaCity.nodes = new List<Areas>();
                        areaPro.nodes.Add(areaCity);
                        //var county = acBLL.GetArea("3", p, c).Select(a=>a.COUNTY).ToList();
                        var county = result.Where(a => a.PROVINCE == p && a.CITY == c).Select(b => b.COUNTY).Distinct().ToList();
                        foreach (var co in county)//村
                        {
                            Areas areaCounty = new Areas();
                            areaCounty.text = co;
                            areaCounty.value = p + c + co;
                            //var town = acBLL.GetArea("4", p, c, co).Select(a => a.TOWN).ToList();
                            var town = result.Where(a => a.PROVINCE == p && a.CITY == c && a.COUNTY == co).Select(b => b.TOWN).Distinct().ToList();
                            if (town.Count() == 1 && town[0] == null)
                                areaCounty.nodes = null;
                            else
                                areaCounty.nodes = new List<Areas>();
                            areaCity.nodes.Add(areaCounty);
                            foreach (var t in town)//镇
                            {
                                Areas areaTown = new Areas();
                                if (!string.IsNullOrEmpty(t))
                                {
                                    areaTown.text = t;
                                    areaTown.value = p + c + co + t;
                                    areaCounty.nodes.Add(areaTown);
                                }
                            }

                        }
                    }
                    areaList.Add(areaPro);
                }
                return Json(new RequestResult(areaList));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetArea", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public class Areas
        {
            public string text { get; set; }
            public string value { get; set; }
            public List<Areas> nodes { get; set; }
        }


        public ActionResult QuickOrd()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.quickord.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [HttpPost]
        public ActionResult GetG_ADDR_DFLT(string SYSID, string ABBREVIATION)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                var result = qBLL.GetG_ADDR_DFLT(SYSID, ABBREVIATION);
                return Json(new RequestResult(result.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetG_ADDR_DFLT", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetG_ADDR_SPL(string SYSID, string ABBREVIATION, string KeyWord, int PageIndex)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                string msg = string.Empty;
                int PageStart = (PageIndex - 1) * 50 + 1;
                int PageEnd = PageIndex * 50;
                var result = qBLL.GetG_ADDR_SPL(SYSID, ABBREVIATION, KeyWord, PageStart, PageEnd);
                if (result.Count() < 50)
                    msg = "End";
                var mainHtml = GetG_ADDR_SPLHtml(result);
                return Json(new RequestResult(true, msg, mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetG_ADDR_SPL");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetG_ADDR_SPLDetail(string SYSID, string ABBREVIATION, string DEST_LOCATION)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                var result = qBLL.GetG_ADDR_SPLDetail(SYSID, ABBREVIATION, DEST_LOCATION);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetG_ADDR_SPLDetail");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetG_ORDER_TYPE(string SYSID, string WAREHOUSE, string DUE_DATE_CODE = "")
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                var result = qBLL.GetG_ORDER_TYPE(SYSID, WAREHOUSE, DUE_DATE_CODE);
                if (!string.IsNullOrEmpty(DUE_DATE_CODE))
                    return Json(new RequestResult(result.First()));
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetG_ORDER_TYPE");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetOrderDetail(string str, List<OrderLines> detail, string Header_DUE_DATE, string CUSTOMER_NO, string WAREHOUSE)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            if (!string.IsNullOrEmpty(str))
            {
                int lineNo;
                if (detail != null && detail.Any())
                    lineNo = detail.Count();
                else
                {
                    lineNo = 0;
                    detail = new List<OrderLines>();
                }
                var data = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
                data = data.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                foreach (var d in data)
                {
                    OrderLines ol = new OrderLines();
                    ol.LINE_NO = lineNo + 1;

                    if (d.IndexOf("\t") > 0)
                    {
                        ol.CUSTOMER_PARTNO = d.Split('\t')[0];
                        ol.BUY_QTY = d.Split('\t')[1];
                    }
                    else
                        ol.CUSTOMER_PARTNO = d;
                    ol.PART_NO = qBLL.CallF_QUICK_PART(CUSTOMER_NO, ol.CUSTOMER_PARTNO);
                    ol.RevisedQty = qBLL.CallF_QUICK_QTY(ol.PART_NO, ol.BUY_QTY);
                    ol.DESCRIPTION = qBLL.CallF_QUICK_DESC(ol.PART_NO);
                    ol.NOTE = qBLL.CallF_QUICK_MSG(ol.PART_NO);
                    ol.Inventory = qBLL.CallF_QUICK_INV(WAREHOUSE, ol.PART_NO);
                    ol.DUE_DATE = Header_DUE_DATE;
                    detail.Add(ol);
                    lineNo++;
                }
                var mainHtml = GetOrderDetailHtml(detail);
                return Json(new RequestResult(mainHtml));
            }
            else
                return Json(new RequestResult(false, "No Data!"));
        }

        [HttpPost]
        public ActionResult QuickOrdSave(QUICK_OEORDER_HEADER header, List<QUICK_OEORDER_DETAIL> detail)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                //Update
                if (header.QUICK_ORDER_NO != 0)
                {
                    //更新header
                    qBLL.UpdateQUICK_OEORDER_HEADER(header);
                    //更新detail
                    qBLL.GetQUICK_OEORDER_DETAILByQUICK_ORDER_NO(header.QUICK_ORDER_NO.ToString());
                    if (detail != null && detail.Any())
                    {
                        foreach (var de in detail)
                        {
                            de.QUICK_ORDER_NO = header.QUICK_ORDER_NO;
                            qBLL.SaveQUICK_OEORDER_DETAIL(de);
                        }
                    }
                    return Json(new RequestResult(header.QUICK_ORDER_NO));
                }
                else//Insert
                {
                    //获取seq
                    var QUICK_ORDER_NO = qBLL.GetQUICK_ORDER_NO();
                    //保存header
                    header.QUICK_ORDER_NO = Convert.ToDecimal(QUICK_ORDER_NO);
                    header.CREATED_BY = Session[CHubConstValues.SessionUser].ToString();
                    qBLL.SaveQUICK_OEORDER_HEADER(header);
                    //保存detail
                    if (detail != null && detail.Any())
                    {
                        foreach (var de in detail)
                        {
                            de.QUICK_ORDER_NO = Convert.ToDecimal(QUICK_ORDER_NO);
                            qBLL.SaveQUICK_OEORDER_DETAIL(de);
                        }
                    }
                    return Json(new RequestResult(QUICK_ORDER_NO));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order QuickOrdSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult QuickOrdDownload(string QUICK_ORDER_NO)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                if (!string.IsNullOrEmpty(QUICK_ORDER_NO))
                {
                    StringBuilder sb = new StringBuilder();
                    var header = qBLL.GetV_QUICK_EXPORT_WEBPART_HDR(Convert.ToDecimal(QUICK_ORDER_NO));
                    if (header == null)
                        return Json(new RequestResult(false, "No Header data"));

                    var detail = qBLL.GetV_QUICK_EXPORT_WEBPART_DTL(Convert.ToDecimal(QUICK_ORDER_NO));
                    if (detail == null || detail.Count == 0)
                        return Json(new RequestResult(false, "No Detail Data"));

                    sb.AppendLine(header.First().TXT);
                    foreach (var de in detail)
                    {
                        sb.AppendLine(de.TXT);
                    }
                    string fileName = header.First().FILE_NAME;
                    string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                    FileInfo folder = new FileInfo(folderPath);
                    if (!Directory.Exists(folder.FullName))
                        Directory.CreateDirectory(folder.FullName);
                    string fullPath = folder.FullName + fileName;

                    FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    return Json(new RequestResult(fileName));
                }
                else
                    return Json(new RequestResult(false, "No QUICK_ORDER_NO date"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order QuickOrdDownload", ex);
                return Json(new RequestResult(false, "fail to download"));
            }
        }

        public void QODownload(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
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
            System.IO.File.Delete(fullPath);
        }

        [HttpPost]
        public ActionResult GetSingleDetail(string CUSTOMER_PARTNO, string BUY_QTY, string CUSTOMER_NO, string WAREHOUSE)
        {
            QuickOrd_BLL qBLL = new QuickOrd_BLL();
            try
            {
                OrderLines ol = new OrderLines();
                ol.CUSTOMER_PARTNO = CUSTOMER_PARTNO;
                ol.BUY_QTY = BUY_QTY;
                ol.PART_NO = qBLL.CallF_QUICK_PART(CUSTOMER_NO, ol.CUSTOMER_PARTNO);
                ol.RevisedQty = qBLL.CallF_QUICK_QTY(ol.PART_NO, ol.BUY_QTY);
                ol.DESCRIPTION = qBLL.CallF_QUICK_DESC(ol.PART_NO);
                ol.NOTE = qBLL.CallF_QUICK_MSG(ol.PART_NO);
                ol.Inventory = qBLL.CallF_QUICK_INV(WAREHOUSE, ol.PART_NO);
                return Json(new RequestResult(ol));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Order GetSingleDetail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public string GetOrderDetailHtml(List<OrderLines> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.LINE_NO).Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm CUSTOMER_PARTNO\" value=\"" + item.CUSTOMER_PARTNO + "\" />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm BUY_QTY\" value=\"" + item.BUY_QTY + "\" />").Append("</td>");
                    if (!string.IsNullOrEmpty(item.PART_NO))
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm PART_NO\" value=\"" + item.PART_NO + "\" disabled />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm PART_NO\" value=\"" + item.PART_NO + "\" disabled style=\"border-color: red;\" />").Append("</td>");

                    if (item.BUY_QTY != item.RevisedQty)
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm RevisedQty\" value=\"" + item.RevisedQty + "\" disabled style=\"color:red;\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm RevisedQty\" value=\"" + item.RevisedQty + "\" disabled />").Append("</td>");


                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm DESCRIPTION\" value=\"" + item.DESCRIPTION + "\" disabled />").Append("</td>");

                    if (!string.IsNullOrEmpty(item.NOTE))
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm NOTE\" value=\"" + item.NOTE + "\" disabled style=\"color:red;\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm NOTE\" value=\"" + item.NOTE + "\" disabled />").Append("</td>");

                    if (item.Inventory == "0")
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm Inventory\" value=\"" + item.Inventory + "\" disabled style=\"color:red;\" />").Append("</td>");
                    else if (Convert.ToInt32(item.Inventory) >= Convert.ToInt32(item.RevisedQty))
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm Inventory\" value=\"" + item.Inventory + "\" disabled style=\"color:green;\" />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm Inventory\" value=\"" + item.Inventory + "\" disabled style=\"color:orange;\" />").Append("</td>");

                    sb.Append("     <td>").Append("<input type=\"text\" class=\"form-control input-sm calendarReWriter DUE_DATE\" value=\"" + item.DUE_DATE + "\" onclick='ShowCalendar(this)' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-xs btnDelete\" value=\"delete\" />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

        public string GetG_ADDR_SPLHtml(List<G_ADDR_SPL> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type='radio' name='Radio' class='RadioSelect' value='" + item.DEST_LOCATION + "' />").Append("</td>");
                    sb.Append("     <td>").Append(item.DEST_LOCATION).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_ADDR_1).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_ADDR_2).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_DEST_ADDR_3).Append("</td>");
                    sb.Append("     <td>").Append(item.DEST_CONTACT).Append("</td>");
                    sb.Append("     <td>").Append(item.DEST_PHONE).Append("</td>");
                    sb.Append("     <td>").Append(item.DEST_ATTENTION).Append("</td>");
                    sb.Append("     <td>").Append(item.WAREHOUSE).Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

        public class OrderLines
        {
            public int LINE_NO { get; set; }
            public string CUSTOMER_PARTNO { get; set; }
            public string BUY_QTY { get; set; }
            public string PART_NO { get; set; }
            public string RevisedQty { get; set; }
            public string DESCRIPTION { get; set; }
            public string NOTE { get; set; }
            public string Inventory { get; set; }
            public string DUE_DATE { get; set; }
        }


    }
}

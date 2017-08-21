using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubDBEntity;
using CHubModel;
using CHubCommon;
using static CHubCommon.CHubEnum;
using CHubModel.ExtensionModel;
using CHubDBEntity.UnmanagedModel;
using CHubBLL.OtherProcess;
using CHubModel.WebArg;
using CHubCommon.Printer;

namespace CHubMVC.Controllers
{
    public class RPController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.wbprt.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Init()
        {
            try
            {
                string defWHID = string.Empty;
                List<APP_WH> appWHList = null;
                //List<RP_WAYBILL_TYPE> whTypeList = null;
                List<DistinctCarCode> carCodeList = null;

                APP_USERS_BLL userBLL = new APP_USERS_BLL();
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                APP_USERS user = userBLL.GetAppUserByDomainName(appUser);
                defWHID = user.DEF_WH_ID;

                APP_WH_BLL whBLL = new APP_WH_BLL();
                appWHList = whBLL.GetAppWHList();
                

                //RP_WAYBILL_TYPE_BLL typeBLL = new RP_WAYBILL_TYPE_BLL();
                //whTypeList = typeBLL.GetWayBillType();

                RP_CAR_MST_BLL carBLL = new RP_CAR_MST_BLL();
                carCodeList = carBLL.GetDistinctCarCode();

                var obj = new
                {
                    defWHID = defWHID,
                    appWHList = appWHList,
                    carCodeList = carCodeList
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP index Init", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetWayBillBaseList(string whID, string carCode, string custName, string Address, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel1> result = wbHBLL.GetWayBillBaseList(whID, carCode, custName, Address, shipmentNo, staged, inProgress, printed);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetWayBillBaseList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetWayBillDetailList(string whID,string carCode, string orderType, string addr,string shipmentNo, bool staged, bool inProgress, string printed)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel2> result = wbHBLL.GetWayBillDetailList(whID,carCode, orderType, addr, shipmentNo, staged, inProgress, printed);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetWayBillDetailList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetPrintFile(RPWayBillHLevel1 group, List<RPWayBillHLevel2> selectedList,string whID, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            if (selectedList == null || selectedList.Count == 0)
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                selectedList = wbHBLL.GetWayBillDetailList(whID,group.CARCOD, group.ORDTYP_WB, group.ADDR_COMBINED,shipmentNo, staged, inProgress, printed);
            }
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            //return Json(new RequestResult(false, "No selected Data"));

            try
            {
                // order by shipNo to Get min shipNo
                selectedList.OrderBy(a => a.SHIP_ID);
                string minShipNo = selectedList[0].SHIP_ID;
                List<string> shipNoList = new List<string>();
                foreach (var item in selectedList)
                {
                    if (string.Compare(minShipNo, item.SHIP_ID) > 0)
                        minShipNo = item.SHIP_ID;

                    shipNoList.Add(item.SHIP_ID);
                }

                V_RP_WAYBILL_H_PRINT_BLL hPrintBLL = new V_RP_WAYBILL_H_PRINT_BLL();
                V_RP_WAYBILL_H_PRINT hPrint = hPrintBLL.GetHByShipNo(minShipNo);

                V_RP_WAYBILL_D_PRINT_BLL dPrintBLL = new V_RP_WAYBILL_D_PRINT_BLL();
                List<V_RP_WAYBILL_D_PRINT> dPrintList = dPrintBLL.GetDByShipNos(shipNoList);


                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                RPWayBillPrintBLL printBLL = new RPWayBillPrintBLL(basePath);
                string fileName = printBLL.BuildPrintFile(hPrint, dPrintList, appUser);
                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPrintFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult BatchPrint(List<RPWayBillHLevel1> groups,string whID,string shipmentNo,bool staged, bool inProgress, string printed)
        {
            if(groups==null || groups.Count==0)
                return Json(new RequestResult(false,"No seleted Data!"));
            try
            {
                List<WayBillPageData> pageDatas = new List<WayBillPageData>();

                string appUser = Session[CHubConstValues.SessionUser].ToString();
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                foreach (var group in groups)
                {
                    List<RPWayBillHLevel2> selectedList = wbHBLL.GetWayBillDetailList(whID,group.CARCOD, group.ORDTYP_WB, group.ADDR_COMBINED,shipmentNo, staged, inProgress, printed);

                    // order by shipNo to Get min shipNo
                    //selectedList.OrderBy(a => a.SHIP_ID);
                    string minShipNo = selectedList[0].SHIP_ID;
                    List<string> shipNoList = new List<string>();
                    foreach (var item in selectedList)
                    {
                        if (string.Compare(minShipNo, item.SHIP_ID) > 0)
                            minShipNo = item.SHIP_ID;

                        shipNoList.Add(item.SHIP_ID);
                    }

                    V_RP_WAYBILL_H_PRINT_BLL hPrintBLL = new V_RP_WAYBILL_H_PRINT_BLL();
                    V_RP_WAYBILL_H_PRINT hPrint = hPrintBLL.GetHByShipNo(minShipNo);

                    V_RP_WAYBILL_D_PRINT_BLL dPrintBLL = new V_RP_WAYBILL_D_PRINT_BLL();
                    List<V_RP_WAYBILL_D_PRINT> dPrintList = dPrintBLL.GetDByShipNos(shipNoList);

                    WayBillPageData pageData = new WayBillPageData() { Header = hPrint, Details = dPrintList };
                    pageDatas.Add(pageData);

                }



                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                RPWayBillPrintBLL printBLL = new RPWayBillPrintBLL(basePath);
                string fileName = printBLL.BuildBatchPrintFile(pageDatas, appUser);
                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("BatchPrint", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }




        [Authorize]
        public ActionResult AdrMst()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.cpackset.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetADRList(string custName)
        {
            try
            {
                RP_ADR_MST_BLL adrBLL = new RP_ADR_MST_BLL();
                List<ExRPADRMst> result = adrBLL.GetADRListByCustName(custName);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetADRList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ADRMstInit()
        {
            try
            {
                RP_CUST_PACK_TYPE_BLL pTypeBLL = new RP_CUST_PACK_TYPE_BLL();
                List<ExRPCustPackType> result = pTypeBLL.GetCustPackType();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ADRMstInit", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveCustPackID(ExRPADRMst model)
        {
            try
            {
                RP_ADR_MST_BLL adrBLL = new RP_ADR_MST_BLL();
                adrBLL.SaveCustPackID(model);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SaveCarWayBillID", ex);
                return Json(new RequestResult(false, ex.Message));
            }

        }



        #region Car Mst part
        [Authorize]
        public ActionResult CarMst()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.rpcar.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CarMstInit()
        {
            try
            {
                RP_CAR_MST_BLL carBLL = new RP_CAR_MST_BLL();
                List<DistinctCarCode> carCodes = carBLL.GetDistinctCarCode();

                RP_WAYBILL_TYPE_BLL typeBLL = new RP_WAYBILL_TYPE_BLL();
                List<RP_WAYBILL_TYPE> wbType = typeBLL.GetWayBillType();

                var obj = new
                {
                    carCodes = carCodes,
                    wbType = wbType
                };

                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("CarMstInit", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetCarList(string carCode)
        {
            try
            {
                RP_CAR_MST_BLL carBLL = new RP_CAR_MST_BLL();
                List<ExRPCarMst> result = carBLL.GetCARListByCode(carCode);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetCarList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveCarWayBillID(ExRPCarMst car)
        {
            try
            {
                RP_CAR_MST_BLL carBLL = new RP_CAR_MST_BLL();
                carBLL.SaveCARWayBillID(car);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SaveCarWayBillID", ex);
                return Json(new RequestResult(false, ex.Message));
            }

        }
        #endregion

        #region custom pack part
        [Authorize]
        public ActionResult Pack()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.custpack.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        public ActionResult PackInit()
        {
            try
            {
                string defWHID = string.Empty;
                List<APP_WH> appWHList = null;

                APP_USERS_BLL userBLL = new APP_USERS_BLL();
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                APP_USERS user = userBLL.GetAppUserByDomainName(appUser);
                defWHID = user.DEF_WH_ID;

                APP_WH_BLL whBLL = new APP_WH_BLL();
                appWHList = whBLL.GetAppWHList();
             

                var obj = new
                {
                    defWHID = defWHID,
                    appWHList = appWHList
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP PackInit", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        public ActionResult GetPackList(string whID, string shipID, string custName, string address, bool staged, int range)
        {
            try
            {
                V_RP_PACK_H_BASE_BLL packBLL = new V_RP_PACK_H_BASE_BLL();
                var result = packBLL.GetPackList(whID, shipID, custName, address, staged, range);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPackList", ex);
                return Json(new RequestResult(false, ex.Message));
            }

        }

        [Authorize]
        public ActionResult PrintPackData(List<string> lodList)
        {
            lodList = lodList.Distinct().ToList();
            if(lodList == null || lodList.Count==0)
                    return Json(new RequestResult(false, "No selected Data"));
            try
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                //List<PackPageData> pageDatas = new List<PackPageData>();
                //foreach (var id in idList)
                //{
                //    PackPageData page = new PackPageData();
                //    V_RP_PACK_H_PRINT_BLL hBLL = new V_RP_PACK_H_PRINT_BLL();
                //    var header = hBLL.GetPackHeader(id);

                //    V_RP_PACK_D_PRINT_BLL dBLL = new V_RP_PACK_D_PRINT_BLL();
                //    var detail = dBLL.GetPackDetails(id);
                //    page.Header = header;
                //    page.Details = detail;
                //    pageDatas.Add(page);

                //}
                //if(pageDatas.Count==0)
                //    return Json(new RequestResult(false,"No Page Data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                CustPackPrintBLL printBLL = new CustPackPrintBLL(basePath);
                string fileName = printBLL.PrintPackData(lodList, appUser);
                string webPath = "/temp/" + fileName;

                

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPackList", ex);
                return Json(new RequestResult(false, ex.Message));
            }

        }

        #endregion

        #region custom pack part
        [Authorize]
        public ActionResult Label()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbprt.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        public ActionResult LabelInit()
        {
            try
            {
                string defPrinter = string.Empty;
                List<ExRPLabelType> typeList = null;
                //List<RP_PRINTER> printerList = null;

                //APP_USERS_BLL userBLL = new APP_USERS_BLL();
                //string appUser = Session[CHubConstValues.SessionUser].ToString();
                //APP_USERS user = userBLL.GetAppUserByDomainName(appUser);
                //defPrinter = user.PRINTER_ID;

                RP_LABEL_TYPE_BLL typeBLL = new RP_LABEL_TYPE_BLL();
                typeList = typeBLL.GetLabelTypeExList();

                //RP_PRINTER_BLL printBLL = new RP_PRINTER_BLL();
                //printerList = printBLL.GetPrinterList();

                //defPrinter = defPrinter ?? "",
                //    printers = printerList,
                var obj = new
                {
                    types = typeList
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP LabelInit", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult QueryByParts(string printPartNo, string partNo, string status)
        {
            if(string.IsNullOrEmpty(printPartNo)&&string.IsNullOrEmpty(partNo)&&string.IsNullOrEmpty(status))
                return Json(new RequestResult(false,"no condition input"));

            try
            {
                V_PLABEL_BASE_BLL baseBLL = new V_PLABEL_BASE_BLL();
                var result = baseBLL.QueryByPart(printPartNo, partNo, status);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("QueryByParts", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult QueryByShipment(string shipmentNo, string boxNo, string printPartNo)
        {
            if (string.IsNullOrEmpty(shipmentNo) && string.IsNullOrEmpty(boxNo) && string.IsNullOrEmpty(printPartNo))
                return Json(new RequestResult(false, "no condition input"));
            try
            {
                V_SHIPMENT_D_ALL1ONE_BLL sdBLL = new V_SHIPMENT_D_ALL1ONE_BLL();
                var result = sdBLL.GetPLabelRows(shipmentNo, boxNo, printPartNo);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("QueryByShipment", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult PrintLabel(LabelPrintArg arg)
        {
            if (arg == null|| arg.items==null ||arg.items.Count==0)
                return Json(new RequestResult(false, "No data in Arg"));
            if(string.IsNullOrEmpty(arg.labelCode) || string.IsNullOrEmpty(arg.printer))
                return Json(new RequestResult(false, "No labelcode or printer info"));

            try
            {
                V_PLABEL_PRINT_BLL pBLL = new V_PLABEL_PRINT_BLL();

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);

                PrintHelper pHelper = new PrintHelper();
                foreach (var item in arg.items)
                {
                    List<V_PLABEL_PRINT> printData = pBLL.BatchGetLabelPrintData(new List<string> { item.partNo}, arg.labelCode);
                    if (printData == null || printData.Count == 0)
                        continue;
                    string fileName = lpBLL.BuildPDF(printData,arg.items);
                    string fullPath = basePath + fileName;
                    pHelper.PrintFileWithCopies(fullPath, arg.printer, item.copies);
                }

                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("PrintLabel", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ExportPDF(LabelPrintArg arg)
        {
            try
            {
                List<string> partNoList = (from a in arg.items select a.partNo).ToList();

                V_PLABEL_PRINT_BLL pBLL = new V_PLABEL_PRINT_BLL();
                List<V_PLABEL_PRINT> printData = pBLL.BatchGetLabelPrintData(partNoList, arg.labelCode);

                if(printData==null || printData.Count==0)
                    return Json(new RequestResult(false,"Get no page data"));


                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.BuildPDF(printData,arg.items);

                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ExportPDF", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        #endregion

        #region Track Num Query part
        [Authorize]
        public ActionResult TrackNum()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.trackinq.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult TrackNumInit()
        {
            try
            {
                string defWHID = string.Empty;
                List<APP_WH> appWHList = null;

                APP_USERS_BLL userBLL = new APP_USERS_BLL();
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                APP_USERS user = userBLL.GetAppUserByDomainName(appUser);
                defWHID = user.DEF_WH_ID;

                APP_WH_BLL whBLL = new APP_WH_BLL();
                appWHList = whBLL.GetAppWHList();

                var obj = new
                {
                    defWHID = defWHID,
                    appWHList = appWHList
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TrackNumInit", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetTrackNumLevel1(TrackNumQueryArg arg)
        {
            if(arg==null || string.IsNullOrEmpty(arg.WHID))
                return Json(new RequestResult(false,"Invalid arg data"));
            try
            {
                V_INQ_TRACKNUM_BLL tBLL = new V_INQ_TRACKNUM_BLL();
                var result = tBLL.GetTrackNumLevel1(arg);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetTrackNumLevel1", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetTrackNumLevel2(string shipID, TrackNumQueryArg arg)
        {
            try
            {
                V_INQ_TRACKNUM_BLL tBLL = new V_INQ_TRACKNUM_BLL();
                var result = tBLL.GetTrackNumLevel2(shipID,arg);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetTrackNumLevel2", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        #endregion




    }
}
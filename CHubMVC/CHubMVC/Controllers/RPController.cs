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
using System.Reflection;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using System.Configuration;

namespace CHubMVC.Controllers
{
    public class RPController : BaseController
    {
        private static string webUrl = ConfigurationManager.AppSettings["WebUrl"].ToString();
        private static string webType = ConfigurationManager.AppSettings["WebType"].ToString();

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
                wbHBLL.PreWorkRP_H(whID);
                Thread.Sleep(1000);
                wbHBLL.PreWorkRP_SMRY(whID);
                Thread.Sleep(1000);
                wbHBLL.PreWorkRP_D(whID);
                Thread.Sleep(1000);

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
        public ActionResult GetWayBillDetailList(string whID, string carCode, string orderType, string addr, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel2> result = wbHBLL.GetWayBillDetailList(whID, carCode, orderType, addr, shipmentNo, staged, inProgress, printed);
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
        public ActionResult GetPrintFile(RPWayBillHLevel1 group, List<RPWayBillHLevel2> selectedList, string whID, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            if (selectedList == null || selectedList.Count == 0)
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                selectedList = wbHBLL.GetWayBillDetailList(whID, group.CARCOD, group.ORDTYP_WB, group.ADDR_COMBINED, shipmentNo, staged, inProgress, printed);
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
        public ActionResult BatchPrint(List<RPWayBillHLevel1> groups, string whID, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            if (groups == null || groups.Count == 0)
                return Json(new RequestResult(false, "No seleted Data!"));
            try
            {
                List<WayBillPageData> pageDatas = new List<WayBillPageData>();

                string appUser = Session[CHubConstValues.SessionUser].ToString();
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                foreach (var group in groups)
                {
                    List<RPWayBillHLevel2> selectedList = wbHBLL.GetWayBillDetailList(whID, group.CARCOD, group.ORDTYP_WB, group.ADDR_COMBINED, shipmentNo, staged, inProgress, printed);

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
                packBLL.PreWorkRP_H(whID);
                Thread.Sleep(1000);
                packBLL.PreWorkRP_SMRY(whID);
                Thread.Sleep(1000);
                packBLL.PreWorkRP_D(whID);
                Thread.Sleep(1000);

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
            if (lodList == null || lodList.Count == 0)
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


        
        public ActionResult Label2()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbprt2.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            ViewBag.Url = webUrl;
            ViewBag.Type = webType;
            return View();
        }



        
        public ActionResult LabelType()
        {
            RP_LABEL_TYPE2_BLL BLL = new RP_LABEL_TYPE2_BLL();
            List<string> LABEL_CODE = new List<string>();

            try
            {
                LABEL_CODE = BLL.GetLABEL_CODEList().ToList();
                return Json(new RequestResult(LABEL_CODE));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP LabelType", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public ActionResult GetWHID()
        {
            APP_WH_BLL bll = new APP_WH_BLL();
            List<string> whid = new List<string>();
            try
            {
                whid = bll.GetAppWHList().Select(w => w.WH_ID).ToList();
                return Json(new RequestResult(whid));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetWHID", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult GetLabel_Code()
        {
            RP_LABEL_TYPE2_BLL bll = new RP_LABEL_TYPE2_BLL();
            List<string> Label_Code = new List<string>();
            try
            {
                Label_Code = bll.GetLabel_Code();
                return Json(new RequestResult(Label_Code));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetLabel_Code");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// GetSite
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSite()
        {
            RP_LABEL_TYPE2_BLL bll = new RP_LABEL_TYPE2_BLL();
            List<APP_SITES> sites = new List<APP_SITES>();
            try
            {
                sites = bll.GetSite();
                return Json(new RequestResult(sites));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetSite");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult ChangeSite(string SITE_NAME)
        {
            RP_LABEL_TYPE2_BLL bll = new RP_LABEL_TYPE2_BLL();
            List<RP_PRINTER> printers = new List<RP_PRINTER>();
            try
            {
                printers = bll.ChangeSite(SITE_NAME);
                return Json(new RequestResult(printers));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP ChangeSite");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        /// <summary>
        /// GetLabelTypeDESC
        /// </summary>
        /// <param name="Label_TYPE"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetLabelTypeDesc(string Label_TYPE)
        {
            RP_LABEL_TYPE2_BLL BLL = new RP_LABEL_TYPE2_BLL();
            try
            {
                var result = BLL.GetLabel_DESC(Label_TYPE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetLabelTypeDesc", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        public ActionResult GetADRNAMEByShip_ID(string Ship_ID)
        {
            V_PLABEL_BY_LOD_PRINT_BLL bll = new V_PLABEL_BY_LOD_PRINT_BLL();
            try
            {
                var result = bll.GetADRNAMEByShip_ID(Ship_ID);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetADRNAMEByShip_ID", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        public ActionResult GetADRNAMEByLODNUM(string LODNUM)
        {
            V_PLABEL_BY_LOD_PRINT_BLL bll = new V_PLABEL_BY_LOD_PRINT_BLL();
            try
            {
                var result = bll.GetADRNAMEByLODNUM(LODNUM);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetADRNAMEByLODNUM", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }



        /// <summary>
        /// SearchByCondition
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult PLABEL_PRINTBySearch(string LabelTYPE, string ShipmentNo, string BoxNumber, string PartNumber_RP, string PartNumber_GOMS)
        {
            V_PLABEL_BY_LOD_PRINT_BLL bll = new V_PLABEL_BY_LOD_PRINT_BLL();
            try
            {
                var result = bll.QueryBySearch(LabelTYPE, ShipmentNo, BoxNumber, PartNumber_RP, PartNumber_GOMS);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP PLABEL_PRINTBySearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ExportPDFNew(LabelPrintModel arg)
        {
            V_PLABEL_BY_LOD_PRINT_BLL bll = new V_PLABEL_BY_LOD_PRINT_BLL();
            try
            {
                //List<string> partNoList = (from a in arg.items select a.PART_NO).ToList();
                List<string> VID = (from i in arg.items select i.VID).ToList();
                var result = bll.BatchGetLabelPrintData(VID, arg.LabelTYPE, arg.ShipmentNo, arg.BoxNumber, arg.PartNumber_RP, arg.PartNumber_GOMS);

                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);

                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.BuildPDF_New(result, arg.items);

                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ExportPDFNew", ex);
                return Json(new RequestResult(false, ex.Message));
            }


        }

        [HttpPost]
        public ActionResult AutoPrint_LOD(LabelPrintModel arg)
        {
            TxtLog.WriteLog("打印准备");
            V_PLABEL_BY_LOD_PRINT_BLL bll = new V_PLABEL_BY_LOD_PRINT_BLL();
            try
            {
                List<string> VID = (from i in arg.items select i.VID).ToList();
                var result = bll.BatchGetLabelPrintData(VID, arg.LabelTYPE, arg.ShipmentNo, arg.BoxNumber, arg.PartNumber_RP, arg.PartNumber_GOMS);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
                TxtLog.WriteLog("网络路径：" + basePath);

                LabelPrintBLL lpBll = new LabelPrintBLL(basePath);
                string baseBTW = new RP_LABEL_TYPE2_BLL().GetBTW(arg.LabelTYPE);//调用的模板文件名
                TxtLog.WriteLog("调用模板" + baseBTW);

                var bo = lpBll.AutoPrint_LOD(result, arg.items, baseBTW, arg.Printer_Name);
                if (bo)
                {
                    TxtLog.WriteLog("打印通过");
                    return Json(new RequestResult(true));
                }
                else
                {
                    TxtLog.WriteLog("打印失败");
                    return Json(new RequestResult(false, "fail to Print"));
                }
            }
            catch (Exception ex)
            {
                TxtLog.WriteLog(ex.Message);
                LogHelper.WriteLog("AutoPrint_LOD", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        public ActionResult PrintLBScan_M(LBScanPrintItems item)
        {
            TxtLog.WriteLog("打印准备");
            V_PLABEL_BY_MOBILE_PRINT_BLL vBLL = new V_PLABEL_BY_MOBILE_PRINT_BLL();
            try
            {
                var result = vBLL.GetPrintData(item.VID, item.WH_ID, item.LODNUM, item.PRTNUM, item.LABEL_CODE);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
                TxtLog.WriteLog("网络路径：" + basePath);

                LabelPrintBLL lpBll = new LabelPrintBLL(basePath);
                string baseBTW = new RP_LABEL_TYPE2_BLL().GetBTW(item.LABEL_CODE);//调用的模板文件名
                TxtLog.WriteLog("调用模板" + baseBTW);

                var bo = lpBll.PrintLBScan_M(result, item, baseBTW, item.PrinterName, Session[CHubConstValues.SessionUser].ToString());
                if (bo)
                {
                    TxtLog.WriteLog("打印通过");
                    return Json(new RequestResult(true));
                }
                else
                {
                    TxtLog.WriteLog("打印失败");
                    return Json(new RequestResult(false, "fail to Print"));
                }

            }
            catch (Exception ex)
            {
                TxtLog.WriteLog(ex.Message);
                LogHelper.WriteLog("RP PrintLBScan_M", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// By Parts Search
        /// </summary>
        /// <param name="Label_TYPE"></param>
        /// <param name="PRTNUM"></param>
        /// <param name="Part_No"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SearchByParts(string Label_TYPE, string PRTNUM, string Part_No)
        {
            V_PLABEL_BY_PART_PRINT_BLL bll = new V_PLABEL_BY_PART_PRINT_BLL();
            try
            {
                var result = bll.GetSearchByParts(Label_TYPE, PRTNUM, Part_No);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP SearchByParts");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// By Parts PrintPDF
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ExportPDFByParts(LabelPrintModel arg)
        {
            V_PLABEL_BY_PART_PRINT_BLL bll = new V_PLABEL_BY_PART_PRINT_BLL();
            try
            {
                List<string> partNoList = (from p in arg.items select p.PART_NO).ToList();
                var result = bll.GetPrintDataByPart(partNoList, arg.LabelTYPE);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);

                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.ExportPDFByParts(result, arg.items);

                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ExportPDFByParts", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult AutoPrint_PART(LabelPrintModel arg)
        {
            V_PLABEL_BY_PART_PRINT_BLL bll = new V_PLABEL_BY_PART_PRINT_BLL();
            try
            {
                List<string> partNoList = (from p in arg.items select p.PART_NO).ToList();
                var result = bll.GetPrintDataByPart(partNoList, arg.LabelTYPE);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));
                string basePath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string baseBTW = new RP_LABEL_TYPE2_BLL().GetBTW(arg.LabelTYPE);//调用的模板文件名

                var bo = lpBLL.AutoPrint_PART(result, arg.items, baseBTW, arg.Printer_Name);
                if (bo)
                {
                    return Json(new RequestResult(true));
                }
                else
                {
                    return Json(new RequestResult(false, "fail to Print"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("AutoPrint_PART", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// By ASN Search
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SearchByASN(string LABEL_CODE, string ASN_NO, string PRINT_PART_NO, string PART_NO)
        {
            V_PLABEL_BY_ASN_PRINT_BLL bll = new V_PLABEL_BY_ASN_PRINT_BLL();
            try
            {
                var result = bll.GetSearchByASN(LABEL_CODE, ASN_NO, PRINT_PART_NO, PART_NO);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP SearchByASN");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// By ASN Print
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ExportPDFByASN(LabelPrintModel arg)
        {
            V_PLABEL_BY_ASN_PRINT_BLL bll = new V_PLABEL_BY_ASN_PRINT_BLL();
            try
            {
                //List<string> partNo = (from i in arg.items select i.PART_NO).ToList();
                List<string> VID = (from i in arg.items select i.VID).ToList();

                var result = bll.GetPrintDatasByASN(VID, arg.LabelTYPE, arg.ASN_NO, arg.PART_NO_ASN, arg.PRINT_PART_NO_ASN);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);

                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.ExportPDFByASN(result, arg.items);

                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP ExportPDFByASN");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult AutoPrint_ASN(LabelPrintModel arg)
        {
            V_PLABEL_BY_ASN_PRINT_BLL bll = new V_PLABEL_BY_ASN_PRINT_BLL();
            try
            {
                List<string> VID = (from i in arg.items select i.VID).ToList();

                var result = bll.GetPrintDatasByASN(VID, arg.LabelTYPE, arg.ASN_NO, arg.PART_NO_ASN, arg.PRINT_PART_NO_ASN);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string baseBTW = new RP_LABEL_TYPE2_BLL().GetBTW(arg.LabelTYPE);//调用的模板文件名

                string basePath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                var bo = lpBLL.AutoPrint_ASN(result, arg.items, baseBTW, arg.Printer_Name);
                if (bo)
                {
                    return Json(new RequestResult(true));
                }
                else
                {
                    return Json(new RequestResult(false, "fail to Print"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP AutoPrint_ASN", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetCOMPANY_NAMEByASN_NO(string ASN_NO)
        {
            V_PLABEL_BY_ASN_PRINT_BLL bll = new V_PLABEL_BY_ASN_PRINT_BLL();
            try
            {
                var result = bll.GetCOMPANY_NAMEByASN_NO(ASN_NO);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetCOMPANY_NAMEByASN_NO");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// By Uncatalog Parts Search
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SearchByUncatalogsParts(string Label_TYPE, string PRINT_PART_NO_UParts, string PART_NO_UParts)
        {
            V_PLABEL_BY_UNCATALOG_PRINT_BLL bll = new V_PLABEL_BY_UNCATALOG_PRINT_BLL();
            try
            {
                var result = bll.GetSearchByUncatalog(Label_TYPE, PRINT_PART_NO_UParts, PART_NO_UParts);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP SearchByUncatalogsParts");
                return Json(new RequestResult(false, ex.Message));
            }
        }
        /// <summary>
        /// By Uncatalog Parts Print
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ExportPDFByUncatalogParts(LabelPrintModel arg)
        {
            V_PLABEL_BY_UNCATALOG_PRINT_BLL bll = new V_PLABEL_BY_UNCATALOG_PRINT_BLL();
            try
            {
                List<string> partNoList = (from q in arg.items select q.PART_NO).ToList();
                var result = bll.GetPrintDataByUncatalog(partNoList, arg.LabelTYPE, arg.PRINT_PART_NO_UParts, arg.PART_NO_UParts);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);

                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.ExportPDFByUncatalogParts(result, arg.items);

                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP ExportPDFByUncatalogParts");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        public ActionResult AutoPrint_UParts(LabelPrintModel arg)
        {
            V_PLABEL_BY_UNCATALOG_PRINT_BLL bll = new V_PLABEL_BY_UNCATALOG_PRINT_BLL();
            try
            {
                List<string> partNoList = (from q in arg.items select q.PART_NO).ToList();
                var result = bll.GetPrintDataByUncatalog(partNoList, arg.LabelTYPE, arg.PRINT_PART_NO_UParts, arg.PART_NO_UParts);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string baseBTW = new RP_LABEL_TYPE2_BLL().GetBTW(arg.LabelTYPE);//调用的模板文件名

                string basePath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                var bo = lpBLL.AutoPrint_UParts(result, arg.items, baseBTW, arg.Printer_Name);
                if (bo)
                {
                    return Json(new RequestResult(true));
                }
                else
                {
                    return Json(new RequestResult(false, "fail to Print"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP AutoPrint_UParts", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// By KITS Search
        /// </summary>
        /// <param name="Label_Code"></param>
        /// <param name="Part_No"></param>
        /// <returns></returns>
        public ActionResult SearchByKITS(string Label_Code, string Part_No)
        {
            V_PLABEL_BY_KITS_PRINT_BLL bll = new V_PLABEL_BY_KITS_PRINT_BLL();
            try
            {
                var result = bll.SearchByKITS(Label_Code, Part_No);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP SearchByKITS");
                return Json(new RequestResult(false, ex.Message));
            }
        }
        /// <summary>
        /// By KITS Print
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public ActionResult ExportPDFByKITS(PlabelByKITSPrintArg arg)
        {
            V_PLABEL_BY_KITS_PRINT_BLL bll = new V_PLABEL_BY_KITS_PRINT_BLL();
            try
            {
                //List<string> VIDs = (from p in arg.items select p.VID).ToList();
                var result = bll.GetPrintDataByKITS(arg.LABEL_CODE, arg.PART_NO);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.ExportPDFByKITs(result);

                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP ExportPDFByKITS");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult SaveByKITs(GKITSArg arg)
        {
            V_PLABEL_BY_KITS_PRINT_BLL bll = new V_PLABEL_BY_KITS_PRINT_BLL();
            G_KITS_BLL gbll = new G_KITS_BLL();
            try
            {
                if (!string.IsNullOrEmpty(arg.VID)) //Update
                {
                    if (bll.IsExistMore(arg) > 1)   //重复
                    {
                        return Json(new RequestResult(false, "The Data Has Existed"));
                    }
                    else
                    {
                        var response = bll.GetKITSByVID(arg.VID).FirstOrDefault();
                        G_KITS result = gbll.GetGKITSBySearch(response.PARENT_PART, response.COMPONENT_PART, response.LINE_ITEM_NO).FirstOrDefault();//OLD
                        G_KITS gkits = new G_KITS(); //NEW
                        gkits.PARENT_PART = arg.PARENT_PART;
                        gkits.LINE_ITEM_NO = arg.LINE_ITEM_NO;
                        gkits.COMPONENT_PART = arg.COMPONENT_PART;
                        gkits.COMPONENT_PRINT_NO = arg.COMPONENT_PRINT_NO;
                        gkits.COMPONENT_DESC = arg.COMPONENT_DESC;
                        gkits.COMPONENT_DESC_CN = arg.COMPONENT_DESC_CN;
                        gkits.COMPONENT_COO = arg.COMPONENT_COO;
                        gkits.QTY_PER_ASSEMBLY = arg.QTY_PER_ASSEMBLY;
                        gkits.EFF_PHASE_IN_DATE = arg.EFF_PHASE_IN_DATE;
                        gkits.EFF_PHASE_OUT_DATE = arg.EFF_PHASE_OUT_DATE;
                        gkits.NOTE_TEXT = result.NOTE_TEXT;
                        gkits.CREATE_DATE = response.CREATE_DATE;
                        gkits.LOAD_DATE = DateTime.Now;
                        gbll.UpdateKITS(result, gkits);
                        return Json(new RequestResult("The Data Has Saved"));
                    }
                }
                else  //Add
                {
                    G_KITS gkits = new G_KITS();
                    gkits.PARENT_PART = arg.PARENT_PART;
                    gkits.LINE_ITEM_NO = arg.LINE_ITEM_NO;
                    gkits.COMPONENT_PART = arg.COMPONENT_PART;
                    gkits.COMPONENT_PRINT_NO = arg.COMPONENT_PRINT_NO;
                    gkits.COMPONENT_DESC = arg.COMPONENT_DESC;
                    gkits.COMPONENT_DESC_CN = arg.COMPONENT_DESC_CN;
                    gkits.COMPONENT_COO = arg.COMPONENT_COO;
                    gkits.QTY_PER_ASSEMBLY = arg.QTY_PER_ASSEMBLY;
                    gkits.EFF_PHASE_IN_DATE = arg.EFF_PHASE_IN_DATE;
                    gkits.EFF_PHASE_OUT_DATE = arg.EFF_PHASE_OUT_DATE;
                    gkits.NOTE_TEXT = "";
                    gkits.LOAD_DATE = DateTime.Now;
                    gbll.AddKITS(gkits);
                    return Json(new RequestResult("The Data Has Saved"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP SaveByKITs");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public ActionResult GetLabelCodeDesc(string Label_Code)
        {
            V_PLABEL_BY_KITS_PRINT_BLL bll = new V_PLABEL_BY_KITS_PRINT_BLL();
            try
            {
                var result = bll.GetLabelCodeDesc(Label_Code);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetLabelCodeDesc");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult GetPartNoDescription(string Part_No)
        {
            V_PLABEL_BY_KITS_PRINT_BLL bll = new V_PLABEL_BY_KITS_PRINT_BLL();
            try
            {
                var result = bll.GetPartNoDescription(Part_No);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetPartNoDescription");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        /// <summary>
        /// By Uncatalog Parts Add
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddOrModifyUncatalogParts()
        {
            return View();
        }

        /// <summary>
        /// By Uncatalog Parts Modify
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddOrModifyUncatalogParts2(string PART_NO)
        {
            Session["PART_NO"] = PART_NO;
            return View();
        }


        public ActionResult GetuUncatalogByPART_NO()
        {
            G_UNCATALOG_PART_BLL bll = new G_UNCATALOG_PART_BLL();
            try
            {
                var result = bll.GetUncatalogByPART_NO(Session["PART_NO"].ToString());
                return Json(new RequestResult(result.First()));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetuUncatalogByPART_NO");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public ActionResult AddUncatalogParts(UncatalogPartsArg arg)
        {
            G_UNCATALOG_PART_BLL bll = new G_UNCATALOG_PART_BLL();
            try
            {
                var result = bll.GetUncatalogPart(arg.PART_NO, arg.PRINT_PART_NO);
                if (result != null && result.Count > 0)
                {
                    return Json(new RequestResult(false, "The PART_NO already exists"));
                }
                else
                {
                    G_UNCATALOG_PART gp = new G_UNCATALOG_PART();
                    gp.PART_NO = !string.IsNullOrEmpty(arg.PART_NO) ? arg.PART_NO : "";
                    gp.PRINT_PART_NO = !string.IsNullOrEmpty(arg.PRINT_PART_NO) ? arg.PRINT_PART_NO : "";
                    gp.DESCRIPTION = !string.IsNullOrEmpty(arg.DESCRIPTION) ? arg.DESCRIPTION : "";
                    gp.DESC_CN = !string.IsNullOrEmpty(arg.DESC_CN) ? arg.DESC_CN : "";
                    gp.COUNTRY_OF_ORIGIN = !string.IsNullOrEmpty(arg.COUNTRY_OF_ORIGIN) ? arg.COUNTRY_OF_ORIGIN : "";
                    gp.PART_WEIGHT = !string.IsNullOrEmpty(arg.PART_WEIGHT) ? Convert.ToDecimal(arg.PART_WEIGHT) : 0;
                    gp.QTY_IN_CARTON = !string.IsNullOrEmpty(arg.QTY_IN_CARTON) ? Convert.ToDecimal(arg.QTY_IN_CARTON) : 0;
                    gp.UNIT_MEAS = !string.IsNullOrEmpty(arg.UNIT_MEAS) ? arg.UNIT_MEAS : "";
                    gp.PART_LENGTH = !string.IsNullOrEmpty(arg.PART_LENGTH) ? Convert.ToDecimal(arg.PART_LENGTH) : 0;
                    gp.PART_WIDTH = !string.IsNullOrEmpty(arg.PART_WIDTH) ? Convert.ToDecimal(arg.PART_WIDTH) : 0;
                    gp.PART_HEIGHT = !string.IsNullOrEmpty(arg.PART_HEIGHT) ? Convert.ToDecimal(arg.PART_HEIGHT) : 0;
                    gp.NOTE_TEXT = !string.IsNullOrEmpty(arg.NOTE_TEXT) ? arg.NOTE_TEXT : "";
                    gp.CREATE_DATE = DateTime.Now;
                    gp.SHORT_DESCRIPTION = "";
                    gp.PART_STATUS = "";
                    gp.CURRENT_SALES_STATUS_CODE = "";
                    bll.AddUncatalogPart(gp);
                    return Json(new RequestResult("The Data has been added"));
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP AddUncatalogParts");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult ModifyUncatalogParts(UncatalogPartsArg arg)
        {
            G_UNCATALOG_PART_BLL bll = new G_UNCATALOG_PART_BLL();
            try
            {
                G_UNCATALOG_PART gp = new G_UNCATALOG_PART();
                gp.PART_NO = !string.IsNullOrEmpty(arg.PART_NO) ? arg.PART_NO : "";
                gp.PRINT_PART_NO = !string.IsNullOrEmpty(arg.PRINT_PART_NO) ? arg.PRINT_PART_NO : "";
                gp.DESCRIPTION = !string.IsNullOrEmpty(arg.DESCRIPTION) ? arg.DESCRIPTION : "";
                gp.DESC_CN = !string.IsNullOrEmpty(arg.DESC_CN) ? arg.DESC_CN : "";
                gp.COUNTRY_OF_ORIGIN = !string.IsNullOrEmpty(arg.COUNTRY_OF_ORIGIN) ? arg.COUNTRY_OF_ORIGIN : "";
                gp.PART_WEIGHT = !string.IsNullOrEmpty(arg.PART_WEIGHT) ? Convert.ToDecimal(arg.PART_WEIGHT) : 0;
                gp.QTY_IN_CARTON = !string.IsNullOrEmpty(arg.QTY_IN_CARTON) ? Convert.ToDecimal(arg.QTY_IN_CARTON) : 0;
                gp.UNIT_MEAS = !string.IsNullOrEmpty(arg.UNIT_MEAS) ? arg.UNIT_MEAS : "";
                gp.PART_LENGTH = !string.IsNullOrEmpty(arg.PART_LENGTH) ? Convert.ToDecimal(arg.PART_LENGTH) : 0;
                gp.PART_WIDTH = !string.IsNullOrEmpty(arg.PART_WIDTH) ? Convert.ToDecimal(arg.PART_WIDTH) : 0;
                gp.PART_HEIGHT = !string.IsNullOrEmpty(arg.PART_HEIGHT) ? Convert.ToDecimal(arg.PART_HEIGHT) : 0;
                gp.NOTE_TEXT = !string.IsNullOrEmpty(arg.NOTE_TEXT) ? arg.NOTE_TEXT : "";
                gp.RECORD_DATE = DateTime.Now;
                gp.SHORT_DESCRIPTION = "";
                gp.PART_STATUS = "";
                gp.CURRENT_SALES_STATUS_CODE = "";
                bll.ModifyUncatalogPart(gp);
                return Json(new RequestResult("The Data has been updated"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP ModifyUncatalogParts");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// TMS ADR AUTO Correct
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ADRCRT()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.adrcrt.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SearchADRCRT(string ADRNAM, string ADRLN1, int LOAD_DATE)
        {
            TMS_ADR_AUTO_CORRECT_BLL bll = new TMS_ADR_AUTO_CORRECT_BLL();
            try
            {
                var result = bll.SearchADRCRT(ADRNAM, ADRLN1, LOAD_DATE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ITT SearchADRCRTBy");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveADRCRT(TmsAdrAutoCorrectArg arg)
        {
            TMS_ADR_AUTO_CORRECT_BLL bll = new TMS_ADR_AUTO_CORRECT_BLL();
            try
            {
                TMS_ADR_AUTO_CORRECT tc = new TMS_ADR_AUTO_CORRECT();
                foreach (PropertyInfo info in tc.GetType().GetProperties())
                {
                    if (arg.GetType().GetProperty(info.Name) != null)
                    {
                        info.SetValue(tc, arg.GetType().GetProperty(info.Name).GetValue(arg));
                    }
                }
                bll.SaveADRCRT(tc);
                return Json(new RequestResult("The Data Has Saved"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ITT SaveADRCRT");
                return Json(new RequestResult(false, ex.Message));
            }
        }



        [Authorize]
        [HttpPost]
        public ActionResult QueryByParts(string printPartNo, string partNo, string status)
        {
            if (string.IsNullOrEmpty(printPartNo) && string.IsNullOrEmpty(partNo) && string.IsNullOrEmpty(status))
                return Json(new RequestResult(false, "no condition input"));

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
            if (arg == null || arg.items == null || arg.items.Count == 0)
                return Json(new RequestResult(false, "No data in Arg"));
            if (string.IsNullOrEmpty(arg.labelCode) || string.IsNullOrEmpty(arg.printer))
                return Json(new RequestResult(false, "No labelcode or printer info"));

            try
            {
                V_PLABEL_PRINT_BLL pBLL = new V_PLABEL_PRINT_BLL();

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);

                PrintHelper pHelper = new PrintHelper();
                foreach (var item in arg.items)
                {
                    List<V_PLABEL_PRINT> printData = pBLL.BatchGetLabelPrintData(new List<string> { item.partNo }, arg.labelCode);
                    if (printData == null || printData.Count == 0)
                        continue;
                    string fileName = lpBLL.BuildPDF(printData, arg.items);
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

                if (printData == null || printData.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));


                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.BuildPDF(printData, arg.items);

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
                var res = JsonConvert.SerializeObject(obj);

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
            if (arg == null) //|| string.IsNullOrEmpty(arg.WHID)
                return Json(new RequestResult(false, "Invalid arg data"));
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
                var result = tBLL.GetTrackNumLevel2(shipID, arg);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetTrackNumLevel2", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetTrackInfo(string UserID, string Sign, string TrackNum)
        {
            string str = string.Empty;
            Process pro = new Process();
            pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.FileName = @"D:\ControlApp\PostTest.exe"; //C:\inetpub\wwwroot\CHub\ControllerApp  //C:\Users\oo450\Documents\visual studio 2015\Projects\PostTest\PostTest\bin\Debug
            pro.StartInfo.Arguments = "" + UserID + " " + Sign + " " + TrackNum + "";
            try
            {
                Write("开始调用");
                pro.Start();
            }
            catch (Exception ex)
            {
                Write(ex.Message);
            }

            using (StreamReader sr = new StreamReader(pro.StandardOutput.BaseStream, Encoding.Unicode, true))
            {
                str = sr.ReadToEnd();
            }
            Write(str);
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var data = new
            //{
            //    UserID = UserID,
            //    Sign = Sign,
            //    TrackNum = TrackNum
            //};
            //string postData = serializer.Serialize(data);
            //str = HttpPostHelper.HttpPost("http://cummins.hi-genious.com/Interface/api/PodTrack/GetPodTrack", postData);

            return Json(new { data = str });
        }

        public void Write(string str)
        {
            //C:\Users\lg166a\Desktop  //C:\Users\oo450\Desktop\Log.txt
            StreamWriter sw = new StreamWriter(@"C:\Temp\Log.txt", true);
            sw.Write(str + "\n\r");
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region Dock part
        [Authorize]
        public ActionResult Dock()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.dockdte.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult DockInit()
        {
            try
            {
                List<APP_WH> appWHList = null;


                APP_WH_BLL whBLL = new APP_WH_BLL();
                appWHList = whBLL.GetAppWHList();

                var obj = new
                {
                    appWHList = appWHList
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DockInit", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult DearchDockData(string wareHouse, string from, string asn, int range)
        {
            try
            {
                GOMS_ASN_H_BLL gBLL = new GOMS_ASN_H_BLL();
                var result = gBLL.GetDockData(wareHouse, from, asn, range);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DearchDockData", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveDockData(GOMS_ASN_H data)
        {
            //Date check
            /*
            if (data.DOCK_DATE != null && data.CREATE_DATE != null)
                if (data.DOCK_DATE <= data.CREATE_DATE)
                    return Json(new RequestResult(false, "Dock Date should larger than create date"));
            */
            if (data.DOCK_DATE != null && data.SHIPMENT_DATE != null)
                if (data.DOCK_DATE <= data.SHIPMENT_DATE)
                    return Json(new RequestResult(false, "到货日期应大于发货日期！"));
            /*
           if (data.SHIPMENT_DATE != null && data.EST_DLVY_DATE != null)
               if (data.SHIPMENT_DATE >=data.EST_DLVY_DATE)
                   return Json(new RequestResult(false, "发货日期应小于预计到货日期！"));
            */

            try
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                GOMS_ASN_H_BLL gBLL = new GOMS_ASN_H_BLL();

                data.DOCK_DATE_BY = appUser;
                gBLL.SaveDockData(data);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DearchDockData", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        #endregion


        [Authorize]
        public ActionResult HSCODE()
        {
            return View();
        }


        [Authorize]
        public ActionResult GetCID()
        {
            TC_PART_CATEGORY_BLL bll = new TC_PART_CATEGORY_BLL();
            try
            {
                var result = bll.GetCIDList();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetCID");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        /// <summary>
        /// SearchByCode
        /// </summary>
        /// <param name="HSCODE"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetHSCODEByCode(string HSCODE)
        {
            TC_HSCODE_MST_BLL bll = new TC_HSCODE_MST_BLL();
            TC_PART_CATEGORY_BLL cbll = new TC_PART_CATEGORY_BLL();
            try
            {
                var result = bll.GetHSCODEByCode(HSCODE);
                var cresult = cbll.GetCIDList();
                return Json(new { Success = true, Data = result, Data1 = cresult });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetHSCODEByCode");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddHSCODE(TCHSCODEMSTArg HSCode)
        {
            TC_HSCODE_MST_BLL bll = new TC_HSCODE_MST_BLL();
            CHubDBEntity.UnmanagedModel.TC_HSCODE_MST tc = new CHubDBEntity.UnmanagedModel.TC_HSCODE_MST();
            try
            {
                string type = string.Empty;
                if (bll.IsExistHSCODE(HSCode.HSCODE))
                {
                    tc.RECORD_DATE = DateTime.Now;
                    type = "Update";
                }
                else
                {
                    tc.CREATE_DATE = DateTime.Now;
                    type = "Add";
                }

                foreach (PropertyInfo info in tc.GetType().GetProperties())
                {
                    if (HSCode.GetType().GetProperty(info.Name) != null)
                    {
                        info.SetValue(tc, HSCode.GetType().GetProperty(info.Name).GetValue(HSCode), null);
                    }
                }

                bll.AddOrUpdate(tc, type);
                return Json(new RequestResult(true, "Data Has Saved"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP AddHSCODE");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public ActionResult GetHSCODEAUDIT(string HSCODE)
        {
            TC_HSCODE_MST_BLL bll = new TC_HSCODE_MST_BLL();
            try
            {
                var result = bll.GetHsCodeAudit(HSCODE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetHSCODEAUDIT");
                return Json(new RequestResult(false, ex.Message));
            }
        }


    }
}
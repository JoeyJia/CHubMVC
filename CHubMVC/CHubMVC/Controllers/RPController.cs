﻿using System;
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

namespace CHubMVC.Controllers
{
    public class RPController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.cgldb.ToString(), this.Request.Url.AbsoluteUri);
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
                if (string.IsNullOrEmpty(defWHID))
                {
                    APP_WH_BLL whBLL = new APP_WH_BLL();
                    appWHList = whBLL.GetAppWHList();
                }

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
        public ActionResult GetWayBillBaseList(string whID,string carCode,string custName,string Address,string shipmentNo, bool staged, bool inProgress)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel1> result = wbHBLL.GetWayBillBaseList(whID, carCode, custName, Address, shipmentNo,staged,inProgress);
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
        public ActionResult GetWayBillDetailList(string carCode,string orderType,string addr,bool staged,bool inProgress)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel2> result = wbHBLL.GetWayBillDetailList(carCode, orderType,addr,staged,inProgress);
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
        public ActionResult GetPrintFile(RPWayBillHLevel1 group, List<RPWayBillHLevel2> selectedList, bool staged, bool inProgress)
        {
            if (selectedList == null || selectedList.Count == 0)
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                selectedList = wbHBLL.GetWayBillDetailList(group.CARCOD, group.ORDTYP_WB, group.ADDR_COMBINED,staged,inProgress);
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
                string fileName = printBLL.BuildPrintFile(hPrint, dPrintList,appUser);
                string webPath= "/temp/" + fileName;
               
                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetPrintFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }




        [Authorize]
        public ActionResult AdrMst()
        {
            //string appUser = Session[CHubConstValues.SessionUser].ToString();
            //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            //rpBLL.Add(appUser, CHubEnum.PageNameEnum.cgldb.ToString(), this.Request.Url.AbsoluteUri);
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
            //string appUser = Session[CHubConstValues.SessionUser].ToString();
            //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            //rpBLL.Add(appUser, CHubEnum.PageNameEnum.cgldb.ToString(), this.Request.Url.AbsoluteUri);
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
            //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            //rpBLL.Add(appUser, CHubEnum.PageNameEnum.cgldb.ToString(), this.Request.Url.AbsoluteUri);
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
                if (string.IsNullOrEmpty(defWHID))
                {
                    APP_WH_BLL whBLL = new APP_WH_BLL();
                    appWHList = whBLL.GetAppWHList();
                }

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

        #endregion



    }
}
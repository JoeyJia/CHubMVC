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
        public ActionResult GetWayBillBaseList(string whID,string carCode,string custName,string Address,string shipmentNo)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel1> result = wbHBLL.GetWayBillBaseList(whID, carCode, custName, Address, shipmentNo);
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
        public ActionResult GetWayBillDetailList(string carCode,string orderType,string addr)
        {
            try
            {
                V_RP_WAYBILL_H_BASE_BLL wbHBLL = new V_RP_WAYBILL_H_BASE_BLL();
                List<RPWayBillHLevel2> result = wbHBLL.GetWayBillDetailList(carCode, orderType,addr);
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
        public ActionResult GetPrintFile(List<RPWayBillHLevel2> selectedList)
        {
            if (selectedList == null || selectedList.Count == 0)
                return Json(new RequestResult(false, "No selected Data"));

            try
            {
                // order by shipNo to Get min shipNo
                selectedList.OrderBy(a => a.SHIP_ID);
                string minShipNo = selectedList[0].SHIP_ID;
                List<string> shipNoList = new List<string>();
                foreach (var item in selectedList)
                {
                    shipNoList.Add(item.SHIP_ID);
                }

                V_RP_WAYBILL_H_PRINT_BLL hPrintBLL = new V_RP_WAYBILL_H_PRINT_BLL();
                V_RP_WAYBILL_H_PRINT hPrint = hPrintBLL.GetHByShipNo(minShipNo);

                V_RP_WAYBILL_D_PRINT_BLL dPrintBLL = new V_RP_WAYBILL_D_PRINT_BLL();
                List<V_RP_WAYBILL_D_PRINT> dPrintList = dPrintBLL.GetDByShipNos(shipNoList);


                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                RPWayBillPrintBLL printBLL = new RPWayBillPrintBLL(basePath);
                string fileName = printBLL.BuildPrintFile(hPrint, dPrintList);
                string webPath= "/temp/" + fileName;
                //string ss = string.Empty;
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


    }
}
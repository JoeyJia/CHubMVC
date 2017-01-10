using CHubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubDBEntity;
using CHubBLL;
using CHubModel.ExtensionModel;
using CHubModel;
using CHubCommon;
using CHubDBEntity;
using System.Net;

namespace CHubMVC.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                //Session[CHubConstValues.SessionUser] = "lg166";// For test using
               return RedirectToAction("Login", "Account");

            ViewBag.AppUser = Session[CHubConstValues.SessionUser].ToString();
            return View();
        }

        [HttpPost]
        public JsonResult Init()
        {
            using (CHubEntities db = new CHubEntities())
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();
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

                return Json(obj);
            }

        }

        [HttpPost]
        public ActionResult SearchAddrs(string shipName,string addr,string aliasName, bool isSpecialShip)
        {
            try
            {
                List<ExVAliasAddr> list = new List<ExVAliasAddr>();
                CHubEntities db = new CHubEntities();
                if (isSpecialShip)
                {
                    V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                    list = bll.GetAliasAddrSPL(shipName.Trim(), addr.Trim(), aliasName);
                }
                else
                {
                    V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                    list = bll.GetAliasAddrDFLT(aliasName);
                }

                //Get from parameter table***
                if (list.Count > 10)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content(string.Format("Result has {0} items, Make Condition more strict", list.Count.ToString()));
                }
                if (list.Count == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("No result");
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
                TS_OR_HEADER_STAGE orHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.headInfo,arg.seq, arg.dueDate,arg.orderType,arg.shipCompFlag,arg.customerPONO, arg.orderNote,arg.isSpecialShip,appUser);
                TS_OR_HEADER_STAGE altORHeaderStage = null;
                if (arg.altHeadInfo != null)
                {
                    altORHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.altHeadInfo,arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag,arg.customerPONO, arg.orderNote, arg.isSpecialShip,appUser,true);
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
                    seq = bll.AddHeadersWithDetailsStage(orHeaderStage, altORHeaderStage,dStageList);
                else
                    seq = bll.UpdateHeadersWithDetailsStage(orHeaderStage, altORHeaderStage,dStageList);

                if (seq != 0.00M)
                    return Content(seq.ToString());
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("Fail to save draft");
                }
            }
            catch(Exception ee)
            {
                //log ee
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ee.Message);
            }
        }

        [HttpPost]
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
                TS_OR_HEADER orHeader = ManualClassConvert.ConvertExAliaAddr2Header(arg.headInfo, arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote,arg.isSpecialShip, appUser);
                TS_OR_HEADER altORHeader = null;
                if (arg.altHeadInfo != null)
                {
                    altORHeader = ManualClassConvert.ConvertExAliaAddr2Header(arg.altHeadInfo,arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, arg.isSpecialShip,appUser, true);
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
                    seq = bll.AddHeadersWithDetails(orHeader, altORHeader,detailList);
                else
                    seq = bll.UpdateHeadersWithDetails(orHeader, altORHeader,detailList);

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
                    altWareHosue = arg.altWareHosue
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


        #region *** private function ***
        private string GetPartNoFromCustPartNo(string custPartNo)
        {
            if (string.IsNullOrEmpty(custPartNo))
                return string.Empty;
            using (CHubEntities db = new CHubEntities())
            {
                G_CATALOG_CUSTOMER_PART_BLL custBLL = new G_CATALOG_CUSTOMER_PART_BLL(db);
                string PartNo = custBLL.GetPartNoFromCustPartNo(custPartNo);
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

            olArg.olItem.LastCheckNo = olArg.olItem.CustomerPartNo;
            olArg.olItem.LastQty = olArg.olItem.Qty;

            string msg = string.Empty;

            olArg.olItem.PartNo = GetPartNoFromCustPartNo(olArg.olItem.CustomerPartNo);


            olArg.olItem.PartNoPlaceHolder = string.Empty;
            if (string.IsNullOrEmpty(olArg.olItem.PartNo))
            {
                olArg.olItem.PartNoPlaceHolder = "Can't find Part NO";
                return null;
            }
            else
            {
                CHubEntities db = new CHubEntities();
                //Get description
                G_PART_DESCRIPTION_BLL pDescBLL = new G_PART_DESCRIPTION_BLL(db);
                G_PART_DESCRIPTION pDesc = pDescBLL.GetPartDescription(olArg.olItem.PartNo);
                olArg.olItem.Description = pDesc.DESCRIPTION;
                olArg.olItem.DescCN = pDesc.DESC_CN;

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
                                olArg.olItem.AltAVLCheckColor = CHubConstValues.SatisfyStockColor;
                            else
                                olArg.olItem.AltAVLCheckColor = CHubConstValues.PartialStockColor;
                            olArg.olItem.AltAVLCheck = altNet;
                        }
                    }
                }
                
            }

            return msg;
        }

        #endregion


    }
}

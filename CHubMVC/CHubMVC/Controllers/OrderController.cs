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
            //if (Session[CHubConstValues.SessionUser] == null)
                Session[CHubConstValues.SessionUser] = "lg166";// For test using
                                                               //return RedirectToAction("Login", "Account");

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
        public JsonResult SearchAddrs(string shipName,string addr,string aliasName, bool isSpecialShip)
        {
            List<ExVAliasAddr> list = new List<ExVAliasAddr>();
            CHubEntities db = new CHubEntities();
            if (isSpecialShip)
            {
                V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                list = bll.GetAliasAddrSPL(shipName.Trim(), addr.Trim(),aliasName);
            }
            else
            {
                V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                list = bll.GetAliasAddrDFLT(aliasName);
            }

            //Get from parameter table?
            if (list.Count > 10)
                return Json("OverFlow");

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetStrictAddrs(string shipName, string addr, string aliasName, bool isSpecialShip)
        {
            List<ExVAliasAddr> list = new List<ExVAliasAddr>();
            CHubEntities db = new CHubEntities();
            if (isSpecialShip)
            {
                V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                list = bll.GetStrictAliasAddrSPL(shipName.Trim(), addr.Trim(),aliasName);
            }
            else
            {
                V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                list = bll.GetStrictAliasAddrDFLT(shipName.Trim(), addr.Trim(),aliasName);
            }

            return Json(list);
        }


        [HttpPost]
        public ActionResult SaveDraft(OrderSaveArg arg)
        {
            try
            {
                if (arg.headInfo == null)
                    return Content("fail");

                string AppUser = Session[CHubConstValues.SessionUser].ToString();

                CHubEntities db = new CHubEntities();
                TS_OR_HEADER_STAGE_BLL bll = new TS_OR_HEADER_STAGE_BLL(db);

                TS_OR_HEADER_STAGE orHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.headInfo,arg.seq, arg.dueDate,arg.orderType,arg.shipCompFlag,arg.customerPONO, arg.orderNote,AppUser);
                TS_OR_HEADER_STAGE altORHeaderStage = null;
                if (arg.altHeadInfo != null)
                {
                    altORHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.altHeadInfo,arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag,arg.customerPONO, arg.orderNote, AppUser,true);
                }
                decimal seq = 0;
                if (string.IsNullOrEmpty(arg.seq))
                    seq = bll.AddHeaderwithAltStage(orHeaderStage, altORHeaderStage);
                else
                    seq = bll.UpdateHeaderWithAltStage(orHeaderStage, altORHeaderStage);

                if (seq != 0.00M)
                    return Content(seq.ToString());
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("Fail to save header draft");
                }
            }
            catch(Exception ee)
            {
                //log ee
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content("Fail to save header draft");
            }
        }

        [HttpPost]
        public ActionResult SaveOrder(OrderSaveArg arg)
        {
            try
            {
                if (arg.headInfo == null)
                    return Content("fail");

                string AppUser = Session[CHubConstValues.SessionUser].ToString();

                CHubEntities db = new CHubEntities();
                TS_OR_HEADER_BLL bll = new TS_OR_HEADER_BLL(db);

                TS_OR_HEADER orHeader = ManualClassConvert.ConvertExAliaAddr2Header(arg.headInfo, arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, AppUser);
                TS_OR_HEADER altORHeader = null;
                if (arg.altHeadInfo != null)
                {
                    altORHeader = ManualClassConvert.ConvertExAliaAddr2Header(arg.altHeadInfo,arg.seq, arg.dueDate, arg.orderType, arg.shipCompFlag, arg.customerPONO, arg.orderNote, AppUser, true);
                }

                decimal seq = 0;
                if (string.IsNullOrEmpty(arg.seq))
                    seq = bll.AddHeaderwithAlt(orHeader, altORHeader);
                else
                    seq = bll.UpdateHeaderwithAlt(orHeader, altORHeader);

                if (seq != 0.00M)
                    return Content(seq.ToString());
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("Fail to save header");
                }
            }
            catch (Exception ee)
            {
                //log ee
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content("Fail to save header");
            }
        }

        [HttpPost]
        public ActionResult GetPartNoFromCustPartNo(string custPartNo)
        {
            if (string.IsNullOrEmpty(custPartNo))
                return Content(string.Empty);
            using (CHubEntities db = new CHubEntities())
            {
                G_CATALOG_CUSTOMER_PART_BLL custBLL = new G_CATALOG_CUSTOMER_PART_BLL(db);
                string PartNo = custBLL.GetPartNoFromCustPartNo(custPartNo);
                if (string.IsNullOrEmpty(PartNo))
                {
                    G_PART_DESCRIPTION_BLL partBLL = new G_PART_DESCRIPTION_BLL(db);
                    if (partBLL.IsPartNoExist(custPartNo))
                        return Content(custPartNo);
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return Content("Can't get Part No");
                    }

                }
                return Content(PartNo);
            }
        }

        
    }
}

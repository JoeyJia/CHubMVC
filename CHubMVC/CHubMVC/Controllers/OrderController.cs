﻿using CHubCommon;
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
                ExAppCustAlias selectedAlias = acaList.First(a => a.DEFAULT_FLAG == CHubConstValues.IndY);

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
                    selectedAlias = selectedAlias
                };

                return Json(obj);
            }

        }

        [HttpPost]
        public JsonResult SearchAddrs(string shipName,string addr,bool isSpecialShip)
        {
            List<ExVAliasAddr> list = new List<ExVAliasAddr>();
            CHubEntities db = new CHubEntities();
            if (isSpecialShip)
            {
                V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                list = bll.GetAliasAddrSPL(shipName, addr);
            }
            else
            {
                V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                list = bll.GetAliasAddrDFLT(shipName, addr);
            }

            //Get from parameter table?
            if (list.Count > 10)
                return Json("OverFlow");

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetStrictAddrs(string shipName, string addr, bool isSpecialShip)
        {
            List<ExVAliasAddr> list = new List<ExVAliasAddr>();
            CHubEntities db = new CHubEntities();
            if (isSpecialShip)
            {
                V_ALIAS_ADDR_SPL_BLL bll = new V_ALIAS_ADDR_SPL_BLL();
                list = bll.GetStrictAliasAddrSPL(shipName.Trim(), addr.Trim());
            }
            else
            {
                V_ALIAS_ADDR_DFLT_BLL bll = new V_ALIAS_ADDR_DFLT_BLL();
                list = bll.GetStrictAliasAddrDFLT(shipName.Trim(), addr.Trim());
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

                CHubEntities db = new CHubEntities();
                TS_OR_HEADER_STAGE orHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.headInfo);
                TS_OR_HEADER_STAGE_BLL bll = new TS_OR_HEADER_STAGE_BLL(db);
                bll.AddHeaderStage(orHeaderStage);

                if (arg.altHeadInfo != null)
                {
                    TS_OR_HEADER_STAGE altORHeaderStage = ManualClassConvert.ConvertExAliaAddr2HeaderStage(arg.altHeadInfo);
                    bll.AddHeaderStage(altORHeaderStage);
                }

                return Content("success");
            }
            catch(Exception ee)
            {
                return Content("fail");
            }
        }

        [HttpPost]
        public ActionResult SaveOrder(TS_OR_HEADER orHeader,TS_OR_HEADER altORHeader)
        {
            try
            {
                if (orHeader == null)
                    return Content("fail");
                TS_OR_HEADER_BLL bll = new TS_OR_HEADER_BLL();
                bll.AddHeader(orHeader);
                if (altORHeader != null)
                    bll.AddHeader(altORHeader);

                return Content("success");
            }
            catch
            {
                return Content("fail");
            }
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using CHubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubDBEntity;
using CHubBLL;
using CHubModel.ExtensionModel;

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
            //using (CHubEntities db = new CHubEntities())
            //{
            CHubEntities db = new CHubEntities();
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
                    //selectedAlias = selectedAlias
                };

                return Json(obj);
            //}
                
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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

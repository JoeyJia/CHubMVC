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

namespace CHubMVC.Controllers
{
    public class TCController : Controller
    {
        [Authorize]
        public ActionResult Maint()
        {
            

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult QueryAction(string partNo, string hsCode, string declrName, string element)
        {
            CHubEntities db = new CHubEntities();
            V_TC_MDM_ALL_BLL mdmBLL = new V_TC_MDM_ALL_BLL(db);
            List<V_TC_MDM_ALL> result = mdmBLL.GetTCMDMList(partNo, hsCode, declrName, element);
            return Json(result);
        }

        public ActionResult EditAction(V_TC_MDM_ALL mdmAll)
        {

            return null;
        }


    }
}
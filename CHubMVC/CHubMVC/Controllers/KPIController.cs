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

namespace CHubMVC.Controllers
{
    public class KPIController : BaseController
    {
        //[Authorize]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        //[Authorize]
        public ActionResult GetKPIGroupList()
        {
            try
            {
                DB_KPI_GROUP_BLL gBLL = new DB_KPI_GROUP_BLL();
                List<DB_KPI_GROUP> result = gBLL.GetKPIGroups();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetKPIGroupList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        
    }
}
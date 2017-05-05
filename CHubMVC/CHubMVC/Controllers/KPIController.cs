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
        [Authorize]
        public ActionResult Index()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.cgldb.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [HttpPost]
        [Authorize]
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

        [HttpPost]
        [Authorize]
        public ActionResult GetLatestHistory(string kpiGroup)
        {
            try
            {
                DB_KPI_HISTORY_BLL hBLL = new DB_KPI_HISTORY_BLL();
                List<ExDBKPICode> codeList = hBLL.GetDistinctKPICode(kpiGroup);

                List<ExDBKPIHistory> result = hBLL.GetLatestHistory(codeList,kpiGroup);
                

                if (result != null)
                {
                    foreach (var r in result)
                    {
                        r.WEEK = DateHelper.GetWeekOfYear(r.KPI_DATE);
                        r.VALUE_COLOR = GetKPIValueColor(r);
                    }
                }

                var obj = new
                {
                    History = result,
                    Group = codeList
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetLatestHistory", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTrendData(string code,string subCode)
        {
            try
            {
                DB_KPI_HISTORY_BLL hBLL = new DB_KPI_HISTORY_BLL();
                List<DB_KPI_HISTORY> hList = hBLL.GetTrendData(code,subCode);
                if(hList == null || hList.Count==0)
                    return Json(new RequestResult(null));

                List<string> kpiDates = new List<string>();
                List<decimal> kpiValues = new List<decimal>();
                List<decimal> kpiTarget = new List<decimal>();
                foreach (var h in hList)
                {
                    kpiDates.Add(h.KPI_DATE.ToString("yyyy/MM/dd"));
                    kpiValues.Add(h.KPI_VALUE);
                    kpiTarget.Add(h.KPI_TARGET??0);
                }

                var obj = new
                {
                    kpiDates = kpiDates,
                    kpiValues = kpiValues,
                    kpiTarget = kpiTarget
                };
                return Json(new RequestResult(obj));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetTrendData", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        #region private function part
        private string GetKPIValueColor(ExDBKPIHistory data)
        {
            if (data.KPI_TARGET == null)
                return null;
            if (data.IND_Y == null)
                return null;
            //less is good 
            if (data.IND_Y > data.KPI_TARGET)
            {
                if (data.KPI_VALUE <= data.KPI_TARGET)
                    return CHubConstValues.GoodColor;
                else if (data.KPI_VALUE <= data.IND_Y)
                    return CHubConstValues.WarningColor;
                else
                    return CHubConstValues.ErrorColor;
            }
            //more is good
            else if (data.IND_Y < data.KPI_TARGET)
            {
                if (data.KPI_VALUE >= data.KPI_TARGET)
                    return CHubConstValues.GoodColor;
                else if (data.KPI_VALUE >= data.IND_Y)
                    return CHubConstValues.WarningColor;
                else
                    return CHubConstValues.ErrorColor;
            }
            //data.IND_Y == data.KPI_TARGET not exist , for in case
            else
                return CHubConstValues.GoodColor;
        }


        #endregion

    }
}
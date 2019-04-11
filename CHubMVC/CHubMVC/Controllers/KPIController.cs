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
using CHubModel.WebArg;
using System.Text;
using CHubDBEntity.UnmanagedModel;

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

                List<ExDBKPIHistory> result = hBLL.GetLatestHistory(codeList, kpiGroup);


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
        public ActionResult GetTrendData(string code, string subCode)
        {
            try
            {
                DB_KPI_HISTORY_BLL hBLL = new DB_KPI_HISTORY_BLL();
                List<DB_KPI_HISTORY> hList = hBLL.GetTrendData(code, subCode);
                if (hList == null || hList.Count == 0)
                    return Json(new RequestResult(null));

                List<string> kpiDates = new List<string>();
                List<decimal> kpiValues = new List<decimal>();
                List<decimal> kpiTarget = new List<decimal>();
                foreach (var h in hList)
                {
                    kpiDates.Add(h.KPI_DATE.ToString("yyyy/MM/dd"));
                    kpiValues.Add(h.KPI_VALUE);
                    kpiTarget.Add(h.KPI_TARGET ?? 0);
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


        [Authorize]
        public ActionResult KpiSet()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.kpiset.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetKpiCode(string AppUser)
        {
            KPISET_BLL ksBLL = new KPISET_BLL();
            try
            {
                var result = ksBLL.GetKpiCode(AppUser).Select(i => i.ORG_KPI).ToList();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("KPI GetKpiCode", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult KpiSetSearch(string ORG_KPI, string KPI_YEAR)
        {
            KPISET_BLL ksBLL = new KPISET_BLL();
            try
            {
                string ORG_ID = string.Empty;
                string KPI_CODE = string.Empty;
                if (!string.IsNullOrEmpty(ORG_KPI))
                {
                    var str = ORG_KPI.Split('-');
                    ORG_ID = str[0];
                    KPI_CODE = str[1];
                }
                var result = ksBLL.KpiSetSearch(ORG_ID, KPI_CODE, KPI_YEAR);
                var mainHtml = GetKpiSetHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("KPI KpiSetSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult KpiSetSave(List<KpiSetArg> list)
        {
            KPISET_BLL ksBLL = new KPISET_BLL();
            try
            {
                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        ksBLL.KpiSetSave(item);
                    }
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("KPI KpiSetSave");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetKpiSetHtml(List<DASH_KPI_HISTORY> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_YEAR' value='" + item.KPI_YEAR + "' title='" + item.KPI_YEAR + "' readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm ORG_ID' value='" + item.ORG_ID + "' title='" + item.ORG_ID + "' readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_CODE' value='" + item.KPI_CODE + "' title='" + item.KPI_CODE + "' readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_SUB_CODE' value='" + item.KPI_SUB_CODE + "' title='" + item.KPI_SUB_CODE + "' readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_DESC' value='" + item.KPI_DESC + "' title='" + item.KPI_DESC + "' readonly />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_01' value='" + item.KPI_VAL_01 + "' title='" + item.KPI_VAL_01 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_02' value='" + item.KPI_VAL_02 + "' title='" + item.KPI_VAL_02 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_03' value='" + item.KPI_VAL_03 + "' title='" + item.KPI_VAL_03 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_04' value='" + item.KPI_VAL_04 + "' title='" + item.KPI_VAL_04 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_05' value='" + item.KPI_VAL_05 + "' title='" + item.KPI_VAL_05 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_06' value='" + item.KPI_VAL_06 + "' title='" + item.KPI_VAL_06 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_07' value='" + item.KPI_VAL_07 + "' title='" + item.KPI_VAL_07 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_08' value='" + item.KPI_VAL_08 + "' title='" + item.KPI_VAL_08 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_09' value='" + item.KPI_VAL_09 + "' title='" + item.KPI_VAL_09 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_10' value='" + item.KPI_VAL_10 + "' title='" + item.KPI_VAL_10 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_11' value='" + item.KPI_VAL_11 + "' title='" + item.KPI_VAL_11 + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_VAL_12' value='" + item.KPI_VAL_12 + "' title='" + item.KPI_VAL_12 + "' />").Append("</td>");
                    //sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_TARGET' value='" + item.KPI_TARGET + "' title='" + item.KPI_TARGET + "' />").Append("</td>");
                    //sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KPI_TARGET_THRESH' value='" + item.KPI_TARGET_THRESH + "' title='" + item.KPI_TARGET_THRESH + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm NOTE' value='" + item.NOTE + "' title='" + item.NOTE + "' />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

    }
}
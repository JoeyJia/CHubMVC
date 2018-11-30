using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubDBEntity.UnmanagedModel;
using System.Data;
using CHubModel;
using CHubCommon;

namespace CHubMVC.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ChartDemo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMapChart()
        {
            Chart_BLL cBLL = new Chart_BLL();
            try
            {
                var result = cBLL.GetTMSData();
                return Json(new { Success = true, Data = result, Msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Chart GetMapChart");
                return Json(new { Success = false, Data = "", Msg = "Fail" + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Dash_Ship_Chart()
        {
            return View();
        }

        /// <summary>
        /// 图表1-中国地图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDashShipChart11()
        {
            Chart_BLL cBLL = new Chart_BLL();
            try
            {
                var result = cBLL.GetV_DASH_SHIP11();
                return Json(new { Success = true, Data = result, Msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Chart GetDashShipChart11", ex);
                return Json(new { Success = false, Data = "", Msg = "Fail" + ex.Message });
            }
        }
        /// <summary>
        /// 图表1-折线图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDashShipChart12()
        {
            Chart_BLL cBLL = new Chart_BLL();
            try
            {
                var result = cBLL.GetV_DASH_SHIP12();
                List<string> PERIOD = result.Select(i => i.PERIOD).Distinct().OrderBy(a => a).ToList();//x轴
                var SHData = GetDashShipChart12Data(result.Where(r => r.SHIP_FROM == "上海PDC").OrderBy(a => a.PERIOD).ToList());
                var BJData = GetDashShipChart12Data(result.Where(r => r.SHIP_FROM == "北京BJRDC").OrderBy(a => a.PERIOD).ToList());
                var SWData = GetDashShipChart12Data(result.Where(r => r.SHIP_FROM == "成都SWRDC").OrderBy(a => a.PERIOD).ToList());
                var SYData = GetDashShipChart12Data(result.Where(r => r.SHIP_FROM == "沈阳SYRDC").OrderBy(a => a.PERIOD).ToList());
                var XAData = GetDashShipChart12Data(result.Where(r => r.SHIP_FROM == "西安XARDC").OrderBy(a => a.PERIOD).ToList());
                return Json(new { Success = true, PERIOD= PERIOD, SHData = SHData, BJData = BJData, SWData = SWData, SYData = SYData, XAData = XAData, Msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Chart GetDashShipChart12", ex);
                return Json(new { Success = false, Data = "", Msg = "Fail" + ex.Message });
            }
        }

        public List<decimal> GetDashShipChart12Data(List<V_DASH_SHIP12> result)
        {
            List<decimal> list = new List<decimal>();
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    list.Add(item.SHIP_LINES);
                }
            }
            return list;
        }


        /// <summary>
        /// 图表2-世界地图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDashShipChart21()
        {
            Chart_BLL cBLL = new Chart_BLL();
            try
            {
                var result = cBLL.GetV_DASH_SHIP21();
                return Json(new { Success = true, Data = result, Msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Chart GetDashShipChart21", ex);
                return Json(new { Success = false, Data = "", Msg = "Fail" + ex.Message });
            }
        }


        /// <summary>
        /// 图表2-折线图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDashShipChart22()
        {
            Chart_BLL cBLL = new Chart_BLL();
            try
            {
                var result = cBLL.GetV_DASH_SHIP22();
                List<string> PERIOD = result.Select(i => i.PERIOD).OrderBy(a => a).ToList();
                List<decimal> SHIP_LINES = result.OrderBy(a => a.PERIOD).Select(i => i.SHIP_LINES).ToList();
                return Json(new { Success = true, PERIOD = PERIOD, SHIP_LINES = SHIP_LINES, Msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Chart GetDashShipChart22", ex);
                return Json(new { Success = false, Data = "", Msg = "Fail" + ex.Message });
            }
        }
    }
}
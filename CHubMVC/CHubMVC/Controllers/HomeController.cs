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
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if(Session[CHubConstValues.SessionUser]==null)
                return RedirectToAction("Login", "Account");

            string userName = Session[CHubConstValues.SessionUser].ToString();

            //Get Welcome Part
            APP_WELCOME_BLL welBLL = new APP_WELCOME_BLL();
            ViewBag.welcomeList = welBLL.GetAppWelcome();

            //Recently page
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            ViewBag.recentList = rpBLL.GetRecentPages(userName);

            //Get Notice Part
            APP_NOTICE_BLL noticeBLL = new APP_NOTICE_BLL();
            ViewBag.noticeList = noticeBLL.GetValidAppNotice();

            ViewBag.pageList = GetPages(userName);

            ViewBag.userName = userName;

            return View();
        }


        public List<PageList> GetPages(string appUser)
        {
            V_USER_NAV_ALL_BLL navBLL = new V_USER_NAV_ALL_BLL();
            List<V_USER_NAV_ALL> navList = new List<V_USER_NAV_ALL>();
            navList = navBLL.GetNavByAppUser(appUser);

            List<PageList> pls = new List<PageList>();
            string space_desc = string.Empty;
            navList.GroupBy(i => i.SPACE_DESC).ToList().ForEach(a => {
                PageList pl = new PageList();
                pl.SPACE_DESC = a.Key;
                pl.pages = new List<PageList>();
                foreach (var item in a)
                {
                    PageList pp = new PageList();
                    pp.SPACE_DESC = item.SPACE_DESC;
                    pp.URL = item.URL;
                    pp.DISPLAY = item.DISPLAY;
                    pp.DESCRIPTION = item.DESCRIPTION;
                    pp.ICON = item.ICON;
                    pp.ICON_DESC = item.ICON_DESC;
                    pl.pages.Add(pp);
                }
                pls.Add(pl);
            });
            #region OLD
            //var navs = navList.GroupBy(i => i.SPACE_DESC).Select(a =>a).ToList();
            //foreach (var i in navs)
            //{
            //    PageList pl = new PageList();
            //    pl.SPACE_DESC = i.Key;
            //    pl.pages = new List<PageList>();
            //    foreach (var j in i)
            //    {
            //        PageList pp = new PageList();
            //        pp.SPACE_DESC = j.SPACE_DESC;
            //        pp.URL = j.URL;
            //        pp.DISPLAY = j.DISPLAY;
            //        pp.DESCRIPTION = j.DESCRIPTION;
            //        pp.ICON = j.ICON;
            //        pp.ICON_DESC = j.ICON_DESC;
            //        pl.pages.Add(pp);
            //    }
            //    pls.Add(pl);
            //}
            #endregion
            return pls;
        }




        /// <summary>
        /// Use session user to get nav tree data
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetLeftNav(string appUser)
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            appUser = Session[CHubConstValues.SessionUser].ToString();

            V_USER_NAV_ALL_BLL navBLL = new V_USER_NAV_ALL_BLL();
            List<V_USER_NAV_ALL> navList = new List<V_USER_NAV_ALL>();
            navList = navBLL.GetNavByAppUser(appUser);

            //Const have 2 level tree deep
            List<TreeNode> nodeList = new List<TreeNode>();
            for (int i = 0; i < navList.Count; )
            {
                TreeNode tn = new TreeNode();
                tn.text = navList[i].SPACE_DESC;
                tn.selectable = true;

                //if (i + 1 < navList.Count && navList[i + 1].DISPLAY_SEQ == navList[i].DISPLAY_SEQ)
                //{
                    tn.nodes = new List<TreeNode>();
                    int offset = 0;
                    while (true)
                    {

                        TreeNode childNode = new TreeNode();
                        childNode.text = navList[i + offset].DISPLAY;
                        childNode.selectable = true;
                        childNode.href = navList[i + offset].URL;
                        tn.nodes.Add(childNode);
                        offset++;
                        if ((i + offset) >= navList.Count || navList[i + offset].DISPLAY_SEQ != navList[i].DISPLAY_SEQ)
                            break;
                    }
                    i = i + offset;
                //}
                //else
                //{
                //    tn.href =navList[i].URL;
                   // i++;
                //}
                nodeList.Add(tn);
            }

            return Json(nodeList);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetApplicationLink()
        {
            List<M_APPS> apps = new List<M_APPS>();
            M_APPS_BLL BLL = new M_APPS_BLL();
            apps = BLL.GetMAppList();
            return Json(apps);
        }

        [Authorize]
        public ActionResult About()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Message = "Your application description page.";


            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Message = "Your contact page.";


            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult LogPageClick(string URL)
        {
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            try
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                string PAGE_NAME = rpBLL.GetPAGE_NAME(URL);

                rpBLL.AddPageLog(appUser, PAGE_NAME);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home LogPageClick", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

    }
}
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

            ViewBag.userName = userName;

            return View();
        }

        /// <summary>
        /// Use session user to get nav tree data
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
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

        public ActionResult About()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Message = "Your application description page.";


            return View();
        }

        public ActionResult Contact()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Message = "Your contact page.";


            return View();
        }
    }
}
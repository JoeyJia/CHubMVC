using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubDBEntity;
using CHubModel;

namespace CHubMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetLeftNav(string appUser)
        {
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

                if (i + 1 < navList.Count && navList[i + 1].DISPLAY_SEQ == navList[i].DISPLAY_SEQ)
                {
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
                }
                else
                {
                    tn.href =navList[i].URL;
                    i++;
                }
                nodeList.Add(tn);
            }

            return Json(nodeList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
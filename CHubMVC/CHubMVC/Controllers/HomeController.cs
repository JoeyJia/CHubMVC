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
using System.Text;
using CHubDBEntity.UnmanagedModel;

namespace CHubMVC.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Account");

            string userName = Session[CHubConstValues.SessionUser].ToString();

            //Get Welcome Part
            APP_WELCOME_BLL welBLL = new APP_WELCOME_BLL();
            ViewBag.welcomeList = welBLL.GetAppWelcome();
            ViewBag.ihubEnv = welBLL.GetAppEnv().First().IHUB_ENV;

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
            navList.GroupBy(i => i.SPACE_DESC).ToList().ForEach(a =>
            {
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
            for (int i = 0; i < navList.Count;)
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

        [Authorize]
        public ActionResult UsrMnt()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, "usrmnt", this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntSearch(string APP_USER)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                var result = bll.UsrMntSearch(APP_USER.ToLower());
                var mainHmtl = GetUsrMntHtml(result);
                return Json(new RequestResult(mainHmtl));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntSave(APP_USERS arg)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                bll.UsrMntSave(arg);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntRoles(string APP_USER)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                var result = bll.UsrMntRoles(APP_USER);
                var mainHtml = GetUsrMntRolesHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntRoles", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntSecurity(string APP_USER)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                var result = bll.UsrMntSecurity(APP_USER);
                var mainHtml = GetUsrMntSercurityHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntSecurity", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntRolesNew(string APP_USER)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                var result = bll.UsrMntRolesNew(APP_USER);
                var mainHtml = GetUsrMntRolesNewHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntRolesNew", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntSecurityNew(string APP_USER)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                var result = bll.UsrMntSecurityNew(APP_USER);
                var mainHmtl = GetUsrMntSecurityNewHtml(result);
                return Json(new RequestResult(mainHmtl));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntSecurityNew", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntRolesSave(List<APP_USER_ROLE_LINK> list)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        bll.UsrMntRolesSave(item);
                    }
                    var result = bll.UsrMntRoles(list.First().APP_USER);
                    var mainHtml = GetUsrMntRolesHtml(result);
                    return Json(new RequestResult(mainHtml));
                }
                else
                    return Json(new RequestResult(false, "No data save"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntRolesSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntSecuritySave(List<APP_SECURE_PROC_ASSIGN> list)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        bll.UsrMntSecuritySave(item);
                    }
                    var result = bll.UsrMntSecurity(list.First().APP_USER);
                    var mainHtml = GetUsrMntSercurityHtml(result);
                    return Json(new RequestResult(mainHtml));
                }
                else
                    return Json(new RequestResult(false, "No data save"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntSecuritySave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult UsrMntRolesDelete(APP_USER_ROLE_LINK arg)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                bll.UsrMntRolesDelete(arg);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntRolesDelete", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntSecurityDelete(APP_SECURE_PROC_ASSIGN arg)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                bll.UsrMntSecurityDelete(arg);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntSecurityDelete", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UsrMntRolesRole_NameChange(string ROLE_NAME)
        {
            APP_USERS_BLL bll = new APP_USERS_BLL();
            try
            {
                var result = bll.UsrMntRolesRole_NameChange(ROLE_NAME);
                return Json(new RequestResult(result.DESCRIPTION));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home UsrMntRolesRole_NameChange", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public string GetUsrMntHtml(List<APP_USERS> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.APP_USER).Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm FIRST_NAME' value='" + item.FIRST_NAME + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm LAST_NAME' value='" + item.LAST_NAME + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm DESCRIPTION' value='" + item.DESCRIPTION + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm PHONE' value='" + item.PHONE + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm EMAIL_ADDR' value='" + item.EMAIL_ADDR + "' />").Append("</td>");
                    sb.Append("     <td>").Append("<select class='form-control input-sm STATUS'>").Append(GetUsrMntStatus(item.STATUS)).Append("</select>").Append("</td>");
                    sb.Append("     <td>")
                        .Append("<input type='button' class='btn btn-primary btn-xs btnSave' value='SAVE' data-appuser='" + item.APP_USER + "' />")
                        .Append("<input type='button' class='btn btn-primary btn-xs btnRoles' value='ROLES' data-appuser='" + item.APP_USER + "' />")
                        .Append("<input type='button' class='btn btn-primary btn-xs btnSecurity' value='Security PROC' data-appuser='" + item.APP_USER + "' />")
                        .Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetUsrMntRolesHtml(List<APP_USER_ROLE_LINK> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.ROLE_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.COMMENTS).Append("</td>");
                    sb.Append("     <td>").Append(item.CREATE_DATE.HasValue ? item.CREATE_DATE.Value.ToString("yyyy/MM/dd") : "").Append("</td>");
                    sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-xs btnRolesDelete' value='delete' data-rolename='" + item.ROLE_NAME + "' />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetUsrMntSercurityHtml(List<APP_SECURE_PROC_ASSIGN> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.SECURE_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.COMMENTS).Append("</td>");
                    sb.Append("     <td>").Append(item.ACTIVEIND).Append("</td>");
                    sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-xs btnSecurityDelete' value='delete' data-securityid='" + item.SECURE_ID + "' />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

        public string GetUsrMntRolesNewHtml(List<APP_ROLES> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                sb.Append(" <tr>");
                sb.Append("     <td>");
                sb.Append("         <select class='form-control input-sm ROLE_NAME'>");
                sb.Append("             <option value=''>").Append("</option>");
                foreach (var item in list)
                {
                    sb.Append("         <option value='" + item.ROLE_NAME + "'>").Append(item.ROLE_NAME + " (" + item.DESCRIPTION + ")").Append("</option>");
                }
                sb.Append("         </select>");
                sb.Append("     </td>");
                sb.Append("     <td>").Append("<input type='text' class='form-control input-sm COMMENTS' />").Append("</td>");
                sb.Append("     <td>").Append(DateTime.Now.ToString("yyyy/MM/dd")).Append("</td>");
                sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-xs btnRolesNewDelete' value='delete' />").Append("</td>");
                sb.Append(" </tr>");
            }
            return sb.ToString();
        }

        public string GetUsrMntSecurityNewHtml(List<APP_SECURE_PROCESS> list)
        {
            StringBuilder sb = new StringBuilder();
            //List<string> ACTIVEINDs = new List<string>() { "Y", "N" };
            if (list != null && list.Any())
            {
                sb.Append(" <tr>");
                sb.Append("     <td>");
                sb.Append("         <select class='form-control input-sm SECURE_ID'>");
                sb.Append("             <option value=''>").Append("</option>");
                foreach (var item in list)
                {
                    sb.Append("         <option value='" + item.SECURE_ID + "'>").Append(item.SECURE_ID + " (" + item.SECURE_DESC + ")").Append("</option>");
                }
                sb.Append("         </select>");
                sb.Append("     </td>");
                sb.Append("     <td>").Append("<input type='text' class='form-control input-sm COMMENTS' />").Append("</td>");
                sb.Append("     <td>");
                sb.Append("         <select class='form-control input-sm ACTIVEIND'>");
                sb.Append("             <option value='Y' selected>").Append("Y").Append("</option>");
                sb.Append("             <option value='N'>").Append("N").Append("</option>");
                sb.Append("         </select>");
                sb.Append("     </td>");
                sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-xs btnSecurityNewDelete' value='delete' />").Append("</td>");
                sb.Append(" </tr>");
            }
            return sb.ToString();
        }

        public string GetUsrMntStatus(string STATUS)
        {
            StringBuilder sb = new StringBuilder();
            string[] status = new string[] { "A", "I" };

            foreach (var s in status)
            {
                if (STATUS == s)
                    sb.Append("<option value='" + s + "' selected>").Append(s).Append("</option>");
                else
                    sb.Append("<option value='" + s + "'>").Append(s).Append("</option>");
            }
            return sb.ToString();
        }



        public ActionResult IhubJob()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, "ihubjob", this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetJOB_DISPLAY(string App_User)
        {
            IhubJob_BLL jBLL = new IhubJob_BLL();
            try
            {
                var result = jBLL.GetJOB_DISPLAY(App_User);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home GetJOB_DISPLAY", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetJOB_DESC(string JOB_NAME, string App_User)
        {
            IhubJob_BLL jBLL = new IhubJob_BLL();
            try
            {
                var result = jBLL.GetJOB_DESC(JOB_NAME, App_User);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home GetJOB_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult IhubJobRun(V_JOB_LINK_USER arg, List<string> paras)
        {
            IhubJob_BLL jBLL = new IhubJob_BLL();

            try
            {
                //执行Proc
                jBLL.ExecuteProc(arg, paras);
                //记录执行结果
                jBLL.LogHistory(arg, "");
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Home IhubJobRun", ex);
                jBLL.LogHistory(arg, ex.Message);
                return Json(new RequestResult(false, ex.Message));
            }
        }


    }
}
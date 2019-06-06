using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubDBEntity;
using CHubModel;
using CHubCommon;
using CHubMVC.Models;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using CHubModel.WebArg;
using System.Configuration;

namespace CHubMVC.Controllers
{
    public class MobileController : MobileBaseController
    {
        private static string webUrl = ConfigurationManager.AppSettings["LBScan"].ToString();
        private static string webType = ConfigurationManager.AppSettings["WebType"].ToString();

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            APP_USERS appUser = DomainUserAuth.IsAuthenticated("CED", model.UserName, model.Password);
            if (appUser != null)
            {
                //FormsAuthentication.SetAuthCookie(model.UserName, true);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, appUser.FIRST_NAME));
                claims.Add(new Claim(ClaimTypes.Email, appUser.EMAIL_ADDR));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, model.UserName));

                ClaimsIdentity cIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, cIdentity);

                APP_USERS_BLL userBLL = new APP_USERS_BLL();
                APP_USERS user = userBLL.GetAppUserByDomainName(model.UserName);

                if (user == null)
                {
                    if (!userBLL.AddAppUserWithRole(model.UserName, appUser.FIRST_NAME, appUser.EMAIL_ADDR))
                        throw new Exception("Fail to add App User");
                    user = userBLL.GetAppUserByDomainName(model.UserName);
                }
                Session[CHubConstValues.SessionUser] = user.APP_USER;
                Session[CHubConstValues.FirstName] = user.FIRST_NAME;
                LogHelper.WriteLog("log in action, user: " + user.APP_USER);
                return RedirectToAction("Home", "Mobile");
            }
            return View();
        }


        public ActionResult Home()
        {
            if (Session[CHubConstValues.SessionUser] == null)
                return RedirectToAction("Login", "Mobile");

            string userName = Session[CHubConstValues.SessionUser].ToString();

            //Get Welcome Part
            APP_WELCOME_BLL welBLL = new APP_WELCOME_BLL();
            ViewBag.welcomeList = welBLL.GetAppWelcome();

            V_USER_NAV_ALL_BLL userBLL = new V_USER_NAV_ALL_BLL();
            var result = userBLL.GetMobilePageByAppUser(userName);
            ViewBag.pageList = GetPageList(result);


            ViewBag.FIRST_NAME = Session[CHubConstValues.FirstName];
            return View();
        }

        public List<MobilePageArg> GetPageList(List<V_USER_NAV_ALL> user)
        {
            List<MobilePageArg> list = new List<MobilePageArg>();

            var space = user.Select(u => u.SPACE_DESC).Distinct();

            foreach (var item in space)
            {
                MobilePageArg ma = new MobilePageArg();
                ma.pagelist = new List<MobilePageList>();
                ma.SPACE_DESC = item;
                var icon = user.Where(u => u.SPACE_DESC == item);
                foreach (var ic in icon)
                {
                    ma.pagelist.Add(new MobilePageList()
                    {
                        URL = ic.URL,
                        ICON = ic.ICON,
                        ICON_DESC = ic.ICON_DESC
                    });
                }
                list.Add(ma);
            }
            return list;
        }


        public ActionResult LogOff()
        {
            Session[CHubConstValues.SessionUser] = null;
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Mobile");
        }


        public ActionResult IAScan_M(string LODNUM_DISPLAY)
        {
            ViewBag.LODNUM_DISPLAY = LODNUM_DISPLAY;
            return View();
        }

        public ActionResult IAScanTest_M()
        {
            return View();
        }


        public ActionResult IAToday_M()
        {
            return View();
        }


        public ActionResult LBScan_M()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbprt2.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            ViewBag.Url = webUrl;
            ViewBag.Type = webType;
            return View();
        }

        [HttpPost]
        public ActionResult GetADRNAM(string LODNUM)
        {
            V_PLABEL_BY_MOBILE_PRINT_BLL vBLL = new V_PLABEL_BY_MOBILE_PRINT_BLL();
            try
            {
                var result = vBLL.GetADRNAM(LODNUM);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MOBILE GetADRNAM", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }
        [HttpPost]
        public ActionResult RunPRE_WORK_MOBILE_PRINT(string WH_ID, string LODNUM)
        {
            V_PLABEL_BY_MOBILE_PRINT_BLL vBLL = new V_PLABEL_BY_MOBILE_PRINT_BLL();
            try
            {
                vBLL.RunPRE_WORK_MOBILE_PRINT(WH_ID, LODNUM);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MOBILE RunPRE_WORK_MOBILE_PRINT", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult RunProcAndSelectInfo(string WH_ID, string LODNUM, string PRTNUM, string LABEL_CODE)
        {
            V_PLABEL_BY_MOBILE_PRINT_BLL vBLL = new V_PLABEL_BY_MOBILE_PRINT_BLL();
            try
            {
                //当LODNUM为空的时候执行Proc PRE_WORK_MOBILE_UnCatalog
                if (string.IsNullOrEmpty(LODNUM))
                    vBLL.RunPRE_WORK_MOBILE_UnCatalog(WH_ID, PRTNUM);

                //查询当前数据，只有一条
                var result = vBLL.GetV_PLABEL_BY_MOBILE_PRINT(WH_ID, LODNUM, PRTNUM, LABEL_CODE);
                if (result == null)
                    return Json(new RequestResult(true, "Empty", "PART NO EXSIT!"));
                else
                    return Json(new RequestResult(true, "", result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MOBILE RunProcAndSelectInfo", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

    }
}
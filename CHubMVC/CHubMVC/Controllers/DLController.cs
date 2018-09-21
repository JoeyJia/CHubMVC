using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel;
using CHubBLL;
using System.Text;
using System.Data;
using System.IO;

namespace CHubMVC.Controllers
{
    public class DLController : BaseController
    {
        public ActionResult IHubLoad()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.ihubload.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }


    }
}
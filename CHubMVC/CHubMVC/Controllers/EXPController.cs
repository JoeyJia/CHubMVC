using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubDBEntity.UnmanagedModel;
using CHubBLL;
using System.Web.Mvc;
using CHubCommon;

namespace CHubMVC.Controllers
{
    public class EXPController : BaseController
    {
        public ActionResult EXPWB()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.expwb.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }
    }
}
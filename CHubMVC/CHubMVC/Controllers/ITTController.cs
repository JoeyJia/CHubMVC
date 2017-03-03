using CHubBLL;
using CHubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHubMVC.Controllers
{
    public class ITTController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            
            //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            //rpBLL.Add(appUser, CHubEnum.PageNameEnum.tcmnt.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }
    }
}
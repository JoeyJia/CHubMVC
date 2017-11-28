using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHubMVC.Controllers
{
    public class IAController : BaseController
    {
        [Authorize]
        public ActionResult IACode()
        {
            return View();
        }

    }
}
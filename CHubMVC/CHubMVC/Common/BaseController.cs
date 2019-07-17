using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CHubCommon;
using CHubDBEntity;

namespace CHubMVC
{
    public class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    base.OnActionExecuting(filterContext);
        //    if (System.Web.HttpContext.Current.Session[CHubConstValues.SessionUser] == null)
        //    {
        //        filterContext.Result = Redirect("/Account/Login");
        //    }
        //    return;
        //}
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Session[CHubConstValues.SessionUser] == null)
            {
                filterContext.HttpContext.Response.Redirect("/Account/Login");
            }
        }

        public string AppUser
        {
            get
            {
                return Session[CHubConstValues.SessionUser].ToString();
            }
        }

    }
}
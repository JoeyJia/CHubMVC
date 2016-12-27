using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHubMVC
{
    public class JsonNetResult:JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                //solve loop issue             
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {try
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("JSON GET is not allowed");
                HttpResponseBase response = context.HttpContext.Response;
                response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
                if (this.ContentEncoding != null)
                    response.ContentEncoding = this.ContentEncoding;
                if (this.Data == null)
                    return;
                var scriptSerializer = JsonSerializer.Create(this.Settings);
                using (var sw = new StringWriter())
                {
                    scriptSerializer.Serialize(sw, this.Data);
                    response.Write(sw.ToString());
                }
            }
            catch (Exception ee)
            {
                string ss = ee.Message;
            }
        }
    }
}
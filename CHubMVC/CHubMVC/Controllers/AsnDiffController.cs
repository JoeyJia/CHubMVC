using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubModel;
using CHubBLL;
using CHubCommon;
using CHubModel.WebArg;
using CHubDBEntity;
using CHubMVC.Models;
using CHubBLL.OtherProcess;
using System.Threading;
using CHubDBEntity.UnmanagedModel;

namespace CHubMVC.Controllers
{
    public class AsnDiffController : BaseController
    {

       
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AsnDiff(AsnDiffArg asndif)
        {
            XX_ASN_DIFF_BLL bll = new XX_ASN_DIFF_BLL();
            var result = bll.GetAsnDiffListBySearch(asndif);
            return View(result);
        }

        public ActionResult UpdateAsn(XX_ASN_DIFF asnmodel)
        {

            XX_ASN_DIFF_BLL bll = new XX_ASN_DIFF_BLL();
            var result = bll.UpdateXXAsnDiff(asnmodel);
            return View(result);
        }

        public bool UpdateAsnRemark(string asnid,string remark)
        {
            XX_ASN_DIFF_BLL bll = new XX_ASN_DIFF_BLL();
            return bll.UpdateXXAsnDiffRemark(asnid, remark);
        }

        public string SaveAsnDiff(string warehouse, string asnid)
        {
            XX_ASN_DIFF_BLL bll = new XX_ASN_DIFF_BLL();
            string res = string.Empty;
            bll.SaveAsnDiff(warehouse, asnid, out res);
            return res;
        }
    }
}
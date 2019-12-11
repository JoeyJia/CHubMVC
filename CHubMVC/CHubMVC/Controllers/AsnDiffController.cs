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
using System.Text;

namespace CHubMVC.Controllers
{
    public class AsnDiffController : BaseController
    {
        [HttpPost]
        public ActionResult GetResultTypeList()
        {
            XX_ASNDIFF_RESULT_CODE_BLL mpBLL = new XX_ASNDIFF_RESULT_CODE_BLL();
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var result = mpBLL.GetXXASNDIFFRESULTCODEList();
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        list.Add(new SelectListItem() { Value = item.DIFFResult_Code, Text = item.Result_Code_Desc});
                    }
                }
                return Json(new RequestResult(list));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("AsnDiff GetResultTypeList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AsnDiff(AsnDiffArg arg)
        {
            bool showMore = true;
            XX_ASN_DIFF_BLL bll = new XX_ASN_DIFF_BLL();
            var result = bll.GetAsnDiffListBySearch(arg);
            var mainHtml = GetMP_MAINHtml(result);
            return Json(new RequestResult(true, showMore ? "" : "End", mainHtml));
        }
        public ActionResult GetAsnDiffInfoById(long asndiffid)
        {

            XX_ASN_DIFF_BLL bll = new XX_ASN_DIFF_BLL();
            var result = bll.GetAsnDiffById(asndiffid);
            return Json(new RequestResult(true, "", result));
        }
        public string GetMP_MAINHtml(List<XX_ASN_DIFF> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.ASN_DIFF_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.WAREHOUSE).Append("</td>");
                    sb.Append("     <td>").Append(item.MANIFEST_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.PRINT_PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.CREATE_DATE).Append("</td>");
                    sb.Append("     <td>").Append(item.DISP_ACTION).Append("</td>");
                    sb.Append("     <td>").Append(item.CLAIM_RESULT).Append("</td>");
                    sb.Append("     <td>").Append(item.CLOSE_DATE).Append("</td>");

                    sb.Append("     <td>");
                    if (item.IS_CLOSE == "1")
                    {
                        sb.Append("<input type='button' class='btn btn-primary btn-xs btnDetail' value='LINES' data-sono='" + item.ASN_DIFF_ID + "' />");

                    }
                    else
                    {
                        sb.Append("<input type='button' class='btn btn-primary btn-xs btnEdit' value='Edit' data-sono='" + item.ASN_DIFF_ID + "' />");

                    }
                    sb.Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
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
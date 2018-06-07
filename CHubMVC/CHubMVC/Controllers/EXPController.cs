using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubDBEntity.UnmanagedModel;
using CHubBLL;
using System.Web.Mvc;
using CHubCommon;
using CHubModel;
using System.Text;
using CHubModel.WebArg;
using System.IO;
using System.Data;

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

        [HttpPost]
        public ActionResult GetSHIP_TO_LOCATION()
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.GetSHIP_TO_LOCATION();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP GetSHIP_TO_LOCATION", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetSHIP_TO_ALIAS(string SHIP_TO_LOCATION)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.GetSHIP_TO_ALIAS(SHIP_TO_LOCATION);
                return Json(new RequestResult(result.First().SHIP_TO_ALIAS));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP GetSHIP_TO_ALIAS", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ExecP_Load_Stg_from_RP(string SHIP_TO_LOCATION)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                eBLL.ExecP_Load_Stg_from_RP(SHIP_TO_LOCATION);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP ExecP_Load_Stg_from_RP", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetSHIP_TO_INDEX()
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.GetSHIP_TO_INDEX();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP GetSHIP_TO_INDEX", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        public ActionResult SearchV_EXP_STAGE_UNINVOICED(string SHIP_TO_INDEX)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.SearchV_EXP_STAGE_UNINVOICED(SHIP_TO_INDEX);
                string mainHtml = GetSearchHTML(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP SearchV_EXP_STAGE_UNINVOICED", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult SearchDetailsByEXP_STG_D(string LODNUM)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.SearchDetailsByEXP_STG_D(LODNUM);
                string mainHtml = GetSearchDetailHTML(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP SearchDetailsByEXP_STG_D", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        public ActionResult CheckSecurityOfbtnCreateInv()
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                //Securite Check
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                if (eBLL.CheckSecurityOfbtnCreateInv("EXP_COMM_INV", appUser))
                {
                    var result = eBLL.GetCOMM_INV_ID();//序列号：SEQ_COMM_INV （用户不可修改）
                    return Json(new RequestResult(result));
                }
                else
                    return Json(new RequestResult(false, "You cannot Operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP CheckSecurityOfbtnCreateInv", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult AddEXP_COMM_INV(EXPCOMMINVArg COMM, List<EXPSTGHArg> STG)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                //添加新纪录到 EXP_COMM_INV
                COMM.CREATED_BY = Session[CHubConstValues.SessionUser].ToString();
                eBLL.AddEXP_COMM_INV(COMM);
                //修改 table EXP_STG_H ( COMM_INV_ID) 按照 关键字 （LODNUM）
                var COMM_INV_ID = COMM.COMM_INV_ID;
                foreach (var item in STG)
                {
                    eBLL.ChangeEXP_STG_H(COMM_INV_ID, item.LODNUM);
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP AddEXP_COMM_INV", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        public ActionResult UploadEXPFile()
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                HttpPostedFileBase hpf = Request.Files[0];
                string tempGuid = Guid.NewGuid().ToString();
                string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                FileInfo folder = new FileInfo(folderPath);
                if (!Directory.Exists(folder.FullName))
                    Directory.CreateDirectory(folder.FullName);

                string fileFullName = folder.FullName + tempGuid + ".xls";
                hpf.SaveAs(fileFullName);

                NPOIExcelHelper excelHelper = new NPOIExcelHelper(fileFullName);
                DataTable dt = excelHelper.ExcelToDataTable();
                System.IO.File.Delete(fileFullName);

                if (dt == null && dt.Rows.Count == 0)
                    return Content("No data in excel");

                List<EXP_STG_LOAD> expList = ClassConvert.DataTableToList<EXP_STG_LOAD>(dt);
                if (expList == null || expList.Count == 0)
                    return Content("Wrong excel strut");

                //每批导入取一次 SEQ_STG_LOAD
                var LOAD_BATCH = eBLL.GetSEQ_STG_LOAD();
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                foreach (var item in expList)
                {
                    eBLL.AddEXP_STG_LOAD(item, LOAD_BATCH, appUser);
                }
                return Json(new RequestResult(true, "Successfully"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP UploadEXPFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadEXPFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.EXPExcelTemplateName;
            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        public string GetSearchHTML(List<V_EXP_STAGE_UNINVOICED> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append("<input type=\"checkbox\" class=\"selectCheckbox\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.PICKLIST_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.STCUST).Append("</td>");
                    sb.Append("         <td>").Append(item.CARRIER_CODE).Append("</td>");
                    sb.Append("         <td>").Append(item.SHIP_ID_STG).Append("</td>");
                    sb.Append("         <td>").Append(item.LODNUM).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_VOL).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_PALWGT).Append("</td>");
                    sb.Append("         <td>").Append(CallFunc_GET_EXP_EST_AMT(item.LODNUM)).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDetail\" value=\"Details\" data-lodnum=\"" + item.LODNUM + "\" />").Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }
        public string GetSearchDetailHTML(List<EXP_STG_D> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append(item.LODNUM).Append("</td>");
                    sb.Append("         <td>").Append(item.PRTNUM).Append("</td>");
                    sb.Append("         <td>").Append(item.UNTQTY).Append("</td>");
                    sb.Append("         <td>").Append(item.ORGCOD).Append("</td>");
                    sb.Append("         <td>").Append(item.HOST_EXT_ID).Append("</td>");
                    sb.Append("         <td>").Append(item.ORDLIN).Append("</td>");
                    sb.Append("         <td>").Append(item.SHIP_ID_STG).Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }

        public string CallFunc_GET_EXP_EST_AMT(string LODNUM)
        {
            string result = string.Empty;
            EXPWB_BLL eBLL = new EXPWB_BLL();
            result = eBLL.CallFunc_GET_EXP_EST_AMT(LODNUM);
            return result;
        }


        public ActionResult CINVINQ()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.cinvinq.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [HttpPost]
        public ActionResult SearchEXP_COMM_INV(string COMM_INV_ID, string SHIP_TO_INDEX, string CREATE_DATE, string CREATED_BY)
        {
            CINVINQ_BLL cBLL = new CINVINQ_BLL();
            try
            {
                var result = cBLL.SearchEXP_COMM_INV(COMM_INV_ID, SHIP_TO_INDEX, CREATE_DATE, CREATED_BY);
                string mainHtml = GetSearchEXP_COMM_INVHTML(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP SearchEXP_COMM_INV", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult CheckSecurity()
        {
            CINVINQ_BLL cBLL = new CINVINQ_BLL();
            try
            {
                string SECURE_ID = "EXP_COMM_INV";
                string APP_USER = Session[CHubConstValues.SessionUser].ToString();
                var bo = cBLL.CheckSecurity(SECURE_ID, APP_USER);
                if (bo)
                    return Json(new RequestResult(true));
                else
                    return Json(new RequestResult(false));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP CheckSecurity", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ExecP_EXP_INV_DISCARD(string COMM_INV_ID)
        {
            CINVINQ_BLL cBLL = new CINVINQ_BLL();
            try
            {
                cBLL.ExecP_EXP_INV_DISCARD(COMM_INV_ID);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP ExecP_EXP_INV_DISCARD", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ExecP_EXP_INV_COMP(string COMM_INV_ID)
        {
            CINVINQ_BLL cBLL = new CINVINQ_BLL();
            try
            {
                cBLL.ExecP_EXP_INV_COMP(COMM_INV_ID);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP ExecP_EXP_INV_COMP", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult SearchDetailsByV_EXP_STAGE_BASE(string COMM_INV_ID)
        {
            CINVINQ_BLL cBLL = new CINVINQ_BLL();
            try
            {
                var result = cBLL.SearchDetailsByV_EXP_STAGE_BASE(COMM_INV_ID);
                string mainHtml = GetSearchDetailsByV_EXP_STAGE_BASEHTML(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP SearchDetailsByV_EXP_STAGE_BASE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public string GetSearchEXP_COMM_INVHTML(List<EXP_COMM_INV> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append(item.COMM_INV_ID).Append("</td>");
                    sb.Append("         <td>").Append(item.SHIP_TO_INDEX).Append("</td>");
                    sb.Append("         <td>").Append(item.CREATED_BY).Append("</td>");
                    sb.Append("         <td>").Append(item.CREATE_DATE).Append("</td>");
                    sb.Append("         <td>").Append(item.TOTAL_WGT).Append("</td>");
                    sb.Append("         <td>").Append(item.TOTAL_VOL).Append("</td>");
                    sb.Append("         <td>").Append(item.TOTAL_AMT).Append("</td>");
                    sb.Append("         <td>").Append(item.BOXES).Append("</td>");
                    sb.Append("         <td>").Append(item.COMM_DESC).Append("</td>");
                    GetStatusColor(sb, item.COMM_STATUS);
                    GetButtonDisplay(sb, item.COMM_STATUS, item.COMM_INV_ID.ToString());
                    sb.Append("     </tr>");
                }
            }

            return sb.ToString();
        }
        public string GetSearchDetailsByV_EXP_STAGE_BASEHTML(List<V_EXP_STAGE_BASE> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append(item.PICKLIST_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.STCUST).Append("</td>");
                    sb.Append("         <td>").Append(item.SHIP_ID_STG).Append("</td>");
                    sb.Append("         <td>").Append(item.LODNUM).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_PALWGT).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_PALLEN).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_PALWID).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_PALHGT).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_VOL).Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }

        public StringBuilder GetStatusColor(StringBuilder sb, string COMM_STATUS)
        {
            string color = string.Empty;
            switch (COMM_STATUS)
            {
                case "DRAFT":
                    color = "#FFC125";
                    break;
                case "COMP":
                    color = "green";
                    break;
                case "DISCARD":
                    color = "red";
                    break;
            }
            sb.Append("<td style=\"color:" + color + ";\">").Append(COMM_STATUS).Append("</td>");
            return sb;
        }

        public StringBuilder GetButtonDisplay(StringBuilder sb, string COMM_STATUS, string COMM_INV_ID)
        {
            string detailDisplay = string.Empty; string completeDisplay = string.Empty; string discardDisplay = string.Empty;
            if (COMM_STATUS == "DISCARD")
                detailDisplay = "disabled";
            if (COMM_STATUS != "DRAFT")
            {
                completeDisplay = "disabled";
                discardDisplay = "disabled";
            }
            sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDetail\" value=\"DETAILS\" data-comminvid=\"" + COMM_INV_ID + "\" " + detailDisplay + " />")
                                      .Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnComplete\" value=\"COMPLETE\" data-comminvid=\"" + COMM_INV_ID + "\" " + completeDisplay + " />")
                                      .Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDiscard\" value=\"DISCARD\" data-comminvid=\"" + COMM_INV_ID + "\" " + discardDisplay + " />");
            return sb;
        }


    }
}
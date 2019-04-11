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
        [Authorize]
        public ActionResult EXPWB()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.expwb.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult SearchV_EXP_STAGE_UNINVOICED(string SHIP_TO_INDEX)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.SearchV_EXP_STAGE_UNINVOICED(SHIP_TO_INDEX);
                string mainHtml = GetSearchHTML(result);
                var ordtyp = result.Select(a => a.ORDTYP).Distinct();
                return Json(new { Success = true, Data = mainHtml, OrdType = ordtyp });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP SearchV_EXP_STAGE_UNINVOICED", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeByORDTYP(string SHIP_TO_INDEX, string ORDTYP)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                var result = eBLL.ChangeByORDTYP(SHIP_TO_INDEX, ORDTYP);
                string mainHtml = GetSearchHTML(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP ChangeByORDTYP", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult AddEXP_COMM_INV(EXPCOMMINVArg COMM, List<EXPSTGHArg> STG)
        {
            EXPWB_BLL eBLL = new EXPWB_BLL();
            try
            {
                //添加新纪录到 EXP_COMM_INV
                COMM.CREATED_BY = Session[CHubConstValues.SessionUser].ToString();
                COMM.EXCHANGE_RATE = Convert.ToDecimal(eBLL.CallFunc_GET_EXP_EXCHANGE_RATE());
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

        [Authorize]
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
                    //新增
                    eBLL.AddEXP_STG_LOAD(item, LOAD_BATCH, appUser);
                }
                //执行存过 P_EXP_STG_LOAD_POST
                eBLL.ExecP_EXP_STG_LOAD_POST(LOAD_BATCH);

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
                    var EstAmt = CallFunc_GET_EXP_EST_AMT(item.LODNUM);
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append("<input type=\"checkbox\" class=\"selectCheckbox\" data-picklistno=\"" + item.PICKLIST_NO + "\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.ORDTYP).Append("</td>");
                    sb.Append("         <td>").Append(item.PICKLIST_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.STCUST).Append("</td>");
                    sb.Append("         <td>").Append(item.CARRIER_CODE).Append("</td>");
                    sb.Append("         <td>").Append(item.SHIP_ID_STG).Append("</td>");
                    sb.Append("         <td>").Append(item.LODNUM).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_VOL).Append("</td>");
                    sb.Append("         <td>").Append(item.VC_PALWGT).Append("</td>");
                    sb.Append("         <td>").Append(EstAmt).Append("</td>");
                    CallF_EXP_HSCODE_CHK_BY_LOD(sb, item.LODNUM);
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDetail\" value=\"Details\" data-lodnum=\"" + item.LODNUM + "\" />").Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }

        public void CallF_EXP_HSCODE_CHK_BY_LOD(StringBuilder sb, string LODNUM)
        {
            string msg = new EXPWB_BLL().CallF_EXP_HSCODE_CHK_BY_LOD(LODNUM);
            if (msg == "OK")
                sb.Append("         <td style=\"color:green;\">").Append(msg).Append("</td>");
            else
                sb.Append("         <td style=\"color:red;\">").Append(msg).Append("</td>");
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

        [Authorize]
        public ActionResult CINVINQ()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.cinvinq.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult Export(string COMM_INV_ID)
        {
            CINVINQ_BLL cBLL = new CINVINQ_BLL();
            try
            {
                //执行存过 P_EXP_UPT_HSCODE
                cBLL.ExecP_EXP_UPT_HSCODE(COMM_INV_ID);
                //执行Function F_EXP_HSCODE_CHK
                var msg = cBLL.CallFuncF_EXP_HSCODE_CHK(COMM_INV_ID);
                if (msg != "OK")
                {
                    string fullpath = CreateTXT(msg);
                    return Json(new RequestResult(false, msg, fullpath));//function返回不是OK,就报错显示返回值
                }

                //function返回值为OK
                var getSql = cBLL.CallFunc_GET_SQL(COMM_INV_ID);
                string filename = getSql.Split('~')[0];
                string sql = getSql.Split('~')[1];
                DataTable dt = cBLL.RunSql(sql);
                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                string fullName = basePath + filename + ".xlsx";
                NPOIExcelHelper npoi = new NPOIExcelHelper(fullName);
                npoi.DataTableToExcel(dt, "Sheet1");
                return Json(new RequestResult(fullName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP Export", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        //生成文件
        public string CreateTXT(string msg)
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            string filepath = Server.MapPath(CHubConstValues.ChubTempFolder);
            string fullpath = filepath + filename;
            FileStream fs = new FileStream(fullpath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(msg);
            sw.Flush();
            sw.Close();
            fs.Close();
            return fullpath;
        }



        public void DownloadTXT(string fullpath)
        {
            FileStream fs = new FileStream(fullpath, FileMode.OpenOrCreate);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(fullpath), System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public ActionResult DownLoad(string fullname)
        {
            return File(fullname, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullname));
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
            string detailDisplay = string.Empty; string completeDisplay = string.Empty; string discardDisplay = string.Empty; string exportDisplay = string.Empty;
            if (COMM_STATUS == "DISCARD")
                detailDisplay = "disabled";
            if (COMM_STATUS != "DRAFT")
            {
                completeDisplay = "disabled";
                discardDisplay = "disabled";
            }
            else
                exportDisplay = "disabled";
            sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnComplete\" value=\"完成\" data-comminvid=\"" + COMM_INV_ID + "\" " + completeDisplay + " />")
                                      .Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDiscard\" value=\"作废\" data-comminvid=\"" + COMM_INV_ID + "\" " + discardDisplay + " />")
                                      .Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnExport\" value=\"导出\" data-comminvid=\"" + COMM_INV_ID + "\" " + exportDisplay + " />")
                                      .Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDetail\" value=\"细节\" data-comminvid=\"" + COMM_INV_ID + "\" " + detailDisplay + " />");
            return sb;
        }

        [Authorize]
        public ActionResult EXPRATE()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.exprate.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetEXCHANGE_TYPE()
        {
            EXP_EXCHANGE_RATE_BLL eerBLL = new EXP_EXCHANGE_RATE_BLL();
            try
            {
                var result = eerBLL.GetEXCHANGE_TYPE();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP GetEXCHANGE_TYPE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetTableResult(string EXCHANGE_TYPE)
        {
            EXP_EXCHANGE_RATE_BLL eerBLL = new EXP_EXCHANGE_RATE_BLL();
            try
            {
                var result = eerBLL.GetTableResult(EXCHANGE_TYPE);
                string mainHTML = GetEXPRATEHtml(result);
                return Json(new RequestResult(mainHTML));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP GetTableResult", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult InsertOrUpdateEXPRATE(string EXCHANGETYPE, string STARTDATE, EXP_EXCHANGE_RATE eer, string method)
        {
            EXP_EXCHANGE_RATE_BLL eerBLL = new EXP_EXCHANGE_RATE_BLL();
            try
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                if (string.IsNullOrEmpty(appUser))
                    return RedirectToAction("Login", "Account");
                eerBLL.InsertOrUpdateEXPRATE(EXCHANGETYPE, STARTDATE, eer, method, appUser);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP InsertOrUpdateEXPRATE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public static string GetEXPRATEHtml(List<EXP_EXCHANGE_RATE> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append(item.EXCHANGE_TYPE).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm START_DATE\" value=\"" + item.START_DATE.ToString("yyyy/MM/dd") + "\" onclick=\"ShowCalendar(this)\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm END_DATE\" value=\"" + item.END_DATE.ToString("yyyy/MM/dd") + "\" onclick=\"ShowCalendar(this)\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm EXCHANGE_RATE\" value=\"" + item.EXCHANGE_RATE + "\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm NOTE\" value=\"" + item.NOTE + "\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnSave\" value=\"SAVE\" data-exchangetype=\"" + item.EXCHANGE_TYPE + "\" data-startdate=\"" + item.START_DATE.ToString("yyyy/MM/dd") + "\" />").Append("</td>");
                    sb.Append("     </tr>");
                }
            }

            return sb.ToString();
        }

        [Authorize]
        public ActionResult FinLoad()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.finload.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        #region
        //[Authorize]
        //[HttpPost]
        //public ActionResult UploadFinLoadFile()
        //{
        //    FINLOAD_BLL flBLL = new FINLOAD_BLL();
        //    try
        //    {
        //        HttpPostedFileBase hpf = Request.Files[0];

        //        DataTable dt = GetDataTable(hpf);
        //        if (dt == null && dt.Rows.Count == 0)
        //            return Content("No data in excel");

        //        List<EXP_VAT_LOAD> list = ClassConvert.DataTableToList<EXP_VAT_LOAD>(dt);
        //        if (list == null || list.Count == 0)
        //            return Content("Wrong excel strut");

        //        //每批导入去一次批次号 EXP_VAT_TOKEN.NEXTVAL
        //        //string LOAD_BATCH = flBLL.GetLOAD_BATCH();
        //        string appUser = Session[CHubConstValues.SessionUser].ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("EXP UploadFinLoadFile", ex);
        //        return Json(new RequestResult(false, ex.Message));
        //    }
        //    return View();
        //}

        //public ActionResult DownloadFinLoadFileTemplate()
        //{
        //    string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
        //    string fileName = CHubConstValues.EXPFinLoadTemplateName;
        //    return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //}
        #endregion


        [Authorize]
        [HttpPost]
        public ActionResult UploadOneFinLoadFile()
        {
            FINLOAD_BLL flBLL = new FINLOAD_BLL();
            try
            {
                //获取文件
                HttpPostedFileBase hpf = Request.Files[0];

                DataTable dt = GetDataTable(hpf);
                if (dt == null && dt.Rows.Count == 0)
                    return Content("No data in excel");

                List<EXP_VAT_LOAD> evlists = ClassConvert.DataTableToList<EXP_VAT_LOAD>(dt);
                if (evlists == null || evlists.Count == 0)
                    return Content("Wrong excel strut");

                //每批导入取一次EXP_VAT_TOKEN.NEXTVAL
                var LOAD_BATCH = flBLL.GetExpVatLoad_Batch();
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                foreach (var item in evlists)
                {
                    //新增
                    flBLL.InsertIntoEXP_VAT_LOAD(item, Convert.ToDecimal(LOAD_BATCH), appUser);
                }
                //执行Procedure
                flBLL.ExecP_EXP_VAT_LOAD_POST(LOAD_BATCH);
                //查询导入成功数据的数量
                string num = flBLL.GetNumOfEXP_VAT_D(LOAD_BATCH);
                return Json(new RequestResult(true, "成功导入数据: " + num + "条"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP UploadOneFinLoadFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadOneFinLoadFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.EXPFinLoadOneTemplateName;
            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        [Authorize]
        [HttpPost]
        public ActionResult UploadTwoFinLoadFile()
        {
            FINLOAD_BLL flBLL = new FINLOAD_BLL();
            try
            {
                //获取文件
                HttpPostedFileBase hpf = Request.Files[0];

                //文件内容转DataTable
                DataTable dt = GetDataTable(hpf);
                if (dt == null && dt.Rows.Count == 0)
                    return Content("No data in excel");

                //DataTable转List
                IList<EXP_VAT_XREF_LOAD> evxlists = ClassConvert.DataTableToList<EXP_VAT_XREF_LOAD>(dt);
                if (evxlists == null || evxlists.Count == 0)
                    return Content("Wrong excel strut");

                //每批导入取一次EXP_XREF_TOKEN.NEXTVAL
                var LOAD_BATCH = flBLL.GetExpXrefLoad_Batch();
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                foreach (var item in evxlists)
                {
                    //新增
                    flBLL.InsertIntoEXP_VAT_XREF_LOAD(item, Convert.ToDecimal(LOAD_BATCH), appUser);
                }
                //查询导入成功数据的数量
                string num = flBLL.GetNumOfEXP_VAT_XREF_LOAD(LOAD_BATCH);
                return Json(new RequestResult(true, "成功导入数据: " + num + "条"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP UploadTwoFinLoadFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadTwoFinLoadFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.EXPFinLoadTwoTemplateName;
            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadThreeFinLoadFile()
        {
            FINLOAD_BLL flBLL = new FINLOAD_BLL();
            try
            {
                //获取文件
                HttpPostedFileBase hpf = Request.Files[0];

                //文件内容转DataTable
                DataTable dt = GetDataTable(hpf);
                if (dt == null && dt.Rows.Count == 0)
                    return Content("No data in excel");

                //DataTable转List
                IList<EXP_COLLECTION_LOAD> eclists = ClassConvert.DataTableToList<EXP_COLLECTION_LOAD>(dt);
                if (eclists == null || eclists.Count == 0)
                    return Content("Wrong excel strut");

                //每批导入取一次 EXP_COLLECTION_TOKEN.NEXTVAL
                var LOAD_BATCH = flBLL.GetExpCollectionLoad_Batch();
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                foreach (var item in eclists)
                {
                    //新增
                    flBLL.InsertIntoEXP_COLLECTION_LOAD(item, Convert.ToDecimal(LOAD_BATCH), appUser);
                }
                //查询导入成功数据的数量
                string num = flBLL.GetNumOfEXP_COLLECTION_LOAD(LOAD_BATCH);
                return Json(new RequestResult(true, "成功导入数据: " + num + "条"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("EXP UploadThreeFinLoadFile", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadThreeFinLoadFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.EXPFinLoadThreeTemplateName;
            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        public DataTable GetDataTable(HttpPostedFileBase hpf)
        {
            DataTable dt = new DataTable();
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = folder.FullName + tempGuid + ".xls";
            hpf.SaveAs(fileFullName);

            NPOIExcelHelper excelHelper = new NPOIExcelHelper(fileFullName);
            dt = excelHelper.ExcelToDataTable();
            System.IO.File.Delete(fileFullName);

            return dt;
        }

    }
}
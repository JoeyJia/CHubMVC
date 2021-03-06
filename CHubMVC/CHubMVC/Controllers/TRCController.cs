﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;
using CHubModel;
using System.Text;
using System.IO;
using System.Data;

namespace CHubMVC.Controllers
{
    public class TRCController : BaseController
    {
        // GET: TRC
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// TBTRACEC
        /// </summary>
        /// <returns></returns>
        public ActionResult LBTRACEC()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbtracec.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [HttpPost]
        public ActionResult GetWH_ID()
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.GetWH_ID();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC GetWH_ID", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetWH_ID_DESC(string WH_ID)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.GetWH_ID_DESC(WH_ID);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC GetWH_ID_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult LBTRACECSearch(string ADRNAM, string WH_ID)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.LBTRACECSearch(ADRNAM, WH_ID);
                var mainHtml = LBTRACECHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACECSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult LBTRACECSave(List<RP_ADR_MST_NEW> list)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        if (tBLL.LBTRACECCheck(item))
                            tBLL.LBTRACECSave(item);
                        else
                            continue;
                    }
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACECSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBTRACECDownload()
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                DataTable dt = tBLL.LBTRACECDownload();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                    string fileName = "lbtracec" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    NPOIExcelHelper npoi = new NPOIExcelHelper(basePath + fileName);
                    npoi.DataTableToExcel(dt, "Sheet1");
                    return Json(new RequestResult(fileName));
                }
                else
                    return Json(new RequestResult(false, "No Data"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACECDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        public string LBTRACECHtml(List<RP_ADR_MST_NEW> list)
        {
            StringBuilder sb = new StringBuilder();

            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.WH_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.ADRNAM).Append("</td>");
                    if (item.LABEL_TRACE == "Y")
                        sb.Append("     <td>").Append("<input type='checkbox' class='singleSelect' checked />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type='checkbox' class='singleSelect' />").Append("</td>");
                    sb.Append("     <td>").Append(item.LOAD_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }



        /// <summary>
        /// LBTRACEP
        /// </summary>
        /// <returns></returns>
        public ActionResult LBTRACEP()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbtracep.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }
        [HttpPost]
        public ActionResult LBTRACEPSearch(string PART_NO, int PageIndex)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                bool show = true;
                int PageSize = 50;
                int RowStart = PageIndex * PageSize + 1;
                int RowEnd = RowStart + PageSize - 1;

                int RowCount = tBLL.LBTRACEPSearchCount(PART_NO);
                if (RowEnd >= RowCount)
                    show = false;

                var result = tBLL.LBTRACEPSearch(PART_NO, RowStart, RowEnd);
                var mainHtml = LBTRACEPHtml(result);

                return Json(new { Success = true, Data = mainHtml, show = show });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACEPSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult LBTRACEPSave(List<V_G_PART_ADDTIONAL> list)
        {
            TRC_BLL tBLL = new TRC_BLL();

            try
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        if (tBLL.LBTRACEPCheck(item))
                            tBLL.LBTRACEPSave(item);
                        else
                            continue;
                    }
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACEPSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult LBTRACEPDownload()
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                DataTable dt = tBLL.LBTRACEPDownload();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                    string fileName = "lbtracep" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    NPOIExcelHelper npoi = new NPOIExcelHelper(basePath + fileName);
                    npoi.DataTableToExcel(dt, "Sheet1");
                    return Json(new RequestResult(fileName));
                }
                else
                    return Json(new RequestResult(false, "No Data"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACEPDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string LBTRACEPHtml(List<V_G_PART_ADDTIONAL> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.PRINT_PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.DESCRIPTION).Append("</td>");
                    sb.Append("     <td>").Append(item.DESC_CN).Append("</td>");
                    if (item.LABEL_TRACE == "Y")
                        sb.Append("     <td>").Append("<input type='checkbox' class='singleSelect' checked />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type='checkbox' class='singleSelect' />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// LBTRACEINQ
        /// </summary>
        /// <returns></returns>
        public ActionResult LBTRACEINQ()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbtraceinq.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [HttpPost]
        public ActionResult LBTRACEINQSearch(string BARCODE, string DOC_NO)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.LBTRACEINQSearch(BARCODE, DOC_NO);
                var mainHtml = LBTRACEINQHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACEINQSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult LBTRACEINQDetail(string SCAN_SEQ)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.LBTRACEINQDetail(SCAN_SEQ);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBTRACEINQDetail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string LBTRACEINQHtml(List<V_TRC_SCAN_HISTORY> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.DOC_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.BARCODE).Append("</td>");
                    sb.Append("     <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("     <td>").Append(item.LOCAL_SHIP_TO_NAME).Append("</td>");
                    sb.Append("     <td>").Append(item.SCAN_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.SHIP_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.APP_USER).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-xs btnDetail' value='Details' data-scan_seq='" + item.SCAN_SEQ + "' />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        [Authorize]
        [HttpGet]
        public ActionResult LBAutoSetup()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.lbautosetup.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBAutoSetupSearch(string KEY_WORD)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.LBAutoSetupSearch(KEY_WORD);
                var mainHtml = LBAutoSetupHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBAutoSetupSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBAutoSetupSave(List<V_TRC_AUTO_SETUP> list)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        if (item.AUTO_ID == 0)//add
                        {
                            item.AUTO_ID = Convert.ToDecimal(tBLL.GetLBAutoSetupSeq());
                            tBLL.LBAutoSetupAddOrUpdate(item, "Add");
                        }
                        else//update
                            tBLL.LBAutoSetupAddOrUpdate(item, "Update");
                    }
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBAutoSetupSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBAutoSetupApplyAll()
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                tBLL.LBAutoSetupApplyAll();
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBAutoSetupApplyAll", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBAutoSetupDownload()
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                DataTable dt = tBLL.LBAutoSetupDownload();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                    string fileName = "lbautosetup" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    NPOIExcelHelper npoi = new NPOIExcelHelper(basePath + fileName);
                    npoi.DataTableToExcel(dt, "Sheet1");
                    return Json(new RequestResult(fileName));
                }
                else
                    return Json(new RequestResult(false, "No Data"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBAutoSetupDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBAutoSetupShowList(string WH_ID, string KEY_WORD)
        {
            TRC_BLL tBLL = new TRC_BLL();
            try
            {
                var result = tBLL.LBAutoSetupShowList(WH_ID, KEY_WORD);
                var mainHtml = LBAutoSetupModalHtml(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBAutoSetupShowList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult LBAutoSetupAddNew()
        {
            List<V_TRC_AUTO_SETUP> list = new List<V_TRC_AUTO_SETUP>();
            try
            {
                V_TRC_AUTO_SETUP item = new V_TRC_AUTO_SETUP();
                item.CREATE_DATE = DateTime.Now;
                list.Add(item);
                var mainHtml = LBAutoSetupHtml(list);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TRC LBAutoSetupAddNew", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string LBAutoSetupHtml(List<V_TRC_AUTO_SETUP> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr data-id='" + item.AUTO_ID + "'>");
                    sb.Append("     <td>").Append("<input type='text' class='form-control input-sm KEY_WORD' value='" + item.KEY_WORD + "' />").Append("</td>");
                    sb.Append("     <td>").Append(GetWH_ID(item.WH_ID)).Append("</td>");
                    if (item.ACTIVEIND == "Y")
                        sb.Append("     <td>").Append("<input type='checkbox' class='ACTIVEIND' checked />").Append("</td>");
                    else
                        sb.Append("     <td>").Append("<input type='checkbox' class='ACTIVEIND' />").Append("</td>");
                    sb.Append("     <td>").Append(item.DESCRIPTION).Append("</td>");
                    sb.Append("     <td>").Append(item.CREATE_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append("<input type='button' class='btn btn-primary btn-sm btnShowList' value='Show List' data-whid='" + item.WH_ID + "' data-keyword='" + item.KEY_WORD + "' />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            return sb.ToString();
        }

        public string LBAutoSetupModalHtml(List<RP_ADR_MST_NEW> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td>").Append(item.ADRNAM).Append("</td>");
                    sb.Append("     <td>").Append(item.WH_ID).Append("</td>");
                    sb.Append("     <td>").Append(item.LABEL_TRACE).Append("</td>");
                    sb.Append("     <td>").Append(item.LOAD_DATE.ToString("yyyy/MM/dd")).Append("</td>");
                    sb.Append("     <td>").Append(item.NOTE).Append("</td>");
                    sb.Append(" </tr>");
                }
            }
            
            return sb.ToString();
        }

        public string GetWH_ID(string WH_ID = "")
        {
            StringBuilder sb = new StringBuilder();
            TRC_BLL tBLL = new TRC_BLL();
            var result = tBLL.GetWH_ID();
            sb.Append("<select class='form-control input-sm WH_ID'>");
            sb.Append(" <option value=''></option>");
            foreach (var item in result)
            {
                if (WH_ID == item.WH_ID)
                    sb.Append("<option value='" + item.WH_ID + "' selected>" + item.WH_ID + "   " + item.DESCRIPTION + "</option>");
                else
                    sb.Append("<option value='" + item.WH_ID + "'>" + item.WH_ID + " " + item.DESCRIPTION + "</option>");
            }
            sb.Append("</select>");
            return sb.ToString();
        }


        public ActionResult DownLoad(string fileName)
        {
            string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(basePath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fullPath = folder.FullName + fileName;
            return base.ExcelDownload(fullPath);
        }

    }
}
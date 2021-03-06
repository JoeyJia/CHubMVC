﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubDBEntity;
using CHubModel;
using CHubCommon;
using static CHubCommon.CHubEnum;
using System.IO;
using System.Data;
using System.Net;
using CHubModel.WebArg;
using System.Reflection;
using CHubDBEntity.UnmanagedModel;
using System.Text;

namespace CHubMVC.Controllers
{
    public class TCController : BaseController
    {
        [Authorize]
        public ActionResult Maint()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, PageNameEnum.tcmnt.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [Authorize]
        public ActionResult Query()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, PageNameEnum.tcinq.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult QueryAction(string partNo, string hsCode, string declrName, string element, int currentPage, int pageSize)
        {
            CHubEntities db = new CHubEntities();
            V_TC_MDM_ALL_BLL mdmBLL = new V_TC_MDM_ALL_BLL();
            List<V_TC_MDM_ALL> result = new List<V_TC_MDM_ALL>();
            int totalCount = 0;
            try
            {
                result = mdmBLL.GetTCMDMList(partNo, hsCode, declrName, element, currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {

            }

            var obj = new
            {
                result = result,
                totalCount = totalCount
            };
            return Json(obj);
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetGOOD_DESC(string HSCODE, string CIQ)
        {
            V_TC_MDM_ALL_BLL vBLL = new V_TC_MDM_ALL_BLL();
            try
            {
                var result = vBLL.GetGOOD_DESC(HSCODE, CIQ);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TC GetGOOD_DESC");
                return Json(new RequestResult(new RequestResult(false, ex.Message)));
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetELEMENTCK(string PART_NO)
        {
            V_TC_MDM_ALL_BLL vBLL = new V_TC_MDM_ALL_BLL();
            try
            {
                var result = vBLL.GetELEMENTCK(PART_NO);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TC GetELEMENTCK");
                return Json(new RequestResult(new RequestResult(false, ex.Message)));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetCIQLists(string HSCODE)
        {
            V_TC_MDM_ALL_BLL vBLL = new V_TC_MDM_ALL_BLL();
            var lists = new List<TC_HSCODE_CIQ_MST>();
            lists.Add(new TC_HSCODE_CIQ_MST()
            {
                HSCODE = HSCODE,
                CIQ = "",
                GOOD_DESC = "",
                NOTE = ""
            });
            try
            {
                var result = vBLL.GetCIQLists(HSCODE);
                foreach (var item in result)
                {
                    lists.Add(item);
                }
                return Json(new RequestResult(lists));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TC GetCIQLists");
                return Json(new RequestResult(new RequestResult(false, ex.Message)));
            }
        }
        


        [HttpPost]
        [Authorize]
        public ActionResult InitTCPartForm()
        {
            TC_PART_CATEGORY_BLL cateBLL = new TC_PART_CATEGORY_BLL();
            List<TC_PART_CATEGORY> cateList = cateBLL.GetTCPartCategory();
            return Json(cateList);
        }

        public ActionResult CheckPartNo(string partNo)
        {
            M_PART_BLL mPartBLL = new M_PART_BLL();
            M_PART mPart = mPartBLL.GetMPartByPartNo(partNo);
            if (mPart != null)
                return Content(mPart.DESCRIPTION);
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("wrong part No.");
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult CheckHSCODE(string HSCODE)
        {
            TC_HSCODE_MST_BLL tBLL = new TC_HSCODE_MST_BLL();
            try
            {
                var bo = tBLL.IsExistHSCODE(HSCODE);
                if (bo)
                    return Json(new RequestResult(true, "EXIST"));
                else
                    return Json(new RequestResult(false, "HSCODE Not EXIST"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TC CheckHSCODE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CopySearch(string PART_NO)
        {
            M_PART_BLL bll = new M_PART_BLL();
            try
            {
                var result = bll.CopySearch(PART_NO);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TC CopySearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CopyData(string PART_NO)
        {
            M_PART_BLL bll = new M_PART_BLL();
            try
            {
                var result = bll.CopyData(PART_NO);
                return Json(new RequestResult(result.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("TC CopyData", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult SaveAction(V_TC_MDM_ALL mdmAll)
        {
            TC_PART_HS partHS = new TC_PART_HS();
            ClassConvert.DrawObj(mdmAll, partHS);

            M_PART mPart = new M_PART();
            mPart.PART_NO = mdmAll.PART_NO;
            mPart.TC_CATEGORY_BY_MAN = mdmAll.TC_CATEGORY_BY_MAN;

            bool success = SaveTCPartData(partHS, mPart);

            if (success)
                return Content("success");
            else
                return Content("Fail");
        }


        

        /// <summary>
        /// For HS code upload
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult UploadHSFile(IEnumerable<HttpPostedFileBase> fileInput)
        {
            string failParts = string.Empty;
            try
            {
                HttpPostedFileBase fb = Request.Files[0];
                string tempGuid = Guid.NewGuid().ToString();
                string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                FileInfo folder = new FileInfo(folderPath);
                if (!Directory.Exists(folder.FullName))
                    Directory.CreateDirectory(folder.FullName);

                string fileFullName = folder.FullName + tempGuid + ".xlsx";
                fb.SaveAs(fileFullName);

                NPOIExcelHelper excelHelper = new NPOIExcelHelper(fileFullName);
                DataTable dt = excelHelper.ExcelToDataTable();
                //Delete temp file
                System.IO.File.Delete(fileFullName);

                if (dt == null || dt.Rows.Count == 0)
                    return Content("No data in excel");

                dt.Columns.Remove("TAX_REFUND_RATE");
                dt.Columns.Remove("MFN_RATE");
                dt.Columns.Remove("UOM");
                //DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
                List<TC_PART_HS> partList = ClassConvert.ConvertDT2List<TC_PART_HS>(dt);

                if (partList == null || partList.Count == 0)
                    return Content("wrong excel strut");

                int successCount = 0;
                int failCount = 0;

                foreach (var item in partList)
                {
                    if (SaveTCPartData(item, null))
                        successCount++;
                    else
                    {
                        failCount++;
                        failParts += (item.PART_NO + "|");
                    }
                }
                //foreach add try catch region
                string msg = string.Format("Total Count:{0}, Success Count:{1}, Fail Count:{2} :{3}", partList.Count, successCount, failCount, failParts);
                return Json(new RequestResult(true, msg));
            }
            catch (Exception ex)
            {
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownloadHSFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.HSPartExcelTemplateName;

            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        /// <summary>
        /// TC Category Upload
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult UploadTCCateFile()
        {
            HttpPostedFileBase fb = Request.Files[0];
            //HttpPostedFileBase fb = fileInput.ToArray()[0];
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = folder.FullName + tempGuid + ".xlsx";
            fb.SaveAs(fileFullName);
            NPOIExcelHelper excelHelper = new NPOIExcelHelper(fileFullName);
            DataTable dt = excelHelper.ExcelToDataTable();
            //DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
            //delete temp file
            System.IO.File.Delete(fileFullName);

            //validate dt struct
            if (dt == null || dt.Rows.Count == 0)
            {
                return Content("No data in excel");
            }
            else
            {
                if (dt.Columns.Count != 2)
                    return Content("wrong excel struct");
            }


            int successCount = 0;
            int failCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                M_PART mpart = new M_PART();
                mpart.PART_NO = dt.Rows[i][0].ToString();
                mpart.TC_CATEGORY_BY_MAN = dt.Rows[i][1].ToString();
                if (SaveMPart(mpart))
                    successCount++;
                else
                    failCount++;
            }

            return Content(string.Format("Total Count:{0}, Success Count:{1}, Fail Count:{2}", dt.Rows.Count, successCount, failCount));
        }

        public ActionResult DownloadMPartFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.MPartExcelTemplateName;

            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetPartAuditLog(string partNo)
        {

            TC_PART_HS_AUDIT_BLL auditBLL = new TC_PART_HS_AUDIT_BLL();
            return Json(auditBLL.GetAuditLog(partNo));

        }


        #region  private function
        public bool SaveTCPartData(TC_PART_HS partHS, M_PART mPart)
        {
            using (CHubEntities db = new CHubEntities())
            {
                M_PART_BLL mPartBLL = new M_PART_BLL();
                if (mPartBLL.Exist(partHS.PART_NO))
                {
                    //must exist in m_part otherwise error
                    if (mPart != null && !string.IsNullOrEmpty(mPart.TC_CATEGORY_BY_MAN))
                    {
                        M_PART currMpart = mPartBLL.GetMPartByPartNo(partHS.PART_NO);
                        currMpart.TC_CATEGORY_BY_MAN = mPart.TC_CATEGORY_BY_MAN;
                        mPartBLL.Update(currMpart);
                    }
                    if (partHS != null)
                    {
                        TC_PART_HS_BLL pHSBLL = new TC_PART_HS_BLL(db);
                        if (pHSBLL.Exist(partHS.PART_NO))
                        {
                            //update
                            TC_PART_HS existModel = pHSBLL.GetTCPartHS(partHS.PART_NO);
                            List<string> skipList = new List<string>();
                            skipList.Add("PART_NO");
                            skipList.Add("CREATED_BY");
                            skipList.Add("CREATE_DATE");
                            skipList.Add("UPDATED_BY");
                            skipList.Add("RECORD_DATE");

                            ClassConvert.DrawObj(partHS, existModel, skipList);

                            existModel.UPDATED_BY = Session[CHubConstValues.SessionUser].ToString();
                            existModel.RECORD_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                            pHSBLL.update(existModel, false);

                        }
                        else
                        {
                            //add
                            partHS.CREATED_BY = Session[CHubConstValues.SessionUser].ToString();
                            partHS.CREATE_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                            partHS.UPDATED_BY = Session[CHubConstValues.SessionUser].ToString();
                            partHS.RECORD_DATE = partHS.CREATE_DATE;
                            pHSBLL.Add(partHS, false);
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    //error part
                    LogHelper.WriteErrorLog(string.Format("Fail Save Action, reason: Wrong partNo {0}", partHS.PART_NO));
                    return false;
                }
            }
        }

        private bool SaveMPart(M_PART mPart)
        {
            using (CHubEntities db = new CHubEntities())
            {
                M_PART_BLL mPartBLL = new M_PART_BLL();
                if (mPartBLL.Exist(mPart.PART_NO))
                {
                    //must exist in m_part otherwise error
                    if (mPart != null && !string.IsNullOrEmpty(mPart.TC_CATEGORY_BY_MAN))
                    {
                        M_PART currMpart = mPartBLL.GetMPartByPartNo(mPart.PART_NO);
                        currMpart.TC_CATEGORY_BY_MAN = mPart.TC_CATEGORY_BY_MAN;
                        mPartBLL.Update(currMpart);
                    }

                    return true;
                }
                else
                {
                    //error part
                    LogHelper.WriteErrorLog(string.Format("Fail Save Action, reason: Wrong partNo {0}", mPart.PART_NO));
                    return false;
                }
            }
        }

        #endregion


        [Authorize]
        public ActionResult HSCODE()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.hscode.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        public ActionResult GetCID()
        {
            TC_PART_CATEGORY_BLL bll = new TC_PART_CATEGORY_BLL();
            try
            {
                var result = bll.GetCIDList();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetCID");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        /// <summary>
        /// SearchByCode
        /// </summary>
        /// <param name="HSCODE"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetHSCODEByCode(string HSCODE)
        {
            TC_HSCODE_MST_BLL bll = new TC_HSCODE_MST_BLL();
            TC_PART_CATEGORY_BLL cbll = new TC_PART_CATEGORY_BLL();
            try
            {
                var result = bll.GetHSCODEByCode(HSCODE);
                var cresult = cbll.GetCIDList();
                return Json(new { Success = true, Data = result, Data1 = cresult });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetHSCODEByCode");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddHSCODE(TCHSCODEMSTArg HSCode, string Type)
        {
            TC_HSCODE_MST_BLL bll = new TC_HSCODE_MST_BLL();
            CHubDBEntity.UnmanagedModel.TC_HSCODE_MST tc = new CHubDBEntity.UnmanagedModel.TC_HSCODE_MST();
            try
            {
                if (Type == "Update")
                {
                    tc.RECORD_DATE = DateTime.Now;
                    tc.UPDATED_BY = Session[CHubConstValues.SessionUser].ToString();
                }
                else
                {
                    if (bll.IsExistHSCODE(HSCode.HSCODE))
                        return Json(new RequestResult(false, "The Data is Exist!"));
                    else
                        tc.CREATE_DATE = DateTime.Now;
                }

                foreach (PropertyInfo info in tc.GetType().GetProperties())
                {
                    if (HSCode.GetType().GetProperty(info.Name) != null)
                    {
                        info.SetValue(tc, HSCode.GetType().GetProperty(info.Name).GetValue(HSCode), null);
                    }
                }

                bll.AddOrUpdate(tc, Type);
                return Json(new RequestResult(true, "Data Has Saved"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP AddHSCODE");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetHSCODEAUDIT(string HSCODE)
        {
            TC_HSCODE_MST_BLL bll = new TC_HSCODE_MST_BLL();
            try
            {
                var result = bll.GetHsCodeAudit(HSCODE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RP GetHSCODEAUDIT");
                return Json(new RequestResult(false, ex.Message));
            }
        }




    }
}
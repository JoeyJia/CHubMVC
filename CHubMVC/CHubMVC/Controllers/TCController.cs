using System;
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
        public ActionResult QueryAction(string partNo, string hsCode, string declrName, string element,int currentPage,int pageSize)
        {
            CHubEntities db = new CHubEntities();
            V_TC_MDM_ALL_BLL mdmBLL = new V_TC_MDM_ALL_BLL(db);
            int totalCount = 0;
            List<V_TC_MDM_ALL> result = mdmBLL.GetTCMDMList(partNo, hsCode, declrName, element, currentPage, pageSize,out totalCount);

            var obj = new
            {
                result = result,
                totalCount = totalCount
            };
            return Json(obj);
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
            HttpPostedFileBase fb = Request.Files[0];
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder); 
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = folder.FullName + tempGuid + ".xlsx";
            fb.SaveAs(fileFullName);
            DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
            List<TC_PART_HS> partList = ClassConvert.ConvertDT2List<TC_PART_HS>(dt);
            //Delete temp file
            System.IO.File.Delete(fileFullName);

            int successCount = 0;
            int failCount = 0;
            foreach (var item in partList)
            {
                if (SaveTCPartData(item, null))
                    successCount++;
                else
                    failCount++;
            }

            return Content(string.Format("Total Count:{0}, Success Count:{1}, Fail Count:{2}",partList.Count,successCount,failCount));
        }

        public ActionResult DownloadHSFileTemplate()
        {
            string templateFolder = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            string fileName = CHubConstValues.HSPartExcelTemplateName;

            return File(templateFolder+fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);
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
            DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
            //delete temp file
            System.IO.File.Delete(fileFullName);

            int successCount = 0;
            int failCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                M_PART mpart = new M_PART();
                mpart.PART_NO = dt.Rows[i][0].ToString();
                mpart.TC_CATEGORY_BY_MAN = dt.Rows[i][1].ToString();
                if(SaveMPart(mpart))
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

            return File(templateFolder + fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);
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
                    if (mPart!=null && !string.IsNullOrEmpty(mPart.TC_CATEGORY_BY_MAN))
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
                            partHS.UPDATED_BY = Session[CHubConstValues.SessionUser].ToString();
                            pHSBLL.update(partHS,false);

                        }
                        else
                        {
                            //add
                            partHS.CREATED_BY = Session[CHubConstValues.SessionUser].ToString();
                            partHS.CREATE_DATE = DateTime.Now;
                            pHSBLL.Add(partHS,false);
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


    }
}
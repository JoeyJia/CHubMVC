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

namespace CHubMVC.Controllers
{
    public class TCController : Controller
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
        public ActionResult QueryAction(string partNo, string hsCode, string declrName, string element)
        {
            CHubEntities db = new CHubEntities();
            V_TC_MDM_ALL_BLL mdmBLL = new V_TC_MDM_ALL_BLL(db);
            List<V_TC_MDM_ALL> result = mdmBLL.GetTCMDMList(partNo, hsCode, declrName, element);
            return Json(result);
        }

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

        public ActionResult UploadHSFile(IEnumerable<HttpPostedFileBase> fileInput)
        {
            HttpPostedFileBase fb = Request.Files[0];
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = @"C:\Users\oo450\Source\Repos\CHubMVC\CHubMVC\CHubMVC\temp\";//For temp folder path.
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = folder.FullName + tempGuid + ".xlsx";
            fb.SaveAs(fileFullName);
            DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);

            System.IO.File.Delete(fileFullName);
            return Content("1");
        }

        public ActionResult UploadTCCateFile()
        {
            HttpPostedFileBase fb = Request.Files[0];
            //HttpPostedFileBase fb = fileInput.ToArray()[0];
            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = @"C:\Users\oo450\Source\Repos\CHubMVC\CHubMVC\CHubMVC\temp\";//For temp folder path.
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = folder.FullName + tempGuid + ".xlsx";
            fb.SaveAs(fileFullName);
            DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);

            System.IO.File.Delete(fileFullName);
            return Content("1");
        }


        #region  private function
        public bool SaveTCPartData(TC_PART_HS partHS, M_PART mPart)
        {
            using (CHubEntities db = new CHubEntities())
            {
                M_PART_BLL mPartBLL = new M_PART_BLL();
                if (mPartBLL.Exist(partHS.PART_NO))
                {
                    if (!string.IsNullOrEmpty(mPart.TC_CATEGORY_BY_MAN))
                    {
                        M_PART currMpart = mPartBLL.GetMPartByPartNo(partHS.PART_NO);
                        currMpart.TC_CATEGORY_BY_MAN = mPart.TC_CATEGORY_BY_MAN;
                        mPartBLL.Update(currMpart,false);
                    }
                    if (partHS != null)
                    {
                        TC_PART_HS_BLL pHSBLL = new TC_PART_HS_BLL(db);
                        if (pHSBLL.Exist(partHS.PART_NO))
                        {
                            //update
                            pHSBLL.update(partHS,false);

                        }
                        else
                        {
                            //add
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

        #endregion


    }
}
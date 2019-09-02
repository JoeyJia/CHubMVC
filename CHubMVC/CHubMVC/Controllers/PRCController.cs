using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubBLL;
using CHubCommon;
using CHubModel;
using System.Data;
using System.IO;

namespace CHubMVC.Controllers
{
    public class PRCController : BaseController
    {
        // GET: PRC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PRCBench()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.prcbench.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult PRCVerify()
        {
            PRC_BLL pBLL = new PRC_BLL();
            try
            {
                DataTable dt = pBLL.PRCVerify();

                string folder = Server.MapPath(CHubConstValues.ChubTempFolder);
                string fileName = "PRC" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                string fullName = folder + fileName;
                NPOIExcelHelper npoi = new NPOIExcelHelper(fullName);
                npoi.DataTableToExcel(dt, "Sheet1");

                return Json(new RequestResult(fileName));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("PRC PRCVerify", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult DownLoad(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fullPath = folder.FullName + fileName;

            return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullPath));
        }

    }
}
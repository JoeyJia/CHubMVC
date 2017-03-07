using CHubBLL;
using CHubCommon;
using CHubDBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHubMVC.Controllers
{
    public class ITTController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            
            //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            //rpBLL.Add(appUser, CHubEnum.PageNameEnum.tcmnt.ToString(), this.Request.Url.AbsoluteUri);

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchWillBill(string willBillNo)
        {
            V_ITT_SHIPPING_SMRY_BLL ittBLL = new V_ITT_SHIPPING_SMRY_BLL();
            List<V_ITT_SHIPPING_SMRY> result = ittBLL.GetWillBillList(willBillNo);
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTranLoadList(string willBillNo)
        {
            ITT_TRAN_LOAD_BLL tranBLL = new ITT_TRAN_LOAD_BLL();
            List<ITT_TRAN_LOAD> result = tranBLL.GetTranLoadList(willBillNo);
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadTranLoadFile()
        {
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

                //DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
                List<ITT_TRAN_LOAD> modelList = ClassConvert.ConvertDT2List<ITT_TRAN_LOAD>(dt);

                //if (partList == null || partList.Count == 0)
                //    return Content("wrong excel strut");

                //int successCount = 0;
                //int failCount = 0;
                //foreach (var item in partList)
                //{
                //    if (SaveTCPartData(item, null))
                //        successCount++;
                //    else
                //        failCount++;
                //}

                //return Content(string.Format("Total Count:{0}, Success Count:{1}, Fail Count:{2}", partList.Count, successCount, failCount));
                return Content("1");
            }
            catch (Exception ex)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ex.Message);
            }
        }

    }
}
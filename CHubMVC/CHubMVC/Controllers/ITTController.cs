﻿using CHubBLL;
using CHubCommon;
using CHubDBEntity;
using CHubModel;
using CHubMVC.Validations;
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
        public ActionResult SaveTranLoad(ITT_TRAN_LOAD model)
        {
            try
            {
                string msg = SaveTranLoadAction(model);
                if(!string.IsNullOrEmpty(msg))
                     return Json(new RequestResult(false, msg));
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                //this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //return Content(ex.Message);
                return Json(new RequestResult(false, ex.Message));
            }
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
                    return Json(new RequestResult(false, "No data in excel"));

                //DataTable dt = ExcelHelper.GetDTFromExcel(fileFullName);
                List<ITT_TRAN_LOAD> modelList = ClassConvert.ConvertDT2List<ITT_TRAN_LOAD>(dt);

                if (modelList == null || modelList.Count == 0)
                    return Json(new RequestResult(false, "wrong excel strut"));

                int successCount = 0;
                int failCount = 0;
                foreach (var item in modelList)
                {
                    string msg = SaveTranLoadAction(item);
                    if (string.IsNullOrEmpty(msg))
                        successCount++;
                    else
                    {
                        failCount++;
                        LogHelper.WriteErrorLog(msg);
                    }
                }

                return Content(string.Format("Total Count:{0}, Success Count:{1}, Fail Count:{2}", modelList.Count, successCount, failCount));
            }
            catch (Exception ex)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ex.Message);
            }
        }


        //Custom process part
        [HttpPost]
        [Authorize]
        public ActionResult GetCustList(string willBillNo)
        {
            ITT_CUST_LOAD_BLL custBLL = new ITT_CUST_LOAD_BLL();
            List<ITT_CUST_LOAD> result = custBLL.GetCustList(willBillNo);
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveCustLoad(ITT_CUST_LOAD model)
        {
            try
            {
                string msg = SaveCustLoadAction(model);
                if (!string.IsNullOrEmpty(msg))
                    return Json(new RequestResult(false, msg));
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                //this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //return Content(ex.Message);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadCustLoadFile()
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
                List<ITT_CUST_LOAD> modelList = ClassConvert.ConvertDT2List<ITT_CUST_LOAD>(dt);

                if (modelList == null || modelList.Count == 0)
                    return Content("wrong excel strut");

                int successCount = 0;
                int failCount = 0;
                foreach (var item in modelList)
                {
                    string msg = SaveCustLoadAction(item);
                    if (string.IsNullOrEmpty(msg))
                        successCount++;
                    else
                    {
                        failCount++;
                        LogHelper.WriteErrorLog(msg);
                    }
                }

                return Content(string.Format("Total Count:{0}, Success Count:{1}, Fail Count:{2}", modelList.Count, successCount, failCount));
            }
            catch (Exception ex)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ex.Message);
            }
        }


        #region private function part

        private string SaveTranLoadAction(ITT_TRAN_LOAD model)
        {
            try
            {
                ITT_TRAN_LOAD_VALIDATION validation = new ITT_TRAN_LOAD_VALIDATION(model);
                string msg = validation.ValidationAction();
                if (!string.IsNullOrEmpty(msg))
                {
                    return msg;
                }

                model.LOADED_BY = Session[CHubConstValues.SessionUser].ToString();
                model.LOAD_DATE = DateTime.Now;
                ITT_TRAN_LOAD_BLL tranBLL = new ITT_TRAN_LOAD_BLL();
                tranBLL.Save(model);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Fail Saved:{0}", ex.Message);
            }
        }

        private string SaveCustLoadAction(ITT_CUST_LOAD model)
        {
            try
            {
                ITT_CUST_LOAD_VALICATION validation = new ITT_CUST_LOAD_VALICATION(model);
                string msg = validation.ValidationAction();
                if (!string.IsNullOrEmpty(msg))
                {
                    return msg;
                }

                model.LOADED_BY = Session[CHubConstValues.SessionUser].ToString();
                model.LOAD_DATE = DateTime.Now;
                ITT_CUST_LOAD_BLL tranBLL = new ITT_CUST_LOAD_BLL();
                tranBLL.Save(model);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Fail Saved:{0}", ex.Message);
            }
        }

        #endregion



    }
}
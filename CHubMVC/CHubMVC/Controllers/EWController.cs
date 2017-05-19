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
using CHubModel.ExtensionModel;

namespace CHubMVC.Controllers
{
    public class EWController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            //string appUser = Session[CHubConstValues.SessionUser].ToString();
            //APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            //rpBLL.Add(appUser, CHubEnum.PageNameEnum.cgldb.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetEWGroupList()
        {
            try
            {
                EW_MESSAGE_GROUP_BLL ewBLL = new EW_MESSAGE_GROUP_BLL();
                List<EW_MESSAGE_GROUP> result = ewBLL.GetMsgGroups();

                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetEWGroupList", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetEWGroupDetail(string ewGroup)
        {
            try
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                EW_MESSAGE_BLL ewBLL = new EW_MESSAGE_BLL();
                List<ExEWMessage> result = ewBLL.GetMsgByGroup(ewGroup,appUser);

                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("GetEWGroupDetail", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        //[HttpPost]
        //[Authorize]
        //public ActionResult RunJobs(List<string> idList)
        //{
        //    try
        //    {
        //        string folder = Server.MapPath(CHubConstValues.WebEmailAttachFolder);
        //        BatchJobs jobs = new BatchJobs();
        //        foreach (var item in idList)
        //        {
        //            jobs.SendM1Mail(item,folder);
        //        }

        //        return Json(new RequestResult(true));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("RunJobs", ex);
        //        return Json(new RequestResult(false, ex.Message));
        //    }
        //}

        [HttpPost]
        [Authorize]
        public ActionResult SaveApplies(List<string> idList,string group)
        {
            try
            {
                if (idList == null)
                    idList = new List<string>();
                string appUser = Session[CHubConstValues.SessionUser].ToString();
                EW_USER_APPLY_BLL aBLL = new EW_USER_APPLY_BLL();
                aBLL.SaveApply(idList, group, appUser);

                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SaveApplies", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendSample(string id)
        {
            try
            {
                string toAddr = string.Empty;
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                if(IsTooOften(id,appUser))
                    return Json(new RequestResult(false,"Too Frequent,Need 10 minites interval"));

                APP_USERS_BLL userBLL = new APP_USERS_BLL();
                APP_USERS user = userBLL.GetAppUserByDomainName(appUser);
                if (user == null)
                    return Json(new RequestResult(false, "Can't find User:" + appUser));
                if (user.EMAIL_ADDR != null)
                    toAddr = user.EMAIL_ADDR;
                else
                    toAddr = string.Format(CHubConstValues.EmailFormat, appUser);

                string folder = Server.MapPath(CHubConstValues.WebEmailAttachFolder);
                BatchJobs jobs = new BatchJobs();
                jobs.SendM1Mail(id, folder,toAddr);


                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("RunJobs", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        //private function
        private bool IsTooOften(string messageID, string appUser)
        {
            EW_USER_APPLY_BLL aBLL = new EW_USER_APPLY_BLL();
            EW_USER_APPLY apply = aBLL.GetSpecifyUserApply(messageID, appUser);

            if (apply != null)
            {
                DateTime? lastDate = apply.SAMPLE_DATE;
                if (lastDate != null && lastDate.Value.AddMinutes(10) > DateTime.Now)
                    return true;
            }
            else
            {
                EW_USER_APPLY model = new EW_USER_APPLY();
                model.MESSAGE_ID = messageID;
                model.APP_USER = appUser;
                model.APPLY = CHubConstValues.IndN;
                apply.SAMPLE_DATE = DateTime.Now;
                aBLL.Add(model);
                return false;
            }

            apply.SAMPLE_DATE = DateTime.Now;
            aBLL.update(apply);

            return false;
        }

    }
}
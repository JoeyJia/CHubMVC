using CHubBLL;
using CHubCommon;
using CHubDBEntity;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHubMVC.Tasks
{
    public class BaseScheduleJob
    {
        public string ScheduleID { get; set; }
        public List<string> MsgIDList = null;
        public string AttachFolder = null;

        private EW_SCHEDULE scheduleInstance;
        public EW_SCHEDULE ScheduleInstance
        {
            get
            {
                if (scheduleInstance != null)
                    return scheduleInstance;
                else
                {
                    EW_SCHEDULE_BLL sBLL = new EW_SCHEDULE_BLL();
                    scheduleInstance= sBLL.GetSchedule(ScheduleID);
                    return scheduleInstance;
                }
            }
            set { scheduleInstance = value; }
        }

        public BaseScheduleJob()
        {
            AttachFolder = System.Web.HttpContext.Current.Server.MapPath(CHubConstValues.WebEmailAttachFolder); 
        }

        //public void AddHangfireJob()
        //{
        //    RecurringJob.AddOrUpdate(() => jobs.BuildAndSendEmail("M1", attachFolder, null), Cron.Daily(20));
        //}

        public string GetCronString()
        {
            if (ScheduleInstance == null)
                return null;

            if (scheduleInstance.WEEK != null)
                return Cron.Weekly((DayOfWeek)((int)scheduleInstance.WEEK.Value), (int)(scheduleInstance.HOUR ?? 0), (int)(scheduleInstance.MIN ?? 0));
            else
                return Cron.Daily((int)(scheduleInstance.HOUR ?? 0), (int)(scheduleInstance.MIN ?? 0));
        }

        public virtual void Execute()
        {
            if (this.scheduleInstance == null || this.scheduleInstance.ACTIVEIND == CHubConstValues.IndN)
                return;

            GetMsgList();
            if (MsgIDList == null || MsgIDList.Count == 0)
                return;

            foreach (var item in MsgIDList)
            {
                try
                {
                    if (HasExecuted(item))
                        continue;
                    SendEmail(item);
                    LogStatus(item);
                }
                catch (Exception ex)
                {
                    //Fail branch
                    LogStatus(item);
                    string msg = ex.Message;
                    //continue;
                }
            }

        }

        public virtual bool HasExecuted(string msgID)
        {
            EW_LOG_BLL logBLL = new EW_LOG_BLL();
            return logBLL.HasExecuted(msgID);
        }

        public bool SendEmail(string msgID)
        {
            EmailBLL eBLL = new EmailBLL();
            eBLL.BuildAndSendEmail(msgID, AttachFolder);
            return true;
        }

        public virtual void LogStatus(string msgID,Exception ex=null)
        {
            EW_LOG_BLL logBLL = new EW_LOG_BLL();
            EW_LOG log = new EW_LOG();
            log.MESSAGE_ID = msgID;
            log.LOG_DATE = DateTime.Now;
            if (ex == null)
            {
                log.STATUS = CHubConstValues.IndY;
            }
            else
            {
                log.STATUS = CHubConstValues.IndN;
                log.ERR_MSG = ex.Message;
            }
            logBLL.AddOrUpdate(log);
        }

        public void GetMsgList()
        {
            EW_SCHEDULE_TASK_BLL tBLL = new EW_SCHEDULE_TASK_BLL();
            MsgIDList = tBLL.GetTaskIDsBySchedule(ScheduleID);
        }

    }
}
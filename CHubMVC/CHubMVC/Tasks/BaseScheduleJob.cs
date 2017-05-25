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
        public List<string> MsgIDList = new List<string>();

        public virtual void Execute()
        {
            GetMsgList();

            foreach (var item in MsgIDList)
            {
                try
                {
                    if (IsExecuted(item))
                        continue;
                    SendEmail(item);
                    LogStatus(item);
                }
                catch (Exception ex)
                {
                    //Fail
                    LogStatus(item);
                    string msg = ex.Message;
                    //continue;
                }
            }

        }

        public bool IsExecuted(string msgID)
        {
            return false;
        }

        public bool SendEmail(string msgID)
        {

            return true;
        }

        public void LogStatus(string msgID)
        {

        }

        public void GetMsgList()
        {
            MsgIDList = new List<string>();
        }

    }
}
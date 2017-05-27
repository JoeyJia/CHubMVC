using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily4am : BaseScheduleJob
    {
        public Daily4am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily4am.ToString();
        }
    }
}
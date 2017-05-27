using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily5am : BaseScheduleJob
    {
        public Daily5am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily5am.ToString();
        }
    }
}
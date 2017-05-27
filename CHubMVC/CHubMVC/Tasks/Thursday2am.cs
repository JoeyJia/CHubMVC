using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Thursday2am : BaseScheduleJob
    {
        public Thursday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Thursday2am.ToString();
        }
    }
}
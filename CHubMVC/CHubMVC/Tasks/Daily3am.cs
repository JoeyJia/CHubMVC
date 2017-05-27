using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily3am:BaseScheduleJob
    {
        public Daily3am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily3am.ToString();
        }
    }
}
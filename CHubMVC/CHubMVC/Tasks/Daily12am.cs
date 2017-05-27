using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily12am : BaseScheduleJob
    {
        public Daily12am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily12am.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily6am : BaseScheduleJob
    {
        public Daily6am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily6am.ToString();
        }
    }
}
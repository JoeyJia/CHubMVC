using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily6pm : BaseScheduleJob
    {
        public Daily6pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily6pm.ToString();
        }
    }
}
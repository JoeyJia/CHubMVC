using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily10pm : BaseScheduleJob
    {
        public Daily10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily10pm.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily11pm : BaseScheduleJob
    {
        public Daily11pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily11pm.ToString();
        }
    }
}
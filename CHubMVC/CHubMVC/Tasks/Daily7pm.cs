using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily7pm : BaseScheduleJob
    {
        public Daily7pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily7pm.ToString();
        }
    }
}
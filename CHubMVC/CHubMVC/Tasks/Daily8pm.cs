using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily8pm : BaseScheduleJob
    {
        public Daily8pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily8pm.ToString();
        }
    }
}
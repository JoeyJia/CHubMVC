using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily9pm : BaseScheduleJob
    {
        public Daily9pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily9pm.ToString();
        }
    }
}
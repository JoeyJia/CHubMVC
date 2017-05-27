using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Thursday10pm : BaseScheduleJob
    {
        public Thursday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Thursday10pm.ToString();
        }
    }
}
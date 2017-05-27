using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Friday10pm : BaseScheduleJob
    {
        public Friday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Friday10pm.ToString();
        }
    }
}
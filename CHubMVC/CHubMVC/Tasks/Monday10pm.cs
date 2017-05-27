using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Monday10pm : BaseScheduleJob
    {
        public Monday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Monday10pm.ToString();
        }
    }
}
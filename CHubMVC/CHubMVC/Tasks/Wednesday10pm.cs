using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Wednesday10pm : BaseScheduleJob
    {
        public Wednesday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Wednesday10pm.ToString();
        }
    }
}
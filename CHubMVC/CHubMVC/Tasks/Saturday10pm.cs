using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Saturday10pm : BaseScheduleJob
    {
        public Saturday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Saturday10pm.ToString();
        }
    }
}
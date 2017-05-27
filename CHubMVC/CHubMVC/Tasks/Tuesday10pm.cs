using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Tuesday10pm : BaseScheduleJob
    {
        public Tuesday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Tuesday10pm.ToString();
        }
    }
}
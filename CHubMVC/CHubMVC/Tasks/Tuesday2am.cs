using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Tuesday2am : BaseScheduleJob
    {
        public Tuesday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Tuesday2am.ToString();
        }
    }
}
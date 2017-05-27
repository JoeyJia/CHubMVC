using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Monday2am : BaseScheduleJob
    {
        public Monday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Monday2am.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily2am : BaseScheduleJob
    {
        public Daily2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily2am.ToString();
        }
    }
}
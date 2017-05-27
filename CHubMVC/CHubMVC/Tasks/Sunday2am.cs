using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Sunday2am : BaseScheduleJob
    {
        public Sunday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Sunday2am.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Wednesday2am : BaseScheduleJob
    {
        public Wednesday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Wednesday2am.ToString();
        }
    }
}
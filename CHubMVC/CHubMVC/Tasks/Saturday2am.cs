using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Saturday2am : BaseScheduleJob
    {
        public Saturday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Saturday2am.ToString();
        }
    }
}
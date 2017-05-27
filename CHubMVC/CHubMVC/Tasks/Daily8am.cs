using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily8am : BaseScheduleJob
    {
        public Daily8am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily8am.ToString();
        }
    }
}
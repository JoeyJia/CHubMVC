using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily1am : BaseScheduleJob
    {
        public Daily1am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily1am.ToString();
        }
    }
}
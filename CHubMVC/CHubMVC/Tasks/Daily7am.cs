using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Daily7am : BaseScheduleJob
    {
        public Daily7am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Daily7am.ToString();
        }
    }
}
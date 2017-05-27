using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Friday2am : BaseScheduleJob
    {
        public Friday2am()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Friday2am.ToString();
        }
    }
}
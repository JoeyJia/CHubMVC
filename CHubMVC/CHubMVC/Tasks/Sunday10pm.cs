using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubCommon;

namespace CHubMVC.Tasks
{
    public class Sunday10pm : BaseScheduleJob
    {
        public Sunday10pm()
        {
            this.ScheduleID = CHubEnum.ScheduleEnum.Sunday10pm.ToString();
        }
    }
}
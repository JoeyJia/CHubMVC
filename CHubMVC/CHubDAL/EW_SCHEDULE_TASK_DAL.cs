using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_SCHEDULE_TASK_DAL : BaseDAL
    {
        public EW_SCHEDULE_TASK_DAL()
            : base() { }

        public EW_SCHEDULE_TASK_DAL(CHubEntities db)
            : base(db) { }


        public List<string> GetTaskIDsBySchedule(string scheduleID)
        {
            var result = (
                from t in db.EW_SCHEDULE_TASK
                where t.EW_SCHEDULE_ID == scheduleID
                && t.ACTIVEIND == "Y"
                select t.MESSAGE_ID
                );
            return result.ToList();
        }

    }
}

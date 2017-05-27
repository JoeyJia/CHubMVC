using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_SCHEDULE_DAL : BaseDAL
    {
        public EW_SCHEDULE_DAL()
            : base() { }

        public EW_SCHEDULE_DAL(CHubEntities db)
            : base(db) { }

        static List<EW_SCHEDULE> AllSchedule;

        public EW_SCHEDULE GetSchedule(string id)
        {
            //return db.EW_SCHEDULE.FirstOrDefault(a => a.EW_SCHEDULE_ID == id);
            if (AllSchedule == null || AllSchedule.Count == 0)
                InitSchedules();
            return AllSchedule.FirstOrDefault(a => a.EW_SCHEDULE_ID == id);

        }

        private void InitSchedules()
        {
            AllSchedule = db.EW_SCHEDULE.ToList();
        }

    }
}

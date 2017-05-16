using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_SCHEDULE_TASK_BLL
    {
        private readonly EW_SCHEDULE_TASK_DAL dal;

        public EW_SCHEDULE_TASK_BLL()
        {
            dal = new EW_SCHEDULE_TASK_DAL();
        }
        public EW_SCHEDULE_TASK_BLL(CHubEntities db)
        {
            dal = new EW_SCHEDULE_TASK_DAL(db);
        }

    }
}

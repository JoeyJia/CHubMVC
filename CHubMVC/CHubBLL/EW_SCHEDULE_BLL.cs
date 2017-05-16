using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_SCHEDULE_BLL
    {
        private readonly EW_SCHEDULE_DAL dal;

        public EW_SCHEDULE_BLL()
        {
            dal = new EW_SCHEDULE_DAL();
        }
        public EW_SCHEDULE_BLL(CHubEntities db)
        {
            dal = new EW_SCHEDULE_DAL(db);
        }

    }
}

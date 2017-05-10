using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_LOG_BLL
    {
        private readonly EW_LOG_DAL dal;

        public EW_LOG_BLL()
        {
            dal = new EW_LOG_DAL();
        }
        public EW_LOG_BLL(CHubEntities db)
        {
            dal = new EW_LOG_DAL(db);
        }

    }
}

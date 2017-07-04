using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_AUTOPACK_LOG_BLL
    {
        private readonly RP_AUTOPACK_LOG_DAL dal;

        public RP_AUTOPACK_LOG_BLL()
        {
            dal = new RP_AUTOPACK_LOG_DAL();
        }
        public RP_AUTOPACK_LOG_BLL(CHubEntities db)
        {
            dal = new RP_AUTOPACK_LOG_DAL(db);
        }

    }
}

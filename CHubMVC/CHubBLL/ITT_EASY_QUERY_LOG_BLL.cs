using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_EASY_QUERY_LOG_BLL
    {
        private readonly ITT_EASY_QUERY_LOG_DAL dal;

        public ITT_EASY_QUERY_LOG_BLL()
        {
            dal = new ITT_EASY_QUERY_LOG_DAL();
        }
        public ITT_EASY_QUERY_LOG_BLL(CHubEntities db)
        {
            dal = new ITT_EASY_QUERY_LOG_DAL(db);
        }

    }
}

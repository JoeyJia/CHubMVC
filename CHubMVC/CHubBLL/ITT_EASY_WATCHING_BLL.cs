using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_EASY_WATCHING_BLL
    {
        private readonly ITT_EASY_WATCHING_DAL dal;

        public ITT_EASY_WATCHING_BLL()
        {
            dal = new ITT_EASY_WATCHING_DAL();
        }
        public ITT_EASY_WATCHING_BLL(CHubEntities db)
        {
            dal = new ITT_EASY_WATCHING_DAL(db);
        }

    }
}

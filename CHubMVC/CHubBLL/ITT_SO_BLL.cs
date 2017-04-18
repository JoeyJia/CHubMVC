using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_SO_BLL
    {
        private readonly ITT_SO_DAL dal;

        public ITT_SO_BLL()
        {
            dal = new ITT_SO_DAL();
        }
        public ITT_SO_BLL(CHubEntities db)
        {
            dal = new ITT_SO_DAL(db);
        }

    }
}

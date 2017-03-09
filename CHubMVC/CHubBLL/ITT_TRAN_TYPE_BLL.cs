using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_TRAN_TYPE_BLL
    {
        private readonly ITT_TRAN_TYPE_DAL dal;

        public ITT_TRAN_TYPE_BLL()
        {
            dal = new ITT_TRAN_TYPE_DAL();
        }
        public ITT_TRAN_TYPE_BLL(CHubEntities db)
        {
            dal = new ITT_TRAN_TYPE_DAL(db);
        }

    }
}

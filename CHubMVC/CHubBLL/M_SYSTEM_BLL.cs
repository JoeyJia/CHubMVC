using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class M_SYSTEM_BLL
    {
        private readonly M_SYSTEM_DAL dal;

        public M_SYSTEM_BLL()
        {
            dal = new M_SYSTEM_DAL();
        }
        public M_SYSTEM_BLL(CHubEntities db)
        {
            dal = new M_SYSTEM_DAL(db);
        }

    }
}

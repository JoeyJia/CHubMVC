using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_PLABEL_BASE_BLL
    {
        private readonly V_PLABEL_BASE_DAL dal;

        public V_PLABEL_BASE_BLL()
        {
            dal = new V_PLABEL_BASE_DAL();
        }
        public V_PLABEL_BASE_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_BASE_DAL(db);
        }

    }
}

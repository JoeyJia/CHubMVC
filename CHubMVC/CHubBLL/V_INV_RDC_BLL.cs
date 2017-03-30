using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_INV_RDC_BLL
    {
        private readonly V_INV_RDC_DAL dal;

        public V_INV_RDC_BLL()
        {
            dal = new V_INV_RDC_DAL();
        }
        public V_INV_RDC_BLL(CHubEntities db)
        {
            dal = new V_INV_RDC_DAL(db);
        }

    }
}

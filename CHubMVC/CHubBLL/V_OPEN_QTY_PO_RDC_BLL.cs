using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_OPEN_QTY_PO_RDC_BLL
    {
        private readonly V_OPEN_QTY_PO_RDC_DAL dal;

        public V_OPEN_QTY_PO_RDC_BLL()
        {
            dal = new V_OPEN_QTY_PO_RDC_DAL();
        }
        public V_OPEN_QTY_PO_RDC_BLL(CHubEntities db)
        {
            dal = new V_OPEN_QTY_PO_RDC_DAL(db);
        }

    }
}

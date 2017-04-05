using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_OPEN_QTY_PO_RDC_DAL : BaseDAL
    {
        public V_OPEN_QTY_PO_RDC_DAL()
            : base() { }

        public V_OPEN_QTY_PO_RDC_DAL(CHubEntities db)
            : base(db) { }

        public List<V_OPEN_QTY_PO_RDC> GetOpenRDCData(string partNo)
        {
            return db.V_OPEN_QTY_PO_RDC.Where(a => a.PART_NO == partNo).ToList();
        }

    }
}

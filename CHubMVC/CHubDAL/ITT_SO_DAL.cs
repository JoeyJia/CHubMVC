using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class ITT_SO_DAL : BaseDAL
    {
        public ITT_SO_DAL()
            : base() { }

        public ITT_SO_DAL(CHubEntities db)
            : base(db) { }

        public List<ITT_SO> GetLevel3Data(string partNo, string poNo, decimal poLineNo)
        {
            return db.ITT_SO.Where(a => a.PART_NO == partNo 
            && a.CUSTOMER_PO_NO == poNo 
            && a.CUST_PO_LINE_NO == poLineNo).ToList();
        }

    }
}

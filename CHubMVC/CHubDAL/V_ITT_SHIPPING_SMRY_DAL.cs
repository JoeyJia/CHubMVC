using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_ITT_SHIPPING_SMRY_DAL : BaseDAL
    {
        public V_ITT_SHIPPING_SMRY_DAL()
            : base() { }

        public V_ITT_SHIPPING_SMRY_DAL(CHubEntities db)
            : base(db) { }


        public List<V_ITT_SHIPPING_SMRY> GetWillBillList(string willBillNo,string invoiceNo)
        {
            IQueryable<V_ITT_SHIPPING_SMRY> result = db.V_ITT_SHIPPING_SMRY;
            if (!string.IsNullOrEmpty(willBillNo))
            {
                result = result.Where(a => a.WILL_BILL_NO == willBillNo);
            }
            if (!string.IsNullOrEmpty(invoiceNo))
            {
                result = result.Where(a => a.INVOICE_NO == invoiceNo);
            }

            return result.ToList();
        }

    }
}

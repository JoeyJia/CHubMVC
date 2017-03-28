using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_SHIPPING_ALL_BASE_DAL : BaseDAL
    {
        public V_SHIPPING_ALL_BASE_DAL()
            : base() { }

        public V_SHIPPING_ALL_BASE_DAL(CHubEntities db)
            : base(db) { }

        public V_SHIPPING_ALL_BASE GetFirstBaseInfo(string wayBillNo)
        {
            return db.V_SHIPPING_ALL_BASE.FirstOrDefault(a => a.TRACKING_NO == wayBillNo);
        }

        public V_SHIPPING_ALL_BASE GetFirstBaseInfoByInvoice(string invoiceNo)
        {
            return db.V_SHIPPING_ALL_BASE.FirstOrDefault(a => a.INVOICE_NO == invoiceNo);
        }

    }
}

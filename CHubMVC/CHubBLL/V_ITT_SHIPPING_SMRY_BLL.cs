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
    public class V_ITT_SHIPPING_SMRY_BLL
    {
        private readonly V_ITT_SHIPPING_SMRY_DAL dal;

        public V_ITT_SHIPPING_SMRY_BLL()
        {
            dal = new V_ITT_SHIPPING_SMRY_DAL();
        }
        public V_ITT_SHIPPING_SMRY_BLL(CHubEntities db)
        {
            dal = new V_ITT_SHIPPING_SMRY_DAL(db);
        }

        public List<V_ITT_SHIPPING_SMRY> GetWillBillList(string willBillNo,string invoiceNo)
        {
            return dal.GetWillBillList(willBillNo,invoiceNo);
        }

    }
}

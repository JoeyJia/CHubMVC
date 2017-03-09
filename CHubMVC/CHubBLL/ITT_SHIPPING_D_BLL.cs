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
    public class ITT_SHIPPING_D_BLL
    {
        private readonly ITT_SHIPPING_D_DAL dal;

        public ITT_SHIPPING_D_BLL()
        {
            dal = new ITT_SHIPPING_D_DAL();
        }
        public ITT_SHIPPING_D_BLL(CHubEntities db)
        {
            dal = new ITT_SHIPPING_D_DAL(db);
        }

        public bool ExistInvoiceNo(string invoiceNo)
        {
            return dal.ExistInvoiceNo(invoiceNo);
        }

    }
}

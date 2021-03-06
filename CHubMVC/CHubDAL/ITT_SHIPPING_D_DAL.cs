﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class ITT_SHIPPING_D_DAL : BaseDAL
    {
        public ITT_SHIPPING_D_DAL()
            : base() { }

        public ITT_SHIPPING_D_DAL(CHubEntities db)
            : base(db) { }

        public bool ExistInvoiceNo(string invoiceNo)
        {
            return db.ITT_SHIPPING_D.Any(a => a.INVOICE_NO == invoiceNo);
        }


    }
}

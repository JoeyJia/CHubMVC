﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_OPEN_QTY_ASN_PDC_DAL : BaseDAL
    {
        public V_OPEN_QTY_ASN_PDC_DAL()
            : base() { }

        public V_OPEN_QTY_ASN_PDC_DAL(CHubEntities db)
            : base(db) { }

        public List<V_OPEN_QTY_ASN_PDC> GetOpenPDCData(string partNo)
        {
            return db.V_OPEN_QTY_ASN_PDC.Where(a => a.PART_NO == partNo).ToList();
        }

    }
}

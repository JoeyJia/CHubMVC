using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class IHUB_ASN_STG
    {
        public string ASN_NO { get; set; }
        public decimal LINE_NO { get; set; }
        public string COMPANY_CODE { get; set; }
        public DateTime SHIP_DATE { get; set; }
        public string PO_NO { get; set; }
        public decimal PO_LINE_NO { get; set; }
        public decimal PO_REL_NO { get; set; }
        public string PART_NO { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public string COO { get; set; }
        public string NOTE { get; set; }
        public decimal LOAD_BATCH { get; set; }
        public string LOAD_BY { get; set; }
        public DateTime LOAD_DATE { get; set; }
        public string ERR_CHECK { get; set; }
    }
}

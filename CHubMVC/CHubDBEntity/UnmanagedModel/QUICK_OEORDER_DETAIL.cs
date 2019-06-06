using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class QUICK_OEORDER_DETAIL
    {
        public decimal QUICK_ORDER_NO { get; set; }
        public decimal LINE_NO { get; set; }
        public string CUSTOMER_PARTNO { get; set; }
        public string PART_NO { get; set; }
        public decimal BUY_QTY { get; set; }
        public string DESCRIPTION { get; set; }
        public string CARRIER_CODE { get; set; }
        public DateTime DUE_DATE { get; set; }
        public decimal CUST_PO_LINE_NO { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RET_INV_RETURN_H
    {
        public string INVOICE_ID { get; set; }
        public string INVOICE_CODE { get; set; }
        public DateTime INVOICE_DATE { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string CUST_DESC	 { get; set; }
        public string WAREHOUSE { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string REFERENCE_NO { get; set; }
        public decimal RET_REQ_NO	 { get; set; }
        public decimal INVOICE_AMT { get; set; }
        public decimal CHARGES { get; set; }
        public decimal VAT { get; set; }
        public decimal INVOICE_QTY { get; set; }
        public decimal  REMAINING_QTY { get; set; }
        public string RECONCILE_STATUS { get; set; }
        public string NOTE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RET_INV_RETURN_D
    {
        public string INVOICE_ID { get; set; }
        public long INVOICE_LINE_NO { get; set; }
        public string INVOICE_CODE { get; set; }
        public DateTime INVOICE_DATE { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string WAREHOUSE { get; set; }
        public decimal INVOICE_QTY { get; set; }
        public string CURRENCY_CODE { get; set; }
        public decimal UNIT_PRICE_AMT { get; set; }
        public decimal INVOICE_AMT { get; set; }
        public decimal CHARGES { get; set; }
        public decimal VAT { get; set; }
        public string PART_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string PRINT_PART_NO { get; set; }
        public string REFERENCE_NO { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string NOTE { get; set; }
        public DateTime LOAD_DATE { get; set; }
        public decimal REMAINING_QTY { get; set; }
        public string VAT_TIED { get; set; }
    }
}

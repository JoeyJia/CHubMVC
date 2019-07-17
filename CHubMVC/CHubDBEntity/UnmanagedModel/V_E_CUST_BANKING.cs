using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_E_CUST_BANKING
    {
        public string CUSTOMER_NO { get; set; }
        public decimal BILL_TO_LOCATION { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string BILL_TO_NAME { get; set; }
        public string BANK_PAYER { get; set; }
        public decimal BALANCE { get; set; }
        public decimal CREDIT_LIMIT { get; set; }
        public decimal CREDIT_LIMIT_2 { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime? RECORD_DATE { get; set; }
        public string RECORD_BY { get; set; }
        public string BALANCE_ACTIVE_FLAG { get; set; }
    }
}

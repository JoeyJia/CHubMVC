using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_E_BANKING_TRANS
    {
        public decimal TRANS_ID { get; set; }
        public string CUSTOMER_NO { get; set; }
        public decimal BILL_TO_LOCATION { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string TRANS_TYPE { get; set; }
        public decimal TRANS_AMT { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string TRANS_DOC_NO { get; set; }
        public string TRANS_BRIEF	 { get; set; }
        public string APP_USER	 { get; set; }
        public string NOTE	 { get; set; }
        public decimal BALANCE_AFT_TRANS	 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string BANK_PAYER { get; set; }
        public string ORDER_NO { get; set; }
    }
}

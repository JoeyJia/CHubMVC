using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class E_CUST_BANKING_ADDT
    {
        public string CUSTOMER_NO { get; set; }
        public string BALANCE_AUTO_FLAG { get; set; }
        public string BALANCE_EMAIL_TO { get; set; }
        public string BALANCE_EMAIL_CC { get; set; }
        public string NOTE { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
        public string FLEX3 { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

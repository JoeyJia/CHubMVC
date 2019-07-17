using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class E_BANKING_TRANS_LOAD
    {
        public string TRANS_TYPE { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string BANK_PAYER { get; set; }
        public decimal TRANS_AMT { get; set; }
        public string TRANS_DOC_NO { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string TRANS_BRIEF { get; set; }
        public string NOTE { get; set; }
        public string LOAD_MSG { get; set; }
        public string APP_USER { get; set; }
        public decimal LOAD_BATCH { get; set; }
        public decimal LINE_NO { get; set; }
        public DateTime LOAD_DATE { get; set; }
    }
}

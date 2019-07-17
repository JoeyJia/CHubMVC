using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class E_TRANS_TYPE
    {
        public string TRANS_TYPE { get; set; }
        public string TRANS_TYPE_DESC { get; set; }
        public decimal DEBIT_CREDIT_FLAG { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string MANUAL_FLAG { get; set; }
    }
}

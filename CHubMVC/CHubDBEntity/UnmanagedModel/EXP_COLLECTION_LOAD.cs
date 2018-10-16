using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_COLLECTION_LOAD
    {
        public decimal LOAD_BATCH { get; set; }
        public string INVOICE_ID { get; set; }
        public string UBIVNO { get; set; }
        public string RECEIVED_AMT_USD { get; set; }
        public string RECEIVED_RATE { get; set; }
        public string NOTE { get; set; }
        public string LOAD_DATE { get; set; }
        public string LOADED_BY { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_VAT_XREF_LOAD
    {
        public decimal LOAD_BATCH { get; set; }
        public string UBIVNO { get; set; }
        public string VAT_INVOICE_NO { get; set; }
        public string VAT_AMT { get; set; }
        public string VAT_DATE	 { get; set; }
        public string NOTE { get; set; }
        public string LOAD_DATE { get; set; }
        public string LOADED_BY { get; set; }
    }
}

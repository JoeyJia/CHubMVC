using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_VAT_LOAD
    {
        public decimal LOAD_BATCH { get; set; }
        public string UBIVNO { get; set; }
        public string UACUNO { get; set; }
        public string MMITDS { get; set; }
        public string UBIVQS { get; set; }
        public string TAX_REFUND_RATE { get; set; }
        public string AMT_USD { get; set; }
        public string UBSPUN { get; set; }
        public string UBLNAM { get; set; }
        public string NOTE { get; set; }
        public string LOADED_BY	 { get; set; }
        public DateTime LOAD_DATE	 { get; set; }
        public string POST_FLAG { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_XCEC_ORDER_LN_BASE
    {
        public string CUST_ORDER_NO { get; set; }
        public string CUST_PART_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESC_CN { get; set; }
        public string KITS_FLAG { get; set; }
        public decimal ORDER_LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public decimal? QTY { get; set; }
        public DateTime DUE_DATE { get; set; }
    }
}

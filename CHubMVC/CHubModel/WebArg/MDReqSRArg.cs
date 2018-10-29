using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class MDReqSRArg
    {
        public decimal SR_REQ_NO { get; set; }
        public string COMPANY_CODE { get; set; }
        public string Supplier_PARTNO { get; set; }
        public string PRICE { get; set; }
        public string MOQ { get; set; }
        public string LOT_SIZE { get; set; }
        public string LT { get; set; }
        public string IS_COMMON { get; set; }
        public string SR_COMMENTS { get; set; }
    }
}

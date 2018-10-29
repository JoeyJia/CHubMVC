using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class MDReqArg
    {
        public decimal REQ_LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public string PART_DESC { get; set; }
        public string CHECK_EXIST { get; set; }
        public string GLOBAL_PARTNO { get; set; }
        public string GLOBAL_PARTDESC { get; set; }
        public string SHORT_DESC { get; set; }
        public string CHECK_PRI_SUP { get; set; }
        public string CHECK_PRI_PB { get; set; }
        public string CHECK_PRI_BPA { get; set; }
        public string CHECK_COST { get; set; }
        public string PRODUCT_GROUP_ID { get; set; }
        public string NOTE { get; set; }
    }
}

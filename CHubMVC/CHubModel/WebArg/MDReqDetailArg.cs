using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class MDReqDetailArg
    {
        public decimal MD_REQ_NO { get; set; }
        public decimal REQ_LINE_NO { get; set; }
        public string PART_DESC { get; set; }
        public string PRODUCT_GROUP_ID { get; set; }
        public string COMM_PART { get; set; }
        public string GLOBAL_PARTNO { get; set; }
        public string PART_DESC_SHORT { get; set; }
        public string GLOBAL_DESC { get; set; }
        public string APP_STATUS { get; set; }
        public string APP_COMMENTS { get; set; }
        public string NOTE { get; set; }

        public List<MDReqDetailListArg> detaillist { get; set; }
    }

    public class MDReqDetailListArg
    {
        public decimal MD_REQ_NO { get; set; }
        public decimal REQ_LINE_NO { get; set; }
    }
}

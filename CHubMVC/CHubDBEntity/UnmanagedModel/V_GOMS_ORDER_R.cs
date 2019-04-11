using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_GOMS_ORDER_R
    {
        public string LOAD_FROM { get; set; }
        public string STATUS_CODE { get; set; }
        public string STATUS_DESC { get; set; }
        public string ORDER_NO { get; set; }
        public Int64 LINE_NO { get; set; }
        public Int64 REL_NO { get; set; }
        public string PART_NO { get; set; }
        public decimal REVISED_QTY_DUE { get; set; }
        public decimal QTY_RESERVED { get; set; }
        public decimal QTY_PICKED { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public DateTime RDD { get; set; }
        public DateTime? PROMISE_DATE { get; set; }
        public string ESD_REASON { get; set; }
        public DateTime PROM_DATE_ORG { get; set; }
        public string ESD_CODE { get; set; }
        public string ESD_CODE_ORG { get; set; }
        public DateTime CREATE_DATE	 { get; set; }
        public string PROMISE_MSG { get; set; }
        public string COLOR { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
  public  class GKITSArg
    {
        public string VID { get; set; }
        public string PARENT_PART { get; set; }
        public string COMPONENT_PART { get; set; }
        public string COMPONENT_PRINT_NO { get; set; }
        public string COMPONENT_COO { get; set; }
        public string COMPONENT_DESC { get; set; }
        public string COMPONENT_DESC_CN { get; set; }
        public long LINE_ITEM_NO { get; set; }
        public Nullable<System.DateTime> EFF_PHASE_IN_DATE { get; set; }
        public Nullable<System.DateTime> EFF_PHASE_OUT_DATE { get; set; }
        public Nullable<decimal> QTY_PER_ASSEMBLY { get; set; }
    }
}

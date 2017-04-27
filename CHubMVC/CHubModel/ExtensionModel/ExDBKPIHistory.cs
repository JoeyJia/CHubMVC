using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class ExDBKPIHistory
    {
        public System.DateTime KPI_DATE { get; set; }
        public string KPI_CODE { get; set; }
        public string KPI_SUB_CODE { get; set; }
        public decimal KPI_VALUE { get; set; }
        public Nullable<decimal> KPI_TARGET { get; set; }
        public Nullable<decimal> IND_Y { get; set; }
        public string KPI_OWNER { get; set; }
        public string NOTE { get; set; }
        public string OWNER_HIGHLIGHT { get; set; }
        public Nullable<System.DateTime> HIGHLIGHT_DATE { get; set; }

        public string DESC { get; set; }
    }
}

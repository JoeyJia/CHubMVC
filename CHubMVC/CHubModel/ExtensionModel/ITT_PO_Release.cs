using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class ITT_PO_Release
    {
        public decimal PUR_RELEASE_NO { get; set; }

        public DateTime? RELEASE_DATE { get; set; }

        public decimal? RELEASE_QTY { get; set; }

        public decimal? REMAINING_QTY { get; set; }

        public DateTime? REVISED_DUE_DATE { get; set; }

        public DateTime? ETA_DATE { get; set; }

        public DateTime? RECEIVING_DATE { get; set; }

        public string PO_STATUS { get; set; }
    }
}

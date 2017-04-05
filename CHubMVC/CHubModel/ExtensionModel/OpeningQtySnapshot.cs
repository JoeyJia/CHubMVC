using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class OpeningQtySnapshot
    {
        public string WHAlias { get; set; }

        public decimal BackOrderedQty { get; set; }

        public decimal OpeningQty { get; set; }

        public decimal ReservedQty { get; set; }

        public decimal InPickingQty { get; set; }

        public decimal RemainingQty { get; set; }

        public string LatestETA { get; set; }

        public decimal InTransit { get; set; }
    }
}

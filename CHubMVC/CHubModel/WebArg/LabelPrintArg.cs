using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class LabelPrintArg
    {
        public List<LabelPrintItem> items { get; set; }

        public string labelCode { get; set; }
        public string printer { get; set; }
    }

    public class LabelPrintItem
    {
        public string partNo { get; set; }
        public int copies { get; set; }
        public int MOQ { get; set; }
    }
}

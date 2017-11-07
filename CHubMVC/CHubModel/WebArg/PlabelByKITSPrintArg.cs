using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
  public  class PlabelByKITSPrintArg
    {
        public List<PlabelByKITSPrintIDs> items { get; set; }
        public string LABEL_CODE { get; set; }
        public string PART_NO { get; set; }

    }

    public class PlabelByKITSPrintIDs {
        public string VID { get; set; }
    }
}

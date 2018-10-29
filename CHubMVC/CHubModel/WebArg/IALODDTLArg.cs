using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class IALODDTLArg
    {
        public string LODNUM { get; set; }
        public List<IaLodDtlist> DList { get; set; }
    }

    public class IaLodDtlist
    {
        public string PRTNUM { get; set; }
        public string NOTE { get; set; }
        public string QTY_DISPLAY { get; set; }
        public decimal? IA_QTY { get; set; }
        public string IA_CODE1 { get; set; }
        public string IA_CODE2 { get; set; }

    }

}

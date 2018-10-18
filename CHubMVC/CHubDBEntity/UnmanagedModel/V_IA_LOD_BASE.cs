using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_IA_LOD_BASE
    {
        public string WH_ID { get; set; }
        public string LODNUM { get; set; }
        public DateTime? STGDTE { get; set; }
        public string ADRNAM { get; set; }
        public string PRTNUM { get; set; }
        public decimal? UNTQTY { get; set; }
        public string QTY_DISPLAY { get; set; }
        public string LODNUM_DISPLAY { get; set; }
        public string CUST_NOTE { get; set; }
        public string PRT_NOTE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_EXP_STAGE_BASE
    {
        public decimal PICKLIST_NO { get; set; }
        public string STCUST { get; set; }
        public string SHIP_ID_STG { get; set; }
        public string LODNUM { get; set; }
        public decimal VC_PALWGT { get; set; }
        public Int64 VC_PALLEN { get; set; }
        public Int64 VC_PALWID { get; set; }
        public Int64 VC_PALHGT { get; set; }
        public decimal VC_VOL { get; set; }
    }
}

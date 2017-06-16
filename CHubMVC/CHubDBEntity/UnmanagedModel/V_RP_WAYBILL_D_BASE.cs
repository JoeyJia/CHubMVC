using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RP_WAYBILL_D_BASE
    {
        public string WH_ID { get; set; }
        public string SHIP_ID { get; set; }
        public string LODNUM { get; set; }
        public Nullable<decimal> VC_PALWGT { get; set; }
        public string PALVOL { get; set; }
    }
}

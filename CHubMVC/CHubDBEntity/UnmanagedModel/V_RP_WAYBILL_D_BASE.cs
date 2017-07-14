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

        public decimal? VC_PALLEN { get; set; }
        public decimal? VC_PALWID { get; set; }
        public decimal? VC_PALHGT { get; set; }
        public decimal? QTYS { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RP_WAYBILL_D_PRINT
    {
        public string WH_ID { get; set; }
        public string SHIP_ID { get; set; }

        public string LODNUM { get; set; }

        public decimal? VC_PALWGT { get; set; }

        public string PALVOL { get; set; }

        public string REMARK1 { get; set; }

        public string REMKAR2 { get; set; }

        public decimal? VC_PALLEN { get; set; }
        public decimal? VC_PALWID { get; set; }
        public decimal? VC_PALHGT { get; set; }
        public decimal? QTYS { get; set; }

        public decimal? PALVOL_M3 { get; set; }
        public string VC_DLRPONUM { get; set; }
    }
}

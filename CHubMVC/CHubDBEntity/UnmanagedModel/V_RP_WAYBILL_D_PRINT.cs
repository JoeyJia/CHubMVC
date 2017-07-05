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
    }
}

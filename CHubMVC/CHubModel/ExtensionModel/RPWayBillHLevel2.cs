using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class RPWayBillHLevel2
    {
        public string TRACK_NUM_IHUB { get; set; }

        public string SHIP_ID { get; set; }

        public string SHPSTS { get; set; }

        public DateTime? STGDTE { get; set; }

        public string ORDTYP { get; set; }

        public decimal? BOXES { get; set; }

        public decimal? VC_PALWGT { get; set; }

        public decimal? VOL_M3 { get; set; }

        public string CUST_NO { get; set; }

        public string WAYBILL_ID { get; set; }

        public string HOST_EXT_ID { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class ExRPCarMst
    {
        public string WH_ID { get; set; }
        public string CARCOD { get; set; }
        public string CARNAM { get; set; }
        public Nullable<System.DateTime> LOAD_DATE { get; set; }
        public string WAYBILL_ID { get; set; }
        public string SEND_TO_TMS { get; set; }
        public string CARNAM_SHORT { get; set; }

        public string WAYBILL_DESC { get; set; }
    }
}

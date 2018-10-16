using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_SHIP_TO
    {
        public string CUSTOMER_NO { get; set; }
        public Int64 BILL_TO_LOCATION { get; set; }
        public Int64 SHIP_TO_LOCATION { get; set; }
        public string SHIP_TO_ALIAS { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string NOTE { get; set; }
    }
}

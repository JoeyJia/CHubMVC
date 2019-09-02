using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_E_ADDR_MST
    {
        public string MAP_STATUS { get; set; }
        public string MAP_COLOR { get; set; }
        public decimal ADDR_TOKEN { get; set; }
        public string TO_SYSTEM { get; set; }
        public string CUSTOMER_NO { get; set; }
        public decimal BILL_TO_LOCATION { get; set; }
        public decimal SHIP_TO_LOCATION { get; set; }
        public string SHIP_NAME { get; set; }
        public string SHIP_TERRITORY { get; set; }
        public string SHIP_ADDR { get; set; }
        public string SHIP_CONTACT { get; set; }
        public string SHIP_MOBILE { get; set; }
        public string ABBR { get; set; }
        public decimal DEST_LOCATION { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime RECORD_DATE { get; set; }
        public string LastDays { get; set; }
    }
}

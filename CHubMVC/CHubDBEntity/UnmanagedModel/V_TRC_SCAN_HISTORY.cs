using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_TRC_SCAN_HISTORY
    {
        public decimal SCAN_SEQ { get; set; }
        public string DOC_NO { get; set; }
        public string BARCODE { get; set; }
        public string SHIP_ID { get; set; }
        public string PART_NO { get; set; }
        public string APP_USER { get; set; }
        public DateTime SCAN_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string NOTE { get; set; }
        public DateTime SHIP_DATE { get; set; }
        public decimal SHIPMENT_NO { get; set; }
        public string USER_INITIALS { get; set; }
        public string SHIP_TO_ABBR { get; set; }
        public decimal DEST_LOCATION { get; set; }
        public string CARRIER_CODE { get; set; }
        public string CARRIER_PRO_NO { get; set; }
        public string TRAILER_NO { get; set; }
        public string WAREHOUSE	 { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string NAME { get; set; }
        public Int64 BILL_TO_LOCATION { get; set; }
        public Int64 SHIP_TO_LOCATION { get; set; }
        public string SHIP_TO_NAME { get; set; }
        public string SHIP_TO_ADDR_1 { get; set; }
        public string SHIP_TO_ADDR_2 { get; set; }
        public string SHIP_TO_ADDR_3 { get; set; }
        public string SHIP_TO_CONTACT { get; set; }
        public string SHIP_TO_PHONE	 { get; set; }
        public string SHIP_TO_FAX { get; set; }
        public string SHIP_TO_CITY { get; set; }
        public string SHIP_TO_COUNTRY { get; set; }
        public string SHIP_TO_ATTEN	 { get; set; }
        public string LOCAL_SHIP_TO_NAME { get; set; }
        public string LOCAL_SHIP_TO_ADDR_1	 { get; set; }
        public string LOCAL_SHIP_TO_ADDR_2 { get; set; }
        public string LOCAL_SHIP_TO_ADDR_3 { get; set; }
        public string LOCAL_SHIP_TO_CITY { get; set; }
        public string LOCAL_SHIP_TO_COUNTRY { get; set; }
    }
}

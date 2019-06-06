using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class G_ADDR_DFLT
    {
        public string SYSID { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string NAME { get; set; }
        public string ACTIVEIND { get; set; }
        public int AUTO_SUPERSESSION_FLAG { get; set; }
        public int ORDER_SUPERSEDED_PART_FLAG { get; set; }
        public int BILL_TO_LOCATION { get; set; }
        public int SHIP_TO_LOCATION { get; set; }
        public string SHIP_TO_NAME { get; set; }
        public string SHIP_TO_ADDR_1 { get; set; }
        public string SHIP_TO_ADDR_2 { get; set; }
        public string SHIP_TO_ADDR_3 { get; set; }
        public string SHIP_TO_CONTACT { get; set; }
        public string SHIP_TO_PHONE { get; set; }
        public string SHIP_TO_FAX { get; set; }
        public string SHIP_TO_CITY { get; set; }
        public string SHIP_TO_STATE { get; set; }
        public string SHIP_TO_COUNTRY { get; set; }
        public string SHIP_TO_ZIP { get; set; }
        public DateTime RECORD_DATE_OS { get; set; }
        public string WAREHOUSE { get; set; }
        public string SHIP_TO_ATTEN { get; set; }
        public string EC_ENTITY_ID { get; set; }
        public string LOCAL_SHIP_TO_NAME { get; set; }
        public string LOCAL_SHIP_TO_ADDR_1 { get; set; }
        public string LOCAL_SHIP_TO_ADDR_2 { get; set; }
        public string LOCAL_SHIP_TO_ADDR_3 { get; set; }
        public string LOCAL_SHIP_TO_CITY { get; set; }
        public string LOCAL_SHIP_TO_COUNTRY { get; set; }
        public string LOCAL_SHIP_TO_STATE { get; set; }
        public DateTime RECORD_DATE_OSL { get; set; }
        public string ABBREVIATION { get; set; }
        public string DUE_DATE_CODE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class G_ADDR_SPL
    {
        public string SYSID { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string NAME { get; set; }
        public string ACTIVEIND { get; set; }
        public int BILL_TO_LOCATION { get; set; }
        public Int64 SHIP_TO_LOCATION { get; set; }
        public Int64 DEST_LOCATION { get; set; }
        public string DEST_NAME { get; set; }
        public string DEST_ADDR_1 { get; set; }
        public string DEST_ADDR_2 { get; set; }
        public string DEST_ADDR_3 { get; set; }
        public string DEST_CITY { get; set; }
        public string DEST_CONTACT { get; set; }
        public string DEST_PHONE { get; set; }
        public string DEST_FAX { get; set; }
        public string DEST_STATE { get; set; }
        public string DEST_COUNTRY { get; set; }
        public string DEST_ZIP { get; set; }
        public DateTime RECORD_DATE_OSD { get; set; }
        public string WAREHOUSE { get; set; }
        public string DEST_ATTENTION { get; set; }
        public string LOCAL_DEST_NAME { get; set; }
        public string LOCAL_DEST_ADDR_1 { get; set; }
        public string LOCAL_DEST_ADDR_2 { get; set; }
        public string LOCAL_DEST_ADDR_3 { get; set; }
        public string LOCAL_DEST_CITY { get; set; }
        public string LOCAL_DEST_COUNTRY { get; set; }
        public string LOCAL_DEST_STATE { get; set; }
        public DateTime? RECORD_DATE_OSDL { get; set; }
        public string ABBREVIATION { get; set; }
        public string DUE_DATE_CODE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_XCEC_ORDER_HDR_BASE
    {
        public string GOMS_ADDR_MATCHED { get; set; }
        public string XCEC_ADDR { get; set; }
        public string MATCH_FLAG { get; set; }
        /// <summary>
        /// key 1
        /// </summary>
        public string WAREHOUSE { get; set; }
        public string SHIP_WH { get; set; }
        public string CUST_ORDER_NO { get; set; }
        /// <summary>
        /// key 2
        /// </summary>
        public string IHUB_ORDER_NO { get; set; }
        public string ORDER_TYPE { get; set; }
        public DateTime DUE_DATE { get; set; }
        public string CUST_NAME { get; set; }
        public string SHIP_PROVINCE { get; set; }
        public string SHIP_CITY { get; set; }
        public string SHIP_ADDR { get; set; }
        public string CONTACT { get; set; }
        public string TEL { get; set; }
        public string DEALER_PO_NO { get; set; }
        public string KITS_FLAG { get; set; }
        public string NOTE { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string KITS_NO { get; set; }
        public string PROCESS_FLAG { get; set; }
        public string PROCESS_STATUS { get; set; }
        public DateTime? LAST_PROCESS_DATE { get; set; }
        public string LAST_ERROR_MSG { get; set; }
        public string SYSID { get; set; }
        public string GOMS_CUST_NO { get; set; }
        public string BILL_TO_LOCATION { get; set; }
        public string SHIP_TO_LOCATION { get; set; }
        public decimal? DEST_LOCATION { get; set; }
        public string ALIAS_NAME { get; set; }
        public decimal? XCEC_ADDR_SEQ { get; set; }
        public decimal? ORDER_SEQ_NO { get; set; }

    }
}

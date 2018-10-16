using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_EXP_STAGE_UNINVOICED
    {
        public string SHIP_TO_INDEX { get; set; }
        public decimal PICKLIST_NO { get; set; }
        public string SHIP_TO_ABBR { get; set; }
        public Int64 DEST_LOCATION { get; set; }
        public string STCUST { get; set; }
        public string WAREHOUSE { get; set; }
        public string CARRIER_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string SHIP_TO_NAME { get; set; }
        public string SHIP_TO_ADDR_1 { get; set; }
        public string SHIP_TO_ADDR_2 { get; set; }
        public string SHIP_TO_ADDR_3 { get; set; }
        public string SHIP_TO_CONTACT { get; set; }
        public string SHIP_TO_PHONE { get; set; }
        public string SHIP_TO_CITY { get; set; }
        public string SHIP_TO_STATE { get; set; }
        public string SHIP_TO_ZIP { get; set; }
        public string SHIP_TO_COUNTRY { get; set; }
        public string WH_ID { get; set; }
        public string SHIP_ID_STG { get; set; }
        public string LODNUM { get; set; }
        public string HOST_EXT_ID { get; set; }
        public decimal VC_PALWGT { get; set; }
        public Int64 VC_PALLEN { get; set; }
        public Int64 VC_PALWID { get; set; }
        public Int64 VC_PALHGT { get; set; }
        public decimal VC_VOL { get; set; }
        public Int64 COMM_INV_ID { get; set; }
        public string ORDTYP { get; set; }
    }
}

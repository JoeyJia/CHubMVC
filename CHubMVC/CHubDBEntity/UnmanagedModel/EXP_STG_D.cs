using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_STG_D
    {
        public decimal STG_TOKEN { get; set; }
        public string WH_ID { get; set; }
        public string SHIP_ID_STG { get; set; }
        public string LODNUM { get; set; }
        public string HOST_EXT_ID { get; set; }
        public string CARCOD { get; set; }
        public string ORDNUM { get; set; }
        public string ORDLIN { get; set; }
        public string BTCUST { get; set; }
        public string STCUST { get; set; }
        public string ORDTYP { get; set; }
        public string PRTNUM { get; set; }
        public Int64 UNTQTY { get; set; }
        public string ORGCOD { get; set; }
        public decimal VC_PALWGT { get; set; }
        public Int64 VC_PALLEN { get; set; }
        public Int64 VC_PALWID { get; set; }
        public Int64 VC_PALHGT { get; set; }
        public string NOTE { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
        public string LOADED_BY { get; set; }
        public DateTime LOAD_DATE { get; set; }

    }
}

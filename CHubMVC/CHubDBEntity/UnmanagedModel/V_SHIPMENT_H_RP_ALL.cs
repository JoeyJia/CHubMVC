using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_SHIPMENT_H_RP_ALL
    {
        public string SHIP_ID { get; set; }
        public string WH_ID { get; set; }
        public string HOST_EXT_ID { get; set; }
        public string SHPSTS { get; set; }
        public string CARCOD { get; set; }
        public string CARNAM { get; set; }
        public string TRACK_NUM { get; set; }
        public System.DateTime ADDDTE { get; set; }
        public Nullable<System.DateTime> ALCDTE { get; set; }
        public Nullable<System.DateTime> STGDTE { get; set; }
        public Nullable<System.DateTime> LODDTE { get; set; }
        public System.DateTime ENTDTE { get; set; }
        public Nullable<System.DateTime> MODDTE { get; set; }
        public string MOD_USR_ID { get; set; }
        public string ADRNAM { get; set; }
        public string ADRLN1 { get; set; }
        public string ADRLN2 { get; set; }
        public string ADRLN3 { get; set; }
        public string ADRCTY { get; set; }
        public string ADRSTC { get; set; }
        public string ADRPSZ { get; set; }
        public string CTRY_NAME { get; set; }
        public string PHNNUM { get; set; }
        public string EMAIL_ADR { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string CONT_NAME { get; set; }
    }
}

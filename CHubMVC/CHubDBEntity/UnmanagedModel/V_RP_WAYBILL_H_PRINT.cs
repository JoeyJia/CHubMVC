using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RP_WAYBILL_H_PRINT
    {
        public string SHIP_ID { get; set; }
        public string WH_ID { get; set; }
        public string HOST_EXT_ID { get; set; }
        public string SHPSTS { get; set; }
        public string CARCOD { get; set; }
        public string CARNAM { get; set; }
        public string TRACK_NUM { get; set; }
        public DateTime? ADDDTE { get; set; }
        public DateTime? ALCDTE { get; set; }
        public DateTime? STGDTE { get; set; }
        public DateTime? LODDTE { get; set; }
        public DateTime? ENTDTE { get; set; }
        public DateTime? MODDTE { get; set; }
        public string MOD_USR_ID { get; set; }
        public string L_ADRNAM { get; set; }
        public string L_ADRLN1 { get; set; }
        public string L_ADRLN2 { get; set; }
        public string L_ADRLN3 { get; set; }
        public string L_ADRCTY { get; set; }
        public string L_ADRSTC { get; set; }
        public string L_ADRPSZ { get; set; }
        public string L_CTRY_NAME { get; set; }
        public string L_PHNNUM { get; set; }
        public string L_EMAIL_ADR { get; set; }
        public string L_FIRST_NAME { get; set; }
        public string L_LAST_NAME { get; set; }
        public string L_CONT_NAME { get; set; }
        public string R_ADRNAM { get; set; }
        public string R_ADRLN1 { get; set; }
        public string R_ADRLN2 { get; set; }
        public string R_ADRLN3 { get; set; }
        public string R_ADRCTY { get; set; }
        public string R_ADRSTC { get; set; }
        public string R_ADRPSZ { get; set; }
        public string R_CTRY_NAME { get; set; }
        public string R_PHNNUM { get; set; }
        public string R_EMAIL_ADR { get; set; }
        public string R_FIRST_NAME { get; set; }
        public string R_LAST_NAME { get; set; }
        public string R_CONT_NAME { get; set; }
        public string COMPANY { get; set; }
        public string SENDER { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACT { get; set; }
        public string TELEPHONE { get; set; }
        public string SIGNATURE1 { get; set; }
        public string SIGNATURE2 { get; set; }
        public string SIGNATURE3 { get; set; }
        public string NOTE1 { get; set; }
        public string NOTE2 { get; set; }
        public string NOTE3 { get; set; }
        public string NOTE4 { get; set; }
        public string NOTE5 { get; set; }
        public string HEADER1 { get; set; }
        public string HEADER2 { get; set; }
        public string HEADER3 { get; set; }
        public string FOOTER1 { get; set; }
        public string FOOTER2 { get; set; }
        public string FOOTER3 { get; set; }
        public string DETAIL_TITLE1 { get; set; }
        public string DETAIL_TITLE2 { get; set; }
        public string DETAIL_TITLE3 { get; set; }
        public string DETAIL_TITLE4 { get; set; }
        public string DETAIL_TITLE5 { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
        public string FLEX3 { get; set; }
        public string FLEX4 { get; set; }
        public string FLEX5 { get; set; }
        public string WAYBILL_ID { get; set; }
        public string PRINT_DETAIL { get; set; }

        public string CUST_NO { get; set; }
        public decimal? BOXES { get; set; }
        public decimal? VC_PALWGT { get; set; }
        public decimal? VOL_M3 { get; set; }
        public string ORDTYP { get; set; }
        public string ORDTYP_WB { get; set; }
        public string ADDR_COMBINED { get; set; }

        public string TRACK_NUM_BY_IHUB { get; set; }

        //logo part
        public string LOGO { get; set; }
        public string PRINT_LOGO { get; set; }

        public string CARNAM_SHORT { get; set; }
        public string PAPER_SIZE { get; set; }
        public decimal PAPER_HORIZONTAL { get; set; }
        public decimal PAPER_VERTICAL { get; set; }

        public string LOGO2 { get; set; }
        public string TITLE { get; set; }

    }
}

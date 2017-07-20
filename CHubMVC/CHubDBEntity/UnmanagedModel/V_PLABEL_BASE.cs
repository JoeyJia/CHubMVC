using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_PLABEL_BASE
    {
        public string PART_CATALOG { get; set; }
        public string PART_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESC_CN { get; set; }
        public string SHORT_DESCRIPTION { get; set; }
        public string COUNTRY_OF_ORIGIN { get; set; }
        public string UNIT_MEAS { get; set; }
        public string PART_STATUS { get; set; }
        public decimal? PART_HEIGHT { get; set; }
        public decimal? PART_LENGTH { get; set; }
        public decimal? PART_WIDTH { get; set; }
        public decimal? PART_WEIGHT { get; set; }
        public string CURRENT_SALES_STATUS_CODE { get; set; }
        public string PRINT_PART_NO { get; set; }
        public decimal? MIN_ORDER_QTY { get; set; }
        public decimal? QTY_IN_CARTON { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? RECORD_DATE { get; set; }
        public string NOTE_TEXT { get; set; }

    }
}

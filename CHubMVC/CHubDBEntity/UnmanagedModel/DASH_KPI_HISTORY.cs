using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class DASH_KPI_HISTORY
    {
        public decimal KPI_YEAR { get; set; }
        public string ORG_ID { get; set; }
        public string KPI_CODE { get; set; }
        public string KPI_SUB_CODE { get; set; }
        public string KPI_DESC { get; set; }
        public decimal? KPI_VAL_01 { get; set; }
        public decimal? KPI_VAL_02 { get; set; }
        public decimal? KPI_VAL_03 { get; set; }
        public decimal? KPI_VAL_04 { get; set; }
        public decimal? KPI_VAL_05 { get; set; }
        public decimal? KPI_VAL_06 { get; set; }
        public decimal? KPI_VAL_07 { get; set; }
        public decimal? KPI_VAL_08 { get; set; }
        public decimal? KPI_VAL_09 { get; set; }
        public decimal? KPI_VAL_10 { get; set; }
        public decimal? KPI_VAL_11 { get; set; }
        public decimal? KPI_VAL_12 { get; set; }
        public decimal KPI_TARGET { get; set; }
        public double KPI_TARGET_THRESH { get; set; }
        public string NOTE { get; set; }
        public string RECORD_BY { get; set; }
        public DateTime RECORD_DATE { get; set; }
    }
}

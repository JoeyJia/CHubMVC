using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_COMM_INV
    {
        public decimal COMM_INV_ID { get; set; }
        public string COMM_DESC { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string COMM_STATUS { get; set; }
        public string SHIP_TO_INDEX { get; set; }
        public decimal TOTAL_WGT { get; set; }
        public decimal TOTAL_VOL { get; set; }
        public decimal TOTAL_AMT { get; set; }
        public Int64 BOXES { get; set; }
    }
}

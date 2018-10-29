using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class EXPCOMMINVArg
    {
        public string COMM_INV_ID { get; set; }
        public string COMM_DESC { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string SHIP_TO_INDEX { get; set; }
        public decimal TOTAL_WGT { get; set; }
        public decimal TOTAL_VOL { get; set; }
        public decimal TOTAL_AMT { get; set; }
        public decimal BOXES { get; set; }
        public decimal EXCHANGE_RATE { get; set; }
    }

    public class EXPSTGHArg
    {
        public string LODNUM { get; set; }
    }
}

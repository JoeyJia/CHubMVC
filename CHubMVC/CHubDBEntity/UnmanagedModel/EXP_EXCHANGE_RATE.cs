using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class EXP_EXCHANGE_RATE
    {
        public string EXCHANGE_TYPE { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public decimal EXCHANGE_RATE { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string CREATED_BY { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity
{
    public class G_OESALES_CATALOG
    {
        public string SYSID { get; set; }

        public string PART_NO { get; set; }

        public decimal MOQ { get; set; }

        public decimal QTY_IN_CARTON { get; set; }

        public string PVC_CODE { get; set; }

        public decimal  OUTRIGHT_EXCHANGE_FLAG { get; set; }

        public string MANUAL_ALLOCATED_FLAG { get; set; }
    }
}

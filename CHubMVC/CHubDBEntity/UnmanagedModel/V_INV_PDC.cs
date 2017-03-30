using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_INV_PDC
    {
        public string SYS_ID { get; set; }
        public string PART_NO { get; set; }
        public string WAREHOUSE { get; set; }
        public string WH_ALIAS { get; set; }
        public string BAY_NO { get; set; }
        public decimal QTY_AVL { get; set; }
        public decimal QTY_ONHAND { get; set; }
        public decimal QTY_RESERVED { get; set; }
        public decimal QTY_IN_RECEIVING { get; set; }
        public decimal QTY_IN_QA { get; set; }
        public DateTime? LOAD_DATE { get; set; }
    }
}

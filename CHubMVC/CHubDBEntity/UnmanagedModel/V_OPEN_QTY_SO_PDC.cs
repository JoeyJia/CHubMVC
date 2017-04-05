using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_OPEN_QTY_SO_PDC
    {
        public string PART_NO { get; set; }
        public string WAREHOUSE { get; set; }
        public string WH_ALIAS { get; set; }
        public decimal QTY_BACKORDERED { get; set; }
        public decimal QTY_OPENING { get; set; }
        public decimal QTY_RESERVED { get; set; }
        public decimal QTY_IN_PICKING { get; set; }
    }
}

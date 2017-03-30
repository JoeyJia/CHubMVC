using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_OPEN_QTY_SO_RDC
    {
        public string PART_NO { get; set; }
        public string WAREHOUSE { get; set; }
        public string WH_ALIAS { get; set; }
        public string QTY_BACKORDERED { get; set; }
        public string QTY_OPENING { get; set; }
        public string QTY_RESERVED { get; set; }
        public string QTY_IN_PICKING { get; set; }
    }
}

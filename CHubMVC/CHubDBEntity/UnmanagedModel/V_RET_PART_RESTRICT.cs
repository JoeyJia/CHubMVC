using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RET_PART_RESTRICT
    {
        public string PART_NO { get; set; }
        public string RETURN_RESTRICT { get; set; }
        public decimal RETURN_MOQ { get; set; }
        public string NOTE { get; set; }
        public DateTime RECORD_DATE { get; set; }
    }
}

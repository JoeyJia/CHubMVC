using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class MD_REQ_HEADER
    {
        public decimal MD_REQ_NO { get; set; }
        public string REQ_DESC { get; set; }
        public string REQ_BY { get; set; }
        public DateTime REQ_DATE { get; set; }
    }
}

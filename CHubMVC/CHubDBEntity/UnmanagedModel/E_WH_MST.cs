using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class E_WH_MST
    {
        public string WAREHOUSE { get; set; }
        public string WAREHOUSE_DESC { get; set; }
        public string ACTIVEIND { get; set; }
        public string TO_SYSTEM { get; set; }
        public string SYSTEM_WH { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

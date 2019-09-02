using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class E_ORDER_TYPE_MST
    {
        public string ORDER_TYPE { get; set; }
        public string WAREHOUSE { get; set; }
        public string ORDER_TYPE_DESC { get; set; }
        public string ACTIVEIND { get; set; }
        public string SYSTEM_ORDER_TYPE { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

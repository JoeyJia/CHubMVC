using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class E_ORDER_STATUS
    {
        public string ORDER_STATUS { get; set; }
        public string STATUS_DESC { get; set; }
        public string NOTE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string STATUS_COLOR { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class APP_OECUSTOMER_MST
    {
        public string CUSTOMER_NO { get; set; }
        public string CUST_NAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string CWS_FLAG { get; set; }
        public string FLAG01 { get; set; }
        public string FLAG02 { get; set; }
        public string FLAG03 { get; set; }
        public string NOTE { get; set; }
        public DateTime LOAD_DATE { get; set; }
    }
}

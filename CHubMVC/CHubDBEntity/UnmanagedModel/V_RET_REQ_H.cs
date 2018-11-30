using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RET_REQ_H
    {
        public decimal RET_REQ_NO { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string CUST_DESC { get; set; }
        public string MOVEX_WH { get; set; }
        public string RET_REQ_DESC { get; set; }
        public DateTime REQ_DATE { get; set; }
        public string NOTE { get; set; }
        public string REQ_BY { get; set; }
        public string REQ_STATUS { get; set; }
        public string RETURN_TYPE { get; set; }
    }
}

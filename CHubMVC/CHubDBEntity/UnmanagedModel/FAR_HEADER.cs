using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class FAR_HEADER
    {
        public decimal FAR_NO { get; set; }
        public string FAR_STATUS { get; set; }
        public string CUSTOMER_NO { get; set; }
        public decimal PERIOD { get; set; }
        public string FAR_PROJECT { get; set; }
        public string ADJ_TYPE { get; set; }
        public string PRIORITY_CODE { get; set; }
        public string FAR_DESC { get; set; }
        public string RECURRING { get; set; }
        public string SUBSTITUTE { get; set; }
        public string INV_STRATEGY_CODE { get; set; }
        public string MITIGATION_PLAN { get; set; }
        public string REQ_BY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime RECORD_DATE { get; set; }
        public string NOTE { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
        public string FLEX3 { get; set; }
    }
}

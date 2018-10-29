using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RET_REQ_D
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
        public long LINE_NO { get; set; }
        public string CUST_ITEM { get; set; }
        public decimal PRICE { get; set; }
        public long QTY { get; set; }
        public decimal QTY_APPROVED { get; set; }
        public string REJECT_REASON { get; set; }
        public string PART_GROUP { get; set; }
        public string DESCRIPTION { get; set; }
        public string PART_NO { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public decimal RETURN_ALLOW_DAYS { get; set; }
    }
}

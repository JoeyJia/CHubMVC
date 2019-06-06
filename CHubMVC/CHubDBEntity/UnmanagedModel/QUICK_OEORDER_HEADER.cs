using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class QUICK_OEORDER_HEADER
    {
        public string GOMS { get; set; }
        public decimal QUICK_ORDER_NO { get; set; }
        public string ORDER_STATUS { get; set; }
        public string ABBREVIATION { get; set; }
        public string CUSTOMER_NO { get; set; }
        public decimal BILL_TO_LOCATION { get; set; }
        public decimal SHIP_TO_LOCATION { get; set; }
        public decimal DEST_LOCATION { get; set; }
        public string SPL_IND { get; set; }
        public string CUSTOMER_PO_NO { get; set; }
        public string DEALER_PO_NO { get; set; }
        public string ORDER_TYPE { get; set; }
        public string PRIORITY_CODE { get; set; }
        public DateTime DUE_DATE { get; set; }
        public decimal SHIP_COMPLETE_FLAG { get; set; }
        public string CARRIER_CODE { get; set; }
        public int SCHEDULE_ORDER_FLAG { get; set; }
        public string WAREHOUSE { get; set; }
        public string ORDER_NOTES { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string PROCESS_STATUS { get; set; }
        public DateTime PROCESS_DATE { get; set; }
        public string PROCESS_ERROR { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_GOMS_ORDER_H
    {
        public string LOAD_FROM { get; set; }
        public string TITLE { get; set; }
        public string SHIP_STATUS { get; set; }
        public string ORDER_NO { get; set; }
        public string CUSTOMER_NO { get; set; }
        public Int32 BILL_TO_LOCATION { get; set; }
        public Int64 SHIP_TO_LOCATION { get; set; }
        public decimal DEST_LOCATION { get; set; }
        public string CUSTOMER_PO_NO { get; set; }
        public DateTime RDD { get; set; }
        public string ORDER_CURRENCY_CODE { get; set; }
        public string DUE_DATE_CODE { get; set; }
        public string DUE_DATE_DESC { get; set; }
        public Int32 MAX_SHIPMENTS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string DEALER_PO_NO { get; set; }
        public string DB_SO_NO { get; set; }
        public string REF_NO { get; set; }
        public string NOTE_TEXT { get; set; }
        public string COLOR { get; set; }
    }
}

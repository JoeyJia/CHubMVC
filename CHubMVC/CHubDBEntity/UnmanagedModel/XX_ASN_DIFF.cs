using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class XX_ASN_DIFF
    {
        public decimal ASN_DIFF_ID { get; set; }
        public string BUY_FROM_COMPANY { get; set; }
        public string WAREHOUSE { get; set; }
        public string MANIFEST_ID { get; set; }
        public string PUR_ORDER_ID { get; set; }
        public string PACKING_LIST_ID { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public decimal QTY_RECEIVED { get; set; }
        public decimal QTY_DIFF { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public string PRINT_PART_NO { get; set; }
        public string TC_CATEGORY_BY_MAN { get; set; }
        public DateTime? DOCK_DATE { get; set; }
        public string PLANNER { get; set; }
        public string INVOICE_NO { get; set; }
        public DateTime? SHIP_DATE { get; set; }
        public string WILL_BILL_NO { get; set; }
        public string ASN_CREATED_BY { get; set; }
        public string PLANNER_NOTES { get; set; }
        public string DISP_ACTION { get; set; }
        public string WRITE_OFF_NO { get; set; }
        public decimal QTY_FIN_WRITEOFF { get; set; }
        public DateTime? CLAIM_DATE { get; set; }
        public string CLAIM_NOTES { get; set; }
        public string CLAIM_NO { get; set; }
        public string CLAIM_RESULT { get; set; }
        public string CREDIT_NO { get; set; }
        public string CLAIM_DENY_REASON { get; set; }
        public string RECEIVE_CREDIT { get; set; }
        public DateTime? CLOSE_DATE { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string CREATE_USER { get; set; }
        public DateTime? PLANNER_DATE { get; set; }
        public string PLANNER_USER { get; set; }
        public DateTime? FIN_DATE { get; set; }
        public string FIN_USER { get; set; }
        public string IS_CLOSE { get; set; }
        public string DIFFREMARK { get; set; }


    }
}
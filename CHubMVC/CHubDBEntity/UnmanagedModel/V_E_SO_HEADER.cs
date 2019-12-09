using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_E_SO_HEADER
    {
        public string TO_SYSTEM { get; set; }
        public string GOMS_ORDER_NO { get; set; }
        /// <summary>
        /// PK
        /// </summary>
        public string SO_NO { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string BILL_TO_LOCATION { get; set; }
        public string SHIP_TO_LOCATION { get; set; }
        public string ABBR { get; set; }
        public string DEST_LOCATION { get; set; }
        public string WAREHOUSE { get; set; }
        public string ORDER_TYPE { get; set; }
        public string DEALER_PO_NO { get; set; }
        public DateTime ENTER_DATE { get; set; }
        public DateTime DUE_DATE { get; set; }
        public string SHIP_COMPLETE_FLAG { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string TAX_RATE { get; set; }
        public string ORDER_NOTE { get; set; }
        public string SHIP_METHOD { get; set; }
        public string SHIP_NAME { get; set; }
        public string SHIP_TERRITORY { get; set; }
        public string SHIP_ADDR { get; set; }
        public string SHIP_CONTACT { get; set; }
        public string SHIP_MOBILE { get; set; }
        public string SHIP_PROVINCE { get; set; }
        public string SHIP_CITY { get; set; }
        public string SHIP_NOTE { get; set; }
        public string SELF_PICK_CODE { get; set; }
        public string ADDR_TOKEN { get; set; }
        public string APP_ID { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string TOTAL_LINES { get; set; }
        public string TOTAL_AMT { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
        public string FLEX3 { get; set; }
        public string ORDER_STATUS { get; set; }
        public string STATUS_COMMENTS { get; set; }
        public string CHARGE_PCT { get; set; }
        public string MISC_CHARGE_AMT { get; set; }
        public string CHARGE_NOTE { get; set; }
        public string PAY_METHOD { get; set; }
        public string PAY_AMT { get; set; }
        public string PAY_REF_NO { get; set; }
        public DateTime PAY_DATE { get; set; }
        public string PAY_NOTE { get; set; }
        public string FP_TYPE { get; set; }
        public string FP_COMPANY_NAME { get; set; }
        public string FP_TAX_NO { get; set; }
        public string FP_ADDR { get; set; }
        public string FP_TEL { get; set; }
        public string FP_BANK { get; set; }
        public string FP_BANK_ACCT { get; set; }
        public string FP_NOTE { get; set; }
        public string FP_EMAIL { get; set; }
        public string FP_MAIL_ADDR { get; set; }
        public string FP_MAIL_CONTACT { get; set; }
        public string FP_MAIL_TEL { get; set; }
        public DateTime CHECK_DATE { get; set; }
        public string CHECK_MSG { get; set; }
        public string PROCESS_FLAG { get; set; }
        public DateTime LAST_PROC_DATE { get; set; }
        public string PROC_ERROR_MSG { get; set; }
        public string STATUS_DESC { get; set; }
        public string STATUS_COLOR { get; set; }
        public string MAP_STATUS { get; set; }
        public string MAP_COLOR { get; set; }
        public string GOMS_STATUS { get; set; }
        public string GOMS_COLOR { get; set; }
        public string CANCEL_FLAG { get; set; }
        public string APPROVAL_FLAG { get; set; }
    }
}

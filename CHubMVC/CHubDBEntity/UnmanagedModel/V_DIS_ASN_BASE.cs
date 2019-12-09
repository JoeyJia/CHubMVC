using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_DIS_ASN_BASE
    {
        public string ASN_CREATED_BY { get; set; }
        public DateTime? ASN_CREATE_DATE { get; set; }
        public string BILL_OF_LADING_ID { get; set; }
        public string BUY_FROM_COMPANY { get; set; }
        public string COUNTRY_OF_ORIGIN { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string CURRENT_SALES_STATUS_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string MANIFEST_CODE { get; set; }
        public string MANIFEST_ID { get; set; }
        public decimal MANIFEST_LINE_NO { get; set; }
        public string MATERIAL_CODE { get; set; }
        public string PACKING_LIST_ID { get; set; }
        public string PART_NO { get; set; }
        public string PLANNER_BUYER { get; set; }
        public string PRINT_PART_NO { get; set; }
        public string PUR_ORDER_ID { get; set; }
        public decimal PUR_ORDER_LINE_NO { get; set; }
        public decimal PUR_ORDER_REL_NO { get; set; }
        public decimal QTY_RECEIVED { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public DateTime? SHIPMENT_DATE { get; set; }
        public string SOURCE_PLANT { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public string WAREHOUSE { get; set; }
        public string WAY_BILL_ID { get; set; }
        public string WRITE_OFF_TYPE { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_DS_ORDER_BASE
    {
        public string WAREHOUSE { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public string ORDER_NO { get; set; }
        public string CUSTOMER_PO_NO { get; set; }
        public string CUSTOMER_NO { get; set; }
        public Int64 SHIP_TO_LOCATION { get; set; }
        public Int64 DEST_LOCATION { get; set; }
        public string PO_NO { get; set; }
        public string COMPANY_CODE { get; set; }
        public string PART_NO { get; set; }
        public string CATALOG_DESC { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public decimal ORDER_QTY { get; set; }
        public decimal QTY_RESERVED { get; set; }
        public decimal QTY_PICKED { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public string SHIP_TO_NAME { get; set; }
        public string STATUS_CODE { get; set; }
        public DateTime LOAD_DATE { get; set; }
        public string SUPPLIER_ITEM { get; set; }
        public decimal MOQ { get; set; }
        public decimal NOTE { get; set; }
        public DateTime? ETA { get; set; }
        public string ETA_NOTE { get; set; }
        public string COMPANY_NAME_SHORT { get; set; }
        public string CUSTOMER_NAME { get; set; }
    }
}

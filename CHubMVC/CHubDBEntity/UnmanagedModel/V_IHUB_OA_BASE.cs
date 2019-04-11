using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_IHUB_OA_BASE
    {
        public string COMPANY_CODE { get; set; }
        public string SUPP_SO { get; set; }
        public string LINE_NO { get; set; }
        public string SUPPLIER_ITEM { get; set; }
        public string SUPPLIER_ITEM2 { get; set; }
        public string ITEM_DESC { get; set; }
        public decimal ORDER_QTY { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public DateTime SHIP_DATE_PLAN { get; set; }
        public DateTime SHIP_DATE { get; set; }
        public string SHIP_WH { get; set; }
        public string PART_NO { get; set; }
        public string PO_NO { get; set; }
        public decimal PO_LINE_NO { get; set; }
        public string NOTE { get; set; }
        public decimal LOAD_BATCH { get; set; }
        public string LOAD_BY { get; set; }
        public DateTime RECORD_DATE { get; set; }
        public string OA_STATUS { get; set; }
        public string SUPP_SO_ADDT { get; set; }
    }
}

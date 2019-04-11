using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_GOMS_ORDER_D
    {
        public string LOAD_FROM { get; set; }
        public string STATUS_CODE { get; set; }
        public string STATUS_DESC { get; set; }
        public string ORDER_NO { get; set; }
        public Int64 LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public string CATALOG_DESC { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public DateTime RDD { get; set; }
        public Int32 DEST_LOCATION { get; set; }
        public string SUPPLY_ORDER_NO { get; set; }
        public string RELEASE_COMP { get; set; }
        public string RELEASE_COMP_DESC { get; set; }
        public string DUE_DATE_CODE { get; set; }
        public string WAREHOUSE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public decimal REVISED_QTY_DUE { get; set; }
        public decimal QTY_RESERVED { get; set; }
        public decimal QTY_PICKED { get; set; }
        public decimal QTY_SHIPPED { get; set; }
        public string COLOR { get; set; }

    }
}

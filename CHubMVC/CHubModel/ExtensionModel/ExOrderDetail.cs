using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class ExOrderDetail
    {
        public decimal ORDER_REQ_NO { get; set; }
        public decimal ORDER_LINE_NO { get; set; }
        public decimal SHIPFROM_SEQ { get; set; }
        public string PART_NO { get; set; }
        public string CUSTOMER_PART_NO { get; set; }
        public decimal BUY_QTY { get; set; }
        public string DESCRIPTION { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
    }
}

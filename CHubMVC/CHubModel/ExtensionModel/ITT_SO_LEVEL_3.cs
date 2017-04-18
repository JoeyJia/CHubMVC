using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class ITT_SO_LEVEL_3
    {
        public decimal FROM_SYSTEM { get; set; }

        public string ORDER_NO { get; set; }

        public string LINE_NO { get; set; }

        public string SHIP_TO_ABBR { get; set; }

        public string DUE_DATE_CODE { get; set; }

        public decimal BUY_QTY { get; set; }

        public decimal QTY_RESERVED { get; set; }

        public decimal QTY_PICKED { get; set; }

        public decimal QTY_SHIPPED { get; set; }

        public DateTime? RDD { get; set; }

        public DateTime? ESD { get; set; }
    }
}

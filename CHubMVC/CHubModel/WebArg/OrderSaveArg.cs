using CHubModel.ExtensionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class OrderSaveArg
    {
        public string seq { get; set; }

        public string dueDate { get; set; }

        public string orderType { get; set; }

        public string shipCompFlag { get; set; }

        public string orderNote { get; set; }

        public string  customerPONO { get; set; }

        public bool isSpecialShip { get; set; }

        public ExVAliasAddr headInfo { get; set; }

        public ExVAliasAddr altHeadInfo { get; set; }

        public List<OrderLineItem> olList { get; set; }
    }
}

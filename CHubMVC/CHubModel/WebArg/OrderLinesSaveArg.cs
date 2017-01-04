using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class OrderLinesSaveArg
    {
        public decimal orderReqNo { get; set; }

        public List<OrderLineItem> olList { get; set; }
    }
}

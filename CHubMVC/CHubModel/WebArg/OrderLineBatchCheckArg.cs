using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class OrderLineBatchCheckArg
    {
        public string primarySysID { get; set; }

        public string primaryWareHouse { get; set; }

        public string altSysID { get; set; }

        public string altWareHosue { get; set; }

        public List<OrderLineItem> olItemList { get; set; }
    }
}

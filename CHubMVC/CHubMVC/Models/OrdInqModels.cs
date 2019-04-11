using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubDBEntity.UnmanagedModel;

namespace CHubMVC.Models
{
    public class OrdInqModels
    {
        public string CUSTOMER_PO_NO { get; set; }
        public string ORDER_NO { get; set; }
        public List<OrderList> OrderList { get; set; }
        public List<string> titleList { get; set; }
    }

    public class OrderList
    {
        public V_GOMS_ORDER_H Order_H { get; set; }
        public List<V_GOMS_ORDER_D> Order_DList { get; set; }
        public string LOCAL_SHIP_TO_NAME { get; set; }
        public string LOCAL_SHIP_TO_ADDR_1 { get; set; }
        public string LOCAL_SHIP_TO_ADDR_2 { get; set; }
        public string LOCAL_SHIP_TO_ADDR_3 { get; set; }
        public string LOCAL_SHIP_TO_CITY { get; set; }
    }


}
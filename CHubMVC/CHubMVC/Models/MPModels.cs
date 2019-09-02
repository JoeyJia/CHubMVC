using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubDBEntity.UnmanagedModel;

namespace CHubMVC.Models
{
    public class MPModels
    {
        public V_E_CUST_BANKING SearchCondition { get; set; }
        public List<V_E_CUST_BANKING> CBCollection { get; set; }

        public V_E_ADDR_MST addrSearchCondition { get; set; }
        public List<V_E_ADDR_MST> addrCollection { get; set; }
        
        public string appUser { get; set; }

    }
}
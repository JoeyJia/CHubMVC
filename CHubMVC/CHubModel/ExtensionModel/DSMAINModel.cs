using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;

namespace CHubModel.ExtensionModel
{
    public class DSMAINModel
    {
        public List<V_DS_ORDER_BASE> mainList { get; set; }
        public V_DS_ORDER_BASE main { get; set; }
        public string days { get; set; }
    }
}

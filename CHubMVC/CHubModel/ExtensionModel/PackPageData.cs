using CHubDBEntity.UnmanagedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class PackPageData
    {
        public V_RP_PACK_H_PRINT Header { get; set; }
        public List<V_RP_PACK_D_PRINT> Details { get; set; }
    }
}

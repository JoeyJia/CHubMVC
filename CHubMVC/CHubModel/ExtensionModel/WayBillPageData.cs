using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;

namespace CHubModel.ExtensionModel
{
    public class WayBillPageData
    {
        public V_RP_WAYBILL_H_PRINT Header { get; set; }

        public List<V_RP_WAYBILL_D_PRINT> Details { get; set; }
    }
}

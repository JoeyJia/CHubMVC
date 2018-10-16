using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class TC_HSCODE_MST
    {
        public string HSCODE { get; set; }
        public string HSCODE_DESC { get; set; }
        public string TC_CATEGORY_ID { get; set; }
        public string NOTE1 { get; set; }
        public string NOTE2 { get; set; }
        public string NOTE3 { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? RECORD_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public decimal? TAX_REFUND_RATE { get; set; }
        public decimal? MFN_RATE { get; set; }
        public string UOM { get; set; }
        public string REGULATION { get; set; }
    }
}

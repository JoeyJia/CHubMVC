using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_E_SO_DETAIL
    {
        public string SO_NO { get; set; }
        public string LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public string UNIT_PRICE { get; set; }
        public string QTY { get; set; }
        public string PRINT_PART_NO { get; set; }
        public string MOVEX_PART_NO { get; set; }
        public string CUSTOMER_PARTNO { get; set; }
        public string LINE_NOTE { get; set; }
        public string LIST_PRICE { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

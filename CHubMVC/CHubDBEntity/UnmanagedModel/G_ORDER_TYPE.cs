using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class G_ORDER_TYPE
    {
        public string SYSID { get; set; }
        public Int64 DUE_DATE_TYPE { get; set; }
        public string DUE_DATE_CODE { get; set; }
        public string DUE_DATE_DESC { get; set; }
        public string WAREHOUSE { get; set; }
        public Int64 PRIORITY_CODE_TYPE { get; set; }
        public string PRIORITY_CODE { get; set; }
        public string PRIORITY_DESC { get; set; }
        public int CUST_DOWN_ORDER_FLAG { get; set; }
        public string MOVE_ORDER_TYPE { get; set; }
    }
}

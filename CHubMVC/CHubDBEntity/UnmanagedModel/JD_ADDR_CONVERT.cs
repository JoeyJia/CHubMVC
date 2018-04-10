using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class JD_ADDR_CONVERT
    {
        public decimal JID { get; set; }
        public string GOMS_ADDR { get; set; }
        public string CONVERTED_ADDR { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? RECORD_DATE { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class IHUB_LOAD_TYPE
    {
        public string LOAD_TYPE { get; set; }
        public string LOAD_DESC { get; set; }
        public decimal FIRST_ROW { get; set; }
        public string LOAD_FMT { get; set; }
        public string LOAD_TEMPLATE { get; set; }
        public string DELIMITER { get; set; }
    }
}

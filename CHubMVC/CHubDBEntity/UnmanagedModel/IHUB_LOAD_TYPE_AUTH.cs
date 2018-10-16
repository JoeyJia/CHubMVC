using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class IHUB_LOAD_TYPE_AUTH
    {
        public string LOAD_TYPE { get; set; }
        public string APP_USER { get; set; }
        public string AUTH_DESC { get; set; }
        public DateTime AUTH_BEGIN_DATE { get; set; }
        public DateTime AUTH_END_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

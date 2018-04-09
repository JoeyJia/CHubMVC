using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class APP_SECURE_PROC_ASSIGN
    {
        public string SECURE_ID { get; set; }
        public string APP_USER { get; set; }
        public string COMMENTS { get; set; }
        public string ACTIVEIND { get; set; }
        public DateTime? CREATE_DATE { get; set; }
    }
}

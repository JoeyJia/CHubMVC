using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class MD_COMPANY_SNAP
    {
        public string COMPANY_CODE { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_NAME_CN { get; set; }
        public string PLANNER { get; set; }
        public string PLANNER_CODE { get; set; }
        public string DEFAULT_MSC { get; set; }
        public string NOTE { get; set; }
        public string ACTIVE_IND { get; set; }
        public DateTime? LOAD_DATE { get; set; }
        public string GSM_SUPPLIER_NO { get; set; }
        public string VENDOR_SITE_ID { get; set; }
        public string BPA_NO { get; set; }
        public string INSURANCE_CODE { get; set; }
        public string DS_TRACK { get; set; }
        public string DS_TRACK_EML { get; set; }
        public string COMPANY_NAME_SHORT { get; set; }
        public decimal RETURN_ALLOW_DAYS { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class MD_REQ_SR
    {
        public decimal SR_REQ_NO { get; set; }
        public string PART_NO { get; set; }
        public string COMPANY_CODE { get; set; }
        public decimal? PRICE { get; set; }
        public decimal? MOQ { get; set; }
        public decimal? LT { get; set; }
        public string IS_COMMON { get; set; }
        public string SR_COMMENTS { get; set; }
        public string SR_STATUS { get; set; }
        public DateTime? RECORD_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? REQ_DATE { get; set; }
        public string ERR_MSG { get; set; }

    }
}

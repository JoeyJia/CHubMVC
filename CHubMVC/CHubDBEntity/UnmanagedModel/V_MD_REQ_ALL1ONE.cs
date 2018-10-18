using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_MD_REQ_ALL1ONE
    {
        public decimal MD_REQ_NO { get; set; }
        public string REQ_DESC { get; set; }
        public DateTime? REQ_DATE { get; set; }
        public string REQ_BY { get; set; }
        public decimal REQ_LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public string PART_DESC { get; set; }
        public string CHECK_EXIST { get; set; }
        public string CHECK_PRI_SUP { get; set; }
        public string CHECK_PRI_PB { get; set; }
        public string CHECK_PRI_BPA { get; set; }
        public string CHECK_COST { get; set; }
        public string GLOBAL_PARTNO { get; set; }
        public string GLOBAL_DESC { get; set; }
        public string PART_DESC_SHORT { get; set; }
        public string GLOBAL_PART_DESC { get; set; }
        public string COMM_PART { get; set; }
        public string PRODUCT_GROUP_ID { get; set; }
        public string REQ_STATUS { get; set; }
        public string REQ_STATUS_DESC { get; set; }
        public string APP_STATUS { get; set; }
        public string APP_STATUS_DESC { get; set; }
        public string APP_COMMENTS { get; set; }
        public string NOTE { get; set; }
        public DateTime? DTE_EXIST { get; set; }
        public DateTime? DTE_PRI_SUP { get; set; }
        public DateTime? DTE_PRI_PB { get; set; }
        public DateTime? DTE_PRI_BPA { get; set; }
        public DateTime? DTE_COST { get; set; }
        public DateTime? APP_DATE { get; set; }
        public string GROUP_DESC { get; set; }
        public string FIN_RESP_CODE { get; set; }
        public string FBC { get; set; }
        public string FSBC { get; set; }
        public string SOUCE_PLANT { get; set; }
        public string PLANNER { get; set; }
        public string SPC_MAJOR_CODE { get; set; }
        public string SPC_INTERMEDIATE_CODE { get; set; }
        public string SPC_MINIMUM_CODE { get; set; }
        public string MTO_FLAG { get; set; }
        public string PVC_CODE { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
        public string FLEX3 { get; set; }
        public string CHECK_GLOBAL_PRT { get; set; }
        public string LOOKS_LIKE { get; set; }
        public string SR_COMMENTS { get; set; }
    }
}

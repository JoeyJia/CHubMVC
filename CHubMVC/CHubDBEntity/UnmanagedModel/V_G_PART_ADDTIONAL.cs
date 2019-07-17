using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_G_PART_ADDTIONAL
    {
        public string PART_NO { get; set; }
        public string PRINT_PART_NO { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string PAPER_ID { get; set; }
        public string NOTE { get; set; }
        public decimal MOQ_OVERRIDE { get; set; }
        public string MSG_ADDT1 { get; set; }
        public string MSG_ADDT2 { get; set; }
        public string MSG_ADDT3 { get; set; }
        public string QC_NOTE { get; set; }
        public decimal PACKING_MOQ { get; set; }
        public string LABEL_TRACE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESC_CN { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class LabelPrintModel
    {
        public List<LabelPrintItems> items { get; set; }
        public string LabelTYPE { get; set; }
        public string ShipmentNo { get; set; }
        public string BoxNumber { get; set; }
        public string PartNumber_RP { get; set; }
        public string PartNumber_GOMS { get; set; }
        public string ASN_NO { get; set; }
        public string PRINT_PART_NO_ASN { get; set; }
        public string PART_NO_ASN { get; set; }
        public string PRINT_PART_NO_UParts { get; set; }
        public string PART_NO_UParts { get; set; }
    }

    public class LabelPrintItems
    {
        public string VID { get; set; }
        public string PART_NO { get; set; }
        public int COPIES { get; set; }

    }

}

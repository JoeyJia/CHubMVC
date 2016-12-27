using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.ExtensionModel
{
    public class ExVAliasAddr
    {
        public string aliasName { get; set; } 

        public string Description { get; set; } 

        public string SysID { get; set; } 

        public string CustomerNo { get; set; } 

        public string Name { get; set; } 

        public string ActiveInd { get; set; } 

        public int? Bill2Location { get; set; } 

        public long? Ship2Location { get; set; } 

        public long? DestLocation { get; set; } //SPL

        public string DestName { get; set; } 

        public string DestAddr1 { get; set; }

        public string DestAddr2 { get; set; }

        public string DestAddr3 { get; set; }

        public string DestCity { get; set; }

        public string DestContact { get; set; }

        public string DestPhone { get; set; }

        public string DestFax { get; set; }

        public string DestState { get; set; }

        public string DestCountry { get; set; }

        public string DestZip { get; set; }

        public DateTime? RecordDateOSD { get; set; }

        public string WareHouse { get; set; }

        public string DestAttention { get; set; }

        public string LocalDestName { get; set; }

        public string LocalDestAddr1 { get; set; }

        public string LocalDestAddr2 { get; set; }

        public string LocalDestAddr3 { get; set; }

        public string LocalDestCity { get; set; }

        public string LocalDestCountry { get; set; }

        public string LocalDestState { get; set; }

        public DateTime? RecordDateOSDL { get; set; }

        public decimal Days { get; set; }

        public decimal Distance { get; set; }

        public byte? KGFreight { get; set; }

    }
}

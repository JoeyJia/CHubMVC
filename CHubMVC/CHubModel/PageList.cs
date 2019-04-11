using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class PageList
    {
        public string SPACE_DESC { get; set; }
        public string URL { get; set; }
        public string DISPLAY { get; set; }
        public string DESCRIPTION { get; set; }
        public string ICON { get; set; }
        public string ICON_DESC { get; set; }
        public List<PageList> pages { get; set; }
    }
}

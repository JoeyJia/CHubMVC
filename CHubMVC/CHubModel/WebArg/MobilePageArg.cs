using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class MobilePageArg
    {
        public string SPACE_DESC { get; set; }

        public List<MobilePageList> pagelist { get; set; }

    }

    public class MobilePageList
    {
        public string URL { get; set; }
        public string ICON { get; set; }
        public string ICON_DESC { get; set; }
    }


}

using CHubModel.ExtensionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class OrderSaveArg
    {
        public ExVAliasAddr headInfo { get; set; }

        public ExVAliasAddr altHeadInfo { get; set; }
    }
}

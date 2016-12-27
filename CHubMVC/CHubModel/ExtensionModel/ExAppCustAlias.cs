using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubModel.ExtensionModel
{
    public class ExAppCustAlias
    {
        public string ALIAS_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string ACTIVEIND { get; set; }

        public string APP_USER { get; set; }
        public string DEFAULT_FLAG { get; set; }


        public void CopyFromAppCustAlias(APP_CUST_ALIAS aca)
        {
            this.ALIAS_NAME = aca.ALIAS_NAME;
            this.DESCRIPTION = aca.DESCRIPTION;
            this.ACTIVEIND = aca.ACTIVEIND;
        }
    }
}

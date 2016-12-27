using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class APP_CUST_ALIAS_BLL
    {
        private readonly APP_CUST_ALIAS_DAL dal;

        public APP_CUST_ALIAS_BLL()
        {
            dal = new APP_CUST_ALIAS_DAL();
        }
        public APP_CUST_ALIAS_BLL(CHubEntities db)
        {
            dal = new APP_CUST_ALIAS_DAL(db);
        }

        public List<ExAppCustAlias> GetAppCustAliasByAppUser(string appUser)
        {
            APP_USER_ALIAS_LINK_DAL ualDAL = new APP_USER_ALIAS_LINK_DAL(dal.db);
            List<APP_USER_ALIAS_LINK> ualList = ualDAL.GetUserAliasLinks(appUser);

            List<ExAppCustAlias> exACAList = new List<ExAppCustAlias>();
            foreach (var item in ualList)
            {
                APP_CUST_ALIAS aca = dal.GetAppCustAlias(item.ALIAS_NAME);
                ExAppCustAlias exACA = new ExAppCustAlias();
                exACA.CopyFromAppCustAlias(aca);
                exACA.APP_USER = item.APP_USER;
                exACA.DEFAULT_FLAG = item.DEFAULT_FLAG;

                exACAList.Add(exACA);
            }
            return exACAList;
        }

    }
}

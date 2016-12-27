using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class APP_CUST_ALIAS_DAL : BaseDAL
    {
        public APP_CUST_ALIAS_DAL()
            : base() { }

        public APP_CUST_ALIAS_DAL(CHubEntities db)
            : base(db) { }

        /// <summary>
        /// Valid appNotice datetime.now between begindate and end date
        /// </summary>
        /// <returns></returns>
        public List<APP_CUST_ALIAS> GetAppCustAliasList(List<string> aliasNames)
        {
            return db.APP_CUST_ALIAS.Where(a => aliasNames.Contains(a.ALIAS_NAME) && a.ACTIVEIND== CHubConstValues.IndY).ToList();
        }

        public APP_CUST_ALIAS GetAppCustAlias(string aliasName)
        {
            return db.APP_CUST_ALIAS.FirstOrDefault(a => a.ALIAS_NAME == aliasName && a.ACTIVEIND == CHubConstValues.IndY);
        }


    }
}

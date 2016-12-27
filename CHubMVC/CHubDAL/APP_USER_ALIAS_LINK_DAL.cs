using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using static CHubCommon.CHubEnum;

namespace CHubDAL
{
    public class APP_USER_ALIAS_LINK_DAL : BaseDAL
    {
        public APP_USER_ALIAS_LINK_DAL()
            : base() { }

        public APP_USER_ALIAS_LINK_DAL(CHubEntities db)
            : base(db) { }

        public List<APP_USER_ALIAS_LINK> GetUserAliasLinks(string appUser)
        {
            return db.APP_USER_ALIAS_LINK.Where(a => a.APP_USER == appUser).ToList();
        }


    }
}

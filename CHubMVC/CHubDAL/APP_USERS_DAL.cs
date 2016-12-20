using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using static CHubCommon.CHubEnum;

namespace CHubDAL
{
    public class APP_USERS_DAL : BaseDAL
    {
        public APP_USERS_DAL()
            : base() { }

        public APP_USERS_DAL(CHubEntities db)
            : base(db) { }

        public APP_USERS GetAppUserByDomainName(string appUser)
        {
            return db.APP_USERS.FirstOrDefault(a => a.APP_USER == appUser); 
        }

        public bool AddAppUserWithRole(APP_USERS appUser)
        {
            this.Add(appUser);
            return true;
        }



    }
}

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

        public bool AddAppUserWithRole(APP_USERS appUser, APP_USER_ROLE_LINK urLink)
        {
            this.Add(appUser, false);
            //Check work or not
            this.Add(urLink, false);
            this.SaveChanges();
            return true;
        }



    }
}

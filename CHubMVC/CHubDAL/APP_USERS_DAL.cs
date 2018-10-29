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
            : base()
        { }

        public APP_USERS_DAL(CHubEntities db)
            : base(db)
        { }

        public APP_USERS GetAppUserByDomainName(string appUser)
        {
            //string sql = string.Format(@"select * from APP_USERS where APP_USER='{0}'", appUser);
            //var result = db.Database.SqlQuery<APP_USERS>(sql).ToList();

            var result = db.APP_USERS.FirstOrDefault(a => a.APP_USER == appUser);
            return result; //db.APP_USERS.FirstOrDefault(a => a.APP_USER == appUser);
        }

        public bool AddAppUserWithRole(APP_USERS appUser)
        {
            this.Add(appUser);
            return true;
        }

        public void UpdateLAST_LOGIN(APP_USERS appUser)
        {
            this.Update(appUser);
        }

    }
}

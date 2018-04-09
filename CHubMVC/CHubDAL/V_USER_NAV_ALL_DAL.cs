using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_USER_NAV_ALL_DAL : BaseDAL
    {
        public V_USER_NAV_ALL_DAL()
            : base()
        { }

        public V_USER_NAV_ALL_DAL(CHubEntities db)
            : base(db)
        { }

        public List<V_USER_NAV_ALL> GetNavByAppUser(string appUser)
        {
            string sql = string.Format(@"select * from V_USER_NAV_ALL where APP_USER='{0}' and nvl(mobile,'xx') <>'Y'", appUser);
            var result = db.Database.SqlQuery<V_USER_NAV_ALL>(sql).ToList();

            return result; //db.V_USER_NAV_ALL.Where(a => a.APP_USER == appUser && a.MOBILE != "Y").ToList();  //db.V_USER_NAV_ALL.Where(a => a.APP_USER == appUser && a.MOBILE != "Y").ToList();
        }


        public List<V_USER_NAV_ALL> GetMobilePageByAppUser(string appuser)
        {
            return db.V_USER_NAV_ALL.Where(a => a.APP_USER == appuser && a.MOBILE == "Y").ToList();
        }




    }
}

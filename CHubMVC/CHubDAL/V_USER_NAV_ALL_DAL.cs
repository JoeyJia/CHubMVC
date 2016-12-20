using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_USER_NAV_ALL_DAL:BaseDAL
    {
        public V_USER_NAV_ALL_DAL()
            : base() { }

        public V_USER_NAV_ALL_DAL(CHubEntities db)
            : base(db) { }

        public List<V_USER_NAV_ALL> GetNavByAppUser(string appUser)
        {
            return db.V_USER_NAV_ALL.Where(a => a.APP_USER == appUser).ToList();
        }


    }
}

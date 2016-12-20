using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_USER_NAV_ALL_BLL
    {
        private readonly V_USER_NAV_ALL_DAL dal;

        public V_USER_NAV_ALL_BLL()
        {
            dal = new V_USER_NAV_ALL_DAL();
        }
        public V_USER_NAV_ALL_BLL(CHubEntities db)
        {
            dal = new V_USER_NAV_ALL_DAL(db);
        }

        public List<V_USER_NAV_ALL> GetNavByAppUser(string appUser)
        {
            return dal.GetNavByAppUser(appUser);
        }

    }
}

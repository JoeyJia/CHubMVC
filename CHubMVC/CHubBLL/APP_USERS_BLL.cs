using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using static CHubCommon.CHubEnum;

namespace CHubBLL
{
    public class APP_USERS_BLL
    {
        private readonly APP_USERS_DAL dal;

        public APP_USERS_BLL()
        {
            dal = new APP_USERS_DAL();
        }
        public APP_USERS_BLL(CHubEntities db)
        {
            dal = new APP_USERS_DAL(db);
        }

        public APP_USERS GetAppUserByDomainName(string appUser)
        {
            return dal.GetAppUserByDomainName(appUser);
        }

        public bool AddAppUserWithRole(string domainName, CHubRoles role=CHubRoles.Public)
        {
            APP_USERS appUser = new APP_USERS()
            {
                APP_USER = domainName,
                FIRST_NAME = "User",
                LAST_NAME = string.Empty,
                CREATED_BY = domainName,
                CREATE_DATE = DateTime.Now,
                STATUS = UserStates.A.ToString()
            };

            APP_USER_ROLE_LINK urLink = new APP_USER_ROLE_LINK()
            {
                APP_USER = domainName,
                ROLE_NAME = role.ToString().ToLower(),
                CREATE_DATE = DateTime.Now
            };

            return dal.AddAppUserWithRole(appUser, urLink);
        }

    }
}

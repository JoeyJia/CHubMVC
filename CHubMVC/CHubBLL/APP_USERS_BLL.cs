using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using static CHubCommon.CHubEnum;
using CHubCommon;

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
            return dal.GetAppUserByDomainName(appUser.ToLower());
        }

        /// <summary>
        /// Add a app User ,will default add a "Public" role link by a trigger
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public bool AddAppUserWithRole(string domainName,string firstName,string mail)
        {
            APP_USERS appUser = new APP_USERS()
            {
                APP_USER = domainName.ToLower(),
                FIRST_NAME = firstName,
                LAST_NAME = string.Empty,
                EMAIL_ADDR = mail??string.Format(CHubConstValues.EmailFormat, domainName),
                CREATED_BY = domainName,
                CREATE_DATE = DateTime.Now,
                STATUS = UserStatesEnum.A.ToString(),
                LAST_LOGIN = DateTime.Now
            };

            return dal.AddAppUserWithRole(appUser);
        }

        public void UpdateLAST_LOGIN(APP_USERS appUser)
        {
            appUser.LAST_LOGIN = DateTime.Now;
            dal.UpdateLAST_LOGIN(appUser);
        }

    }
}

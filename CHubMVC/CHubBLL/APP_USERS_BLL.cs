using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using static CHubCommon.CHubEnum;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

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
        public List<APP_USERS> UsrMntSearch(string APP_USER)
        {
            return dal.UsrMntSearch(APP_USER);
        }
        public void UsrMntSave(APP_USERS appUser)
        {
            dal.UsrMntSave(appUser);
        }
        public List<APP_USER_ROLE_LINK> UsrMntRoles(string APP_USER)
        {
            return dal.UsrMntRoles(APP_USER);
        }
        public List<APP_ROLES> UsrMntRolesNew(string APP_USER)
        {
            return dal.UsrMntRolesNew(APP_USER);
        }
        public void UsrMntRolesDelete(APP_USER_ROLE_LINK arg)
        {
            dal.UsrMntRolesDelete(arg);
        }
        public APP_ROLES UsrMntRolesRole_NameChange(string ROLE_NAME)
        {
            return dal.UsrMntRolesRole_NameChange(ROLE_NAME);
        }
        public List<APP_SECURE_PROC_ASSIGN> UsrMntSecurity(string APP_USER)
        {
            return dal.UsrMntSecurity(APP_USER);
        }
        public void UsrMntSecurityDelete(APP_SECURE_PROC_ASSIGN arg)
        {
            dal.UsrMntSecurityDelete(arg);
        }
        public List<APP_SECURE_PROCESS> UsrMntSecurityNew(string APP_USER)
        {
            return dal.UsrMntSecurityNew(APP_USER);
        }
        public void UsrMntRolesSave(APP_USER_ROLE_LINK item)
        {
            dal.UsrMntRolesSave(item);
        }
        public void UsrMntSecuritySave(APP_SECURE_PROC_ASSIGN item)
        {
            dal.UsrMntSecuritySave(item);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using static CHubCommon.CHubEnum;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;

namespace CHubDAL
{
    public class APP_USERS_DAL : BaseDAL
    {
        private CHubCommonHelper ccHelper;
        public APP_USERS_DAL()
            : base()
        {
            ccHelper = new CHubCommonHelper();
        }

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

        public List<APP_USERS> UsrMntSearch(string APP_USER)
        {
            string sql = string.Format(@"select * from APP_USERS where APP_USER like '%{0}%'", APP_USER);
            var result = db.Database.SqlQuery<APP_USERS>(sql).ToList();
            return result;
        }
        public void UsrMntSave(APP_USERS appUser)
        {
            this.Update(appUser);
        }
        public List<APP_USER_ROLE_LINK> UsrMntRoles(string APP_USER)
        {
            string sql = string.Format(@"select * from APP_USER_ROLE_LINK where APP_USER='{0}'", APP_USER);
            var result = db.Database.SqlQuery<APP_USER_ROLE_LINK>(sql).ToList();
            return result;
        }
        public List<APP_ROLES> UsrMntRolesNew(string APP_USER)
        {
            string sql = string.Format(@"select * from APP_ROLES where ROLE_NAME not in (select ROLE_NAME from APP_USER_ROLE_LINK where APP_USER='{0}')", APP_USER);
            var result = db.Database.SqlQuery<APP_ROLES>(sql).ToList();
            return result;
        }
        public void UsrMntRolesDelete(APP_USER_ROLE_LINK arg)
        {
            this.Delete(arg);
        }
        public APP_ROLES UsrMntRolesRole_NameChange(string ROLE_NAME)
        {
            string sql = string.Format("select * from APP_ROLES where ROLE_NAME='{0}'", ROLE_NAME);
            var result = db.Database.SqlQuery<APP_ROLES>(sql).ToList().First();
            return result;
        }

        public List<APP_SECURE_PROC_ASSIGN> UsrMntSecurity(string APP_USER)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where APP_USER='{0}'", APP_USER);
            var result = db.Database.SqlQuery<APP_SECURE_PROC_ASSIGN>(sql).ToList();
            return result;
        }

        public void UsrMntSecurityDelete(APP_SECURE_PROC_ASSIGN arg)
        {
            string sql = string.Format("delete from APP_SECURE_PROC_ASSIGN where APP_USER='{0}' and SECURE_ID='{1}'", arg.APP_USER, arg.SECURE_ID);
            ccHelper.ExecuteNonQuery(sql);
        }

        public List<APP_SECURE_PROCESS> UsrMntSecurityNew(string APP_USER)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROCESS where SECURE_ID not in (select SECURE_ID from APP_SECURE_PROC_ASSIGN where APP_USER='{0}')", APP_USER);
            var result = db.Database.SqlQuery<APP_SECURE_PROCESS>(sql).ToList();
            return result;
        }
        public void UsrMntRolesSave(APP_USER_ROLE_LINK item)
        {
            string sql = string.Format(@"insert into APP_USER_ROLE_LINK(APP_USER,ROLE_NAME,COMMENTS,CREATED_BY,CREATE_DATE)
                                            values('{0}','{1}','{2}','{3}',sysdate)", item.APP_USER, item.ROLE_NAME, item.COMMENTS, item.CREATED_BY);
            ccHelper.ExecuteNonQuery(sql);
        }
        public void UsrMntSecuritySave(APP_SECURE_PROC_ASSIGN item)
        {
            string sql = string.Format(@"insert into APP_SECURE_PROC_ASSIGN(SECURE_ID,APP_USER,COMMENTS,ACTIVEIND,CREATE_DATE)
                                            values('{0}','{1}','{2}','{3}',sysdate)", item.SECURE_ID, item.APP_USER, item.COMMENTS, item.ACTIVEIND);
            ccHelper.ExecuteNonQuery(sql);
        }
    }
}

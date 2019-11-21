using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class APP_WELCOME_BLL
    {
        private readonly APP_WELCOME_DAL dal;

        public APP_WELCOME_BLL()
        {
            dal = new APP_WELCOME_DAL();
        }
        public APP_WELCOME_BLL(CHubEntities db)
        {
            dal = new APP_WELCOME_DAL(db);
        }

        public List<APP_WELCOME> GetAppWelcome()
        {
            return dal.GetAppWelcome();
        }
        public List<APP_ENV> GetAppEnv()
        {
            return dal.GetAppEnv();
        }

    }
}

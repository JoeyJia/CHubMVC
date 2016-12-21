using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class APP_PAGES_BLL
    {
        private readonly APP_PAGES_DAL dal;

        public APP_PAGES_BLL()
        {
            dal = new APP_PAGES_DAL();
        }
        public APP_PAGES_BLL(CHubEntities db)
        {
            dal = new APP_PAGES_DAL(db);
        }

        public APP_PAGES GetPageByName(string pageName)
        {
            return dal.GetPageByName(pageName);
        }

    }
}

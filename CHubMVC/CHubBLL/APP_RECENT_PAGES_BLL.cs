using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class APP_RECENT_PAGES_BLL
    {
        private readonly APP_RECENT_PAGES_DAL dal;

        public APP_RECENT_PAGES_BLL()
        {
            dal = new APP_RECENT_PAGES_DAL();
        }
        public APP_RECENT_PAGES_BLL(CHubEntities db)
        {
            dal = new APP_RECENT_PAGES_DAL(db);
        }

        public List<APP_RECENT_PAGES> GetRecentPages()
        {
            return dal.GetRecentPages();
        }

    }
}

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

        public List<APP_RECENT_PAGES> GetRecentPages(string appUser)
        {
            return dal.GetRecentPages(appUser);
        }

        public bool Add(APP_RECENT_PAGES model)
        {
            dal.Add(model);
            return true;
        }

        public bool Update(APP_RECENT_PAGES model)
        {
            dal.Update(model);
            return true;
                
        }

        public bool Add(string userName,string pageName,string pageUrl)
        {
            APP_RECENT_PAGES appRP = dal.GetSpecifyRecentPage(userName, pageName);
            if (appRP == null)
            {
                appRP = new APP_RECENT_PAGES()
                {
                    APP_USER = userName,
                    PAGE_NAME = pageName,
                    PAGE_URL = pageUrl,
                    LAST_ACTIVITY = DateTime.Now
                };
                this.Add(appRP);
            }
            else
            {
                appRP.LAST_ACTIVITY = DateTime.Now;
                this.Update(appRP);
            }
            return true;
            
        }

    }
}

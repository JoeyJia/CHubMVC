using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;
using CHubCommon;

namespace CHubDAL
{
    public class APP_RECENT_PAGES_DAL : BaseDAL
    {
        public APP_RECENT_PAGES_DAL()
            : base()
        { }

        public APP_RECENT_PAGES_DAL(CHubEntities db)
            : base(db)
        { }

        public List<APP_RECENT_PAGES> GetRecentPages(string appUser)
        {
            //For primary key constraint, surely distinct
            //10 days need to be changed to get form m_parameter tables
            //string sql = string.Format(@"select * from APP_RECENT_PAGES");
            //var result = db.Database.SqlQuery<APP_RECENT_PAGES>(sql).ToList();
            //result = result.Where(a => a.APP_USER == appUser && DbFunctions.AddDays(a.LAST_ACTIVITY, 10) > DateTime.Now).ToList();

            var result = db.APP_RECENT_PAGES.Where(a => a.APP_USER == appUser && DbFunctions.AddDays(a.LAST_ACTIVITY, 10) > DateTime.Now).ToList();

            return result; //db.APP_RECENT_PAGES.Where(a => a.APP_USER == appUser && DbFunctions.AddDays(a.LAST_ACTIVITY, 10) > DateTime.Now).ToList();//.Distinct(new PropertyComparer<APP_RECENT_PAGES>("PAGE_URL"))
        }

        public APP_RECENT_PAGES GetSpecifyRecentPage(string appUser, string pageName)
        {
            string sql = string.Format(@"select * from APP_RECENT_PAGES where APP_USER='{0}' and PAGE_NAME='{1}'", appUser, pageName);
            var result = db.Database.SqlQuery<APP_RECENT_PAGES>(sql).ToList();

            return result.FirstOrDefault(); //db.APP_RECENT_PAGES.FirstOrDefault(a => a.APP_USER == appUser && a.PAGE_NAME == pageName);
        }


    }
}

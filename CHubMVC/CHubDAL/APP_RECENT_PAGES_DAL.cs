using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class APP_RECENT_PAGES_DAL : BaseDAL
    {
        public APP_RECENT_PAGES_DAL()
            : base() { }

        public APP_RECENT_PAGES_DAL(CHubEntities db)
            : base(db) { }

        public List<APP_RECENT_PAGES> GetRecentPages()
        {
            return db.APP_RECENT_PAGES.OrderBy(a => a.PAGE_NAME).ToList();
        }


    }
}

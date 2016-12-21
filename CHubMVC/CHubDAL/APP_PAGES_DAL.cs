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
    public class APP_PAGES_DAL : BaseDAL
    {
        public APP_PAGES_DAL()
            : base() { }

        public APP_PAGES_DAL(CHubEntities db)
            : base(db) { }

        public List<APP_PAGES> GetPagesByNames(List<string> pageNames)
        {
            return db.APP_PAGES.Where(a => pageNames.Contains(a.PAGE_NAME)).ToList();
        }

        public APP_PAGES GetPageByName(string pageName)
        {
            return db.APP_PAGES.FirstOrDefault(a => a.PAGE_NAME == pageName);
        }




    }
}

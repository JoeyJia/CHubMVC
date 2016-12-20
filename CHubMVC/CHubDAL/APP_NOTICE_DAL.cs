using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class APP_NOTICE_DAL : BaseDAL
    {
        public APP_NOTICE_DAL()
            : base() { }

        public APP_NOTICE_DAL(CHubEntities db)
            : base(db) { }

        /// <summary>
        /// Valid appNotice datetime.now between begindate and end date
        /// </summary>
        /// <returns></returns>
        public List<APP_NOTICE> GetValidAppNotice()
        {
            return db.APP_NOTICE.Where(a => DateTime.Now > a.BEGIN_DATE && DateTime.Now < a.END_DATE).OrderBy(a => a.NOTICE_ID).ToList();
        }


    }
}

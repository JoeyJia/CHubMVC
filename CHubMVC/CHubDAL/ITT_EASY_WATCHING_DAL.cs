using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class ITT_EASY_WATCHING_DAL : BaseDAL
    {
        public ITT_EASY_WATCHING_DAL()
            : base() { }

        public ITT_EASY_WATCHING_DAL(CHubEntities db)
            : base(db) { }

        public List<ITT_EASY_WATCHING> GetWatchingList(decimal token)
        {
            return db.ITT_EASY_WATCHING.Where(a => a.EASY_QUREY_TOKEN == token).ToList();
        }
    }
}

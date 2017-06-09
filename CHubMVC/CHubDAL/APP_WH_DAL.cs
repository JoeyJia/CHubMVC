using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class APP_WH_DAL : BaseDAL
    {
        public APP_WH_DAL()
            : base() { }

        public APP_WH_DAL(CHubEntities db)
            : base(db) { }

        public List<APP_WH> GetAppWHList()
        {
            return db.APP_WH.ToList();
        }

    }
}

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

        public string GetDefPrinter(string whID)
        {
            APP_WH wh = db.APP_WH.FirstOrDefault(a => a.WH_ID == whID);
            if (wh == null)
                return null;
            else
                return wh.DEF_PACK_PRINTER;

        }

    }
}

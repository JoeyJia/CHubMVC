using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class APP_WELCOME_DAL : BaseDAL
    {
        public APP_WELCOME_DAL()
            : base() { }

        public APP_WELCOME_DAL(CHubEntities db)
            : base(db) { }

        public List<APP_WELCOME> GetAppWelcome()
        {
            return db.APP_WELCOME.OrderBy(a=>a.MSG_SEQ).ToList();
        }


    }
}

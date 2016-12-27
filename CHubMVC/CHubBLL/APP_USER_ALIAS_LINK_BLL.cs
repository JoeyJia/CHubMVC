using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class APP_USER_ALIAS_LINK_BLL
    {
        private readonly APP_USER_ALIAS_LINK_DAL dal;

        public APP_USER_ALIAS_LINK_BLL()
        {
            dal = new APP_USER_ALIAS_LINK_DAL();
        }
        public APP_USER_ALIAS_LINK_BLL(CHubEntities db)
        {
            dal = new APP_USER_ALIAS_LINK_DAL(db);
        }

    }
}

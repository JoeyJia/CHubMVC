using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_USER_APPLY_BLL
    {
        private readonly EW_USER_APPLY_DAL dal;

        public EW_USER_APPLY_BLL()
        {
            dal = new EW_USER_APPLY_DAL();
        }
        public EW_USER_APPLY_BLL(CHubEntities db)
        {
            dal = new EW_USER_APPLY_DAL(db);
        }

    }
}

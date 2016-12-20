using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class APP_NOTICE_BLL
    {
        private readonly APP_NOTICE_DAL dal;

        public APP_NOTICE_BLL()
        {
            dal = new APP_NOTICE_DAL();
        }
        public APP_NOTICE_BLL(CHubEntities db)
        {
            dal = new APP_NOTICE_DAL(db);
        }

        public List<APP_NOTICE> GetValidAppNotice()
        {
            return dal.GetValidAppNotice();
        }

    }
}

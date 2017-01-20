using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class M_APPS_BLL
    {
        private readonly M_APPS_DAL dal;

        public M_APPS_BLL()
        {
            dal = new M_APPS_DAL();
        }
        public M_APPS_BLL(CHubEntities db)
        {
            dal = new M_APPS_DAL(db);
        }

        public List<M_APPS> GetMAppList()
        {
            return dal.GetMAppsList();
        }

    }
}

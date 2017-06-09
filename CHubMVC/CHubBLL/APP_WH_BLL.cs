using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class APP_WH_BLL
    {
        private readonly APP_WH_DAL dal;

        public APP_WH_BLL()
        {
            dal = new APP_WH_DAL();
        }
        public APP_WH_BLL(CHubEntities db)
        {
            dal = new APP_WH_DAL(db);
        }

        public List<APP_WH> GetAppWHList()
        {
            return dal.GetAppWHList();
        }

    }
}

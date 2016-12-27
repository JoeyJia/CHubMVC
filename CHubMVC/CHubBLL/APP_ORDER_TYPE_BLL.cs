using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class APP_ORDER_TYPE_BLL
    {
        private readonly APP_ORDER_TYPE_DAL dal;

        public APP_ORDER_TYPE_BLL()
        {
            dal = new APP_ORDER_TYPE_DAL();
        }
        public APP_ORDER_TYPE_BLL(CHubEntities db)
        {
            dal = new APP_ORDER_TYPE_DAL(db);
        }

        public List<APP_ORDER_TYPE> GetValidOrderType()
        {
            return dal.GetValidOrderType();
        }

    }
}

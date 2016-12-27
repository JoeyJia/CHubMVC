using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class APP_ORDER_STATUS_BLL
    {
        private readonly APP_ORDER_STATUS_DAL dal;

        public APP_ORDER_STATUS_BLL()
        {
            dal = new APP_ORDER_STATUS_DAL();
        }
        public APP_ORDER_STATUS_BLL(CHubEntities db)
        {
            dal = new APP_ORDER_STATUS_DAL(db);
        }

        public List<APP_ORDER_STATUS> GetValidOrderSTATUS()
        {
            return dal.GetValidOrderStatus();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class APP_ORDER_STATUS_DAL : BaseDAL
    {
        public APP_ORDER_STATUS_DAL()
            : base() { }

        public APP_ORDER_STATUS_DAL(CHubEntities db)
            : base(db) { }


        public List<APP_ORDER_STATUS> GetValidOrderStatus()
        {
            return db.APP_ORDER_STATUS.ToList();
        }


    }
}

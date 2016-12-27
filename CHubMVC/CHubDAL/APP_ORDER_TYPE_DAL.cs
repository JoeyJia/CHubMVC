using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class APP_ORDER_TYPE_DAL : BaseDAL
    {
        public APP_ORDER_TYPE_DAL()
            : base() { }

        public APP_ORDER_TYPE_DAL(CHubEntities db)
            : base(db) { }


        public List<APP_ORDER_TYPE> GetValidOrderType()
        {
            return db.APP_ORDER_TYPE.Where(a => a.ACTIVEIND == CHubConstValues.IndY).ToList();
        }


    }
}

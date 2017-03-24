using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_SHIPPING_ALL_BASE_BLL
    {
        private readonly V_SHIPPING_ALL_BASE_DAL dal;

        public V_SHIPPING_ALL_BASE_BLL()
        {
            dal = new V_SHIPPING_ALL_BASE_DAL();
        }
        public V_SHIPPING_ALL_BASE_BLL(CHubEntities db)
        {
            dal = new V_SHIPPING_ALL_BASE_DAL(db);
        }

        public V_SHIPPING_ALL_BASE GetFirstBaseInfo(string wayBillNo)
        {
            return dal.GetFirstBaseInfo(wayBillNo);
        }


    }
}

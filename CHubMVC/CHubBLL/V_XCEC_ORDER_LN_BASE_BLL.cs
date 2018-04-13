using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;

namespace CHubBLL
{
    public class V_XCEC_ORDER_LN_BASE_BLL
    {
        private V_XCEC_ORDER_LN_BASE_DAL dal;

        public V_XCEC_ORDER_LN_BASE_BLL()
        {
            dal = new V_XCEC_ORDER_LN_BASE_DAL();
        }

        public V_XCEC_ORDER_LN_BASE_BLL(CHubEntities db)
        {
            dal = new V_XCEC_ORDER_LN_BASE_DAL(db);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_RP_WAYBILL_D_BASE_BLL
    {
        private readonly V_RP_WAYBILL_D_BASE_DAL dal;

        public V_RP_WAYBILL_D_BASE_BLL()
        {
            dal = new V_RP_WAYBILL_D_BASE_DAL();
        }
        public V_RP_WAYBILL_D_BASE_BLL(CHubEntities db)
        {
            dal = new V_RP_WAYBILL_D_BASE_DAL(db);
        }

    }
}

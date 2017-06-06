using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_WAYBILL_TYPE_BLL
    {
        private readonly RP_WAYBILL_TYPE_DAL dal;

        public RP_WAYBILL_TYPE_BLL()
        {
            dal = new RP_WAYBILL_TYPE_DAL();
        }
        public RP_WAYBILL_TYPE_BLL(CHubEntities db)
        {
            dal = new RP_WAYBILL_TYPE_DAL(db);
        }

    }
}

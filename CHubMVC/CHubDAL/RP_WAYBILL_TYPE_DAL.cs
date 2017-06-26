using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class RP_WAYBILL_TYPE_DAL : BaseDAL
    {
        public RP_WAYBILL_TYPE_DAL()
            : base() { }

        public RP_WAYBILL_TYPE_DAL(CHubEntities db)
            : base(db) { }

        public List<RP_WAYBILL_TYPE> GetWayBillType()
        {
            return db.RP_WAYBILL_TYPE.ToList();
        }

        public RP_WAYBILL_TYPE GetSpecifyItem(string wayBillID)
        {
            return db.RP_WAYBILL_TYPE.FirstOrDefault(a => a.WAYBILL_ID == wayBillID);
        }

    }
}

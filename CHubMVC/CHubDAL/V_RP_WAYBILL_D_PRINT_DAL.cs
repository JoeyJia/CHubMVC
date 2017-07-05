using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;

namespace CHubDAL
{
    public class V_RP_WAYBILL_D_PRINT_DAL : BaseDAL
    {
        public V_RP_WAYBILL_D_PRINT_DAL()
            : base() { }

        public V_RP_WAYBILL_D_PRINT_DAL(CHubEntities db)
            : base(db) { }

        public List<V_RP_WAYBILL_D_PRINT> GetDByShipNos(List<string> shipNoList)
        {

            string sqlin = shipNoList.ToSqlInStr();
            string sql = string.Format(@"select * from V_RP_WAYBILL_D_PRINT
where ship_id in ({0})", sqlin);

            var result = db.Database.SqlQuery<V_RP_WAYBILL_D_PRINT>(sql);
            return result.OrderBy(a=>a.SHIP_ID).ThenBy(a=>a.LODNUM).ToList();
        }

    }
}

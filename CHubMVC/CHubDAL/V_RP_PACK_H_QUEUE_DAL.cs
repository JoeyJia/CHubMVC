using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_RP_PACK_H_QUEUE_DAL : BaseDAL
    {
        public V_RP_PACK_H_QUEUE_DAL()
            : base() { }

        public V_RP_PACK_H_QUEUE_DAL(CHubEntities db)
            : base(db) { }

        public List<string> GetDistinctWHID()
        {

            string sql = "select distinct WH_ID from v_rp_pack_H_queue";

            var result = db.Database.SqlQuery<string>(sql);
            return result.ToList();

        }

        public List<string> GetShipIDByWhID(string whID)
        {
            string sql = string.Format(@"select SHIP_ID from v_rp_pack_H_queue 
where WH_ID='{0}'", whID);

            var result = db.Database.SqlQuery<string>(sql);
            return result.ToList();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_RP_PACK_D_PRINT_DAL : BaseDAL
    {
        public V_RP_PACK_D_PRINT_DAL()
            : base() { }

        public V_RP_PACK_D_PRINT_DAL(CHubEntities db)
            : base(db) { }

        public List<V_RP_PACK_D_PRINT> GetPackDetails(string shipNo)
        {
            string sql = string.Format(@"select * from V_RP_PACK_D_PRINT
 where  SHIP_ID='{0}'", shipNo);

            var result = db.Database.SqlQuery<V_RP_PACK_D_PRINT>(sql);
            return result.ToList();

        }

    }
}

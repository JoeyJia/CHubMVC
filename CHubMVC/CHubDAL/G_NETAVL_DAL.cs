using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class G_NETAVL_DAL : BaseDAL
    {
        public G_NETAVL_DAL()
            : base() { }

        public G_NETAVL_DAL(CHubEntities db)
            : base(db) { }

        public decimal GetSpecifyNETAVL(string sysId, string partNo, string wareHouse)
        {
            try
            {
                string sql = string.Format("select  * from G_NETAVL where SYSID='{0}' and PART_NO='{1}' and WAREHOUSE='{2}'",
                                             sysId, partNo, wareHouse);
                var list = db.Database.SqlQuery<G_NETAVL>(sql).ToList();
                if (list != null && list.Count == 1)
                {
                    decimal result = list[0].NETAVL;
                    return result < 0 ? 0 : result;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

    }
}

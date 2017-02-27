using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class G_OESALES_CATALOG_DAL : BaseDAL
    {
        public G_OESALES_CATALOG_DAL()
            : base() { }

        public G_OESALES_CATALOG_DAL(CHubEntities db)
            : base(db) { }

        public G_OESALES_CATALOG GetOESalesCatalog(string sysId, string partNo)
        {
            try
            {
                string sql = string.Format("select  * from G_OESALES_CATALOG where SYSID='{0}' and PART_NO='{1}'",
                                             sysId, partNo);
                var list = db.Database.SqlQuery<G_OESALES_CATALOG>(sql).ToList();
                if (list != null && list.Count == 1)
                {
                    return list[0];
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}

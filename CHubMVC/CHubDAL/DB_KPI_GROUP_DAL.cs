using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class DB_KPI_GROUP_DAL : BaseDAL
    {
        public DB_KPI_GROUP_DAL()
            : base() { }

        public DB_KPI_GROUP_DAL(CHubEntities db)
            : base(db) { }

        public List<DB_KPI_GROUP> GetKPIGroups()
        {
            return db.DB_KPI_GROUP.Where(a => a.ACTIVEIND == CHubConstValues.IndY).OrderBy(a=>a.GROUP_DESC_SHORT).ToList();
        }

    }
}

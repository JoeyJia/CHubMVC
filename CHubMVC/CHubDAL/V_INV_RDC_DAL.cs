using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_INV_RDC_DAL : BaseDAL
    {
        public V_INV_RDC_DAL()
            : base() { }

        public V_INV_RDC_DAL(CHubEntities db)
            : base(db) { }

        public List<V_INV_RDC> GetRDCData(string partNo)
        {
            string sql = string.Format("select * from V_INV_RDC where PART_NO='{0}'", partNo);
            return db.Database.SqlQuery<V_INV_RDC>(sql).ToList();
        }
    

    }
}

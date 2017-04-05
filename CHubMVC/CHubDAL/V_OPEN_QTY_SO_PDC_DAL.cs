using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_OPEN_QTY_SO_PDC_DAL : BaseDAL
    {
        public V_OPEN_QTY_SO_PDC_DAL()
            : base() { }

        public V_OPEN_QTY_SO_PDC_DAL(CHubEntities db)
            : base(db) { }

        public List<V_OPEN_QTY_SO_PDC> GetOpenPDCData(string partNo)
        {
            string sql = string.Format("select * from V_OPEN_QTY_SO_PDC where PART_NO='{0}'", partNo);
            return db.Database.SqlQuery<V_OPEN_QTY_SO_PDC>(sql).ToList();
        }

    }
}

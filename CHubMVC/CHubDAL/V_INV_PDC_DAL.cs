using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_INV_PDC_DAL : BaseDAL
    {
        public V_INV_PDC_DAL()
            : base()
        { }

        public V_INV_PDC_DAL(CHubEntities db)
            : base(db)
        { }

        public List<V_INV_PDC> GetPDCData(string partNo)
        {
            string sql = string.Format("select * from V_INV_PDC where PART_NO='{0}'", partNo);
            return db.Database.SqlQuery<V_INV_PDC>(sql).ToList();
        }

        public List<V_INV_PDC> GetPDCData(string partNo, string print_partNo)
        {
            string sql = string.Format(@"select * from select * from V_INV_PDC where 1=1");
            if (!string.IsNullOrEmpty(partNo))
                sql += string.Format(@" and PART_NO ='{0}'", partNo);
            if (!string.IsNullOrEmpty(print_partNo))
                sql += string.Format(@" and PRINT_PART_NO ='{0}'", print_partNo);
            return db.Database.SqlQuery<V_INV_PDC>(sql).ToList();
        }


    }
}

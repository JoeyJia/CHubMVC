using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_PLABEL_BASE_DAL : BaseDAL
    {
        public V_PLABEL_BASE_DAL()
            : base() { }

        public V_PLABEL_BASE_DAL(CHubEntities db)
            : base(db) { }

        public List<V_PLABEL_BASE> QueryByPart(string printPartNo, string partNo, string status)
        {
            string sql = "select* from V_PLABEL_BASE where 1=1";
            if (!string.IsNullOrEmpty(printPartNo))
                sql += string.Format(" and PRINT_PART_NO ='{0}'",printPartNo);

            if (!string.IsNullOrEmpty(partNo))
                sql += string.Format(" and PART_NO ='{0}'", partNo);

            if (!string.IsNullOrEmpty(status))
                sql += string.Format(" and PART_CATALOG='{0}'", status);

            var result = db.Database.SqlQuery<V_PLABEL_BASE>(sql);
            return result.ToList();
        }


    }
}

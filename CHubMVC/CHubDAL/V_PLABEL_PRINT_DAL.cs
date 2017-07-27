using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_PLABEL_PRINT_DAL : BaseDAL
    {
        public V_PLABEL_PRINT_DAL()
            : base() { }

        public V_PLABEL_PRINT_DAL(CHubEntities db)
            : base(db) { }


        public List<V_PLABEL_PRINT> GetLabelPrintData(string partNo, string labelCode)
        {
            string sql = string.Format(@"select * from V_PLABEL_print
where PART_NO = '{0}'
and LABEL_CODE = '{1}'", partNo, labelCode);
            

            var result = db.Database.SqlQuery<V_PLABEL_PRINT>(sql);
            return result.ToList();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_IA_REPORT_PRINT_DAL : BaseDAL
    {
        public V_IA_REPORT_PRINT_DAL() : base()
        {

        }

        public V_IA_REPORT_PRINT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_IA_REPORT_PRINT> IAReportPrint(string LODNUM)
        {
            string sql = string.Format(@"select * from V_IA_REPORT_PRINT where LODNUM='{0}'", LODNUM);
            var result = db.Database.SqlQuery<V_IA_REPORT_PRINT>(sql);
            return result.ToList();
        }


    }
}

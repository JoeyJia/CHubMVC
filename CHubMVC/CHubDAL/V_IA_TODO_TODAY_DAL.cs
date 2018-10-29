using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_IA_TODO_TODAY_DAL : BaseDAL
    {
        public V_IA_TODO_TODAY_DAL() : base()
        {

        }

        public V_IA_TODO_TODAY_DAL(CHubEntities db) : base(db)
        {

        }


        public void RunRefreshProc()
        {
            this.CheckCultureInfoForDate();
            db.P_IA_TODO_REFRESH();
        }

        public List<V_IA_TODO_TODAY> GetIATodoToday(string WH_ID, string Cust)
        {
            this.CheckCultureInfoForDate();
            string sql = string.Format(@"select * from V_IA_TODO_TODAY where WH_ID='{0}'", WH_ID);
            if (!string.IsNullOrEmpty(Cust))
                sql += string.Format(@" and ADRNAM like '%{0}%'", Cust);
            var result = db.Database.SqlQuery<V_IA_TODO_TODAY>(sql);
            return result.ToList();
        }

    }
}

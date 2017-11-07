using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;

namespace CHubDAL
{
    public class TC_HSCODE_MST_DAL : BaseDAL
    {
        public TC_HSCODE_MST_DAL() : base()
        {

        }

        public TC_HSCODE_MST_DAL(CHubEntities db) : base(db)
        {

        }


        public List<TC_HSCODE_MST> GetHSCODEByCode(string HSCODE)
        {
            string sql = string.Format(@"select * from TC_HSCODE_MST where HSCODE like '%{0}%'", HSCODE);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<TC_HSCODE_MST>(sql);
            return result.ToList();
        }

        public bool IsExistHSCODE(string HSCODE)
        {
            var result = db.TC_HSCODE_MST.AsNoTracking().FirstOrDefault(h => h.HSCODE == HSCODE);
            if (result != null)
                return true;
            else
                return false;
        }

        public void AddOrUpdate(TC_HSCODE_MST tc, string type)
        {
            this.CheckCultureInfoForDate();
            if (type == "Update")
            {
                var ts = db.TC_HSCODE_MST.AsNoTracking().FirstOrDefault(h => h.HSCODE == tc.HSCODE);
                ts.HSCODE_DESC = tc.HSCODE_DESC;
                ts.TC_CATEGORY_ID = tc.TC_CATEGORY_ID;
                ts.NOTE1 = tc.NOTE1;
                ts.NOTE2 = tc.NOTE2;
                ts.NOTE3 = tc.NOTE3;
                ts.RECORD_DATE = tc.RECORD_DATE;
                base.Update(ts);
            }
            else
                base.Add(tc);
        }


        public List<TC_HSCODE_AUDIT> GetHsCodeAudit(string HSCODE)
        {
            string sql = string.Format(@"select * from TC_HSCODE_AUDIT where HSCODE ='{0}' order by ACTIVITY_DATE desc", HSCODE);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<TC_HSCODE_AUDIT>(sql);
            return result.ToList();
        }



    }
}

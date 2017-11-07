using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
  public  class TMS_ADR_AUTO_CORRECT_DAL:BaseDAL
    {
        public TMS_ADR_AUTO_CORRECT_DAL() : base()
        {

        }

        public TMS_ADR_AUTO_CORRECT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<TMS_ADR_AUTO_CORRECT> SearchADRCRT(string ADRNAM, string ADRLN1, int LOAD_DATE)
        {
            string startday = DateTime.Now.AddDays(-LOAD_DATE).ToString("yyyy-MM-dd 00:00:00");
            string endday = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            string sql = string.Format(@"select * from TMS_ADR_AUTO_CORRECT 
                                        where load_date >= to_date('{0}','yyyy-mm-dd hh24:mi:ss') 
                                          and load_date <= to_date('{1}','yyyy-mm-dd hh24:mi:ss')",startday,endday);
            if (!string.IsNullOrEmpty(ADRNAM))
                sql += string.Format(@" and ADRNAM like '%{0}%'", ADRNAM);
            if (!string.IsNullOrEmpty(ADRLN1))
                sql += string.Format(@" and ADRLN1 like '%{0}%'", ADRLN1);

            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<TMS_ADR_AUTO_CORRECT>(sql);
            return result.ToList();
        }


        public void SaveADRCRT(TMS_ADR_AUTO_CORRECT tc)
        {
            this.CheckCultureInfoForDate();
            base.Update(tc, true);
        }




    }
}

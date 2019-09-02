using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class EXP_EXCHANGE_RATE_DAL
    {
        private CHubCommonHelper ccHelper;
        public EXP_EXCHANGE_RATE_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<string> GetEXCHANGE_TYPE()
        {
            List<string> types = new List<string>();
            string sql = "select distinct EXCHANGE_TYPE from EXP_EXCHANGE_RATE";
            var result = ccHelper.ExecuteSqlToList<EXP_EXCHANGE_RATE>(sql);
            foreach (var item in result)
            {
                types.Add(item.EXCHANGE_TYPE);
            }
            return types;
        }

        public List<EXP_EXCHANGE_RATE> GetTableResult(string EXCHANGE_TYPE)
        {
            string sql = string.Format(@"select * from EXP_EXCHANGE_RATE where 1=1");
            if (!string.IsNullOrEmpty(EXCHANGE_TYPE))
                sql += string.Format(@" and EXCHANGE_TYPE='{0}'", EXCHANGE_TYPE);
            sql += string.Format(@" order by CREATE_DATE desc");
            var result = ccHelper.ExecuteSqlToList<EXP_EXCHANGE_RATE>(sql);
            return result;
        }

        public void InsertOrUpdateEXPRATE(string EXCHANGETYPE, string STARTDATE, EXP_EXCHANGE_RATE eer, string method, string appUser)
        {
            string sql = string.Empty;

            if (method == "Update")
                sql = string.Format(@"Update EXP_EXCHANGE_RATE Set 
                                        START_DATE =to_date('{0}','yyyy/mm/dd'),
                                        END_DATE =to_date('{1}','yyyy/mm/dd'),
                                        EXCHANGE_RATE='{2}',
                                        NOTE='{3}'
                                        Where EXCHANGE_TYPE='{4}' And START_DATE=to_date('{5}','yyyy/mm/dd')",
                                        eer.START_DATE.ToString("yyyy/MM/dd"), eer.END_DATE.ToString("yyyy/MM/dd"), eer.EXCHANGE_RATE, eer.NOTE, EXCHANGETYPE, STARTDATE);
            else
                sql = string.Format(@"Insert Into  EXP_EXCHANGE_RATE(EXCHANGE_TYPE,START_DATE,END_DATE,EXCHANGE_RATE,NOTE,CREATE_DATE,CREATED_BY)
                                    Values('{0}',to_date('{1}','yyyy/mm/dd'),to_date('{2}','yyyy/mm/dd'),'{3}','{4}',sysdate,'{5}')",
                                    eer.EXCHANGE_TYPE, eer.START_DATE.ToString("yyyy/MM/dd"), eer.END_DATE.ToString("yyyy/MM/dd"), eer.EXCHANGE_RATE, eer.NOTE, appUser);
            ccHelper.ExecuteNonQuery(sql);
        }

    }
}

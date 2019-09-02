using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using System.Data;

namespace CHubDAL
{
    public class PRC_DAL
    {
        private CHubCommonHelper ccHelper;
        public PRC_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public DataTable PRCVerify()
        {
            string sql = string.Format(@"select * from V_PRC_LATEST_VERIFY");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
    }
}

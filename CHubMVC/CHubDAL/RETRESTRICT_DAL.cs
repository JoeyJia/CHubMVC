using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CHubDAL
{
    public class RETRESTRICT_DAL
    {
        private CHubCommonHelper ccHelper;
        public RETRESTRICT_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_RET_PART_RESTRICT> RetRestrictSearch(string PART_NO)
        {
            string sql = string.Format(@"select * from V_RET_PART_RESTRICT where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO like '%{0}%'", PART_NO);
            var result = ccHelper.Search<V_RET_PART_RESTRICT>(sql);
            return result;

        }

        public void RetRestrictSave(RetRestrictArg arg)
        {
            string sql = string.Format(@"Update RET_PART_RESTRICT set RETURN_RESTRICT=:RETURN_RESTRICT,RETURN_MOQ=:RETURN_MOQ where PART_NO=:PART_NO");
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":RETURN_RESTRICT",OracleDbType.Varchar2),
                new OracleParameter(":RETURN_MOQ",OracleDbType.Decimal),
                new OracleParameter(":PART_NO",OracleDbType.Varchar2)
            };
            param[0].Value = arg.RETURN_RESTRICT;
            param[1].Value = Convert.ToDecimal(arg.RETURN_MOQ);
            param[2].Value = arg.PART_NO;

            ccHelper.AddOrUpdateWithParams(sql, param);
        }

        public string GetSql()
        {
            string sql = string.Format(@"select get_sql('RET_RESTRICT','','','','','') from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public DataTable GetDataTableBySql(string sql)
        {
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

    }
}

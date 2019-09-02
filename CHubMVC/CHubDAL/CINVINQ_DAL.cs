using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CHubDAL
{
    public class CINVINQ_DAL
    {
        private CHubCommonHelper ccHelper;
        public CINVINQ_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<EXP_COMM_INV> SearchEXP_COMM_INV(string COMM_INV_ID, string SHIP_TO_INDEX, string CREATE_DATE, string CREATED_BY)
        {
            string sql = string.Format(@"select * from V_EXP_COMM_INV where 1=1");
            if (!string.IsNullOrEmpty(COMM_INV_ID))
                sql += string.Format(@" and COMM_INV_ID='{0}'", COMM_INV_ID);
            if (!string.IsNullOrEmpty(SHIP_TO_INDEX))
                sql += string.Format(@" and SHIP_TO_INDEX like '%{0}%'", SHIP_TO_INDEX);
            if (!string.IsNullOrEmpty(CREATE_DATE))
                sql += string.Format(@" and CREATE_DATE>sysdate-{0}", Convert.ToInt32(CREATE_DATE));
            if (!string.IsNullOrEmpty(CREATED_BY))
                sql += string.Format(@" and CREATED_BY='{0}'", CREATED_BY);
            var result = ccHelper.ExecuteSqlToList<EXP_COMM_INV>(sql);
            return result;
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='Y'", SECURE_ID, APP_USER);
            var result = ccHelper.ExecuteSqlToList<APP_SECURE_PROC_ASSIGN>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public void ExecP_EXP_INV_DISCARD(string COMM_INV_ID)
        {
            ccHelper.ExecP_EXP_INV_DISCARD(COMM_INV_ID);
        }

        public void ExecP_EXP_INV_COMP(string COMM_INV_ID)
        {
            ccHelper.ExecP_EXP_INV_COMP(COMM_INV_ID);
        }

        public List<V_EXP_STAGE_BASE> SearchDetailsByV_EXP_STAGE_BASE(string COMM_INV_ID)
        {
            string sql = string.Format(@"select * from V_EXP_STAGE_BASE where COMM_INV_ID='{0}'", COMM_INV_ID);
            var result = ccHelper.ExecuteSqlToList<V_EXP_STAGE_BASE>(sql);
            return result;
        }

        public void ExecP_EXP_UPT_HSCODE(string COMM_INV_ID)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_comm_id",OracleDbType.Decimal)
            };
            param[0].Value = decimal.Parse(COMM_INV_ID);
            param[0].Direction = ParameterDirection.Input;

            ccHelper.ExecProcedureWithParams("P_EXP_UPT_HSCODE", param);
        }

        public string CallFuncF_EXP_HSCODE_CHK(string COMM_INV_ID)
        {
            string sql = string.Format(@"select F_EXP_HSCODE_CHK('{0}') from dual", COMM_INV_ID);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string CallFunc_GET_SQL(string COMM_INV_ID)
        {
            string sql = string.Format(@"select GET_SQL('COMMINV','{0}','','','','') from dual", COMM_INV_ID);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public DataTable RunSql(string sql)
        {
            DataTable dt = new DataTable();
            dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

        public void SureToReDiscard(string COMM_INV_ID)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_inv_id",OracleDbType.Decimal)
            };
            param[0].Value = Convert.ToDecimal(COMM_INV_ID);
            param[0].Direction = ParameterDirection.Input;

            ccHelper.ExecProcedureWithParams("P_EXP_INV_DISCARD", param);
        }
    }
}

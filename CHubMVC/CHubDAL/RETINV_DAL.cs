using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using Oracle.ManagedDataAccess.Client;
using CHubCommon;
using CHubModel.WebArg;
using System.Data;

namespace CHubDAL
{
    public class RETINV_DAL
    {
        private CHubCommonHelper ccHelper;
        public RETINV_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_RET_USER_CUST_LINK> GetCustomer_No(string AppUser)
        {
            string sql = string.Format(@"select * from V_RET_USER_CUST_LINK where APP_USER='{0}'", AppUser);
            var result = ccHelper.Search<V_RET_USER_CUST_LINK>(sql);
            return result;
        }

        public List<V_RET_INV_RETURN_H> RetInvSearch(RetInvArg arg)
        {
            string sql = string.Format(@"select * from V_RET_INV_RETURN_H where 1=1 and CUSTOMER_NO='{0}'", arg.CUSTOMER_NO);
            if (!string.IsNullOrEmpty(arg.REFERENCE_NO))
                sql += string.Format(@" and REFERENCE_NO='{0}'", arg.REFERENCE_NO);
            if (!string.IsNullOrEmpty(arg.INVOICE_ID))
                sql += string.Format(@" and INVOICE_ID='{0}'", arg.INVOICE_ID);
            if (!string.IsNullOrEmpty(arg.RET_REQ_NO))
                sql += string.Format(@" and RET_REQ_NO='{0}'", arg.RET_REQ_NO);
            if (arg.INVOICE_DATE > 0)
                sql += string.Format(@" and INVOICE_DATE>sysdate-{0}", arg.INVOICE_DATE);
            var result = ccHelper.Search<V_RET_INV_RETURN_H>(sql);
            return result;
        }

        public List<V_RET_INV_RETURN_D> GetRetInvDetailModal(string INVOICE_ID)
        {
            string sql = string.Format(@"select * from V_RET_INV_RETURN_D where INVOICE_ID='{0}'", INVOICE_ID);
            var result = ccHelper.Search<V_RET_INV_RETURN_D>(sql);
            return result;
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            var result = ccHelper.CheckSecurity(SECURE_ID, APP_USER);
            return result;
        }

        public void RunProc_P_RET_Match(string INVOICE_ID)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_invoice_id",OracleDbType.Varchar2)
            };
            param[0].Value = INVOICE_ID;
            param[0].Direction = System.Data.ParameterDirection.Input;

            ccHelper.ExecProcedure("P_RET_Match", param);
        }

        public void RunProc_P_RET_INV_CLOSE(string INVOICE_ID)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_inv_id",OracleDbType.Varchar2)
            };
            param[0].Value = INVOICE_ID;
            param[0].Direction = ParameterDirection.Input;

            ccHelper.ExecProcedure("P_RET_INV_CLOSE", param);
        }

        public string RetInvGetSql(string INVOICE_ID)
        {
            string sql = string.Format(@"select get_sql('RET_INV','{0}','','','','') from dual", INVOICE_ID);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public DataTable RunRetInvSql(string sql)
        {
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }


    }
}

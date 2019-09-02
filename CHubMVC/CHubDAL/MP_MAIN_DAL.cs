using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using System.Data;
using CHubModel.WebArg;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class MP_MAIN_DAL
    {
        private CHubCommonHelper ccHelper;
        public MP_MAIN_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<E_WH_MST> GetWarehouseList()
        {
            string sql = string.Format(@"select * from E_WH_MST where ACTIVEIND = 'Y'");
            var result = ccHelper.ExecuteSqlToList<E_WH_MST>(sql);
            return result;
        }
        public List<E_ORDER_STATUS> GetOrderStatusList()
        {
            string sql = string.Format(@"select * from E_ORDER_STATUS");
            var result = ccHelper.ExecuteSqlToList<E_ORDER_STATUS>(sql);
            return result;
        }
        public List<E_SHIP_METHOD_MST> GetShipMethodList()
        {
            string sql = string.Format(@"select * from E_SHIP_METHOD_MST where ACTIVEIND = 'Y'");
            var result = ccHelper.ExecuteSqlToList<E_SHIP_METHOD_MST>(sql);
            return result;
        }
        public List<E_ORDER_TYPE_MST> GetOrderTypeList()
        {
            string sql = string.Format(@"select * from E_ORDER_TYPE_MST where ACTIVEIND = 'Y'");
            var result = ccHelper.ExecuteSqlToList<E_ORDER_TYPE_MST>(sql);
            return result;
        }

        public List<V_E_SO_HEADER> MP_MAINSearch(MPMainArg arg)
        {
            string sql = string.Format(@"select * from V_E_SO_HEADER where 1=1");
            if (!string.IsNullOrEmpty(arg.WAREHOUSE))
                sql += string.Format(@" and WAREHOUSE='{0}'", arg.WAREHOUSE);
            if (!string.IsNullOrEmpty(arg.ORDER_TYPE))
                sql += string.Format(@" and ORDER_TYPE='{0}'", arg.ORDER_TYPE);
            if (!string.IsNullOrEmpty(arg.ORDER_STATUS))
                sql += string.Format(@" and ORDER_STATUS='{0}'", arg.ORDER_STATUS);
            if (!string.IsNullOrEmpty(arg.SHIP_METHOD))
                sql += string.Format(@" and SHIP_METHOD='{0}'", arg.SHIP_METHOD);
            if (!string.IsNullOrEmpty(arg.SHIP_NAME))
                sql += string.Format(@" and SHIP_NAME like '%{0}%'", arg.SHIP_NAME);
            if (!string.IsNullOrEmpty(arg.CUSTOMER_NO))
                sql += string.Format(@" and CUSTOMER_NO='{0}'", arg.CUSTOMER_NO);
            if (!string.IsNullOrEmpty(arg.SO_NO))
                sql += string.Format(@" and SO_NO='{0}'", arg.SO_NO);
            if (!string.IsNullOrEmpty(arg.LAST_DAY))
                sql += string.Format(@" and CREATE_DATE>=sysdate-{0}", Convert.ToDecimal(arg.LAST_DAY));
            var result = ccHelper.ExecuteSqlToList<V_E_SO_HEADER>(sql);
            return result;
        }

        public List<V_E_SO_DETAIL> MP_MAINDetail(string SO_NO)
        {
            string sql = string.Format(@"select * from V_E_SO_DETAIL where SO_NO='{0}'", SO_NO);
            var result = ccHelper.ExecuteSqlToList<V_E_SO_DETAIL>(sql);
            return result;
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return ccHelper.CheckSecurity(SECURE_ID, APP_USER);
        }
        public string RunF_CHECK_IN_GOMS(string SO_NO)
        {
            string sql = string.Format(@"select F_CHECK_IN_GOMS('{0}') from dual", SO_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public void MP_MAINConfirm(string SO_NO, string Code, string Reason)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":V_SO_NO",OracleDbType.Varchar2),
                new OracleParameter(":V_opt_code",OracleDbType.Varchar2),
                new OracleParameter(":V_Reason",OracleDbType.Varchar2)
            };
            param[0].Value = SO_NO;
            param[1].Value = Code;
            param[2].Value = Reason;
            ccHelper.ExecProcedureWithParams("P_MP_ORDER_OPT", param);
        }

    }
}

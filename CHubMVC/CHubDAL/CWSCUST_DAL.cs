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
    public class CWSCUST_DAL
    {
        private CHubCommonHelper ccHelper;
        public CWSCUST_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<APP_OECUSTOMER_MST> CwsCustSearch(string CUSTOMER_NO)
        {
            string sql = string.Format(@"select * from APP_OECUSTOMER_MST where CUSTOMER_NO like '%{0}%'", CUSTOMER_NO);
            var result = ccHelper.ExecuteSqlToList<APP_OECUSTOMER_MST>(sql);
            return result;
        }

        public List<OA_TYPE_MST> GetOA_TYPE()
        {
            string sql = string.Format(@"select * from OA_TYPE_MST");
            var result = ccHelper.ExecuteSqlToList<OA_TYPE_MST>(sql);
            return result;
        }

        public void CwsCustSave(APP_OECUSTOMER_MST arg)
        {
            //string sql = string.Format(@"update APP_OECUSTOMER_MST set CWS_FLAG=:CWS_FLAG,FLAG01=:FLAG01,FLAG02=:FLAG02,FLAG03=:FLAG03,NOTE=:NOTE,LOAD_DATE=sysdate where CUSTOMER_NO=:CUSTOMER_NO");
            //OracleParameter[] param = new OracleParameter[] {
            //    new OracleParameter(":CWS_FLAG",OracleDbType.Varchar2),
            //    new OracleParameter(":FLAG01",OracleDbType.Varchar2),
            //    new OracleParameter(":FLAG02",OracleDbType.Varchar2),
            //    new OracleParameter(":FLAG03",OracleDbType.Varchar2),
            //    new OracleParameter(":NOTE",OracleDbType.Varchar2),
            //    new OracleParameter(":CUSTOMER_NO",OracleDbType.Varchar2)
            //};
            //param[0].Value = arg.CWS_FLAG;
            //param[1].Value = arg.FLAG01;
            //param[2].Value = arg.FLAG02;
            //param[3].Value = arg.FLAG03;
            //param[4].Value = arg.NOTE;
            //param[5].Value = arg.CUSTOMER_NO;

            string sql = string.Format(@"update APP_OECUSTOMER_MST set CUST_EML_OA='{0}',OA_TYPE='{1}',CC_EML_OA='{2}',CUST_EML_DS='{3}',CC_EML_DS='{4}',
                                            CWS_FLAG='{5}',FAR_FLAG='{6}',CSR='{7}',CSR_EML='{8}',FLAG01='{9}',FLAG02='{10}',FLAG03='{11}',NOTE='{12}'
                                            where CUSTOMER_NO='{13}'", arg.CUST_EML_OA, arg.OA_TYPE, arg.CC_EML_OA, arg.CUST_EML_DS, arg.CC_EML_DS,
                                            arg.CWS_FLAG, arg.FAR_FLAG, arg.CSR, arg.CSR_EML, arg.FLAG01, arg.FLAG02, arg.FLAG03, arg.NOTE, arg.CUSTOMER_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public APP_OECUSTOMER_MST CwsCustEdit(string CUSTOMER_NO)
        {
            string sql = string.Format(@"select * from APP_OECUSTOMER_MST where CUSTOMER_NO='{0}'", CUSTOMER_NO);
            var result = ccHelper.ExecuteSqlToList<APP_OECUSTOMER_MST>(sql).First();
            return result;
        }
        public DataTable CheckAPP_USER(string APP_USER)
        {
            string sql = string.Format(@"select * from APP_USERS where lower(APP_USER)='{0}'", APP_USER.ToLower());
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
    }
}

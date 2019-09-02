using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using Oracle.ManagedDataAccess.Client;

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

        public void CwsCustSave(APP_OECUSTOMER_MST arg)
        {
            string sql = string.Format(@"update APP_OECUSTOMER_MST set CWS_FLAG=:CWS_FLAG,FLAG01=:FLAG01,FLAG02=:FLAG02,FLAG03=:FLAG03,NOTE=:NOTE,LOAD_DATE=sysdate where CUSTOMER_NO=:CUSTOMER_NO");
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":CWS_FLAG",OracleDbType.Varchar2),
                new OracleParameter(":FLAG01",OracleDbType.Varchar2),
                new OracleParameter(":FLAG02",OracleDbType.Varchar2),
                new OracleParameter(":FLAG03",OracleDbType.Varchar2),
                new OracleParameter(":NOTE",OracleDbType.Varchar2),
                new OracleParameter(":CUSTOMER_NO",OracleDbType.Varchar2)
            };
            param[0].Value = arg.CWS_FLAG;
            param[1].Value = arg.FLAG01;
            param[2].Value = arg.FLAG02;
            param[3].Value = arg.FLAG03;
            param[4].Value = arg.NOTE;
            param[5].Value = arg.CUSTOMER_NO;

            ccHelper.AddOrUpdateWithParams(sql, param);
        }
    }
}

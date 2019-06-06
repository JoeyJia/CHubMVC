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
    public class DS_DAL
    {
        private CHubCommonHelper ccHelper;
        public DS_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public string GetLOAD_BATCH()
        {
            string sql = string.Format(@"select SEQ_IHUB_ASN.nextval from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public void IhubASNUpload(IHUB_ASN_STG item)
        {
            string sql = string.Format(@"insert into IHUB_ASN_STG (
                                                ASN_NO,LINE_NO,COMPANY_CODE,SHIP_DATE,
                                                PO_NO,PO_LINE_NO,PO_REL_NO,PART_NO,QTY_SHIPPED,COO,NOTE,
                                                LOAD_BATCH,LOAD_BY
                                                )
                                                values(
                                                '{0}',{1},'{2}',to_date('{3}','yyyy/mm/dd'),
                                                '{4}','{5}','{6}','{7}',{8},'{9}','{10}',
                                                {11},'{12}'
                                                )",
                                                item.ASN_NO, item.LINE_NO, item.COMPANY_CODE, item.SHIP_DATE.ToString("yyyy/MM/dd"),
                                                item.PO_NO, item.PO_LINE_NO, item.PO_REL_NO, item.PART_NO, item.QTY_SHIPPED, item.COO, item.NOTE,
                                                item.LOAD_BATCH, item.LOAD_BY);
            ccHelper.Update(sql);
        }

        public string RunF_IHUB_ASN_LOAD_CHK(decimal LOAD_BATCH)
        {
            string sql = string.Format(@"select F_IHUB_ASN_LOAD_CHK({0}) from dual", LOAD_BATCH);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public void ExecP_IHUB_ASN_LOAD_POST(decimal LOAD_BATCH)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":v_load_batch",OracleDbType.Decimal)
            };
            param[0].Value = LOAD_BATCH;
            param[0].Direction = System.Data.ParameterDirection.Input;
            ccHelper.ExecProcedure("P_IHUB_ASN_LOAD_POST", param);
        }

        public List<V_IHUB_ASN> IhubASNSearch(string COMPANY_CODE, string ASN_NO, int LOAD_DAY)
        {
            string sql = string.Format(@"select * from V_IHUB_ASN where 1=1");
            if (!string.IsNullOrEmpty(COMPANY_CODE))
                sql += string.Format(@" and COMPANY_CODE='{0}'", COMPANY_CODE);
            if (!string.IsNullOrEmpty(ASN_NO))
                sql += string.Format(@" and ASN_NO='{0}'", ASN_NO);
            if (LOAD_DAY > 0)
                sql += string.Format(@" and LOAD_DATE>sysdate-{0}", LOAD_DAY);
            var result = ccHelper.Search<V_IHUB_ASN>(sql);
            return result;
        }

        public string GetCOMPANY_NAME(string COMPANY_CODE)
        {
            string sql = string.Format(@"select COMPANY_NAME from V_MD_COMPANY_SNAP where COMPANY_CODE='{0}'", COMPANY_CODE);
            var result = ccHelper.ExecuteSql(sql);
            return result;
        }

    }
}

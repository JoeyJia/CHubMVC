using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class MP_CUSTBANK_DAL
    {
        private CHubCommonHelper ccHelper;
        public MP_CUSTBANK_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_E_CUST_BANKING> MP_CUSTBANKSearch(V_E_CUST_BANKING SearchCondition)
        {
            string sql = string.Format(@"select * from V_E_CUST_BANKING where 1=1");
            if (!string.IsNullOrEmpty(SearchCondition.CUSTOMER_NO))
                sql += string.Format(@" and CUSTOMER_NO='{0}'", SearchCondition.CUSTOMER_NO);
            if (!string.IsNullOrEmpty(SearchCondition.BANK_PAYER))
                sql += string.Format(@" and BANK_PAYER like '%{0}%'", SearchCondition.BANK_PAYER);
            var result = ccHelper.Search<V_E_CUST_BANKING>(sql);
            return result;
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return ccHelper.CheckSecurity(SECURE_ID, APP_USER);
        }

        public void MP_CUSTBANKSave(V_E_CUST_BANKING item, string AppUser)
        {
            string sql = string.Format(@"Update E_CUST_BANKING set 
                                            BANK_PAYER ='{0}',CREDIT_LIMIT='{1}',CREDIT_LIMIT_2='{2}',
                                            NOTE='{3}',BALANCE_ACTIVE_FLAG='{4}',RECORD_DATE=sysdate,RECORD_BY='{5}' 
                                            where CUSTOMER_NO='{6}' and BILL_TO_LOCATION='{7}' and CURRENCY_CODE='{8}'",
                                            item.BANK_PAYER, item.CREDIT_LIMIT, item.CREDIT_LIMIT_2,
                                            item.NOTE, item.BALANCE_ACTIVE_FLAG, AppUser,
                                            item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.CURRENCY_CODE);
            ccHelper.Update(sql);
        }

        public V_E_CUST_BANKING MP_CUSTBANK(V_E_CUST_BANKING item)
        {
            string sql = string.Format(@"select * from V_E_CUST_BANKING where CUSTOMER_NO='{0}' and BILL_TO_LOCATION='{1}' and CURRENCY_CODE='{2}'", item.CUSTOMER_NO, item.BILL_TO_LOCATION, item.CURRENCY_CODE);
            var result = ccHelper.Search<V_E_CUST_BANKING>(sql).First();
            return result;
        }

        public List<V_E_TRANS_TYPE_ASSIGN> GetManualADJTransType(string App_User, string TRANS_TYPE)
        {
            //string sql = string.Format(@"select * from V_E_TRANS_TYPE_ASSIGN where APP_USER='{0}' and ACTIVEIND='Y'", App_User);
            string sql = string.Format(@"select * from V_E_TRANS_TYPE_ASSIGN where 1=1");
            if (!string.IsNullOrEmpty(App_User))
                sql += string.Format(@" and APP_USER='{0}' and ACTIVEIND='Y'", App_User);
            if (!string.IsNullOrEmpty(TRANS_TYPE))
                sql += string.Format(@" and TRANS_TYPE='{0}'", TRANS_TYPE);
            var result = ccHelper.Search<V_E_TRANS_TYPE_ASSIGN>(sql);
            return result;
        }

        public string CheckOrderNo(string CUSTOMER_NO, string BILL_TO_LOCATION, string ORDER_NO)
        {
            string sql = string.Format(@"select F_EAPP_ORD_ADJ_PRECHECK('{0}','{1}','{2}') from dual", CUSTOMER_NO, BILL_TO_LOCATION, ORDER_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string GetAmtByOrderNo(string ORDER_NO)
        {
            string sql = string.Format(@"select F_GOMS_ORD_TOTAL_AMT('{0}') from dual", ORDER_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public bool ManualADJcheckDOC_NO(string DOC_NO)
        {
            string sql = string.Format(@"select * from E_BANKING_TRANS where TRANS_DOC_NO='{0}'", DOC_NO);
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void RunP_BANK_TRANS_NEW(E_BANKING_TRANS item)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":v_CUSTOMER_NO",OracleDbType.Varchar2),
                new OracleParameter(":V_BILL_TO",OracleDbType.Decimal),
                new OracleParameter(":V_CURRENCY_CODE",OracleDbType.Varchar2),
                new OracleParameter(":V_TRANS_TYPE",OracleDbType.Varchar2),
                new OracleParameter(":V_TRANS_AMT",OracleDbType.Decimal),
                new OracleParameter(":V_DOC_NO",OracleDbType.Varchar2),
                new OracleParameter(":V_USER",OracleDbType.Varchar2),
                new OracleParameter(":V_BRIEF",OracleDbType.Varchar2),
                new OracleParameter(":V_NOTE",OracleDbType.Varchar2),
                new OracleParameter(":V_bank_payer",OracleDbType.Varchar2),
                new OracleParameter(":v_Order_no",OracleDbType.Varchar2),
                new OracleParameter(":v_load_batch",null)
            };
            param[0].Value = item.CUSTOMER_NO;
            param[1].Value = Convert.ToDecimal(item.BILL_TO_LOCATION);
            param[2].Value = item.CURRENCY_CODE;
            param[3].Value = item.TRANS_TYPE;
            param[4].Value = Convert.ToDecimal(item.TRANS_AMT);
            param[5].Value = item.TRANS_DOC_NO;
            param[6].Value = item.APP_USER;
            param[7].Value = item.TRANS_BRIEF;
            param[8].Value = item.NOTE;
            param[9].Value = item.BANK_PAYER;
            param[10].Value = item.ORDER_NO;

            ccHelper.ExecProcedure("P_BANK_TRANS_NEW", param);
        }


        public List<E_TRANS_TYPE> GetTransType()
        {
            string sql = string.Format(@"select * from E_TRANS_TYPE");
            var result = ccHelper.Search<E_TRANS_TYPE>(sql);
            return result;
        }

        public string TransHistoryGetTrans_TYPE(string TRANS_TYPE)
        {
            string sql = string.Format(@"select * from E_TRANS_TYPE where TRANS_TYPE='{0}'", TRANS_TYPE);
            var result = ccHelper.Search<E_TRANS_TYPE>(sql).First().TRANS_TYPE_DESC;
            return result;
        }

        public List<V_E_BANKING_TRANS> TransHistoryQuery(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE, string TRANS_TYPE, string TRANS_DATE)
        {
            string sql = string.Format(@"select * from V_E_BANKING_TRANS where CUSTOMER_NO='{0}' and BILL_TO_LOCATION='{1}' and CURRENCY_CODE='{2}'", CUSTOMER_NO, BILL_TO_LOCATION, CURRENCY_CODE);
            if (!string.IsNullOrEmpty(TRANS_TYPE))
                sql += string.Format(@" and TRANS_TYPE='{0}'", TRANS_TYPE);
            if (!string.IsNullOrEmpty(TRANS_DATE))
                sql += string.Format(@" and TRANS_DATE>=sysdate-{0}", Convert.ToInt32(TRANS_DATE));
            var result = ccHelper.Search<V_E_BANKING_TRANS>(sql);
            return result;
        }

        public DataTable TransHistoryDownload(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE, string TRANS_TYPE, string TRANS_DATE)
        {
            string sql = string.Format(@"select * from V_E_BANKING_TRANS where CUSTOMER_NO='{0}' and BILL_TO_LOCATION='{1}' and CURRENCY_CODE='{2}'", CUSTOMER_NO, BILL_TO_LOCATION, CURRENCY_CODE);
            if (!string.IsNullOrEmpty(TRANS_TYPE))
                sql += string.Format(@" and TRANS_TYPE='{0}'", TRANS_TYPE);
            if (!string.IsNullOrEmpty(TRANS_DATE))
                sql += string.Format(@" and TRANS_DATE>=sysdate-{0}", Convert.ToInt32(TRANS_DATE));
            sql += string.Format(@" order by TRANS_ID");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

        public decimal GetLOAD_BATCH()
        {
            string sql = string.Format(@"select BANK_RCPT_LOAD.nextval from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return Convert.ToDecimal(result);
        }

        public void LoadData(E_BANKING_TRANS_LOAD item,string AppUser,decimal LOAD_BATCH,decimal LINE_NO)
        {
            string sql = string.Format(@"insert into E_BANKING_TRANS_LOAD(TRANS_TYPE,
                                            CURRENCY_CODE,
                                            BANK_PAYER,
                                            TRANS_AMT,
                                            TRANS_DOC_NO,
                                            TRANS_DATE,
                                            TRANS_BRIEF,
                                            NOTE,
                                            APP_USER,
                                            LOAD_BATCH,
                                            LINE_NO,
                                            LOAD_DATE)
                                            values('{0}','{1}','{2}',{3},'{4}',to_date('{5}','yyyy/mm/dd'),'{6}','{7}','{8}',{9},{10},sysdate)",
                                            item.TRANS_TYPE,
                                            item.CURRENCY_CODE,
                                            item.BANK_PAYER,
                                            item.TRANS_AMT,
                                            item.TRANS_DOC_NO,
                                            item.TRANS_DATE.ToString("yyyy/MM/dd"),
                                            item.TRANS_BRIEF,
                                            item.NOTE,
                                            AppUser,
                                            LOAD_BATCH,
                                            LINE_NO
                                            );
            ccHelper.Update(sql);
        }

        public void RunP_BANK_TRANS_LOAD_POST(decimal LOAD_BATCH)
        {
            OracleParameter[] param = new OracleParameter[] {
               new OracleParameter(":v_load_batch",OracleDbType.Decimal)
            };
            param[0].Value = LOAD_BATCH;
            ccHelper.ExecProcedure("EAPP.P_BANK_TRANS_LOAD_POST", param);
        }

        public DataTable BankReceiptDownload(decimal LOAD_BATCH)
        {
            string sql = string.Format(@"select * from V_E_BANKING_TRANS_LOAD where LOAD_BATCH={0}", LOAD_BATCH);
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

        public string CallF_GOMS_ORD_BRIEF(string ORDER_NO)
        {
            var sql = string.Format(@"select F_GOMS_ORD_BRIEF('{0}') from dual", ORDER_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

    }
}

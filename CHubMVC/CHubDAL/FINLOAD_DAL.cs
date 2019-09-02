using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class FINLOAD_DAL
    {
        private CHubCommonHelper ccHelper;
        public FINLOAD_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public string GetExpVatLoad_Batch()
        {
            string sql = "select EXP_VAT_TOKEN.NEXTVAL from dual";
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public void InsertIntoEXP_VAT_LOAD(EXP_VAT_LOAD evl, decimal LOAD_BATCH, string appUser)
        {
            string sql = string.Format(@"INSERT INTO EXP_VAT_LOAD(
                                    LOAD_BATCH,
                                    UBIVNO,
                                    UACUNO,
                                    MMITDS,
                                    UBIVQS,
                                    TAX_REFUND_RATE,
                                    AMT_USD,
                                    UBSPUN,
                                    UBLNAM,
                                    NOTE,
                                    LOADED_BY,
                                    LOAD_DATE) 
                            VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}','{10}', sysdate)",
                            LOAD_BATCH, evl.UBIVNO, evl.UACUNO, evl.MMITDS, evl.UBIVQS, evl.TAX_REFUND_RATE, evl.AMT_USD, evl.UBSPUN, evl.UBLNAM, evl.NOTE, appUser);
            ccHelper.ExecuteNonQuery(sql);
        }

        public void ExecP_EXP_VAT_LOAD_POST(string LOAD_BATCH)
        {
            ccHelper.ExecP_EXP_VAT_LOAD_POST(LOAD_BATCH);
        }

        public string GetNumOfEXP_VAT_D(string LOAD_BATCH)
        {
            string sql = string.Format(@"SELECT COUNT(*) NUM FROM　EXP_VAT_D　WHERE LOAD_BATCH ={0}", Convert.ToDecimal(LOAD_BATCH));
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string GetExpXrefLoad_Batch()
        {
            string sql = "select EXP_XREF_TOKEN.NEXTVAL from dual";
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public void InsertIntoEXP_VAT_XREF_LOAD(EXP_VAT_XREF_LOAD item, decimal LOAD_BATCH, string appUser)
        {
            string sql = string.Format(@"INSERT INTO EXP_VAT_XREF_LOAD(
                                LOAD_BATCH,
                                UBIVNO,
                                VAT_INVOICE_NO,
                                VAT_AMT,
                                VAT_DATE,
                                NOTE,
                                LOAD_DATE,
                                LOADED_BY)
                                VALUES('{0}','{1}','{2}','{3}',to_date('{4}','yyyy/mm/dd hh24:mi:ss'),'{5}',sysdate,'{6}')",
                                LOAD_BATCH, item.UBIVNO, item.VAT_INVOICE_NO, item.VAT_AMT, item.VAT_DATE, item.NOTE, appUser);
            ccHelper.ExecuteNonQuery(sql);
        }

        public string GetNumOfEXP_VAT_XREF_LOAD(string LOAD_BATCH)
        {
            string sql = string.Format(@"SELECT COUNT(*) NUM FROM EXP_VAT_XREF_LOAD WHERE LOAD_BATCH={0}", Convert.ToDecimal(LOAD_BATCH));
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string GetExpCollectionLoad_Batch()
        {
            string sql = "select EXP_COLLECTION_TOKEN.NEXTVAL from dual";
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public void InsertIntoEXP_COLLECTION_LOAD(EXP_COLLECTION_LOAD item, decimal LOAD_BATCH, string appUser)
        {
            string sql = string.Format(@"INSERT INTO EXP_COLLECTION_LOAD(
                                        LOAD_BATCH,
                                        INVOICE_ID,
                                        UBIVNO,
                                        RECEIVED_AMT_USD,
                                        RECEIVED_RATE,
                                        NOTE,
                                        LOAD_DATE,
                                        LOADED_BY)
                                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}',sysdate,'{6}')",
                                        LOAD_BATCH, item.INVOICE_ID, item.UBIVNO, item.RECEIVED_AMT_USD, item.RECEIVED_RATE, item.NOTE, appUser);
            ccHelper.ExecuteNonQuery(sql);
        }

        public string GetNumOfEXP_COLLECTION_LOAD(string LOAD_BATCH)
        {
            string sql = string.Format(@"SELECT COUNT(*) NUM FROM EXP_COLLECTION_LOAD WHERE LOAD_BATCH={0}", Convert.ToDecimal(LOAD_BATCH));
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
    }
}

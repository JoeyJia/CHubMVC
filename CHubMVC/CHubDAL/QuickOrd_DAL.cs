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
    public class QuickOrd_DAL
    {
        private CHubCommonHelper ccHelper;
        public QuickOrd_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<G_ADDR_DFLT> GetG_ADDR_DFLT(string SYSID, string ABBREVIATION)
        {
            string sql = string.Format(@"select * from G_ADDR_DFLT where SYSID='{0}' and ABBREVIATION='{1}'", SYSID, ABBREVIATION);
            var result = ccHelper.ExecuteSqlToList<G_ADDR_DFLT>(sql);
            return result;
        }

        public List<G_ADDR_SPL> GetG_ADDR_SPL(string SYSID, string ABBREVIATION, string KeyWord, int PageStart, int PageEnd)
        {
            string sql = string.Format(@"select a.* from (select rownum line, g.* from G_ADDR_SPL g where SYSID='{0}' and ABBREVIATION='{1}'", SYSID, ABBREVIATION);
            if (!string.IsNullOrEmpty(KeyWord))
                sql += string.Format(@" and (LOCAL_DEST_NAME like '%{0}%' or LOCAL_DEST_ADDR_1 like '%{0}%' or LOCAL_DEST_ADDR_2 like '%{0}%' or LOCAL_DEST_ADDR_3 like '%{0}%')", KeyWord);
            sql += string.Format(@" order by DEST_LOCATION) a");
            sql += string.Format(@" where a.line>={0} and a.line<={1}", PageStart, PageEnd);
            var result = ccHelper.ExecuteSqlToList<G_ADDR_SPL>(sql);
            return result;
        }

        public G_ADDR_SPL GetG_ADDR_SPLDetail(string SYSID, string ABBREVIATION, string DEST_LOCATION)
        {
            string sql = string.Format(@"select * from G_ADDR_SPL where SYSID='{0}' and ABBREVIATION='{1}' and DEST_LOCATION='{2}'", SYSID, ABBREVIATION, DEST_LOCATION);
            var result = ccHelper.ExecuteSqlToList<G_ADDR_SPL>(sql).First();
            return result;
        }

        public List<G_ORDER_TYPE> GetG_ORDER_TYPE(string SYSID, string WAREHOUSE, string DUE_DATE_CODE)
        {
            string sql = string.Format(@"select * from G_ORDER_TYPE where SYSID='{0}' and WAREHOUSE='{1}'", SYSID, WAREHOUSE);
            if (!string.IsNullOrEmpty(DUE_DATE_CODE))
                sql += string.Format(@" and DUE_DATE_CODE='{0}'", DUE_DATE_CODE);
            var result = ccHelper.ExecuteSqlToList<G_ORDER_TYPE>(sql);
            return result;
        }

        public string CallF_QUICK_PART(string GOMS, string CUSTOMER_NO, string CUSTOMER_PARTNO)
        {
            string sql = string.Format(@"select F_QUICK_PART_V2('{0}','{1}','{2}') from dual", GOMS, CUSTOMER_NO, CUSTOMER_PARTNO);
            //string sql = string.Format(@"select F_QUICK_PART('{0}','{1}') from dual", CUSTOMER_NO, CUSTOMER_PARTNO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string CallF_QUICK_QTY(string PART_NO, string BUY_QTY)
        {
            string sql = string.Format(@"select F_QUICK_QTY('{0}','{1}') from dual", PART_NO, BUY_QTY);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string CallF_QUICK_DESC(string PART_NO)
        {
            string sql = string.Format(@"select F_QUICK_DESC('{0}') from dual", PART_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string CallF_QUICK_MSG(string PART_NO)
        {
            string sql = string.Format(@"select F_QUICK_MSG('{0}') from dual", PART_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string CallF_QUICK_INV(string WAREHOUSE, string PART_NO)
        {
            string sql = string.Format(@"select F_QUICK_INV('{0}','{1}') from dual", WAREHOUSE, PART_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string GetQUICK_ORDER_NO()
        {
            string sql = string.Format(@"select SEQ_QUICK_ORDER.nextval from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public void SaveQUICK_OEORDER_HEADER(QUICK_OEORDER_HEADER header)
        {
            string sql = string.Format(@"insert into QUICK_OEORDER_HEADER (
                                            GOMS,QUICK_ORDER_NO,ABBREVIATION,CUSTOMER_NO,BILL_TO_LOCATION,
                                            SHIP_TO_LOCATION,DEST_LOCATION,SPL_IND,CUSTOMER_PO_NO,DEALER_PO_NO,
                                            ORDER_TYPE,PRIORITY_CODE,DUE_DATE,SHIP_COMPLETE_FLAG,WAREHOUSE,
                                            ORDER_NOTES,CREATED_BY,CREATE_DATE)
                                            values('{0}',{1},'{2}','{3}',{4},
                                            {5},{6},'{7}','{8}','{9}',
                                            '{10}','{11}',to_date('{12}','yyyy/mm/dd'),{13},'{14}',
                                            '{15}','{16}',sysdate)", header.GOMS, header.QUICK_ORDER_NO, header.ABBREVIATION, header.CUSTOMER_NO, header.BILL_TO_LOCATION,
                                            header.SHIP_TO_LOCATION, header.DEST_LOCATION, header.SPL_IND, header.CUSTOMER_PO_NO, header.DEALER_PO_NO,
                                            header.ORDER_TYPE, header.PRIORITY_CODE, !string.IsNullOrEmpty(header.DUE_DATE.ToString()) ? header.DUE_DATE.ToString("yyyy-MM-dd") : "", header.SHIP_COMPLETE_FLAG, header.WAREHOUSE,
                                            header.ORDER_NOTES, header.CREATED_BY, "sysdate");
            ccHelper.ExecuteNonQuery(sql);
        }

        public void UpdateQUICK_OEORDER_HEADER(QUICK_OEORDER_HEADER header)
        {
            string sql = string.Format(@"update QUICK_OEORDER_HEADER set 
                                                    GOMS='{0}',ABBREVIATION='{1}',CUSTOMER_NO='{2}',BILL_TO_LOCATION='{3}',SHIP_TO_LOCATION='{4}',
                                                    DEST_LOCATION='{5}',SPL_IND='{6}',CUSTOMER_PO_NO='{7}',DEALER_PO_NO='{8}',ORDER_TYPE='{9}',
                                                    PRIORITY_CODE='{10}',DUE_DATE=to_date('{11}','yyyy/mm/dd'),SHIP_COMPLETE_FLAG='{12}',WAREHOUSE='{13}',ORDER_NOTES='{14}' 
                                                    where QUICK_ORDER_NO='{15}'",
                                                    header.GOMS, header.ABBREVIATION, header.CUSTOMER_NO, header.BILL_TO_LOCATION, header.SHIP_TO_LOCATION,
                                                    header.DEST_LOCATION, header.SPL_IND, header.CUSTOMER_PO_NO, header.DEALER_PO_NO, header.ORDER_TYPE,
                                                    header.PRIORITY_CODE, !string.IsNullOrEmpty(header.DUE_DATE.ToString()) ? header.DUE_DATE.ToString("yyyy-MM-dd") : "", header.SHIP_COMPLETE_FLAG, header.WAREHOUSE, header.ORDER_NOTES,
                                                    header.QUICK_ORDER_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public void SaveQUICK_OEORDER_DETAIL(QUICK_OEORDER_DETAIL detail)
        {
            string sql = string.Format(@"insert into QUICK_OEORDER_DETAIL (
                                            QUICK_ORDER_NO,LINE_NO,CUSTOMER_PARTNO,PART_NO,BUY_QTY,
                                            DESCRIPTION,DUE_DATE,CREATE_DATE)
                                            values({0},{1},'{2}','{3}',{4},
                                            '{5}',to_date('{6}','yyyy/mm/dd'),sysdate)", detail.QUICK_ORDER_NO, detail.LINE_NO, detail.CUSTOMER_PARTNO, detail.PART_NO, detail.BUY_QTY,
                                            detail.DESCRIPTION, !string.IsNullOrEmpty(detail.DUE_DATE.ToString()) ? detail.DUE_DATE.ToString("yyyy-MM-dd") : "", "sysdate");
            ccHelper.ExecuteNonQuery(sql);
        }

        public void GetQUICK_OEORDER_DETAILByQUICK_ORDER_NO(string QUICK_ORDER_NO)
        {
            string sql = string.Format(@"select * from QUICK_OEORDER_DETAIL where QUICK_ORDER_NO='{0}'", QUICK_ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<QUICK_OEORDER_DETAIL>(sql);
            if (result != null && result.Count() > 0)
            {
                string dSql = string.Format(@"delete QUICK_OEORDER_DETAIL where QUICK_ORDER_NO='{0}'", QUICK_ORDER_NO);
                ccHelper.ExecuteNonQuery(dSql);
            }
        }

        public List<V_QUICK_EXPORT_WEBPART_HDR> GetV_QUICK_EXPORT_WEBPART_HDR(decimal QUICK_ORDER_NO)
        {
            string sql = string.Format(@"select * from V_QUICK_EXPORT_WEBPART_HDR where QUICK_ORDER_NO={0}", QUICK_ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_QUICK_EXPORT_WEBPART_HDR>(sql);
            return result;
        }
        public List<V_QUICK_EXPORT_WEBPART_DTL> GetV_QUICK_EXPORT_WEBPART_DTL(decimal QUICK_ORDER_NO)
        {
            string sql = string.Format(@"select * from V_QUICK_EXPORT_WEBPART_DTL where QUICK_ORDER_NO={0}", QUICK_ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_QUICK_EXPORT_WEBPART_DTL>(sql);
            return result;
        }

        public string RunFunc(string QUICK_ORDER_NO, string Identifier)
        {
            string sql = string.Format(@"select F_ORDER_FILE_QORD('{0}','{1}') from dual", QUICK_ORDER_NO, Identifier);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public DataTable GetQORDLine(string QUICK_ORDER_NO)
        {
            string sql = string.Format(@"select order_file from V_ORDER_FILE_LINE_QORD where Quick_Order_no ='{0}'", QUICK_ORDER_NO);
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public void UpdateState(string QUICK_ORDER_NO, string PROCESS_STATUS, string PROCESS_ERROR)
        {
            string sql = string.Format(@"update QUICK_OEORDER_HEADER set PROCESS_STATUS='{0}',PROCESS_DATE=sysdate,PROCESS_ERROR='{1}' where QUICK_ORDER_NO='{2}'", PROCESS_STATUS, PROCESS_ERROR.Replace("'", "''"), QUICK_ORDER_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public void ExecP_CRT_ORDER_FILE_QORD(string QUICK_ORDER_NO)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":V_order_no",OracleDbType.Decimal)
            };
            param[0].Value = Convert.ToDecimal(QUICK_ORDER_NO);
            param[0].Direction = ParameterDirection.Input;
            ccHelper.ExecProcedureWithParams("P_CRT_ORDER_FILE_QORD", param);
        }
        public void UpdateQORDStatus(string QUICK_ORDER_NO)
        {
            string sql = string.Format(@"update QUICK_OEORDER_HEADER set PROCESS_STATUS='C',PROCESS_DATE=sysdate where QUICK_ORDER_NO='{0}'", QUICK_ORDER_NO);
            ccHelper.ExecuteNonQuery(sql);
        }
    }
}

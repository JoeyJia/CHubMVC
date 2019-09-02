using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;

namespace CHubDAL
{
    public class V_XCEC_ORDER_HDR_BASE_DAL : BaseDAL
    {
        private CHubCommonHelper cchelper;
        public V_XCEC_ORDER_HDR_BASE_DAL() : base()
        {
            cchelper = new CHubCommonHelper();
        }

        public V_XCEC_ORDER_HDR_BASE_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_XCEC_ORDER_HDR_BASE> SearchXcecWB(string CUST_ORDER_NO, string CUST_NAME, string CREATE_DATE,string PROCESS_STATUS)
        {
            string sql = string.Format(@"select * from V_XCEC_ORDER_HDR_BASE where 1=1");
            if (!string.IsNullOrEmpty(CUST_ORDER_NO))
                sql += string.Format(@" and CUST_ORDER_NO='{0}'", CUST_ORDER_NO);
            if (!string.IsNullOrEmpty(CUST_NAME))
                sql += string.Format(@" and CUST_NAME like '%{0}%'", CUST_NAME);
            if (!string.IsNullOrEmpty(CREATE_DATE))
                sql += string.Format(@" and CREATE_DATE >= sysdate-{0}", Convert.ToInt32(CREATE_DATE));
            if (!string.IsNullOrEmpty(PROCESS_STATUS))
                sql += string.Format(@" and PROCESS_STATUS='{0}'", PROCESS_STATUS);
            var result = cchelper.ExecuteSqlToList<V_XCEC_ORDER_HDR_BASE>(sql);
            return result;
        }

        public List<V_XCEC_ORDER_HDR_BASE> SearchXcecWBDetail(string WAREHOUSE, string IHUB_ORDER_NO)
        {
            string sql = string.Format(@"select * from V_XCEC_ORDER_HDR_BASE where WAREHOUSE='{0}' and IHUB_ORDER_NO='{1}'", WAREHOUSE, IHUB_ORDER_NO);
            var result = cchelper.ExecuteSqlToList<V_XCEC_ORDER_HDR_BASE>(sql);
            return result;
        }

        public List<V_XCEC_ORDER_LN_BASE> GetLinesDetail(string CUST_ORDER_NO)
        {
            //this.CheckCultureInfoForDate();
            //string sql = string.Format(@"select * from xcec_int.xcec_order_base");
            string sql = string.Format(@"select * from V_XCEC_ORDER_LN_BASE where CUST_ORDER_NO='{0}' order by ORDER_LINE_NO", CUST_ORDER_NO);
            var result = cchelper.ExecuteSqlToList<V_XCEC_ORDER_LN_BASE>(sql);
            return result;
        }

        public void UpdateProcessStatus(V_XCEC_ORDER_HDR_BASE result)
        {
            string sql = string.Format(@"UPDATE xcec_int.XCEC_ORDER_LOG SET PROCESS_STATUS = '{0}' where IHUB_ORDER_NO = '{1}'", "Q", result.IHUB_ORDER_NO);
            cchelper.ExecuteNonQuery(sql);
        }

        public void ExecProc()
        {
            cchelper.ExecProcedureWithoutParams("P_CRT_XCEC_ORDER");
        }


    }
}

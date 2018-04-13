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
        public V_XCEC_ORDER_HDR_BASE_DAL() : base()
        {

        }

        public V_XCEC_ORDER_HDR_BASE_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_XCEC_ORDER_HDR_BASE> SearchXcecWB(string CUST_ORDER_NO, string CUST_NAME, string CREATE_DATE)
        {
            string sql = string.Format(@"select * from V_XCEC_ORDER_HDR_BASE where 1=1");
            if (!string.IsNullOrEmpty(CUST_ORDER_NO))
                sql += string.Format(@" and CUST_ORDER_NO='{0}'", CUST_ORDER_NO);
            if (!string.IsNullOrEmpty(CUST_NAME))
                sql += string.Format(@" and CUST_NAME like '%{0}%'", CUST_NAME);
            if (!string.IsNullOrEmpty(CREATE_DATE))
                sql += string.Format(@" and CREATE_DATE between to_date('{0}','yyyy-mm-dd hh24:mi:ss') and to_date('{1}','yyyy-mm-dd hh24:mi:ss')",
                    DateTime.Now.AddDays(-Convert.ToInt32(CREATE_DATE)).ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            var result = CHubCommonHelper.Search<V_XCEC_ORDER_HDR_BASE>(sql);
            return result;
        }

        public List<V_XCEC_ORDER_HDR_BASE> SearchXcecWBDetail(string WAREHOUSE, string IHUB_ORDER_NO)
        {
            this.CheckCultureInfoForDate();
            string sql = string.Format(@"select * from V_XCEC_ORDER_HDR_BASE where WAREHOUSE='{0}' and IHUB_ORDER_NO='{1}'", WAREHOUSE, IHUB_ORDER_NO);
            var result = CHubCommonHelper.Search<V_XCEC_ORDER_HDR_BASE>(sql);
            return result;
        }

        public List<V_XCEC_ORDER_LN_BASE> GetLinesDetail(string CUST_ORDER_NO)
        {
            this.CheckCultureInfoForDate();
            //string sql = string.Format(@"select * from xcec_int.xcec_order_base");
            string sql = string.Format(@"select * from V_XCEC_ORDER_LN_BASE where CUST_ORDER_NO='{0}'", CUST_ORDER_NO);
            var result = CHubCommonHelper.Search<V_XCEC_ORDER_LN_BASE>(sql);
            return result;
        }


    }
}

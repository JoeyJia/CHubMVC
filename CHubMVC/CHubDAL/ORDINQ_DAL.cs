using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;

namespace CHubDAL
{
    public class ORDINQ_DAL
    {
        private CHubCommonHelper ccHelper;
        public ORDINQ_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_GOMS_ORDER_H> GetORDER_HList(string CUSTOMER_PO_NO,string ORDER_NO)
        {
            string sql = string.Format(@"select * from V_GOMS_ORDER_H where 1=1");
            if (!string.IsNullOrEmpty(CUSTOMER_PO_NO))
                sql += string.Format(@" and CUSTOMER_PO_NO='{0}'", CUSTOMER_PO_NO);
            if (!string.IsNullOrEmpty(ORDER_NO))
                sql += string.Format(@" and ORDER_NO='{0}'", ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_GOMS_ORDER_H>(sql);
            return result;
        }

        public List<G_OECUST_SHIPPING_LOCAL> GetSHIPPING_LOCAL(string LOAD_FROM, string CUSTOMER_NO, decimal BILL_TO_LOCATION, decimal SHIP_TO_LOCATION, decimal DEST_LOCATION)
        {
            string sql = string.Format(@"select * from G_OECUST_SHIPPING_LOCAL where LOAD_FROM='{0}' and CUSTOMER_NO='{1}' and BILL_TO_LOCATION={2} and SHIP_TO_LOCATION={3} and DEST_LOCATION={4}",
                                            LOAD_FROM, CUSTOMER_NO, BILL_TO_LOCATION, SHIP_TO_LOCATION, DEST_LOCATION);
            var result = ccHelper.ExecuteSqlToList<G_OECUST_SHIPPING_LOCAL>(sql);
            return result;
        }

        public List<V_GOMS_ORDER_D> GetORDER_DList(string LOAD_FROM, string ORDER_NO)
        {
            string sql = string.Format(@"select * from V_GOMS_ORDER_D where LOAD_FROM='{0}' and ORDER_NO='{1}' order by LINE_NO", LOAD_FROM, ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_GOMS_ORDER_D>(sql);
            return result;
        }

        public List<V_GOMS_ORDER_R> GetORDER_RList(string LOAD_FROM, string ORDER_NO, string LINE_NO)
        {
            string sql = string.Format(@"select * from V_GOMS_ORDER_R where LOAD_FROM='{0}' and ORDER_NO='{1}' and LINE_NO='{2}'", LOAD_FROM, ORDER_NO, LINE_NO);
            var result = ccHelper.ExecuteSqlToList<V_GOMS_ORDER_R>(sql);
            return result;
        }

        public List<V_SHIP_TRACK_PRO> GetTrackList(string LOAD_FROM, string ORDER_NO)
        {
            string sql = string.Format(@"select * from V_SHIP_TRACK_PRO where LOAD_FROM='{0}' and ORDER_NO='{1}' order by SHIP_DATE desc", LOAD_FROM, ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_SHIP_TRACK_PRO>(sql);
            return result;
        }

        public bool CheckPrintSecurity(string SECURE_ID, string APP_USER)
        {
            var check = ccHelper.CheckSecurity(SECURE_ID, APP_USER);
            return check;
        }

        public V_OA_H_PRINT SearchV_OA_H_PRINT(string LOAD_FROM, string ORDER_NO)
        {
            string sql = string.Format(@"select * from V_OA_H_PRINT where LOAD_FROM='{0}' and ORDER_NO='{1}'", LOAD_FROM, ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_OA_H_PRINT>(sql).FirstOrDefault();
            return result;
        }
        public OA_TYPE_MST SearchOA_TYPE_MST(string OA_TYPE)
        {
            string sql = string.Format(@"select * from OA_TYPE_MST where OA_TYPE='{0}'", OA_TYPE);
            var result = ccHelper.ExecuteSqlToList<OA_TYPE_MST>(sql).FirstOrDefault();
            return result;
        }
        public List<V_OA_D_PRINT> SearchV_OA_D_PRINT(string LOAD_FROM, string ORDER_NO)
        {
            string sql = string.Format(@"select * from V_OA_D_PRINT where LOAD_FROM='{0}' and ORDER_NO='{1}' order by LINE_NO", LOAD_FROM, ORDER_NO);
            var result = ccHelper.ExecuteSqlToList<V_OA_D_PRINT>(sql);
            return result;
        }


    }
}

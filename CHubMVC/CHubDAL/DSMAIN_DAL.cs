using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class DSMAIN_DAL
    {
        private CHubCommonHelper ccHelper;
        public DSMAIN_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_DS_ORDER_BASE> DSMAINSearch(DSMAINArg arg)
        {
            string sql = string.Format(@"select * from V_DS_ORDER_BASE where 1=1");
            if (!string.IsNullOrEmpty(arg.CUSTOMER_PO_NO))
                sql += string.Format(@" and CUSTOMER_PO_NO='{0}'", arg.CUSTOMER_PO_NO);
            if (!string.IsNullOrEmpty(arg.ORDER_NO))
                sql += string.Format(@" and ORDER_NO='{0}'", arg.ORDER_NO);
            if (!string.IsNullOrEmpty(arg.CUSTOMER_NO))
                sql += string.Format(@" and CUSTOMER_NO='{0}'", arg.CUSTOMER_NO);
            if (!string.IsNullOrEmpty(arg.SHIP_TO_NAME))
                sql += string.Format(@" and SHIP_TO_NAME like '%{0}%'", arg.SHIP_TO_NAME);
            if (!string.IsNullOrEmpty(arg.PART_NO))
                sql += string.Format(@" and PART_NO='{0}'", arg.PART_NO);
            if (!string.IsNullOrEmpty(arg.PO_NO))
                sql += string.Format(@" and PO_NO='{0}'", arg.PO_NO);
            if (!string.IsNullOrEmpty(arg.STATUS_CODE))
                sql += string.Format(@" and STATUS_CODE='{0}'", arg.STATUS_CODE);
            if (!string.IsNullOrEmpty(arg.ORDER_DATE))
                sql += string.Format(@" and ORDER_DATE>=sysdate-{0}", Convert.ToDecimal(arg.ORDER_DATE));
            var result = ccHelper.Search<V_DS_ORDER_BASE>(sql);
            return result;
        }

        public void RunP_DS_STatus_REFRESH()
        {
            ccHelper.ExecProcedureWithoutParams("P_DS_STATUS_REFRESH");
        }

        public List<V_IHUB_OA_BASE> DSMAINMore(string PO_NO, string PART_NO)
        {
            string sql = string.Format(@"select * from V_IHUB_OA_BASE where PO_NO='{0}' and PART_NO='{1}'", PO_NO, PART_NO);
            var result = ccHelper.Search<V_IHUB_OA_BASE>(sql);
            return result;
        }

    }
}

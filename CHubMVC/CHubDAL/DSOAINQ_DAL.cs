using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class DSOAINQ_DAL
    {
        private CHubCommonHelper ccHelper;
        public DSOAINQ_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_IHUB_OA_BASE> DSOAINQSearch(string PART_NO, string COMPANY_CODE, string PO_NO, string OA_STATUS, string ORDER_DATE)
        {
            string sql = string.Format(@"select * from V_IHUB_OA_BASE where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO like '%{0}%'", PART_NO);
            if (!string.IsNullOrEmpty(COMPANY_CODE))
                sql += string.Format(@" and COMPANY_CODE='{0}'", COMPANY_CODE);
            if (!string.IsNullOrEmpty(PO_NO))
                sql += string.Format(@" and PO_NO='{0}'", PO_NO);
            if (!string.IsNullOrEmpty(OA_STATUS))
                sql += string.Format(@" and OA_STATUS='{0}'", OA_STATUS);
            if (!string.IsNullOrEmpty(ORDER_DATE))
                sql += string.Format(@" and ORDER_DATE>=sysdate-{0}", Convert.ToDecimal(ORDER_DATE));
            var result = ccHelper.Search<V_IHUB_OA_BASE>(sql);
            return result;
        }


    }
}

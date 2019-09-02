using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;

namespace CHubDAL
{
    public class LBTraceScan_DAL
    {
        public CHubCommonHelper ccHelper;
        public LBTraceScan_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public string LBTraceScanGetCUST(string DOC_NO)
        {
            string sql = string.Format(@"select F_TRC_GET_CUST('{0}') from dual", DOC_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string LBTraceScanGetCount(string DOC_NO)
        {
            string sql = string.Format(@"select F_TRC_GET_COUNT('{0}') from dual", DOC_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string LBTraceScanGetTotal(string DOC_NO)
        {
            string sql = string.Format(@"select F_TRC_GET_TOTAL('{0}') from dual", DOC_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string GetSeq()
        {
            string sql = string.Format(@"select TRC_SCAN_SEQ.nextval from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public void LBTraceScanComplete(string seq, string doc_no, string barcode, string app_user)
        {
            string sql = string.Format(@"insert into TRC_SCAN_HISTORY(SCAN_SEQ,DOC_NO,BARCODE,APP_USER,SCAN_DATE,CREATE_DATE,NOTE)
                                        values('{0}','{1}','{2}','{3}',sysdate,sysdate,'NOTE')", seq, doc_no, barcode, app_user);
            ccHelper.ExecuteNonQuery(sql);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;

namespace CHubBLL
{
    public class LBTraceScan_BLL
    {
        private LBTraceScan_DAL dal;
        public LBTraceScan_BLL()
        {
            dal = new LBTraceScan_DAL();
        }
        public string LBTraceScanGetCUST(string DOC_NO)
        {
            return dal.LBTraceScanGetCUST(DOC_NO);
        }
        public string LBTraceScanGetCount(string DOC_NO)
        {
            return dal.LBTraceScanGetCount(DOC_NO);
        }
        public string LBTraceScanGetTotal(string DOC_NO)
        {
            return dal.LBTraceScanGetTotal(DOC_NO);
        }
        public string GetSeq()
        {
            return dal.GetSeq();
        }
        public void LBTraceScanComplete(string seq, string doc_no, string barcode, string app_user)
        {
            dal.LBTraceScanComplete(seq, doc_no, barcode, app_user);
        }
    }
}

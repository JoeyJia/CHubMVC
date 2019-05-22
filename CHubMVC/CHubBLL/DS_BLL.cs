using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class DS_BLL
    {
        private DS_DAL dal;
        public DS_BLL()
        {
            dal = new DS_DAL();
        }
        public string GetLOAD_BATCH()
        {
            return dal.GetLOAD_BATCH();
        }
        public void IhubASNUpload(IHUB_ASN_STG item)
        {
            dal.IhubASNUpload(item);
        }
        public string RunF_IHUB_ASN_LOAD_CHK(decimal LOAD_BATCH)
        {
            return dal.RunF_IHUB_ASN_LOAD_CHK(LOAD_BATCH);
        }
        public void ExecP_IHUB_ASN_LOAD_POST(decimal LOAD_BATCH)
        {
            dal.ExecP_IHUB_ASN_LOAD_POST(LOAD_BATCH);
        }
        public List<V_IHUB_ASN> IhubASNSearch(string COMPANY_CODE, string ASN_NO, int LOAD_DAY)
        {
            return dal.IhubASNSearch(COMPANY_CODE, ASN_NO, LOAD_DAY);
        }
        public string GetCOMPANY_NAME(string COMPANY_CODE)
        {
            return dal.GetCOMPANY_NAME(COMPANY_CODE);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class MDSUPP_BLL
    {
        private MDSUPP_DAL dal;
        public MDSUPP_BLL()
        {
            dal = new MDSUPP_DAL();
        }
        public List<MD_COMPANY_SNAP> MDSuppSearch(string COMPANY_CODE)
        {
            return dal.MDSuppSearch(COMPANY_CODE);
        }
        public void MDSuppSave(MD_COMPANY_SNAP item)
        {
            dal.MDSuppSave(item);
        }
        public List<MD_INSURANCE_CODES> GetINSURANCE_CODE()
        {
            return dal.GetINSURANCE_CODE();
        }
    }
}

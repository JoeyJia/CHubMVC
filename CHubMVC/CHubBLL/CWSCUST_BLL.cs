using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;
using System.Data;

namespace CHubBLL
{
    public class CWSCUST_BLL
    {
        private CWSCUST_DAL dal;
        public CWSCUST_BLL()
        {
            dal = new CWSCUST_DAL();
        }
        public List<APP_OECUSTOMER_MST> CwsCustSearch(string CUSTOMER_NO)
        {
            return dal.CwsCustSearch(CUSTOMER_NO);
        }
        public List<OA_TYPE_MST> GetOA_TYPE()
        {
            return dal.GetOA_TYPE();
        }
        public void CwsCustSave(APP_OECUSTOMER_MST arg)
        {
            dal.CwsCustSave(arg);
        }
        public APP_OECUSTOMER_MST CwsCustEdit(string CUSTOMER_NO)
        {
            return dal.CwsCustEdit(CUSTOMER_NO);
        }
        public DataTable CheckAPP_USER(string APP_USER)
        {
            return dal.CheckAPP_USER(APP_USER);
        }
    }
}

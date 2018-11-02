using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;

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
        public void CwsCustSave(APP_OECUSTOMER_MST arg)
        {
            dal.CwsCustSave(arg);
        }
    }
}

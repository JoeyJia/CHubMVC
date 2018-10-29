using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;
using CHubModel.WebArg;

namespace CHubBLL
{
    public class RETINV_BLL
    {
        private RETINV_DAL dal;
        public RETINV_BLL()
        {
            dal = new RETINV_DAL();
        }
        public List<V_RET_USER_CUST_LINK> GetCustomer_No(string AppUser)
        {
            return dal.GetCustomer_No(AppUser);
        }
        public List<V_RET_INV_RETURN_H> RetInvSearch(RetInvArg arg)
        {
            return dal.RetInvSearch(arg);
        }
        public List<V_RET_INV_RETURN_D> GetRetInvDetailModal(string INVOICE_ID)
        {
            return dal.GetRetInvDetailModal(INVOICE_ID);
        }
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckSecurity(SECURE_ID, APP_USER);
        }
        public void RunProc_P_RET_Match(string INVOICE_ID)
        {
            dal.RunProc_P_RET_Match(INVOICE_ID);
        }
    }
}

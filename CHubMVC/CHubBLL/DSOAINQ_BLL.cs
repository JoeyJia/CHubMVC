using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class DSOAINQ_BLL
    {
        private DSOAINQ_DAL dal;
        public DSOAINQ_BLL()
        {
            dal = new DSOAINQ_DAL();
        }
        public List<V_IHUB_OA_BASE> DSOAINQSearch(string PART_NO, string COMPANY_CODE, string PO_NO, string OA_STATUS, string ORDER_DATE)
        {
            return dal.DSOAINQSearch(PART_NO, COMPANY_CODE, PO_NO, OA_STATUS, ORDER_DATE);
        }
    }
}

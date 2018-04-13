using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_XCEC_ORDER_HDR_BASE_BLL
    {
        private V_XCEC_ORDER_HDR_BASE_DAL dal;
        public V_XCEC_ORDER_HDR_BASE_BLL()
        {
            dal = new V_XCEC_ORDER_HDR_BASE_DAL();
        }

        public V_XCEC_ORDER_HDR_BASE_BLL(CHubEntities db)
        {
            dal = new V_XCEC_ORDER_HDR_BASE_DAL(db);
        }

        public List<V_XCEC_ORDER_HDR_BASE> SearchXcecWB(string CUST_ORDER_NO, string CUST_NAME, string CREATE_DATE)
        {
            return dal.SearchXcecWB(CUST_ORDER_NO, CUST_NAME, CREATE_DATE);
        }

        public List<V_XCEC_ORDER_HDR_BASE> SearchXcecWBDetail(string WAREHOUSE, string IHUB_ORDER_NO)
        {
            return dal.SearchXcecWBDetail(WAREHOUSE, IHUB_ORDER_NO);
        }

        public List<V_XCEC_ORDER_LN_BASE> GetLinesDetail(string CUST_ORDER_NO)
        {
            return dal.GetLinesDetail(CUST_ORDER_NO);
        }

    }
}

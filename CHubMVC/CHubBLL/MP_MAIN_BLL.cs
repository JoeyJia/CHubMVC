using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;
using System.Data;
using CHubModel.WebArg;

namespace CHubBLL
{
    public class MP_MAIN_BLL
    {
        private MP_MAIN_DAL dal;
        public MP_MAIN_BLL()
        {
            dal = new MP_MAIN_DAL();
        }
        public List<E_WH_MST> GetWarehouseList()
        {
            return dal.GetWarehouseList();
        }
        public List<E_ORDER_STATUS> GetOrderStatusList()
        {
            return dal.GetOrderStatusList();
        }
        public List<E_SHIP_METHOD_MST> GetShipMethodList()
        {
            return dal.GetShipMethodList();
        }
        public List<E_ORDER_TYPE_MST> GetOrderTypeList()
        {
            return dal.GetOrderTypeList();
        }
        public List<V_E_SO_HEADER> MP_MAINSearch(MPMainArg arg)
        {
            return dal.MP_MAINSearch(arg);
        }
        public List<V_E_SO_DETAIL> MP_MAINDetail(string SO_NO)
        {
            return dal.MP_MAINDetail(SO_NO);
        }
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckSecurity(SECURE_ID, APP_USER);
        }
        public string RunF_CHECK_IN_GOMS(string SO_NO)
        {
            return dal.RunF_CHECK_IN_GOMS(SO_NO);
        }
        public void MP_MAINConfirm(string SO_NO, string Code, string Reason)
        {
            dal.MP_MAINConfirm(SO_NO, Code, Reason);
        }
    }
}

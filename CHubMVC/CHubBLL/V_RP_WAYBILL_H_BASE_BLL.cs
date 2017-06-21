using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class V_RP_WAYBILL_H_BASE_BLL
    {
        private readonly V_RP_WAYBILL_H_BASE_DAL dal;

        public V_RP_WAYBILL_H_BASE_BLL()
        {
            dal = new V_RP_WAYBILL_H_BASE_DAL();
        }
        public V_RP_WAYBILL_H_BASE_BLL(CHubEntities db)
        {
            dal = new V_RP_WAYBILL_H_BASE_DAL(db);
        }

        public List<RPWayBillHLevel1> GetWayBillBaseList(string whID, string carCode, string custName, string Address, string shipmentNo)
        {
            return dal.GetWayBillBaseList(whID, carCode, custName, Address, shipmentNo);
        }

        public List<RPWayBillHLevel2> GetWayBillDetailList(string carCode, string orderType, string addr)
        {
            return dal.GetWayBillDetailList(carCode, orderType,addr);
        }
    }
}

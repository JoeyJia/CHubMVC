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

        public List<V_RP_WAYBILL_H_BASE> GetWayBillBaseList(string whID, string wbType, string stageDate, string carCode, string custName, string Address, string shipmentNo)
        {
            return dal.GetWayBillBaseList(whID, wbType, stageDate, carCode, custName, Address, shipmentNo);
        }

    }
}

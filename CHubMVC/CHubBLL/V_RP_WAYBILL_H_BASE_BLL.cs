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

        public List<RPWayBillHLevel1> GetWayBillBaseList(string whID, string carCode, string custName, string Address, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            List<string> statusList = new List<string>();
            if (staged)
                statusList.Add("S");
            if (inProgress)
                statusList.Add("I");
            return dal.GetWayBillBaseList(whID, carCode, custName, Address, shipmentNo, statusList, printed);
        }

        public List<RPWayBillHLevel2> GetWayBillDetailList(string whID, string carCode, string orderType, string addr, string shipmentNo, bool staged, bool inProgress, string printed)
        {
            List<string> statusList = new List<string>();
            if (staged)
                statusList.Add("S");
            if (inProgress)
                statusList.Add("I");
            return dal.GetWayBillDetailList(whID, carCode, orderType, addr, shipmentNo, statusList, printed);
        }


        public void PreWorkRP_H(string WHID)
        {
            dal.PreWorkRP_H(WHID);
        }

        public void PreWorkRP_SMRY(string WHID)
        {
            dal.PreWorkRP_SMRY(WHID);
        }

        public void PreWorkRP_D(string WHID)
        {
            dal.PreWorkRP_D(WHID);
        }
    }
}

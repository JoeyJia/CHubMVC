using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using System.Data.Entity;

namespace CHubDAL
{
    public class V_RP_WAYBILL_H_BASE_DAL : BaseDAL
    {
        public V_RP_WAYBILL_H_BASE_DAL()
            : base() { }

        public V_RP_WAYBILL_H_BASE_DAL(CHubEntities db)
            : base(db) { }

        public List<V_RP_WAYBILL_H_BASE> GetWayBillBaseList(string whID, string wbType, string stageDate, string carCode, string custName, string Address, string shipmentNo)
        {
            DateTime sdDate = Convert.ToDateTime(stageDate);
            string sql = string.Format("select * from V_RP_WAYBILL_H_BASE where WH_ID='{0}' and to_char(STGDTE,'yyyy-MM-dd') ='{1}'",whID,stageDate);
            var baseresult = db.Database.SqlQuery<V_RP_WAYBILL_H_BASE>(sql).ToList();

            var result = baseresult.Where(a=>1==1);

            if (!string.IsNullOrEmpty(wbType))
                result = result.Where(a => a.WAYBILL_ID == wbType);
            //if (!string.IsNullOrEmpty(wbType))
            //    result = result.Where(a => a.STGDTE == wbType);
            if (!string.IsNullOrEmpty(carCode))
                result = result.Where(a => a.CARCOD == carCode);
            if (!string.IsNullOrEmpty(custName))
                result = result.Where(a => a.ADRNAM.Contains(custName));
            if (!string.IsNullOrEmpty(Address))
                result = result.Where(a => a.ADRLN1.Contains(Address));
            if (!string.IsNullOrEmpty(shipmentNo))
                result = result.Where(a => a.SHIP_ID == shipmentNo);


            return result.ToList();
        }

    }
}

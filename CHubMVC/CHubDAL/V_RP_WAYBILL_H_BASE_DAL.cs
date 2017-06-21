using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using System.Data.Entity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class V_RP_WAYBILL_H_BASE_DAL : BaseDAL
    {
        public V_RP_WAYBILL_H_BASE_DAL()
            : base() { }

        public V_RP_WAYBILL_H_BASE_DAL(CHubEntities db)
            : base(db) { }

        public List<RPWayBillHLevel1> GetWayBillBaseList(string whID, string carCode, string custName, string Address, string shipmentNo)
        {
            
            string sql = string.Format("select distinct ORDTYP_WB ,CARCOD,CARNAM,ADDR_COMBINED from V_RP_WAYBILL_H_BASE where WH_ID='{0}' and SHPSTS='S' ", whID);
            
            if (!string.IsNullOrEmpty(carCode))
                sql += string.Format(" and CARCOD='{0}'", carCode);
            if (!string.IsNullOrEmpty(custName))
                sql += string.Format(" and ADRNAM like '%{0}%'", custName);
            if (!string.IsNullOrEmpty(Address))
                sql += string.Format(" and ADRLN1 like '%{0}%'", Address);
            if (!string.IsNullOrEmpty(shipmentNo))
                sql += string.Format(" and SHIP_ID = '{0}'", shipmentNo);
            var result = db.Database.SqlQuery<RPWayBillHLevel1>(sql);
            return result.ToList();
        }

        public List<RPWayBillHLevel2> GetWayBillDetailList(string carCode, string orderType,string addr)
        {
            string sql = string.Format(@"select 
TRACK_NUM_IHUB,
          SHIP_ID,
          SHPSTS,
          STGDTE,
          ORDTYP,
         BOXES,
          VC_PALWGT,
          VOL_M3,
          CUST_NO,
          WAYBILL_ID,
          HOST_EXT_ID,
          ordtyp_wb
  from V_RP_WAYBILL_H_BASE
  where 1 = 1
  and carcod = '{0}'
  and ordtyp_wb = '{1}'
  and ADDR_COMBINED = '{2}'
  and SHPSTS = 'S'", carCode,orderType,addr);

            var result = db.Database.SqlQuery<RPWayBillHLevel2>(sql);
            return result.ToList();

        }

    }
}

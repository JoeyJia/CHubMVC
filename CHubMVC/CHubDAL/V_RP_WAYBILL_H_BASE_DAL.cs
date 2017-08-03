using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using System.Data.Entity;
using CHubModel.ExtensionModel;
using CHubCommon;
namespace CHubDAL
{
    public class V_RP_WAYBILL_H_BASE_DAL : BaseDAL
    {
        public V_RP_WAYBILL_H_BASE_DAL()
            : base() { }

        public V_RP_WAYBILL_H_BASE_DAL(CHubEntities db)
            : base(db) { }

        public List<RPWayBillHLevel1> GetWayBillBaseList(string whID, string carCode, string custName, string Address, string shipmentNo, List<string> statusList,string printed)
        {
            
            string sql = string.Format("select distinct ORDTYP_WB ,CARCOD,CARNAM,ADDR_COMBINED from V_RP_WAYBILL_H_BASE where WH_ID='{0}' ", whID);
            
            if (!string.IsNullOrEmpty(carCode))
                sql += string.Format(" and CARCOD='{0}'", carCode);
            if (!string.IsNullOrEmpty(custName))
                sql += string.Format(" and ADRNAM like '%{0}%'", custName);
            if (!string.IsNullOrEmpty(Address))
                sql += string.Format(" and ADDR_COMBINED like '%{0}%'", Address);
            if (!string.IsNullOrEmpty(shipmentNo))
                sql += string.Format(" and SHIP_ID = '{0}'", shipmentNo);

            //if (statusList != null && statusList.Count != 0)
                sql += string.Format(" and SHPSTS in ({0})", statusList.ToSqlInStr());

            if(printed=="N")
                sql += string.Format(" and IHUB_PRINTED = '{0}'", printed);

            var result = db.Database.SqlQuery<RPWayBillHLevel1>(sql);
            return result.OrderBy(a=>a.ORDTYP_WB).ThenBy(a=>a.CARCOD).ThenBy(a=>a.ADDR_COMBINED).ToList();
        }

        public List<RPWayBillHLevel2> GetWayBillDetailList(string whID,string carCode, string orderType,string addr, string shipmentNo, List<string> statusList, string printed)
        {
            string sql = string.Format(@"select 
h.TRACK_NUM_IHUB,
          h.SHIP_ID,
          h.WH_ID,
          h.SHPSTS,
          h.STGDTE,
          h.ORDTYP,
         h.BOXES,
          h.VC_PALWGT,
          h.VOL_M3,
          h.CUST_NO,
          h.WAYBILL_ID,
          h.HOST_EXT_ID,
          h.ordtyp_wb,
          case when (select 'X' from RP_SHIP_TRACK t
          where t.ship_id =h.ship_id and T.WH_ID =h.wh_id
           ) is not null then 'green'
           else '' end COLOR
          
  from V_RP_WAYBILL_H_BASE  h
  where 1 = 1
  and h.carcod = '{0}'
  and h.ordtyp_wb = '{1}'
  and h.ADDR_COMBINED = '{2}'
and h.WH_ID ='{3}' ", carCode,orderType,addr,whID);//and h.SHPSTS = 'S'

            if (statusList != null && statusList.Count != 0)
                sql += string.Format(" and h.SHPSTS in ({0})", statusList.ToSqlInStr());

            if (!string.IsNullOrEmpty(shipmentNo))
                sql += string.Format(" and SHIP_ID = '{0}'", shipmentNo);

            if (printed == "N")
                sql += string.Format(" and IHUB_PRINTED = '{0}'", printed);

            var result = db.Database.SqlQuery<RPWayBillHLevel2>(sql);
            return result.ToList();

        }

    }
}

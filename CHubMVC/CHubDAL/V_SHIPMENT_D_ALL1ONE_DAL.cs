using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class V_SHIPMENT_D_ALL1ONE_DAL : BaseDAL
    {
        public V_SHIPMENT_D_ALL1ONE_DAL()
            : base() { }

        public V_SHIPMENT_D_ALL1ONE_DAL(CHubEntities db)
            : base(db) { }


        public List<PLabelRow> GetPLabelRows(string shipmentNo, string boxNo, string printPartNo)
        {
            string sql = @"select 
sum(sd.untqty)/nvl(PB.QTY_IN_CARTON,1) copies,
PB.QTY_IN_CARTON MOQ,
sum(sd.untqty) SHIP_QTYS,
PB.PRINT_PART_NO ,
PB.PART_NO ,
PB.DESCRIPTION ,
PB.DESC_CN ,
PB.SHORT_DESCRIPTION ,
PB.COUNTRY_OF_ORIGIN, 
PB.PART_WEIGHT
 from V_SHIPMENT_D_ALL1ONE sd
inner join V_PLABEL_BASE pb on sd.prtnum = PB.PRINT_PART_NO
where 1 = 1 ";

            if (!string.IsNullOrEmpty(shipmentNo))
                sql += string.Format(" and sd.ship_id='{0}' ",shipmentNo);

            if (!string.IsNullOrEmpty(boxNo))
                sql += string.Format(" and SD.LODNUM='{0}' ", boxNo);

            if (!string.IsNullOrEmpty(printPartNo))
                sql += string.Format(" and SD.PRTNUM='{0}' ", printPartNo);

            sql += @"group by 
PB.QTY_IN_CARTON ,
PB.PRINT_PART_NO ,
PB.PART_NO ,
PB.DESCRIPTION ,
PB.DESC_CN ,
PB.SHORT_DESCRIPTION ,
PB.COUNTRY_OF_ORIGIN, 
PB.PART_WEIGHT";

            var result = db.Database.SqlQuery<PLabelRow>(sql);

           return result.OrderBy(a=>a.PART_NO).ToList();
        }

    }
}

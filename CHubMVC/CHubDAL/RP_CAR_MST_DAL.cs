using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class RP_CAR_MST_DAL : BaseDAL
    {
        public RP_CAR_MST_DAL()
            : base() { }

        public RP_CAR_MST_DAL(CHubEntities db)
            : base(db) { }

        public List<ExRPCarMst> GetCARListByCode(string carCode)
        {
            //return db.RP_CAR_MST.Where(a => a.CARCOD == carCode).ToList();
            var result = (
                from c in db.RP_CAR_MST
                join t in db.RP_WAYBILL_TYPE on c.WAYBILL_ID equals t.WAYBILL_ID
                where c.CARCOD == carCode
                select new ExRPCarMst {
                    WH_ID = c.WH_ID,
                    CARCOD = c.CARCOD,
                    CARNAM = c.CARNAM,
                    LOAD_DATE = c.LOAD_DATE,
                    WAYBILL_ID = c.WAYBILL_ID,
                    SEND_TO_TMS = c.SEND_TO_TMS,
                    CARNAM_SHORT = c.CARNAM_SHORT,
                    WAYBILL_DESC = t.WAYBILL_DESC
                }
                );
            return result.ToList();
        }

        public void SaveCARWayBillID(ExRPCarMst model)
        {
            RP_CAR_MST exist = db.RP_CAR_MST.FirstOrDefault(a => a.WH_ID == model.WH_ID && a.CARCOD == model.CARCOD);
            exist.WAYBILL_ID = model.WAYBILL_ID;
            exist.SEND_TO_TMS = model.SEND_TO_TMS;
            exist.CARNAM_SHORT = model.CARNAM_SHORT;
            Update(exist);
        }

        public List<DistinctCarCode> GetDistinctCarCode()
        {
            var result = (
                from c in db.RP_CAR_MST
                select new DistinctCarCode {
                    CarCode = c.CARCOD,
                    CarName = c.CARNAM
                }
                ).Distinct().ToList();

            //some data have same carCode but diff carName, so need to handle
            List<DistinctCarCode> realList = new List<DistinctCarCode>();
            foreach (var item in result)
            {
                DistinctCarCode exist = realList.FirstOrDefault(a => a.CarCode == item.CarCode);
                if (exist == null)
                    realList.Add(item);
                else
                {
                    //carName get the longer one
                    if ((item.CarName ?? string.Empty).Length > (exist.CarName ?? string.Empty).Length)
                        exist.CarName = item.CarName;
                }
            }
            return realList;
        
        }

    }
}

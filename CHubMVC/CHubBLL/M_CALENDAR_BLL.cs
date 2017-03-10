using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using CHubCommon;

namespace CHubBLL
{
    public class M_CALENDAR_BLL
    {
        private readonly M_CALENDAR_DAL dal;

        public M_CALENDAR_BLL()
        {
            dal = new M_CALENDAR_DAL();
        }
        public M_CALENDAR_BLL(CHubEntities db)
        {
            dal = new M_CALENDAR_DAL(db);
        }

        public DateTime? GetArrivalDateFromOutDate(DateTime outDate)
        {
            List<M_CALENDAR> next3Days =  dal.GetDateRange(outDate, 3);
            foreach (var item in next3Days)
            {
                if (item.WORK_DAY_FLAG == CHubConstValues.IndY)
                    return item.CAL_DATE;
            }
            return null;
        }

    }
}

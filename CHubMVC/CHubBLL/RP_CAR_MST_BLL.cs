using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class RP_CAR_MST_BLL
    {
        private readonly RP_CAR_MST_DAL dal;

        public RP_CAR_MST_BLL()
        {
            dal = new RP_CAR_MST_DAL();
        }
        public RP_CAR_MST_BLL(CHubEntities db)
        {
            dal = new RP_CAR_MST_DAL(db);
        }

        public List<RP_CAR_MST> GetCARListByCode(string carCode)
        {
            return dal.GetCARListByCode(carCode);
        }

        public List<DistinctCarCode> GetDistinctCarCode()
        {
            return dal.GetDistinctCarCode();
        }

        public void SaveCARWayBillID(RP_CAR_MST model)
        {
            dal.SaveCARWayBillID(model);
        }



    }
}

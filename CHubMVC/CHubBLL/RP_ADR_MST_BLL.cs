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
    public class RP_ADR_MST_BLL
    {
        private readonly RP_ADR_MST_DAL dal;

        public RP_ADR_MST_BLL()
        {
            dal = new RP_ADR_MST_DAL();
        }
        public RP_ADR_MST_BLL(CHubEntities db)
        {
            dal = new RP_ADR_MST_DAL(db);
        }

        public List<ExRPADRMst> GetADRListByCustName(string custName)
        {
            return dal.GetADRListByCustName(custName);
        }

        public void SaveCustPackID(ExRPADRMst model)
        {
            dal.SaveCustPackID(model);
        }

    }
}

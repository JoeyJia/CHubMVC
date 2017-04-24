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
    public class ITT_PO_BLL
    {
        private readonly ITT_PO_DAL dal;

        public ITT_PO_BLL()
        {
            dal = new ITT_PO_DAL();
        }
        public ITT_PO_BLL(CHubEntities db)
        {
            dal = new ITT_PO_DAL(db);
        }

        public List<ITT_PO_LEVEL_1> GetLevel1Data(string partNo)
        {
            return dal.GetLevel1Data(partNo);
        }

        public List<ITT_PO_LEVEL_2> GetLevel2Data(string partNo, string poNo)
        {
            return dal.GetLevel2Data(partNo, poNo);
        }

        public List<ITT_PO_Release> GetPOReleaseData(string partNo, string poNo, long poLineNo)
        {
            return dal.GetPOReleaseData(partNo, poNo, poLineNo);
        }
    }
}

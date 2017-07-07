using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_AUTOPACK_LOG_BLL
    {
        private readonly RP_AUTOPACK_LOG_DAL dal;

        public RP_AUTOPACK_LOG_BLL()
        {
            dal = new RP_AUTOPACK_LOG_DAL();
        }
        public RP_AUTOPACK_LOG_BLL(CHubEntities db)
        {
            dal = new RP_AUTOPACK_LOG_DAL(db);
        }

        public bool HasSuccessPrint(string lodNum)
        {
            return dal.HasSuccessPrint(lodNum);
        }

        public void AddOrUpdatePrintLog(RP_AUTOPACK_LOG model)
        {
            RP_AUTOPACK_LOG exist = dal.GetSpecifyLog(model.LODNUM);
            if (exist == null)
                dal.Add(model);
            else {
                if ((exist.SUCCEE_FLAG ?? "N") != CHubCommon.CHubConstValues.IndY)
                {
                    exist.SUCCEE_FLAG = model.SUCCEE_FLAG;
                    exist.AUTO_PRINT_DATE = model.AUTO_PRINT_DATE;
                    dal.Update(exist);
                }
            }

        }

    }
}

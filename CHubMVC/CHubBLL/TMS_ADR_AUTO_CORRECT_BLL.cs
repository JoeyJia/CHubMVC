using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
  public class TMS_ADR_AUTO_CORRECT_BLL
    {
        private readonly TMS_ADR_AUTO_CORRECT_DAL dal;

        public TMS_ADR_AUTO_CORRECT_BLL()
        {
            dal = new TMS_ADR_AUTO_CORRECT_DAL();
        }

        public TMS_ADR_AUTO_CORRECT_BLL(CHubEntities db)
        {
            dal = new TMS_ADR_AUTO_CORRECT_DAL(db);
        }

        public List<TMS_ADR_AUTO_CORRECT> SearchADRCRT(string ADRNAM, string ADRLN1, int LOAD_DATE)
        {
            return dal.SearchADRCRT(ADRNAM, ADRLN1, LOAD_DATE);
        }

        public void SaveADRCRT(TMS_ADR_AUTO_CORRECT tc)
        {
            dal.SaveADRCRT(tc);
        }


    }
}

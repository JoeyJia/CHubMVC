using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class M_ADRNAM_MST_BLL
    {
        private M_ADRNAM_MST_DAL dal;
        public M_ADRNAM_MST_BLL()
        {
            dal = new M_ADRNAM_MST_DAL();
        }

        public M_ADRNAM_MST_BLL(CHubEntities db)
        {
            dal = new M_ADRNAM_MST_DAL(db);
        }

        public List<RP_LABEL_TYPE2> GetLabelCode()
        {
            return dal.GetLabelCode();
        }

        public List<M_ADRNAM_MST> GetCustAddt(string ADRNAM)
        {
            return dal.GetCustAddt(ADRNAM);
        }

        public void SaveCustAddt(M_ADRNAM_MST mam)
        {
            dal.SaveCustAddt(mam);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class IA_LOD_DTL_BLL
    {
        private readonly IA_LOD_DTL_DAL dal;
        public IA_LOD_DTL_BLL()
        {
            dal = new IA_LOD_DTL_DAL();
        }

        public IA_LOD_DTL_BLL(CHubEntities db)
        {
            dal = new IA_LOD_DTL_DAL(db);
        }

        public List<IA_LOD_DTL> GetIaLodDtl(string LODNUM, string PRTNUM)
        {
            return dal.GetIaLodDtl(LODNUM, PRTNUM);
        }

        public string GetNewPRTNUM(string PRTNUM)
        {
            return dal.GetNewPRTNUM(PRTNUM);
        }

        public void SaveIALODDTL(IA_LOD_DTL iad)
        {
            dal.SaveIALODDTL(iad);
        }
    }
}

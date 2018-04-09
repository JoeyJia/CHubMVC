using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_IA_LOD_BASE_BLL
    {
        private readonly V_IA_LOD_BASE_DAL dal;

        public V_IA_LOD_BASE_BLL()
        {
            dal = new V_IA_LOD_BASE_DAL();
        }
        public V_IA_LOD_BASE_BLL(CHubEntities db)
        {
            dal = new V_IA_LOD_BASE_DAL(db);
        }

        public List<CHubDBEntity.UnmanagedModel.V_IA_LOD_BASE> GetInfoFromBASE(string LODNUM_DISPLAY)
        {
            return dal.GetInfoFromBASE(LODNUM_DISPLAY);
        }

        public void RunProc(string LODNUM, string UserID)
        {
            dal.RunProc(LODNUM, UserID);
        }
    }
}

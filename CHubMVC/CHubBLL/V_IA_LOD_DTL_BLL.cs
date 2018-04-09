using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_IA_LOD_DTL_BLL
    {
        private readonly V_IA_LOD_DTL_DAL dal;

        public V_IA_LOD_DTL_BLL()
        {
            dal = new V_IA_LOD_DTL_DAL();
        }

        public V_IA_LOD_DTL_BLL(CHubEntities db)
        {
            dal = new V_IA_LOD_DTL_DAL(db);
        }

        public List<CHubDBEntity.UnmanagedModel.V_IA_LOD_DTL> GetInfoLODDTL(string LODNUM)
        {
            return dal.GetInfoLODDTL(LODNUM);
        }

        public List<V_IA_LOD_DTL> GetVIALODDTL(string LODNUM)
        {
            return dal.GetVIALODDTL(LODNUM);
        }




    }
}

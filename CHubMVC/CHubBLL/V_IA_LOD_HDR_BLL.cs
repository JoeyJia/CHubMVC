using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_IA_LOD_HDR_BLL
    {
        private readonly V_IA_LOD_HDR_DAL dal;
        public V_IA_LOD_HDR_BLL()
        {
            dal = new V_IA_LOD_HDR_DAL();
        }

        public V_IA_LOD_HDR_BLL(CHubEntities db)
        {
            dal = new V_IA_LOD_HDR_DAL(db);
        }

        public List<V_IA_LOD_HDR> GetInfoFromHDR(string LODNUM_DISPLAY)
        {
            return dal.GetInfoFromHDR(LODNUM_DISPLAY);
        }

        public List<V_IA_LOD_HDR> GetVIALODHDR(string WH_ID, string ADRNAM, string LODNUM_DISPLAY, string NEED_SIGN_YN, string IA_STATUS, int CREATE_DATE)
        {
            return dal.GetVIALODHDR(WH_ID, ADRNAM, LODNUM_DISPLAY, NEED_SIGN_YN, IA_STATUS, CREATE_DATE);
        }

        public IA_LOD_HDR GetIALODHDR(string LODNUM)
        {
            return dal.GetIALODHDR(LODNUM);
        }

        public void UpdateIALODHDR(IA_LOD_HDR iah)
        {
            dal.UpdateIALODHDR(iah);
        }


        public List<IA_STATUS_CODE> GetIAStatus()
        {
            return dal.GetIAStatus();
        }

        public bool CheckUser(string UserName)
        {
            return dal.CheckUser(UserName);
        }


    }
}

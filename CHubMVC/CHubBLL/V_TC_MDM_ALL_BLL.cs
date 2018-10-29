using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_TC_MDM_ALL_BLL
    {
        private readonly V_TC_MDM_ALL_DAL dal;

        public V_TC_MDM_ALL_BLL()
        {
            dal = new V_TC_MDM_ALL_DAL();
        }
        public V_TC_MDM_ALL_BLL(CHubEntities db)
        {
            dal = new V_TC_MDM_ALL_DAL(db);
        }

        public List<V_TC_MDM_ALL> GetTCMDMList(string partNo, string hsCode, string declrName, string element, int currentPage, int pageSize, out int totalCount)
        {
            return dal.GetTCMDMList(partNo, hsCode, declrName, element,currentPage,pageSize,out totalCount);
        }

        public V_TC_MDM_ALL GetSpecifyMDM(string partNo)
        {
            return dal.GetSpecifyMDM(partNo);
        }

        public string GetGOOD_DESC(string HSCODE, string CIQ)
        {
            return dal.GetGOOD_DESC(HSCODE, CIQ);
        }

        public string GetELEMENTCK(string PART_NO)
        {
            return dal.GetELEMENTCK(PART_NO);
        }

        public List<TC_HSCODE_CIQ_MST> GetCIQLists(string HSCODE)
        {
            return dal.GetCIQLists(HSCODE);
        }

    }
}

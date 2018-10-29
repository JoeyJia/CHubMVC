using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_PLABEL_BY_MOBILE_PRINT_BLL
    {
        private V_PLABEL_BY_MOBILE_PRINT_DAL dal;
        public V_PLABEL_BY_MOBILE_PRINT_BLL()
        {
            dal = new V_PLABEL_BY_MOBILE_PRINT_DAL();
        }
        public string GetADRNAM(string LODNUM)
        {
            return dal.GetADRNAM(LODNUM);
        }
        public void RunPRE_WORK_MOBILE_PRINT(string WH_ID, string LODNUM)
        {
            dal.RunPRE_WORK_MOBILE_PRINT(WH_ID, LODNUM);
        }
        public void RunPRE_WORK_MOBILE_UnCatalog(string WH_ID, string PRTNUM)
        {
            dal.RunPRE_WORK_MOBILE_UnCatalog(WH_ID, PRTNUM);
        }
        public V_PLABEL_BY_MOBILE_PRINT GetV_PLABEL_BY_MOBILE_PRINT(string WH_ID, string LODNUM, string PRTNUM, string LABEL_CODE)
        {
            return dal.GetV_PLABEL_BY_MOBILE_PRINT(WH_ID, LODNUM, PRTNUM, LABEL_CODE);
        }
        public List<V_PLABEL_BY_MOBILE_PRINT> GetPrintData(string VID, string WH_ID, string LODNUM, string PRTNUM, string LABEL_CODE)
        {
            return dal.GetPrintData(VID, WH_ID, LODNUM, PRTNUM, LABEL_CODE);
        }
    }
}

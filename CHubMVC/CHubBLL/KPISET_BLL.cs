using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;
using CHubModel.WebArg;

namespace CHubBLL
{
    public class KPISET_BLL
    {
        public KPISET_DAL dal;
        public KPISET_BLL()
        {
            dal = new KPISET_DAL();
        }
        public List<V_DASH_KPI_AUTH> GetKpiCode(string AppUser)
        {
            return dal.GetKpiCode(AppUser);
        }
        public List<DASH_KPI_HISTORY> KpiSetSearch(string ORG_ID, string KPI_CODE, string KPI_YEAR)
        {
            var result = dal.KpiSetSearch(ORG_ID, KPI_CODE, KPI_YEAR);
            return result;
        }
        public void KpiSetSave(KpiSetArg arg)
        {
            dal.KpiSetSave(arg);
        }
    }
}

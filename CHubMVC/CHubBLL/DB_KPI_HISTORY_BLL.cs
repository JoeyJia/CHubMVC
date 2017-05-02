using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class DB_KPI_HISTORY_BLL
    {
        private readonly DB_KPI_HISTORY_DAL dal;

        public DB_KPI_HISTORY_BLL()
        {
            dal = new DB_KPI_HISTORY_DAL();
        }
        public DB_KPI_HISTORY_BLL(CHubEntities db)
        {
            dal = new DB_KPI_HISTORY_DAL(db);
        }

        public List<ExDBKPIHistory> GetLatestHistory(List<string> codeList, string kpiGroup)
        {
            return dal.GetLatestHistory(codeList, kpiGroup);
        }

        public List<string> GetDistinctKPICode(string kpiGroup)
        {
            return dal.GetDistinctKPICode(kpiGroup);
        }

        public List<DB_KPI_HISTORY> GetTrendData(string code, string subCode)
        {
            return dal.GetTrendData(code, subCode);
        }
    }
}

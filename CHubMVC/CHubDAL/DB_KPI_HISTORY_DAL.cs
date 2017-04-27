using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class DB_KPI_HISTORY_DAL : BaseDAL
    {
        public DB_KPI_HISTORY_DAL()
            : base() { }

        public DB_KPI_HISTORY_DAL(CHubEntities db)
            : base(db) { }

        public List<ExDBKPIHistory> GetLatestHistory(string kpiGroup)
        {
            var maxDate=(from a in db.DB_KPI_HISTORY
                        join b in db.DB_KPI on new { a.KPI_CODE, a.KPI_SUB_CODE } equals new { b.KPI_CODE, b.KPI_SUB_CODE }
                        where b.KPI_GROUP == kpiGroup
                        select a.KPI_DATE
                        ).Max();

               var result = (from a in db.DB_KPI_HISTORY
                          join b in db.DB_KPI on new { a.KPI_CODE,a.KPI_SUB_CODE} equals new { b.KPI_CODE,b.KPI_SUB_CODE}
                          where b.KPI_GROUP==kpiGroup
                          && a.KPI_DATE ==maxDate
                          select new ExDBKPIHistory
                          {
                              KPI_DATE = a.KPI_DATE,
                              KPI_CODE = a.KPI_CODE,
                              KPI_SUB_CODE = a.KPI_SUB_CODE,
                              KPI_VALUE = a.KPI_VALUE,
                              KPI_TARGET = a.KPI_TARGET,
                              IND_Y = a.IND_Y,
                              KPI_OWNER = a.KPI_OWNER,
                              NOTE = a.NOTE,
                              OWNER_HIGHLIGHT = a.OWNER_HIGHLIGHT,
                              HIGHLIGHT_DATE = a.HIGHLIGHT_DATE,
                              DESC = b.KPI_DESC
                          }
                          );

            return result.ToList();
        }

    }
}

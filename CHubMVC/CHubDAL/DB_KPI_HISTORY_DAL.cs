using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using System.Data.Entity;

namespace CHubDAL
{
    public class DB_KPI_HISTORY_DAL : BaseDAL
    {
        public DB_KPI_HISTORY_DAL()
            : base() { }

        public DB_KPI_HISTORY_DAL(CHubEntities db)
            : base(db) { }

        public List<ExDBKPICode> GetDistinctKPICode(string kpiGroup)
        {
            var codeList = (from a in db.DB_KPI_HISTORY
                            join b in db.DB_KPI on new { a.KPI_CODE, a.KPI_SUB_CODE } equals new { b.KPI_CODE, b.KPI_SUB_CODE }
                            join c in db.DB_KPI_CODE on b.KPI_CODE equals c.KPI_CODE
                            where b.KPI_GROUP == kpiGroup
                            select new ExDBKPICode {
                                KPI_CODE = a.KPI_CODE,
                                KPI_CODE_DESC = c.KPI_CODE_DESC
                            }
                        );
            if (codeList.Count() == 0)
                return null;
            return codeList.Distinct().ToList();
        }


        public List<ExDBKPIHistory> GetLatestHistory(List<ExDBKPICode> codeList, string kpiGroup)
        {
            if (codeList == null || codeList.Count == 0)
                return null;
            List<ExDBKPIHistory> result = new List<ExDBKPIHistory>();

            foreach (var item in codeList)
            {
                var dateList = (from a in db.DB_KPI_HISTORY
                                join b in db.DB_KPI on new { a.KPI_CODE, a.KPI_SUB_CODE } equals new { b.KPI_CODE, b.KPI_SUB_CODE }
                                where b.KPI_GROUP == kpiGroup
                                && a.KPI_CODE==item.KPI_CODE
                                select a.KPI_DATE
                        );
                if (dateList.Count() == 0)
                    continue;
                DateTime maxDate = dateList.Max();

                var resultUnit = (from a in db.DB_KPI_HISTORY
                              join b in db.DB_KPI on new { a.KPI_CODE, a.KPI_SUB_CODE } equals new { b.KPI_CODE, b.KPI_SUB_CODE }
                              where b.KPI_GROUP == kpiGroup
                              && a.KPI_DATE == maxDate
                              && a.KPI_CODE==item.KPI_CODE
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
                result.AddRange(resultUnit.ToList());

            }
            return result;
        }

        public List<DB_KPI_HISTORY> GetTrendData(string code, string subCode)
        {
            return db.DB_KPI_HISTORY.Where(a => a.KPI_CODE == code 
            && a.KPI_SUB_CODE == subCode 
            && a.KPI_DATE.Year == DateTime.Now.Year).OrderBy(a=>a.KPI_DATE).ToList();
        }

    }
}

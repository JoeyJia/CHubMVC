using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class KPISET_DAL
    {
        private CHubCommonHelper ccHelper;
        public KPISET_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_DASH_KPI_AUTH> GetKpiCode(string AppUser)
        {
            string sql = string.Format(@"select * from V_DASH_KPI_AUTH where APP_USER='{0}'", AppUser);
            var result = ccHelper.ExecuteSqlToList<V_DASH_KPI_AUTH>(sql);
            return result;
        }

        public List<DASH_KPI_HISTORY> KpiSetSearch(string ORG_ID, string KPI_CODE, string KPI_YEAR)
        {
            string sql = string.Format(@"select * from DASH_KPI_HISTORY where 1=1");
            if (!string.IsNullOrEmpty(ORG_ID))
                sql += string.Format(@" and ORG_ID='{0}'", ORG_ID);
            if (!string.IsNullOrEmpty(KPI_CODE))
                sql += string.Format(@" and KPI_CODE='{0}'", KPI_CODE);
            if (!string.IsNullOrEmpty(KPI_YEAR))
                sql += string.Format(@" and KPI_YEAR='{0}'", KPI_YEAR);
            var result = ccHelper.ExecuteSqlToList<DASH_KPI_HISTORY>(sql);
            return result;
        }

        public void KpiSetSave(KpiSetArg arg)
        {
            string sql = string.Format(@"Update DASH_KPI_HISTORY set KPI_VAL_01=:KPI_VAL_01,KPI_VAL_02=:KPI_VAL_02,KPI_VAL_03=:KPI_VAL_03,
                                KPI_VAL_04=:KPI_VAL_04,KPI_VAL_05=:KPI_VAL_05,KPI_VAL_06=:KPI_VAL_06,KPI_VAL_07=:KPI_VAL_07,KPI_VAL_08=:KPI_VAL_08,
                                KPI_VAL_09=:KPI_VAL_09,KPI_VAL_10=:KPI_VAL_10,KPI_VAL_11=:KPI_VAL_11,KPI_VAL_12=:KPI_VAL_12,
                                NOTE=:NOTE where KPI_YEAR=:KPI_YEAR and ORG_ID=:ORG_ID and KPI_CODE=:KPI_CODE and KPI_SUB_CODE=:KPI_SUB_CODE");
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":KPI_VAL_01",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_02",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_03",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_04",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_05",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_06",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_07",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_08",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_09",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_10",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_11",OracleDbType.Decimal),
                new OracleParameter(":KPI_VAL_12",OracleDbType.Decimal),
                new OracleParameter(":NOTE",OracleDbType.Varchar2),
                new OracleParameter(":KPI_YEAR",OracleDbType.Decimal),
                new OracleParameter(":ORG_ID",OracleDbType.Varchar2),
                new OracleParameter(":KPI_CODE",OracleDbType.Varchar2),
                new OracleParameter(":KPI_SUB_CODE",OracleDbType.Varchar2)
            };
            param[0].Value = arg.KPI_VAL_01;
            param[1].Value = arg.KPI_VAL_02;
            param[2].Value = arg.KPI_VAL_03;
            param[3].Value = arg.KPI_VAL_04;
            param[4].Value = arg.KPI_VAL_05;
            param[5].Value = arg.KPI_VAL_06;
            param[6].Value = arg.KPI_VAL_07;
            param[7].Value = arg.KPI_VAL_08;
            param[8].Value = arg.KPI_VAL_09;
            param[9].Value = arg.KPI_VAL_10;
            param[10].Value = arg.KPI_VAL_11;
            param[11].Value = arg.KPI_VAL_12;
            param[12].Value = arg.NOTE;
            param[13].Value = arg.KPI_YEAR;
            param[14].Value = arg.ORG_ID;
            param[15].Value = arg.KPI_CODE;
            param[16].Value = arg.KPI_SUB_CODE;
            ccHelper.AddOrUpdateWithParams(sql, param);
        }

    }
}

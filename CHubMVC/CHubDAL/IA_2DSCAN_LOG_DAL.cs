using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class IA_2DSCAN_LOG_DAL : BaseDAL
    {
        public IA_2DSCAN_LOG_DAL() : base()
        {

        }
        public IA_2DSCAN_LOG_DAL(CHubEntities db) : base(db)
        {

        }

        public string GetConvertStr(string ScanStr)
        {
            string sql = string.Format(@"select GET_PARTNO('{0}') from dual", ScanStr);
            var result = db.Database.SqlQuery<string>(sql).ToList();
            return result[0];
        }


        public bool CheckPrintPartNo(string ConvertStr)
        {
            string sql = string.Format(@"select * from G_PART_ADDTIONAL where PRINT_PART_NO='{0}'", ConvertStr);
            var result = db.Database.SqlQuery<G_PART_ADDTIONAL>(sql).ToList();
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public bool CheckInputStr(string ConvertStr)
        {
            string sql = string.Format(@"select * from IA_PART_AUTOMAP where PRTNUM='{0}'", ConvertStr);
            var result = db.Database.SqlQuery<IA_PART_AUTOMAP>(sql).ToList();
            if (result != null && result.Any())
                return true;
            else
                return false;
        }



        public void LogScanStr(IA_2DSCAN_LOG scanlog)
        {
            base.CheckCultureInfoForDate();
            base.Add(scanlog);
        }


    }
}

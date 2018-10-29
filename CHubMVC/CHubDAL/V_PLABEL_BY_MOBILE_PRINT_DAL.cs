using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using Oracle.ManagedDataAccess.Client;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_PLABEL_BY_MOBILE_PRINT_DAL
    {
        private CHubCommonHelper ccHelper;
        public V_PLABEL_BY_MOBILE_PRINT_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public string GetADRNAM(string LODNUM)
        {
            string sql = string.Format(@"select distinct ADRNAM from V_PLABEL_BY_MOBILE_PRINT where trim(lodnum)='{0}'", LODNUM);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public void RunPRE_WORK_MOBILE_PRINT(string WH_ID, string LODNUM)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_wh",OracleDbType.Varchar2,100),
                new OracleParameter("v_doc",OracleDbType.Varchar2,100)
            };
            param[0].Value = WH_ID;
            param[1].Value = !string.IsNullOrEmpty(LODNUM) ? LODNUM : " ";

            ccHelper.ExecProcedure("PRE_WORK_MOBILE_PRINT", param);
        }

        public void RunPRE_WORK_MOBILE_UnCatalog(string WH_ID, string PRTNUM)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_wh",OracleDbType.Varchar2,100),
                new OracleParameter("v_prtnum",OracleDbType.Varchar2,100)
            };
            param[0].Value = WH_ID;
            param[1].Value = PRTNUM;
            ccHelper.ExecProcedure("PRE_WORK_MOBILE_UnCatalog", param);
        }

        public V_PLABEL_BY_MOBILE_PRINT GetV_PLABEL_BY_MOBILE_PRINT(string WH_ID, string LODNUM, string PRTNUM, string LABEL_CODE)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_MOBILE_PRINT where WH_ID='{0}' and trim(LODNUM)='{1}' and PRTNUM='{2}' and LABEL_CODE='{3}' and rownum =1",
                                        WH_ID, string.IsNullOrEmpty(LODNUM) ? "NA" : LODNUM, PRTNUM, LABEL_CODE);
            var result = ccHelper.Search<V_PLABEL_BY_MOBILE_PRINT>(sql).ToList().FirstOrDefault();
            return result;
        }

        public List<V_PLABEL_BY_MOBILE_PRINT> GetPrintData(string VID, string WH_ID, string LODNUM, string PRTNUM, string LABEL_CODE)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_MOBILE_PRINT where VID='{0}' and WH_ID='{1}' and trim(LODNUM)='{2}' and PRTNUM='{3}' and LABEL_CODE='{4}'",
                                        VID, WH_ID, LODNUM, PRTNUM, LABEL_CODE);
            var result = ccHelper.Search<V_PLABEL_BY_MOBILE_PRINT>(sql).ToList();
            return result;
        }


    }
}

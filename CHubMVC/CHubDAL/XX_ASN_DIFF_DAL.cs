using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;
using Oracle.ManagedDataAccess.Client;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class XX_ASN_DIFF_DAL : BaseDAL
    {
        private CHubCommonHelper ccHelper;
        public XX_ASN_DIFF_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<XX_ASN_DIFF> GetAsnDiffListBySearch(AsnDiffArg AsnDiffSearch)
        {
            string sqlwhere = "";
            if(!string.IsNullOrEmpty(AsnDiffSearch.WareHouse))
            {
                sqlwhere += " and warehouse='" + AsnDiffSearch.WareHouse + "'";
            }
            if (!string.IsNullOrEmpty(AsnDiffSearch.AsnNo))
            {
                sqlwhere += " and manifest_id='" + AsnDiffSearch.AsnNo + "'";
            }
            if(AsnDiffSearch.CreateDate_Start!=null && AsnDiffSearch.CreateDate_Start.Value!=null)
            {
                sqlwhere += " and CREATE_DATE>='" + AsnDiffSearch.CreateDate_Start.Value.ToString() + "'";
            }
            if (AsnDiffSearch.CreateDate_End != null && AsnDiffSearch.CreateDate_End.Value != null)
            {
                sqlwhere += " and CREATE_DATE<='" + AsnDiffSearch.CreateDate_End.Value.ToString() + "'";
            }
            if(!string.IsNullOrEmpty(AsnDiffSearch.ResultType))
            {
                sqlwhere += " and CLAIM_RESULT='" + AsnDiffSearch.ResultType + "'";
            }
            if(AsnDiffSearch.IsClose!="-1")
            {
                sqlwhere += " and IsClose='" + AsnDiffSearch.IsClose + "'";
            }
            string sql = string.Format(@"select * from XX_ASN_DIFF where 1=1 "+sqlwhere);
            var result = ccHelper.ExecuteSqlToList<XX_ASN_DIFF>(sql);
            return result;
        }
        public List<V_DIS_ASN_BASE> GetAsnDiffInfo(string warehouse,string AsnID)
        {
            string sql = string.Format(@"select * from V_DIS_ASN_BASE where warehouse='" + warehouse + "' and manifestId='" + AsnID + "'");
            var result = ccHelper.ExecuteSqlToList<V_DIS_ASN_BASE>(sql);
            return result;
        }
        public void MP_MAINConfirm(string warehouse, string asnid,out string SaveMsg)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":V_SO_NO",OracleDbType.Varchar2),
                new OracleParameter(":V_opt_code",OracleDbType.Varchar2),
                new OracleParameter(":V_Reason",OracleDbType.Varchar2)
            };
            param[0].Value = warehouse;
            param[1].Value = asnid;
            SaveMsg = "faild";
            param[2].Direction = System.Data.ParameterDirection.Output;
            ccHelper.ExecProcedureWithParams("XX_P_SaveAsnDiff", param);
            SaveMsg = param[2].ToString();
        }
    }
}


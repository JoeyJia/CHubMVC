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

        /// <summary>
        /// 更新订单明细
        /// </summary>
        /// <param name="asndiff"></param>
        /// <returns></returns>
        public string UpdateXXAsnDiff(XX_ASN_DIFF asndiff)
        {
            string sqlwhere = string.Empty;
            if (asndiff == null || asndiff.ASN_DIFF_ID == 0)
            {
                return "请求数据有误";
            }
            string sqlmodel = string.Format(@"select * from XX_ASN_DIFF where ASN_DIFF_ID=" + asndiff.ASN_DIFF_ID);
            List<XX_ASN_DIFF> res = ccHelper.ExecuteSqlToList<XX_ASN_DIFF>(sqlmodel);
            if (res == null || res.Count <= 0)
            {
                return "订单不存在";
            }
            if (res[0].IS_CLOSE == "1")
            {
                return "订单已关闭";
            }
            if (!string.IsNullOrEmpty(asndiff.DISP_ACTION))
            {
                sqlwhere += " DISP_ACTION='" + asndiff.DISP_ACTION + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.PLANNER_NOTES))
            {
                sqlwhere += " PLANNER_NOTES='" + asndiff.PLANNER_NOTES + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.WRITE_OFF_NO))
            {
                sqlwhere += " WRITE_OFF_NO='" + asndiff.WRITE_OFF_NO + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.INVOICE_NO))
            {
                sqlwhere += " INVOICE_NO='" + asndiff.INVOICE_NO + "' ,";
            }
            if (asndiff.SHIP_DATE != null && asndiff.SHIP_DATE.Value != null)
            {
                sqlwhere += " SHIP_DATE='" + asndiff.SHIP_DATE.Value.ToString() + "' ,";
            }
            if (asndiff.QTY_FIN_WRITEOFF != 0)
            {
                sqlwhere += " QTY_FIN_WRITEOFF=" + asndiff.QTY_FIN_WRITEOFF + " ,";
            }
            if (asndiff.CLAIM_DATE != null && asndiff.CLAIM_DATE.Value != null)
            {
                sqlwhere += " CLAIM_DATE='" + asndiff.CLAIM_DATE.Value.ToString() + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.CLAIM_NOTES))
            {
                sqlwhere += " CLAIM_NOTES='" + asndiff.CLAIM_NOTES + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.CLAIM_NO))
            {
                sqlwhere += " CLAIM_NO='" + asndiff.CLAIM_NO + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.CLAIM_RESULT))
            {
                sqlwhere += " CLAIM_RESULT='" + asndiff.CLAIM_RESULT + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.CREDIT_NO))
            {
                sqlwhere += " CREDIT_NO='" + asndiff.CREDIT_NO + "' ,";
            }

            if (!string.IsNullOrEmpty(asndiff.CLAIM_DENY_REASON))
            {
                sqlwhere += " CLAIM_DENY_REASON='" + asndiff.CLAIM_DENY_REASON + "' ,";
            }
            if (!string.IsNullOrEmpty(asndiff.RECEIVE_CREDIT))
            {
                sqlwhere += " RECEIVE_CREDIT='" + asndiff.RECEIVE_CREDIT + "' ,";
            }
            if (asndiff.CLOSE_DATE != null && asndiff.CLOSE_DATE.Value != null)
            {
                sqlwhere += " CLOSE_DATE='" + asndiff.CLOSE_DATE + "' ,";
                sqlwhere += " Is_Close='1' ,";
            }
            if (!string.IsNullOrEmpty(sqlwhere))
            {
                sqlwhere = sqlwhere.Substring(0, sqlwhere.Length - 1);
                string sql = string.Format(@"UPDATE XX_ASN_DIFF SET " + sqlwhere + " where ASN_DIFF_ID= " + asndiff.ASN_DIFF_ID);
                ccHelper.ExecuteNonQuery(sql);
            }
            return "";
        }

        /// <summary>
        /// 更新订单备注
        /// </summary>
        /// <param name="asnid"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool UpdateXXAsnDiffRemark(string asnid, string remark)
        {
            string sql = string.Format(@"UPDATE XX_ASN_DIFF SET DIFFREMARK=' " + remark + " where ASN_DIFF_ID= " + asnid);
            ccHelper.ExecuteNonQuery(sql);
            return true;
        }

        public XX_ASN_DIFF GetAsnDiffById(long asndiffid)
        {
            string sql = string.Format(@"select * from XX_ASN_DIFF where 1=1 ASN_DIFF_ID=" + asndiffid);
            var result = ccHelper.ExecuteSqlToList<XX_ASN_DIFF>(sql).FirstOrDefault();
            return result;
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

                sqlwhere += " and CREATE_DATE<=to_date('" + AsnDiffSearch.CreateDate_Start.Value.ToString() + "','yyyy-mm-dd')";
            }
            if (AsnDiffSearch.CreateDate_End != null && AsnDiffSearch.CreateDate_End.Value != null)
            {
                sqlwhere += " and CREATE_DATE<=to_date('" + AsnDiffSearch.CreateDate_End.Value.ToString() + "','yyyy-mm-dd')";
            }
            if(!string.IsNullOrEmpty(AsnDiffSearch.ResultType))
            {
                sqlwhere += " and CLAIM_RESULT='" + AsnDiffSearch.ResultType + "'";
            }
            if(!string.IsNullOrEmpty(AsnDiffSearch.IsClose) && AsnDiffSearch.IsClose!="-1")
            {
                sqlwhere += " and Is_Close='" + AsnDiffSearch.IsClose + "'";
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
        public void SaveAsnDiff(string warehouse, string asnid,out string SaveMsg)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":asn_wh",OracleDbType.Varchar2),
                new OracleParameter(":asn_manid",OracleDbType.Varchar2),
                new OracleParameter(":asnmsg",OracleDbType.Varchar2)
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


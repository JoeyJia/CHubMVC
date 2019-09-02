using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class RETMAIN_DAL
    {
        private CHubCommonHelper ccHelper;
        public RETMAIN_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_RET_REQ_H> RetMainSearch(string CUSTOMER_NO, string RET_REQ_NO, string REQ_DATE)
        {
            string sql = string.Format(@"select * from V_RET_REQ_H where CUSTOMER_NO='{0}'", CUSTOMER_NO);
            if (!string.IsNullOrEmpty(RET_REQ_NO))
                sql += string.Format(@" and RET_REQ_NO='{0}'", RET_REQ_NO);
            if (!string.IsNullOrEmpty(REQ_DATE))
                sql += string.Format(@" and REQ_DATE>sysdate-{0}", Convert.ToDecimal(REQ_DATE));
            var result = ccHelper.ExecuteSqlToList<V_RET_REQ_H>(sql);
            return result;
        }

        public List<V_RET_REQ_D> GetRetMainDetailModal(string RET_REQ_NO)
        {
            string sql = string.Format(@"select * from  V_RET_REQ_D where ret_req_no ='{0}' order by LINE_NO", RET_REQ_NO);
            var result = ccHelper.ExecuteSqlToList<V_RET_REQ_D>(sql);
            return result;
        }
        public List<V_RET_REQ_D> GetRetMainDetailByRejectReason(string RET_REQ_NO, string REJECT_REASON)
        {
            string sql = string.Format(@"select * from  V_RET_REQ_D where ret_req_no ='{0}'", RET_REQ_NO);
            if (!string.IsNullOrEmpty(REJECT_REASON))
                sql += string.Format(@" and REJECT_REASON='{0}'", REJECT_REASON);
            sql += string.Format(@" order by LINE_NO");
            var result = ccHelper.ExecuteSqlToList<V_RET_REQ_D>(sql);
            return result;
        }

        public List<RET_PART_GROUP> GetPART_GROUP()
        {
            string sql = string.Format(@"select * from RET_PART_GROUP");
            var result = ccHelper.ExecuteSqlToList<RET_PART_GROUP>(sql);
            return result;
        }

        public void DeleteFromRET_REQ_D(string RET_REQ_NO, string LINE_NO)
        {
            string sql = string.Format(@"delete from RET_REQ_D where RET_REQ_NO='{0}' and LINE_NO='{1}'", RET_REQ_NO, LINE_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public void SaveRET_REQ_D(RET_REQ_DArg arg, string RET_REQ_NO)
        {
            string sql = string.Empty;
            OracleParameter[] param = null;
            string existSql = string.Format(@"select * from V_RET_REQ_D where RET_REQ_NO='{0}' and LINE_NO='{1}'", RET_REQ_NO, arg.LINE_NO);
            var result = ccHelper.ExecuteSqlToList<V_RET_REQ_D>(existSql);
            //更新
            if (result != null && result.Count() > 0)
            {
                sql = string.Format(@"update RET_REQ_D set QTY=:QTY,QTY_APPROVED=:QTY_APPROVED,REJECT_REASON=:REJECT_REASON,PART_GROUP=:PART_GROUP,SUPPLIER_CODE=:SUPPLIER_CODE where RET_REQ_NO=:RET_REQ_NO and LINE_NO=:LINE_NO");
                param = new OracleParameter[] {
                    new OracleParameter(":QTY",OracleDbType.Int64),
                    new OracleParameter(":QTY_APPROVED",OracleDbType.Decimal),
                    new OracleParameter(":REJECT_REASON",OracleDbType.Varchar2),
                    new OracleParameter(":PART_GROUP",OracleDbType.Varchar2),
                    new OracleParameter(":SUPPLIER_CODE",OracleDbType.Varchar2),
                    new OracleParameter(":RET_REQ_NO",OracleDbType.Decimal),
                    new OracleParameter(":LINE_NO",OracleDbType.Int64),
                };
                param[0].Value = Convert.ToInt64(arg.QTY);
                param[1].Value = Convert.ToDecimal(arg.QTY_APPROVED);
                param[2].Value = arg.REJECT_REASON;
                param[3].Value = arg.PART_GROUP;
                param[4].Value = arg.SUPPLIER_CODE;
                param[5].Value = Convert.ToDecimal(RET_REQ_NO);
                param[6].Value = Convert.ToInt64(arg.LINE_NO);

            }
            else//新增
            {
                sql = string.Format(@"insert into RET_REQ_D(RET_REQ_NO,LINE_NO,CUST_ITEM,QTY,PART_NO,DESCRIPTION,QTY_APPROVED,REJECT_REASON,PART_GROUP,SUPPLIER_CODE,CREATE_DATE) 
                                values(:RET_REQ_NO,:LINE_NO,:CUST_ITEM,:QTY,:PART_NO,:DESCRIPTION,:QTY_APPROVED,:REJECT_REASON,:PART_GROUP,:SUPPLIER_CODE,:CREATE_DATE)");
                param = new OracleParameter[] {
                    new OracleParameter(":RET_REQ_NO",OracleDbType.Decimal),
                    new OracleParameter(":LINE_NO",OracleDbType.Int64),
                    new OracleParameter(":CUST_ITEM",OracleDbType.Varchar2),
                    new OracleParameter(":QTY",OracleDbType.Int64),
                    new OracleParameter(":PART_NO",OracleDbType.Varchar2),
                    new OracleParameter(":DESCRIPTION",OracleDbType.Varchar2),
                    new OracleParameter(":QTY_APPROVED",OracleDbType.Decimal),
                    new OracleParameter(":REJECT_REASON",OracleDbType.Varchar2),
                    new OracleParameter(":PART_GROUP",OracleDbType.Varchar2),
                    new OracleParameter(":SUPPLIER_CODE",OracleDbType.Varchar2),
                    new OracleParameter(":CREATE_DATE",OracleDbType.Date)
                };
                param[0].Value = Convert.ToDecimal(RET_REQ_NO);
                param[1].Value = Convert.ToInt64(arg.LINE_NO);
                param[2].Value = arg.CUST_ITEM;
                param[3].Value = Convert.ToInt64(arg.QTY);
                param[4].Value = arg.PART_NO;
                param[5].Value = arg.DESCRIPTION;
                param[6].Value = Convert.ToDecimal(arg.QTY_APPROVED);
                param[7].Value = arg.REJECT_REASON;
                param[8].Value = arg.PART_GROUP;
                param[9].Value = arg.SUPPLIER_CODE;
                param[10].Value = DateTime.Now;
            }
            ccHelper.AddOrUpdateWithParams(sql, param);
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return ccHelper.CheckSecurity(SECURE_ID, APP_USER);
        }

        public void UpdateREQ_STATUS(string REQ_STATUS, string RET_REQ_NO)
        {
            string sql = string.Format(@"update RET_REQ_H set REQ_STATUS='{0}' where RET_REQ_NO='{1}'", REQ_STATUS, RET_REQ_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public void ExecP_RET_Verify(string RET_REQ_NO)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("v_req_no",OracleDbType.Decimal)
            };
            param[0].Value = Convert.ToDecimal(RET_REQ_NO);
            param[0].Direction = ParameterDirection.Input;
            ccHelper.ExecProcedureWithParams("P_RET_Verify", param);
        }

        public string CallF_GOMS_PARTNO(string CUST_ITEM)
        {
            string sql = string.Format(@"select F_GOMS_PARTNO('" + CUST_ITEM + "') from dual");
            string result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string CallF_GOMS_desc(string CUST_ITEM)
        {
            string sql = string.Format(@"select F_GOMS_desc('" + CUST_ITEM + "') from dual");
            string result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string CallF_RET_PART_GROUP(string CUST_ITEM)
        {
            string sql = string.Format(@"select F_RET_PART_GROUP('" + CUST_ITEM + "') from dual");
            string result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public string CallF_RET_PART_supp(string CUST_ITEM)
        {
            string sql = string.Format(@"select F_RET_PART_supp('" + CUST_ITEM + "') from dual");
            string result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public string RetMainGetSql(string RET_REQ_NO)
        {
            string sql = string.Format(@"select get_sql('RET_REQ','{0}','','','','') from dual", RET_REQ_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public DataTable RunRetMainSql(string sql)
        {
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public void ChangeRET_REQ_H_Status(string RET_REQ_NO)
        {
            string sql = string.Format(@"Update RET_REQ_H set REQ_STATUS='{0}' where RET_REQ_NO='{1}'", "APPROVED", RET_REQ_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public List<string> GetLOAD_TYPEs(string appUser)
        {
            List<string> str = new List<string>();
            string sql = string.Format(@"select * from IHUB_LOAD_TYPE_AUTH where APP_USER='{0}' and AUTH_BEGIN_DATE <=sysdate and (AUTH_END_DATE>=sysdate or AUTH_END_DATE is null)", appUser);
            var result = ccHelper.ExecuteSqlToList<IHUB_LOAD_TYPE_AUTH>(sql);
            str = result.Select(a => a.LOAD_TYPE).ToList();
            return str;
        }
        public string GetLOAD_DESC(string LOAD_TYPE)
        {
            string str = string.Empty;
            string sql = string.Format(@"select * from IHUB_LOAD_TYPE where LOAD_TYPE='{0}'", LOAD_TYPE);
            var result = ccHelper.ExecuteSqlToList<IHUB_LOAD_TYPE>(sql);
            str = result.Select(a => a.LOAD_DESC).FirstOrDefault();
            return str;
        }

        public List<RET_RETURN_TYPE> GetRETURN_TYPEs()
        {
            string sql = string.Format(@"select * from RET_RETURN_TYPE");
            var result = ccHelper.ExecuteSqlToList<RET_RETURN_TYPE>(sql);
            return result;
        }

        public IHUB_LOAD_TYPE GetIHUB_LOAD_TYPE(string LOAD_TYPE)
        {
            string sql = string.Format(@"select * from IHUB_LOAD_TYPE where LOAD_TYPE='{0}'", LOAD_TYPE);
            var result = ccHelper.ExecuteSqlToList<IHUB_LOAD_TYPE>(sql).First();
            return result;
        }
    }
}

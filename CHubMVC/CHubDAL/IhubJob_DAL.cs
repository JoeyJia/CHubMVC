using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using Oracle.ManagedDataAccess.Client;


namespace CHubDAL
{
    public class IhubJob_DAL
    {
        private CHubCommonHelper ccHelper;
        public IhubJob_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_JOB_LINK_USER> GetJOB_DISPLAY(string App_User)
        {
            string sql = string.Format(@"select * from V_JOB_LINK_USER where APP_USER='{0}'", App_User);
            var result = ccHelper.ExecuteSqlToList<V_JOB_LINK_USER>(sql);
            return result;
        }
        public V_JOB_LINK_USER GetJOB_DESC(string JOB_NAME, string App_User)
        {
            string sql = string.Format(@"select * from V_JOB_LINK_USER where JOB_NAME='{0}' and APP_USER='{1}'", JOB_NAME, App_User);
            var result = ccHelper.ExecuteSqlToList<V_JOB_LINK_USER>(sql).FirstOrDefault();
            return result;
        }

        public void ExecuteProc(V_JOB_LINK_USER item, List<string> paras)
        {
            if (paras != null && paras.Any())
            {
                List<OracleParameter> pp = new List<OracleParameter>();
                #region P1
                if (!string.IsNullOrEmpty(item.JOB_P1))
                {
                    var p1 = new OracleParameter();
                    p1.ParameterName = item.JOB_P1_COMMENTS;
                    if (item.JOB_P1_TYPE == "CHAR")
                    {
                        p1.Value = paras[0];
                        p1.OracleDbType = OracleDbType.Varchar2;
                    }

                    else if (item.JOB_P1_TYPE == "NUMBER")
                    {
                        p1.Value = Convert.ToDecimal(paras[0]);
                        p1.OracleDbType = OracleDbType.Decimal;
                    }

                    else
                    {
                        p1.Value = Convert.ToDateTime(paras[0]);
                        p1.OracleDbType = OracleDbType.Date;
                    }
                    pp.Add(p1);
                }
                #endregion
                #region P2
                if (!string.IsNullOrEmpty(item.JOB_P2))
                {
                    var p2 = new OracleParameter();
                    p2.ParameterName = item.JOB_P2_COMMENTS;
                    if (item.JOB_P2_TYPE == "CHAR")
                    {
                        p2.Value = paras[1];
                        p2.OracleDbType = OracleDbType.Varchar2;
                    }

                    else if (item.JOB_P2_TYPE == "NUMBER")
                    {
                        p2.Value = Convert.ToDecimal(paras[1]);
                        p2.OracleDbType = OracleDbType.Decimal;
                    }

                    else
                    {
                        p2.Value = Convert.ToDateTime(paras[1]);
                        p2.OracleDbType = OracleDbType.Date;
                    }
                    pp.Add(p2);
                }
                #endregion
                #region P3
                if (!string.IsNullOrEmpty(item.JOB_P3))
                {
                    var p3 = new OracleParameter();
                    p3.ParameterName = item.JOB_P3_COMMENTS;
                    if (item.JOB_P3_TYPE == "CHAR")
                    {
                        p3.Value = paras[2];
                        p3.OracleDbType = OracleDbType.Varchar2;
                    }

                    else if (item.JOB_P3_TYPE == "NUMBER")
                    {
                        p3.Value = Convert.ToDecimal(paras[2]);
                        p3.OracleDbType = OracleDbType.Decimal;
                    }

                    else
                    {
                        p3.Value = Convert.ToDateTime(paras[2]);
                        p3.OracleDbType = OracleDbType.Date;
                    }
                    pp.Add(p3);
                }
                #endregion
                #region P4
                if (!string.IsNullOrEmpty(item.JOB_P4))
                {
                    var p4 = new OracleParameter();
                    p4.ParameterName = item.JOB_P4_COMMENTS;
                    if (item.JOB_P4_TYPE == "CHAR")
                    {
                        p4.Value = paras[3];
                        p4.OracleDbType = OracleDbType.Varchar2;
                    }

                    else if (item.JOB_P4_TYPE == "NUMBER")
                    {
                        p4.Value = Convert.ToDecimal(paras[3]);
                        p4.OracleDbType = OracleDbType.Decimal;
                    }

                    else
                    {
                        p4.Value = Convert.ToDateTime(paras[3]);
                        p4.OracleDbType = OracleDbType.Date;
                    }
                    pp.Add(p4);
                }
                #endregion
                #region P5
                if (!string.IsNullOrEmpty(item.JOB_P5))
                {
                    var p5 = new OracleParameter();
                    p5.ParameterName = item.JOB_P5_COMMENTS;
                    if (item.JOB_P5_TYPE == "CHAR")
                    {
                        p5.Value = paras[4];
                        p5.OracleDbType = OracleDbType.Varchar2;
                    }

                    else if (item.JOB_P5_TYPE == "NUMBER")
                    {
                        p5.Value = Convert.ToDecimal(paras[4]);
                        p5.OracleDbType = OracleDbType.Decimal;
                    }

                    else
                    {
                        p5.Value = Convert.ToDateTime(paras[4]);
                        p5.OracleDbType = OracleDbType.Date;
                    }
                    pp.Add(p5);
                }
                #endregion

                OracleParameter[] param = pp.ToArray();
                ccHelper.ExecProcedureWithParams(item.JOB_PROC, param);
            }
            else
                ccHelper.ExecProcedureWithoutParams(item.JOB_PROC);
        }

        public void LogHistory(V_JOB_LINK_USER item, string msg)
        {
            string sql = string.Format(@"insert into JOB_HISTORY(JOB_NAME,APP_USER,RUN_DATE,JOB_MSG)
                                            values('{0}','{1}',sysdate,'{2}')", item.JOB_NAME, item.APP_USER, msg);
            ccHelper.ExecuteSql(sql);
        }
    }
}

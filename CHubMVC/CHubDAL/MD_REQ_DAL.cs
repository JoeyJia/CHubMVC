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
    public class MD_REQ_DAL
    {
        private CHubCommonHelper cchelper;
        public MD_REQ_DAL()
        {
            cchelper = new CHubCommonHelper();
        }

        public string GetPART_NO(string partno)
        {
            string sql = GetSql("F_MD_PART_AUTO_FMT", partno); //string.Format(@"select F_MD_PART_AUTO_FMT('{0}') from dual", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetPART_DESC(string partno)
        {
            string sql = GetSql("GET_MD_DESC", partno); //string.Format(@"select GET_MD_DESC('{0}') from dual", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetCHECK_EXIST(string partno)
        {
            string sql = GetSql("F_MD_EXIST_CHK", partno); //string.Format(@"select F_MD_EXIST_CHK('{0}') from dual", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetGLOBAL_PARTNO(string partno)
        {
            string sql = GetSql("F_MD_GET_GLOBAL_PRT", partno); //string.Format(@"select F_MD_GET_GLOBAL_PRT('{0}') from dual")
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetGLOBAL_PARTDESC(string GLOBAL_PARTNO)
        {
            string sql = GetSql("F_MD_GET_GLOBAL_DESC", GLOBAL_PARTNO);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetSHORT_DESC(string GLOBAL_PARTNO)
        {
            string sql = GetSql("F_MD_GET_GLOBAL_DESC_SHORT", GLOBAL_PARTNO);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetCHECK_PRI_SUP(string partno)
        {
            string sql = GetSql("F_MD_PRI_SUP_CHK", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetCHECK_PRI_PB(string partno)
        {
            string sql = GetSql("F_MD_PRI_PB_CHK", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetCHECK_PRI_BPA(string partno)
        {
            string sql = GetSql("F_MD_BPA_CHK", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetCHECK_COST(string partno)
        {
            string sql = GetSql("F_cost_readiness", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public string GetPRODUCT_GROUP_ID(string partno)
        {
            string sql = GetSql("F_MD_GET_PROD_GROUP", partno);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }


        public string GetRequestNo()
        {
            string sql = "select S_MD_REQ_NO.NEXTVAL from dual";
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }


        public void SaveMD_REQ_HEADER(MD_REQ_HEADER mrHeader)
        {
            string sql = string.Format(@"insert into MD_REQ_HEADER(MD_REQ_NO,REQ_DESC,REQ_BY) values('{0}','{1}','{2}')", mrHeader.MD_REQ_NO, mrHeader.REQ_DESC, mrHeader.REQ_BY);
            cchelper.ExecuteNonQuery(sql);
        }

        public void SaveMD_REQ_DETAIL(MD_REQ_DETAIL mrDetail)
        {
            string sql = string.Format(@"insert into MD_REQ_DETAIL(
                                            MD_REQ_NO,REQ_LINE_NO,PART_NO,PART_DESC,CHECK_EXIST,CHECK_PRI_SUP,CHECK_PRI_PB,CHECK_PRI_BPA,CHECK_COST,GLOBAL_PARTNO,PRODUCT_GROUP_ID,NOTE,PART_DESC_SHORT,GLOBAL_DESC)
                                            values(
                                            :MD_REQ_NO,
                                            :REQ_LINE_NO,
                                            :PART_NO,
                                            :PART_DESC,
                                            :CHECK_EXIST,
                                            :CHECK_PRI_SUP,
                                            :CHECK_PRI_PB,
                                            :CHECK_PRI_BPA,
                                            :CHECK_COST,
                                            :GLOBAL_PARTNO,
                                            :PRODUCT_GROUP_ID,
                                            :NOTE,
                                            :PART_DESC_SHORT,
                                            :GLOBAL_DESC
                                            )");
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":MD_REQ_NO",OracleDbType.Decimal),
                new OracleParameter(":REQ_LINE_NO",OracleDbType.Decimal),
                new OracleParameter(":PART_NO",OracleDbType.Varchar2),
                new OracleParameter(":PART_DESC",OracleDbType.Varchar2),
                new OracleParameter(":CHECK_EXIST",OracleDbType.Varchar2),
                new OracleParameter(":CHECK_PRI_SUP",OracleDbType.Varchar2),
                new OracleParameter(":CHECK_PRI_PB",OracleDbType.Varchar2),
                new OracleParameter(":CHECK_PRI_BPA",OracleDbType.Varchar2),
                new OracleParameter(":CHECK_COST",OracleDbType.Varchar2),
                new OracleParameter(":GLOBAL_PARTNO",OracleDbType.Varchar2),
                new OracleParameter(":PRODUCT_GROUP_ID",OracleDbType.Varchar2),
                new OracleParameter(":NOTE",OracleDbType.Varchar2),
                new OracleParameter(":PART_DESC_SHORT",OracleDbType.Varchar2),
                new OracleParameter(":GLOBAL_DESC",OracleDbType.Varchar2),
            };
            param[0].Value = mrDetail.MD_REQ_NO;
            param[1].Value = mrDetail.REQ_LINE_NO;
            param[2].Value = mrDetail.PART_NO;
            param[3].Value = mrDetail.PART_DESC;
            param[4].Value = mrDetail.CHECK_EXIST;
            param[5].Value = mrDetail.CHECK_PRI_SUP;
            param[6].Value = mrDetail.CHECK_PRI_PB;
            param[7].Value = mrDetail.CHECK_PRI_BPA;
            param[8].Value = mrDetail.CHECK_COST;
            param[9].Value = mrDetail.GLOBAL_PARTNO;
            param[10].Value = mrDetail.PRODUCT_GROUP_ID;
            param[11].Value = mrDetail.NOTE;
            param[12].Value = mrDetail.PART_DESC_SHORT;
            param[13].Value = mrDetail.GLOBAL_DESC;
            cchelper.AddOrUpdateWithParams(sql, param);           
        }


        public static string GetSql(string func, string param)
        {
            string str = "select " + func + "('" + param + "') from dual";
            return str;
        }

        public void ExecP_MD_SR_NEW(string PART_NO, string PRODUCT_GROUP_ID, string NOTE, string REQ_BY)
        {
            cchelper.ExecP_MD_SR_NEW(PART_NO, PRODUCT_GROUP_ID, NOTE, REQ_BY);
        }

    }
}

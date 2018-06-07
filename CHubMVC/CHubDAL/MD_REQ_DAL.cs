using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

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
            cchelper.Update(sql);
        }

        public void SaveMD_REQ_DETAIL(MD_REQ_DETAIL mrDetail)
        {
            string sql = string.Format(@"insert into MD_REQ_DETAIL(MD_REQ_NO,REQ_LINE_NO,PART_NO,PART_DESC,
                                    CHECK_EXIST,CHECK_PRI_SUP,CHECK_PRI_PB,CHECK_PRI_BPA,CHECK_COST,
                                    GLOBAL_PARTNO,PRODUCT_GROUP_ID)
                                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                    mrDetail.MD_REQ_NO, mrDetail.REQ_LINE_NO, mrDetail.PART_NO, mrDetail.PART_DESC,
                                    mrDetail.CHECK_EXIST, mrDetail.CHECK_PRI_SUP, mrDetail.CHECK_PRI_PB, mrDetail.CHECK_PRI_BPA, mrDetail.CHECK_COST,
                                    mrDetail.GLOBAL_PARTNO, mrDetail.PRODUCT_GROUP_ID);
            cchelper.Update(sql);
        }


        public static string GetSql(string func, string param)
        {
            string str = "select " + func + "('" + param + "') from dual";
            return str;
        }

        public void ExecP_MD_SR_NEW(string PART_NO, string PRODUCT_GROUP_ID)
        {
            cchelper.ExecP_MD_SR_NEW(PART_NO, PRODUCT_GROUP_ID);
        }

    }
}

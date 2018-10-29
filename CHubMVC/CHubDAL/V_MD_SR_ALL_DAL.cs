using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class V_MD_SR_ALL_DAL
    {
        private CHubCommonHelper ccHelper;
        public V_MD_SR_ALL_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<MD_SR_STATUS> GetSR_STATUS()
        {
            string sql = string.Format(@"select SR_STATUS,SR_STATUS_DESC from MD_SR_STATUS");
            var result = ccHelper.Search<MD_SR_STATUS>(sql);
            return result;
        }

        public MD_SR_STATUS GetSR_STATUS_DESC(string SR_STATUS)
        {
            string sql = string.Format(@"select SR_STATUS,SR_STATUS_DESC from MD_SR_STATUS where SR_STATUS='{0}'", SR_STATUS);
            var result = ccHelper.Search<MD_SR_STATUS>(sql).FirstOrDefault();
            return result;
        }

        public List<V_MD_SR_ALL> MDSRSearch(string PART_NO, string COMPANY_CODE, string SR_STATUS, string REQ_DATE,string IS_COMMON)
        {
            string sql = string.Format(@"select * from V_MD_SR_ALL where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO like '%{0}%'", PART_NO);
            if (!string.IsNullOrEmpty(COMPANY_CODE))
                sql += string.Format(@" and COMPANY_CODE like '%{0}%'", COMPANY_CODE);
            if (!string.IsNullOrEmpty(SR_STATUS))
                sql += string.Format(@" and SR_STATUS ='{0}'", SR_STATUS);
            if (!string.IsNullOrEmpty(REQ_DATE))
                sql += string.Format(@" and REQ_DATE >= sysdate-{0}", Convert.ToInt32(REQ_DATE));
            if (!string.IsNullOrEmpty(IS_COMMON))
                sql += string.Format(@" and IS_COMMON ='{0}'", IS_COMMON);

            var result = ccHelper.Search<V_MD_SR_ALL>(sql);
            return result;
        }

        public void MDSRSave(MDReqSRArg arg)
        {
            string sql = string.Format(@"Update MD_REQ_SR Set COMPANY_CODE='{0}',SUPPLIER_PARTNO='{1}',PRICE='{2}',MOQ='{3}',LOT_SIZE='{4}',LT='{5}',IS_COMMON='{6}',SR_COMMENTS='{7}' where SR_REQ_NO='{8}'", arg.COMPANY_CODE, arg.Supplier_PARTNO, arg.PRICE, arg.MOQ, arg.LOT_SIZE, arg.LT, arg.IS_COMMON, arg.SR_COMMENTS, arg.SR_REQ_NO);
            ccHelper.Update(sql);
        }

        public bool IsOperate(string SECURE_ID, string UserName)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='{2}'", SECURE_ID, UserName, "Y");
            var result = ccHelper.Search<APP_SECURE_PROC_ASSIGN>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public void MDSRChangeStatus(List<string> SR_REQ_NO, string SR_STATUS, string SR_COMMENTS)
        {
            string sql = string.Format(@"Update MD_REQ_SR set SR_STATUS='{0}',SR_COMMENTS='{1}' where SR_REQ_NO in ({2})", SR_STATUS, SR_COMMENTS, ValueConvert.ToSqlInStr(SR_REQ_NO));
            ccHelper.Update(sql);
        }

        public DataTable MDSRDownloadTemp()
        {
            string sql = "select * from V_MD_SR_template";
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

        public string GetSR_LOAD_SEQ()
        {
            string sql = string.Format(@"select SEQ_SR_LOAD.NEXTVAL from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public void InsertMD_SR_LOAD(string SR_LOAD_SEQ, MDSRLOADArg arg, string appUser)
        {
            //string sql = string.Format(@"insert into MD_SR_LOAD(SR_LOAD_SEQ,PART_NO,COMPANY_CODE,PRICE,MOQ,LT,IS_COMMON,COMMENTS,LOAD_DATE,LOADED_BY) 
            //                        values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}')", SR_LOAD_SEQ, arg.PART_NO, arg.COMPANY_CODE, arg.PRICE,
            //                        arg.MOQ, arg.LT, arg.IS_COMMON, arg.COMMENTS, "sysdate", appUser);

            string sql = string.Format(@"Insert into MD_SR_LOAD(SR_LOAD_SEQ,SR_REQ_NO,PART_NO,COMPANY_CODE,SUPPLIER_PARTNO,PRICE,MOQ,LOT_SIZE,LT,IS_COMMON,COMMENTS,NOTE,LOAD_DATE,LOADED_BY)      
                                        Values(:SR_LOAD_SEQ,
                                               :SR_REQ_NO,
                                               :PART_NO,
                                               :COMPANY_CODE,
                                               :SUPPLIER_PARTNO,
                                               :PRICE,
                                               :MOQ,
                                               :LOT_SIZE,
                                               :LT,
                                               :IS_COMMON,
                                               :COMMENTS,
                                               :NOTE,
                                               :LOAD_DATE,
                                               :LOADED_BY)");
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":SR_LOAD_SEQ",OracleDbType.Decimal,ParameterDirection.Input),
                new OracleParameter(":SR_REQ_NO",OracleDbType.Decimal,ParameterDirection.Input),
                new OracleParameter(":PART_NO",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter(":COMPANY_CODE",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter(":SUPPLIER_PARTNO",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter(":PRICE",OracleDbType.Decimal,ParameterDirection.Input),
                new OracleParameter(":MOQ",OracleDbType.Decimal,ParameterDirection.Input),
                new OracleParameter(":LOT_SIZE",OracleDbType.Decimal,ParameterDirection.Input),
                new OracleParameter(":LT",OracleDbType.Decimal,ParameterDirection.Input),
                new OracleParameter(":IS_COMMON",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter(":COMMENTS",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter(":NOTE",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter(":LOAD_DATE",OracleDbType.Date,ParameterDirection.Input),
                new OracleParameter(":LOADED_BY",OracleDbType.Varchar2,ParameterDirection.Input),
            };
            param[0].Value = decimal.Parse(SR_LOAD_SEQ);
            param[1].Value = decimal.Parse(arg.SR_REQ_NO);
            param[2].Value = arg.PART_NO;
            param[3].Value = arg.COMPANY_CODE;
            param[4].Value = arg.SUPPLIER_PARTNO;
            param[5].Value = arg.PRICE;
            param[6].Value = arg.MOQ;
            param[7].Value = arg.LOT_SIZE;
            param[8].Value = arg.LT;
            param[9].Value = arg.IS_COMMON;
            param[10].Value = arg.COMMENTS;
            param[11].Value = arg.NOTE;
            param[12].Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            param[13].Value = appUser;

            ccHelper.AddOrUpdateWithParams(sql, param);            
        }

        public MD_COMPANY_SNAP CheckCOMPANY_CODE(string COMPANY_CODE)
        {
            string sql = string.Format(@"select * from MD_COMPANY_SNAP where COMPANY_CODE='{0}'", COMPANY_CODE);
            var result = ccHelper.Search<MD_COMPANY_SNAP>(sql).FirstOrDefault();
            return result;
        }

        public void RunP_MD_SR_UPD_Status()
        {
            ccHelper.ExecProcedureWithoutParams("P_MD_SR_UPD_Status");
        }

        public string CallGET_SQL()
        {
            string sql = string.Format(@"select GET_SQL('MD_SR_TEMPLATE','','','','','') from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public DataTable RunSql(string sql)
        {
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

    }
}

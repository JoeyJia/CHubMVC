using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using System.Data;

namespace CHubDAL
{
    public class FAR_DAL
    {
        private CHubCommonHelper ccHelper;
        public FAR_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_FAR_HEADER> MyFarSearch(FAR_Arg arg)
        {
            string sql = string.Format(@"select * from V_FAR_HEADER where 1=1");
            if (!string.IsNullOrEmpty(arg.FAR_NO))
                sql += string.Format(@" and FAR_NO='{0}'", arg.FAR_NO);
            if (!string.IsNullOrEmpty(arg.PERIOD))
                sql += string.Format(@" and PERIOD='{0}'", arg.PERIOD);
            if (!string.IsNullOrEmpty(arg.CUSTOMER_NO))
                sql += string.Format(@" and CUSTOMER_NO like '%{0}%'", arg.CUSTOMER_NO);
            sql += string.Format(@" and REQ_BY='{0}'", arg.APP_USER);
            var result = ccHelper.ExecuteSqlToList<V_FAR_HEADER>(sql);
            return result;
        }

        public DataTable GetFAR_STATUS()
        {
            string sql = string.Format(@"select * from FAR_STATUS where ACTIVEIND='Y'");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public DataTable GetCUSTOMER_NO()
        {
            string sql = string.Format(@"select * from APP_OECUSTOMER_MST where FAR_FLAG='Y'");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public DataTable GetADJ_TYPE()
        {
            string sql = string.Format(@"select * from FAR_ADJ_TYPE where ACTIVEIND='Y'");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public DataTable GetPRIORITY_CODE()
        {
            string sql = string.Format(@"select * from FAR_PRIORITY where ACTIVEIND='Y'");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public DataTable GetINV_STRATEGY_CODE()
        {
            string sql = string.Format(@"select * from FAR_INV_STRATEGY where ACTIVEIND='Y'");
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }

        public bool IsExistMyFar(FAR_HEADER far)
        {
            string sql = string.Format(@"select * from FAR_HEADER where PERIOD='{0}' and CUSTOMER_NO='{1}' and lower(FAR_PROJECT)=lower('{2}')", far.PERIOD, far.CUSTOMER_NO, far.FAR_PROJECT);
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void MyFarUpdate(FAR_HEADER far)
        {
            string sql = string.Format(@"update FAR_HEADER set 
                                            FAR_STATUS='{0}',
                                            PERIOD={1},
                                            CUSTOMER_NO='{2}',
                                            FAR_PROJECT='{3}',
                                            ADJ_TYPE='{4}',
                                            PRIORITY_CODE='{5}',
                                            FAR_DESC='{6}',
                                            RECURRING='{7}',
                                            SUBSTITUTE='{8}',
                                            INV_STRATEGY_CODE='{9}',
                                            MITIGATION_PLAN='{10}',
                                            RECORD_DATE=sysdate where FAR_NO={11}",
                                            far.FAR_STATUS, far.PERIOD, far.CUSTOMER_NO, far.FAR_PROJECT, far.ADJ_TYPE,
                                            far.PRIORITY_CODE, far.FAR_DESC, far.RECURRING, far.SUBSTITUTE, far.INV_STRATEGY_CODE,
                                            far.MITIGATION_PLAN, far.FAR_NO);
            ccHelper.ExecuteNonQuery(sql);
        }

        public void MyFarAdd(FAR_HEADER far)
        {
            string sql = string.Format(@"insert into FAR_HEADER(
                                            FAR_NO,
                                            FAR_STATUS,
                                            PERIOD,
                                            CUSTOMER_NO,
                                            FAR_PROJECT,
                                            ADJ_TYPE,
                                            PRIORITY_CODE,
                                            FAR_DESC,
                                            RECURRING,
                                            SUBSTITUTE,
                                            INV_STRATEGY_CODE,
                                            MITIGATION_PLAN,
                                            REQ_BY) values ({0},'{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                                            far.FAR_NO, far.FAR_STATUS, far.PERIOD, far.CUSTOMER_NO, far.FAR_PROJECT,
                                            far.ADJ_TYPE, far.PRIORITY_CODE, far.FAR_DESC, far.RECURRING, far.SUBSTITUTE,
                                            far.INV_STRATEGY_CODE, far.MITIGATION_PLAN, far.REQ_BY);
            ccHelper.ExecuteNonQuery(sql);

        }

        public string GetFar_No()
        {
            string sql = string.Format(@"select FAR_NO.nextval from dual");
            string result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public V_FAR_HEADER GetMyFarHeader(string FAR_NO)
        {
            string sql = string.Format(@"select * from V_FAR_HEADER where FAR_NO='{0}'", FAR_NO);
            var result = ccHelper.ExecuteSqlToList<V_FAR_HEADER>(sql).First();
            return result;
        }
        public List<V_FAR_DETAIL> GetMyFarDetail(string FAR_NO)
        {
            string sql = string.Format(@"select * from V_FAR_DETAIL where FAR_NO='{0}'", FAR_NO);
            var result = ccHelper.ExecuteSqlToList<V_FAR_DETAIL>(sql);
            return result;
        }
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return ccHelper.CheckSecurity(SECURE_ID, APP_USER);
        }
        public void UpdateFarStatus(string FAR_NO, string FAR_STATUS)
        {
            string sql = string.Format(@"Update FAR_HEADER set FAR_STATUS='{0}' where FAR_NO='{1}'", FAR_STATUS, FAR_NO);
            ccHelper.ExecuteNonQuery(sql);
        }
        public string ExportFar(string FAR_NO)
        {
            string sql = string.Format(@"select get_SQL ('FAR_LOAD',{0},'','','','') from dual", Convert.ToDecimal(FAR_NO));
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }

        public DataTable RunSql(string sql)
        {
            DataTable dt = ccHelper.ExecuteSqlToDataTable(sql);
            return dt;
        }
        public string GetFAR_LOAD()
        {
            string sql = string.Format(@"select FAR_LOAD.nextval from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public void InsertFAR_DETAIL_STG(FAR_DETAIL_STG fd, string LOAD_SEQ)
        {
            string sql = string.Format(@"insert into FAR_DETAIL_STG(
                                            FAR_NO,
                                            LOAD_SEQ,
                                            CUST_PARTNO,
                                            LOCATION_CODE,
                                            M01,
                                            M02,
                                            M03,
                                            M04,
                                            M05,
                                            M06,
                                            M07,
                                            M08,
                                            M09,
                                            M10,
                                            M11,
                                            M12,
                                            NOTE,
                                            CREATE_DATE)
                                            values({0},'{1}','{2}','{3}',{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},'{16}',sysdate)",
                                            fd.FAR_NO, LOAD_SEQ, fd.CUST_PARTNO, fd.LOCATION_CODE, 
                                            fd.M01, fd.M02, fd.M03, fd.M04, fd.M05, fd.M06, fd.M07, fd.M08,
                                            fd.M09, fd.M10, fd.M11, fd.M12, fd.NOTE);
            ccHelper.ExecuteNonQuery(sql);
        }

        public string CheckCUST_PARTNO(string CUST_PARTNO)
        {
            string sql = string.Format(@"select F_GOMS_PARTNO('{0}') from dual", CUST_PARTNO);
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }
        public void DeleteFarDetail(string FAR_NO, string LOAD_SEQ, string CUST_PARTNO)
        {
            string sql = string.Format(@"delete FAR_DETAIL where FAR_NO='{0}' 
                                                and LOAD_SEQ='{1}' 
                                                and CUST_PARTNO='{2}'", FAR_NO, LOAD_SEQ, CUST_PARTNO);
            ccHelper.ExecuteNonQuery(sql);
        }
    }
}

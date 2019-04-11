using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class MDSUPP_DAL
    {
        private CHubCommonHelper ccHelper;
        public MDSUPP_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<MD_COMPANY_SNAP> MDSuppSearch(string COMPANY_CODE)
        {
            string sql = string.Format(@"select * from V_MD_COMPANY_SNAP where 1=1");
            if (!string.IsNullOrEmpty(COMPANY_CODE))
                sql += string.Format(@" and COMPANY_CODE like '%{0}%' ", COMPANY_CODE);
            var result = ccHelper.Search<MD_COMPANY_SNAP>(sql);
            return result;
        }

        public void MDSuppSave(MD_COMPANY_SNAP item)
        {
            string sql = string.Format(@"Update MD_COMPANY_SNAP set 
                                        COMPANY_NAME_CN=:COMPANY_NAME_CN,
                                        PLANNER=:PLANNER,
                                        PLANNER_CODE=:PLANNER_CODE,
                                        NOTE=:NOTE,
                                        GSM_SUPPLIER_NO=:GSM_SUPPLIER_NO,
                                        VENDOR_SITE_ID=:VENDOR_SITE_ID,
                                        BPA_NO=:BPA_NO,
                                        INSURANCE_CODE=:INSURANCE_CODE,
                                        DS_TRACK=:DS_TRACK,
                                        DS_TRACK_EML=:DS_TRACK_EML,
                                        COMPANY_NAME_SHORT=:COMPANY_NAME_SHORT,
                                        RETURN_ALLOW_DAYS=:RETURN_ALLOW_DAYS
                                        where COMPANY_CODE=:COMPANY_CODE
                                        ");
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter(":COMPANY_NAME_CN",OracleDbType.Varchar2,item.COMPANY_NAME_CN,ParameterDirection.Input),
                new OracleParameter(":PLANNER",OracleDbType.Varchar2,item.PLANNER,ParameterDirection.Input),
                new OracleParameter(":PLANNER_CODE",OracleDbType.Varchar2,item.PLANNER_CODE,ParameterDirection.Input),
                new OracleParameter(":NOTE",OracleDbType.Varchar2,item.NOTE,ParameterDirection.Input),
                new OracleParameter(":GSM_SUPPLIER_NO",OracleDbType.Varchar2,item.GSM_SUPPLIER_NO,ParameterDirection.Input),
                new OracleParameter(":VENDOR_SITE_ID",OracleDbType.Varchar2,item.VENDOR_SITE_ID,ParameterDirection.Input),
                new OracleParameter(":BPA_NO",OracleDbType.Varchar2,item.BPA_NO,ParameterDirection.Input),
                new OracleParameter(":INSURANCE_CODE",OracleDbType.Varchar2,item.INSURANCE_CODE,ParameterDirection.Input),
                new OracleParameter(":DS_TRACK",OracleDbType.Varchar2,item.DS_TRACK,ParameterDirection.Input),
                new OracleParameter(":DS_TRACK_EML",OracleDbType.Varchar2,item.DS_TRACK_EML,ParameterDirection.Input),
                new OracleParameter(":COMPANY_NAME_SHORT",OracleDbType.Varchar2,item.COMPANY_NAME_SHORT,ParameterDirection.Input),
                new OracleParameter(":RETURN_ALLOW_DAYS",OracleDbType.Decimal,item.RETURN_ALLOW_DAYS,ParameterDirection.Input),
                new OracleParameter(":COMPANY_CODE",OracleDbType.Varchar2,item.COMPANY_CODE,ParameterDirection.Input)
            };
            ccHelper.AddOrUpdateWithParams(sql, param);
        }

        public List<MD_INSURANCE_CODES> GetINSURANCE_CODE()
        {
            string sql = string.Format(@"select * from MD_INSURANCE_CODES");
            var result = ccHelper.Search<MD_INSURANCE_CODES>(sql);
            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class CINVINQ_DAL
    {
        private CHubCommonHelper ccHelper;
        public CINVINQ_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<EXP_COMM_INV> SearchEXP_COMM_INV(string COMM_INV_ID, string SHIP_TO_INDEX, string CREATE_DATE, string CREATED_BY)
        {
            string sql = string.Format(@"select * from EXP_COMM_INV where 1=1");
            if (!string.IsNullOrEmpty(COMM_INV_ID))
                sql += string.Format(@" and COMM_INV_ID='{0}'", COMM_INV_ID);
            if (!string.IsNullOrEmpty(SHIP_TO_INDEX))
                sql += string.Format(@" and SHIP_TO_INDEX like '%{0}%'", SHIP_TO_INDEX);
            if (!string.IsNullOrEmpty(CREATE_DATE))
                sql += string.Format(@" and CREATE_DATE>sysdate-{0}", Convert.ToInt32(CREATE_DATE));
            if (!string.IsNullOrEmpty(CREATED_BY))
                sql += string.Format(@" and CREATED_BY='{0}'", CREATED_BY);
            var result = ccHelper.Search<EXP_COMM_INV>(sql);
            return result;
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='Y'", SECURE_ID, APP_USER);
            var result = ccHelper.Search<APP_SECURE_PROC_ASSIGN>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public void ExecP_EXP_INV_DISCARD(string COMM_INV_ID)
        {
            ccHelper.ExecP_EXP_INV_DISCARD(COMM_INV_ID);
        }

        public void ExecP_EXP_INV_COMP(string COMM_INV_ID)
        {
            ccHelper.ExecP_EXP_INV_COMP(COMM_INV_ID);
        }

        public List<V_EXP_STAGE_BASE> SearchDetailsByV_EXP_STAGE_BASE(string COMM_INV_ID)
        {
            string sql = string.Format(@"select * from V_EXP_STAGE_BASE where COMM_INV_ID='{0}'", COMM_INV_ID);
            var result = ccHelper.Search<V_EXP_STAGE_BASE>(sql);
            return result;
        }
    }
}

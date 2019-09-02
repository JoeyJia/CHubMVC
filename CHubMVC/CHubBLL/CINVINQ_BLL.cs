using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;
using System.Data;

namespace CHubBLL
{
    public class CINVINQ_BLL
    {
        private CINVINQ_DAL dal;
        public CINVINQ_BLL()
        {
            dal = new CINVINQ_DAL();
        }

        public List<EXP_COMM_INV> SearchEXP_COMM_INV(string COMM_INV_ID, string SHIP_TO_INDEX, string CREATE_DATE, string CREATED_BY)
        {
            return dal.SearchEXP_COMM_INV(COMM_INV_ID, SHIP_TO_INDEX, CREATE_DATE, CREATED_BY);
        }

        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckSecurity(SECURE_ID, APP_USER);
        }
        public void ExecP_EXP_INV_DISCARD(string COMM_INV_ID)
        {
            dal.ExecP_EXP_INV_DISCARD(COMM_INV_ID);
        }
        public void ExecP_EXP_INV_COMP(string COMM_INV_ID)
        {
            dal.ExecP_EXP_INV_COMP(COMM_INV_ID);
        }

        public List<V_EXP_STAGE_BASE> SearchDetailsByV_EXP_STAGE_BASE(string COMM_INV_ID)
        {
            return dal.SearchDetailsByV_EXP_STAGE_BASE(COMM_INV_ID);
        }

        public void ExecP_EXP_UPT_HSCODE(string COMM_INV_ID)
        {
            dal.ExecP_EXP_UPT_HSCODE(COMM_INV_ID);
        }

        public string CallFuncF_EXP_HSCODE_CHK(string COMM_INV_ID)
        {
            return dal.CallFuncF_EXP_HSCODE_CHK(COMM_INV_ID);
        }

        public string CallFunc_GET_SQL(string COMM_INV_ID)
        {
            return dal.CallFunc_GET_SQL(COMM_INV_ID);
        }
        public DataTable RunSql(string sql)
        {
            return dal.RunSql(sql);
        }
        public void SureToReDiscard(string COMM_INV_ID)
        {
            dal.SureToReDiscard(COMM_INV_ID);
        }
    }
}

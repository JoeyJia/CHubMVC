using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;

namespace CHubBLL
{
    public class EXPWB_BLL
    {
        private EXPWB_DAL dal;
        public EXPWB_BLL()
        {
            dal = new EXPWB_DAL();
        }

        public List<EXP_SHIP_TO> GetSHIP_TO_LOCATION()
        {
            return dal.GetSHIP_TO_LOCATION();
        }

        public List<EXP_SHIP_TO> GetSHIP_TO_ALIAS(string SHIP_TO_LOCATION)
        {
            return dal.GetSHIP_TO_ALIAS(SHIP_TO_LOCATION);
        }

        public void ExecP_Load_Stg_from_RP(string SHIP_TO_LOCATION)
        {
            dal.ExecP_Load_Stg_from_RP(SHIP_TO_LOCATION);
        }

        public List<V_EXP_STAGE_UNINVOICED> GetSHIP_TO_INDEX()
        {
            return dal.GetSHIP_TO_INDEX();
        }

        public List<V_EXP_STAGE_UNINVOICED> SearchV_EXP_STAGE_UNINVOICED(string SHIP_TO_INDEX)
        {
            return dal.SearchV_EXP_STAGE_UNINVOICED(SHIP_TO_INDEX);
        }

        public List<V_EXP_STAGE_UNINVOICED> ChangeByORDTYP(string SHIP_TO_INDEX, string ORDTYP)
        {
            return dal.ChangeByORDTYP(SHIP_TO_INDEX, ORDTYP);
        }

        public string CallFunc_GET_EXP_EST_AMT(string LODNUM)
        {
            return dal.CallFunc_GET_EXP_EST_AMT(LODNUM);
        }

        public List<EXP_STG_D> SearchDetailsByEXP_STG_D(string LODNUM)
        {
            return dal.SearchDetailsByEXP_STG_D(LODNUM);
        }

        public bool CheckSecurityOfbtnCreateInv(string SECURE_ID, string appUser)
        {
            return dal.CheckSecurityOfbtnCreateInv(SECURE_ID, appUser);
        }

        public string GetCOMM_INV_ID()
        {
            return dal.GetCOMM_INV_ID();
        }

        public string CallFunc_GET_EXP_EXCHANGE_RATE()
        {
            return dal.CallFunc_GET_EXP_EXCHANGE_RATE();
        }

        public void AddEXP_COMM_INV(EXPCOMMINVArg arg)
        {
            dal.AddEXP_COMM_INV(arg);
        }

        public void ChangeEXP_STG_H(string COMM_INV_ID, string LODNUM)
        {
            dal.ChangeEXP_STG_H(COMM_INV_ID, LODNUM);
        }

        public string GetSEQ_STG_LOAD()
        {
            return dal.GetSEQ_STG_LOAD();
        }
        public void AddEXP_STG_LOAD(EXP_STG_LOAD obj, string LOAD_BATCH, string appUser)
        {
            dal.AddEXP_STG_LOAD(obj, LOAD_BATCH, appUser);
        }

        public void ExecP_EXP_STG_LOAD_POST(string LOAD_BATCH)
        {
            dal.ExecP_EXP_STG_LOAD_POST(LOAD_BATCH);
        }

        public string CallF_EXP_HSCODE_CHK_BY_LOD(string LODNUM)
        {
            return dal.CallF_EXP_HSCODE_CHK_BY_LOD(LODNUM);
        }

        public void RunP_RELOAD_SHIP_FROM_RP(string SHIP_ID)
        {
            dal.RunP_RELOAD_SHIP_FROM_RP(SHIP_ID);
        }
    }
}

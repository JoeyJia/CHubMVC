using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class EXPWB_DAL
    {
        private CHubCommonHelper cchelper;
        public EXPWB_DAL()
        {
            cchelper = new CHubCommonHelper();
        }

        public List<EXP_SHIP_TO> GetSHIP_TO_LOCATION()
        {
            string sql = string.Format(@"select SHIP_TO_LOCATION from EXP_SHIP_TO");
            var result = cchelper.Search<EXP_SHIP_TO>(sql);
            return result;
        }

        public List<EXP_SHIP_TO> GetSHIP_TO_ALIAS(string SHIP_TO_LOCATION)
        {
            string sql = string.Format(@"select SHIP_TO_ALIAS from EXP_SHIP_TO where SHIP_TO_LOCATION='{0}'", SHIP_TO_LOCATION);
            var result = cchelper.Search<EXP_SHIP_TO>(sql);
            return result;
        }

        public void ExecP_Load_Stg_from_RP(string SHIP_TO_LOCATION)
        {
            cchelper.ExecP_Load_Stg_from_RP(SHIP_TO_LOCATION);
        }

        public List<V_EXP_STAGE_UNINVOICED> GetSHIP_TO_INDEX()
        {
            string sql = string.Format(@"select distinct SHIP_TO_INDEX from V_EXP_STAGE_UNINVOICED");
            var result = cchelper.Search<V_EXP_STAGE_UNINVOICED>(sql);
            return result;
        }

        public List<V_EXP_STAGE_UNINVOICED> SearchV_EXP_STAGE_UNINVOICED(string SHIP_TO_INDEX)
        {
            string sql = string.Format(@"select * from V_EXP_STAGE_UNINVOICED where 1=1");
            if (!string.IsNullOrEmpty(SHIP_TO_INDEX))
                sql += string.Format(@" and SHIP_TO_INDEX='{0}'", SHIP_TO_INDEX);
            var result = cchelper.Search<V_EXP_STAGE_UNINVOICED>(sql);
            return result;
        }

        public string CallFunc_GET_EXP_EST_AMT(string LODNUM)
        {
            string sql = string.Format(@"select get_EXP_EST_AMT('{0}') from dual", LODNUM);
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public List<EXP_STG_D> SearchDetailsByEXP_STG_D(string LODNUM)
        {
            string sql = string.Format(@"select * from EXP_STG_D where LODNUM='{0}'", LODNUM);
            var result = cchelper.Search<EXP_STG_D>(sql);
            return result;
        }

        public bool CheckSecurityOfbtnCreateInv(string SECURE_ID, string appUser)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN  where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='Y'", SECURE_ID, appUser);
            var result = cchelper.Search<APP_SECURE_PROC_ASSIGN>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public void AddEXP_COMM_INV(EXPCOMMINVArg arg)
        {
            string sql = string.Format(@"insert into EXP_COMM_INV(
                                        COMM_INV_ID,
                                        COMM_DESC,
                                        CREATED_BY,
                                        CREATE_DATE,
                                        SHIP_TO_INDEX,
                                        TOTAL_WGT,
                                        TOTAL_VOL,
                                        TOTAL_AMT,
                                        BOXES
                                        ) values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}')",
                                        arg.COMM_INV_ID, arg.COMM_DESC, arg.CREATED_BY, "sysdate", arg.SHIP_TO_INDEX, arg.TOTAL_WGT, arg.TOTAL_VOL, arg.TOTAL_AMT, arg.BOXES);
            cchelper.Update(sql);
        }

        public string GetCOMM_INV_ID()
        {
            string sql = string.Format(@"select SEQ_COMM_INV.NEXTVAL from dual");
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public void ChangeEXP_STG_H(string COMM_INV_ID, string LODNUM)
        {
            string sql = string.Format(@"update EXP_STG_H set COMM_INV_ID='{0}' where LODNUM='{1}'", COMM_INV_ID, LODNUM);
            cchelper.Update(sql);
        }

        public string GetSEQ_STG_LOAD()
        {
            string sql = string.Format(@"select SEQ_STG_LOAD.NEXTVAl from dual");
            var result = cchelper.ExecuteFunc(sql);
            return result;
        }

        public void AddEXP_STG_LOAD(EXP_STG_LOAD obj, string LOAD_BATCH, string appUser)
        {
            string sql = string.Format(@"insert into EXP_STG_LOAD(
                                    LOAD_BATCH,
                                    WH_ID,
                                    SHIP_ID_STG,
                                    LODNUM,
                                    HOST_EXT_ID,
                                    CARCOD,
                                    ORDNUM,
                                    ORDLIN,
                                    BTCUST,
                                    STCUST,
                                    ORDTYP,
                                    PRTNUM,
                                    UNTQTY,
                                    ORGCOD,
                                    VC_PALWGT,
                                    VC_PALLEN,
                                    VC_PALWID,
                                    VC_PALHGT,
                                    NOTE,
                                    FLEX1,
                                    FLEX2,
                                    LOADED_BY,
                                    LOAD_DATE
                                    ) values(
                                    '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}',{22}
                                    )", LOAD_BATCH, obj.WH_ID, obj.SHIP_ID_STG, obj.LODNUM, obj.HOST_EXT_ID, obj.CARCOD, obj.ORDNUM, obj.ORDLIN, obj.BTCUST, obj.STCUST, obj.ORDTYP, obj.PRTNUM, obj.UNTQTY,
                                    obj.ORGCOD, obj.VC_PALWGT, obj.VC_PALLEN, obj.VC_PALWID, obj.VC_PALHGT, obj.NOTE, obj.FLEX1, obj.FLEX2, appUser, "sysdate");
            cchelper.Update(sql);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using System.Data;

namespace CHubBLL
{
    public class V_MD_SR_ALL_BLL
    {
        private V_MD_SR_ALL_DAL dal;
        public V_MD_SR_ALL_BLL()
        {
            dal = new V_MD_SR_ALL_DAL();
        }

        public List<MD_SR_STATUS> GetSR_STATUS()
        {
            return dal.GetSR_STATUS();
        }

        public MD_SR_STATUS GetSR_STATUS_DESC(string SR_STATUS)
        {
            return dal.GetSR_STATUS_DESC(SR_STATUS);
        }

        public List<V_MD_SR_ALL> MDSRSearch(string PART_NO, string COMPANY_CODE, string SR_STATUS, string REQ_DATE, string IS_COMMON)
        {
            return dal.MDSRSearch(PART_NO, COMPANY_CODE, SR_STATUS, REQ_DATE, IS_COMMON);
        }

        public void MDSRSave(MDReqSRArg arg)
        {
            dal.MDSRSave(arg);
        }

        public bool IsOperate(string SECURE_ID, string UserName)
        {
            return dal.IsOperate(SECURE_ID, UserName);
        }

        public void MDSRChangeStatus(List<string> SR_REQ_NO, string SR_STATUS, string SR_COMMENTS)
        {
            dal.MDSRChangeStatus(SR_REQ_NO, SR_STATUS, SR_COMMENTS);
        }

        public DataTable MDSRDownloadTemp()
        {
            return dal.MDSRDownloadTemp();
        }

        public string GetSR_LOAD_SEQ()
        {
            return dal.GetSR_LOAD_SEQ();
        }
        public void InsertMD_SR_LOAD(string SR_LOAD_SEQ, MDSRLOADArg arg, string appUser)
        {
            dal.InsertMD_SR_LOAD(SR_LOAD_SEQ, arg, appUser);
        }

        public MD_COMPANY_SNAP CheckCOMPANY_CODE(string COMPANY_CODE)
        {
            return dal.CheckCOMPANY_CODE(COMPANY_CODE);
        }

        public void RunP_MD_SR_UPD_Status()
        {
            dal.RunP_MD_SR_UPD_Status();
        }
        public string CallGET_SQL()
        {
            return dal.CallGET_SQL();
        }
        public DataTable RunSql(string sql)
        {
            return dal.RunSql(sql);
        }
    }
}

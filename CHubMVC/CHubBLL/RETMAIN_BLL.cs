using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using System.Data;

namespace CHubBLL
{
    public class RETMAIN_BLL
    {
        private RETMAIN_DAL dal;
        public RETMAIN_BLL()
        {
            dal = new RETMAIN_DAL();
        }
        public List<V_RET_REQ_H> RetMainSearch(string CUSTOMER_NO, string RET_REQ_NO, string REQ_DATE)
        {
            return dal.RetMainSearch(CUSTOMER_NO, RET_REQ_NO, REQ_DATE);
        }
        public List<V_RET_REQ_D> GetRetMainDetailModal(string RET_REQ_NO)
        {
            return dal.GetRetMainDetailModal(RET_REQ_NO);
        }
        public List<V_RET_REQ_D> GetRetMainDetailByRejectReason(string RET_REQ_NO, string REJECT_REASON)
        {
            return dal.GetRetMainDetailByRejectReason(RET_REQ_NO, REJECT_REASON);
        }
        public List<RET_PART_GROUP> GetPART_GROUP()
        {
            return dal.GetPART_GROUP();
        }
        public void DeleteFromRET_REQ_D(string RET_REQ_NO, string LINE_NO)
        {
            dal.DeleteFromRET_REQ_D(RET_REQ_NO, LINE_NO);
        }
        public void SaveRET_REQ_D(RET_REQ_DArg arg, string RET_REQ_NO)
        {
            dal.SaveRET_REQ_D(arg, RET_REQ_NO);
        }
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckSecurity(SECURE_ID, APP_USER);
        }
        public void UpdateREQ_STATUS(string REQ_STATUS,string RET_REQ_NO)
        {
            dal.UpdateREQ_STATUS(REQ_STATUS, RET_REQ_NO);
        }
        public void ExecP_RET_Verify(string RET_REQ_NO)
        {
            dal.ExecP_RET_Verify(RET_REQ_NO);
        }
        public string CallF_GOMS_PARTNO(string CUST_ITEM)
        {
            return dal.CallF_GOMS_PARTNO(CUST_ITEM);
        }
        public string CallF_GOMS_desc(string CUST_ITEM)
        {
            return dal.CallF_GOMS_desc(CUST_ITEM);
        }
        public string CallF_RET_PART_GROUP(string CUST_ITEM)
        {
            return dal.CallF_RET_PART_GROUP(CUST_ITEM);
        }
        public string CallF_RET_PART_supp(string CUST_ITEM)
        {
            return dal.CallF_RET_PART_supp(CUST_ITEM);
        }
        public string RetMainGetSql(string RET_REQ_NO)
        {
            return dal.RetMainGetSql(RET_REQ_NO);
        }
        public DataTable RunRetMainSql(string sql)
        {
            return dal.RunRetMainSql(sql);
        }
        public void ChangeRET_REQ_H_Status(string RET_REQ_NO)
        {
            dal.ChangeRET_REQ_H_Status(RET_REQ_NO);
        }
        public List<string> GetLOAD_TYPEs(string appUser)
        {
            return dal.GetLOAD_TYPEs(appUser);
        }
        public string GetLOAD_DESC(string LOAD_TYPE)
        {
            return dal.GetLOAD_DESC(LOAD_TYPE);
        }
        public List<RET_RETURN_TYPE> GetRETURN_TYPEs()
        {
            return dal.GetRETURN_TYPEs();
        }
        public IHUB_LOAD_TYPE GetIHUB_LOAD_TYPE(string LOAD_TYPE)
        {
            return dal.GetIHUB_LOAD_TYPE(LOAD_TYPE);
        }
    }
}

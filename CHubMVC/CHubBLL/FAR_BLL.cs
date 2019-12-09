using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubModel.WebArg;
using CHubDBEntity.UnmanagedModel;
using System.Data;

namespace CHubBLL
{
    public class FAR_BLL
    {
        private FAR_DAL dal;
        public FAR_BLL()
        {
            dal = new FAR_DAL();
        }

        public List<V_FAR_HEADER> MyFarSearch(FAR_Arg arg)
        {
            return dal.MyFarSearch(arg);
        }
        public DataTable GetFAR_STATUS()
        {
            return dal.GetFAR_STATUS();
        }
        public DataTable GetCUSTOMER_NO()
        {
            return dal.GetCUSTOMER_NO();
        }
        public DataTable GetADJ_TYPE()
        {
            return dal.GetADJ_TYPE();
        }
        public DataTable GetPRIORITY_CODE()
        {
            return dal.GetPRIORITY_CODE();
        }
        public DataTable GetINV_STRATEGY_CODE()
        {
            return dal.GetINV_STRATEGY_CODE();
        }
        public bool IsExistMyFar(FAR_HEADER far)
        {
            return dal.IsExistMyFar(far);
        }
        public void MyFarUpdate(FAR_HEADER far)
        {
            dal.MyFarUpdate(far);
        }
        public void MyFarAdd(FAR_HEADER far)
        {
            dal.MyFarAdd(far);
        }
        public string GetFar_No()
        {
            return dal.GetFar_No();
        }
        public V_FAR_HEADER GetMyFarHeader(string FAR_NO)
        {
            return dal.GetMyFarHeader(FAR_NO);
        }
        public List<V_FAR_DETAIL> GetMyFarDetail(string FAR_NO)
        {
            return dal.GetMyFarDetail(FAR_NO);
        }
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckSecurity(SECURE_ID, APP_USER);
        }
        public void UpdateFarStatus(string FAR_NO, string FAR_STATUS)
        {
            dal.UpdateFarStatus(FAR_NO, FAR_STATUS);
        }
        public string ExportFar(string FAR_NO)
        {
            return dal.ExportFar(FAR_NO);
        }
        public DataTable RunSql(string sql)
        {
            return dal.RunSql(sql);
        }
        public string GetFAR_LOAD()
        {
            return dal.GetFAR_LOAD();
        }
        public void InsertFAR_DETAIL_STG(FAR_DETAIL_STG fd, string LOAD_SEQ)
        {
            dal.InsertFAR_DETAIL_STG(fd, LOAD_SEQ);
        }
        public string CheckCUST_PARTNO(string CUST_PARTNO)
        {
            return dal.CheckCUST_PARTNO(CUST_PARTNO);
        }
        public void DeleteFarDetail(string FAR_NO, string LOAD_SEQ, string CUST_PARTNO)
        {
            dal.DeleteFarDetail(FAR_NO, LOAD_SEQ, CUST_PARTNO);
        }
    }
}

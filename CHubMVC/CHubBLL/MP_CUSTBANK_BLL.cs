using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;
using System.Data;

namespace CHubBLL
{
    public class MP_CUSTBANK_BLL
    {
        private MP_CUSTBANK_DAL dal;
        public MP_CUSTBANK_BLL()
        {
            dal = new MP_CUSTBANK_DAL();
        }
        public List<V_E_CUST_BANKING> MP_CUSTBANKSearch(V_E_CUST_BANKING SearchCondition)
        {
            return dal.MP_CUSTBANKSearch(SearchCondition);
        }
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckSecurity(SECURE_ID, APP_USER);
        }
        public void MP_CUSTBANKSave(V_E_CUST_BANKING item, string AppUser)
        {
            dal.MP_CUSTBANKSave(item, AppUser);
        }
        public V_E_CUST_BANKING MP_CUSTBANK(V_E_CUST_BANKING item)
        {
            return dal.MP_CUSTBANK(item);
        }
        public List<V_E_TRANS_TYPE_ASSIGN> GetManualADJTransType(string App_User,string TRANS_TYPE)
        {
            return dal.GetManualADJTransType(App_User, TRANS_TYPE);
        }
        public string CheckOrderNo(string CUSTOMER_NO, string BILL_TO_LOCATION, string ORDER_NO)
        {
            return dal.CheckOrderNo(CUSTOMER_NO, BILL_TO_LOCATION, ORDER_NO);
        }
        public string GetAmtByOrderNo(string ORDER_NO)
        {
            return dal.GetAmtByOrderNo(ORDER_NO);
        }
        public bool ManualADJcheckDOC_NO(string DOC_NO)
        {
            return dal.ManualADJcheckDOC_NO(DOC_NO);
        }
        public void RunP_BANK_TRANS_NEW(E_BANKING_TRANS item)
        {
            dal.RunP_BANK_TRANS_NEW(item);
        }
        public List<E_TRANS_TYPE> GetTransType()
        {
            return dal.GetTransType();
        }
        public string TransHistoryGetTrans_TYPE(string TRANS_TYPE)
        {
            return dal.TransHistoryGetTrans_TYPE(TRANS_TYPE);
        }
        public List<V_E_BANKING_TRANS> TransHistoryQuery(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE, string TRANS_TYPE, string TRANS_DATE)
        {
            return dal.TransHistoryQuery(CUSTOMER_NO, BILL_TO_LOCATION, CURRENCY_CODE, TRANS_TYPE, TRANS_DATE);
        }
        public DataTable TransHistoryDownload(string CUSTOMER_NO, string BILL_TO_LOCATION, string CURRENCY_CODE, string TRANS_TYPE, string TRANS_DATE)
        {
            return dal.TransHistoryDownload(CUSTOMER_NO, BILL_TO_LOCATION, CURRENCY_CODE, TRANS_TYPE, TRANS_DATE);
        }
        public decimal GetLOAD_BATCH()
        {
            return dal.GetLOAD_BATCH();
        }
        public void LoadData(E_BANKING_TRANS_LOAD item, string AppUser, decimal LOAD_BATCH, decimal LINE_NO)
        {
            dal.LoadData(item, AppUser, LOAD_BATCH, LINE_NO);
        }
        public void RunP_BANK_TRANS_LOAD_POST(decimal LOAD_BATCH)
        {
            dal.RunP_BANK_TRANS_LOAD_POST(LOAD_BATCH);
        }
        public DataTable BankReceiptDownload(decimal LOAD_BATCH)
        {
            return dal.BankReceiptDownload(LOAD_BATCH);
        }
        public string CallF_GOMS_ORD_BRIEF(string ORDER_NO)
        {
            return dal.CallF_GOMS_ORD_BRIEF(ORDER_NO);
        }
    }
}

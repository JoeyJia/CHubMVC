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
    public class QuickOrd_BLL
    {
        private QuickOrd_DAL dal;
        public QuickOrd_BLL()
        {
            dal = new QuickOrd_DAL();
        }
        public List<G_ADDR_DFLT> GetG_ADDR_DFLT(string SYSID, string ABBREVIATION)
        {
            return dal.GetG_ADDR_DFLT(SYSID, ABBREVIATION);
        }
        public List<G_ADDR_SPL> GetG_ADDR_SPL(string SYSID, string ABBREVIATION, string KeyWord, int PageStart, int PageEnd)
        {
            return dal.GetG_ADDR_SPL(SYSID, ABBREVIATION, KeyWord, PageStart, PageEnd);
        }
        public G_ADDR_SPL GetG_ADDR_SPLDetail(string SYSID, string ABBREVIATION, string DEST_LOCATION)
        {
            return dal.GetG_ADDR_SPLDetail(SYSID, ABBREVIATION, DEST_LOCATION);
        }
        public List<G_ORDER_TYPE> GetG_ORDER_TYPE(string SYSID, string WAREHOUSE, string DUE_DATE_CODE)
        {
            return dal.GetG_ORDER_TYPE(SYSID, WAREHOUSE, DUE_DATE_CODE);
        }
        public string CallF_QUICK_PART(string GOMS, string CUSTOMER_NO, string CUSTOMER_PARTNO)
        {
            return dal.CallF_QUICK_PART(GOMS, CUSTOMER_NO, CUSTOMER_PARTNO);
        }
        public string CallF_QUICK_QTY(string PART_NO, string BUY_QTY)
        {
            return dal.CallF_QUICK_QTY(PART_NO, BUY_QTY);
        }
        public string CallF_QUICK_DESC(string PART_NO)
        {
            return dal.CallF_QUICK_DESC(PART_NO);
        }
        public string CallF_QUICK_MSG(string PART_NO)
        {
            return dal.CallF_QUICK_MSG(PART_NO);
        }
        public string CallF_QUICK_INV(string WAREHOUSE, string PART_NO)
        {
            return dal.CallF_QUICK_INV(WAREHOUSE, PART_NO);
        }
        public string GetQUICK_ORDER_NO()
        {
            return dal.GetQUICK_ORDER_NO();
        }
        public void SaveQUICK_OEORDER_HEADER(QUICK_OEORDER_HEADER header)
        {
            dal.SaveQUICK_OEORDER_HEADER(header);
        }
        public void UpdateQUICK_OEORDER_HEADER(QUICK_OEORDER_HEADER header)
        {
            dal.UpdateQUICK_OEORDER_HEADER(header);
        }
        public void SaveQUICK_OEORDER_DETAIL(QUICK_OEORDER_DETAIL detail)
        {
            dal.SaveQUICK_OEORDER_DETAIL(detail);
        }
        public void GetQUICK_OEORDER_DETAILByQUICK_ORDER_NO(string QUICK_ORDER_NO)
        {
            dal.GetQUICK_OEORDER_DETAILByQUICK_ORDER_NO(QUICK_ORDER_NO);
        }
        public List<V_QUICK_EXPORT_WEBPART_HDR> GetV_QUICK_EXPORT_WEBPART_HDR(decimal QUICK_ORDER_NO)
        {
            return dal.GetV_QUICK_EXPORT_WEBPART_HDR(QUICK_ORDER_NO);
        }
        public List<V_QUICK_EXPORT_WEBPART_DTL> GetV_QUICK_EXPORT_WEBPART_DTL(decimal QUICK_ORDER_NO)
        {
            return dal.GetV_QUICK_EXPORT_WEBPART_DTL(QUICK_ORDER_NO);
        }
        public string RunFunc(string QUICK_ORDER_NO, string Identifier)
        {
            return dal.RunFunc(QUICK_ORDER_NO, Identifier);
        }
        public DataTable GetQORDLine(string QUICK_ORDER_NO)
        {
            return dal.GetQORDLine(QUICK_ORDER_NO);
        }
        public void UpdateState(string QUICK_ORDER_NO, string PROCESS_STATUS, string PROCESS_ERROR = "")
        {
            dal.UpdateState(QUICK_ORDER_NO, PROCESS_STATUS, PROCESS_ERROR);
        }
        public void ExecP_CRT_ORDER_FILE_QORD(string QUICK_ORDER_NO)
        {
            dal.ExecP_CRT_ORDER_FILE_QORD(QUICK_ORDER_NO);
        }
        public void UpdateQORDStatus(string QUICK_ORDER_NO)
        {
            dal.UpdateQORDStatus(QUICK_ORDER_NO);
        }
    }
}

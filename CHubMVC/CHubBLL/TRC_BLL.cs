using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class TRC_BLL
    {
        private TRC_DAL dal;
        public TRC_BLL()
        {
            dal = new TRC_DAL();
        }

        public List<APP_WH_NEW> GetWH_ID()
        {
            return dal.GetWH_ID();
        }
        public APP_WH_NEW GetWH_ID_DESC(string WH_ID)
        {
            return dal.GetWH_ID_DESC(WH_ID);
        }
        public List<RP_ADR_MST_NEW> LBTRACECSearch(string ADRNAM, string WH_ID)
        {
            return dal.LBTRACECSearch(ADRNAM, WH_ID);
        }
        public bool LBTRACECCheck(RP_ADR_MST_NEW item)
        {
            return dal.LBTRACECCheck(item);
        }
        public void LBTRACECSave(RP_ADR_MST_NEW item)
        {
            dal.LBTRACECSave(item);
        }
        public int LBTRACEPSearchCount(string PART_NO)
        {
            return dal.LBTRACEPSearchCount(PART_NO);
        }
        public List<V_G_PART_ADDTIONAL> LBTRACEPSearch(string PART_NO, int RowStart, int RowEnd)
        {
            return dal.LBTRACEPSearch(PART_NO,RowStart,RowEnd);
        }
        public bool LBTRACEPCheck(V_G_PART_ADDTIONAL item)
        {
            return dal.LBTRACEPCheck(item);
        }
        public void LBTRACEPSave(V_G_PART_ADDTIONAL item)
        {
            dal.LBTRACEPSave(item);
        }
        public List<V_TRC_SCAN_HISTORY> LBTRACEINQSearch(string BARCODE, string DOC_NO)
        {
            return dal.LBTRACEINQSearch(BARCODE, DOC_NO);
        }
        public V_TRC_SCAN_HISTORY LBTRACEINQDetail(string SCAN_SEQ)
        {
            return dal.LBTRACEINQDetail(SCAN_SEQ);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using System.Data;

namespace CHubDAL
{
    public class TRC_DAL
    {
        private CHubCommonHelper ccHelper;
        public TRC_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<APP_WH_NEW> GetWH_ID()
        {
            string sql = string.Format(@"select * from APP_WH");
            var result = ccHelper.Search<APP_WH_NEW>(sql);
            return result;
        }

        public APP_WH_NEW GetWH_ID_DESC(string WH_ID)
        {
            string sql = string.Format(@"select * from APP_WH where WH_ID='{0}'", WH_ID);
            var result = ccHelper.Search<APP_WH_NEW>(sql).First();
            return result;
        }

        public List<RP_ADR_MST_NEW> LBTRACECSearch(string ADRNAM, string WH_ID)
        {
            string sql = string.Format(@"select * from RP_ADR_MST where 1=1 ");
            if (!string.IsNullOrEmpty(ADRNAM))
                sql += string.Format(@" and ADRNAM like '%{0}%'", ADRNAM);
            if (!string.IsNullOrEmpty(WH_ID))
                sql += string.Format(@" and WH_ID='{0}'", WH_ID);
            var result = ccHelper.Search<RP_ADR_MST_NEW>(sql);
            return result;
        }

        public bool LBTRACECCheck(RP_ADR_MST_NEW item)
        {
            string sql = string.Format(@"select * from RP_ADR_MST where WH_ID='{0}' and ADRNAM='{1}'", item.WH_ID, item.ADRNAM);
            var result = ccHelper.Search<RP_ADR_MST_NEW>(sql).First();
            if (result.LABEL_TRACE == item.LABEL_TRACE)
                return false;
            else
                return true;
        }

        public void LBTRACECSave(RP_ADR_MST_NEW item)
        {
            string sql = string.Format(@"update RP_ADR_MST set LABEL_TRACE='{0}' where WH_ID='{1}' and ADRNAM='{2}'", item.LABEL_TRACE, item.WH_ID, item.ADRNAM);
            ccHelper.Update(sql);
        }

        public int LBTRACEPSearchCount(string PART_NO)
        {
            string sql = string.Format(@"select count(*) TOTALCOUNT from V_G_PART_ADDTIONAL where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO like '%{0}%'", PART_NO);
            var result = ccHelper.ExecuteFunc(sql);
            return Convert.ToInt32(result);
        }

        public List<V_G_PART_ADDTIONAL> LBTRACEPSearch(string PART_NO, int RowStart, int RowEnd)
        {
            string sql = string.Format(@"select t.* from (
                                select rownum rn, v.* from V_G_PART_ADDTIONAL v where PART_NO like '%{0}%'
                                        ) t where t.rn>={1} and t.rn<={2}", PART_NO, RowStart, RowEnd);
            var result = ccHelper.Search<V_G_PART_ADDTIONAL>(sql);
            return result;
        }

        public bool LBTRACEPCheck(V_G_PART_ADDTIONAL item)
        {
            string sql = string.Format(@"select * from V_G_PART_ADDTIONAL where PART_NO='{0}'", item.PART_NO);
            var result = ccHelper.Search<V_G_PART_ADDTIONAL>(sql).First();
            if (result.LABEL_TRACE == item.LABEL_TRACE)
                return false;
            else
                return true;
        }
        public void LBTRACEPSave(V_G_PART_ADDTIONAL item)
        {
            string sql = string.Format(@"update G_PART_ADDTIONAL set LABEL_TRACE='{0}' where PART_NO='{1}'", item.LABEL_TRACE, item.PART_NO);
            ccHelper.Update(sql);
        }

        public List<V_TRC_SCAN_HISTORY> LBTRACEINQSearch(string BARCODE, string DOC_NO)
        {
            string sql = string.Format(@"select * from V_TRC_SCAN_HISTORY where 1=1");
            if (!string.IsNullOrEmpty(BARCODE))
                sql += string.Format(@" and BARCODE='{0}'", BARCODE);
            if (!string.IsNullOrEmpty(DOC_NO))
                sql += string.Format(@" and DOC_NO='{0}'", DOC_NO);
            var result = ccHelper.Search<V_TRC_SCAN_HISTORY>(sql);
            return result;
        }

        public V_TRC_SCAN_HISTORY LBTRACEINQDetail(string SCAN_SEQ)
        {
            string sql = string.Format(@"select * from V_TRC_SCAN_HISTORY where SCAN_SEQ='{0}'", SCAN_SEQ);
            var result = ccHelper.Search<V_TRC_SCAN_HISTORY>(sql).First();
            return result;
        }

    }
}

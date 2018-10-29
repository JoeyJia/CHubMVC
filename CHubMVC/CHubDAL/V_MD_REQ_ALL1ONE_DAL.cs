using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class V_MD_REQ_ALL1ONE_DAL
    {
        private CHubCommonHelper cchelper;
        public V_MD_REQ_ALL1ONE_DAL()
        {
            cchelper = new CHubCommonHelper();
        }

        public List<V_MD_REQ_ALL1ONE> MDReqInqSearch(string MD_REQ_NO, string PART_NO, string REQ_DATE, string APP_STATUS, string REQ_BY, string WWID, string CHECK_EXIST, string COMM_PART)
        {
            string sql = string.Format(@"select * from V_MD_REQ_ALL1ONE where 1=1");
            if (!string.IsNullOrEmpty(MD_REQ_NO))
                sql += string.Format(@" and MD_REQ_NO = '{0}'", MD_REQ_NO);
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO like '%{0}%'", PART_NO);
            if (!string.IsNullOrEmpty(REQ_DATE))
                sql += string.Format(@" and REQ_DATE >= sysdate - {0}", REQ_DATE);
            if (!string.IsNullOrEmpty(APP_STATUS))
                sql += string.Format(@" and APP_STATUS ='{0}'", APP_STATUS);
            if (!string.IsNullOrEmpty(REQ_BY))
                sql += string.Format(@" and REQ_BY ='{0}'", REQ_BY);
            if (!string.IsNullOrEmpty(WWID) && string.IsNullOrEmpty(REQ_BY))
                sql += string.Format(@" and REQ_BY like '%{0}%'", WWID);
            if (!string.IsNullOrEmpty(CHECK_EXIST))
                sql += string.Format(@" and CHECK_EXIST ='{0}'", CHECK_EXIST);
            if (!string.IsNullOrEmpty(COMM_PART))
                sql += string.Format(@" and COMM_PART = '{0}'", COMM_PART);

            var result = cchelper.Search<V_MD_REQ_ALL1ONE>(sql);

            return result;
        }


        public List<MD_PRODUCT_GROUP> GetProductGroupID()
        {
            string sql = string.Format(@"select PRODUCT_GROUP_ID,GROUP_DESC from MD_PRODUCT_GROUP where ACTIVEIND='Y' ");
            var result = cchelper.Search<MD_PRODUCT_GROUP>(sql);
            return result;
        }

        public List<MD_PIM_SNAP> GetPartDesc(string PART_NO)
        {
            string sql = string.Format(@"select PART_NO,DESCRIPTION,PART_SHORT_DESC from MD_PIM_SNAP where PART_NO='{0}'", PART_NO);
            var result = cchelper.Search<MD_PIM_SNAP>(sql);
            return result;
        }

        public List<MD_REQ_DETAIL> GetMDReqDetail(string MD_REQ_NO, string REQ_LINE_NO)
        {
            string sql = string.Format(@"select * from MD_REQ_DETAIL where MD_REQ_NO='{0}' and REQ_LINE_NO='{1}'", MD_REQ_NO, REQ_LINE_NO);
            var result = cchelper.Search<MD_REQ_DETAIL>(sql);
            return result;
        }

        public bool IsOperated(string secureID, string appUser)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='Y'", secureID, appUser);
            var result = cchelper.Search<APP_SECURE_PROC_ASSIGN>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public void UpdateMDReqInq(MD_REQ_DETAIL mrd)
        {
            string sql = string.Format(@"Update MD_REQ_DETAIL set 
                                            PART_DESC='{0}',
                                            PRODUCT_GROUP_ID='{1}',
                                            COMM_PART='{2}',
                                            GLOBAL_PARTNO='{3}',
                                            PART_DESC_SHORT='{4}',
                                            GLOBAL_DESC='{5}',
                                            NOTE='{6}' 
                                            where MD_REQ_NO='{7}' and REQ_LINE_NO='{8}'", mrd.PART_DESC, mrd.PRODUCT_GROUP_ID, mrd.COMM_PART, mrd.GLOBAL_PARTNO, mrd.PART_DESC_SHORT, mrd.GLOBAL_DESC, mrd.NOTE, mrd.MD_REQ_NO, mrd.REQ_LINE_NO);
            cchelper.Update(sql);
        }

        public void UpdateMDReqInqStatus(MDReqDetailListArg arg, string APP_STATUS, string APP_COMMENTS)
        {
            string sql = string.Format(@"Update MD_REQ_DETAIL set 
                                        APP_STATUS='{0}'", APP_STATUS);
            if (!string.IsNullOrEmpty(APP_COMMENTS))
                sql += string.Format(@",APP_COMMENTS='{0}'", APP_COMMENTS);
            else
                sql += string.Format(@",APP_COMMENTS=''");
            sql += string.Format(@" where MD_REQ_NO='{0}' and REQ_LINE_NO='{1}'", arg.MD_REQ_NO, arg.REQ_LINE_NO);
            cchelper.Update(sql);
        }


        public List<MD_APP_STATUS> GetAPP_STATUS()
        {
            string sql = string.Format(@"select * from MD_APP_STATUS");
            var result = cchelper.Search<MD_APP_STATUS>(sql);
            return result;
        }


    }
}

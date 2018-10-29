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
    public class V_MD_REQ_ALL1ONE_BLL
    {
        private V_MD_REQ_ALL1ONE_DAL dal;

        public V_MD_REQ_ALL1ONE_BLL()
        {
            dal = new V_MD_REQ_ALL1ONE_DAL();
        }

        public List<V_MD_REQ_ALL1ONE> MDReqInqSearch(string MD_REQ_NO, string PART_NO, string REQ_DATE,string APP_STATUS, string REQ_BY, string WWID, string CHECK_EXIST,string COMM_PART)
        {
            return dal.MDReqInqSearch(MD_REQ_NO, PART_NO, REQ_DATE,APP_STATUS, REQ_BY,WWID,CHECK_EXIST,COMM_PART);
        }

        public List<MD_PRODUCT_GROUP> GetProductGroupID()
        {
            return dal.GetProductGroupID();
        }

        public List<MD_PIM_SNAP> GetPartDesc(string PART_NO)
        {
            return dal.GetPartDesc(PART_NO);
        }

        public List<MD_REQ_DETAIL> GetMDReqDetail(string MD_REQ_NO, string REQ_LINE_NO)
        {
            return dal.GetMDReqDetail(MD_REQ_NO, REQ_LINE_NO);
        }


        public bool IsOperated(string secureID, string appUser)
        {
            return dal.IsOperated(secureID, appUser);
        }

        public void UpdateMDReqInq(MD_REQ_DETAIL mrd)
        {
            dal.UpdateMDReqInq(mrd);
        }

        public void UpdateMDReqInqStatus(MDReqDetailListArg arg, string APP_STATUS, string APP_COMMENTS)
        {
            dal.UpdateMDReqInqStatus(arg, APP_STATUS, APP_COMMENTS);
        }

        public List<MD_APP_STATUS> GetAPP_STATUS()
        {
            return dal.GetAPP_STATUS();
        }
    }
}

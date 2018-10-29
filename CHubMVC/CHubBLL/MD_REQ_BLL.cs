using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class MD_REQ_BLL
    {
        private MD_REQ_DAL dal;

        public MD_REQ_BLL()
        {
            dal = new MD_REQ_DAL();
        }

        public string GetPART_NO(string partno)
        {
            return dal.GetPART_NO(partno);
        }

        public string GetPART_DESC(string partno)
        {
            return dal.GetPART_DESC(partno);
        }

        public string GetCHECK_EXIST(string partno)
        {
            return dal.GetCHECK_EXIST(partno);
        }

        public string GetGLOBAL_PARTNO(string partno)
        {
            return dal.GetGLOBAL_PARTNO(partno);
        }

        public string GetGLOBAL_PARTDESC(string globalpartno)
        {
            return dal.GetGLOBAL_PARTDESC(globalpartno);
        }

        public string GetSHORT_DESC(string GLOBAL_PARTNO)
        {
            return dal.GetSHORT_DESC(GLOBAL_PARTNO);
        }

        public string GetCHECK_PRI_SUP(string partno)
        {
            return dal.GetCHECK_PRI_SUP(partno);
        }

        public string GetCHECK_PRI_PB(string partno)
        {
            return dal.GetCHECK_PRI_PB(partno);
        }

        public string GetCHECK_PRI_BPA(string partno)
        {
            return dal.GetCHECK_PRI_BPA(partno);
        }

        public string GetCHECK_COST(string partno)
        {
            return dal.GetCHECK_COST(partno);
        }

        public string GetPRODUCT_GROUP_ID(string partno)
        {
            return dal.GetPRODUCT_GROUP_ID(partno);
        }

        public string GetRequestNo()
        {
            return dal.GetRequestNo();
        }

        public void SaveMD_REQ_HEADER(MD_REQ_HEADER mrHeader)
        {
            dal.SaveMD_REQ_HEADER(mrHeader);
        }

        public void SaveMD_REQ_DETAIL(MD_REQ_DETAIL mrDetail)
        {
            dal.SaveMD_REQ_DETAIL(mrDetail);
        }

        public void ExecP_MD_SR_NEW(string PART_NO, string PRODUCT_GROUP_ID, string NOTE, string REQ_BY)
        {
            dal.ExecP_MD_SR_NEW(PART_NO, PRODUCT_GROUP_ID, NOTE, REQ_BY);
        }

    }
}

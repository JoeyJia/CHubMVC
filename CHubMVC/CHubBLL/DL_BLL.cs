using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class DL_BLL
    {
        private DL_DAL dal;
        public DL_BLL()
        {
            dal = new DL_DAL();
        }

        public List<string> GetLOAD_TYPEs(string appUser)
        {
            return dal.GetLOAD_TYPEs(appUser);
        }

        public string GetLOAD_DESC(string LOAD_TYPE)
        {
            return dal.GetLOAD_DESC(LOAD_TYPE);
        }
        public IHUB_LOAD_TYPE GetIHUB_LOAD_TYPE(string LOAD_TYPE)
        {
            return dal.GetIHUB_LOAD_TYPE(LOAD_TYPE);
        }
        public string GetLOAD_BATCH()
        {
            return dal.GetLOAD_BATCH();
        }
        public void AddIHUB_LOAD_BASE(IHUB_LOAD_BASE ilb, string LOAD_BATCH, string LOAD_TYPE, string LOAD_BY, string LOAD_LINE_NO)
        {
            dal.AddIHUB_LOAD_BASE(ilb, LOAD_BATCH, LOAD_TYPE, LOAD_BY, LOAD_LINE_NO);
        }
        public void ExecP_IHUB_LOAD_POST(decimal LOAD_BATCH, string LOAD_TYPE)
        {
            dal.ExecP_IHUB_LOAD_POST(LOAD_BATCH, LOAD_TYPE);
        }
    }
}

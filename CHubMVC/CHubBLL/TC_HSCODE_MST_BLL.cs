using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
   public class TC_HSCODE_MST_BLL
    {
        private readonly TC_HSCODE_MST_DAL dal;

        public TC_HSCODE_MST_BLL()
        {
            dal = new TC_HSCODE_MST_DAL();
        }

        public TC_HSCODE_MST_BLL(CHubEntities db)
        {
            dal = new TC_HSCODE_MST_DAL(db);
        }

        public List<TC_HSCODE_MST> GetHSCODEByCode(string HSCODE)
        {
            return dal.GetHSCODEByCode(HSCODE);
        }


        public bool IsExistHSCODE(string HSCODE)
        {
            return dal.IsExistHSCODE(HSCODE);
        }

        public void AddOrUpdate(TC_HSCODE_MST tc,string type)
        {
            dal.AddOrUpdate(tc, type);
        }

        public List<TC_HSCODE_AUDIT> GetHsCodeAudit(string HSCODE)
        {
            return dal.GetHsCodeAudit(HSCODE);
        }


    }
}

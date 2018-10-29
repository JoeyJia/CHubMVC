using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;

namespace CHubBLL
{
    public class EXP_EXCHANGE_RATE_BLL
    {
        private EXP_EXCHANGE_RATE_DAL dal;
        public EXP_EXCHANGE_RATE_BLL()
        {
            dal = new EXP_EXCHANGE_RATE_DAL();
        }

        public List<string> GetEXCHANGE_TYPE()
        {
            return dal.GetEXCHANGE_TYPE();
        }
        public List<EXP_EXCHANGE_RATE> GetTableResult(string EXCHANGE_TYPE)
        {
            return dal.GetTableResult(EXCHANGE_TYPE);
        }
        public void InsertOrUpdateEXPRATE(string EXCHANGETYPE, string STARTDATE, EXP_EXCHANGE_RATE eer, string method, string appUser)
        {
            dal.InsertOrUpdateEXPRATE(EXCHANGETYPE, STARTDATE, eer, method, appUser);
        }
    }
}

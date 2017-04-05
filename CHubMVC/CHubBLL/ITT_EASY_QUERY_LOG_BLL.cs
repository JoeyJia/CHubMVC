using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_EASY_QUERY_LOG_BLL
    {
        private readonly ITT_EASY_QUERY_LOG_DAL dal;

        public ITT_EASY_QUERY_LOG_BLL()
        {
            dal = new ITT_EASY_QUERY_LOG_DAL();
        }
        public ITT_EASY_QUERY_LOG_BLL(CHubEntities db)
        {
            dal = new ITT_EASY_QUERY_LOG_DAL(db);
        }

        public decimal Add(ITT_EASY_QUERY_LOG model)
        {
            decimal seq =  model.EASY_QUERY_TOKEN = dal.GetEasyQuerySqeNextVal();
            dal.Add(model);
            return seq;
        }

    }
}

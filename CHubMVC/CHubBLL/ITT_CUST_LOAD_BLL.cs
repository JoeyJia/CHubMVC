using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_CUST_LOAD_BLL
    {
        private readonly ITT_CUST_LOAD_DAL dal;

        public ITT_CUST_LOAD_BLL()
        {
            dal = new ITT_CUST_LOAD_DAL();
        }
        public ITT_CUST_LOAD_BLL(CHubEntities db)
        {
            dal = new ITT_CUST_LOAD_DAL(db);
        }

        public List<ITT_CUST_LOAD> GetCustList(string willBillNo)
        {
            return dal.GetCustList(willBillNo);
        }

        public void Save(ITT_CUST_LOAD model)
        {
            if (model.LOAD_BATCH_TOKEN == 0)
            {
                model.LOAD_BATCH_TOKEN = dal.GetCustLoadSqeNextVal();
                dal.Add(model);
            }
            else
                dal.Update(model);
        }

    }
}

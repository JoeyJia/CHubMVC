using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class ITT_TRAN_LOAD_BLL
    {
        private readonly ITT_TRAN_LOAD_DAL dal;

        public ITT_TRAN_LOAD_BLL()
        {
            dal = new ITT_TRAN_LOAD_DAL();
        }
        public ITT_TRAN_LOAD_BLL(CHubEntities db)
        {
            dal = new ITT_TRAN_LOAD_DAL(db);
        }

        public List<ITT_TRAN_LOAD> GetTranLoadList(string willBillNo)
        {
            return dal.GetTranLoadList(willBillNo);
        }

        public void Save(ITT_TRAN_LOAD model)
        {
            if (model.LOAD_BATCH_TOKEN == 0)
            {
                model.LOAD_BATCH_TOKEN = dal.GetTranLoadSqeNextVal();
                dal.Add(model);
            }
            else
                dal.Update(model);
        }

        public void Delete(string token)
        {
            ITT_TRAN_LOAD model = dal.GetTranLoad(decimal.Parse(token));
            dal.Delete(model);
        }

    }
}

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

        public bool ExistCustLoad(string WayBillNo, string tcGroup)
        {
            return dal.ExistCustLoad(WayBillNo, tcGroup);
        }

        public ITT_CUST_LOAD GetCustLoadbyConstraint(string WayBillNo, string tcGroup)
        {
            return dal.GetCustLoadbyConstraint(WayBillNo, tcGroup);
        }

        public void Save(ITT_CUST_LOAD model)
        {
            if (model.LOAD_BATCH_TOKEN == 0 && !dal.ExistCustLoad(model.WILL_BILL_NO, model.TC_GROUP))
            {
                model.LOAD_BATCH_TOKEN = dal.GetCustLoadSqeNextVal();
                dal.Add(model);
            }
            else
            {
                ITT_CUST_LOAD existCL = dal.GetCustLoadbyConstraint(model.WILL_BILL_NO, model.TC_GROUP);
                existCL.DO_RELEASE_DATE = model.DO_RELEASE_DATE != null ? model.DO_RELEASE_DATE : existCL.DO_RELEASE_DATE;
                existCL.BND_ARRIVAL_DATE = model.BND_ARRIVAL_DATE != null ? model.BND_ARRIVAL_DATE : existCL.BND_ARRIVAL_DATE;
                existCL.BND_OUT_DATE = model.BND_OUT_DATE != null ? model.BND_OUT_DATE : existCL.BND_OUT_DATE;
                existCL.NBND_ARRIVAL_DATE = model.NBND_ARRIVAL_DATE != null ? model.NBND_ARRIVAL_DATE : existCL.NBND_ARRIVAL_DATE;
                existCL.NOTE = !string.IsNullOrEmpty(model.NOTE) ? model.NOTE : existCL.NOTE;
                existCL.LOADED_BY = model.LOADED_BY;
                existCL.LOAD_DATE = model.LOAD_DATE;
                dal.Update(existCL);
            }
        }

        public void Delete(string token)
        {
            ITT_CUST_LOAD model = dal.GetCustLoad(decimal.Parse(token));
            dal.Delete(model);
        }

    }
}

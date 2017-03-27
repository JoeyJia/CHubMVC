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

        public ITT_TRAN_LOAD GetTranLoadByInvoice(string invoiceNo)
        {
            return dal.GetTranLoadByInvoice(invoiceNo);
        }

        public void Save(ITT_TRAN_LOAD model)
        {
            if (model.LOAD_BATCH_TOKEN == 0 && !dal.ExistInvoiceNo(model.INVOICE_NO))
            {
                model.LOAD_BATCH_TOKEN = dal.GetTranLoadSqeNextVal();
                dal.Add(model);
            }
            else
            {
                //override existTL, but if a property in model(new) has no value, user existTL proerty value
                ITT_TRAN_LOAD existTL = dal.GetTranLoadByInvoice(model.INVOICE_NO);
                existTL.WILL_BILL_NO = model.WILL_BILL_NO;
                existTL.DEPART_DATE = model.DEPART_DATE != null ? model.DEPART_DATE : existTL.DEPART_DATE;
                existTL.ARRIVAL_DATE = model.ARRIVAL_DATE != null ? model.ARRIVAL_DATE : existTL.ARRIVAL_DATE;
                existTL.NOTE = !string.IsNullOrEmpty(model.NOTE) ? model.NOTE : existTL.NOTE;
                existTL.TRAN_TYPE = !string.IsNullOrEmpty(model.TRAN_TYPE) ? model.TRAN_TYPE : existTL.TRAN_TYPE;
                existTL.FROM_SYSTEM = !string.IsNullOrEmpty(model.FROM_SYSTEM) ? model.FROM_SYSTEM : existTL.FROM_SYSTEM;
                existTL.LOADED_BY = model.LOADED_BY;
                existTL.LOAD_DATE = model.LOAD_DATE;
                dal.Update(existTL);
            }
        }

        public void Delete(string token)
        {
            ITT_TRAN_LOAD model = dal.GetTranLoad(decimal.Parse(token));
            dal.Delete(model);
        }

        public bool ExistInvoiceNo(string invoiceNo)
        {
            return dal.ExistInvoiceNo(invoiceNo);
        }

    }
}

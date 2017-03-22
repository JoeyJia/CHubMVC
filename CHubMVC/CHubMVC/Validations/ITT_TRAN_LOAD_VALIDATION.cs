using CHubBLL;
using CHubDBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHubMVC.Validations
{
    public class ITT_TRAN_LOAD_VALIDATION
    {
        private ITT_TRAN_LOAD model;

        public ITT_TRAN_LOAD_VALIDATION(ITT_TRAN_LOAD tranLoad)
        {
            this.model = tranLoad;
        }

        public string ValidationAction()
        {
            if (model == null)
                return "Data is null";

            if (string.IsNullOrEmpty(model.WILL_BILL_NO)  || string.IsNullOrEmpty(model.FROM_SYSTEM))
                return "No Way Bill No or Form System Data";

            if (!string.IsNullOrEmpty(model.INVOICE_NO))
            {
                ITT_SHIPPING_D_BLL sdBLL = new ITT_SHIPPING_D_BLL();
                if (!sdBLL.ExistInvoiceNo(model.INVOICE_NO))
                    return "InvoiceNo. Not Exist";
            }

            if (model.DEPART_DATE != null && model.ARRIVAL_DATE != null)
            {
                if (DateTime.Compare(model.ARRIVAL_DATE.Value, model.DEPART_DATE.Value) < 0)
                    return "Depart Date need less than or equal Arrival Date";
            }
            return string.Empty;
        }

    }
}
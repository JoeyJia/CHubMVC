using CHubDBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHubMVC.Validations
{
    public class ITT_CUST_LOAD_VALICATION
    {
        private ITT_CUST_LOAD model;

        public ITT_CUST_LOAD_VALICATION(ITT_CUST_LOAD custLoad)
        {
            this.model = custLoad;
        }

        public string ValidationAction()
        {
            if (model == null)
                return "Data is null";
            if (string.IsNullOrEmpty(model.WILL_BILL_NO))
                return "No will Bill No";

            if (model.NBND_ARRIVAL_DATE != null && model.BND_OUT_DATE != null)
            {
                if (DateTime.Compare(model.NBND_ARRIVAL_DATE.Value, model.BND_OUT_DATE.Value) < 0)
                    return "Arrival Date(305) need no less than out Date(300)";
            }

            if (model.BND_OUT_DATE != null && model.BND_ARRIVAL_DATE != null)
            {
                if (DateTime.Compare(model.BND_OUT_DATE.Value, model.BND_ARRIVAL_DATE.Value) < 0)
                    return "Out Date(300) need no less than Arrival Date(300)";
            }

            if (model.BND_ARRIVAL_DATE != null && model.DO_RELEASE_DATE != null)
            {
                if (DateTime.Compare(model.BND_ARRIVAL_DATE.Value, model.DO_RELEASE_DATE.Value) < 0)
                    return "Arrival Date(300) need no less than Do Release Date";
            }

            return string.Empty;
        }

    }
}
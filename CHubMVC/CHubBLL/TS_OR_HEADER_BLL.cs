using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class TS_OR_HEADER_BLL
    {
        private readonly TS_OR_HEADER_DAL dal;

        public TS_OR_HEADER_BLL()
        {
            dal = new TS_OR_HEADER_DAL();
        }
        public TS_OR_HEADER_BLL(CHubEntities db)
        {
            dal = new TS_OR_HEADER_DAL(db);
        }

        private bool AddHeader(TS_OR_HEADER orHeader)
        {

            decimal nextVal = dal.GetOrderSqeNextVal();
            orHeader.ORDER_REQ_NO = nextVal;
            dal.Add(orHeader);
            return true;
        }

        public bool AddHeaderwithAlt(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader)
        {
            try
            {
                if (altORHeader == null)
                    return AddHeader(orHeader);
                else
                {
                    decimal nextVal = dal.GetOrderSqeNextVal();
                    orHeader.ORDER_REQ_NO = nextVal;
                    altORHeader.ORDER_REQ_NO = nextVal;

                    dal.Add(orHeader, false);
                    dal.Add(altORHeader, false);
                    dal.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}

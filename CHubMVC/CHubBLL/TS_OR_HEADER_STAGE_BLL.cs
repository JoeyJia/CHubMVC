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
    public class TS_OR_HEADER_STAGE_BLL
    {
        private readonly TS_OR_HEADER_STAGE_DAL dal;

        public TS_OR_HEADER_STAGE_BLL()
        {
            dal = new TS_OR_HEADER_STAGE_DAL();
        }
        public TS_OR_HEADER_STAGE_BLL(CHubEntities db)
        {
            dal = new TS_OR_HEADER_STAGE_DAL(db);
        }

        public TS_OR_HEADER_STAGE GetSpecifyHeaderStage(decimal orderSeq, decimal shipFromSeq)
        {
            return dal.GetSpecifyHeaderStage(orderSeq, shipFromSeq);
        }


        public decimal AddHeaderwithAltStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage)
        {
            try
            {
                decimal nextVal = dal.GetOrderSqeNextVal();
                orHeaderStage.ORDER_REQ_NO = nextVal;
                dal.Add(orHeaderStage, false);

                if (altORHeaderStage != null)
                {
                    altORHeaderStage.ORDER_REQ_NO = nextVal;
                    dal.Add(altORHeaderStage, false);
                }
                dal.SaveChanges();
                return nextVal;
            }
            catch
            {
                return 0;
            }
        }

        public decimal UpdateHeaderWithAltStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage)
        {
            try
            {
                dal.Update(orHeaderStage, false);
                if (altORHeaderStage != null)
                    dal.Update(altORHeaderStage, false);
                dal.SaveChanges();
                return orHeaderStage.ORDER_REQ_NO;
            }
            catch
            {
                return 0;
            }
        }


    }
}

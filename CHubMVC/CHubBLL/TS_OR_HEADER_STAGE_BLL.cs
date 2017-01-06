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


        public decimal AddHeadersWithDetailsStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage, List<TS_OR_DETAIL_STAGE> dStageList)
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
                //add Detail part
                if (dStageList != null && dStageList.Count > 0)
                {
                    foreach (var item in dStageList)
                    {
                        item.ORDER_REQ_NO = nextVal;
                        dal.Add(item, false);
                    }
                }

                dal.SaveChanges();
                return nextVal;
            }
            catch
            {
                return 0;
            }
        }

        public decimal UpdateHeadersWithDetailsStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage, List<TS_OR_DETAIL_STAGE> dStageList)
        {
            try
            {
                dal.Update(orHeaderStage, false);
                if (altORHeaderStage != null)
                    dal.Update(altORHeaderStage, false);

                //Update detail part
                if (dStageList != null && dStageList.Count > 0)
                {
                    foreach (var item in dStageList)
                    {
                        dal.Update(item, false);
                    }
                }

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

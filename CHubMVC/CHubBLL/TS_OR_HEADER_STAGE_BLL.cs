using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using static CHubCommon.CHubEnum;

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

        public List<TS_OR_HEADER_STAGE> GetHeaderStageBySeq(decimal orderSeq)
        {
            return dal.GetHeaderStageBySeq(orderSeq);
        }

        public List<TS_OR_HEADER_STAGE> GetHeaderStageByUser(string appUser)
        {
            return dal.GetHeaderStageByUser(appUser);
        }

        public void DeleteDraft(decimal orderSeq, decimal shipFrom = 0)
        {
            dal.DeleteDraft(orderSeq, shipFrom);
        }
        public decimal AddHeadersWithDetailsStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage, List<TS_OR_DETAIL_STAGE> dStageList)
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

        public decimal UpdateHeadersWithDetailsStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage, List<TS_OR_DETAIL_STAGE> dStageList)
        {
            dal.Update(orHeaderStage, false);
            if (altORHeaderStage != null)
                dal.AddOrUpdateHeaderStage(altORHeaderStage, false);
            else
            {
                //if from have alt header to no alt header, need to delete exist header items
                TS_OR_HEADER_STAGE existAlt = GetSpecifyHeaderStage(orHeaderStage.ORDER_REQ_NO, (decimal)ShipFromSeqEnum.Alternative);
                if (existAlt != null)
                    dal.Delete(existAlt,false);
            }

            //Update detail part
            TS_OR_DETAIL_STAGE_DAL dStageDal = new TS_OR_DETAIL_STAGE_DAL(dal.db);

            //Delete exist details
            List<TS_OR_DETAIL_STAGE> existDetails = dStageDal.GetDetailsStageByOrderSeq(orHeaderStage.ORDER_REQ_NO);
            foreach (var item in existDetails)
            {
                dal.Delete(item, false);
            }

            //Add current detail list
            if (dStageList != null && dStageList.Count > 0)
            {
                foreach (var item in dStageList)
                {
                    dal.Add(item, false);
                }
            }

            dal.SaveChanges();
            return orHeaderStage.ORDER_REQ_NO;
        }


    }
}

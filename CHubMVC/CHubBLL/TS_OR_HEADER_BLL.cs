using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using static CHubCommon.CHubEnum;
using System.Data.Entity;

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

        public List<TS_OR_HEADER> GetHeaders(decimal? orderSeq, string custAlias, string poNum, int currentPage, int pageSize, out int totalCount)
        {
            return dal.GetHeaders(orderSeq, custAlias, poNum,  currentPage,  pageSize, out totalCount);
        }

        public List<TS_OR_HEADER> GetHeadersBySeq(decimal orderSeq)
        {
            //need to consider stage or not
            return dal.GetHeadersBySeq(orderSeq);
        }


        public decimal AddHeadersWithDetails(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader, List<TS_OR_DETAIL> detailList,string DUE_DATE)
        {
            try
            {
                decimal nextVal = dal.GetOrderSqeNextVal();
                orHeader.ORDER_REQ_NO = nextVal;
                dal.Add(orHeader, false);

                if (altORHeader != null)
                {
                    altORHeader.ORDER_REQ_NO = nextVal;
                    dal.Add(altORHeader, false);
                }

                //detail part
                if (detailList != null && detailList.Count > 0)
                {
                    foreach (var item in detailList)
                    {
                        item.ORDER_REQ_NO = nextVal;
                        dal.Add(item, false);
                    }
                }

                dal.SaveChanges();

                if (detailList != null && detailList.Count > 0)
                {
                    foreach (var item in detailList)
                    {
                        item.ORDER_REQ_NO = nextVal;
                        dal.UpdateTS_OR_DETAIL_DUEDATE(item.ORDER_REQ_NO, item.ORDER_LINE_NO, DUE_DATE);
                    }
                }
                
                return nextVal;

            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// update header and detail ,need to delete stage date if exist stage data
        /// </summary>
        /// <param name="orHeader"></param>
        /// <param name="altORHeader"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public decimal UpdateHeadersWithDetails(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader, List<TS_OR_DETAIL> detailList,string DUE_DATE)
        {
            TS_OR_HEADER_STAGE_BLL hsBLL = new TS_OR_HEADER_STAGE_BLL(dal.db);
            //Or Header part => if exist stage =>delete stage and add header. if not exist stage => update header
            TS_OR_HEADER_STAGE headerStage = hsBLL.GetSpecifyHeaderStage(orHeader.ORDER_REQ_NO, orHeader.SHIPFROM_SEQ);
            if (headerStage != null)
            {
                //Must be save draft to save order
                dal.Delete(headerStage, false);
                dal.Add(orHeader, false);

                TS_OR_HEADER_STAGE altHeaderStage = hsBLL.GetSpecifyHeaderStage(orHeader.ORDER_REQ_NO, (decimal)ShipFromSeqEnum.Alternative);
                if (altHeaderStage != null)
                {
                    dal.Delete(altHeaderStage, false);

                }
                if (altORHeader != null)
                    dal.Add(altORHeader);

                TS_OR_DETAIL_STAGE_DAL dStageDal = new TS_OR_DETAIL_STAGE_DAL(dal.db);

                //Delete exist details
                List<TS_OR_DETAIL_STAGE> existDetails = dStageDal.GetDetailsStageByOrderSeq(orHeader.ORDER_REQ_NO);
                foreach (var item in existDetails)
                {
                    dal.Delete(item, false);
                }

            }
            else
            {
                //Must be override save order
                dal.Update(orHeader, false);
                if (altORHeader != null)
                    dal.AddOrUpdateHeader(altORHeader,false);
            }


            //Detail part
            TS_OR_DETAIL_DAL detailDal = new TS_OR_DETAIL_DAL(dal.db);
            //delete old part will insert new data
            List<TS_OR_DETAIL> oldDetails = detailDal.GetDetailsBySeq(orHeader.ORDER_REQ_NO);
            if (oldDetails != null && oldDetails.Count > 0)
            {
                foreach (var item in oldDetails)
                {
                    detailDal.Delete(item, false);
                }
            }

            if (detailList != null && detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    detailDal.Add(item, false);
                }
            }

            dal.SaveChanges();

            if (detailList != null && detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    detailDal.UpdateTS_OR_DETAIL_DUEDATE(item.ORDER_REQ_NO, item.ORDER_LINE_NO, DUE_DATE);
                }
            }
            
            return orHeader.ORDER_REQ_NO;
        }

    }
}

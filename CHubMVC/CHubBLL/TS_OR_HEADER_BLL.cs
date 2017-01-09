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


        public decimal AddHeadersWithDetails(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader, List<TS_OR_DETAIL> detailList)
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
        public decimal UpdateHeadersWithDetails(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader, List<TS_OR_DETAIL> detailList)
        {
            try
            {
                TS_OR_HEADER_STAGE_BLL hsBLL = new TS_OR_HEADER_STAGE_BLL(dal.db);
                //Or Header part => if exist stage =>delete stage and add header. if not exist stage => update header
                TS_OR_HEADER_STAGE headerStage = hsBLL.GetSpecifyHeaderStage(orHeader.ORDER_REQ_NO, orHeader.SHIPFROM_SEQ);
                if (headerStage != null)
                {
                    dal.Delete(headerStage, false);
                    dal.Add(orHeader, false);
                }
                else
                    dal.Update(orHeader, false);

                //Alt Or header part
                if (altORHeader != null)
                {
                    TS_OR_HEADER_STAGE altHeaderStage = hsBLL.GetSpecifyHeaderStage(altORHeader.ORDER_REQ_NO, altORHeader.SHIPFROM_SEQ);
                    if (altHeaderStage != null)
                    {
                        dal.Delete(altHeaderStage, false);
                        dal.Add(altORHeader);
                    }
                    else
                        dal.Update(altORHeader, false);
                }

                //Detail part
                if (detailList != null && detailList.Count > 0)
                {
                    TS_OR_DETAIL_STAGE_BLL dStageBLL = new TS_OR_DETAIL_STAGE_BLL(dal.db);
                    foreach (var item in detailList)
                    {
                        TS_OR_DETAIL_STAGE dStage = dStageBLL.GetSpecifyDetailStage(item.ORDER_REQ_NO, item.ORDER_LINE_NO);
                        if (dStage != null)
                        {
                            dal.Delete(dStage, false);
                            dal.Add(item, false);
                        }
                        else
                            dal.Update(item, false);

                    }
                }

                dal.SaveChanges();
                return orHeader.ORDER_REQ_NO;
            }
            catch
            {
                return 0;
            }
        }

    }
}

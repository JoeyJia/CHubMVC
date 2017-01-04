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


        public decimal AddHeaderwithAlt(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader)
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
                dal.SaveChanges();
                return nextVal;

            }
            catch
            {
                return 0;
            }
        }

        public decimal UpdateHeaderwithAlt(TS_OR_HEADER orHeader, TS_OR_HEADER altORHeader)
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

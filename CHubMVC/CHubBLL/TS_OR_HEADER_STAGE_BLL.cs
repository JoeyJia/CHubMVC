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

        private bool AddHeaderStage(TS_OR_HEADER_STAGE orHeaderStage)
        {
           
            decimal nextVal = dal.GetOrderSqeNextVal();
            orHeaderStage.ORDER_REQ_NO = nextVal;
            dal.Add(orHeaderStage);
            return true;
        }

        public bool AddHeaderwithAltStage(TS_OR_HEADER_STAGE orHeaderStage, TS_OR_HEADER_STAGE altORHeaderStage)
        {
            try
            {
                if (altORHeaderStage == null)
                    return AddHeaderStage(orHeaderStage);
                else
                {
                    decimal nextVal = dal.GetOrderSqeNextVal();
                    orHeaderStage.ORDER_REQ_NO = nextVal;
                    altORHeaderStage.ORDER_REQ_NO = nextVal;

                    dal.Add(orHeaderStage, false);
                    dal.Add(altORHeaderStage, false);
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

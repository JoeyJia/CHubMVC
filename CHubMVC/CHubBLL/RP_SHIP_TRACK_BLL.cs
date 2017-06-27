using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_SHIP_TRACK_BLL
    {
        private readonly RP_SHIP_TRACK_DAL dal;

        public RP_SHIP_TRACK_BLL()
        {
            dal = new RP_SHIP_TRACK_DAL();
        }
        public RP_SHIP_TRACK_BLL(CHubEntities db)
        {
            dal = new RP_SHIP_TRACK_DAL(db);
        }

        public void AddOrUpdate(RP_SHIP_TRACK model)
        {
            RP_SHIP_TRACK exist = dal.GetSpecifyTrack(model.WH_ID, model.SHIP_ID);
            if (exist != null)
            {
                exist.TRACK_NUM_IHUB = model.TRACK_NUM_IHUB;
                exist.RECORD_DATE = model.RECORD_DATE;
                exist.UPDATED_BY = model.UPDATED_BY;
                dal.Update(exist);
            }
            else
                dal.Add(model);
        }


    }
}

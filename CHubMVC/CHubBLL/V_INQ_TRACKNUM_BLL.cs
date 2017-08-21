using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using CHubModel.WebArg;

namespace CHubBLL
{
    public class V_INQ_TRACKNUM_BLL
    {
        private readonly V_INQ_TRACKNUM_DAL dal;

        public V_INQ_TRACKNUM_BLL()
        {
            dal = new V_INQ_TRACKNUM_DAL();
        }
        public V_INQ_TRACKNUM_BLL(CHubEntities db)
        {
            dal = new V_INQ_TRACKNUM_DAL(db);
        }

        public List<TrackNumLevel1> GetTrackNumLevel1(TrackNumQueryArg arg)
        {
            return dal.GetTrackNumLevel1(arg);
        }

        public List<TrackNumLevel2> GetTrackNumLevel2(string shipID, TrackNumQueryArg arg)
        {
            return dal.GetTrackNumLevel2(shipID,arg);
        }



    }
}

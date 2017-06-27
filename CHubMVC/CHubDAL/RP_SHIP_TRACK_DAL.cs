using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class RP_SHIP_TRACK_DAL : BaseDAL
    {
        public RP_SHIP_TRACK_DAL()
            : base() { }

        public RP_SHIP_TRACK_DAL(CHubEntities db)
            : base(db) { }


        public RP_SHIP_TRACK GetSpecifyTrack(string whID, string shipID)
        {
            return db.RP_SHIP_TRACK.FirstOrDefault(a => a.WH_ID == whID && a.SHIP_ID == shipID);
        }

    }
}

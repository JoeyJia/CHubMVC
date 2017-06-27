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

    }
}

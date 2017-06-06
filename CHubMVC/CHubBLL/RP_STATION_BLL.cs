using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_STATION_BLL
    {
        private readonly RP_STATION_DAL dal;

        public RP_STATION_BLL()
        {
            dal = new RP_STATION_DAL();
        }
        public RP_STATION_BLL(CHubEntities db)
        {
            dal = new RP_STATION_DAL(db);
        }

    }
}

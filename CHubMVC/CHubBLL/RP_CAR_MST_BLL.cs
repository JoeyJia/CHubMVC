using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_CAR_MST_BLL
    {
        private readonly RP_CAR_MST_DAL dal;

        public RP_CAR_MST_BLL()
        {
            dal = new RP_CAR_MST_DAL();
        }
        public RP_CAR_MST_BLL(CHubEntities db)
        {
            dal = new RP_CAR_MST_DAL(db);
        }

    }
}

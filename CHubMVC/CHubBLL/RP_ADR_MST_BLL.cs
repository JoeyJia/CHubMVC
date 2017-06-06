using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_ADR_MST_BLL
    {
        private readonly RP_ADR_MST_DAL dal;

        public RP_ADR_MST_BLL()
        {
            dal = new RP_ADR_MST_DAL();
        }
        public RP_ADR_MST_BLL(CHubEntities db)
        {
            dal = new RP_ADR_MST_DAL(db);
        }

    }
}

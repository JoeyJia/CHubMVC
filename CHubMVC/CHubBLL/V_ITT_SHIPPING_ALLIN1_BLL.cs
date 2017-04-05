using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_ITT_SHIPPING_ALLIN1_BLL
    {
        private readonly V_ITT_SHIPPING_ALLIN1_DAL dal;

        public V_ITT_SHIPPING_ALLIN1_BLL()
        {
            dal = new V_ITT_SHIPPING_ALLIN1_DAL();
        }
        public V_ITT_SHIPPING_ALLIN1_BLL(CHubEntities db)
        {
            dal = new V_ITT_SHIPPING_ALLIN1_DAL(db);
        }

    }
}

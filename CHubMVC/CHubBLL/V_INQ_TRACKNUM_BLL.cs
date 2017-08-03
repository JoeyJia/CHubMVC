using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

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

    }
}

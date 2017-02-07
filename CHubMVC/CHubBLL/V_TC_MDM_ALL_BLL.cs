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
    public class V_TC_MDM_ALL_BLL
    {
        private readonly V_TC_MDM_ALL_DAL dal;

        public V_TC_MDM_ALL_BLL()
        {
            dal = new V_TC_MDM_ALL_DAL();
        }
        public V_TC_MDM_ALL_BLL(CHubEntities db)
        {
            dal = new V_TC_MDM_ALL_DAL(db);
        }


    }
}

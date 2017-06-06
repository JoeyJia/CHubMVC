using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_SHIPMENT_H_RP_ALL_BLL
    {
        private readonly V_SHIPMENT_H_RP_ALL_DAL dal;

        public V_SHIPMENT_H_RP_ALL_BLL()
        {
            dal = new V_SHIPMENT_H_RP_ALL_DAL();
        }
        public V_SHIPMENT_H_RP_ALL_BLL(CHubEntities db)
        {
            dal = new V_SHIPMENT_H_RP_ALL_DAL(db);
        }

    }
}

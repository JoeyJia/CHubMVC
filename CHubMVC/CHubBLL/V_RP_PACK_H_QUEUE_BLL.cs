using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_RP_PACK_H_QUEUE_BLL
    {
        private readonly V_RP_PACK_H_QUEUE_DAL dal;

        public V_RP_PACK_H_QUEUE_BLL()
        {
            dal = new V_RP_PACK_H_QUEUE_DAL();
        }
        public V_RP_PACK_H_QUEUE_BLL(CHubEntities db)
        {
            dal = new V_RP_PACK_H_QUEUE_DAL(db);
        }

        public List<string> GetDistinctWHID()
        {
            return dal.GetDistinctWHID();
        }

        public List<string> GetShipIDByWhID(string whID)
        {
            return dal.GetShipIDByWhID(whID);
        }



    }
}

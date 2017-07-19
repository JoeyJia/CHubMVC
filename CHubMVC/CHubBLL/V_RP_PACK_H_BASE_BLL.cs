using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_RP_PACK_H_BASE_BLL
    {
        private readonly V_RP_PACK_H_BASE_DAL dal;

        public V_RP_PACK_H_BASE_BLL()
        {
            dal = new V_RP_PACK_H_BASE_DAL();
        }
        public V_RP_PACK_H_BASE_BLL(CHubEntities db)
        {
            dal = new V_RP_PACK_H_BASE_DAL(db);
        }

        public List<V_RP_PACK_H_BASE> GetPackList(string whID, string shipID, string custName, string address, bool staged, int range)
        {
            return dal.GetPackList(whID, shipID, custName, address, staged, range);
        }

        public List<string> GetStagedPackList()
        {
            return dal.GetStagedPackList();
        }

    }
}

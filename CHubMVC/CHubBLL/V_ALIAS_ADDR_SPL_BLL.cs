using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_ALIAS_ADDR_SPL_BLL
    {
        private readonly V_ALIAS_ADDR_SPL_DAL dal;

        public V_ALIAS_ADDR_SPL_BLL()
        {
            dal = new V_ALIAS_ADDR_SPL_DAL();
        }
        public V_ALIAS_ADDR_SPL_BLL(CHubEntities db)
        {
            dal = new V_ALIAS_ADDR_SPL_DAL(db);
        }

        public List<V_ALIAS_ADDR_SPL> GetAliasAddrSPL(string localDestName, string addr)
        {
            return dal.GetAliasAddrSPL(localDestName, addr);
        }

        public V_ALIAS_ADDR_SPL GetSpecifyAliasAddrSPL(string aliasName, string sysID, string cusNo, int? bill2Location, long? ship2Location, long? destLocation)
        {
            return dal.GetSpecifyAliasAddrSPL(aliasName, sysID, cusNo, bill2Location, ship2Location, destLocation);
        }

    }
}

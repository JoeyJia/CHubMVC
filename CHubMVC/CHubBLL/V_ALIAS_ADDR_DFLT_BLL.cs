using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_ALIAS_ADDR_DFLT_BLL
    {
        private readonly V_ALIAS_ADDR_DFLT_DAL dal;

        public V_ALIAS_ADDR_DFLT_BLL()
        {
            dal = new V_ALIAS_ADDR_DFLT_DAL();
        }
        public V_ALIAS_ADDR_DFLT_BLL(CHubEntities db)
        {
            dal = new V_ALIAS_ADDR_DFLT_DAL(db);
        }

        public List<V_ALIAS_ADDR_DFLT> GetAliasAddrDFLT(string shipName, string addr)
        {
            return dal.GetAliasAddrDFLT(shipName, addr);
        }

        public V_ALIAS_ADDR_DFLT GetSpecifyAliasAddrDFLT(string aliasName, string sysID, string cusNo, int? bill2Location, int? ship2Location)
        {
            return dal.GetSpecifyAliasAddrDFLT(aliasName, sysID, cusNo, bill2Location, ship2Location);
        }

    }
}

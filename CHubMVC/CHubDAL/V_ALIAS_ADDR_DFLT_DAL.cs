using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class V_ALIAS_ADDR_DFLT_DAL : BaseDAL
    {
        public V_ALIAS_ADDR_DFLT_DAL()
            : base() { }

        public V_ALIAS_ADDR_DFLT_DAL(CHubEntities db)
            : base(db) { }


        public List<V_ALIAS_ADDR_DFLT> GetAliasAddrDFLT(string shipName, string addr, string aliasName)
        {
            return db.V_ALIAS_ADDR_DFLT.Where(a => a.LOCAL_SHIP_TO_NAME.Contains(shipName)
                                                  && a.LOCAL_SHIP_TO_ADDR_1.Contains(addr)
                                                  && a.ALIAS_NAME == aliasName
                                                  && a.ACTIVEIND == CHubConstValues.IndY).OrderBy(a => a.DAYS).ToList();
        }

        public List<V_ALIAS_ADDR_DFLT> GetStrictAliasAddrDFLT(string shipName, string addr, string aliasName)
        {
            return db.V_ALIAS_ADDR_DFLT.Where(a => a.LOCAL_SHIP_TO_NAME == shipName
                                                  && a.LOCAL_SHIP_TO_ADDR_1 == addr
                                                  && a.ALIAS_NAME == aliasName
                                                  && a.ACTIVEIND == CHubConstValues.IndY).OrderBy(a => a.DAYS).ToList();
        }

        public V_ALIAS_ADDR_DFLT GetSpecifyAliasAddrDFLT(string aliasName,string sysID, string cusNo,int? bill2Location,int? ship2Location )
        {
            return db.V_ALIAS_ADDR_DFLT.FirstOrDefault(a => a.ALIAS_NAME == aliasName
            && a.SYSID == sysID
            && a.CUSTOMER_NO == cusNo
            && a.BILL_TO_LOCATION == bill2Location
            && a.SHIP_TO_LOCATION == ship2Location
            && a.ACTIVEIND == CHubConstValues.IndY
            );
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class V_ALIAS_ADDR_SPL_DAL : BaseDAL
    {
        public V_ALIAS_ADDR_SPL_DAL()
            : base() { }

        public V_ALIAS_ADDR_SPL_DAL(CHubEntities db)
            : base(db) { }


        public List<V_ALIAS_ADDR_SPL> GetAliasAddrSPL(string localDestName, string addr,string aliasName)
        {
            return db.V_ALIAS_ADDR_SPL.Where(a => a.LOCAL_DEST_NAME.Contains(localDestName)
                                               && a.LOCAL_DEST_ADDR_1.Contains(addr) 
                                               &&a.ALIAS_NAME == aliasName
                                               && a.ACTIVEIND== CHubConstValues.IndY).OrderBy(a=>a.DAYS).ToList();
        }

        public List<V_ALIAS_ADDR_SPL> GetStictAliasAddrSPL(string localDestName, string addr, string aliasName)
        {
            return db.V_ALIAS_ADDR_SPL.Where(a => a.LOCAL_DEST_NAME == localDestName 
                                                 && a.LOCAL_DEST_ADDR_1 == addr 
                                                 && a.ALIAS_NAME == aliasName
                                                 && a.ACTIVEIND == CHubConstValues.IndY).OrderBy(a => a.DAYS).ToList();
        }

        public V_ALIAS_ADDR_SPL GetSpecifyAliasAddrSPL(string aliasName,string sysID, string cusNo,int bill2Location,long ship2Location,long destLocation )
        {
            return db.V_ALIAS_ADDR_SPL.FirstOrDefault(a => a.ALIAS_NAME == aliasName
            && a.SYSID == sysID
            && a.CUSTOMER_NO == cusNo
            && a.BILL_TO_LOCATION == bill2Location
            && a.SHIP_TO_LOCATION == ship2Location
            &&a.DEST_LOCATION == destLocation
            && a.ACTIVEIND == CHubConstValues.IndY
            );
        }

    }
}

﻿using System;
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
            : base()
        { }

        public V_ALIAS_ADDR_SPL_DAL(CHubEntities db)
            : base(db)
        { }


        public List<V_ALIAS_ADDR_SPL> GetAliasAddrSPL(string localDestName, string addr, string destName, long? destLocation, string aliasName)
        {

            //IQueryable<V_ALIAS_ADDR_SPL> result = db.V_ALIAS_ADDR_SPL.Where(a => a.LOCAL_DEST_NAME.Contains(localDestName)
            //                                   && a.LOCAL_DEST_ADDR_1.Contains(addr)
            //                                   && a.DEST_NAME.Contains(destName)
            //                                   && a.ALIAS_NAME == aliasName
            //                                   && a.ACTIVEIND == CHubConstValues.IndY);
            string sql = string.Format(@"select * from V_ALIAS_ADDR_SPL where LOCAL_DEST_NAME like '%{0}%'
                                and LOCAL_DEST_ADDR_1 like '%{1}%' 
                                and DEST_NAME like '%{2}%'
                                and ALIAS_NAME = '{3}'
                                and ACTIVEIND = '{4}'", localDestName, addr, destName, aliasName, CHubConstValues.IndY);
            IEnumerable<V_ALIAS_ADDR_SPL> result = db.Database.SqlQuery<V_ALIAS_ADDR_SPL>(sql).AsEnumerable();
            
            if (destLocation != null && destLocation != 0)
                result = result.Where(a => a.DEST_LOCATION == destLocation.Value);

            //get from parameter table
            if (result.Count() > 20)
                throw new Exception(string.Format("Result has {0} items, Make Condition more strict", result.Count().ToString()));

            return result.OrderBy(a => a.DAYS).ToList();
        }

        public List<V_ALIAS_ADDR_SPL> GetStictAliasAddrSPL(string localDestName, string addr, string aliasName)
        {
            return db.V_ALIAS_ADDR_SPL.Where(a => a.LOCAL_DEST_NAME == localDestName
                                                 && a.LOCAL_DEST_ADDR_1 == addr
                                                 && a.ALIAS_NAME == aliasName
                                                 && a.ACTIVEIND == CHubConstValues.IndY).OrderBy(a => a.DAYS).ToList();
        }

        public V_ALIAS_ADDR_SPL GetSpecifyAliasAddrSPL(string aliasName, string sysID, string cusNo, int bill2Location, long ship2Location, long destLocation)
        {
            return db.V_ALIAS_ADDR_SPL.FirstOrDefault(a => a.ALIAS_NAME == aliasName
            && a.SYSID == sysID
            && a.CUSTOMER_NO == cusNo
            && a.BILL_TO_LOCATION == bill2Location
            && a.SHIP_TO_LOCATION == ship2Location
            && a.DEST_LOCATION == destLocation
            && a.ACTIVEIND == CHubConstValues.IndY
            );
        }

    }
}

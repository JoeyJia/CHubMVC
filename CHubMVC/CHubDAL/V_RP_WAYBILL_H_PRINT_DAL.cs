﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_RP_WAYBILL_H_PRINT_DAL : BaseDAL
    {
        public V_RP_WAYBILL_H_PRINT_DAL()
            : base() { }

        public V_RP_WAYBILL_H_PRINT_DAL(CHubEntities db)
            : base(db) { }

        public V_RP_WAYBILL_H_PRINT GetHByShipNo(string shipNo)
        {
            string sql = string.Format(@"select * from V_RP_WAYBILL_H_PRINT
 where  SHIP_ID='{0}'", shipNo);

            var result = db.Database.SqlQuery<V_RP_WAYBILL_H_PRINT>(sql);
            if(result!=null && result.Count()>0)
                return result.ToList()[0];
            return null;
        }

    }
}

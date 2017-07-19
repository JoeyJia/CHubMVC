using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_RP_PACK_H_BASE_DAL : BaseDAL
    {
        public V_RP_PACK_H_BASE_DAL()
            : base() { }

        public V_RP_PACK_H_BASE_DAL(CHubEntities db)
            : base(db) { }

        public List<V_RP_PACK_H_BASE> GetPackList(string whID,string shipID,string custName,string address,bool staged,int range)
        {
            string sql = string.Format(@"select * from V_RP_PACK_H_BASE 
where WH_ID='{0}'", whID);

            if (!string.IsNullOrEmpty(shipID))
                sql += string.Format(" and SHIP_ID='{0}' ",shipID);

            if (!string.IsNullOrEmpty(custName))
                sql += string.Format(" and ADRNAM like '%{0}%' ", custName);
            if (!string.IsNullOrEmpty(address))
                sql += string.Format(" and ADRLN1 like '%{0}%' ", address);

            if (staged)
                sql += string.Format(" and SHPSTS='S' ");
            else
                sql += string.Format(" and floor(sysdate - STGDTE) <= {0}",range);


            var result = db.Database.SqlQuery<V_RP_PACK_H_BASE>(sql);
            return result.OrderBy(a => a.SHIP_ID).ThenBy(a=>a.LODNUM).ToList();
            
        }

        public List<string> GetStagedPackList()
        {
            string sql = @"select SHIP_ID from V_RP_PACK_H_BASE where SHPSTS='S' and nvl(SUCCEE_FLAG,'N')!='Y' ";

            var result = db.Database.SqlQuery<string>(sql);
            return result.ToList();

        }

    }
}

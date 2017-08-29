using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;

namespace CHubDAL
{
    public class GOMS_ASN_H_DAL : BaseDAL
    {
        public GOMS_ASN_H_DAL()
            : base() { }

        public GOMS_ASN_H_DAL(CHubEntities db)
            : base(db) { }

        public List<GOMS_ASN_H> GetDockData(string wareHouse, string from,string asn,int range)
        {
            string sql = string.Format(@"select * from GOMS_ASN_H 
where WAREHOUSE='{0}'
and floor(sysdate - CREATE_DATE) <= {1}
and IHUB_DOCK='Y' ", wareHouse,range);

            if (!string.IsNullOrEmpty(from))
                sql += string.Format(" and BUY_FROM_COMPANY='{0}' ", from);

            if (!string.IsNullOrEmpty(asn))
                sql += string.Format(" and MANIFEST_ID='{0}' ", asn);

            var result = db.Database.SqlQuery<GOMS_ASN_H>(sql);
            return result.OrderByDescending(a=>a.CREATE_DATE).ToList();

            //var result = (from a in db.GOMS_ASN_H
            //              where a.WAREHOUSE== wareHouse

            //              select a);//&& DbFunctions.AddDays(a.CREATE_DATE,9)>DateTime.Now
            ////result = result.Where(a => DbFunctions.AddDays(a.CREATE_DATE, range) >= DateTime.Now );
            //result = result.Where(a => DbFunctions.DiffDays(DateTime.Now, a.CREATE_DATE) <= range);

            //if (!string.IsNullOrEmpty(from))
            //    result = result.Where(a => a.BUY_FROM_COMPANY == from);
            //if (!string.IsNullOrEmpty(asn))
            //    result = result.Where(a => a.MANIFEST_ID == asn);

            //return result.OrderByDescending(a=>a.CREATE_DATE).ToList();
        }

    }
}

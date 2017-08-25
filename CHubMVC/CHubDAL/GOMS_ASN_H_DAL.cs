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
            var result = (from a in db.GOMS_ASN_H
                          where a.WAREHOUSE== wareHouse

                          select a);
            result = result.Where(a => DbFunctions.DiffDays(DateTime.Now, a.CREATE_DATE) <= range);

            if (!string.IsNullOrEmpty(from))
                result = result.Where(a => a.BUY_FROM_COMPANY == from);
            if (!string.IsNullOrEmpty(asn))
                result = result.Where(a => a.MANIFEST_ID == asn);

            return result.OrderByDescending(a=>a.CREATE_DATE).ToList();
        }

    }
}

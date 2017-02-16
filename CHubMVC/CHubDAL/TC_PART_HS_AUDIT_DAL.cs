using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;
using System.Data.Entity;

namespace CHubDAL
{
    public class TC_PART_HS_AUDIT_DAL : BaseDAL
    {
        public TC_PART_HS_AUDIT_DAL()
            : base() { }

        public TC_PART_HS_AUDIT_DAL(CHubEntities db)
            : base(db) { }

        public List<TC_PART_HS_AUDIT> GetAuditLog(string partNo)
        {
            List<TC_PART_HS_AUDIT> list =  db.TC_PART_HS_AUDIT.Where(a => a.PART_NO == partNo && DbFunctions.AddMonths(a.ACTIVITY_DATE, 12) > DateTime.Now).ToList();
            return list;
        }

    }
}

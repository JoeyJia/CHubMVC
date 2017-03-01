using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class TC_PART_HS_DAL : BaseDAL
    {
        public TC_PART_HS_DAL()
            : base() { }

        public TC_PART_HS_DAL(CHubEntities db)
            : base(db) { }

        public bool Exist(string partNo)
        {
            return db.TC_PART_HS.Any(a => a.PART_NO == partNo);
        }

        public TC_PART_HS GetTCPartHS(string partNo)
        {
            return db.TC_PART_HS.FirstOrDefault(a => a.PART_NO == partNo);
        }

    }
}

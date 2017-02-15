using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class M_PART_DAL : BaseDAL
    {
        public M_PART_DAL()
            : base() { }

        public M_PART_DAL(CHubEntities db)
            : base(db) { }

        public bool Exist(string partNo)
        {
            return db.M_PART.Any(a => a.PART_NO == partNo);
        }

        public M_PART GetMPartByPartNo(string partNo)
        {
            return db.M_PART.FirstOrDefault(a => a.PART_NO == partNo);
        }
    }
}

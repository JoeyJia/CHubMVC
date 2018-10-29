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
            : base()
        { }

        public M_PART_DAL(CHubEntities db)
            : base(db)
        { }

        public bool Exist(string partNo)
        {
            return db.M_PART.Any(a => a.PART_NO == partNo);
        }

        public M_PART GetMPartByPartNo(string partNo)
        {
            return db.M_PART.FirstOrDefault(a => a.PART_NO == partNo);
        }

        public List<TC_PART_HS> CopySearch(string PART_NO)
        {
            string sql = string.Format(@"select * from TC_PART_HS where PART_NO like '%{0}%'", PART_NO);
            var result = db.Database.SqlQuery<TC_PART_HS>(sql).ToList();
            return result;
        }

        public List<V_TC_MDM_ALL> CopyData(string PART_NO)
        {
            string sql = string.Format(@"select * from V_TC_MDM_ALL where PART_NO='{0}'", PART_NO);
            var result = db.Database.SqlQuery<V_TC_MDM_ALL>(sql).ToList();
            return result;
        }
    }
}

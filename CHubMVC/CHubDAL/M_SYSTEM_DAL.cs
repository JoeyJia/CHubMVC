using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class M_SYSTEM_DAL : BaseDAL
    {
        public M_SYSTEM_DAL()
            : base() { }

        public M_SYSTEM_DAL(CHubEntities db)
            : base(db) { }

        public List<string> GetITTSysIDList()
        {
            var result = (from m in db.M_SYSTEM
                where m.INTERPDC_FLAG =="Y"
                select m.SYSID
                );
            return result.ToList();
        }

    }
}

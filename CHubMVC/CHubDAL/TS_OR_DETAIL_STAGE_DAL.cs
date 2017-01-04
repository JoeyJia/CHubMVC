using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;
using CHubCommon;

namespace CHubDAL
{
    public class TS_OR_DETAIL_STAGE_DAL : BaseDAL
    {
        public TS_OR_DETAIL_STAGE_DAL()
            : base() { }

        public TS_OR_DETAIL_STAGE_DAL(CHubEntities db)
            : base(db) { }

    }
}

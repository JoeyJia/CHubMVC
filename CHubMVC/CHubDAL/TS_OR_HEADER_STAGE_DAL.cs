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
    public class TS_OR_HEADER_STAGE_DAL : BaseDAL
    {
        public TS_OR_HEADER_STAGE_DAL()
            : base() { }

        public TS_OR_HEADER_STAGE_DAL(CHubEntities db)
            : base(db) { }

        public TS_OR_HEADER_STAGE GetSpecifyHeaderStage(decimal orderSeq,decimal shipFromSeq)
        {
            return db.TS_OR_HEADER_STAGE.FirstOrDefault(a => a.ORDER_REQ_NO == orderSeq && a.SHIPFROM_SEQ == shipFromSeq);
        }


    }
}

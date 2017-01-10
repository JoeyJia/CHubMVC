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
    public class TS_OR_HEADER_DAL : BaseDAL
    {
        public TS_OR_HEADER_DAL()
            : base() { }

        public TS_OR_HEADER_DAL(CHubEntities db)
            : base(db) { }

        public void AddOrUpdateHeader(TS_OR_HEADER header, bool autoSave = true)
        {
            if (db.TS_OR_HEADER.Any(a => a.ORDER_REQ_NO == header.ORDER_REQ_NO && a.SHIPFROM_SEQ == header.SHIPFROM_SEQ))
                Update(header, autoSave);
            else
                Add(header, autoSave);
        }


    }
}

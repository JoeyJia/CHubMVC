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
    public class TS_OR_DETAIL_DAL : BaseDAL
    {
        public TS_OR_DETAIL_DAL()
            : base() { }

        public TS_OR_DETAIL_DAL(CHubEntities db)
            : base(db) { }

        public void AddOrUpdateDetail(TS_OR_DETAIL model,bool autoSave=true)
        {
            if (db.TS_OR_DETAIL.Any(a => a.ORDER_REQ_NO == model.ORDER_REQ_NO && a.ORDER_LINE_NO == model.ORDER_LINE_NO))
                Update(model, autoSave);
            else
                Add(model, autoSave);
        }

    }
}

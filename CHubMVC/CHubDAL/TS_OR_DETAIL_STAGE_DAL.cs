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


        public void AddOrUpdateDetailStage(TS_OR_DETAIL_STAGE model,bool autoSave = true)
        {
            if (db.TS_OR_DETAIL_STAGE.Any(a => a.ORDER_REQ_NO == model.ORDER_REQ_NO && a.ORDER_LINE_NO == model.ORDER_LINE_NO))
                Update(model,autoSave);
            else
                Add(model,autoSave);
        }

        public TS_OR_DETAIL_STAGE GetSpecifyDetailStage(decimal seq, decimal lineNO)
        {
            return db.TS_OR_DETAIL_STAGE.FirstOrDefault(a => a.ORDER_REQ_NO == seq && a.ORDER_LINE_NO == lineNO);
        }

        public List<TS_OR_DETAIL_STAGE> GetDetailsStageByOrderSeq(decimal seq)
        {
            return db.TS_OR_DETAIL_STAGE.Where(a => a.ORDER_REQ_NO == seq ).ToList();
        }

    }
}

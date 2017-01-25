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

        public List<TS_OR_HEADER_STAGE> GetHeaderStageBySeq(decimal orderSeq)
        {
            return db.TS_OR_HEADER_STAGE.Where(a => a.ORDER_REQ_NO == orderSeq).ToList();
        }

        /// <summary>
        /// mainly for alt addr using
        /// </summary>
        /// <param name="hStage"></param>
        /// <param name="autoSave"></param>
        public void AddOrUpdateHeaderStage(TS_OR_HEADER_STAGE hStage, bool autoSave = true)
        {
            if (db.TS_OR_HEADER_STAGE.Any(a => a.ORDER_REQ_NO == hStage.ORDER_REQ_NO && a.SHIPFROM_SEQ == hStage.SHIPFROM_SEQ))
                Update(hStage, autoSave);
            else
                Add(hStage, autoSave);
        }


        public List<TS_OR_HEADER_STAGE> GetHeaderStageByUser(string appUser)
        {
            return db.TS_OR_HEADER_STAGE.Where(a => a.CREATED_BY == appUser&&a.SHIPFROM_SEQ==0).OrderBy(a => a.ORDER_REQ_NO).ToList();
        }

        public void DeleteDraft(decimal orderSeq, decimal shipFrom = 0)
        {
            TS_OR_HEADER_STAGE hStage = db.TS_OR_HEADER_STAGE.FirstOrDefault(a => a.ORDER_REQ_NO == orderSeq && a.SHIPFROM_SEQ == shipFrom);
            if (hStage != null)
                Delete(hStage);
        }

    }
}

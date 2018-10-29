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
        private CHubCommonHelper ccHelper;
        public TS_OR_DETAIL_DAL()
            : base() {
            ccHelper = new CHubCommonHelper();
        }

        public TS_OR_DETAIL_DAL(CHubEntities db)
            : base(db) {
            ccHelper = new CHubCommonHelper();
        }

        public void AddOrUpdateDetail(TS_OR_DETAIL model,bool autoSave=true)
        {
            if (db.TS_OR_DETAIL.Any(a => a.ORDER_REQ_NO == model.ORDER_REQ_NO && a.ORDER_LINE_NO == model.ORDER_LINE_NO))
                Update(model, autoSave);
            else
                Add(model, autoSave);
        }

        public List<TS_OR_DETAIL> GetDetailsBySeq(decimal orderSeq)
        {
            return db.TS_OR_DETAIL.Where(a => a.ORDER_REQ_NO == orderSeq).OrderBy(a => a.ORDER_LINE_NO).ToList();
        }

        public void UpdateTS_OR_DETAIL_DUEDATE(decimal ORDER_REQ_NO, decimal ORDER_LINE_NO, string DUE_DATE)
        {
            string sql = string.Format(@"Update TS_OR_DETAIL set DUE_DATE=to_date('{0}','yyyy/mm/dd') where ORDER_REQ_NO='{1}' and ORDER_LINE_NO='{2}'", DUE_DATE, ORDER_REQ_NO, ORDER_LINE_NO);
            ccHelper.Update(sql);
        }

    }
}

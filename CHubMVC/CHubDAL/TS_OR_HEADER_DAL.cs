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

        public List<TS_OR_HEADER> GetHeaders(decimal? orderSeq, string custAlias, string poNum,int currentPage,int pageSize, out int totalCount)
        {
            IQueryable<TS_OR_HEADER> result = db.TS_OR_HEADER;
            if (orderSeq != null && orderSeq != 0)
                result = result.Where(a => a.ORDER_REQ_NO == orderSeq);
            if (!string.IsNullOrEmpty(custAlias))
                result = result.Where(a => a.ALIAS_NAME == custAlias);
            if (!string.IsNullOrEmpty(poNum))
                result = result.Where(a => a.CUSTOMER_PO_NO == poNum);

            totalCount = result.Count();
             return result.OrderBy(a => a.ORDER_REQ_NO).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<TS_OR_HEADER> GetHeadersBySeq(decimal orderSeq)
        {
            return db.TS_OR_HEADER.Where(a => a.ORDER_REQ_NO == orderSeq).ToList();
        }

    }
}

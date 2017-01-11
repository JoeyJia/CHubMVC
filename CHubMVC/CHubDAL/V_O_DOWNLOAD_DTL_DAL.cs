using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class V_O_DOWNLOAD_DTL_DAL : BaseDAL
    {
        public V_O_DOWNLOAD_DTL_DAL()
            : base() { }

        public V_O_DOWNLOAD_DTL_DAL(CHubEntities db)
            : base(db) { }

        public List<V_O_DOWNLOAD_DTL> GetDTLList(decimal orderSeq, decimal shipFrom)
        {
            return db.V_O_DOWNLOAD_DTL.Where(a => a.ORDER_REQ_NO == orderSeq && a.SHIPFROM_SEQ == shipFrom).OrderBy(a=>a.ORDER_LINE_NO).ToList();
        }

    }
}

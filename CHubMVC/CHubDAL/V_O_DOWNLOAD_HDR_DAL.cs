using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class V_O_DOWNLOAD_HDR_DAL : BaseDAL
    {
        public V_O_DOWNLOAD_HDR_DAL()
            : base() { }

        public V_O_DOWNLOAD_HDR_DAL(CHubEntities db)
            : base(db) { }


        public V_O_DOWNLOAD_HDR GetSpecfyHDRData(decimal orderSeq, decimal shipFrom)
        {
            return db.V_O_DOWNLOAD_HDR.FirstOrDefault(a => a.ORDER_REQ_NO == orderSeq && a.SHIPFROM_SEQ == shipFrom);
        }

    }
}

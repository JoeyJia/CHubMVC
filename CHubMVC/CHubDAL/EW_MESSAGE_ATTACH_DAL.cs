using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_MESSAGE_ATTACH_DAL : BaseDAL
    {
        public EW_MESSAGE_ATTACH_DAL()
            : base() { }

        public EW_MESSAGE_ATTACH_DAL(CHubEntities db)
            : base(db) { }

        public List<EW_MESSAGE_ATTACH> GetAttachByMsgID(string msgID)
        {
            return db.EW_MESSAGE_ATTACH.Where(a => a.MESSAGE_ID == msgID).ToList();
        }

    }
}

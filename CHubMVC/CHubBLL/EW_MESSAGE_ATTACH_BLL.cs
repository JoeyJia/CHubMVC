using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_MESSAGE_ATTACH_BLL
    {
        private readonly EW_MESSAGE_ATTACH_DAL dal;

        public EW_MESSAGE_ATTACH_BLL()
        {
            dal = new EW_MESSAGE_ATTACH_DAL();
        }
        public EW_MESSAGE_ATTACH_BLL(CHubEntities db)
        {
            dal = new EW_MESSAGE_ATTACH_DAL(db);
        }

        public List<EW_MESSAGE_ATTACH> GetAttachByMsgID(string msgID)
        {
            return dal.GetAttachByMsgID(msgID);
        }
    }
}

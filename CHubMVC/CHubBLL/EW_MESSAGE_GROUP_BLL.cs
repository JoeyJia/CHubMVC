using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_MESSAGE_GROUP_BLL
    {
        private readonly EW_MESSAGE_GROUP_DAL dal;

        public EW_MESSAGE_GROUP_BLL()
        {
            dal = new EW_MESSAGE_GROUP_DAL();
        }
        public EW_MESSAGE_GROUP_BLL(CHubEntities db)
        {
            dal = new EW_MESSAGE_GROUP_DAL(db);
        }

        public List<EW_MESSAGE_GROUP> GetMsgGroups()
        {
            return dal.GetMsgGroups();
        }

    }
}

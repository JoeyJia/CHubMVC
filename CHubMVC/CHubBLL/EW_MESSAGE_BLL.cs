using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_MESSAGE_BLL
    {
        private readonly EW_MESSAGE_DAL dal;

        public EW_MESSAGE_BLL()
        {
            dal = new EW_MESSAGE_DAL();
        }
        public EW_MESSAGE_BLL(CHubEntities db)
        {
            dal = new EW_MESSAGE_DAL(db);
        }

        public List<EW_MESSAGE> GetAllMsg()
        {
            return dal.GetAllMsg();
        }

        public EW_MESSAGE GetMsgByID(string id)
        {
            return dal.GetMsgByID(id);
        }

    }
}

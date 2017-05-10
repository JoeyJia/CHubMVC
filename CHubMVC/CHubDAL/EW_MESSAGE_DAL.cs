using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_MESSAGE_DAL : BaseDAL
    {
        public EW_MESSAGE_DAL()
            : base() { }

        public EW_MESSAGE_DAL(CHubEntities db)
            : base(db) { }

        public List<EW_MESSAGE> GetAllMsg()
        {
            return db.EW_MESSAGE.ToList();
        }

        public EW_MESSAGE GetMsgByID(string id)
        {
            return db.EW_MESSAGE.FirstOrDefault(a => a.MESSAGE_ID == id);
        }
    }
}

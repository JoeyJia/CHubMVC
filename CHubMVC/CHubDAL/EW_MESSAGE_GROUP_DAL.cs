using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_MESSAGE_GROUP_DAL : BaseDAL
    {
        public EW_MESSAGE_GROUP_DAL()
            : base() { }

        public EW_MESSAGE_GROUP_DAL(CHubEntities db)
            : base(db) { }


        public List<EW_MESSAGE_GROUP> GetMsgGroups()
        {
            return db.EW_MESSAGE_GROUP.ToList();
        }

    }
}

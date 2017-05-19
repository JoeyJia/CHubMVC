using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

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

        public List<ExEWMessage> GetMsgByGroup(string ewGroup,string appUser)
        {
            
            var result = (
                from m in db.EW_MESSAGE
                let apply=
                (
                    from a in db.EW_USER_APPLY
                    where m.MESSAGE_ID==a.MESSAGE_ID
                    && a.APP_USER==appUser
                    select a.APPLY
                ).FirstOrDefault()
                where m.EW_GROUP == ewGroup
                select new ExEWMessage {
                    MESSAGE_ID = m.MESSAGE_ID,
                    MESSAGE_DESC = m.MESSAGE_DESC,
                    MESSAGE_DESC_SHORT = m.MESSAGE_DESC_SHORT,
                    OWNER = m.OWNER,
                    Apply= apply
                }
                ).OrderBy(a=>a.MESSAGE_ID);
            return result.ToList() ;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_USER_APPLY_DAL : BaseDAL
    {
        public EW_USER_APPLY_DAL()
            : base() { }

        public EW_USER_APPLY_DAL(CHubEntities db)
            : base(db) { }


        public List<EW_USER_APPLY> GetUserApplyByGroup(string group, string appUser)
        {
            var result = (
                from a in db.EW_USER_APPLY
                join m in db.EW_MESSAGE on a.MESSAGE_ID equals m.MESSAGE_ID
                where m.EW_GROUP==@group
                && a.APP_USER==appUser
                select a
                );

            return result.ToList();
        }

        public List<string> GetApplyUsersMail(string messageID)
        {
            var result = (
                from a in db.EW_USER_APPLY
                join u in db.APP_USERS on a.APP_USER equals u.APP_USER
                where a.MESSAGE_ID==messageID
                select
                u.EMAIL_ADDR== null?u.APP_USER:u.EMAIL_ADDR
                );
            return result.ToList();
        }

        public EW_USER_APPLY GetSpecifyUserApply(string messageID, string appUser)
        {
            return db.EW_USER_APPLY.Where(a => a.MESSAGE_ID == messageID && a.APP_USER == appUser).FirstOrDefault();
        }

    }
}

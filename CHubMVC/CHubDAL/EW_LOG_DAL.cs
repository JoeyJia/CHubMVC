using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;
using CHubCommon;

namespace CHubDAL
{
    public class EW_LOG_DAL : BaseDAL
    {
        public EW_LOG_DAL()
            : base() { }

        public EW_LOG_DAL(CHubEntities db)
            : base(db) { }

        public bool HasExecuted(string msgID)
        {
            EW_LOG log = db.EW_LOG.FirstOrDefault(a => a.MESSAGE_ID == msgID && DbFunctions.DiffDays(a.LOG_DATE, DateTime.Now) == 0);
            
            if (log!=null && log.STATUS == CHubConstValues.IndY)
                return true;
            return false;
        }

        public EW_LOG GetLog(string msgID, DateTime date)
        {
            EW_LOG log = db.EW_LOG.FirstOrDefault(a => a.MESSAGE_ID == msgID && DbFunctions.DiffDays(a.LOG_DATE, date) == 0);
            return log;
        }


    }
}

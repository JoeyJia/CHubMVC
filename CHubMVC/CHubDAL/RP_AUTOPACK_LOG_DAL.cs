using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class RP_AUTOPACK_LOG_DAL : BaseDAL
    {
        public RP_AUTOPACK_LOG_DAL()
            : base() { }

        public RP_AUTOPACK_LOG_DAL(CHubEntities db)
            : base(db) { }

        public bool HasSuccessPrint(string lodNum)
        {
            RP_AUTOPACK_LOG exist = db.RP_AUTOPACK_LOG.FirstOrDefault(a => a.LODNUM == lodNum);
            if (exist == null)
                return false;
            return (exist.SUCCEE_FLAG ?? "N") == CHubCommon.CHubConstValues.IndY;
        }

        public RP_AUTOPACK_LOG GetSpecifyLog(string lodNum)
        {
            return db.RP_AUTOPACK_LOG.FirstOrDefault(a => a.LODNUM == lodNum);
        }

    }
}

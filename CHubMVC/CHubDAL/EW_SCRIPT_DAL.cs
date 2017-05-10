using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class EW_SCRIPT_DAL : BaseDAL
    {
        public EW_SCRIPT_DAL()
            : base() { }

        public EW_SCRIPT_DAL(CHubEntities db)
            : base(db) { }


        public EW_SCRIPT GetScriptByID(string id)
        {
            return db.EW_SCRIPT.FirstOrDefault(a => a.SCRIPT_ID == id);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class EW_SCRIPT_BLL
    {
        private readonly EW_SCRIPT_DAL dal;

        public EW_SCRIPT_BLL()
        {
            dal = new EW_SCRIPT_DAL();
        }
        public EW_SCRIPT_BLL(CHubEntities db)
        {
            dal = new EW_SCRIPT_DAL(db);
        }

        public EW_SCRIPT GetScriptByID(string id)
        {
            return dal.GetScriptByID(id);
        }
    }
}

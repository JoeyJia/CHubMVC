using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class M_APPS_DAL : BaseDAL
    {
        public M_APPS_DAL()
            : base() { }

        public M_APPS_DAL(CHubEntities db)
            : base(db) { }

        public List<M_APPS> GetMAppsList()
        {
            return db.M_APPS.Where(a=>a.ACTIVEIND=="Y").OrderBy(a=>a.APP_DISPLAY).ToList();
        }


    }
}

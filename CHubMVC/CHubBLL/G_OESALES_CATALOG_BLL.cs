using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class G_OESALES_CATALOG_BLL
    {
        private readonly G_OESALES_CATALOG_DAL dal;

        public G_OESALES_CATALOG_BLL()
        {
            dal = new G_OESALES_CATALOG_DAL();
        }
        public G_OESALES_CATALOG_BLL(CHubEntities db)
        {
            dal = new G_OESALES_CATALOG_DAL(db);
        }

        public G_OESALES_CATALOG GetOESalesCatalog(string sysId, string partNo)
        {
            return dal.GetOESalesCatalog(sysId, partNo);
        }

    }
}

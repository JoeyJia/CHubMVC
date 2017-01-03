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
    public class G_NETAVL_BLL
    {
        private readonly G_NETAVL_DAL dal;

        public G_NETAVL_BLL()
        {
            dal = new G_NETAVL_DAL();
        }
        public G_NETAVL_BLL(CHubEntities db)
        {
            dal = new G_NETAVL_DAL(db);
        }


    }
}

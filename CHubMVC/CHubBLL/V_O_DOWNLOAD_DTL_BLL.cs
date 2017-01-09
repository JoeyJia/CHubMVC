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
    public class V_O_DOWNLOAD_DTL_BLL
    {
        private readonly V_O_DOWNLOAD_DTL_DAL dal;

        public V_O_DOWNLOAD_DTL_BLL()
        {
            dal = new V_O_DOWNLOAD_DTL_DAL();
        }
        public V_O_DOWNLOAD_DTL_BLL(CHubEntities db)
        {
            dal = new V_O_DOWNLOAD_DTL_DAL(db);
        }



    }
}

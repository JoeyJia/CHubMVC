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
    public class V_O_DOWNLOAD_HDR_BLL
    {
        private readonly V_O_DOWNLOAD_HDR_DAL dal;

        public V_O_DOWNLOAD_HDR_BLL()
        {
            dal = new V_O_DOWNLOAD_HDR_DAL();
        }
        public V_O_DOWNLOAD_HDR_BLL(CHubEntities db)
        {
            dal = new V_O_DOWNLOAD_HDR_DAL(db);
        }

        public V_O_DOWNLOAD_HDR GetSpecfyHDRData(decimal orderSeq, decimal shipFrom)
        {
            return dal.GetSpecfyHDRData(orderSeq, shipFrom);
        }

    }
}

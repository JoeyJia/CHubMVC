using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubModel.WebArg;
using CHubDBEntity.UnmanagedModel;
using System.Data;

namespace CHubBLL
{
    public class XX_ASNDIFF_RESULT_CODE_BLL
    {
        private XX_ASNDIFF_RESULT_CODE_DAL dal;
        public XX_ASNDIFF_RESULT_CODE_BLL()
        {
            dal = new XX_ASNDIFF_RESULT_CODE_DAL();
        }

        public List<XX_ASNDIFF_RESULT_CODE> GetXXASNDIFFRESULTCODEList()
        {
            return dal.GetASNDIFFRESULTCODEList();
        }
    }
}
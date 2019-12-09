using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class XX_ASNDIFF_RESULT_CODE_DAL : BaseDAL
    {
        private CHubCommonHelper ccHelper;
        public XX_ASNDIFF_RESULT_CODE_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<XX_ASNDIFF_RESULT_CODE> GetASNDIFFRESULTCODEList()
        {
            string sql = string.Format(@"select * from XX_ASNDIFF_RESULT_CODE where ACTIVEING ='1' ");
            var result = ccHelper.ExecuteSqlToList<XX_ASNDIFF_RESULT_CODE>(sql);
            return result;
        }
        
    }
}


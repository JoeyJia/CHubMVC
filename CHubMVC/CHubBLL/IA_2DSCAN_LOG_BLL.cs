using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class IA_2DSCAN_LOG_BLL
    {
        private IA_2DSCAN_LOG_DAL dal;
        public IA_2DSCAN_LOG_BLL()
        {
            dal = new IA_2DSCAN_LOG_DAL();
        }

        public IA_2DSCAN_LOG_BLL(CHubEntities db)
        {
            dal = new IA_2DSCAN_LOG_DAL(db);
        }

        public string GetConvertStr(string ScanStr)
        {
            return dal.GetConvertStr(ScanStr);
        }

        public bool CheckPrintPartNo(string ConvertStr)
        {
            return dal.CheckPrintPartNo(ConvertStr);
        }

        public bool CheckInputStr(string ConvertStr)
        {
            return dal.CheckInputStr(ConvertStr);
        }

        public void LogScanStr(IA_2DSCAN_LOG scanlog)
        {
            dal.LogScanStr(scanlog);
        }

    }
}

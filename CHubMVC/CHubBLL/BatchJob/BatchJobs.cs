using CHubCommon;
using CHubDBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubBLL
{
    public class BatchJobs
    {

        public void SendM1Mail()
        {
            string id = "M1";
            EW_MESSAGE_BLL msgBLL = new EW_MESSAGE_BLL();
            EW_MESSAGE msg= msgBLL.GetMsgByID(id);

            EW_MESSAGE_ATTACH_BLL attachBLL = new EW_MESSAGE_ATTACH_BLL();
            List<EW_MESSAGE_ATTACH> attaches = attachBLL.GetAttachByMsgID(msg.MESSAGE_ID);

            EW_SCRIPT_BLL scriptBLL = new EW_SCRIPT_BLL();
            CHubEntities db = new CHubEntities();
            List<string> attachPathList = new List<string>();
            foreach (var item in attaches)
            {
                EW_SCRIPT script = scriptBLL.GetScriptByID(item.SCRIPT_ID);
                DataTable dt= db.Database.SqlQueryToDataTatable(script.SCRIPT_TEXT);

                FileInfo folder = new FileInfo(CHubConstValues.EmailAttachFolder);
                if (!Directory.Exists(folder.FullName))
                    Directory.CreateDirectory(folder.FullName);

                string attachPath = folder.FullName + string.Format(script.EXPORT_FNAME, DateTime.Now.ToString("yyyy_MM_dd_hh_mm")) + ".xlsx";
                NPOIExcelHelper excelHelper = new NPOIExcelHelper(attachPath);
                excelHelper.DataTableToExcel(dt, "M1 sheet");

                attachPathList.Add(attachPath);
                break;  
            }

            EmailHelper ehelper = new EmailHelper();
            List<string> toList = new List<string>() { "infosys.sh@cummins.com" };
            string from = "infosys.sh@cummins.com";
            string body = msg.MESSAGE_TEXT1 + Environment.NewLine + msg.MESSAGE_TEXT2 + Environment.NewLine + msg.MESSAGE_TEXT3;
            ehelper.SendEmail(toList.ToArray(), from, msg.MESSAGE_SUBJECT, body, attachPathList);

        }
    }
}

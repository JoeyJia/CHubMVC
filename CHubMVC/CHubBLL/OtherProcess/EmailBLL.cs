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
    public class EmailBLL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">message id ,identify</param>
        /// <param name="attachFolder">attacheFolder path, for web using, console app can ignore</param>
        /// <param name="toSample"> will override toList to toSample, for web using now</param>
        public void BuildAndSendEmail(string id="M1",string attachFolder = null,string toSample = null)
        {
            try
            {
                //Console.WriteLine("start send M1 Mail...");
                //string id = "M1";
                EW_MESSAGE_BLL msgBLL = new EW_MESSAGE_BLL();
                EW_MESSAGE msg = msgBLL.GetMsgByID(id);

                //Emailhelper
                EmailHelper ehelper = new EmailHelper();
                List<string> toList = new List<string>();// { "infosys.sh@cummins.com" };//lg166@cummins.com
                List<string> ccList = new List<string>() { "infosys.sh@cummins.com" };

                if (string.IsNullOrEmpty(toSample))
                {
                    string[] toArray = msg.TO_LIST.Split(';');
                    string[] ccArray = msg.CC_LIST.Split(';');
                    toList.AddRange(toArray);
                    ccList.AddRange(ccArray);
                    //Get apply to list
                    EW_USER_APPLY_BLL aBLL = new EW_USER_APPLY_BLL();
                    List<string> applyUserMails = aBLL.GetApplyUsersMail(id);
                    if (applyUserMails != null)
                    {
                        foreach (var item in applyUserMails)
                        {
                            //Just have appuser name , email is null situation
                            if (item.IndexOf("@") < 0)
                                toList.Add(string.Format(CHubConstValues.EmailFormat, item));
                            else
                                toList.Add(item);
                        }
                    }
                    
                    toList.Distinct();

                }
                else
                {
                    toList.Clear();
                    toList.Add(toSample);
                    ccList.Clear();
                }

                string from = CHubConstValues.MailFromAddr;
                StringBuilder body = BuildEmailBody(msg);

                EW_MESSAGE_ATTACH_BLL attachBLL = new EW_MESSAGE_ATTACH_BLL();
                List<EW_MESSAGE_ATTACH> attaches = attachBLL.GetAttachByMsgID(msg.MESSAGE_ID);

                EW_SCRIPT_BLL scriptBLL = new EW_SCRIPT_BLL();
                CHubEntities db = new CHubEntities();
                List<string> attachPathList = new List<string>();
                foreach (var item in attaches)
                {
                    EW_SCRIPT script = scriptBLL.GetScriptByID(item.SCRIPT_ID);
                    DataTable dt = db.Database.SqlQueryToDataTatable(script.SCRIPT_TEXT);

                    if (item.IN_CONTENT == CHubConstValues.IndY)
                    {
                        body.AppendLine(ehelper.BuildBodyFromDT(dt, script.EXPORT_FNAME.Substring(0, script.EXPORT_FNAME.Length - 3)));
                    }
                    else
                    {
                        if (attachFolder == null)
                            attachFolder = CHubConstValues.EmailAttachFolder;
                        FileInfo folder = new FileInfo(attachFolder);
                        if (!Directory.Exists(folder.FullName))
                            Directory.CreateDirectory(folder.FullName);

                        string attachPath = folder.FullName + string.Format(script.EXPORT_FNAME, DateTime.Now.ToString("yyyy_MM_dd_hh_mm")) + ".xlsx";
                        NPOIExcelHelper excelHelper = new NPOIExcelHelper(attachPath);
                        excelHelper.DataTableToExcel(dt, "M1 sheet");

                        attachPathList.Add(attachPath);
                    }
                    //break;
                }

                
                ehelper.SendEmail(toList.ToArray(), ccList.ToArray(), from, msg.MESSAGE_SUBJECT, body.ToString(), attachPathList);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("exception:" + ex.Message);
            }


        }

        private StringBuilder BuildEmailBody(EW_MESSAGE msg)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT1))
                sb.AppendLine(msg.MESSAGE_TEXT1+"<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT2))
                sb.AppendLine(msg.MESSAGE_TEXT2 + "<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT3))
                sb.AppendLine(msg.MESSAGE_TEXT3 + "<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT4))
                sb.AppendLine(msg.MESSAGE_TEXT4 + "<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT5))
                sb.AppendLine(msg.MESSAGE_TEXT5 + "<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT6))
                sb.AppendLine(msg.MESSAGE_TEXT6 + "<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT7))
                sb.AppendLine(msg.MESSAGE_TEXT7 + "<br />");
            if (!string.IsNullOrEmpty(msg.MESSAGE_TEXT8))
                sb.AppendLine(msg.MESSAGE_TEXT8 + "<br />");

            return sb;
        }
    }
}

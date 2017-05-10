using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace CHubCommon
{
    public class EmailHelper
    {
        public bool SendEmail(string[] toAddr, string fromAddr, string subject, string body,List<string> attaches)
        {
            try
            {
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(fromAddr);
                mms.Subject = subject;
                mms.Body = body;
                mms.BodyEncoding = Encoding.UTF8;
                mms.IsBodyHtml = false;
                foreach (var item in toAddr)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        mms.To.Add(new MailAddress(item));
                }
                foreach (var att in attaches)
                {
                    mms.Attachments.Add(new Attachment(att));
                }
                

                SmtpClient client = new SmtpClient();
                client.Host = "mailrelay.cummins.com";
                client.Port = 25;
                client.EnableSsl = false;

                //t1
                //string userName = "";
                //string pwd = "";
                //System.Net.NetworkCredential cre = new System.Net.NetworkCredential(userName, pwd);
                //client.Credentials = cre;

                client.UseDefaultCredentials = false;
                client.Send(mms);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SendEmail", ex);
                return false;
            }

            
        }
    }
}

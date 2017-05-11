using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Data;

namespace CHubCommon
{
    public class EmailHelper
    {
        public bool SendEmail(string[] toAddr,string[] ccAddr, string fromAddr, string subject, string body,List<string> attaches)
        {
            try
            {
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(fromAddr);
                mms.Subject = subject;
                mms.Body = body;
                mms.BodyEncoding = Encoding.UTF8;
                mms.IsBodyHtml = true;
                foreach (var item in toAddr)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        mms.To.Add(new MailAddress(item));
                }
                foreach (var cc in ccAddr)
                {
                    if (!string.IsNullOrWhiteSpace(cc))
                        mms.CC.Add(new MailAddress(cc));
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

            //单一表格邮件内容
            public string BuildBodyFromDT(DataTable data,string title="")
            {
                string MailBody = string.Format("<p style=\"font-size: 10pt\">{0}</p>",title);
            MailBody += "<table cellspacing=\"0\" cellpadding=\"3\" border=\"1\" bgcolor=\"000000\" style=\"font-size: 10pt;line-height: 15px;\">";
                MailBody += "<div align=\"center\">";
                MailBody += "<tr>";
                for (int hcol = 0; hcol < data.Columns.Count; hcol++)
                {
                    MailBody += "<td bgcolor=\"999999\">&nbsp;&nbsp;&nbsp;";
                    MailBody += data.Columns[hcol].ColumnName;
                    MailBody += "&nbsp;&nbsp;&nbsp;</td>";
                }
                MailBody += "</tr>";

                for (int row = 0; row < data.Rows.Count; row++)
                {
                    MailBody += "<tr>";
                    for (int col = 0; col < data.Columns.Count; col++)
                    {
                        MailBody += "<td bgcolor=\"dddddd\">&nbsp;&nbsp;&nbsp;";
                        MailBody += data.Rows[row][col].ToString();
                        MailBody += "&nbsp;&nbsp;&nbsp;</td>";
                    }
                    MailBody += "</tr>";
                }
                MailBody += "</table>";
                MailBody += "</div>";
            return MailBody;
                //Mail.SendStrMail(MailBody);
            }

        }
}

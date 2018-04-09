using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace CHubCommon
{
    public class HttpPostHelper
    {
        public static string HttpPost(string url, string postDataStr)
        {
            string responseStr;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = @"POST";
            req.ContentType = "application/json;charset=utf-8";
            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            WebResponse res;
            try
            {
                res = req.GetResponse();
            }
            catch (WebException ex)
            {
                res = ex.Response;
            }
            Stream responseStream = res.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8, true))
            {
                responseStr = reader.ReadToEnd();
            }
            return responseStr;
        }
    }
}

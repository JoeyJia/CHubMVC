using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class TxtLog
    {
        private string logFile;
        private StreamWriter writer;
        private FileStream fileStream = null;

        public TxtLog()
        {

        }

        public void log(string info, string fullPath)
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fullPath);
                if (!fileInfo.Exists)
                {
                    fileStream = fileInfo.Create();
                    writer = new StreamWriter(fileStream);
                }
                else
                {
                    fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(fileStream);
                }
                writer.WriteLine("Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine(info);
                writer.WriteLine("----------------------------------");
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        public void CreateDirectory(string infoPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        public static void WriteLog(string log)
        {
            FileStream fs = new FileStream(@"C:\temp\bartenderLog.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + log);
            sw.Close();
            fs.Close();
        }


    }
}

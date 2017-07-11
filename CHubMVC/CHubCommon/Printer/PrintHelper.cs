using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CHubCommon.Printer
{
    public class PrintHelper
    {


        static public void PrintFile(string filePath,string printerName = null)
        {
            Process pro = new Process();
            pro.StartInfo.FileName = filePath;
            pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pro.StartInfo.Verb = "Print";
            if (!string.IsNullOrEmpty(printerName))
            {
                Externs.SetDefaultPrinter(printerName);
            }

            pro.Start();

        }
    }
}

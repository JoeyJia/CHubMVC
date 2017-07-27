using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Printing;

namespace CHubCommon.Printer
{
    public class PrintHelper
    {


        public void PrintFile(string filePath,string printerName = null)
        {
            Process pro = new Process();
            pro.StartInfo.FileName = filePath;
            pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pro.StartInfo.Verb = "Print";
            if (!string.IsNullOrEmpty(printerName))
            {
                if (ExistPrinter(printerName))
                    Externs.SetDefaultPrinter(printerName);
                else
                    throw new Exception("No specify printer:" + printerName);
            }

            pro.Start();

        }

        public bool ExistPrinter(string printName)
        {
            List<string> printList = new List<string>();
            foreach (var item in PrinterSettings.InstalledPrinters)
            {
                printList.Add(item.ToString());
            }
            if (printList.Exists(a=>a==printName))
                return true;
            return false;

        }
    }
}

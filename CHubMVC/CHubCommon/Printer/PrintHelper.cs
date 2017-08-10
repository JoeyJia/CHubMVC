using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Printing;
using Spire.Pdf;

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

        public void PrintFileWithCopies(string filePath, string printerName ,int copies)
        {
            PrinterSettings oPrinterSettings = new PrinterSettings();
            PdfDocument pdfdocument = new PdfDocument();
            pdfdocument.LoadFromFile(filePath);
            //pdfdocument.PrinterName = printerName;
            pdfdocument.PrintDocument.PrinterSettings.Copies = (short)copies;
            pdfdocument.PrintDocument.PrinterSettings.PrinterName = printerName;
            pdfdocument.PrintDocument.PrinterSettings.PrintFileName = "AutoPrintLabel";
            //pdfdocument.PrintDocument.PrinterSettings.ToPage = 1;
            //pdfdocument.PrintDocument.PrinterSettings.FromPage = 1;
            //pdfdocument.PrintDocument.PrinterSettings.DefaultPageSettings.Margins.Top = 50;
            //pdfdocument.PrintDocument.PrinterSettings.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(50, 50, 150, 150);
            //pdfdocument.PageSettings.Margins.Bottom = 150;
            //pdfdocument.PageSettings.SetMargins(100, 50, 100, 150);
            //pdfdocument.PrintFromPage = 1;
            //pdfdocument.PrintToPage = 1;
            pdfdocument.PrintDocument.Print();
            pdfdocument.Dispose();
        }

    }
}

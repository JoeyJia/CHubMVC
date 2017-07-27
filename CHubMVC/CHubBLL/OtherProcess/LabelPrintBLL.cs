using CHubCommon.PDFHelper;
using CHubDBEntity.UnmanagedModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubBLL.OtherProcess
{
    public class LabelPrintBLL
    {
        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        private int ContentFontSize = 10;
        private int TableFontSize = 8;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 8;

        private PDFUtility pdfUtility;
        public LabelPrintBLL(string basePath = null)
        {
            //if (basePath == null)
            //    ; ;
            this.BasePath = basePath;
            pdfUtility = new PDFUtility();
        }


        public string BuildPDF(List<V_PLABEL_PRINT> printDatas)
        {
            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmm"));
            string fullPath = BasePath + fileName;


            //List<string> sData = new List<string>();

            //each page size get from header ,
            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL.Value,printDatas[0].PAPER_VERTICAL.Value));

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
           

           // PackPageEventHelper pHelper = new PackPageEventHelper();
            //writer.PageEvent = pHelper;

            doc.SetMargins(10f, 10f, 10f, 36f);


            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp;
using iTextSharp.text;

namespace CHubCommon.PDFHelper
{
    public class CusPDFDocument:iTextSharp.text.pdf.PdfDocument
    {
        public override bool NewPage()
        {
            return base.NewPage();
        }
    }
}

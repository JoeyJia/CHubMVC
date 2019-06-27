using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CHubCommon.PDFHelper
{
    public class PDFBase : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;

        BaseFont BaseFont;

        public int Size = 12;

        public string footer { get; set; }

        public PDFBase(BaseFont _baseFont, string _footer)
        {
            BaseFont = _baseFont;
            footer = _footer;
        }


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }


        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = this.footer; //"Page " + pageN.ToString() + " of ";
            float len = this.BaseFont.GetWidthPoint(text, this.Size);
            iTextSharp.text.Rectangle pageSize = document.PageSize;
            //cb.SetRGBColorFill(100, 100, 100);
            cb.BeginText();
            cb.SetFontAndSize(this.BaseFont, this.Size);
            cb.SetTextMatrix(pageSize.GetLeft(40f), pageSize.GetBottom(20f));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, document.PageSize.Width / 2 - 90f + len, pageSize.GetBottom(20f));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document); template.BeginText();
            template.SetFontAndSize(this.BaseFont, this.Size);
            template.SetTextMatrix(0, 0);
            template.ShowText(""); //"" + (writer.PageNumber - 1)
            template.EndText();
        }


    }
}

using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class LocalPageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private int ContentFontSize = 8;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            //x/total x pages 第X页/共X页
            int pageN = writer.PageNumber;
            //String text = "Page " + pageN.ToString() + " of ";
            String text = string.Format("Page {0}/total   pages 第{0}页/共   页", pageN.ToString());
            //float len = BF_Light.GetWidthPoint(text, ContentFontSize);
            float len1 = BF_Light.GetWidthPoint("Page x/total ", ContentFontSize);
            float len2 = BF_Light.GetWidthPoint("Page x/total   pages 第x页/共 ", ContentFontSize);

            iTextSharp.text.Rectangle pageSize = document.PageSize;

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(BF_Light, ContentFontSize);
            cb.SetTextMatrix(document.PageSize.Width - 180f, pageSize.GetBottom(20f));
            cb.ShowText(text);

            cb.EndText();

            cb.AddTemplate(template, document.PageSize.Width - 180f + len1, pageSize.GetBottom(20f));
            cb.AddTemplate(template, document.PageSize.Width - 180f + len2, pageSize.GetBottom(20f));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(BF_Light, ContentFontSize);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();
        }


    }
}

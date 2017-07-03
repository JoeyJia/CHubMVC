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
        PdfTemplate imgTem;
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private int ContentFontSize = 10;

        public string QRPath;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
            imgTem = cb.CreateTemplate(100, 100);
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

            //cb.SetRGBColorFill(100, 100, 100);

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

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            if(writer.PageNumber!=1)
            {
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
            }

            #region for adding header part for each page
            //Image img = Image.GetInstance(QRPath);

            //iTextSharp.text.Image tImg = iTextSharp.text.Image.GetInstance(QRPath);
            //tImg.Alignment = Element.ALIGN_RIGHT;
            //document.Add(tImg);

            //Paragraph p1 = new Paragraph(string.Format("编号：{0}", hData.SHIP_ID + "C"), new iTextSharp.text.Font(BF_Light, 10));
            //p1.Alignment = Element.ALIGN_RIGHT;
            //doc.Add(p1);
            #endregion


        }


    }
}

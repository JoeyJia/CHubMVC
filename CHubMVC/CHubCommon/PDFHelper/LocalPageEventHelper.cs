using CHubDBEntity.UnmanagedModel;
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
        public int ContentFontSize = 10;
        public int HeaderFontSize = 12;

        public bool printCode = false;
        public string QRPath;
        public string codeString;
        public string line1String;
        public V_RP_WAYBILL_H_PRINT hData;
        public string BasePath;
       

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
            
            if (!printCode)
            {
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
                document.Add(new Paragraph(Environment.NewLine));
            }
            else
            {

                PdfPTable hTable = new PdfPTable(1);
                hTable.WidthPercentage = 100f;
                
                PdfPCell hUnit = new PdfPCell();
                
                /////

                #region for adding header part for each page
                iTextSharp.text.Image tImg = iTextSharp.text.Image.GetInstance(QRPath);
                tImg.Alignment = Element.ALIGN_RIGHT;

                PdfPTable imgTable = new PdfPTable(1);
                imgTable.SetWidths(new float[] { 20f });
                imgTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell imgCell = new PdfPCell(tImg, false);
                imgCell.BorderWidth = 0;
                imgCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                imgTable.AddCell(imgCell);

                hUnit.AddElement(imgTable);


                Paragraph p1 = new Paragraph(codeString, new iTextSharp.text.Font(BF_Light, ContentFontSize));
                p1.Alignment = Element.ALIGN_RIGHT;
                hUnit.AddElement(p1);

                //Paragraph p2 = new Paragraph(line1String, new iTextSharp.text.Font(BF_Light, HeaderFontSize));
                //hUnit.AddElement(p2);
                //hUnit.AddElement(new Paragraph(Environment.NewLine));

                hUnit.BorderWidth = 0;
                hTable.AddCell(hUnit);
                document.Add(hTable);

                #endregion
            }

            Paragraph p2 = new Paragraph(line1String, new iTextSharp.text.Font(BF_Light, HeaderFontSize));
            document.Add(p2);

            //content table
            PdfPTable contentTable = new PdfPTable(4);
            contentTable.WidthPercentage = 100f;
            contentTable.SetWidths(new float[] { 215f, 75f, 200f, 85f });
            PdfPCell cellUnit;

            Paragraph p11 = new Paragraph();
            p11.Add(new Phrase(string.Format("{0}    {1}", hData.NOTE1, hData.FLEX1), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p11.Add(System.Environment.NewLine);
            p11.Add(new Phrase(string.Format("{0}    {1}", hData.COMPANY, hData.SENDER), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p11.Add(System.Environment.NewLine);
            p11.Add(new Phrase(string.Format("{0}", hData.ADDRESS), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p11.Add(System.Environment.NewLine);
            p11.Add(new Phrase(string.Format("{0}    {1}", hData.CONTACT, hData.TELEPHONE), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p11.Add(System.Environment.NewLine);
            p11.Add(System.Environment.NewLine);


            cellUnit = new PdfPCell(p11);
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);

            //wechat imag p12
            if (hData.PRINT_LOGO == CHubConstValues.IndY)
            {
                string imagePath = BasePath.Replace("temp", "images") + hData.LOGO;
                iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(imagePath);
                cellUnit = new PdfPCell(logoImage, true);
            }
            else
            {
                cellUnit = new PdfPCell();
            }
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);


            Paragraph p13 = new Paragraph();
            p13.Add(new Phrase(string.Format("{0}    {1}", hData.NOTE2, hData.FLEX2), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p13.Add(System.Environment.NewLine);
            p13.Add(new Phrase(string.Format("{0}    {1}", hData.R_ADRNAM, hData.R_ADRCTY), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p13.Add(System.Environment.NewLine);
            p13.Add(new Phrase(string.Format("{0}", hData.R_ADRLN1), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p13.Add(System.Environment.NewLine);
            p13.Add(new Phrase(string.Format("{0}    {1}", hData.R_ADRLN2, hData.R_ADRLN3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p13.Add(System.Environment.NewLine);
            p13.Add(new Phrase(string.Format("{0}    {1}", hData.R_LAST_NAME, hData.R_PHNNUM), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p13.Add(System.Environment.NewLine);

            cellUnit = new PdfPCell(p13);
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);

            //p14 signature part
            Paragraph p14 = new Paragraph();
            p14.Add(System.Environment.NewLine);
            p14.Add(System.Environment.NewLine);
            p14.Add(System.Environment.NewLine);
            p14.Add(new Phrase(string.Format("{0}", hData.SIGNATURE3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p14.Add(System.Environment.NewLine);

            cellUnit = new PdfPCell(p14);
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);


            //Line 2 cells
            Paragraph p21 = new Paragraph();
            p21.Add(new Phrase(string.Format("{0}    {1}", hData.NOTE3, hData.FLEX3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);
            p21.Add(new Phrase(string.Format("{0}    {1}", hData.L_ADRNAM, hData.L_ADRCTY), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);
            p21.Add(new Phrase(string.Format("{0}", hData.L_ADRLN1), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);
            p21.Add(new Phrase(string.Format("{0}    {1}", hData.L_ADRLN2, hData.L_ADRLN3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);
            p21.Add(new Phrase(string.Format("{0}    {1}", hData.L_LAST_NAME, hData.L_PHNNUM), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);
            p21.Add(System.Environment.NewLine);

            p21.Add(new Phrase(string.Format("{0}    {1}", hData.NOTE4, hData.FLEX4), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);
            p21.Add(new Phrase(string.Format("{0}", hData.SIGNATURE1), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p21.Add(System.Environment.NewLine);


            cellUnit = new PdfPCell(p21);
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);

            //line 2 mid empty cell
            cellUnit = new PdfPCell();
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);

            //2-3  empty cell
            cellUnit = new PdfPCell();
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);

            //p24 signature part
            Paragraph p24 = new Paragraph();
            p24.Add(System.Environment.NewLine);
            p24.Add(System.Environment.NewLine);
            p24.Add(System.Environment.NewLine);
            p24.Add(System.Environment.NewLine);
            p24.Add(new Phrase(string.Format("{0}", hData.SIGNATURE3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));

            cellUnit = new PdfPCell(p24);
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);


            document.Add(contentTable);

            document.Add(new Paragraph(Environment.NewLine));


        }


    }
}

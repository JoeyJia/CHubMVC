using CHubCommon;
using CHubCommon.FontHelper;
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

        private int ContentFontSize = 8;
        private int TableFontSize = 8;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 8;

        private PDFUtility pdfUtility;
        private FontHelper fontHelper;
        public LabelPrintBLL(string basePath = null)
        {
            //if (basePath == null)
            //    ; ;
            this.BasePath = basePath;
            pdfUtility = new PDFUtility();
            fontHelper = new FontHelper();
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

            doc.SetMargins(10f, 10f, 0f, 0f);
            
            int ColumnHeight = fontHelper.GetFontHeight("柳工", new System.Drawing.Font("黑体", ContentFontSize)) + 1;
            int lineCount = (int)(doc.Top / ColumnHeight);
            int linePointer = 1;
            float currenttop = 0f;
            for (int i = 0; i < printDatas.Count; i++)
            {
                //pHelper.CurrentGroup = pageDatas[i].Header.SHIP_ID;
                if (i != 0)
                {
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL.Value, printDatas[0].PAPER_VERTICAL.Value));
                    doc.NewPage();
                }
                else
                    doc.Open();

                linePointer = 1;

                Paragraph pUnit = new Paragraph(printDatas[i].HEADER, new iTextSharp.text.Font(BF_Light, ContentFontSize));
                doc.Add(pUnit);
                linePointer++;

                doc.Add(new Paragraph(Environment.NewLine, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                linePointer++;

                PdfPCell cellUnit;
                //Content part
                PdfPTable contentTable = new PdfPTable(4);
                contentTable.WidthPercentage = 100f;
                //contentTable.SplitRows = false;
                contentTable.SetWidths(new float[] { 4f,4f,3f,2f });

                //line 1  
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T01, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C01, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 3;
                contentTable.AddCell(cellUnit);

                linePointer++;
                //currenttop = doc.GetTop(51f);
                //Line 2
                cellUnit = new PdfPCell();
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                //picture
                if (string.IsNullOrEmpty(printDatas[i].C01))
                    cellUnit = new PdfPCell();
                else
                    cellUnit = new PdfPCell(pdfUtility.GetCode128Img(printDatas[i].C01, 9),false);
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 3;
                contentTable.AddCell(cellUnit);

                linePointer++;

                //Line3
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T02, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C02, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T09, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C09, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                linePointer++;

                //currenttop = doc.GetTop(51f);
                //Line 4
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T03, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C03, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                //picture
                if (string.IsNullOrEmpty(printDatas[i].C09))
                    cellUnit = new PdfPCell();
                else
                    cellUnit = new PdfPCell(pdfUtility.GetCode128Img(printDatas[i].C09, 9),true);
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 2;

                contentTable.AddCell(cellUnit);

                linePointer++;

                //line 5
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T04, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C04, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T10, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C10, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                linePointer++;
                //line 6
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T05, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C05, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);
                //currenttop = doc.GetTop(51f);
                //picture
                if (string.IsNullOrEmpty(printDatas[i].C10))
                    cellUnit = new PdfPCell();
                else
                    cellUnit = new PdfPCell(pdfUtility.GetCode128Img(printDatas[i].C10, 9),false);
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 2;
                contentTable.AddCell(cellUnit);

                linePointer++;

                //line 7
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T06, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C06, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 3;
                contentTable.AddCell(cellUnit);


                linePointer++;
                //Line 8
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T07, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C07, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 3;
                contentTable.AddCell(cellUnit);


                //line9 
                if (linePointer > lineCount)
                {
                    doc.Add(contentTable);
                    continue;
                }
                cellUnit = new PdfPCell(new Paragraph(printDatas[i].T08, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);

                cellUnit = new PdfPCell(new Paragraph(printDatas[i].C08, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit.BorderWidth = 0;
                cellUnit.Colspan = 3;
                contentTable.AddCell(cellUnit);


                doc.Add(contentTable);

                //var ss = writer.GetPageReference(2);
                
                //int ss= writer.PageNumber;
                //var tt= writer.PageDictEntries;

                //PdfReader reader = new PdfReader(fullPath);
                //var page1 = writer.GetImportedPage(reader, 1);
               


            }
            doc.Close();

            return fileName;
        }

    }
}

using CHubCommon;
using CHubCommon.FontHelper;
using CHubCommon.PDFHelper;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;

namespace CHubBLL.OtherProcess
{
    public class LabelPrintBLL
    {
        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Users\oo450\Desktop\方正黑体简体.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        Font BoldFont;
        

        private int ContentFontSize = 9;
        private int TableFontSize = 9;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 9 ;
      

        private PDFUtility pdfUtility;
        private FontHelper fontHelper;
        public LabelPrintBLL(string basePath = null)
        {
            //if (basePath == null)
            //    ; ;
            this.BasePath = basePath;
            pdfUtility = new PDFUtility();
            fontHelper = new FontHelper();
            // BoldFont = new Font(BF_Light, ContentFontSize, Font.BOLD);
            BoldFont = new Font(BF_Light, ContentFontSize);
        }


        public string BuildPDF(List<V_PLABEL_PRINT> printDatas,List<LabelPrintItem> labelItems)
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

            int headerHeight = fontHelper.GetFontHeight(printDatas[0].HEADER, new System.Drawing.Font("黑体", ContentFontSize, System.Drawing.FontStyle.Bold));

            int ColumnHeight = fontHelper.GetFontHeight("柳工", new System.Drawing.Font("黑体", ContentFontSize)) + 1;
            int lineCount = (int)((doc.Top- headerHeight) / ColumnHeight);
            int linePointer = 1;

            for (int i = 0; i < printDatas.Count; i++)
            {
                //pHelper.CurrentGroup = pageDatas[i].Header.SHIP_ID;
                if (i != 0)
                {
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[i].PAPER_HORIZONTAL.Value, printDatas[i].PAPER_VERTICAL.Value));
                    doc.NewPage();
                }
                else
                    doc.Open();

                int copies = (labelItems.FirstOrDefault(a => a.partNo == printDatas[i].PART_NO)??new LabelPrintItem()).copies;
                //in case senerio 0
                if (copies == 0)
                    copies = 1;

                for (int j = 0; j < copies; j++)
                {
                    if(j!=0)
                        doc.NewPage();

                    linePointer = 1;

                    Paragraph pUnit = new Paragraph(printDatas[i].HEADER, BoldFont);
                    doc.Add(pUnit);
                    linePointer++;

                    doc.Add(new Paragraph(Environment.NewLine, BoldFont));
                    linePointer++;

                    PdfPCell cellUnit;
                    //Content part
                    PdfPTable contentTable = new PdfPTable(4);
                    contentTable.WidthPercentage = 100f;
                    //contentTable.SplitRows = false;
                    contentTable.SetWidths(new float[] { 4f, 4f, 3f, 2f });

                    //line 1  
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T01, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C01, BoldFont));
                    cellUnit.BorderWidth = 0;
                    cellUnit.Colspan = 3;
                    contentTable.AddCell(cellUnit);

                    linePointer++;
                    
                    //Line 2
                    cellUnit = new PdfPCell();
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    //picture
                    if (string.IsNullOrEmpty(printDatas[i].C01))
                        cellUnit = new PdfPCell();
                    else
                        cellUnit = new PdfPCell(pdfUtility.GetCode128Img(printDatas[i].C01, 9), true);
                    cellUnit.BorderWidth = 0;
                    cellUnit.Colspan = 2;  //Jeff 
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell();
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);


                    linePointer++;

                    //Line3
                    if (linePointer > lineCount)
                    {
                        doc.Add(contentTable);
                        continue;
                    }
                    //C09 prepare
                    string c09 = printDatas[i].C09;
                    if (!string.IsNullOrEmpty(c09))
                        c09 = (labelItems.FirstOrDefault(a => a.partNo == printDatas[i].PART_NO) ?? new LabelPrintItem()).MOQ.ToString();

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T02, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C02, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T09, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(c09, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    linePointer++;

                    
                    //Line 4
                    if (linePointer > lineCount)
                    {
                        doc.Add(contentTable);
                        continue;
                    }
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T03, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C03, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    //picture
                    if (string.IsNullOrEmpty(c09))
                        cellUnit = new PdfPCell();
                    else
                        cellUnit = new PdfPCell(pdfUtility.GetCode128Img(c09, 9), false);
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
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T04, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C04, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T10, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C10, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    linePointer++;
                    //line 6
                    if (linePointer > lineCount)
                    {
                        doc.Add(contentTable);
                        continue;
                    }
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T05, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C05, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);
                    
                    //picture
                    if (string.IsNullOrEmpty(printDatas[i].C10))
                        cellUnit = new PdfPCell();
                    else
                        cellUnit = new PdfPCell(pdfUtility.GetCode128Img(printDatas[i].C10, 9), false);
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
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T06, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);

                  
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C06, BoldFont));
                    cellUnit.BorderWidth = 0;
                    cellUnit.Colspan = 3;
                    contentTable.AddCell(cellUnit);
                    


                    linePointer++;
                    //c07 may have new line, if have new line ,need add linePointer
                    //int c01Line = printDatas[i].C07.Count<char>(a => a == Environment.NewLine);
                    //if (printDatas[i].C07.Contains(Environment.NewLine))
                    Regex rg = new Regex("\n");
                    MatchCollection mc = rg.Matches(printDatas[i].C07);
                    linePointer += mc.Count;

                    //Line 8
                    if (linePointer > lineCount)
                    {
                        doc.Add(contentTable);
                        continue;
                    }

                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T07, BoldFont));
                    cellUnit.BorderWidth = 0;
                    contentTable.AddCell(cellUnit);
                   
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].C07, BoldFont));
                    cellUnit.BorderWidth = 0;
                    cellUnit.Colspan = 3;
                    contentTable.AddCell(cellUnit);
                    


                    //line9 
                    if (linePointer > lineCount)
                    {
                        doc.Add(contentTable);
                        continue;
                    }
     
                    cellUnit = new PdfPCell(new Paragraph(printDatas[i].T08, BoldFont));
                    cellUnit.BorderWidth = 0;
                    cellUnit.Colspan = 4;
                    contentTable.AddCell(cellUnit);
            

                    //cellUnit = new PdfPCell(new Paragraph(printDatas[i].C08, BoldFont));
                    //cellUnit = new PdfPCell(new Paragraph(printDatas[i].C08, BoldFont));
                    //cellUnit.BorderWidth = 0;
                    //cellUnit.Colspan = 3;
                    //contentTable.AddCell(cellUnit);



                    doc.Add(contentTable);
                }

            }
            doc.Close();

            return fileName;
        }


        public string BuildPDF_New(List<V_PLABEL_BY_LOD_PRINT> printDatas, List<LabelPrintItems> labelItems)
        {
            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullPath = BasePath + fileName;
            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL));//pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL)
            
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.Open();
            for (int i = 0; i < printDatas.Count; i++)
            {
                if (i != 0)
                { 
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[i].PAPER_HORIZONTAL, printDatas[i].PAPER_VERTICAL));
                    doc.NewPage();
                }

                int copies = labelItems.FirstOrDefault(c => c.PART_NO == printDatas[i].PART_NO).COPIES;
                if (copies == 0)
                    copies = 1;

                for (int j = 0; j < copies; j++)
                {
                    if (j != 0)
                        doc.NewPage();

                    #region
                    //line01 
                    if (printDatas[i].PX01.HasValue)
                    {
                        if (printDatas[i].T01 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C01, "TXT", printDatas[i].PX01, (printDatas[i].PY01 + 22), printDatas[i].S01,1);
                        }

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01,1);               
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03,3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05,5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05,5);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06,6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06,6);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07,7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07,7);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08,8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08,8);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09,9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09,9);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10,10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10,10);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11,11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11,11);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12,12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12,12);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13,13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13,13);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14,14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14,14);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15,15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15,15);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16,16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16,16);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17,17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C18, "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18,18);
                        }
                        GetElementPosition(writer, printDatas[i].C18, printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18,18);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19,19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19,19);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20,20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20,20);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21,21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21,21);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22,22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22,22);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23,23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23,23);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24,24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24,24);
                    }
                    #endregion
                }
            }
            doc.Close();

            return fileName;
        }

        /// <summary>
        /// By Parts Print
        /// </summary>
        /// <returns></returns>
        public string ExportPDFByParts(List<V_PLABEL_BY_PART_PRINT> printDatas, List<LabelPrintItems> labelItems)
        {
            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullPath = BasePath + fileName;
            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL));//pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL)

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.Open();
            for (int i = 0; i < printDatas.Count; i++)
            {
                if (i != 0)
                {
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[i].PAPER_HORIZONTAL, printDatas[i].PAPER_VERTICAL));
                    doc.NewPage();
                }

                int copies = labelItems.FirstOrDefault(c => c.PART_NO == printDatas[i].PART_NO).COPIES;
                if (copies == 0)
                    copies = 1;

                for (int j = 0; j < copies; j++)
                {
                    if (j != 0)
                        doc.NewPage();

                    #region
                    //line01 
                    if (printDatas[i].PX01.HasValue)
                    {
                        if (printDatas[i].T01 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C01, "TXT", printDatas[i].PX01, (printDatas[i].PY01 + 22), printDatas[i].S01, 1);
                        }

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C18, "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, printDatas[i].C18, printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24);
                    }
                    #endregion
                }
            }
            doc.Close();

            return fileName;
        }

        /// <summary>
        /// By ASN Print
        /// </summary>
        /// <param name="printDatas"></param>
        /// <param name="labelItems"></param>
        /// <returns></returns>
        public string ExportPDFByASN(List<V_PLABEL_BY_ASN_PRINT> printDatas, List<LabelPrintItems> labelItems)
        {
            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullPath = BasePath + fileName;
            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL));//pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL)

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.Open();
            for (int i = 0; i < printDatas.Count; i++)
            {
                if (i != 0)
                {
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[i].PAPER_HORIZONTAL, printDatas[i].PAPER_VERTICAL));
                    doc.NewPage();
                }

                int copies = labelItems.FirstOrDefault(c => c.PART_NO == printDatas[i].PART_NO).COPIES;
                if (copies == 0)
                    copies = 1;

                for (int j = 0; j < copies; j++)
                {
                    if (j != 0)
                        doc.NewPage();

                    #region
                    //line01 
                    if (printDatas[i].PX01.HasValue)
                    {
                        if (printDatas[i].T01 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C01, "TXT", printDatas[i].PX01, (printDatas[i].PY01 + 22), printDatas[i].S01, 1);
                        }

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C18, "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, printDatas[i].C18, printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24);
                    }
                    #endregion
                }
            }
            doc.Close();

            return fileName;
        }

        /// <summary>
        /// By Uncatalog Parts Pring
        /// </summary>
        /// <returns></returns>
        public string ExportPDFByUncatalogParts(List<V_PLABEL_BY_UNCATALOG_PRINT> printDatas, List<LabelPrintItems> labelItems)
        {
            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullPath = BasePath + fileName;
            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL));//pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL)

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.Open();
            for (int i = 0; i < printDatas.Count; i++)
            {
                if (i != 0)
                {
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[i].PAPER_HORIZONTAL, printDatas[i].PAPER_VERTICAL));
                    doc.NewPage();
                }

                int copies = labelItems.FirstOrDefault(c => c.PART_NO == printDatas[i].PART_NO).COPIES;
                if (copies == 0)
                    copies = 1;

                for (int j = 0; j < copies; j++)
                {
                    if (j != 0)
                        doc.NewPage();

                    #region
                    //line01 
                    if (printDatas[i].PX01.HasValue)
                    {
                        if (printDatas[i].T01 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C01, "TXT", printDatas[i].PX01, (printDatas[i].PY01 + 22), printDatas[i].S01, 1);
                        }

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C18, "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, printDatas[i].C18, printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24);
                    }
                    #endregion
                }
            }
            doc.Close();

            return fileName;
        }



        /// <summary>
        /// GetElementPosition
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="C">Content</param>
        /// <param name="T">Type</param>
        /// <param name="PX">X</param>
        /// <param name="PY">Y</param>
        /// <param name="S">Size</param>
        public void GetElementPosition(PdfWriter writer, string C, string T, decimal? PX, decimal? PY, decimal? S,int i)
        {
            //Font arial = new Font(BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED), 28, Font.ITALIC, BaseColor.RED);

            PdfContentByte cb = writer.DirectContent;
            writer.CompressionLevel = 0;
            Image img;
            switch (T)
            {
                case "TXT":
                    cb.BeginText();
                    cb.MoveText(PX.HasValue ? (float)PX.Value : 0, PY.HasValue ? (float)PY.Value : 0);
                    cb.SetFontAndSize(BF_Light, (float)S.Value);
                    cb.ShowText(!string.IsNullOrEmpty(C) ? C : "");
                    cb.EndText();
                    break;
                case "1D":
                    img = pdfUtility.GetCode128Img(!string.IsNullOrEmpty(C) ? C : "", (int)S);
                    img.SetAbsolutePosition(PX.HasValue ? (float)PX.Value : 0, PY.HasValue ? (float)PY.Value : 0);
                    img.ScaleAbsoluteHeight(20f);
                    if (i == 18)
                        img.ScaleAbsoluteWidth(40f);
                    else
                        img.ScaleAbsoluteWidth(80f);
                    cb.AddImage(img);
                    break;
                case "2D":
                    img = pdfUtility.QRCodeEncoderUtil(!string.IsNullOrEmpty(C) ? C : "");
                    img.SetAbsolutePosition(PX.HasValue ? (float)PX.Value : 0, PY.HasValue ? (float)PY.Value : 0);
                    img.ScaleAbsoluteHeight(40f);
                    img.ScaleAbsoluteWidth(40f);
                    cb.AddImage(img);
                    break;
                case "PIC":
                    img = Image.GetInstance(@"C:\Users\oo450\Cummins Project\ChubPublish\Images\" + C); //正式 C:\inetpub\wwwroot\CHub\Images //测试 D:\IIS\CHub\Images  //本地 C:\Users\oo450\Cummins Project\ChubPublish\Images
                    img.SetAbsolutePosition(PX.HasValue ? (float)PX.Value : 0, PY.HasValue ? (float)PY.Value : 0);
                    cb.AddImage(img);
                    break;
            }
        }

        //public BaseFont GetBasetFontByStr(string str)
        //{
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        string zt = string.Empty;
        //        Regex reg = new Regex("^[A-Za-z]+$");
        //        char[] chr = str.ToCharArray();
        //        int i = 0;
        //        if (int.TryParse(chr[0].ToString(), out i) || reg.IsMatch(chr[0].ToString()))
        //            zt = "C:\\Windows\\Fonts\\ARIALN.TTF";  //ARIALN.TTF    arial narrow.ttf
        //        if (chr[0] >= 0x4e00 && chr[0] <= 0x9fbb)
        //            zt = "C:\\Windows\\Fonts\\simhei.ttf";

        //        BaseFont BF = BaseFont.CreateFont(zt, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        return BF;
        //    }
        //    else
        //    {
        //        BaseFont BF = BaseFont.CreateFont(@"C:\\Windows\\Fonts\\ARIALN.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        return BF;
        //    }
          
        //}





    }
}

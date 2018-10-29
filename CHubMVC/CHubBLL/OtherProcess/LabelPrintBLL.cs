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
using System.Drawing.Printing;
using System.Windows.Forms;
using Seagull.BarTender.Print;
using System.Diagnostics;
using BarTender;


namespace CHubBLL.OtherProcess
{
    public class LabelPrintBLL
    {
        //字体路径
        private static readonly string fontpath = System.Configuration.ConfigurationManager.AppSettings["FontPath"].ToString();
        //图片路径
        private static readonly string imagepath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"].ToString();

        private CHubCommonHelper ccHelper;

        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        //本地 C:\Users\oo450\Desktop\  测试/正式 C:\Windows\Fonts\
        BaseFont BF_Light = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        Font BoldFont;


        private int ContentFontSize = 9;
        private int TableFontSize = 9;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 9;

        /// <summary>
        /// 大号字体
        /// </summary>
        private int HFirstFontSize = 20;
        /// <summary>
        /// 12号字体
        /// </summary>
        private int HMiddleFontSize = 12;
        /// <summary>
        /// 9号字体
        /// </summary>
        private int HEndFontSize = 9;



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
            ccHelper = new CHubCommonHelper();
        }


        public string BuildPDF(List<V_PLABEL_PRINT> printDatas, List<LabelPrintItem> labelItems)
        {

            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmm"));
            string fullPath = BasePath + fileName;

            //List<string> sData = new List<string>();

            //each page size get from header ,
            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL.Value, printDatas[0].PAPER_VERTICAL.Value));

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));


            // PackPageEventHelper pHelper = new PackPageEventHelper();
            //writer.PageEvent = pHelper;

            doc.SetMargins(10f, 10f, 0f, 0f);

            int headerHeight = fontHelper.GetFontHeight(printDatas[0].HEADER, new System.Drawing.Font("黑体", ContentFontSize, System.Drawing.FontStyle.Bold));

            int ColumnHeight = fontHelper.GetFontHeight("柳工", new System.Drawing.Font("黑体", ContentFontSize)) + 1;
            int lineCount = (int)((doc.Top - headerHeight) / ColumnHeight);
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

                int copies = (labelItems.FirstOrDefault(a => a.partNo == printDatas[i].PART_NO) ?? new LabelPrintItem()).copies;
                //in case senerio 0
                if (copies == 0)
                    copies = 1;

                for (int j = 0; j < copies; j++)
                {
                    if (j != 0)
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


        /// <summary>
        /// By LOD Print
        /// </summary>
        /// <param name="printDatas"></param>
        /// <param name="labelItems"></param>
        /// <returns></returns>
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
                int moq = labelItems.FirstOrDefault(c => c.PART_NO == printDatas[i].PART_NO && c.VID == printDatas[i].VID).MOQ;
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

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1, printDatas[i].SIZE_2D);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2, printDatas[i].SIZE_2D);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3, printDatas[i].SIZE_2D);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4, printDatas[i].SIZE_2D);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5, printDatas[i].SIZE_2D);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6, printDatas[i].SIZE_2D);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7, printDatas[i].SIZE_2D);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8, printDatas[i].SIZE_2D);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9, printDatas[i].SIZE_2D);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10, printDatas[i].SIZE_2D);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11, printDatas[i].SIZE_2D);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12, printDatas[i].SIZE_2D);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13, printDatas[i].SIZE_2D);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14, printDatas[i].SIZE_2D);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15, printDatas[i].SIZE_2D);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16, printDatas[i].SIZE_2D);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17, printDatas[i].SIZE_2D);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, moq.ToString(), "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, moq.ToString(), printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18, printDatas[i].SIZE_2D);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19, printDatas[i].SIZE_2D);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20, printDatas[i].SIZE_2D);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21, printDatas[i].SIZE_2D);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22, printDatas[i].SIZE_2D);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23, printDatas[i].SIZE_2D);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24, printDatas[i].SIZE_2D);
                    }
                    #endregion
                }
            }

            doc.Close();

            return fileName;
        }

        public bool AutoPrint_LOD(List<V_PLABEL_BY_LOD_PRINT> printDatas, List<LabelPrintItems> labelItems, string baseBTW, string Printer_Name)
        {
            //var label_code = printDatas[0].LABEL_CODE;
            //string baseBTW = new RP_LABEL_TYPE2_BLL().GetBTW(label_code);
            string fullpath = BasePath + baseBTW;
            TxtLog.WriteLog("调用模板绝对路径：" + fullpath);
            List<BartenderPrintDatas> bpd = new List<BartenderPrintDatas>();
            foreach (var pd in printDatas)
            {
                int copies = labelItems.FirstOrDefault(c => c.PART_NO == pd.PART_NO).COPIES;
                int moq = labelItems.FirstOrDefault(c => c.PART_NO == pd.PART_NO && c.VID == pd.VID).MOQ;
                bpd.Add(new BartenderPrintDatas()
                {
                    C10 = pd.C10,
                    C11 = pd.C11,
                    C12 = pd.C12,
                    C13 = pd.C13,
                    C14 = pd.C14,
                    C15 = pd.C15.HasValue ? pd.C15.Value.ToString() : "",
                    C16 = pd.C16,
                    C17 = pd.C17,
                    C18 = moq.ToString(),
                    C19 = pd.C19,
                    C20 = pd.C20,
                    C21 = pd.C21,
                    C22 = pd.C22,
                    C23 = pd.C23,
                    C24 = pd.C24,
                    Copies = copies
                });
            }
            try
            {
                TxtLog.WriteLog("准备打印");
                Bartender_Print(bpd, fullpath, Printer_Name);
                TxtLog.WriteLog("打印完成");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool PrintLBScan_M(List<V_PLABEL_BY_MOBILE_PRINT> printDatas, LBScanPrintItems labelItems, string baseBTW, string PrinterName, string UserName)
        {
            string fullpath = BasePath + baseBTW;
            TxtLog.WriteLog("调用模板绝对路径：" + fullpath);
            List<BartenderPrintDatas> bpd = new List<BartenderPrintDatas>();

            var Printer_ID = ccHelper.ExecuteFunc("select PRINTER_ID from RP_Printer where PRINTER_NAME='" + PrinterName + "'");
            TxtLog.WriteLog("打印机ID为" + Printer_ID);

            foreach (var pd in printDatas)
            {
                int copies = labelItems.COPIES;
                int moq = labelItems.MOQ;
                string orgcod = labelItems.COO;
                string country_desc_cn = pd.COUNTRY_DESC_CN;

                string c15 = pd.C15.HasValue ? pd.C15.Value.ToString() : "";
                //Function
                var sql = string.Format(@"select GET_2D_Str_by_LOD('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',
                                            '{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}') from dual", pd.LABEL_CODE, UserName, Printer_ID,
                                pd.WH_ID, pd.SHIP_ID, pd.LODNUM, pd.ADRNAM, pd.PRTNUM, pd.VC_CPONUM, pd.PART_NO, pd.C10, pd.C11, orgcod, pd.C13, pd.C14, c15, pd.C16, pd.C17, moq.ToString(), pd.C19,
                                country_desc_cn, pd.C21, pd.C22, pd.C23, pd.C24, copies);//执行function语句
                TxtLog.WriteLog("function执行语句：" + sql);
                string str = ccHelper.ExecuteFunc(sql);//执行funciton结果 赋值给C10
                TxtLog.WriteLog("function执行结果：" + str);

                bpd.Add(new BartenderPrintDatas()
                {
                    C10 = !string.IsNullOrEmpty(str) ? str : pd.C10,
                    C11 = pd.C11,
                    C12 = orgcod, //pd.C12,
                    C13 = pd.C13,
                    C14 = pd.C14,
                    C15 = c15,
                    C16 = pd.C16,
                    C17 = pd.C17,
                    C18 = moq.ToString(),
                    C19 = pd.C19,
                    C20 = country_desc_cn, //pd.C20,
                    C21 = pd.C21,
                    C22 = pd.C22,
                    C23 = pd.C23,
                    C24 = pd.C24,
                    Copies = copies
                });
            }
            try
            {
                TxtLog.WriteLog("LOD准备打印");
                Bartender_Print(bpd, fullpath, PrinterName);
                return true;
            }
            catch (Exception ex)
            {
                TxtLog.WriteLog("打印失败，" + ex.Message);
                return false;
            }

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

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1, printDatas[i].SIZE_2D);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2, printDatas[i].SIZE_2D);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3, printDatas[i].SIZE_2D);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4, printDatas[i].SIZE_2D);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5, printDatas[i].SIZE_2D);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6, printDatas[i].SIZE_2D);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7, printDatas[i].SIZE_2D);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8, printDatas[i].SIZE_2D);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9, printDatas[i].SIZE_2D);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10, printDatas[i].SIZE_2D);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11, printDatas[i].SIZE_2D);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12, printDatas[i].SIZE_2D);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13, printDatas[i].SIZE_2D);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14, printDatas[i].SIZE_2D);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15, printDatas[i].SIZE_2D);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16, printDatas[i].SIZE_2D);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17, printDatas[i].SIZE_2D);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C18, "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, printDatas[i].C18, printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18, printDatas[i].SIZE_2D);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19, printDatas[i].SIZE_2D);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20, printDatas[i].SIZE_2D);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21, printDatas[i].SIZE_2D);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22, printDatas[i].SIZE_2D);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23, printDatas[i].SIZE_2D);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24, printDatas[i].SIZE_2D);
                    }
                    #endregion
                }
            }

            doc.Close();

            //PrintSetting(fullPath);

            return fileName;
        }


        public bool AutoPrint_PART(List<V_PLABEL_BY_PART_PRINT> printDatas, List<LabelPrintItems> labelItems, string baseBTW, string Printer_Name)
        {
            string fullpath = BasePath + baseBTW;
            List<BartenderPrintDatas> bpd = new List<BartenderPrintDatas>();
            foreach (var pd in printDatas)
            {
                int copies = labelItems.FirstOrDefault(c => c.PART_NO == pd.PART_NO).COPIES;
                bpd.Add(new BartenderPrintDatas()
                {
                    C10 = pd.C10,
                    C11 = pd.C11,
                    C12 = pd.C12,
                    C13 = pd.C13,
                    C14 = pd.C14,
                    C15 = pd.C15.HasValue ? pd.C15.Value.ToString() : "",
                    C16 = pd.C16,
                    C17 = pd.C17,
                    C18 = pd.C18,
                    C19 = pd.C19,
                    C20 = pd.C20,
                    C21 = pd.C21,
                    C22 = pd.C22,
                    C23 = pd.C23,
                    C24 = pd.C24,
                    Copies = copies
                });
            }
            try
            {
                Bartender_Print(bpd, fullpath, Printer_Name);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
                int moq = labelItems.FirstOrDefault(c => c.PART_NO == printDatas[i].PART_NO).MOQ;
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

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1, printDatas[i].SIZE_2D);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2, printDatas[i].SIZE_2D);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3, printDatas[i].SIZE_2D);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4, printDatas[i].SIZE_2D);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5, printDatas[i].SIZE_2D);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6, printDatas[i].SIZE_2D);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7, printDatas[i].SIZE_2D);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8, printDatas[i].SIZE_2D);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9, printDatas[i].SIZE_2D);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10, printDatas[i].SIZE_2D);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11, printDatas[i].SIZE_2D);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12, printDatas[i].SIZE_2D);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13, printDatas[i].SIZE_2D);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14, printDatas[i].SIZE_2D);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15, printDatas[i].SIZE_2D);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16, printDatas[i].SIZE_2D);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17, printDatas[i].SIZE_2D);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, moq.ToString(), "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, moq.ToString(), printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18, printDatas[i].SIZE_2D);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19, printDatas[i].SIZE_2D);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20, printDatas[i].SIZE_2D);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21, printDatas[i].SIZE_2D);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22, printDatas[i].SIZE_2D);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23, printDatas[i].SIZE_2D);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24, printDatas[i].SIZE_2D);
                    }
                    #endregion
                }
            }
            doc.Close();

            return fileName;
        }

        public bool AutoPrint_ASN(List<V_PLABEL_BY_ASN_PRINT> printDatas, List<LabelPrintItems> labelItems, string baseBTW, string Printer_Name)
        {
            string fullpath = BasePath + baseBTW;
            List<BartenderPrintDatas> bpd = new List<BartenderPrintDatas>();
            foreach (var pd in printDatas)
            {
                int copies = labelItems.FirstOrDefault(c => c.PART_NO == pd.PART_NO).COPIES;
                int moq = labelItems.FirstOrDefault(c => c.PART_NO == pd.PART_NO).MOQ;
                bpd.Add(new BartenderPrintDatas()
                {
                    C10 = pd.C10,
                    C11 = pd.C11,
                    C12 = pd.C12,
                    C13 = pd.C13,
                    C14 = pd.C14,
                    C15 = pd.C15.HasValue ? pd.C15.Value.ToString() : "",
                    C16 = pd.C16,
                    C17 = pd.C17,
                    C18 = moq.ToString(),
                    C19 = pd.C19,
                    C20 = pd.C20,
                    C21 = pd.C21,
                    C22 = pd.C22,
                    C23 = pd.C23,
                    C24 = pd.C24,
                    Copies = copies
                });
            }
            try
            {
                Bartender_Print(bpd, fullpath, Printer_Name);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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

                        GetElementPosition(writer, printDatas[i].C01, printDatas[i].T01, printDatas[i].PX01, printDatas[i].PY01, printDatas[i].S01, 1, printDatas[i].SIZE_2D);
                    }
                    //line02
                    if (printDatas[i].PX02.HasValue)
                    {
                        if (printDatas[i].T02 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C02, "TXT", printDatas[i].PX02, (printDatas[i].PY02 + 22), printDatas[i].S02, 2);
                        }

                        GetElementPosition(writer, printDatas[i].C02, printDatas[i].T02, printDatas[i].PX02, printDatas[i].PY02, printDatas[i].S02, 2, printDatas[i].SIZE_2D);
                    }
                    //line03
                    if (printDatas[i].PX03.HasValue)
                    {
                        if (printDatas[i].T03 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C03, "TXT", printDatas[i].PX03, (printDatas[i].PY03 + 22), printDatas[i].S03, 3);
                        }
                        GetElementPosition(writer, printDatas[i].C03, printDatas[i].T03, printDatas[i].PX03, printDatas[i].PY03, printDatas[i].S03, 3, printDatas[i].SIZE_2D);
                    }
                    //line04
                    if (printDatas[i].PX04.HasValue)
                    {
                        if (printDatas[i].T04 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C04, "TXT", printDatas[i].PX04, (printDatas[i].PY04 + 22), printDatas[i].S04, 4);
                        }
                        GetElementPosition(writer, printDatas[i].C04, printDatas[i].T04, printDatas[i].PX04, printDatas[i].PY04, printDatas[i].S04, 4, printDatas[i].SIZE_2D);
                    }
                    //line05
                    if (printDatas[i].PX05.HasValue)
                    {
                        if (printDatas[i].T05 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C05, "TXT", printDatas[i].PX05, (printDatas[i].PY05 + 22), printDatas[i].S05, 5);
                        }
                        GetElementPosition(writer, printDatas[i].C05, printDatas[i].T05, printDatas[i].PX05, printDatas[i].PY05, printDatas[i].S05, 5, printDatas[i].SIZE_2D);
                    }
                    //line06
                    if (printDatas[i].PX06.HasValue)
                    {
                        if (printDatas[i].T06 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C06, "TXT", printDatas[i].PX06, (printDatas[i].PY06 + 22), printDatas[i].S06, 6);
                        }
                        GetElementPosition(writer, printDatas[i].C06, printDatas[i].T06, printDatas[i].PX06, printDatas[i].PY06, printDatas[i].S06, 6, printDatas[i].SIZE_2D);
                    }
                    //line07
                    if (printDatas[i].PX07.HasValue)
                    {
                        if (printDatas[i].T07 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C07, "TXT", printDatas[i].PX07, (printDatas[i].PY07 + 22), printDatas[i].S07, 7);
                        }
                        GetElementPosition(writer, printDatas[i].C07, printDatas[i].T07, printDatas[i].PX07, printDatas[i].PY07, printDatas[i].S07, 7, printDatas[i].SIZE_2D);
                    }
                    //line08
                    if (printDatas[i].PX08.HasValue)
                    {
                        if (printDatas[i].T08 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C08, "TXT", printDatas[i].PX08, (printDatas[i].PY08 + 22), printDatas[i].S08, 8);
                        }
                        GetElementPosition(writer, printDatas[i].C08, printDatas[i].T08, printDatas[i].PX08, printDatas[i].PY08, printDatas[i].S08, 8, printDatas[i].SIZE_2D);
                    }
                    //line09
                    if (printDatas[i].PX09.HasValue)
                    {
                        if (printDatas[i].T09 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C09, "TXT", printDatas[i].PX09, (printDatas[i].PY09 + 22), printDatas[i].S09, 9);
                        }
                        GetElementPosition(writer, printDatas[i].C09, printDatas[i].T09, printDatas[i].PX09, printDatas[i].PY09, printDatas[i].S09, 9, printDatas[i].SIZE_2D);
                    }
                    //line10
                    if (printDatas[i].PX10.HasValue)
                    {
                        if (printDatas[i].T10 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C10, "TXT", printDatas[i].PX10, (printDatas[i].PY10 + 22), printDatas[i].S10, 10);
                        }
                        GetElementPosition(writer, printDatas[i].C10, printDatas[i].T10, printDatas[i].PX10, printDatas[i].PY10, printDatas[i].S10, 10, printDatas[i].SIZE_2D);
                    }
                    //line11
                    if (printDatas[i].PX11.HasValue)
                    {
                        if (printDatas[i].T11 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C11, "TXT", printDatas[i].PX11, (printDatas[i].PY11 + 22), printDatas[i].S11, 11);
                        }

                        GetElementPosition(writer, printDatas[i].C11, printDatas[i].T11, printDatas[i].PX11, printDatas[i].PY11, printDatas[i].S11, 11, printDatas[i].SIZE_2D);
                    }
                    //line12
                    if (printDatas[i].PX12.HasValue)
                    {
                        if (printDatas[i].T12 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C12, "TXT", printDatas[i].PX12, (printDatas[i].PY12 + 22), printDatas[i].S12, 12);
                        }
                        GetElementPosition(writer, printDatas[i].C12, printDatas[i].T12, printDatas[i].PX12, printDatas[i].PY12, printDatas[i].S12, 12, printDatas[i].SIZE_2D);
                    }
                    //line13  change
                    if (printDatas[i].PX13.HasValue)
                    {
                        if (printDatas[i].T13 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C13, "TXT", printDatas[i].PX13, (printDatas[i].PY13 + 22), printDatas[i].S13, 13);
                        }
                        GetElementPosition(writer, printDatas[i].C13, printDatas[i].T13, printDatas[i].PX13, printDatas[i].PY13, printDatas[i].S13, 13, printDatas[i].SIZE_2D);
                    }
                    //line14
                    if (printDatas[i].PX14.HasValue)
                    {
                        if (printDatas[i].T14 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C14, "TXT", printDatas[i].PX14, (printDatas[i].PY14 + 22), printDatas[i].S14, 14);
                        }
                        GetElementPosition(writer, printDatas[i].C14, printDatas[i].T14, printDatas[i].PX14, printDatas[i].PY14, printDatas[i].S14, 14, printDatas[i].SIZE_2D);
                    }
                    //line15
                    if (printDatas[i].PX15.HasValue)
                    {
                        if (printDatas[i].T15 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C15.Value.ToString(), "TXT", printDatas[i].PX15, (printDatas[i].PY15 + 22), printDatas[i].S15, 15);
                        }
                        GetElementPosition(writer, printDatas[i].C15.Value.ToString(), printDatas[i].T15, printDatas[i].PX15, printDatas[i].PY15, printDatas[i].S15, 15, printDatas[i].SIZE_2D);
                    }
                    //line16
                    if (printDatas[i].PX16.HasValue)
                    {
                        if (printDatas[i].T16 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C16, "TXT", printDatas[i].PX16, (printDatas[i].PY16 + 22), printDatas[i].S16, 16);
                        }
                        GetElementPosition(writer, printDatas[i].C16, printDatas[i].T16, printDatas[i].PX16, printDatas[i].PY16, printDatas[i].S16, 16, printDatas[i].SIZE_2D);
                    }
                    //line17
                    if (printDatas[i].PX17.HasValue)
                    {
                        if (printDatas[i].T17 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C17, "TXT", printDatas[i].PX17, (printDatas[i].PY17 + 22), printDatas[i].S17, 17);
                        }
                        GetElementPosition(writer, printDatas[i].C17, printDatas[i].T17, printDatas[i].PX17, printDatas[i].PY17, printDatas[i].S17, 17, printDatas[i].SIZE_2D);
                    }
                    //line18
                    if (printDatas[i].PX18.HasValue)
                    {
                        if (printDatas[i].T18 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C18, "TXT", printDatas[i].PX18, (printDatas[i].PY18 + 22), printDatas[i].S18, 18);
                        }
                        GetElementPosition(writer, printDatas[i].C18, printDatas[i].T18, printDatas[i].PX18, printDatas[i].PY18, printDatas[i].S18, 18, printDatas[i].SIZE_2D);
                    }
                    //line19
                    if (printDatas[i].PX19.HasValue)
                    {
                        if (printDatas[i].T19 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C19, "TXT", printDatas[i].PX19, (printDatas[i].PY19 + 22), printDatas[i].S19, 19);
                        }
                        GetElementPosition(writer, printDatas[i].C19, printDatas[i].T19, printDatas[i].PX19, printDatas[i].PY19, printDatas[i].S19, 19, printDatas[i].SIZE_2D);
                    }
                    //line20
                    if (printDatas[i].PX20.HasValue)
                    {
                        if (printDatas[i].T20 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C20, "TXT", printDatas[i].PX20, (printDatas[i].PY20 + 22), printDatas[i].S20, 20);
                        }
                        GetElementPosition(writer, printDatas[i].C20, printDatas[i].T20, printDatas[i].PX20, printDatas[i].PY20, printDatas[i].S20, 20, printDatas[i].SIZE_2D);
                    }
                    //line21
                    if (printDatas[i].PX21.HasValue)
                    {
                        if (printDatas[i].T21 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C21, "TXT", printDatas[i].PX21, (printDatas[i].PY21 + 22), printDatas[i].S21, 21);
                        }
                        GetElementPosition(writer, printDatas[i].C21, printDatas[i].T21, printDatas[i].PX21, printDatas[i].PY21, printDatas[i].S21, 21, printDatas[i].SIZE_2D);
                    }
                    //line22
                    if (printDatas[i].PX22.HasValue)
                    {
                        if (printDatas[i].T22 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C22, "TXT", printDatas[i].PX22, (printDatas[i].PY22 + 22), printDatas[i].S22, 22);
                        }
                        GetElementPosition(writer, printDatas[i].C22, printDatas[i].T22, printDatas[i].PX22, printDatas[i].PY22, printDatas[i].S22, 22, printDatas[i].SIZE_2D);
                    }
                    //line23
                    if (printDatas[i].PX23.HasValue)
                    {
                        if (printDatas[i].T23 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C23, "TXT", printDatas[i].PX23, (printDatas[i].PY23 + 22), printDatas[i].S23, 23);
                        }
                        GetElementPosition(writer, printDatas[i].C23, printDatas[i].T23, printDatas[i].PX23, printDatas[i].PY23, printDatas[i].S23, 23, printDatas[i].SIZE_2D);
                    }
                    //line24
                    if (printDatas[i].PX24.HasValue)
                    {
                        if (printDatas[i].T24 == "1D")
                        {
                            GetElementPosition(writer, printDatas[i].C24, "TXT", printDatas[i].PX24, (printDatas[i].PY24 + 22), printDatas[i].S24, 24);
                        }
                        GetElementPosition(writer, printDatas[i].C24, printDatas[i].T24, printDatas[i].PX24, printDatas[i].PY24, printDatas[i].S24, 24, printDatas[i].SIZE_2D);
                    }
                    #endregion
                }
            }
            doc.Close();

            return fileName;
        }

        public bool AutoPrint_UParts(List<V_PLABEL_BY_UNCATALOG_PRINT> printDatas, List<LabelPrintItems> labelItems, string baseBTW, string Printer_Name)
        {
            string fullpath = BasePath + baseBTW;
            List<BartenderPrintDatas> bpd = new List<BartenderPrintDatas>();
            foreach (var pd in printDatas)
            {
                int copies = labelItems.FirstOrDefault(c => c.PART_NO == pd.PART_NO).COPIES;
                bpd.Add(new BartenderPrintDatas()
                {
                    C10 = pd.C10,
                    C11 = pd.C11,
                    C12 = pd.C12,
                    C13 = pd.C13,
                    C14 = pd.C14,
                    C15 = pd.C15.HasValue ? pd.C15.Value.ToString() : "",
                    C16 = pd.C16,
                    C17 = pd.C17,
                    C18 = pd.C18,
                    C19 = pd.C19,
                    C20 = pd.C20,
                    C21 = pd.C21,
                    C22 = pd.C22,
                    C23 = pd.C23,
                    C24 = pd.C24,
                    Copies = copies
                });
            }
            try
            {
                Bartender_Print(bpd, fullpath, Printer_Name);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// By KITS Print
        /// </summary>
        /// <param name="printDatas"></param>
        /// <returns></returns>
        public string ExportPDFByKITs(List<V_PLABEL_BY_KITS_PRINT> printDatas)
        {
            string fileName = string.Format("labelPrint-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullPath = BasePath + fileName;

            Document doc = new Document(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL));
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.Open();

            var headData = printDatas.FirstOrDefault();
            var bodyData = printDatas;

            #region
            //line01 
            if (headData.PX01.HasValue)
            {
                if (headData.T01 == "1D")
                {
                    GetElementPosition(writer, headData.C01, "TXT", headData.PX01, (headData.PY01 + 22), headData.S01, 1);
                }

                GetElementPosition(writer, headData.C01, headData.T01, headData.PX01, headData.PY01, headData.S01, 1, headData.SIZE_2D);
            }
            //line02
            if (headData.PX02.HasValue)
            {
                if (headData.T02 == "1D")
                {
                    GetElementPosition(writer, headData.C02, "TXT", headData.PX02, (headData.PY02 + 22), headData.S02, 2);
                }

                GetElementPosition(writer, headData.C02, headData.T02, headData.PX02, headData.PY02, headData.S02, 2, headData.SIZE_2D);
            }
            //line03
            if (headData.PX03.HasValue)
            {
                if (headData.T03 == "1D")
                {
                    GetElementPosition(writer, headData.C03, "TXT", headData.PX03, (headData.PY03 + 22), headData.S03, 3);
                }
                GetElementPosition(writer, headData.C03, headData.T03, headData.PX03, headData.PY03, headData.S03, 3, headData.SIZE_2D);
            }
            //line04
            if (headData.PX04.HasValue)
            {
                if (headData.T04 == "1D")
                {
                    GetElementPosition(writer, headData.C04, "TXT", headData.PX04, (headData.PY04 + 22), headData.S04, 4);
                }
                GetElementPosition(writer, headData.C04, headData.T04, headData.PX04, headData.PY04, headData.S04, 4, headData.SIZE_2D);
            }
            //line05
            if (headData.PX05.HasValue)
            {
                if (headData.T05 == "1D")
                {
                    GetElementPosition(writer, headData.C05, "TXT", headData.PX05, (headData.PY05 + 22), headData.S05, 5);
                }
                GetElementPosition(writer, headData.C05, headData.T05, headData.PX05, headData.PY05, headData.S05, 5, headData.SIZE_2D);
            }
            //line06
            if (headData.PX06.HasValue)
            {
                if (headData.T06 == "1D")
                {
                    GetElementPosition(writer, headData.C06, "TXT", headData.PX06, (headData.PY06 + 22), headData.S06, 6);
                }
                GetElementPosition(writer, headData.C06, headData.T06, headData.PX06, headData.PY06, headData.S06, 6, headData.SIZE_2D);
            }
            //line07
            if (headData.PX07.HasValue)
            {
                if (headData.T07 == "1D")
                {
                    GetElementPosition(writer, headData.C07, "TXT", headData.PX07, (headData.PY07 + 22), headData.S07, 7);
                }
                GetElementPosition(writer, headData.C07, headData.T07, headData.PX07, headData.PY07, headData.S07, 7, headData.SIZE_2D);
            }
            //line08
            if (headData.PX08.HasValue)
            {
                if (headData.T08 == "1D")
                {
                    GetElementPosition(writer, headData.C08, "TXT", headData.PX08, (headData.PY08 + 22), headData.S08, 8);
                }
                GetElementPosition(writer, headData.C08, headData.T08, headData.PX08, headData.PY08, headData.S08, 8, headData.SIZE_2D);
            }
            //line09
            if (headData.PX09.HasValue)
            {
                if (headData.T09 == "1D")
                {
                    GetElementPosition(writer, headData.C09, "TXT", headData.PX09, (headData.PY09 + 22), headData.S09, 9);
                }
                GetElementPosition(writer, headData.C09, headData.T09, headData.PX09, headData.PY09, headData.S09, 9, headData.SIZE_2D);
            }
            //line10
            if (headData.PX10.HasValue)
            {
                if (headData.T10 == "1D")
                {
                    GetElementPosition(writer, headData.C10, "TXT", headData.PX10, (headData.PY10 + 22), headData.S10, 10);
                }
                GetElementPosition(writer, headData.C10, headData.T10, headData.PX10, headData.PY10, headData.S10, 10, headData.SIZE_2D);
            }
            //line11
            if (headData.PX11.HasValue)
            {
                if (headData.T11 == "1D")
                {
                    GetElementPosition(writer, headData.C11, "TXT", headData.PX11, (headData.PY11 + 22), headData.S11, 11);
                }

                GetElementPosition(writer, headData.C11, headData.T11, headData.PX11, headData.PY11, headData.S11, 11, headData.SIZE_2D);
            }
            //line12
            if (headData.PX12.HasValue)
            {
                if (headData.T12 == "1D")
                {
                    GetElementPosition(writer, headData.C12, "TXT", headData.PX12, (headData.PY12 + 22), headData.S12, 12);
                }
                GetElementPosition(writer, headData.C12, headData.T12, headData.PX12, headData.PY12, headData.S12, 12, headData.SIZE_2D);
            }
            //line13  change
            if (headData.PX13.HasValue)
            {
                if (headData.T13 == "1D")
                {
                    GetElementPosition(writer, headData.C13, "TXT", headData.PX13, (headData.PY13 + 22), headData.S13, 13);
                }
                GetElementPosition(writer, headData.C13, headData.T13, headData.PX13, headData.PY13, headData.S13, 13, headData.SIZE_2D);
            }
            //line14
            if (headData.PX14.HasValue)
            {
                if (headData.T14 == "1D")
                {
                    GetElementPosition(writer, headData.C14, "TXT", headData.PX14, (headData.PY14 + 22), headData.S14, 14);
                }
                GetElementPosition(writer, headData.C14, headData.T14, headData.PX14, headData.PY14, headData.S14, 14, headData.SIZE_2D);
            }
            //line15
            if (headData.PX15.HasValue)
            {
                if (headData.T15 == "1D")
                {
                    GetElementPosition(writer, headData.C15.Value.ToString(), "TXT", headData.PX15, (headData.PY15 + 22), headData.S15, 15);
                }
                GetElementPosition(writer, headData.C15.Value.ToString(), headData.T15, headData.PX15, headData.PY15, headData.S15, 15, headData.SIZE_2D);
            }
            //line16
            if (headData.PX16.HasValue)
            {
                if (headData.T16 == "1D")
                {
                    GetElementPosition(writer, headData.C16, "TXT", headData.PX16, (headData.PY16 + 22), headData.S16, 16);
                }
                GetElementPosition(writer, headData.C16, headData.T16, headData.PX16, headData.PY16, headData.S16, 16, headData.SIZE_2D);
            }
            //line17
            if (headData.PX17.HasValue)
            {
                if (headData.T17 == "1D")
                {
                    GetElementPosition(writer, headData.C17, "TXT", headData.PX17, (headData.PY17 + 22), headData.S17, 17);
                }
                GetElementPosition(writer, headData.C17, headData.T17, headData.PX17, headData.PY17, headData.S17, 17, headData.SIZE_2D);
            }
            //line18
            if (headData.PX18.HasValue)
            {
                if (headData.T18 == "1D")
                {
                    GetElementPosition(writer, headData.C18, "TXT", headData.PX18, (headData.PY18 + 22), headData.S18, 18);
                }
                GetElementPosition(writer, headData.C18, headData.T18, headData.PX18, headData.PY18, headData.S18, 18, headData.SIZE_2D);
            }
            //line19
            if (headData.PX19.HasValue)
            {
                if (headData.T19 == "1D")
                {
                    GetElementPosition(writer, headData.C19, "TXT", headData.PX19, (headData.PY19 + 22), headData.S19, 19);
                }
                GetElementPosition(writer, headData.C19, headData.T19, headData.PX19, headData.PY19, headData.S19, 19, headData.SIZE_2D);
            }
            //line20
            if (headData.PX20.HasValue)
            {
                if (headData.T20 == "1D")
                {
                    GetElementPosition(writer, headData.C20, "TXT", headData.PX20, (headData.PY20 + 22), headData.S20, 20);
                }
                GetElementPosition(writer, headData.C20, headData.T20, headData.PX20, headData.PY20, headData.S20, 20, headData.SIZE_2D);
            }
            //line21
            if (headData.PX21.HasValue)
            {
                if (headData.T21 == "1D")
                {
                    GetElementPosition(writer, headData.C21, "TXT", headData.PX21, (headData.PY21 + 22), headData.S21, 21);
                }
                GetElementPosition(writer, headData.C21, headData.T21, headData.PX21, headData.PY21, headData.S21, 21, headData.SIZE_2D);
            }
            //line22
            if (headData.PX22.HasValue)
            {
                if (headData.T22 == "1D")
                {
                    GetElementPosition(writer, headData.C22, "TXT", headData.PX22, (headData.PY22 + 22), headData.S22, 22);
                }
                GetElementPosition(writer, headData.C22, headData.T22, headData.PX22, headData.PY22, headData.S22, 22, headData.SIZE_2D);
            }
            //line23
            if (headData.PX23.HasValue)
            {
                if (headData.T23 == "1D")
                {
                    GetElementPosition(writer, headData.C23, "TXT", headData.PX23, (headData.PY23 + 22), headData.S23, 23);
                }
                GetElementPosition(writer, headData.C23, headData.T23, headData.PX23, headData.PY23, headData.S23, 23, headData.SIZE_2D);
            }
            //line24
            if (headData.PX24.HasValue)
            {
                if (headData.T24 == "1D")
                {
                    GetElementPosition(writer, headData.C24, "TXT", headData.PX24, (headData.PY24 + 22), headData.S24, 24);
                }
                GetElementPosition(writer, headData.C24, headData.T24, headData.PX24, headData.PY24, headData.S24, 24, headData.SIZE_2D);
            }
            #endregion

            var height = headData.PY21.Value;
            var width = ValueConvert.MM2Pixel(printDatas[0].PAPER_HORIZONTAL);
            PdfContentByte cb = writer.DirectContent;
            cb.SetLineWidth(1f);
            cb.MoveTo((float)headData.PX21, (float)(height - 1));
            cb.LineTo((float)(width - headData.PX21), (float)(height - 1));
            cb.Stroke();
            for (int i = 0; i < bodyData.Count(); i++)
            {
                height = height - 10;
                if (height < 10)
                {
                    height = ValueConvert.MM2Pixel(printDatas[0].PAPER_VERTICAL) - ValueConvert.MM2Pixel(30);
                    doc.SetPageSize(pdfUtility.GetDocRectangle(printDatas[0].PAPER_HORIZONTAL, printDatas[0].PAPER_VERTICAL));
                    doc.NewPage();
                }
                GetElementPosition(writer, bodyData[i].D21, "TXT", headData.PX21, height, headData.S21, 21);
                GetElementPosition(writer, bodyData[i].D22.Value.ToString(), "TXT", headData.PX22, height, headData.S22, 22);
                GetElementPosition(writer, bodyData[i].D23, "TXT", headData.PX23, height, headData.S23, 23);
                GetElementPosition(writer, bodyData[i].D24, "TXT", headData.PX24, height, headData.S24, 24);
            }
            doc.Close();

            return fileName;
        }



        public void PrintSetting(string filepath)
        {
            PrintDocument printDoc = new PrintDocument();   //打印

            Spire.Pdf.PdfDocument pdfdoc = new Spire.Pdf.PdfDocument(); //PDF文档
            pdfdoc.LoadFromFile(filepath);
            pdfdoc.PageScaling = Spire.Pdf.PdfPrintPageScaling.ActualSize;
            var a = pdfdoc.PageSettings.Height;
            var b = pdfdoc.PageSettings.Width;
            //for (int i = 0; i < pdfdoc.Pages.Count; i++)
            //{
            //    Spire.Pdf.PdfPageBase page = pdfdoc.Pages[i];
            //    int rotation = (int)page.Rotation;
            //    rotation += (int)Spire.Pdf.PdfPageRotateAngle.RotateAngle270;
            //    page.Rotation = (Spire.Pdf.PdfPageRotateAngle)rotation;
            //}
            pdfdoc.PrinterName = "WGQ";
            printDoc = pdfdoc.PrintDocument;

            PrintDialog printDialog = new PrintDialog();    //打印设置
            printDialog.PrinterSettings.Copies = 1;
            printDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            printDialog.Document = printDoc;

            //PrintPreviewDialog pp = new PrintPreviewDialog();   //页面预览
            //pp.Document = printDoc;
            //pp.ShowDialog();

            //printDoc.Print();
        }

        /// <summary>
        /// Bartender Print
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ModelPath"></param>
        /// <param name="Printer_Name"></param>
        public void Bartender_Print(List<BartenderPrintDatas> list, string ModelPath, string Printer_Name)
        {
            //Process pro = new Process();
            //BarTender.Application btapp;
            //BarTender.Format btformat;
            Engine btEngine = new Engine();
            try
            {
                #region Bartender OLD
                //TxtLog.WriteLog("打印准备开始");
                //btapp = new BarTender.Application();
                //btformat = btapp.Formats.Open(@"D:\IIS\CHub\Template\BASE.btw");
                //btformat.PrintSetup.IdenticalCopiesOfLabel = 1;
                ////btformat.SetNamedSubStringValue("C10", li.C10);
                ////btformat.SetNamedSubStringValue("C11", li.C11);
                ////btformat.SetNamedSubStringValue("C12", li.C12);
                ////btformat.SetNamedSubStringValue("C13", li.C13);
                ////btformat.SetNamedSubStringValue("C14", li.C14);
                ////btformat.SetNamedSubStringValue("C15", li.C15);
                ////btformat.SetNamedSubStringValue("C16", li.C16);
                ////btformat.SetNamedSubStringValue("C17", li.C17);
                ////btformat.SetNamedSubStringValue("C18", li.C18);
                ////btformat.SetNamedSubStringValue("C19", li.C19);
                ////btformat.SetNamedSubStringValue("C20", li.C20);
                ////btformat.SetNamedSubStringValue("C21", li.C21);
                ////btformat.SetNamedSubStringValue("C22", li.C22);
                ////btformat.SetNamedSubStringValue("C23", li.C23);
                ////btformat.SetNamedSubStringValue("C24", li.C24);

                //btformat.PrintSetup.Printer = "WGQ";
                //TxtLog.WriteLog("开始打印");
                //btformat.PrintOut(true, false);
                //btformat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges); //不保存
                //btapp.Quit(BarTender.BtSaveOptions.btSaveChanges); //退出时同步退出bartender进程
                //TxtLog.WriteLog("打印结束，进程已关闭");
                #endregion
                #region OLD
                //foreach (var li in list)
                //{
                //    string C10 = !string.IsNullOrEmpty(li.C10) ? li.C10.Replace(" ", "/!") : "";
                //    string C11 = !string.IsNullOrEmpty(li.C11) ? li.C11.Replace(" ", "/!") : "";
                //    string C12 = !string.IsNullOrEmpty(li.C12) ? li.C12.Replace(" ", "/!") : "";
                //    string C13 = !string.IsNullOrEmpty(li.C13) ? li.C13.Replace(" ", "/!") : "";
                //    string C14 = !string.IsNullOrEmpty(li.C14) ? li.C14.Replace(" ", "/!") : "";
                //    string C15 = !string.IsNullOrEmpty(li.C15) ? li.C15.Replace(" ", "/!") : "";
                //    string C16 = !string.IsNullOrEmpty(li.C16) ? li.C16.Replace(" ", "/!") : "";
                //    string C17 = !string.IsNullOrEmpty(li.C17) ? li.C17.Replace(" ", "/!") : "";
                //    string C18 = !string.IsNullOrEmpty(li.C18) ? li.C18.Replace(" ", "/!") : "";
                //    string C19 = !string.IsNullOrEmpty(li.C19) ? li.C19.Replace(" ", "/!") : "";
                //    string C20 = !string.IsNullOrEmpty(li.C20) ? li.C20.Replace(" ", "/!") : "";
                //    string C21 = !string.IsNullOrEmpty(li.C21) ? li.C21.Replace(" ", "/!") : "";
                //    string C22 = !string.IsNullOrEmpty(li.C22) ? li.C22.Replace(" ", "/!") : "";
                //    string C23 = !string.IsNullOrEmpty(li.C23) ? li.C23.Replace(" ", "/!") : "";
                //    string C24 = !string.IsNullOrEmpty(li.C24) ? li.C24.Replace(" ", "/!") : "";
                //    int copies = li.Copies;

                //    pro.StartInfo.RedirectStandardInput = true;
                //    pro.StartInfo.RedirectStandardOutput = true;
                //    pro.StartInfo.UseShellExecute = false;
                //    pro.StartInfo.CreateNoWindow = true;
                //    pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                //    pro.StartInfo.FileName = @"C:\Users\lg166a\Desktop\BartenderPrint\BartenderPrint\bin\Debug\BartenderPrint.exe"; //@"C:\Users\oo450\Documents\visual studio 2015\Projects\BartenderPrint\BartenderPrint\bin\Debug\BartenderPrint.exe";
                //    //string arg = "" + C10 + " " + C11 + " " + C12 + " " + C13 + " " + C14 + " " + C15 + " " + C16 + " " + C17 + " " + C18 + " " + C19 + " " + C20 + " " + C21 + " " + C22 + " " + C23 + " " + C24 + " " + copies + " " + ModelPath + " " + Printer_Name + "";
                //    string arg = "" + C10 + "|" + C11 + "|" + C12 + "|" + C13 + "|" + C14 + "|" + C15 + "|" + C16 + "|" + C17 + "|" + C18 + "|" + C19 + "|" + C20 + "|" + C21 + "|" + C22 + "|" + C23 + "|" + C24 + "|" + copies + "|" + ModelPath + "|" + Printer_Name + "";

                //    pro.StartInfo.Arguments = arg;
                //    pro.Start();
                //    pro.WaitForExit();
                //    pro.Close();
                //}
                #endregion
                #region
                //Engine btEngine = new Engine(true);
                //TxtLog.WriteLog("引擎启动");
                //LabelFormatDocument btFormat = btEngine.Documents.Open(ModelPath); //C:\Users\oo450\Desktop //D:\IIS\CHub\Template
                //btFormat.PrintSetup.PrinterName = Printer_Name;
                //btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                #endregion
                bool IsAlive = btEngine.IsAlive;
                TxtLog.WriteLog(IsAlive ? "启动中" : "未启动");
                if (IsAlive)
                    btEngine.Stop();
                btEngine.Start();
                TxtLog.WriteLog("程序启动");
                LabelFormatDocument btFormat = btEngine.Documents.Open(ModelPath);
                btFormat.PrintSetup.PrinterName = Printer_Name;
                foreach (var li in list)
                {
                    btFormat.SubStrings["C10"].Value = !string.IsNullOrEmpty(li.C10) ? li.C10 : "";
                    btFormat.SubStrings["C11"].Value = !string.IsNullOrEmpty(li.C11) ? li.C11 : "";
                    btFormat.SubStrings["C12"].Value = !string.IsNullOrEmpty(li.C12) ? li.C12 : "";
                    btFormat.SubStrings["C13"].Value = !string.IsNullOrEmpty(li.C13) ? li.C13 : "";
                    btFormat.SubStrings["C14"].Value = !string.IsNullOrEmpty(li.C14) ? li.C14 : "";
                    btFormat.SubStrings["C15"].Value = !string.IsNullOrEmpty(li.C15) ? li.C15 : "";
                    btFormat.SubStrings["C16"].Value = !string.IsNullOrEmpty(li.C16) ? li.C16 : "";
                    btFormat.SubStrings["C17"].Value = !string.IsNullOrEmpty(li.C17) ? li.C17 : "";
                    btFormat.SubStrings["C18"].Value = !string.IsNullOrEmpty(li.C18) ? li.C18 : "";
                    btFormat.SubStrings["C19"].Value = !string.IsNullOrEmpty(li.C19) ? li.C19 : "";
                    btFormat.SubStrings["C20"].Value = !string.IsNullOrEmpty(li.C20) ? li.C20 : "";
                    btFormat.SubStrings["C21"].Value = !string.IsNullOrEmpty(li.C21) ? li.C21 : "";
                    btFormat.SubStrings["C22"].Value = !string.IsNullOrEmpty(li.C22) ? li.C22 : "";
                    btFormat.SubStrings["C23"].Value = !string.IsNullOrEmpty(li.C23) ? li.C23 : "";
                    btFormat.SubStrings["C24"].Value = !string.IsNullOrEmpty(li.C24) ? li.C24 : "";
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = li.Copies;//打印份数

                    TxtLog.WriteLog("开始打印");
                    btFormat.Print();
                    TxtLog.WriteLog("结束打印");
                }
                btEngine.Documents.Close(ModelPath, SaveOptions.DoNotSaveChanges);
                btEngine.Stop();
                TxtLog.WriteLog("-------------------打印完成");
            }
            catch (Exception ex)
            {
                btEngine.Stop();
                TxtLog.WriteLog(ex.Message);
            }
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
        public void GetElementPosition(PdfWriter writer, string C, string T, decimal? PX, decimal? PY, decimal? S, int i, decimal? SIZE_2D = null)
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
                    //img = pdfUtility.GetCode39Img(!string.IsNullOrEmpty(C) ? C : "", (int)S);
                    img.SetAbsolutePosition(PX.HasValue ? (float)PX.Value - 5 : 0, PY.HasValue ? (float)PY.Value : 0);
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
                    img.ScaleAbsoluteHeight(SIZE_2D.HasValue ? (float)SIZE_2D : 40f);
                    img.ScaleAbsoluteWidth(SIZE_2D.HasValue ? (float)SIZE_2D : 40f);
                    cb.AddImage(img);
                    break;
                case "PIC":
                    img = Image.GetInstance(imagepath + C); //正式 C:\inetpub\wwwroot\CHub\Images\ //测试 D:\IIS\CHub\Images\  //本地 C:\Users\oo450\Cummins Project\ChubPublish\Images\
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



        /// <summary>
        /// IA/V_IA_REPORT_PRINT
        /// </summary>
        /// <param name="printDatas"></param>
        /// <returns></returns>
        public string BuildPdfForIAReport(List<V_IA_REPORT_PRINT> printDatas)
        {
            string filename = string.Format(@"IA_Report-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullpath = BasePath + filename;

            Rectangle pagesize = null;
            decimal horizontal = ValueConvert.MM2Pixel(printDatas[0].PAPER_H.Value); //
            decimal vertical = ValueConvert.MM2Pixel(printDatas[0].PAPER_V.Value); //
            pagesize = new Rectangle((float)horizontal, (float)vertical);
            PDFBase pb = new PDFBase(BF_Light, printDatas[0].FOOTER);
            Document doc = new Document(pagesize);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullpath, FileMode.Create));
            writer.PageEvent = pb;
            doc.Open();

            #region OLD
            //PdfPTable table = new PdfPTable(9);
            //table.WidthPercentage = 99f;
            //PdfPCell cell;
            //cell = pdfUtility.BuildCell(printDatas[0].H01, new Font(BF_Light, HFirstFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell(printDatas[0].H02, new Font(BF_Light, HFirstFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell(printDatas[0].H03, new Font(BF_Light, HFirstFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            ////空行
            //cell = pdfUtility.BuildCell(" ", new Font(BF_Light, HFirstFontSize));
            //cell.Colspan = 9;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);

            //cell = pdfUtility.BuildCell(printDatas[0].H04, new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell(printDatas[0].H05, new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell(printDatas[0].H06, new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);

            //cell = pdfUtility.BuildCell(printDatas[0].H07, new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell("", new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell("", new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);

            //cell = pdfUtility.BuildCell("", new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell("", new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //cell = pdfUtility.BuildCell(printDatas[0].H12, new Font(BF_Light, HMiddleFontSize));
            //cell.Colspan = 3;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);

            ////空行
            //cell = pdfUtility.BuildCell(" ", new Font(BF_Light, HFirstFontSize));
            //cell.Colspan = 9;
            //cell.BorderWidth = 0f;
            //table.AddCell(cell);
            //doc.Add(table);


            //PdfPTable table1 = new PdfPTable(9);
            //table1.WidthPercentage = 99f;
            //PdfPCell cell1;
            //cell1 = pdfUtility.BuildCell("SEQ", new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T1, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T2, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T3, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T4, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T5, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T6, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell("", new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);
            //cell1 = pdfUtility.BuildCell(printDatas[0].T8, new Font(BF_Light, HEndFontSize), BaseColor.GRAY);
            //table1.AddCell(cell1);

            //for (int i = 0; i < printDatas.Count(); i++)
            //{
            //    //if (i > 50)
            //    //{
            //    //    doc.SetPageSize(pagesize);
            //    //    doc.NewPage();
            //    //}

            //    cell1 = pdfUtility.BuildCell((i + 1).ToString(), new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D1, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D2, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D3, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D4, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D5, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D6, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell("", new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //    cell1 = pdfUtility.BuildCell(printDatas[i].D8, new Font(BF_Light, HEndFontSize));
            //    table1.AddCell(cell1);
            //}
            //table1.SetTotalWidth(new float[] { 5f, 12f, 20f, 5f, 5f, 12f, 12f, 12f, 12f });
            //doc.Add(table1);
            #endregion

            Image img = pdfUtility.GetCode128Img(printDatas[0].H01, 9);
            img.ScaleAbsoluteHeight(25f);
            img.ScaleAbsoluteWidth(120f);

            #region 表头
            PdfPTable table1 = new PdfPTable(3);
            PdfPCell cell1;
            table1.WidthPercentage = 99f;

            cell1 = new PdfPCell(img);
            //cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.BorderWidth = 0f;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph(printDatas[0].H02, new Font(BF_Light, HFirstFontSize)));
            cell1.Rowspan = 2;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.BorderWidth = 0f;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph(printDatas[0].H03, new Font(BF_Light, HFirstFontSize)));
            cell1.Rowspan = 2;
            //cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.BorderWidth = 0f;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph(printDatas[0].H01, new Font(BF_Light, HFirstFontSize)));
            cell1.BorderWidth = 0f;
            //cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.AddCell(cell1);

            doc.Add(table1);
            #endregion

            //空行
            Paragraph blank1 = new Paragraph(18f, " ", new Font(BF_Light, HFirstFontSize));
            doc.Add(blank1);

            #region 副表头
            PdfPTable table2 = new PdfPTable(3);
            PdfPCell cell2;
            table2.WidthPercentage = 99f;

            cell2 = new PdfPCell(new Paragraph(printDatas[0].H04, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H05, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H06, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H07, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            cell2.Colspan = 3;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H08, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            cell2.Colspan = 3;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H09, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            cell2.Colspan = 3;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H10, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H11, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(printDatas[0].H12, new Font(BF_Light, HMiddleFontSize)));
            cell2.BorderWidth = 0f;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);

            doc.Add(table2);
            #endregion

            //空行
            Paragraph blank2 = new Paragraph(18f, " ", new Font(BF_Light, HFirstFontSize));
            doc.Add(blank2);


            #region 明细
            PdfPTable table3 = new PdfPTable(9);
            PdfPCell cell3;
            table3.WidthPercentage = 99f;
            table3.SetTotalWidth(new float[] { 5f, 12f, 18f, 23f, 5f, 5f, 10f, 10f, 12f }); //每列宽度
            table3.SplitLate = true;
            table3.SplitRows = false;

            #region 表头
            cell3 = new PdfPCell(new Paragraph("SEQ", new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T1, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T2, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T3, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T4, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T5, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T6, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T7, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            cell3 = new PdfPCell(new Paragraph(printDatas[0].T8, new Font(BF_Light, HEndFontSize)));
            cell3.BackgroundColor = BaseColor.GRAY;
            table3.AddCell(cell3);
            #endregion

            for (int i = 0; i < printDatas.Count(); i++)
            {
                cell3 = new PdfPCell(new Paragraph((i + 1).ToString(), new Font(BF_Light, HEndFontSize)));
                cell3.FixedHeight = 30f;
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
                cell3 = new PdfPCell(new Paragraph(printDatas[i].D1, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);

                Image img2 = pdfUtility.GetCode128Img(printDatas[i].D2, 9);
                img2.ScaleAbsoluteHeight(22f);
                img2.ScaleAbsoluteWidth(86f);
                cell3 = new PdfPCell(img2);
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                table3.AddCell(cell3);

                cell3 = new PdfPCell(new Paragraph(printDatas[i].D3, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
                cell3 = new PdfPCell(new Paragraph(printDatas[i].D4, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
                cell3 = new PdfPCell(new Paragraph(printDatas[i].D5, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
                cell3 = new PdfPCell(new Paragraph(printDatas[i].D6, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
                cell3 = new PdfPCell(new Paragraph(printDatas[i].D7, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
                cell3 = new PdfPCell(new Paragraph(printDatas[i].D8, new Font(BF_Light, HEndFontSize)));
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                table3.AddCell(cell3);
            }

            doc.Add(table3);
            #endregion

            doc.Close();
            return filename;
        }
    }
}

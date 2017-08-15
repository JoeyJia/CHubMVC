using CHubDBEntity.UnmanagedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using CHubCommon;
using System.Drawing;
using CHubDBEntity;
using ThoughtWorks.QRCode.Codec;
using CHubModel.ExtensionModel;
using CHubCommon.Printer;
using CHubCommon.PDFHelper;

namespace CHubBLL.OtherProcess
{
    public class CustPackPrintBLL
    {
        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        private int ContentFontSize = 10;
        private int BigTableFontSize = 10;
        private int TableFontSize = 8;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 8;

        private PDFUtility pdfUtility;
        public CustPackPrintBLL(string basePath=null)
        {
            //if (basePath == null)
            //    ; ;
            this.BasePath = basePath;
            pdfUtility = new PDFUtility();
        }

        public void GetStagedPackList(string appUser)
        {
            V_RP_PACK_H_QUEUE_BLL qBLL = new V_RP_PACK_H_QUEUE_BLL();
            APP_WH_BLL whBLL = new APP_WH_BLL();
            PrintHelper pHelper = new PrintHelper();

            List<string> dWHID = qBLL.GetDistinctWHID();

            if (dWHID == null || dWHID.Count == 0)
            {
                Console.WriteLine("NO queue Data");
                return;
            }

            foreach (var id in dWHID)
            {

                List<string> lodList = qBLL.GetLodNumByWhID(id);

                if (lodList == null || lodList.Count == 0)
                {
                    Console.WriteLine("No staged pack data  found");
                    return;
                }

                string fileName = PrintPackData(lodList, appUser);
                string fullPath = BasePath + fileName;

                string defPrinter = whBLL.GetDefPrinter(id);

                if (string.IsNullOrEmpty(defPrinter))
                {
                    Console.WriteLine("No printer setting for WareHous:"+id);
                    return;
                }

                pHelper.PrintFile(fullPath, defPrinter);

                string msg = "";
                foreach (var item in lodList)
                {
                    msg += (item + "|");
                }
                Console.WriteLine("{0} Pack Print job completed successfully!", DateTime.Now.ToString());
                Console.WriteLine(msg);
                Console.WriteLine(fileName);
                Console.WriteLine("******************************************");
            }
        }

        public string PrintPackData(List<string> lodList, string appUser)
        {
            List<PackPageData> pageDatas = new List<PackPageData>();
            try
            {
                
                foreach (var lodNum in lodList)
                {
                    PackPageData page = new PackPageData();
                    V_RP_PACK_H_PRINT_BLL hBLL = new V_RP_PACK_H_PRINT_BLL();
                    var header = hBLL.GetPackHeader(lodNum);

                    V_RP_PACK_D_PRINT_BLL dBLL = new V_RP_PACK_D_PRINT_BLL();
                    var detail = dBLL.GetPackDetails(lodNum);
                    page.Header = header;
                    page.Details = detail;
                    pageDatas.Add(page);

                }

                if (pageDatas.Count == 0)
                    throw new Exception("Fail to get PageData");

                    string fileName = BuildPrintFile(pageDatas, appUser);
                AddPackLogs(pageDatas, CHubConstValues.IndY);
                return fileName;
            }
            catch(Exception ex)
            {
                string ss = ex.Message;
                //log fail status
                AddPackLogs(pageDatas, CHubConstValues.IndN);
                throw;
            }

        }

        public string BuildPrintFile(List<PackPageData> pageDatas, string appUser)
        {
            string fileName = string.Format("CustPack-{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmm"));
            string fullPath = BasePath + fileName;


            List<string> sData = new List<string>();

            //each page size get from header ,
            Document doc = new Document(pdfUtility.GetDocRectangle(pageDatas[0].Header.PAPER_HORIZONTAL, pageDatas[0].Header.PAPER_VERTICAL));

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));

            //string imagePath = BasePath.Replace("temp", "images") + pageDatas[0].Header.LOGO; //；"custpack.png";
            PackPageEventHelper pHelper = new PackPageEventHelper();

            if (!string.IsNullOrEmpty(pageDatas[0].Header.LOGO))
                pHelper.LogoPath = BasePath.Replace("temp", "images") + pageDatas[0].Header.LOGO;
            writer.PageEvent = pHelper;

            doc.SetMargins(10f, 10f, 10f,36f);

            pHelper.CurrentGroup = pageDatas[0].Header.LODNUM;
            for (int i = 0; i < pageDatas.Count; i++)
            {
                //pHelper.CurrentGroup = pageDatas[i].Header.SHIP_ID;
                if (i != 0)
                {
                    doc.SetPageSize(pdfUtility.GetDocRectangle(pageDatas[i].Header.PAPER_HORIZONTAL, pageDatas[i].Header.PAPER_VERTICAL));
                    doc.NewPage();
                }
                else
                    doc.Open();


                PdfPCell cellUnit;
                //Content part
                PdfPTable contentTable = new PdfPTable(3);
                contentTable.WidthPercentage = 90f;
                contentTable.SetWidths(new float[] { 200f,190f,185f });//575
                
                cellUnit = new PdfPCell(new Paragraph(pageDatas[i].Header.HEADER1, new iTextSharp.text.Font(BF_Light, HeaderFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);
                cellUnit = new PdfPCell(new Paragraph(pageDatas[i].Header.HEADER2, new iTextSharp.text.Font(BF_Light, HeaderFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);
                cellUnit = new PdfPCell(new Paragraph(pageDatas[i].Header.HEADER3, new iTextSharp.text.Font(BF_Light, HeaderFontSize)));
                cellUnit.BorderWidth = 0;
                contentTable.AddCell(cellUnit);
                doc.Add(contentTable);
                doc.Add(new Paragraph(Environment.NewLine));

                //part 2
                PdfPTable table_2 = new PdfPTable(2);
                table_2.WidthPercentage = 90f;
                table_2.SetWidths(new float[] { 285f,285f });//575
                Paragraph p21 = new Paragraph();
                p21.Add(new Phrase(string.Format("{0}    {1}", pageDatas[i].Header.NOTE1, pageDatas[i].Header.FLEX1), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p21.Add(System.Environment.NewLine);
                p21.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.COMPANY), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p21.Add(System.Environment.NewLine);
                p21.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.ADDRESS), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p21.Add(System.Environment.NewLine);
                p21.Add(new Phrase(string.Format("{0}    {1}", pageDatas[i].Header.CONTACT, pageDatas[i].Header.TELEPHONE), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p21.Add(System.Environment.NewLine);
                cellUnit = new PdfPCell(p21);
                cellUnit.BorderWidth = 0;
                table_2.AddCell(cellUnit);

                Paragraph p22 = new Paragraph();
                p22.Add(new Phrase(string.Format("{0}    {1}", pageDatas[i].Header.NOTE2, pageDatas[i].Header.FLEX2), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p22.Add(System.Environment.NewLine);
                p22.Add(new Phrase(string.Format("{0}    {1}", pageDatas[i].Header.ADRNAM, pageDatas[i].Header.ADRCTY), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p22.Add(System.Environment.NewLine);
                p22.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.ADRLN1), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p22.Add(System.Environment.NewLine);
                p22.Add(new Phrase(string.Format("{0}    {1}", pageDatas[i].Header.ADRLN2, pageDatas[i].Header.ADRLN3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p22.Add(System.Environment.NewLine);
                p22.Add(new Phrase(string.Format("{0}    {1}", pageDatas[i].Header.LAST_NAME, pageDatas[i].Header.PHNNUM), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p22.Add(System.Environment.NewLine);
                cellUnit = new PdfPCell(p22);
                cellUnit.BorderWidth = 0;
                table_2.AddCell(cellUnit);

                doc.Add(table_2);
                doc.Add(new Paragraph(Environment.NewLine));

                //part 3
                PdfPTable table_3 = new PdfPTable(3);
                table_3.WidthPercentage =90f;
                table_3.SetWidths(new float[] { 200f, 190f, 185f });//575
                Paragraph p31 = new Paragraph();
                p31.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.NOTE3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p31.Add(System.Environment.NewLine);
                p31.Add(System.Environment.NewLine);
                p31.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.FLEX3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit = new PdfPCell(p31);
                cellUnit.BorderWidth = 0;
                table_3.AddCell(cellUnit);

                Paragraph p32 = new Paragraph();
                p32.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.NOTE4), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p32.Add(System.Environment.NewLine);
                p32.Add(System.Environment.NewLine);
                p32.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.FLEX4), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit = new PdfPCell(p32);
                cellUnit.BorderWidth = 0;
                table_3.AddCell(cellUnit);

                Paragraph p33 = new Paragraph();
                p33.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.NOTE5), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                p33.Add(System.Environment.NewLine);
                p33.Add(System.Environment.NewLine);
                p33.Add(new Phrase(string.Format("{0}", pageDatas[i].Header.FLEX5), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                cellUnit = new PdfPCell(p33);
                cellUnit.BorderWidth = 0;
                table_3.AddCell(cellUnit);

                doc.Add(table_3);
                doc.Add(new Paragraph(Environment.NewLine));

                //Table part
                PdfPTable dTable = new PdfPTable(11);
                dTable.WidthPercentage = 90f;
                dTable.SetWidths(new float[] { 30f, 52f, 98f, 25f, 30f, 47f, 47f, 47f, 47f, 47f, 47f });
                dTable.AddCell(pdfUtility.BuildCell("Index", new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL01, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL02, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL03, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL04, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL05, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL06, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL07, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL08, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL09, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(pdfUtility.BuildCell(pageDatas[i].Header.COL10, new iTextSharp.text.Font(BF_Light, TableFontSize)));

                if (pageDatas[i].Details != null && pageDatas[i].Details.Count > 0)
                {
                    //order 
                    //pageDatas[i].Details.OrderBy(a => a.COL01);

                    int indexPoint = 1;
                    foreach (var item in pageDatas[i].Details)
                    {
                        dTable.AddCell(pdfUtility.BuildCell(indexPoint.ToString(), new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        indexPoint++;
                        dTable.AddCell(pdfUtility.BuildCell(item.COL01, new iTextSharp.text.Font(BF_Light, BigTableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL02, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL03, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL04, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL05, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL06, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL07, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL08, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL09, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(pdfUtility.BuildCell(item.COL10, new iTextSharp.text.Font(BF_Light, TableFontSize)));

                    }
                    doc.Add(dTable);

                    Paragraph pLast = new Paragraph(string.Format("Total Items:{0}          ", pageDatas[i].Details.Count.ToString()), new iTextSharp.text.Font(BF_Light, ContentFontSize));
                    pLast.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(pLast);
                }

                //footer  part
                //
                sData.Clear();
                sData.Add(pageDatas[i].Header.FOOTER1??string.Empty);
                sData.Add(pageDatas[i].Header.FOOTER2 ?? string.Empty);
                sData.Add(pageDatas[i].Header.FOOTER3 ?? string.Empty);

                PdfContentByte cb = writer.DirectContent;
                ColumnText ct = new ColumnText(cb);
                cb.BeginText();
                cb.SetFontAndSize(BF_Light, FooterFontSize);
                cb.SetTextMatrix(doc.LeftMargin, doc.BottomMargin);
                cb.ShowText(pdfUtility.GetLineString(sData));
                cb.EndText();

                if ((i+1)< pageDatas.Count)
                {
                    pHelper.CurrentGroup = pageDatas[i+1].Header.LODNUM;
                    if (!string.IsNullOrEmpty(pageDatas[i + 1].Header.LOGO))
                        pHelper.LogoPath = BasePath.Replace("temp", "images") + pageDatas[i+1].Header.LOGO;
                    else
                        pHelper.LogoPath = null;
                }

            }

            doc.Close();

            //add track data

            //string sourceString1 = hData.SHIP_ID + "C";
            //RP_SHIP_TRACK_BLL trackBLL = new RP_SHIP_TRACK_BLL();
            //foreach (var item in dPrintList)
            //{
            //    RP_SHIP_TRACK track = new RP_SHIP_TRACK();
            //    track.WH_ID = item.WH_ID;
            //    track.SHIP_ID = item.SHIP_ID;

            //    if (wbType.TRACK_NUM_BY_IHUB == CHubConstValues.IndY)
            //        track.TRACK_NUM_IHUB = sourceString1;
            //    else
            //        track.TRACK_NUM_IHUB = "IHUB_Printed";

            //    track.RECORD_DATE = DateTime.Now;
            //    track.UPDATED_BY = appUser;
            //    trackBLL.AddOrUpdate(track);
            //}


            return fileName;
        }


        //private functions
        private void AddPackLogs(List<PackPageData> pageDatas, string flag)
        {
            if (pageDatas == null || pageDatas.Count == 0)
                return;
            //add log
            RP_AUTOPACK_LOG_BLL logBLL = new RP_AUTOPACK_LOG_BLL();
            foreach (var item in pageDatas)
            {
                if (item.Details != null && item.Details.Count > 0)
                {
                    foreach (var d in item.Details)
                    {
                        if (logBLL.HasSuccessPrint(d.LODNUM))
                            continue;
                        else
                        {
                            RP_AUTOPACK_LOG model = new RP_AUTOPACK_LOG();
                            model.WH_ID = d.WH_ID;
                            model.SHIP_ID = d.SHIP_ID;
                            model.LODNUM = d.LODNUM;
                            model.AUTO_PRINT_DATE = DateTime.Now;
                            model.SUCCEE_FLAG = flag;
                            logBLL.AddOrUpdatePrintLog(model);
                        }
                    }
                }
            }
        }

    }
}

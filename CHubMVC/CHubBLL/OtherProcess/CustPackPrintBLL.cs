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

namespace CHubBLL.OtherProcess
{
    public class CustPackPrintBLL
    {
        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        private int ContentFontSize = 10;
        private int TableFontSize = 8;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 8;
        public CustPackPrintBLL(string basePath=null)
        {
            //if (basePath == null)
            //    ; ;
            this.BasePath = basePath;
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

                List<string> idList = qBLL.GetShipIDByWhID(id);

                if (idList == null || idList.Count == 0)
                {
                    Console.WriteLine("No staged pack data  found");
                    return;
                }

                string fileName = PrintPackData(idList, appUser);
                string fullPath = BasePath + fileName;

                string defPrinter = whBLL.GetDefPrinter(id);

                pHelper.PrintFile(fullPath, defPrinter);

                string msg = "";
                foreach (var item in idList)
                {
                    msg += (item + "|");
                }
                Console.WriteLine("{0} Pack Print job completed successfully!", DateTime.Now.ToString());
                Console.WriteLine(msg);
                Console.WriteLine(fileName);
                Console.WriteLine("******************************************");
            }
        }

        public string PrintPackData(List<string> idList,string appUser)
        {
            List<PackPageData> pageDatas = new List<PackPageData>();
            try
            {
                
                foreach (var id in idList)
                {
                    PackPageData page = new PackPageData();
                    V_RP_PACK_H_PRINT_BLL hBLL = new V_RP_PACK_H_PRINT_BLL();
                    var header = hBLL.GetPackHeader(id);

                    V_RP_PACK_D_PRINT_BLL dBLL = new V_RP_PACK_D_PRINT_BLL();
                    var detail = dBLL.GetPackDetails(id);
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
            Document doc = new Document(GetDocRectangle(pageDatas[0].Header.PAPER_HORIZONTAL, pageDatas[0].Header.PAPER_VERTICAL));

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));

            //string imagePath = BasePath.Replace("temp", "images") + pageDatas[0].Header.LOGO; //；"custpack.png";
            PackPageEventHelper pHelper = new PackPageEventHelper();

            if (!string.IsNullOrEmpty(pageDatas[0].Header.LOGO))
                pHelper.LogoPath = BasePath.Replace("temp", "images") + pageDatas[0].Header.LOGO;
            writer.PageEvent = pHelper;

            doc.SetMargins(10f, 10f, 10f,36f);

            pHelper.CurrentGroup = pageDatas[0].Header.SHIP_ID;
            for (int i = 0; i < pageDatas.Count; i++)
            {
                //pHelper.CurrentGroup = pageDatas[i].Header.SHIP_ID;
                if (i != 0)
                {
                    doc.SetPageSize(GetDocRectangle(pageDatas[i].Header.PAPER_HORIZONTAL, pageDatas[i].Header.PAPER_VERTICAL));
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
                dTable.SetWidths(new float[] { 30f, 47f, 98f, 30f, 30f, 47f, 47f, 47f, 47f, 47f, 47f });
                dTable.AddCell(BuildCell("Index", new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL01, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL02, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL03, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL04, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL05, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL06, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL07, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL08, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL09, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                dTable.AddCell(BuildCell(pageDatas[i].Header.COL10, new iTextSharp.text.Font(BF_Light, TableFontSize)));

                if (pageDatas[i].Details != null && pageDatas[i].Details.Count > 0)
                {
                    int indexPoint = 1;
                    foreach (var item in pageDatas[i].Details)
                    {
                        dTable.AddCell(BuildCell(indexPoint.ToString(), new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        indexPoint++;
                        dTable.AddCell(BuildCell(item.COL01, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL02, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL03, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL04, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL05, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL06, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL07, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL08, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL09, new iTextSharp.text.Font(BF_Light, TableFontSize)));
                        dTable.AddCell(BuildCell(item.COL10, new iTextSharp.text.Font(BF_Light, TableFontSize)));

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
                cb.ShowText(GetLineString(sData));
                cb.EndText();

                if ((i+1)< pageDatas.Count)
                {
                    pHelper.CurrentGroup = pageDatas[i+1].Header.SHIP_ID;
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



        


        /// <summary>
        /// base on one line spaces, to allocation position for data source
        /// font size=8, total line =130, font size=10 ,total line =~ 86
        /// </summary>
        /// <param name="data">a string list</param>
        /// <param name="totalSpace"> represent how many spaces in one line </param>
        /// <returns></returns>
        public string GetLineString(List<string> data,int totalSpace = 130)
        {
            //int RowNum = 90;
            int dataLength = 0;
            int space=0;
            foreach (var item in data)
            {
                dataLength += item.Length;
            }
            if (dataLength == 0)
                return string.Empty;
            if (dataLength > totalSpace)
            {
                space = 4;
                totalSpace = dataLength + data.Count * 4;//Give enough space
            }
            else
                space = ((totalSpace - dataLength) / (data.Count - 1));

            string result = new string(' ', totalSpace);
            int point = 0;
            for (int i = 0; i < data.Count; i++)
            {
                result = result.Substring(0,point) + data[i] + result.Substring(point + data[i].Length);
                //if not the last one
                if (i != data.Count - 1)
                    point = point + data[i].Length + space;
            }

            return result;
        }

        public PdfPCell BuildCell(string text, iTextSharp.text.Font font, BaseColor backColor=null)
        {
            PdfPCell cell = new PdfPCell(new Paragraph(text, font));
            if (backColor != null)
                cell.BackgroundColor = backColor;
            //if (width != null)
            //    cell.Width = width.Value;
            
            //cell.BorderWidth = 0;
            return cell;
        }

        public iTextSharp.text.Rectangle GetDocRectangle(decimal x, decimal y)
        {
            decimal xPixel = ValueConvert.MM2Pixel(x);
            decimal yPixel = ValueConvert.MM2Pixel(y);
            return new iTextSharp.text.Rectangle((float)xPixel, (float)yPixel);
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

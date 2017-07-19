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

namespace CHubBLL.OtherProcess
{
    public class RPWayBillPrintBLL
    {
        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        public string Code128PicPath = string.Empty;
        //just for table font here
        private int ContentFontSize = 8;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 8;
        public RPWayBillPrintBLL(string basePath)
        {
            this.BasePath = basePath;
        }

        public string BuildPrintFile(V_RP_WAYBILL_H_PRINT hData, List<V_RP_WAYBILL_D_PRINT> dPrintList, string appUser)
        {
            string fileName = string.Format("{0}-{1}-{2}-{3}.pdf",
                hData.L_ADRNAM, hData.ORDTYP_WB, hData.CARCOD, DateTime.Now.ToString("yyyyMMddHHmm"));
            string fullPath = BasePath + fileName;

            //base waybilltype to get PageSize?, and whether Including details parts
            RP_WAYBILL_TYPE_BLL wbTypeBLL = new RP_WAYBILL_TYPE_BLL();
            RP_WAYBILL_TYPE wbType = wbTypeBLL.GetSpecifyItem(hData.WAYBILL_ID);

            iTextSharp.text.Rectangle pageRec = null;
            float bottomMargin = 0f;
            decimal xPixel = ValueConvert.MM2Pixel(wbType.PAPER_HORIZONTAL);
            decimal yPixel = ValueConvert.MM2Pixel(wbType.PAPER_VERTICAL);
            pageRec = new iTextSharp.text.Rectangle((float)xPixel, (float)yPixel);

            //500m, a flag for A5 or larger than A5
            if (yPixel > 500m)
                bottomMargin = 120f;
            else
                bottomMargin = 35f;

            List<string> sData = new List<string>();
            LocalPageEventHelper leHelper = new LocalPageEventHelper();
            leHelper.hData = hData;
            leHelper.BasePath = BasePath;
            //whether print code128 part
            if (wbType.TRACK_NUM_BY_IHUB == CHubConstValues.IndY)
            {
                leHelper.printCode = true;

                //picture
                string sourceString = hData.SHIP_ID + "C";

                QRCodeEncoder qr = new QRCodeEncoder();
                qr.QRCodeScale = 3;
                qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                qr.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qr.QRCodeVersion = 1;

                Bitmap img = qr.Encode(sourceString, Encoding.ASCII);
                string imgName = Guid.NewGuid().ToString() + ".gif";
                string fullImgPath = this.BasePath + imgName;
                img.Save(fullImgPath, System.Drawing.Imaging.ImageFormat.Gif);

                leHelper.QRPath = fullImgPath;

                leHelper.codeString = string.Format("编号：{0}", hData.SHIP_ID + "C");
                
            }

            //line 1
            sData.Clear();
            sData.Add(hData.HEADER1);
            sData.Add(hData.HEADER2);
            sData.Add(hData.CARCOD);
            sData.Add(hData.CARNAM);
            sData.Add(hData.HEADER3);
            //leHelper.pLine1 =  new Paragraph(GetLineString(sData, 80), new iTextSharp.text.Font(BF_Light, HeaderFontSize));
            leHelper.line1String = GetLineString(sData, 80);

            Document doc = new Document(pageRec);
            doc.SetMargins(30f, 36f, 15f, bottomMargin);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));

            //leHelper.QRPath = fullImgPath;
            writer.PageEvent = leHelper;
            doc.Open();
            //doc.Add(GetCode128(hData.SHIP_ID));

            //Table part
            if (wbType.PRINT_DETAIL == CHubConstValues.IndY)
            {
                PdfPTable dTable = new PdfPTable(5);
                dTable.WidthPercentage = 90f;
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE1, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE2, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE3, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE4, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE5, new iTextSharp.text.Font(BF_Light, ContentFontSize)));

                foreach (var item in dPrintList)
                {
                    dTable.AddCell(BuildCell(item.SHIP_ID, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.LODNUM, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell((item.VC_PALWGT ?? 0).ToString("f2"), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.PALVOL, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.REMARK1, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                }
                doc.Add(dTable);

                Paragraph pLast = new Paragraph(string.Format("Total Items:{0}          ", dPrintList.Count), new iTextSharp.text.Font(BF_Light, 10));
                pLast.Alignment = Element.ALIGN_RIGHT;
                doc.Add(pLast);
            }

            //footer  part
            sData.Clear();
            sData.Add(hData.FOOTER1 ?? string.Empty);
            sData.Add(hData.FOOTER2 ?? string.Empty);
            sData.Add(hData.FOOTER3 ?? string.Empty);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            cb.BeginText();
            cb.SetFontAndSize(BF_Light, FooterFontSize);
            cb.SetTextMatrix(doc.LeftMargin, doc.BottomMargin);
            cb.ShowText(GetLineString(sData));
            cb.EndText();

            doc.Close();

            //add track data

            string sourceString1 = hData.SHIP_ID + "C";
            RP_SHIP_TRACK_BLL trackBLL = new RP_SHIP_TRACK_BLL();
            foreach (var item in dPrintList)
            {
                RP_SHIP_TRACK track = new RP_SHIP_TRACK();
                track.WH_ID = item.WH_ID;
                track.SHIP_ID = item.SHIP_ID;
                track.TRACK_NUM_IHUB = sourceString1;
                track.RECORD_DATE = DateTime.Now;
                track.TRACK_NUM_BY_IHUB = hData.TRACK_NUM_BY_IHUB;
                track.UPDATED_BY = appUser;
                trackBLL.AddOrUpdate(track);
            }
            return fileName;
        }


        public string BuildBatchPrintFile(List<WayBillPageData> pageDatas, string appUser)
        {
            if (pageDatas == null || pageDatas.Count == 0)
                return null;

            string fileName = string.Format("{0}-{1}.pdf",
              pageDatas[0].Header.CARCOD, DateTime.Now.ToString("yyyyMMddHHmm"));
            string fullPath = BasePath + fileName;

            //base waybilltype to get PageSize?, and whether Including details parts
            RP_WAYBILL_TYPE_BLL wbTypeBLL = new RP_WAYBILL_TYPE_BLL();
            RP_WAYBILL_TYPE wbType = wbTypeBLL.GetSpecifyItem(pageDatas[0].Header.WAYBILL_ID);

            iTextSharp.text.Rectangle pageRec = null;
            float bottomMargin = 0f;
            decimal xPixel = ValueConvert.MM2Pixel(wbType.PAPER_HORIZONTAL);
            decimal yPixel = ValueConvert.MM2Pixel(wbType.PAPER_VERTICAL);
            pageRec = new iTextSharp.text.Rectangle((float)xPixel, (float)yPixel);

            //500m, a flag for A5 or larger than A5
            if (yPixel > 500m)
                bottomMargin = 120f;
            else
                bottomMargin = 60f;
            //if (wbType.PAPER_SIZE == "A4")
            //{
            //    pageRec = PageSize.A4;
            //    bottomMargin = 120f;
            //}
            //if (wbType.PAPER_SIZE == "A5")
            //{
            //    pageRec = PageSize.A5.Rotate();
            //    bottomMargin = 60f;
            //}

            List<string> sData = new List<string>();
            LocalPageEventHelper leHelper = new LocalPageEventHelper();
            //leHelper.hData = hData;
            leHelper.BasePath = BasePath;

            Document doc = new Document(pageRec);
            doc.SetMargins(30f, 36f, 15f, bottomMargin);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            writer.PageEvent = leHelper;
            //doc.Open();

            for (int i = 0; i < pageDatas.Count; i++)
            {
                //whether print code128 part
                if (wbType.TRACK_NUM_BY_IHUB == CHubConstValues.IndY)
                {
                    leHelper.printCode = true;

                    //picture
                    string sourceString = pageDatas[i].Header.SHIP_ID + "C";

                    QRCodeEncoder qr = new QRCodeEncoder();
                    qr.QRCodeScale = 3;
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    qr.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qr.QRCodeVersion = 1;

                    Bitmap img = qr.Encode(sourceString, Encoding.ASCII);
                    string imgName = Guid.NewGuid().ToString() + ".gif";
                    string fullImgPath = this.BasePath + imgName;
                    img.Save(fullImgPath, System.Drawing.Imaging.ImageFormat.Gif);

                    leHelper.QRPath = fullImgPath;

                    //leHelper.pCode = new Paragraph(string.Format("编号：{0}", hData.SHIP_ID + "C"), new iTextSharp.text.Font(BF_Light, 10));
                    leHelper.codeString = string.Format("编号：{0}", pageDatas[i].Header.SHIP_ID + "C");
                    //line 1
                    sData.Clear();
                    sData.Add(pageDatas[i].Header.HEADER1);
                    sData.Add(pageDatas[i].Header.HEADER2);
                    sData.Add(pageDatas[i].Header.CARCOD);
                    sData.Add(pageDatas[i].Header.CARNAM);
                    sData.Add(pageDatas[i].Header.HEADER3);
                    //leHelper.pLine1 =  new Paragraph(GetLineString(sData, 80), new iTextSharp.text.Font(BF_Light, HeaderFontSize));
                    leHelper.line1String = GetLineString(sData, 80);
                }

                leHelper.hData = pageDatas[i].Header;
                leHelper.CurrentGroup = pageDatas[i].Header.SHIP_ID;

                if (i != 0)
                    doc.NewPage();
                else
                    doc.Open();

                //Table part
                if (wbType.PRINT_DETAIL == CHubConstValues.IndY)
                {
                    PdfPTable dTable = new PdfPTable(5);
                    dTable.WidthPercentage = 90f;
                    dTable.AddCell(BuildCell(pageDatas[i].Header.DETAIL_TITLE1, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(pageDatas[i].Header.DETAIL_TITLE2, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(pageDatas[i].Header.DETAIL_TITLE3, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(pageDatas[i].Header.DETAIL_TITLE4, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(pageDatas[i].Header.DETAIL_TITLE5, new iTextSharp.text.Font(BF_Light, ContentFontSize)));

                    foreach (var item in pageDatas[i].Details)
                    {
                        dTable.AddCell(BuildCell(item.SHIP_ID, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                        dTable.AddCell(BuildCell(item.LODNUM, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                        dTable.AddCell(BuildCell((item.VC_PALWGT ?? 0).ToString("f2"), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                        dTable.AddCell(BuildCell(item.PALVOL, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                        dTable.AddCell(BuildCell(item.REMARK1, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    }
                    doc.Add(dTable);

                    Paragraph pLast = new Paragraph(string.Format("Total Items:{0}          ", pageDatas[i].Details.Count), new iTextSharp.text.Font(BF_Light, 10));
                    pLast.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(pLast);
                }

                sData.Clear();
                sData.Add(pageDatas[i].Header.FOOTER1 ?? string.Empty);
                sData.Add(pageDatas[i].Header.FOOTER2 ?? string.Empty);
                sData.Add(pageDatas[i].Header.FOOTER3 ?? string.Empty);

                ////
                PdfContentByte cb = writer.DirectContent;
                ColumnText ct = new ColumnText(cb);
                cb.BeginText();
                cb.SetFontAndSize(BF_Light, FooterFontSize);
                cb.SetTextMatrix(doc.LeftMargin, doc.BottomMargin);
                cb.ShowText(GetLineString(sData));
                cb.EndText();

                

                //add track data

                string sourceString1 = pageDatas[i].Header.SHIP_ID + "C";
                RP_SHIP_TRACK_BLL trackBLL = new RP_SHIP_TRACK_BLL();
                foreach (var item in pageDatas[i].Details)
                {
                    RP_SHIP_TRACK track = new RP_SHIP_TRACK();
                    track.WH_ID = item.WH_ID;
                    track.SHIP_ID = item.SHIP_ID;
                    track.TRACK_NUM_IHUB = sourceString1;
                    track.RECORD_DATE = DateTime.Now;
                    track.TRACK_NUM_BY_IHUB = pageDatas[i].Header.TRACK_NUM_BY_IHUB;
                    track.UPDATED_BY = appUser;
                    trackBLL.AddOrUpdate(track);
                }


            }
            doc.Close();


            return fileName;
        }



        private PdfPTable GetCode128(string shipNo)
        {
            PdfPTable table = new PdfPTable(2);
            
            //table.WidthPercentage = 200f;
            PdfPCell cell;

            //编号No.
            Paragraph p = new Paragraph("编号No.",new iTextSharp.text.Font(BF_Light,10));
            cell = new PdfPCell(p);
            //cell.Width = 150f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);

            //picture
            Code128Helper cHelper = new Code128Helper();
            cHelper.ValueFont = new  System.Drawing.Font("宋体", 20);
            string sourceString = shipNo + "C";
            Bitmap img = cHelper.GetCodeImage(sourceString, Code128Helper.Encode.Code128B);
            string imgName = Guid.NewGuid().ToString() + ".gif";
            string fullImgPath = this.BasePath + imgName;
            img.Save(fullImgPath, System.Drawing.Imaging.ImageFormat.Gif);

            Code128PicPath = fullImgPath;
            iTextSharp.text.Image tImg = iTextSharp.text.Image.GetInstance(fullImgPath);
            cell = new PdfPCell(tImg, false);
            //cell.Width = 50f;
            table.AddCell(cell);

            return table;
        }


        /// <summary>
        /// base on one line spaces, to allocation position for data source
        /// </summary>
        /// <param name="data">a string list</param>
        /// <param name="totalSpace"> represent how many spaces in one line </param>
        /// <returns></returns>
        public string GetLineString(List<string> data,int totalSpace = 130)
        {
            //int RowNum = 90;
            int dataLength = 0;
            int space=0;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == null)
                    data[i] = string.Empty;
                dataLength += data[i].Length;
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

        public PdfPCell BuildCell(string text, iTextSharp.text.Font font,  BaseColor backColor=null)
        {
            PdfPCell cell = new PdfPCell(new Paragraph(text, font));
            if (backColor != null)
                cell.BackgroundColor = backColor;
            //cell.BorderWidth = 0;
            return cell;
        }

    }
}

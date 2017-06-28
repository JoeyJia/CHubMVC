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

namespace CHubBLL.OtherProcess
{
    public class RPWayBillPrintBLL
    {
        public string BasePath = string.Empty;
        // ariblk.ttf    simsun.ttc
        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        public string Code128PicPath = string.Empty;
        private int ContentFontSize = 10;
        private int HeaderFontSize = 12;
        private int FooterFontSize = 8;
        public RPWayBillPrintBLL(string basePath)
        {
            this.BasePath = basePath;
        }

        public string BuildPrintFile(V_RP_WAYBILL_H_PRINT hData, List<V_RP_WAYBILL_D_PRINT> dPrintList,string appUser)
        {
            string fileName = string.Format("{0}-{1}-{2}-{3}.pdf", 
                hData.L_ADRNAM, hData.ORDTYP_WB, hData.CARCOD, DateTime.Now.ToString("yyyyMMddHHmm"));
            string fullPath = BasePath + fileName;

            //base waybilltype to get PageSize?, and whether Including details parts
            RP_WAYBILL_TYPE_BLL wbTypeBLL = new RP_WAYBILL_TYPE_BLL();
            RP_WAYBILL_TYPE wbType = wbTypeBLL.GetSpecifyItem(hData.WAYBILL_ID);

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.Open();
            //doc.Add(GetCode128(hData.SHIP_ID));

            //whether print code128 part
            if (wbType.TRACK_NUM_BY_IHUB == CHubConstValues.IndY)
            {
               
                //picture
                //Code128Helper cHelper = new Code128Helper();
                //cHelper.ValueFont = new System.Drawing.Font("宋体", 20);
                string sourceString = hData.SHIP_ID + "C";

                //Bitmap img = cHelper.GetCodeImage(sourceString, Code128Helper.Encode.Code128B);
                QRCodeEncoder qr = new QRCodeEncoder();
                qr.QRCodeScale = 1;

                Bitmap img = qr.Encode(sourceString, Encoding.ASCII);
                string imgName = Guid.NewGuid().ToString() + ".gif";
                string fullImgPath = this.BasePath + imgName;
                img.Save(fullImgPath, System.Drawing.Imaging.ImageFormat.Gif);

                iTextSharp.text.Image tImg = iTextSharp.text.Image.GetInstance(fullImgPath);
                tImg.Alignment = Element.ALIGN_RIGHT;
                doc.Add(tImg);

                Paragraph p1 = new Paragraph(string.Format("编号：{0}", hData.SHIP_ID + "C"), new iTextSharp.text.Font(BF_Light, 10));
                p1.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p1);
            }

            List<string> sData = new List<string>();
            doc.Add(new Paragraph(Environment.NewLine));
            //line 1
            sData.Clear();
            sData.Add(hData.HEADER1);
            sData.Add(hData.HEADER2);
            sData.Add(hData.CARCOD);
            sData.Add(hData.CARNAM);
            sData.Add(hData.HEADER3);
            doc.Add(new Paragraph(GetLineString(sData,80), new iTextSharp.text.Font(BF_Light, HeaderFontSize)));

            doc.Add(new Paragraph(Environment.NewLine));

            //content table
            PdfPTable contentTable = new PdfPTable(3);
            contentTable.WidthPercentage = 100f;
            contentTable.SetWidths(new float[] { 260f, 75f,260f });
            PdfPCell cellUnit;
            Paragraph prTemp;
            Phrase phTemp;

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

            //wechat imag
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
            phTemp = new Phrase(string.Format("{0}    {1}    {2}", hData.R_ADRLN2, hData.R_ADRLN3, hData.SIGNATURE3), new iTextSharp.text.Font(BF_Light, ContentFontSize));

            p13.Add(phTemp);
            p13.Add(System.Environment.NewLine);
            p13.Add(new Phrase(string.Format("{0}    {1}", hData.R_LAST_NAME, hData.R_PHNNUM), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p13.Add(System.Environment.NewLine);

            p13.Alignment = Element.ALIGN_RIGHT;
            cellUnit = new PdfPCell(p13);
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

            //2-3 cell
            Paragraph p23 = new Paragraph();
            p23.Add(System.Environment.NewLine);
            p23.Add(System.Environment.NewLine);
            p23.Add(new Phrase(string.Format("{0}", hData.SIGNATURE3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            p23.Alignment = Element.ALIGN_RIGHT;

            cellUnit = new PdfPCell(p23);
            cellUnit.BorderWidth = 0;
            contentTable.AddCell(cellUnit);

            doc.Add(contentTable);

            doc.Add(new Paragraph(Environment.NewLine));

            //Table part
            if (wbType.PRINT_DETAIL == CHubConstValues.IndY)
            {
                PdfPTable dTable = new PdfPTable(5);
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE1, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE2, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE3, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE4, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                dTable.AddCell(BuildCell(hData.DETAIL_TITLE5, new iTextSharp.text.Font(BF_Light, ContentFontSize)));

                foreach (var item in dPrintList)
                {
                    dTable.AddCell(BuildCell(item.SHIP_ID, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.LODNUM, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.VC_PALWGT.ToString("f2"), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.PALVOL, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                    dTable.AddCell(BuildCell(item.REMARK1, new iTextSharp.text.Font(BF_Light, ContentFontSize)));
                }
                doc.Add(dTable);
            }


            //footer  part
            //
            sData.Clear();
            sData.Add(hData.FOOTER1);
            sData.Add(hData.FOOTER2);
            sData.Add(hData.FOOTER3);
            ////doc.Add(new Paragraph(string.Format("{0}    {1}    {2}", hData.FOOTER1,hData.FOOTER2,hData.FOOTER3), new iTextSharp.text.Font(BF_Light, ContentFontSize)));
            //Paragraph footer = new Paragraph(GetLineString(sData), new iTextSharp.text.Font(BF_Light, ContentFontSize));
            //footer.Alignment = Element.ALIGN_BOTTOM;
            //doc.Add(footer);

            ////
            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            cb.BeginText();
            cb.SetFontAndSize(BF_Light, FooterFontSize);
            cb.SetTextMatrix(doc.LeftMargin, doc.BottomMargin+100f);
            cb.ShowText(GetLineString(sData));
            cb.EndText();

            doc.Close();

            //add track data
            if (wbType.TRACK_NUM_BY_IHUB == CHubConstValues.IndY)
            {
                string sourceString = hData.SHIP_ID + "C";
                RP_SHIP_TRACK_BLL trackBLL = new RP_SHIP_TRACK_BLL();
                foreach (var item in dPrintList)
                {
                    RP_SHIP_TRACK track = new RP_SHIP_TRACK();
                    track.WH_ID = item.WH_ID;
                    track.SHIP_ID = item.SHIP_ID;
                    track.TRACK_NUM_IHUB = sourceString;
                    track.RECORD_DATE = DateTime.Now;
                    track.UPDATED_BY = appUser;
                    trackBLL.AddOrUpdate(track);
                }
            }

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
            foreach (var item in data)
            {
                dataLength += item.Length;
            }
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

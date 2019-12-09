using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CHubDBEntity.UnmanagedModel;
using System.IO;
using CHubCommon;


namespace CHubBLL.OtherProcess
{
    public class IHUBOAPrintBLL
    {
        //字体路径
        private static readonly string fontpath = System.Configuration.ConfigurationManager.AppSettings["FontPath"].ToString();
        //图片路径
        private static readonly string imagepath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"].ToString();

        public IHUBOAPrintBLL(string basePath)
        {
            this.BasePath = basePath;
            contentFont = new Font(BF_Light, ContentFontSize);
            titleFont = new Font(BF_Light, TitleFontSize,Font.BOLD);
            headTitleFont = new Font(BF_Light, HeadTitleFontSize, Font.BOLD);
            headContentFont = new Font(BF_Light, HeadContentFontSize);
            detailTitleFont = new Font(BF_Light, DetailTitleFontSize, Font.BOLD);
            detailContentFont = new Font(BF_Light, DetailContentFontSize);
            footerFont = new Font(BF_Light, FooterFontSize, Font.BOLD);
        }


        public string BasePath = string.Empty;
        //字体
        BaseFont BF_Light = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        Font contentFont;//内容
        Font titleFont;//大title
        Font headTitleFont;//header标题
        Font headContentFont;//header内容
        Font detailTitleFont;//detail标题
        Font detailContentFont;//detail内容
        Font footerFont;//尾部


        //pdf文档大小
        public Rectangle pageSize = PageSize.A4;

        //内容字体大小
        private float ContentFontSize = 10f;
        //标题字体大小
        private float TitleFontSize = 18f;//总标题
        private float HeadTitleFontSize = 10f;//header标题字体大小
        private float HeadContentFontSize = 8f;//header内容字体大小
        private float DetailTitleFontSize = 8f;//detail标题字体大小
        private float DetailContentFontSize = 6f;//detail内容字体大小
        private float FooterFontSize = 12f;//footer字体大小

        private string cumminsImage = "OAPrint.jpg";
        private string wechatImage = "wechat.jpg";


        public string BuildIhubOAPrintFile(OA_TYPE_MST title, V_OA_H_PRINT header, List<V_OA_D_PRINT> details)
        {
            string fileName = string.Format(@"{0}{1}_{2}_{3}.pdf", "OA",header.CUSTOMER_NO, header.ORDER_NO, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fullPath = BasePath + fileName;

            Document doc = new Document(pageSize);
            PdfWriter pWriter = PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
            doc.SetMargins(10f, 10f, 30f, 100f);//文档边框
            IHUBOAPageEventHelper helper = new IHUBOAPageEventHelper();
            pWriter.PageEvent = helper;
            helper.header = header;
            helper.title = title;
            doc.Open();
            

            #region Title
            PdfPTable titleTable = new PdfPTable(1);
            titleTable.WidthPercentage = 100f;
            PdfPCell titleCell;

            titleCell = new PdfPCell(new Paragraph(title.TITLE, titleFont));
            titleCell.BorderWidth = 0f;//边框
            titleCell.FixedHeight = 45f;//高度
            titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
            titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            titleTable.AddCell(titleCell);
            doc.Add(titleTable);
            #endregion

            #region Head
            PdfPTable headTable = new PdfPTable(6);
            headTable.WidthPercentage = 100f;
            PdfPCell headCell;
            
            headCell = new PdfPCell(new Paragraph(title.H14, headTitleFont));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);

            //Image payCode = Image.GetInstance(imagepath + paycodeImage);
            //payCode.ScaleAbsoluteHeight(60f);
            //payCode.ScaleAbsoluteWidth(140f);
            //headCell = new PdfPCell(payCode);
            //headCell.BorderWidth = 0f;
            //headCell.Colspan = 2;
            //headCell.Rowspan = 6;
            //headCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H14) ? header.H14 : "", headContentFont));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headCell.FixedHeight = 12f;
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H15) ? header.H15 : "", headContentFont));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headCell.FixedHeight = 12f;
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H16) ? header.H16 : "", headContentFont));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headCell.FixedHeight = 12f;
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H17) ? header.H17 : "", headContentFont));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headCell.FixedHeight = 12f;
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H18) ? header.H18 : "", headContentFont));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headCell.FixedHeight = 12f;
            headTable.AddCell(headCell);

            //空行
            headCell = new PdfPCell(new Paragraph(""));
            headCell.Colspan = 6;
            headCell.FixedHeight = 10f;
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(title.H02, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H02) ? header.H02 : "", headContentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H04, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H04) ? header.H04 : "", headContentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H01, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H01) ? header.H01 : "", headContentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(title.H05, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H05) ? header.H05 : "", headContentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H06, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H06) ? header.H06 : "", headContentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H07, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H07) ? header.H07 : "", headContentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(title.H08, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H08) ? header.H08 : "", contentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H03, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H03) ? header.H03 : "", contentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H09, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H09) ? header.H09 : "", contentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);

            headCell = new PdfPCell(new Paragraph(title.H10, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H10) ? header.H10 : "", contentFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(title.H11, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H11) ? header.H11 : "", contentFont));
            headCell.BorderWidth = 0f;//边框
            headCell.Colspan = 3;
            headTable.AddCell(headCell);
            
            headCell = new PdfPCell(new Paragraph(title.H12, headTitleFont));
            headCell.BorderWidth = 0f;//边框
            headTable.AddCell(headCell);
            headCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(header.H12) ? header.H12 : "", contentFont));
            headCell.BorderWidth = 0f;//边框
            headCell.Colspan = 5;
            headTable.AddCell(headCell);
            //空行
            headCell = new PdfPCell(new Paragraph(""));
            headCell.Colspan = 6;
            headCell.BorderWidth = 0f;//边框
            headCell.FixedHeight = 10f;
            headTable.AddCell(headCell);

            doc.Add(headTable);
            #endregion

            #region Detail
            PdfPTable detailTable = new PdfPTable(12);
            detailTable.WidthPercentage = 100f;
            detailTable.SetWidths(new float[] { 4f, 8f, 12f, 10f, 5f, 8f, 10f, 8f, 10f, 10f, 6f, 7f });
            PdfPCell detailCell;

            detailCell = new PdfPCell(new Paragraph(title.D01, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D02, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D05, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D03, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D08, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D06, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D01_ADDT, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D02_ADDT, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D03_ADDT, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D04_ADDT, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D07, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.D10, detailTitleFont));
            detailCell.BorderWidth = 0f;
            detailTable.AddCell(detailCell);
            
            decimal sumOfD01_ADDT = 0;
            decimal sumOfD02_ADDT = 0;
            decimal sumOfD03_ADDT = 0;
            decimal sumOfD04_ADDT = 0;
            decimal sumOfH20 = 0;
            
            foreach (var item in details)
            {
                sumOfD01_ADDT += item.D01_ADDT;
                sumOfD02_ADDT += item.D02_ADDT;
                sumOfD03_ADDT += item.D03_ADDT;
                sumOfD04_ADDT += item.D04_ADDT;

                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D01) ? item.D01 : "", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D02) ? item.D02 : "", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D05) ? item.D05 : "", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D03) ? item.D03 : "", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D08.ToString())? item.D08.ToString():"", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(item.D06.ToString(), detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(item.D01_ADDT.ToString(), detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(item.D02_ADDT.ToString(), detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(item.D03_ADDT.ToString(), detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(item.D04_ADDT.ToString(), detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D07) ? item.D07 : "", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Paragraph(!string.IsNullOrEmpty(item.D10) ? item.D10 : "", detailContentFont));
                detailCell.BorderWidth = 0f;//边框
                detailTable.AddCell(detailCell);
            }

            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.BorderWidth = 0f;
            detailCell.Colspan = 12;
            detailCell.FixedHeight = 10f;
            detailTable.AddCell(detailCell);

            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.Colspan = 5;
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph("合计：", new Font(BF_Light,12f,Font.BOLD)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(sumOfD01_ADDT.ToString(), new Font(BF_Light, ContentFontSize)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(sumOfD02_ADDT.ToString(), new Font(BF_Light, ContentFontSize)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(sumOfD03_ADDT.ToString(), new Font(BF_Light, ContentFontSize)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(sumOfD04_ADDT.ToString(), new Font(BF_Light, ContentFontSize)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.Colspan = 2;
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);

            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.Colspan = 8;
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.H13, new Font(BF_Light, 12f, Font.BOLD)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(header.H13.ToString(), new Font(BF_Light, ContentFontSize)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.Colspan = 2;
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);

            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.Colspan = 8;
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(title.H20, new Font(BF_Light, 12f, Font.BOLD)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            sumOfH20 = header.H13 + sumOfD04_ADDT;
            detailCell = new PdfPCell(new Paragraph(sumOfH20.ToString(), new Font(BF_Light, ContentFontSize)));
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);
            detailCell = new PdfPCell(new Paragraph(""));
            detailCell.Colspan = 2;
            detailCell.BorderWidth = 0f;//边框
            detailTable.AddCell(detailCell);

            doc.Add(detailTable);
            #endregion

            //PdfContentByte cb = pWriter.DirectContent;
            //Image payCode = Image.GetInstance(imagepath + paycodeImage);
            //payCode.ScaleAbsoluteHeight(60f);
            //payCode.ScaleAbsoluteWidth(140f);
            //payCode.SetAbsolutePosition(0, 0);
            //cb.AddImage(payCode);
            //cb.MoveTo(doc.GetRight(200f), doc.GetTop(10f));

            
            pWriter.Flush();
            doc.Close();

            return fileName;
        }
    }
}

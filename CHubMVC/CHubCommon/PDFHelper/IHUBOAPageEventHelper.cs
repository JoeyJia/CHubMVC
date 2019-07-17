using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CHubDBEntity.UnmanagedModel;

namespace CHubCommon
{
    public class IHUBOAPageEventHelper : PdfPageEventHelper
    {
        //字体路径
        private static readonly string fontpath = System.Configuration.ConfigurationManager.AppSettings["FontPath"].ToString();
        //图片路径
        private static readonly string imagepath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"].ToString();

        PdfContentByte cb;
        PdfTemplate template;
        public V_OA_H_PRINT header;
        public OA_TYPE_MST title;
        //字体
        BaseFont BF_Light = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //内容字体大小
        private float ContentFontSize = 10f;
        private float PageFontSize = 8f;
        private float FooterFontSize = 6f;

        public string GroupIdentity = string.Empty;
        public int CurrentPage = 0;
        public string CurrentGroup = string.Empty;

        private string cumminsImage = "OAPrint.jpg";
        private string wechatImage = "wechat.jpg";

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            //base.OnOpenDocument(writer, document);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            string date = string.Format(@"日期：" + DateTime.Now.ToString("yyyy/MM/dd"));

            string text = string.Format(@"页码：第{0}页/共  页", CurrentPage.ToString());

            float len = BF_Light.GetWidthPoint("页码：第x页/共", PageFontSize);

            var pageSize = document.PageSize;

            cb.BeginText();
            cb.SetFontAndSize(BF_Light, PageFontSize);
            cb.SetTextMatrix(document.PageSize.Width - 100f, pageSize.GetTop(80f));
            cb.ShowText(date);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(BF_Light, PageFontSize);
            cb.SetTextMatrix(document.PageSize.Width - 100f, pageSize.GetTop(90f));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, document.PageSize.Width - 100f + len, pageSize.GetTop(90f));

        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            int pagenum = writer.PageNumber;

            template.BeginText();
            template.SetFontAndSize(BF_Light, PageFontSize);
            template.SetTextMatrix(0, 0);
            template.ShowText((CurrentPage.ToString()));
            template.EndText();
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            AddPageNo();


            PdfPTable titleTable = new PdfPTable(3);
            titleTable.WidthPercentage = 100f;
            titleTable.SetWidths(new float[] { 10f, 60f, 30f });
            PdfPCell titleCell;

            Image leftImage = Image.GetInstance(imagepath + cumminsImage);
            leftImage.ScaleAbsoluteHeight(60f);//高
            leftImage.ScaleAbsoluteWidth(60f);//宽
            titleCell = new PdfPCell(leftImage);
            titleCell.BorderWidth = 0f;//边框
            titleCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            titleTable.AddCell(titleCell);

            Paragraph titlePara = new Paragraph();
            titlePara.Add(new Phrase(@"康明斯发动机（上海）贸易服务有限公司", new Font(BF_Light, ContentFontSize)));
            titlePara.Add(Environment.NewLine);
            titlePara.Add(new Phrase(@"康明斯中国零部件分拨中心(上海外高桥保税区英伦路999号)", new Font(BF_Light, ContentFontSize)));
            titlePara.Add(Environment.NewLine);
            titlePara.Add(new Phrase(@"Tel: 86(21)61693000", new Font(BF_Light, ContentFontSize)));
            titleCell = new PdfPCell(titlePara);
            titleCell.BorderWidth = 0f;//边框
            titleCell.HorizontalAlignment = Element.ALIGN_LEFT;
            titleCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            titleTable.AddCell(titleCell);

            Image rightImage = Image.GetInstance(imagepath + wechatImage);
            rightImage.ScaleAbsoluteHeight(60f);//高
            rightImage.ScaleAbsoluteWidth(60f);//宽
            titleCell = new PdfPCell(rightImage);
            titleCell.BorderWidth = 0f;//边框
            titleCell.HorizontalAlignment = Element.ALIGN_LEFT;
            titleTable.AddCell(titleCell);

            document.Add(titleTable);

            if (CurrentPage > 1)
            {
                titleCell = new PdfPCell(new Paragraph(""));
                titleCell.BorderWidth = 0f;
                titleCell.Colspan = 3;
                titleCell.FixedHeight = 10f;
                titleTable.AddCell(titleCell);
            }

            //图片
            if (CurrentPage == 1)
            {
                if (!string.IsNullOrEmpty(header.IMAGE_FILE))
                {
                    Image payCode = Image.GetInstance(imagepath + header.IMAGE_FILE);
                    //payCode.DirectReference = new PRIndirectReference(new PdfReader(new Uri(header.IMAGE_URL)), 1);
                    payCode.ScaleAbsoluteHeight(80f);
                    payCode.ScaleAbsoluteWidth(240f);
                    payCode.SetAbsolutePosition(document.GetRight(300f), document.GetTop(180f));

                    cb.AddImage(payCode);
                    //document.Add(new Paragraph(new Chunk(header.IMAGE_URL, new Font(BF_Light, 2)).SetAnchor(header.IMAGE_URL)));
                }
            }

            #region footer
            List<string> footData = new List<string>();
            footData.Add(!string.IsNullOrEmpty(header.F01) ? header.F01 : "");
            footData.Add(!string.IsNullOrEmpty(header.F02) ? header.F02 : "");
            footData.Add(!string.IsNullOrEmpty(header.F03) ? header.F03 : "");
            footData.Add(!string.IsNullOrEmpty(header.F04) ? header.F04 : "");
            footData.Add(!string.IsNullOrEmpty(header.F05) ? header.F05 : "");
            footData.Add(!string.IsNullOrEmpty(header.F06) ? header.F06 : "");
            footData.Add(!string.IsNullOrEmpty(header.F07) ? header.F07 : "");
            footData.Add(!string.IsNullOrEmpty(header.F08) ? header.F08 : "");
            footData.Add(!string.IsNullOrEmpty(header.F09) ? header.F09 : "");
            footData.Add(!string.IsNullOrEmpty(header.F10) ? header.F10 : "");
            footData.Add(!string.IsNullOrEmpty(header.F11) ? header.F11 : "");
            footData.Add(!string.IsNullOrEmpty(header.F12) ? header.F12 : "");
            float totalMargin = (float)footData.Count * FooterFontSize;

            foreach (var item in footData)
            {
                cb.BeginText();
                cb.SetFontAndSize(BF_Light, FooterFontSize);
                cb.MoveText(document.LeftMargin, totalMargin + 20f);
                cb.NewlineShowText(item);
                totalMargin = totalMargin - FooterFontSize;
                cb.EndText();
            }
            #endregion

        }

        private void ResetPage()
        {
            CurrentPage = 0;
        }

        private void AddPageNo()
        {
            CurrentPage++;
        }


    }
}

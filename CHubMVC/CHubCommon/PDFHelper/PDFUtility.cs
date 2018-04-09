using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

namespace CHubCommon.PDFHelper
{
    public class PDFUtility
    {

        /// <summary>
        /// base on one line spaces, to allocation position for data source
        /// font size=8, total line =130, font size=10 ,total line =~ 86
        /// </summary>
        /// <param name="data">a string list</param>
        /// <param name="totalSpace"> represent how many spaces in one line </param>
        /// <returns></returns>
        public string GetLineString(List<string> data, int totalSpace = 130)
        {
            //int RowNum = 90;
            int dataLength = 0;
            int space = 0;
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
                result = result.Substring(0, point) + data[i] + result.Substring(point + data[i].Length);
                //if not the last one
                if (i != data.Count - 1)
                    point = point + data[i].Length + space;
            }

            return result;
        }

        public PdfPCell BuildCell(string text, iTextSharp.text.Font font, BaseColor backColor = null)
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


        public Image GetCode128Img(string source, int? height)
        {
            //Code128Helper cHelper = new Code128Helper();
            //if (height != null)
            //    cHelper.Height = (uint)height.Value;
            //cHelper.ValueFont = new System.Drawing.Font("宋体", 10);

            //System.Drawing.Bitmap img = cHelper.GetCodeImage(source, Code128Helper.Encode.Code128B);

            ////img = new System.Drawing.Bitmap(img, new System.Drawing.Size(100,9));

            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.Code = source;
            if (height != null)
                code128.BarHeight = height.Value;


            System.Drawing.Bitmap img = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            //bm.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);


            return Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Gif);
        }

        public Image GetCode39Img(string source, int? height)
        {
            Code39Helper code39 = new Code39Helper(!string.IsNullOrEmpty(source) ? source : "", false, 200, 400, false);
            string codeValue = string.Empty;
            System.Drawing.Bitmap img = code39.GenerateCodeImage(ref codeValue);

            return Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Gif);
        }


        /// <summary>
        /// GET2D
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Image QRCodeEncoderUtil(string source)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 0;

            System.Drawing.Bitmap img = qrCodeEncoder.Encode(source, Encoding.UTF8);//指定utf-8编码， 支持中文  
            return Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Gif);
        }

    }
}

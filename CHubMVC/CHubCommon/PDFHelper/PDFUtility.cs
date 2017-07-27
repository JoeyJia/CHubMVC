using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CHubCommon
{
    public class Code39Helper
    {
        private string _rawData;
        private bool _enableChecksum;
        private int _height;
        private int _width;
        private bool _includeLabel;
        private string _code;
        private enum LabelPositions : int { TOPLEFT, TOPCENTER, TOPRIGHT, BOTTOMLEFT, BOTTOMCENTER, BOTTOMRIGHT };

        private LabelPositions _labelPosition;
        private Color _ForeColor = Color.Black;
        private Color _BackColor = Color.White;
        private string Code
        {
            get
            {
                if (string.IsNullOrEmpty(this._code))
                {
                    this._code = Encode();
                }
                return this._code;
            }
        }
        private Image _encodedImage;
        public Image EncodedImage
        {
            get { return _encodedImage; }
            set { this._encodedImage = value; }
        }
        private Hashtable C39_Code = new Hashtable(); //is initialized by init_Code39()
        private Hashtable ExtC39_Translation = new Hashtable();
        /// <summary>
        /// Encodes with Code39.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        /// <param name="AllowExtended">Allow Extended Code 39 (Full Ascii mode).</param>
        /// <param name="EnableChecksum">Whether to calculate the Mod 43 checksum and encode it into the barcode</param>
        public Code39Helper(string input, bool enableChecksum, int height, int width, bool includeLabel)
        {
            _rawData = input;
            _enableChecksum = enableChecksum;
            _height = height;
            _width = width;
            _includeLabel = includeLabel;
            _labelPosition = LabelPositions.BOTTOMCENTER;
        }

        /// <summary>
        /// Encode the raw data using the Code 39 algorithm.
        /// </summary>
        private string Encode()
        {
            this.init_Code39();
            this.init_ExtendedCode39();

            string strNoAstr = this._rawData.Replace("*", "");
            string strFormattedData = "*" + strNoAstr + (this._enableChecksum ? getChecksumChar(strNoAstr).ToString() : String.Empty) + "*";

            string result = "";
            //foreach (char c in this.FormattedData)
            foreach (char c in strFormattedData)
            {
                try
                {
                    result += C39_Code[c].ToString();
                    result += "0";//whitespace
                }//try
                catch
                {
                    throw new Exception("EC39-1: Invalid data. (Try using Extended Code39)");
                }//catch
            }//foreach

            result = result.Substring(0, result.Length - 1);

            //clear the hashtable so it no longer takes up memory
            this.C39_Code.Clear();

            return result;
        }//Encode_Code39
        private void init_Code39()
        {
            C39_Code.Clear();
            C39_Code.Add('0', "101001101101");
            C39_Code.Add('1', "110100101011");
            C39_Code.Add('2', "101100101011");
            C39_Code.Add('3', "110110010101");
            C39_Code.Add('4', "101001101011");
            C39_Code.Add('5', "110100110101");
            C39_Code.Add('6', "101100110101");
            C39_Code.Add('7', "101001011011");
            C39_Code.Add('8', "110100101101");
            C39_Code.Add('9', "101100101101");
            C39_Code.Add('A', "110101001011");
            C39_Code.Add('B', "101101001011");
            C39_Code.Add('C', "110110100101");
            C39_Code.Add('D', "101011001011");
            C39_Code.Add('E', "110101100101");
            C39_Code.Add('F', "101101100101");
            C39_Code.Add('G', "101010011011");
            C39_Code.Add('H', "110101001101");
            C39_Code.Add('I', "101101001101");
            C39_Code.Add('J', "101011001101");
            C39_Code.Add('K', "110101010011");
            C39_Code.Add('L', "101101010011");
            C39_Code.Add('M', "110110101001");
            C39_Code.Add('N', "101011010011");
            C39_Code.Add('O', "110101101001");
            C39_Code.Add('P', "101101101001");
            C39_Code.Add('Q', "101010110011");
            C39_Code.Add('R', "110101011001");
            C39_Code.Add('S', "101101011001");
            C39_Code.Add('T', "101011011001");
            C39_Code.Add('U', "110010101011");
            C39_Code.Add('V', "100110101011");
            C39_Code.Add('W', "110011010101");
            C39_Code.Add('X', "100101101011");
            C39_Code.Add('Y', "110010110101");
            C39_Code.Add('Z', "100110110101");
            C39_Code.Add('-', "100101011011");
            C39_Code.Add('.', "110010101101");
            C39_Code.Add(' ', "100110101101");
            C39_Code.Add('$', "100100100101");
            C39_Code.Add('/', "100100101001");
            C39_Code.Add('+', "100101001001");
            C39_Code.Add('%', "101001001001");
            C39_Code.Add('*', "100101101101");
        }//init_Code39
        private void init_ExtendedCode39()
        {
            ExtC39_Translation.Clear();
            ExtC39_Translation.Add(Convert.ToChar(0).ToString(), "%U");
            ExtC39_Translation.Add(Convert.ToChar(1).ToString(), "$A");
            ExtC39_Translation.Add(Convert.ToChar(2).ToString(), "$B");
            ExtC39_Translation.Add(Convert.ToChar(3).ToString(), "$C");
            ExtC39_Translation.Add(Convert.ToChar(4).ToString(), "$D");
            ExtC39_Translation.Add(Convert.ToChar(5).ToString(), "$E");
            ExtC39_Translation.Add(Convert.ToChar(6).ToString(), "$F");
            ExtC39_Translation.Add(Convert.ToChar(7).ToString(), "$G");
            ExtC39_Translation.Add(Convert.ToChar(8).ToString(), "$H");
            ExtC39_Translation.Add(Convert.ToChar(9).ToString(), "$I");
            ExtC39_Translation.Add(Convert.ToChar(10).ToString(), "$J");
            ExtC39_Translation.Add(Convert.ToChar(11).ToString(), "$K");
            ExtC39_Translation.Add(Convert.ToChar(12).ToString(), "$L");
            ExtC39_Translation.Add(Convert.ToChar(13).ToString(), "$M");
            ExtC39_Translation.Add(Convert.ToChar(14).ToString(), "$N");
            ExtC39_Translation.Add(Convert.ToChar(15).ToString(), "$O");
            ExtC39_Translation.Add(Convert.ToChar(16).ToString(), "$P");
            ExtC39_Translation.Add(Convert.ToChar(17).ToString(), "$Q");
            ExtC39_Translation.Add(Convert.ToChar(18).ToString(), "$R");
            ExtC39_Translation.Add(Convert.ToChar(19).ToString(), "$S");
            ExtC39_Translation.Add(Convert.ToChar(20).ToString(), "$T");
            ExtC39_Translation.Add(Convert.ToChar(21).ToString(), "$U");
            ExtC39_Translation.Add(Convert.ToChar(22).ToString(), "$V");
            ExtC39_Translation.Add(Convert.ToChar(23).ToString(), "$W");
            ExtC39_Translation.Add(Convert.ToChar(24).ToString(), "$X");
            ExtC39_Translation.Add(Convert.ToChar(25).ToString(), "$Y");
            ExtC39_Translation.Add(Convert.ToChar(26).ToString(), "$Z");
            ExtC39_Translation.Add(Convert.ToChar(27).ToString(), "%A");
            ExtC39_Translation.Add(Convert.ToChar(28).ToString(), "%B");
            ExtC39_Translation.Add(Convert.ToChar(29).ToString(), "%C");
            ExtC39_Translation.Add(Convert.ToChar(30).ToString(), "%D");
            ExtC39_Translation.Add(Convert.ToChar(31).ToString(), "%E");
            ExtC39_Translation.Add("!", "/A");
            ExtC39_Translation.Add("\"", "/B");
            ExtC39_Translation.Add("#", "/C");
            ExtC39_Translation.Add("$", "/D");
            ExtC39_Translation.Add("%", "/E");
            ExtC39_Translation.Add("&", "/F");
            ExtC39_Translation.Add("'", "/G");
            ExtC39_Translation.Add("(", "/H");
            ExtC39_Translation.Add(")", "/I");
            ExtC39_Translation.Add("*", "/J");
            ExtC39_Translation.Add("+", "/K");
            ExtC39_Translation.Add(",", "/L");
            ExtC39_Translation.Add("/", "/O");
            ExtC39_Translation.Add(":", "/Z");
            ExtC39_Translation.Add(";", "%F");
            ExtC39_Translation.Add("<", "%G");
            ExtC39_Translation.Add("=", "%H");
            ExtC39_Translation.Add(">", "%I");
            ExtC39_Translation.Add("?", "%J");
            ExtC39_Translation.Add("[", "%K");
            ExtC39_Translation.Add("\\", "%L");
            ExtC39_Translation.Add("]", "%M");
            ExtC39_Translation.Add("^", "%N");
            ExtC39_Translation.Add("_", "%O");
            ExtC39_Translation.Add("{", "%P");
            ExtC39_Translation.Add("|", "%Q");
            ExtC39_Translation.Add("}", "%R");
            ExtC39_Translation.Add("~", "%S");
            ExtC39_Translation.Add("`", "%W");
            ExtC39_Translation.Add("@", "%V");
            ExtC39_Translation.Add("a", "+A");
            ExtC39_Translation.Add("b", "+B");
            ExtC39_Translation.Add("c", "+C");
            ExtC39_Translation.Add("d", "+D");
            ExtC39_Translation.Add("e", "+E");
            ExtC39_Translation.Add("f", "+F");
            ExtC39_Translation.Add("g", "+G");
            ExtC39_Translation.Add("h", "+H");
            ExtC39_Translation.Add("i", "+I");
            ExtC39_Translation.Add("j", "+J");
            ExtC39_Translation.Add("k", "+K");
            ExtC39_Translation.Add("l", "+L");
            ExtC39_Translation.Add("m", "+M");
            ExtC39_Translation.Add("n", "+N");
            ExtC39_Translation.Add("o", "+O");
            ExtC39_Translation.Add("p", "+P");
            ExtC39_Translation.Add("q", "+Q");
            ExtC39_Translation.Add("r", "+R");
            ExtC39_Translation.Add("s", "+S");
            ExtC39_Translation.Add("t", "+T");
            ExtC39_Translation.Add("u", "+U");
            ExtC39_Translation.Add("v", "+V");
            ExtC39_Translation.Add("w", "+W");
            ExtC39_Translation.Add("x", "+X");
            ExtC39_Translation.Add("y", "+Y");
            ExtC39_Translation.Add("z", "+Z");
            ExtC39_Translation.Add(Convert.ToChar(127).ToString(), "%T"); //also %X, %Y, %Z 
        }
        private void InsertExtendedCharsIfNeeded(ref string FormattedData)
        {
            string output = "";
            foreach (char c in FormattedData)
            {
                try
                {
                    string s = C39_Code[c].ToString();
                    output += c;
                }//try
                catch
                {
                    //insert extended substitution
                    object oTrans = ExtC39_Translation[c.ToString()];
                    output += oTrans.ToString();
                }//catch
            }//foreach

            FormattedData = output;
        }
        private char getChecksumChar(string strNoAstr)
        {
            //checksum
            string Code39_Charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
            InsertExtendedCharsIfNeeded(ref strNoAstr);
            int sum = 0;

            //Calculate the checksum
            for (int i = 0; i < strNoAstr.Length; ++i)
            {
                sum = sum + Code39_Charset.IndexOf(strNoAstr[i].ToString());
            }

            //return the checksum char
            return Code39_Charset[sum % 43];
        }

        /// <summary>
        /// Gets a bitmap representation of the encoded data.
        /// </summary>
        /// <returns>Bitmap of encoded value.</returns>
        public Bitmap GenerateCodeImage(ref string codeValue)
        {
            if (Code == "") throw new Exception("EGENERATE_IMAGE-1: Encode Error.");
            Bitmap b = null;

            DateTime dtStartTime = DateTime.Now;


            b = new Bitmap(this._width, this._height);
            int iBarWidth = this._width / Code.Length;
            int shiftAdjustment = (this._width % Code.Length) / 2;
            int iBarWidthModifier = 1;

            if (iBarWidth <= 0)
            {
                throw new Exception("EGENERATE_IMAGE-2: Image size specified not large enough to draw image. (Bar size determined to be less than 1 pixel)");
            }

            //draw image
            int pos = 0;
            int halfBarWidth = (int)(iBarWidth * 0.5);

            using (Graphics g = Graphics.FromImage(b))
            {
                //clears the image and colors the entire background
                g.Clear(this._BackColor);

                //lines are fBarWidth wide so draw the appropriate color line vertically
                using (Pen backpen = new Pen(_BackColor, iBarWidth / iBarWidthModifier))
                {
                    using (Pen pen = new Pen(_ForeColor, iBarWidth / iBarWidthModifier))
                    {
                        while (pos < Code.Length)
                        {
                            if (Code[pos] == '1')
                            {
                                g.DrawLine(pen, new Point(pos * iBarWidth + shiftAdjustment + halfBarWidth, 0), new Point(pos * iBarWidth + shiftAdjustment + halfBarWidth, this._height));
                            }
                            pos++;
                        }//while
                    }//using
                }//using

                if (this._includeLabel)
                {
                    GenerateCodeLabel((Image)b);
                }//if

                EncodedImage = (Image)b;
                codeValue = Code;
                return b;
            }
        }

        /// <summary>
        /// Saves an encoded image to a specified file and type.
        /// </summary>
        /// <param name="Filename">Filename to save to.</param>
        /// <param name="FileType">Format to use.</param>
        public void SaveImage(string Filename)
        {
            try
            {
                if (this.Code != null)
                {
                    string FileType = Filename.Substring(Filename.LastIndexOf('.') + 1).ToUpper();
                    System.Drawing.Imaging.ImageFormat imageformat;
                    switch (FileType)
                    {
                        case "BMP": imageformat = System.Drawing.Imaging.ImageFormat.Bmp; break;
                        case "GIF": imageformat = System.Drawing.Imaging.ImageFormat.Gif; break;
                        case "JPG": imageformat = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                        case "PNG": imageformat = System.Drawing.Imaging.ImageFormat.Png; break;
                        case "TIFF": imageformat = System.Drawing.Imaging.ImageFormat.Tiff; break;
                        default: imageformat = System.Drawing.Imaging.ImageFormat.Png; break;
                    }//switch
          ((Bitmap)this.EncodedImage).Save(Filename, imageformat);
                }//if
            }//try
            catch (Exception ex)
            {
                throw new Exception("ESAVEIMAGE-1: Could not save image.\n\n=======================\n\n" + ex.Message);
            }//catch
        }//SaveImage(string, SaveTypes)

        private Image GenerateCodeLabel(Image img)
        {
            try
            {
                System.Drawing.Font font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);

                using (Graphics g = Graphics.FromImage(img))
                {
                    g.DrawImage(img, (float)0, (float)0);

                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;

                    StringFormat f = new StringFormat();
                    f.Alignment = StringAlignment.Near;
                    f.LineAlignment = StringAlignment.Near;
                    int LabelX = 0;
                    int LabelY = 0;

                    switch (this._labelPosition)
                    {
                        case LabelPositions.BOTTOMCENTER:
                            LabelX = img.Width / 2;
                            LabelY = img.Height - (font.Height);
                            f.Alignment = StringAlignment.Center;
                            break;
                        case LabelPositions.BOTTOMLEFT:
                            LabelX = 0;
                            LabelY = img.Height - (font.Height);
                            f.Alignment = StringAlignment.Near;
                            break;
                        case LabelPositions.BOTTOMRIGHT:
                            LabelX = img.Width;
                            LabelY = img.Height - (font.Height);
                            f.Alignment = StringAlignment.Far;
                            break;
                        case LabelPositions.TOPCENTER:
                            LabelX = img.Width / 2;
                            LabelY = 0;
                            f.Alignment = StringAlignment.Center;
                            break;
                        case LabelPositions.TOPLEFT:
                            LabelX = img.Width;
                            LabelY = 0;
                            f.Alignment = StringAlignment.Near;
                            break;
                        case LabelPositions.TOPRIGHT:
                            LabelX = img.Width;
                            LabelY = 0;
                            f.Alignment = StringAlignment.Far;
                            break;
                    }//switch

                    //color a background color box at the bottom of the barcode to hold the string of data
                    g.FillRectangle(new SolidBrush(this._BackColor), new RectangleF((float)0, (float)LabelY, (float)img.Width, (float)font.Height));

                    //draw datastring under the barcode image
                    g.DrawString(this._rawData, font, new SolidBrush(this._ForeColor), new RectangleF((float)0, (float)LabelY, (float)img.Width, (float)font.Height), f);

                    g.Save();
                }//using
                return img;
            }//try
            catch (Exception ex)
            {
                throw new Exception("ELABEL_GENERIC-1: " + ex.Message);
            }//catch
        }//Label_Generic
    }
}

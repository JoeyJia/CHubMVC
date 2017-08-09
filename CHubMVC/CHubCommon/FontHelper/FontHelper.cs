using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHubCommon.FontHelper
{
    public class FontHelper
    {
        public int GetFontHeight(string txt,Font font)
        {
            //Graphics graphics = CreateGraphics();
            //SizeF sizeF = graphics.MeasureString(textBox1.Text, new Font("宋体", 9));
            //MessageBox.Show(string.Format("字体宽度：{0}，高度：{1}", sizeF.Width, sizeF.Height));
            //graphics.Dispose();
            var result = TextRenderer.MeasureText(txt, font);
            return result.Height;
        }
    }
}

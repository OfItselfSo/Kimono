using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

/// +------------------------------------------------------------------------------------------------------------------------------+
/// ¦                                                   TERMS OF USE: MIT License                                                  ¦
/// +------------------------------------------------------------------------------------------------------------------------------¦
/// ¦Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation    ¦
/// ¦files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,    ¦
/// ¦modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software¦
/// ¦is furnished to do so, subject to the following conditions:                                                                   ¦
/// ¦                                                                                                                              ¦
/// ¦The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.¦
/// ¦                                                                                                                              ¦
/// ¦THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE          ¦
/// ¦WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR         ¦
/// ¦COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,   ¦
/// ¦ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                         ¦
/// +------------------------------------------------------------------------------------------------------------------------------+

namespace Kimono
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// A class to provide utility code
    /// </summary>
    public static class Utils
    {
        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Converts epoch time to a DateTime
        /// 
        /// Credit: https://stackoverflow.com/questions/60494296/c-sharp-epoch-datetime-conversion
        /// 
        /// </summary>
        public static DateTime FromEpochTime(double epochTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epochTime);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Scale font to width. We scale a font to a width so that the text is no 
        /// longer than that width. Note the input font is NOT cloned() the caller
        /// needs to do this if required.
        /// 
        /// Credit: https://stackoverflow.com/questions/791830/autoscale-font-in-a-textbox-control-so-that-its-as-big-as-possible-and-still-fit
        /// 
        /// </summary>
        /// <param name="boxWidth">the width the text has to fit in</param>
        /// <param name="fontToAdjust">the font we adjust. We do NOT clone this</param>
        /// <param name="textToUse">the text we have to fit in place</param>
        /// <returns>the scaled font, the old font for error or null for serious error</returns>
        public static Font AutoScaleFontToWidth(float boxWidth, Font fontToAdjust, string textToUse)
        {
            if (boxWidth <= 0) return null;
            if (textToUse == null) return null;
            if ((textToUse.Length == 0)) return null;

            float width = boxWidth * 0.99f;

            Font tmpFont = fontToAdjust;
            Size tempSize = System.Windows.Forms.TextRenderer.MeasureText(textToUse, tmpFont);

            float widthRatio = width / tempSize.Width;

            // just return the input font - use that again
            if (boxWidth > tempSize.Width) return fontToAdjust;

            tmpFont = new Font(tmpFont.FontFamily, (float)Math.Round(tmpFont.Size * widthRatio, 0), tmpFont.Style);

            return tmpFont;
        }

    }
}

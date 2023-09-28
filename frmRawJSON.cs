using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using OISCommon;

using System.Drawing.Imaging;

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
    /// Provides an form to display the JSON data 
    /// </summary>
    public partial class frmRawJSON : frmOISBase
    {
        private const char INDENT_CHAR = ' ';
        private int CHARS_PER_INDENT = 2;
        private const string DEFAULT_JSON_DATA = "No JSON Data";

        private string jsonData = DEFAULT_JSON_DATA;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jsonStrIn">the raw JSON string</param>
        public frmRawJSON(string jsonStrIn)
        {
            InitializeComponent();
            JSONData = jsonStrIn;
            ShowJSONData(PrettyFormatJSONData(JSONData));
            DialogResult = DialogResult.Cancel;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Show the JSON data on the screen
        /// </summary>
        private void ShowJSONData(string jsonStr)
        {
            if (jsonStr == null) jsonStr = "";
            richTextJSONData.Text = jsonStr;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get/Set the raw JSON data. Will never get/set null
        /// </summary>
        public string JSONData
        {
            get
            {
                if (jsonData == null) jsonData = "";
                return jsonData;
            }
            set
            {
                jsonData = value;
                if (jsonData == null) jsonData = "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Make the JSON string look Pretty
        /// </summary>
        /// <param name="jsonStr">the raw JSON string</param>
        private string PrettyFormatJSONData(string jsonStr)
        {
            StringBuilder sb = new StringBuilder();
            string indentStr = "";
            int indentCount = 0;

            if (jsonStr == null) return "";
            // remove all line feeds
            jsonStr = jsonStr.Replace("\r", "").Replace("\n", "");

            for (int index = 0; index < jsonStr.Length; index++)
            {
                // set our current indent string
                if (jsonStr[index] == '{')
                {
                    sb.Append("\r\n");
                    sb.Append(indentStr);
                    sb.Append(jsonStr[index]);
                    sb.Append("\r\n");
                    indentCount++;
                    if (indentCount > 5) indentCount = 5;
                    indentStr = new string(INDENT_CHAR, indentCount * CHARS_PER_INDENT);
                    sb.Append(indentStr);
                    continue;
                }
                else if (jsonStr[index] == '}')
                {
                    sb.Append("\r\n");
                    indentCount--;
                    if (indentCount < 0) indentCount = 0;
                    indentStr = new string(INDENT_CHAR, indentCount * CHARS_PER_INDENT);
                    sb.Append(indentStr);
                    sb.Append(jsonStr[index]);
                    sb.Append("\r\n");
                    continue;
                }
                else if (jsonStr[index] == ',')
                {
                    sb.Append(jsonStr[index]);
                    sb.Append("\r\n");
                    sb.Append(indentStr);
                }
                else
                {
                    // just append what we got
                    sb.Append(jsonStr[index]);
                }
            }
            // clean up and return
            string tmpOutStr = sb.ToString();
            tmpOutStr = tmpOutStr.Replace("\r\n\r\n", "\r\n");
            return tmpOutStr;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the cancel button
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a change on the  ShowRawJSONData check box
        /// </summary>
        private void checkBoxShowRawJSONData_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowRawJSONData.Checked == true)
            {
                // show it as raw
                ShowJSONData(JSONData);
            }
            else
            {
                ShowJSONData(PrettyFormatJSONData(JSONData));
            }
        }
    }
}


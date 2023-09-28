using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    /// A simple IP Address control. Only does rudimentary validation
    /// </summary>
    public partial class IPAddressControl : UserControl
    {
        private const int MIN_IPADDRESS_VALUE = 0;
        private const int MAX_IPADDRESS_VALUE = 254;
        private const string IPADDDR_DELIMITR_STR = ".";
        private const char IPADDDR_DELIMITR_CHAR = '.';
        public const string DEFAULT_IP_ADDRESS = "0.0.0.0";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public IPAddressControl()
        {
            InitializeComponent();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the IP address. Will never get/set null or an empty field. 
        /// Empty fields get a "0"
        /// </summary>
        public string IPAddress
        {
            get
            {
                if (textBoxIPAddrPart1.Text == "") textBoxIPAddrPart1.Text = "0";
                if (textBoxIPAddrPart2.Text == "") textBoxIPAddrPart2.Text = "0";
                if (textBoxIPAddrPart3.Text == "") textBoxIPAddrPart3.Text = "0";
                if (textBoxIPAddrPart4.Text == "") textBoxIPAddrPart4.Text = "0";
                return textBoxIPAddrPart1.Text + IPADDDR_DELIMITR_STR + textBoxIPAddrPart2.Text + IPADDDR_DELIMITR_STR + textBoxIPAddrPart3.Text + IPADDDR_DELIMITR_STR + textBoxIPAddrPart4.Text;
            }
            set
            {
                ClearIPAddressControl();
                if (value == null) value = DEFAULT_IP_ADDRESS;
                char[] sep = { IPADDDR_DELIMITR_CHAR };
                // separate the cleaned dataSource
                string[] strlist = value.Split(sep);
                // we expect four and only four
                if (strlist.Length >= 1) textBoxIPAddrPart1.Text = strlist[0];
                if (strlist.Length >= 2) textBoxIPAddrPart2.Text = strlist[1];
                if (strlist.Length >= 3) textBoxIPAddrPart3.Text = strlist[2];
                if (strlist.Length >= 4) textBoxIPAddrPart4.Text = strlist[3];
                if (textBoxIPAddrPart1.Text == "") textBoxIPAddrPart1.Text = "0";
                if (textBoxIPAddrPart2.Text == "") textBoxIPAddrPart2.Text = "0";
                if (textBoxIPAddrPart3.Text == "") textBoxIPAddrPart3.Text = "0";
                if (textBoxIPAddrPart4.Text == "") textBoxIPAddrPart4.Text = "0";

            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clears the ip address control
        /// </summary>
        public void ClearIPAddressControl()
        {
            textBoxIPAddrPart1.Text = "0";
            textBoxIPAddrPart2.Text = "0";
            textBoxIPAddrPart3.Text = "0";
            textBoxIPAddrPart4.Text = "0";
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Is the IP address valid
        /// </summary>
        public bool IPAddressIsValid()
        {
            int textAsInt = -1;
            // we have only numbers, is the range acceptable
            Int32.TryParse(this.textBoxIPAddrPart1.Text, out textAsInt);
            if ((textAsInt < MIN_IPADDRESS_VALUE) || (textAsInt >= MAX_IPADDRESS_VALUE))
            {
                // not acceptable
                return false;

            }
            // we have only numbers, is the range acceptable
            Int32.TryParse(this.textBoxIPAddrPart2.Text, out textAsInt);
            if ((textAsInt < MIN_IPADDRESS_VALUE) || (textAsInt >= MAX_IPADDRESS_VALUE))
            {
                // not acceptable
                return false;

            }
            // we have only numbers, is the range acceptable
            Int32.TryParse(this.textBoxIPAddrPart3.Text, out textAsInt);
            if ((textAsInt < MIN_IPADDRESS_VALUE) || (textAsInt >= MAX_IPADDRESS_VALUE))
            {
                // not acceptable
                return false;

            }
            // we have only numbers, is the range acceptable
            Int32.TryParse(this.textBoxIPAddrPart4.Text, out textAsInt);
            if ((textAsInt < MIN_IPADDRESS_VALUE) || (textAsInt >= MAX_IPADDRESS_VALUE))
            {
                // not acceptable
                return false;

            }
            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a key press
        /// </summary>
        private void textBoxIPAddrPart1_KeyPress(object sender, KeyPressEventArgs e)
        {

            // are we anything but a number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // not a digit, not acceptable
                e.Handled = true;
                return;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a key press
        /// </summary>
        private void textBoxIPAddrPart2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // are we anything but a number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // not a digit, not acceptable
                e.Handled = true;
                return;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a key press
        /// </summary>
        private void textBoxIPAddrPart3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // are we anything but a number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // not a digit, not acceptable
                e.Handled = true;
                return;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a key press
        /// </summary>
        private void textBoxIPAddrPart4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // are we anything but a number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // not a digit, not acceptable
                e.Handled = true;
                return;
            }
        }
    }
}

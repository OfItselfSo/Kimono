using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
    /// A monitor block to show a string, note we inherit from ctlMonitorBlock_Base
    /// not directly from UserControl. This gives us base functionality
    /// </summary>
    public partial class ctlMonitorBlock_Text : ctlMonitorBlock_Base
    {
        private Font defaultFont = null;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Text()
        {
            InitializeComponent();
            // set our properties now
            Properties = new MonitorBlockProperties_Text();
            //set our default font
            defaultFont = (Font)textBoxIntegerValue.Font.Clone();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {

            // sanity check
            if ((Properties is MonitorBlockProperties_Text) == false) return;
            textBoxTitle.Text = (Properties as MonitorBlockProperties_Text).TitleText;
            // reset this 
            (Properties as MonitorBlockProperties_Text).TextValue = "?";
            textBoxIntegerValue.Text = "?";
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the data on the display, according to the properties. This only
        /// does the changeable data not user settable things like titles.
        /// </summary>
        public override void SyncData()
        {
            // sanity check
            if ((Properties is MonitorBlockProperties_Text) == false) return;

            // auto scale font size so the text appears in full
            string tmpText = FormatDisplayTextFromProperties();
            Font outFont = Utils.AutoScaleFontToWidth(textBoxIntegerValue.Width, (Font)defaultFont.Clone(), tmpText);
            if (outFont != null) textBoxIntegerValue.Font = outFont;

            // set the text data
            textBoxIntegerValue.Text = tmpText;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Formats the output data according to the properties
        /// </summary>
        private string FormatDisplayTextFromProperties()
        {
            if ((Properties is MonitorBlockProperties_Text) == false) return "";
            MonitorBlockProperties_Text blockProperties = (MonitorBlockProperties_Text)Properties;
            StringBuilder sb = new StringBuilder();
            sb.Append(blockProperties.Prefix);
            sb.Append(blockProperties.TextValue.ToString());
            sb.Append(blockProperties.Suffix);
            return sb.ToString();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Mouse down handler
        /// </summary>
        private void panelBlockDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            // the only thing we care about here is setting up for a drag out of the listbox
            // get the object backing the data
            MonitorBlockProperties_Base mbObj = this.Properties;
            // set the drag drop effects
            MonitorBlockDragDropContainer ddObj = new MonitorBlockDragDropContainer(this, mbObj);
            DragDropEffects dde1 = DoDragDrop(ddObj, DragDropEffects.Copy);

        }
    }
}

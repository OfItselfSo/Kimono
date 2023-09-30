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
    /// A monitor block to show a number, note we inherit from ctlMonitorBlock_Base
    /// not directly from UserControl. This gives us base functionality
    /// </summary>
    public partial class ctlMonitorBlock_Number : ctlMonitorBlock_Base
    {
        private const string ERROR_STR = "#Err";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Number()
        {
            InitializeComponent();
            // set our properties now
            Properties = new MonitorBlockProperties_Number();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {

            // sanity check
            if ((Properties is MonitorBlockProperties_Number) == false) return;
            textBoxTitle.Text = (Properties as MonitorBlockProperties_Number).TitleText;
            // reset this 
            (Properties as MonitorBlockProperties_Number).NumberValue = 0;
            (Properties as MonitorBlockProperties_Number).NumberValueIsValid = false;
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
            if ((Properties is MonitorBlockProperties_Number) == false) return;
            // set the integer data
            textBoxIntegerValue.Text = FormatDisplayTextFromProperties();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Formats the output data according to the properties
        /// </summary>
        private string FormatDisplayTextFromProperties()
        {
            if ((Properties is MonitorBlockProperties_Number) == false) return "";
            MonitorBlockProperties_Number blockProperties = (MonitorBlockProperties_Number)Properties;

            StringBuilder sb = new StringBuilder();
            if (blockProperties.NumberValueIsValid == false)
            {
                sb.Append(ERROR_STR);
            }
            else
            {
                // do we wish to round the display value
                double tmpValue = blockProperties.NumberValue;
                int decimalPlaces = blockProperties.DecimalPlaces;
                if (decimalPlaces >= 0) tmpValue = Math.Round(tmpValue, decimalPlaces);
               
                sb.Append(blockProperties.Prefix);
                sb.Append(tmpValue.ToString());
                sb.Append(blockProperties.Suffix);
            }
            return sb.ToString();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Mouse down handler
        /// </summary>
        private void panelBlockDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            // because we set up for drag drop on the mousedown we mess up the 
            // double click event. We trap it here rather than having a separate 
            // event handler for it.
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                // single moust down even

                // the only thing we care about here is setting up for a drag out of the listbox
                // get the object backing the data. There are many other function in scottplot
                // but the little mini display does not take advantage of them
                MonitorBlockProperties_Base mbObj = this.Properties;
                // set the drag drop effects
                MonitorBlockDragDropContainer ddObj = new MonitorBlockDragDropContainer(this, mbObj);
                DragDropEffects dde1 = DoDragDrop(ddObj, DragDropEffects.Copy);
            }
            else if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                // double click event, we launch the plot viewer here as a modeless dialog
                frmPlotReport frmPlot = new frmPlotReport();
                frmPlot.ConfigurePlotToMonitorBlock(Properties);
                frmPlot.Show();
            }
        }
    }
}

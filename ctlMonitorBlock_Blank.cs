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
    /// A monitor block to show a blank panel, note we inherit from ctlMonitorBlock_Base
    /// not directly from UserControl. This gives us base functionality
    /// </summary>
    public partial class ctlMonitorBlock_Blank : ctlMonitorBlock_Base
    {

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Blank()
        {
            InitializeComponent();
            // set our properties now
            Properties = new MonitorBlockProperties_Blank();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {

            // sanity check
            if ((Properties is MonitorBlockProperties_Blank) == false) return;
            textBoxTitle.Text = (Properties as MonitorBlockProperties_Blank).TitleText;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the data on the display, according to the properties. This only
        /// does the changeable data not user settable things like titles.
        /// </summary>
        public override void SyncData()
        {
            // sanity check
            if ((Properties is MonitorBlockProperties_Blank) == false) return;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Formats the output data according to the properties
        /// </summary>
        private string FormatDisplayTextFromProperties()
        {
            return "";
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

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
    public partial class ctlMonitorBlock_Integral : ctlMonitorBlock_Base
    {
        private const string ERROR_STR = "#Err";

        private const int NUMBER_OF_SECONDS_IN_A_MINUTE = 60;
        private const int NUMBER_OF_SECONDS_IN_AN_HOUR = 60*60;

        private Font defaultFont = null;

        // we have a recorder because we need to know to rezero at midnite
        private IntervalRecorder intervalRecorder = new IntervalRecorder();
        // we store the last datasource because if the user changes it we reset the accumulator
        private string lastDataSource = "";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Integral()
        {
            InitializeComponent();
            // set our properties now
            Properties = new MonitorBlockProperties_Integral();
            //set our default font
            defaultFont = (Font)textBoxIntegerValue.Font.Clone();
            // set our last datasource now
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {

            // sanity check
            if ((Properties is MonitorBlockProperties_Integral) == false) return;
            MonitorBlockProperties_Integral propObj = (MonitorBlockProperties_Integral)Properties;

            textBoxTitle.Text = propObj.TitleText;
            // reset this 
            propObj.NumberValue = 0;
            propObj.NumberValueIsValid = false;

            // reset accumulator only if datasource has changed
            if(propObj.DataSource!=LastDataSource)
            {
                propObj.Accumulator = 0;
                propObj.LastResetTime = DateTime.Now;
            }

            LastDataSource = propObj.DataSource;

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
            if ((Properties is MonitorBlockProperties_Integral) == false) return;

            MonitorBlockProperties_Integral propObj = (Properties as MonitorBlockProperties_Integral);

            // note the current dateTime. No need to let this change during all of the calcs
            DateTime timeNow = DateTime.Now;

            // update our interval Recorder
            IntervalRecorder.UpdateMarkerFlags(timeNow);
            // do we want to reset at midnite
            if (propObj.ResetAtMidnite == true)
            {
                // yes we do, if its time then do it
                if (IntervalRecorder.DayMarkerFlag == true)
                {
                    propObj.Accumulator = 0;
                    propObj.LastResetTime = timeNow;
                }
            }

            // set our accumulator now
            double diffInSeconds = (timeNow - propObj.LastRecordedDateTime).TotalSeconds; 
            if ((diffInSeconds<0) || (diffInSeconds> 100))
            {
                // probably our first run though, just set our last date and a dummy val
                propObj.Accumulator = 0;
                propObj.NumberValue = 0;
                propObj.LastResetTime = timeNow;
            }
            else
            {
                // we can add to our accumulator
                propObj.Accumulator = propObj.Accumulator + (propObj.NumberValue * diffInSeconds);
            }
            // always set this
            propObj.LastRecordedDateTime = timeNow;

            // set the integer data
            string tmpText = FormatDisplayTextFromProperties();

            Font outFont = Utils.AutoScaleFontToWidth(textBoxIntegerValue.Width, (Font)defaultFont.Clone(), tmpText);
            if (outFont != null) textBoxIntegerValue.Font = outFont;

            // set the text
            textBoxIntegerValue.Text = tmpText;

            // now set the 
            TimeSpan totalDiff = (timeNow - propObj.LastResetTime);
            textBoxTimeDuration.Text = "Over " + totalDiff.ToString(@"hh\:mm\:ss");
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Formats the output data according to the properties
        /// </summary>
        private string FormatDisplayTextFromProperties()
        {
            if ((Properties is MonitorBlockProperties_Integral) == false) return "";
            MonitorBlockProperties_Integral blockProperties = (MonitorBlockProperties_Integral)Properties;

            StringBuilder sb = new StringBuilder();
            if (blockProperties.NumberValueIsValid == false)
            {
                sb.Append(ERROR_STR);
            }
            else
            {               
                double tmpValue = blockProperties.Accumulator;
                // convert to hours (Accumulator is in seconds)
                tmpValue = tmpValue / NUMBER_OF_SECONDS_IN_AN_HOUR;

                // do we wish to round the display value
                int decimalPlaces = blockProperties.DecimalPlaces;
                if (decimalPlaces >= 0) tmpValue = Math.Round(tmpValue, decimalPlaces);

                // remember this now
                blockProperties.LastDisplayValue = tmpValue;

                // display what we have
                sb.Append(blockProperties.Prefix);
                sb.Append(blockProperties.LastDisplayValue.ToString());
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

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the last datasource used. Will never get/set null will get/set empty
        /// </summary>
        private string LastDataSource
        {
            get
            {
                if (lastDataSource == null) lastDataSource = "";
                return lastDataSource;
            }
            set
            {
                lastDataSource = value;
                if (lastDataSource == null) lastDataSource = "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets intervalRecorder. We never get null
        /// </summary>
        public IntervalRecorder IntervalRecorder
        {
            get
            {
                if (intervalRecorder == null) intervalRecorder = new IntervalRecorder();
                return intervalRecorder;
            }
        }

    }
}

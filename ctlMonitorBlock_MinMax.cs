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
    /// A monitor block to trigger on a number value and record the datetime of the 
    /// event, note we inherit from ctlMonitorBlock_Base
    /// not directly from UserControl. This gives us base functionality
    /// </summary>
    public partial class ctlMonitorBlock_MinMax : ctlMonitorBlock_Base
    {
        private const string ERROR_STR = "#Err";
        private const string NOOP_STR = "-/-";
        private const string MAX_PREFIX = "Max:";
        private const string MIN_PREFIX = "Min:";

        private Font defaultFont = null;
        // we have a recorder because we need to know to rezero at midnite
        private IntervalRecorder intervalRecorder = new IntervalRecorder();

        // we store the last datasource because if the user changes it we reset the max/min
        private string lastDataSource = "";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_MinMax()
        {
            InitializeComponent();
            // make it so we can set a specific height of the text box
            textBoxValueMax.AutoSize = false;
            textBoxValueMax.Height = 42;

           // set our properties now
           Properties = new MonitorBlockProperties_MinMax();
            //set our default font
            defaultFont = (Font)textBoxValueMax.Font.Clone();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {

            // sanity check
            if ((Properties is MonitorBlockProperties_MinMax) == false) return;
            MonitorBlockProperties_MinMax blockProperties = (MonitorBlockProperties_MinMax)Properties;

            textBoxTitle.Text = (Properties as MonitorBlockProperties_MinMax).TitleText;

            // reset the box only if datasource has changed
            if (blockProperties.DataSource != LastDataSource)
            {
                // reset the box
                ResetTheBlock(blockProperties);
                // remove all data
                blockProperties.RecordListMax.Clear();
                blockProperties.RecordListMin.Clear();
                blockProperties.TodaysRecordMax = null;
                blockProperties.TodaysRecordMin = null;
            }
            LastDataSource = blockProperties.DataSource;
            // always reset to the number of days
            blockProperties.ClearOldRecordsMax(blockProperties.DaysToDisplay);
            blockProperties.ClearOldRecordsMin(blockProperties.DaysToDisplay);

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the data on the display. This only
        /// does the changeable data not user settable things like titles.
        /// </summary>
        public override void SyncData()
        {
            StringBuilder sbMax = new StringBuilder();
            StringBuilder sbMin = new StringBuilder();
            string tmpText = null;
            Font outFont = null;

            // sanity check
            if ((Properties is MonitorBlockProperties_MinMax) == false) return;
            MonitorBlockProperties_MinMax blockProperties = (MonitorBlockProperties_MinMax)Properties;
            // note the current dateTime. No need to let this change during all of the calcs
            DateTime timeNow = DateTime.Now;

            // update our interval Recorder
            IntervalRecorder.UpdateMarkerFlags(timeNow);
            // do we want to reset at midnite
            if (blockProperties.ResetAtMidnite == true)
            {
                // yes we do, if its time then do it
                if (IntervalRecorder.DayMarkerFlag == true)
                {
                    ResetTheBlock(blockProperties);
                    // always reset to the number of days
                    blockProperties.ClearOldRecordsMax(blockProperties.DaysToDisplay);
                    blockProperties.ClearOldRecordsMin(blockProperties.DaysToDisplay);
                    // always create a new record. Nulling todays record will be sufficient
                    blockProperties.TodaysRecordMax = null;
                    blockProperties.TodaysRecordMin = null;
                }
            }

            // set the display text data Max value
            DoubleRecord maxValue = blockProperties.GetMaxValue();
            // set the StringBuilder one way or the other
            if (maxValue == null) sbMax.Append(ERROR_STR);
            else
            {
                if ((blockProperties.Prefix != null) && (blockProperties.Prefix.Length > 0)) sbMax.Append(blockProperties.Prefix);
                int decimalPlaces = blockProperties.DecimalPlaces;
                double tmpValue = maxValue.DoubleValue;
                if (decimalPlaces >= 0) tmpValue = Math.Round(tmpValue, decimalPlaces);
                sbMax.Append(tmpValue.ToString());
                if ((blockProperties.Suffix != null) && (blockProperties.Suffix.Length > 0)) sbMax.Append(blockProperties.Suffix);
            }
            // get it as text
            tmpText = MAX_PREFIX + " " + sbMax.ToString();
            // scale it
            outFont = Utils.AutoScaleFontToWidth(textBoxValueMax.Width, (Font)defaultFont.Clone(), tmpText);
            if (outFont != null) textBoxValueMax.Font = outFont;
            // set it
            textBoxValueMax.Text = tmpText;

            // set the display text data Min value
            DoubleRecord minValue = blockProperties.GetMinValue();
            // set the StringBuilder one way or the other
            if (minValue == null) sbMin.Append(ERROR_STR);
            else
            {
                if ((blockProperties.Prefix != null) && (blockProperties.Prefix.Length > 0)) sbMin.Append(blockProperties.Prefix);
                int decimalPlaces = blockProperties.DecimalPlaces;
                double tmpValue = minValue.DoubleValue;
                if (decimalPlaces >= 0) tmpValue = Math.Round(tmpValue, decimalPlaces);
                sbMin.Append(tmpValue.ToString());
                if ((blockProperties.Suffix != null) && (blockProperties.Suffix.Length > 0)) sbMin.Append(blockProperties.Suffix);
            }
            // get it as text
            tmpText = MIN_PREFIX + " " + sbMin.ToString();
            // scale it
            outFont = Utils.AutoScaleFontToWidth(textBoxValueMin.Width, (Font)defaultFont.Clone(), tmpText);
            if (outFont != null) textBoxValueMin.Font = outFont;
            // set it
            textBoxValueMin.Text = tmpText;

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Reset the monitor block
        /// </summary>
        private void ResetTheBlock(MonitorBlockProperties_MinMax blockProperties)
        {
            textBoxValueMax.Text = MAX_PREFIX + " "  + NOOP_STR;
            textBoxValueMin.Text = MIN_PREFIX + " " + NOOP_STR;
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

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
    public partial class ctlMonitorBlock_Latch : ctlMonitorBlock_Base
    {
        private const string ERROR_STR = "#Err";
        private const string NOOP_STR = "-/-";

        private Font defaultFont = null;
        // we have a recorder because we need to know to rezero at midnite
        private IntervalRecorder intervalRecorder = new IntervalRecorder();

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Latch()
        {
            InitializeComponent();
            // make it so we can set a specific height of the text box
            textBoxTriggerText.AutoSize = false;
            textBoxTriggerText.Height = 42;

           // set our properties now
           Properties = new MonitorBlockProperties_Latch();
            //set our default font
            defaultFont = (Font)textBoxTriggerText.Font.Clone();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {

            // sanity check
            if ((Properties is MonitorBlockProperties_Latch) == false) return;
            MonitorBlockProperties_Latch blockProperties = (MonitorBlockProperties_Latch)Properties;

            textBoxTitle.Text = (Properties as MonitorBlockProperties_Latch).TitleText;
            // reset this 
            (Properties as MonitorBlockProperties_Latch).LastValue = 0;
            (Properties as MonitorBlockProperties_Latch).LastValueIsValid = false;

            // reset the box
            ResetTheBlock(blockProperties);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the data on the display, according to the properties. This only
        /// does the changeable data not user settable things like titles.
        /// </summary>
        public override void SyncData()
        {
            // sanity check
            if ((Properties is MonitorBlockProperties_Latch) == false) return;
            MonitorBlockProperties_Latch blockProperties = (MonitorBlockProperties_Latch)Properties;
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
                }
            }

            // set the display text data
            string tmpText = FormatDisplayTextFromProperties();

            Font outFont = Utils.AutoScaleFontToWidth(textBoxTriggerText.Width, (Font)defaultFont.Clone(), tmpText);
            if (outFont != null) textBoxTriggerText.Font = outFont;

            textBoxTriggerText.Text = tmpText;

            // display the trigger date if a trigger has happened
            if (blockProperties.HasEverTriggered() == false) textBoxTriggerTime.Text = "";
            else textBoxTriggerTime.Text = blockProperties.LastRecordedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Formats the output data according to the properties
        /// </summary>
        private string FormatDisplayTextFromProperties()
        {
            bool triggerResult = false;

            if ((Properties is MonitorBlockProperties_Latch) == false) return "";
            MonitorBlockProperties_Latch blockProperties = (MonitorBlockProperties_Latch)Properties;

            StringBuilder sb = new StringBuilder();
            if (blockProperties.LastValueIsValid == false)
            {
                sb.Append(ERROR_STR);
                return sb.ToString();
            }

            // ok the value is good, figure out what type it is
            if ((blockProperties.LastValue is double) == true)
            {
                // it is a double
                double compareVal = 0;
                try
                {
                    compareVal = Convert.ToDouble(blockProperties.ComparisonValue);
                }
                catch 
                {
                    sb.Append(ERROR_STR);
                    return sb.ToString();
                }
                // test if we triggered
                triggerResult = TestForTrigger(blockProperties.TriggerDirection, (double)blockProperties.LastValue, compareVal);
            }
            else if ((blockProperties.LastValue is string) == true)
            {
                // it is a string
                string compareVal = blockProperties.ComparisonValue;
                try
                {
                    compareVal = blockProperties.ComparisonValue;
                }
                catch
                {
                    sb.Append(ERROR_STR);
                    return sb.ToString();
                }
                // test if we triggered
                triggerResult = TestForTrigger(blockProperties.TriggerDirection, (string)blockProperties.LastValue, compareVal);
            }

            // fall through. We may or may not have triggered

            // have we ever triggered
            bool triggerHasPreviouslyHappened = blockProperties.HasEverTriggered();
            if (triggerResult == false)
            {
                // we did not trigger, but have we in the past?
                if (triggerHasPreviouslyHappened == false) sb.Append(blockProperties.DisplayTextNotTriggered);
                // we have triggered in the past so display this
                else sb.Append(blockProperties.DisplayTextTriggered);
                return sb.ToString();
            }
            else
            {
                // we did trigger, definitely display this
                sb.Append(blockProperties.DisplayTextTriggered);
                // but do we record the last time of trigger
                if ((triggerHasPreviouslyHappened == false) || (blockProperties.TriggerAction == MB_TriggerActionEnum.RECORD_LATEST))
                {
                    // definitely do it if this is the case
                    blockProperties.LastRecordedDateTime = DateTime.Now;
                }
            return sb.ToString();
            }

            //sb.Append(ERROR_STR);
            //return sb.ToString();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detect if the trigger has occurred - double version
        /// </summary>
        /// <param name="comparisonValue">the comparison value</param>
        /// <param name="triggerDirection">the trigger direction</param>
        /// <param name="lastValue">the last value</param>
        private bool TestForTrigger(MB_TriggerDirectionEnum triggerDirection, double lastValue, double comparisonValue)
        {
            if ((triggerDirection == MB_TriggerDirectionEnum.IS_GREATER_EQ) == true)
            {
                if (lastValue >= comparisonValue) return true;
                else return false;
            }
            if ((triggerDirection == MB_TriggerDirectionEnum.IS_GREATER) == true)
            {
                if (lastValue > comparisonValue) return true;
                else return false;
            }
            else if ((triggerDirection == MB_TriggerDirectionEnum.IS_LESS_EQ) == true)
            {
                if (lastValue <= comparisonValue) return true;
                else return false;
            }
            else if ((triggerDirection == MB_TriggerDirectionEnum.IS_LESS) == true)
            {
                if (lastValue < comparisonValue) return true;
                else return false;
            }
            else if ((triggerDirection == MB_TriggerDirectionEnum.IS_EQUAL) == true)
            {
                if (lastValue == comparisonValue) return true;
                else return false;
            }
            // probably MB_TriggerDirectionEnum.NONE

            return false;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detect if the trigger has occurred - string version
        /// </summary>
        /// <param name="comparisonValue">the comparison value</param>
        /// <param name="triggerDirection">the trigger direction</param>
        /// <param name="lastValue">the last value</param>
        private bool TestForTrigger(MB_TriggerDirectionEnum triggerDirection, string lastValue, string comparisonValue)
        {
            int compareInt = lastValue.CompareTo(comparisonValue);
            if ((triggerDirection == MB_TriggerDirectionEnum.IS_GREATER_EQ) == true)
            {
                if (compareInt >= 0) return true;
                else return false;
            }
            if ((triggerDirection == MB_TriggerDirectionEnum.IS_GREATER) == true)
            {
                if (compareInt > 0) return true;
                else return false;
            }
            else if ((triggerDirection == MB_TriggerDirectionEnum.IS_LESS_EQ) == true)
            {
                if (compareInt <= 0) return true;
                else return false;
            }
            else if ((triggerDirection == MB_TriggerDirectionEnum.IS_LESS) == true)
            {
                if (compareInt < 0) return true;
                else return false;
            }
            else if ((triggerDirection == MB_TriggerDirectionEnum.IS_EQUAL) == true)
            {
                if (compareInt == 0) return true;
                else return false;
            }
            // probably MB_TriggerDirectionEnum.NONE

            return false;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Reset the monitor block
        /// </summary>
        private void ResetTheBlock(MonitorBlockProperties_Latch blockProperties)
        {
            // reset the box
            textBoxTriggerText.Text = "?";
            textBoxTriggerTime.Text = "";
            if(blockProperties!=null) blockProperties.LastRecordedDateTime = DateTime.MinValue;
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

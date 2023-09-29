using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;
using ScottPlot.Plottable;

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
    /// A monitor block to show a graph, note we inherit from ctlMonitorBlock_Base
    /// not directly from UserControl. This gives us base functionality
    /// </summary>
    public partial class ctlMonitorBlock_Graph : ctlMonitorBlock_Base
    {

        MonitorBlockPlotConfig_Graph plotConfig = new MonitorBlockPlotConfig_Graph();

        // if true we cannot update the plot
        bool lockThePlot = false;

        // thios is the scatter plot, it can be null
        private ScatterPlot scatterPlot1 = null;

        private const int DEFAULT_DB_REFRESH_INTERVAL_MIN = 2;
        private int dbAccessInterval_Min = DEFAULT_DB_REFRESH_INTERVAL_MIN; // we only pull from the db this often
        private DateTime lastDBAccessTime = DateTime.MinValue; // last time we pulled from the db

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Graph()
        {
            InitializeComponent();
            // set our properties now
            Properties = new MonitorBlockProperties_Graph();

            // set up the plot
            ConfigureThePlot();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Configures the plot in a basic way
        /// </summary>
        /// <returns>z success, nz fail</returns>
        private void ConfigureThePlot()
        {
            // the x axis is always time
            Plot.XAxis.DateTimeFormat(true);

            // this is the bottom axis
            Plot.XAxis.Layout(padding: 4);
            Plot.XAxis.Ticks(false);
            Plot.XAxis.Label("", size: 1);

            // this is the top axis, note the plot title is the label here
            Plot.XAxis2.Layout(padding: 1);

            // this is the left hand axis
            Plot.YAxis.Label("", size: 1);
            Plot.YAxis.Layout(padding: 0);
            Plot.YAxis.TickLabelStyle(rotation: 45, fontSize: 8);
            //  Plot.YAxis.Layout(tick: 0);

            // this is the right hand axis
            Plot.YAxis2.Label("", size: 1);
            Plot.YAxis2.Layout(padding: 4);

            // set up the color pallette
            Plot.Palette = new ScottPlot.Palettes.Category10();

            formsPlot1.Configuration.LockHorizontalAxis = true;
            formsPlot1.Configuration.LockVerticalAxis = true;

            // we never want the benchmark to appear
            Plot.Benchmark(false);

            // PlotTitle = DEFAULT_PLOT_TITLE;
            formsPlot1.Refresh();

            // we have never accessed the db
            LastDBAccessTime = DateTime.MinValue;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Refreshes the plot, will not refresh if things are locked
        /// </summary>
        private void RefreshThePlot()
        {
            if (lockThePlot == true) return;
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the plot configuration option for this Control. Never gets/sets
        /// null
        /// </summary>
        public MonitorBlockPlotConfig_Graph PlotConfig
        {
            get
            {
                if (plotConfig == null) plotConfig = new MonitorBlockPlotConfig_Graph();
                return plotConfig;
            }
            set
            {
                plotConfig = value;
                if (plotConfig == null) plotConfig = new MonitorBlockPlotConfig_Graph();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display setup to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public override void SyncDisplayToProperties()
        {
            // sanity check
            if ((Properties is MonitorBlockProperties_Graph) == false) return;

            try
            {
                PlotConfig.MBProperties = (MonitorBlockProperties_Graph)Properties;
                PlotConfig.InitThePlotConfiguration();

                lockThePlot = true;
                MonitorBlockProperties_Graph workingProp = (MonitorBlockProperties_Graph)Properties;
                Plot.Title(workingProp.TitleText, false);
            }
            finally
            {
                lockThePlot = false;
                // always reset this so we fetch data immediately
                LastDBAccessTime = DateTime.MinValue;
            }

            RefreshThePlot();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the data on the display, according to the properties. This only
        /// does the changeable data not user settable things like titles.
        /// </summary>
        public override void SyncData()
        {
            // sanity check
            if ((Properties is MonitorBlockProperties_Graph) == false) return;

            // how long since we last updated
            TimeSpan ts = DateTime.Now - LastDBAccessTime;
            // do not sync if less than this
            if (ts.TotalMinutes < DBAccessInterval_Min) return;

            try
            {
                // we clear down and rebuild from scratch
                Plot.Clear();

                // we have to do this. This is what rolls over our dates at midnight
                PlotConfig.InitThePlotConfiguration();

                string deviceAlias = "";
                string fieldName = "";
                int retInt = OutbackSystem.GetDeviceNameAndFieldFromDataSource((Properties as MonitorBlockProperties_Graph).DataSource, out deviceAlias, out fieldName);
                if (retInt != 0) return;
                if (deviceAlias == null) return;
                if (deviceAlias.Length == 0) return;
                if (fieldName == null) return;
                if (fieldName.Length == 0) return;

                // fetch the data
                ScatterPlot workingPlot = PlotConfig.FetchPlotOfData(deviceAlias, fieldName);

                if (workingPlot == null) return;
                // set the plot color from the pallette
                workingPlot.Color = Plot.Palette.GetColor(0);
                ScatterPlot1 = workingPlot;


                // add it to the plot
                Plot.Add(scatterPlot1);

                AddAppropriateDayLines();

                // record this
                LastDBAccessTime = DateTime.Now;

            }
            finally
            {
                RefreshThePlot();
            }
        }

        // +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        // <summary>
        // Set our vertical day lines so the user has a nice reference. We do not 
        // have an X axis marker for size reasons
        // </summary>
        private void AddAppropriateDayLines()
        {
            DateTime workingDate;
            double dateAsDouble;

            int numDaysInGraph = (Properties as MonitorBlockProperties_Graph).DaysToDisplay;
            if (numDaysInGraph > MonitorBlockPlotConfig_Graph.MAX_DAYLINES_ON_PLOT) return;

            if ((Properties as MonitorBlockProperties_Graph).DataDateEndPoint == DataDateEndpontEnum.FIXED_EOD)
            {
                // we always add in a vertical line for the end of the day. This is what gives us the blank space
                // between the end of the data and the end of the day
                workingDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                dateAsDouble = workingDate.ToOADate();
                Plot.AddVerticalLine(x: dateAsDouble, label: "", color: Color.Magenta, width: 1, style: LineStyle.Dot);
            }

            // Now we place a vertical line at the beginning of every day for each day we have
            if (numDaysInGraph <= 0) numDaysInGraph = 1;
            for (int day=0; day < numDaysInGraph; day++)
            {
                // plot the day lines
                workingDate = DateTime.Now.Date.AddDays(day*-1);
                dateAsDouble = workingDate.ToOADate();
                Plot.AddVerticalLine(x: dateAsDouble, label: "", color: Color.Magenta, width: 1, style: LineStyle.Dot);
            }
        }

        // +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        // <summary>
        // Gets the actual Plot object in this control
        // </summary>
        public ScottPlot.Plot Plot
        {
            get
            {
                return this.formsPlot1.Plot;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets ScatterPlot1. Can be null
        /// </summary>
        public ScatterPlot ScatterPlot1 { get => scatterPlot1; set => scatterPlot1 = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the last time we accessed the database
        /// </summary>
        public DateTime LastDBAccessTime { get => lastDBAccessTime; set => lastDBAccessTime = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets how often we access the database
        /// </summary>
        public int DBAccessInterval_Min { get => dbAccessInterval_Min; set => dbAccessInterval_Min = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Formats the output data according to the properties
        /// </summary>
        private string FormatDisplayTextFromProperties()
        {
            if ((Properties is MonitorBlockProperties_Graph) == false) return "";
            MonitorBlockProperties_Graph blockProperties = (MonitorBlockProperties_Graph)Properties;
            StringBuilder sb = new StringBuilder();
            sb.Append(blockProperties.GraphValue.ToString());
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

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Mouse down handler
        /// </summary>
        private void formsPlot1_MouseDown(object sender, MouseEventArgs e)
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

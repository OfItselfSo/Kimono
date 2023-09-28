using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OISCommon;
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
    /// A form to display a plot of historical data from the database
    /// </summary>
    public partial class frmPlotReport : frmOISBase
    {

        private const string DEFAULT_ACTIVEPLOT_TITLE_PREFIX = "Plot of ";
        private const string DEFAULT_PLOT_TITLE = "Data Plot";

        private const int DEFAULT_LINE_THICKNESS = 1;
        private const int DEFAULT_MARKER_THICKNESS = 1;

        private const string FIVE_MINUTE_MARKER_AS_STRING = "FiveMinuteMarker";
        private const string TEN_MINUTE_MARKER_AS_STRING = "TenMinuteMarker";
        private const string FIFTEEN_MINUTE_MARKER_AS_STRING = "FifteenMinuteMarker";
        private const string THIRTY_MINUTE_MARKER_AS_STRING = "ThirtyMinuteMarker";
        private const string ONE_HOUR_MARKER_AS_STRING = "HourMarker";
        private const string ONE_DAY_MARKER_AS_STRING = "DayMarker";

        private const DataIntervalEnum DEFAULT_DATA_INTERVAL = DataIntervalEnum.FIVE_MIN;

        private const string FIELDNAME_REPSTR = "%FieldNameAsString%";
        private const string DEVICEALIAS_REPSTR = "%DeviceAliasAsString%";
        private const string MARKER_REPSTR = "%MarkerAsString%";
        private const string TABLE_REPSTR = "%TableAsString%";
        private const string START_DATETIME_REPSTR = "%STARTDATETIME%";
        private const string END_DATETIME_REPSTR = "%ENDDATETIME%";

        private const string SQLQUERY = "select recordDate, %FieldNameAsString% from PortData_%TableAsString% where DeviceAlias='%DeviceAliasAsString%' and RecordDate>='%STARTDATETIME%' and RecordDate<='%ENDDATETIME%' and %MarkerAsString% = True order by recordDate";

        // this is the date which at the right hand margin (time is always 23:59:59)
        private DateTime leftHandMarginDateTime = DateTime.MinValue;
        private DateTime rightHandMarginDateTime = DateTime.MinValue;

        // these are the scatter plots, they can be null
        private ScatterPlot scatterPlot1 = null;
        private string scatterPlot1Title = "";
        private ScatterPlot scatterPlot2 = null;
        private string scatterPlot2Title = "";
        private ScatterPlot scatterPlot3 = null;
        private string scatterPlot3Title = "";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// A form to display a plot of historical data from the database
        /// </summary>
        public frmPlotReport()
        {
            LogMessage("frmPlotReport Starts");
            InitializeComponent();

            SetupDataFrequencyComboBox();

            ConfigureThePlot();

            // by default set the startdate to today
            StartDate = DateTime.Now.Date.AddDays(-1);
            EndDate = DateTime.Now;

            // we do not permit dates in the future, but the end of the day today is ok
            dateTimePickerStartDate.MaxDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
            dateTimePickerEndDate.MaxDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
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

            Plot.XAxis.TickLabelFormat("MM-dd\nHH:mm", dateTimeFormat: true);
            // add padding to the right to prevent long dates from flowing off the figure
            Plot.YAxis2.SetSizeLimit(min: 40);

            // set up the color pallette
            Plot.Palette = new ScottPlot.Palettes.Category10();

            PlotTitle = DEFAULT_PLOT_TITLE;
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Rebuilds the title according to the plots on display
        /// </summary>
        /// <returns>z success, nz fail</returns>
        private void RebuildTitleBasedOnPlots()
        {
            StringBuilder sb = new StringBuilder();

            // are they all empty - just use the default
            if ((ScatterPlot1Title == "") && (ScatterPlot1Title == ""))
            {
                PlotTitle = DEFAULT_PLOT_TITLE;
                return;
            }
            // put on the first part
            sb.Append(DEFAULT_ACTIVEPLOT_TITLE_PREFIX);
            if (ScatterPlot1Title.Length != 0)
            {
                sb.Append(ScatterPlot1Title);
            }
            if (ScatterPlot2Title.Length != 0)
            {
                // add the spacer, if we need to
                if (sb.Length > DEFAULT_ACTIVEPLOT_TITLE_PREFIX.Length) sb.Append(", ");
                sb.Append(ScatterPlot2Title);
            }
            if (ScatterPlot3Title.Length != 0)
            {
                // add the spacer, if we need to
                if (sb.Length > DEFAULT_ACTIVEPLOT_TITLE_PREFIX.Length) sb.Append(", ");
                sb.Append(ScatterPlot3Title);
            }

            PlotTitle = sb.ToString();
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Builds the sql statement
        /// 
        /// </summary>
        /// <param name="fieldName">the replacement field name</param>
        /// <param name="deviceAlias">the picked device name</param>
        /// <returns>the built sql statement or null fail</returns>
        public string BuildTheSQLQuery(string deviceAlias, string fieldName)
        {
            if (deviceAlias == null) return null;
            if (deviceAlias.Length == 0) return null;
            if (fieldName == null) return null;
            if (fieldName.Length == 0) return null;

            // get the marker string
            string markerString = GetInterValMarkerStringForIntervalEnumVal((DataIntervalEnum)comboBoxDataInterval.SelectedItem);
            if (markerString == null) return null;
            if (markerString.Length == 0) return null;

            // now build the query
            string workingSQL = SQLQUERY;
            workingSQL = workingSQL.Replace(MARKER_REPSTR, markerString);
            workingSQL = workingSQL.Replace(DEVICEALIAS_REPSTR, deviceAlias);
            workingSQL = workingSQL.Replace(FIELDNAME_REPSTR, fieldName);
            

            // the table in the database is always the deviceAlias without the digits on the end
            // for those table that do not have this it does not matter.
            var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string tableRepStr = deviceAlias.TrimEnd(digits);
            if (tableRepStr == null) return null;
            if (tableRepStr.Length == 0) return null;
            workingSQL = workingSQL.Replace(TABLE_REPSTR, tableRepStr);

            // now the dates, the low date is forced to the start of the day, the end date to the end
            workingSQL = workingSQL.Replace(START_DATETIME_REPSTR, StartDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0).ToString("yyyy-MM-dd HH:mm:ss"));
            workingSQL = workingSQL.Replace(END_DATETIME_REPSTR, EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss"));

            //LogMessage("SqlQuery=" + workingSQL);
            return workingSQL;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Fetches the  data from the database. All DB Errors are trapped in here and logged
        /// </summary>
        /// <returns>the plot object or null for fail</returns>
        public ScatterPlot FetchPlotOfData(string deviceAlias, string fieldName)
        {
            ScatterPlot workingPlot = null;

            List<DateTime> dateTimeList = null;
            List<double> doubleList = null;

            if (DBManager == null)
            {
                LogMessage("FetchPlotOfData: DBManager == null");
                return null;
            }
            if (DBManager.ConnectionIsOpen() == false)
            {
                LogMessage("FetchPlotOfData: DBManager.ConnectionIsOpen == false");
                return null;
            }

            // check the device alias
            if (deviceAlias == null) return null;
            if (deviceAlias.Length == 0) return null;
            if (deviceAlias == KnownDeviceEnum.None.ToString()) return null;

            // check the field name
            if (fieldName == null) return null;
            if (fieldName.Length == 0) return null;
            if (fieldName == "None") return null;

            // initialize the SQL statement according to the screen settings
            string sqlToRun = BuildTheSQLQuery(deviceAlias, fieldName);
            if ((sqlToRun == null) || (sqlToRun.Length == 0))
            {
                LogMessage("FetchPlotOfData: sql query failed to build");
                OISMessageBox("There was an error please see the log file.");
                return null;
            }

            // try catch here
            try
            {
                DBManager.ReadData2Col_DateAndDouble(ref dateTimeList, ref doubleList, sqlToRun);
            }
            catch (Exception ex)
            {
                LogMessage("FetchData returned an error");
                LogMessage("FetchData query was>"+ sqlToRun+"<");
                LogMessage("Exception is >" + ex.Message);
                OISMessageBox("There was an error please see the log file.");
                return null;
            }

            // we now have to post process this. ScottPlot wants arrays of doubles.
            if (dateTimeList == null)
            {
                LogMessage("FetchData returned null dateTimeList");
                OISMessageBox("There was an error please see the log file.");
                return null;
            }
            if (doubleList == null)
            {
                LogMessage("FetchData returned null doubleList");
                OISMessageBox("There was an error please see the log file.");
                return null;
            }
            if (doubleList.Count != dateTimeList.Count)
            {
                LogMessage("FetchData returned lists counts unequal");
                OISMessageBox("There was an error please see the log file.");
                return null;
            }

            if(doubleList.Count==0)
            {
                OISMessageBox("There is no data to display");
                return null;
            }

            // build the output arrays
            double[] dateTimeData = new double[dateTimeList.Count];
            double[] doubleData = new double[doubleList.Count];

            // process each data item
            for (int i = 0; i < doubleList.Count; i++)
            {
                doubleData[i] = doubleList[i];
                dateTimeData[i] = dateTimeList[i].ToOADate();
            }

            // create the new scatter plot
            workingPlot = new ScatterPlot(dateTimeData, doubleData);
            workingPlot.LineWidth = DEFAULT_LINE_THICKNESS;
            workingPlot.MarkerSize = DEFAULT_MARKER_THICKNESS;
            return workingPlot;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the start date
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return dateTimePickerStartDate.Value;
            }
            set
            {
                dateTimePickerStartDate.Value = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the end date
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return dateTimePickerEndDate.Value;
            }
            set
            {
                dateTimePickerEndDate.Value = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Configures the plot object to a set of properties. The configuration
        /// is pretty specific to the type of property.
        /// 
        /// NOTE: Anything coming in off a monitor block goes in Plot1
        /// 
        /// </summary>
        /// <param name="propertiesIn">the MB properties to configure to</param>
        public void ConfigurePlotToMonitorBlock(MonitorBlockProperties_Base propertiesIn)
        {
            if (propertiesIn == null) return;
            if((propertiesIn is MonitorBlockProperties_Graph) == true)
            {
                string dataSource = (propertiesIn as MonitorBlockProperties_Graph).DataSource;
                if (dataSource == null) return;
                if (dataSource.Length == 0) return;

                // set the device alias and the field name comboboxes
                SetPlot1DeviceAndFieldFromDataSource(dataSource);

                // set the interval
                comboBoxDataInterval.SelectedItem = (propertiesIn as MonitorBlockProperties_Graph).DataInterval;

                // set the dates

                // the end date when coming off a monitor block is always today
                EndDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                // the start date is the end date minus the number of days
                int numDays = (propertiesIn as MonitorBlockProperties_Graph).DaysToDisplay;
                if (numDays <= 0) numDays = 1;
                // zero base it for the math
                numDays = numDays - 1;
                // set it to the start of the day numDays ago
                StartDate = EndDate.Date.AddDays(numDays * -1).AddHours(0).AddMinutes(0).AddSeconds(0);

                // we should be all set. Do the plot
                BuildAndSetPlot1AccordingToScreen();
                RebuildTitleBasedOnPlots();

            }
            else if ((propertiesIn is MonitorBlockProperties_Number) == true)
            {
                string dataSource = (propertiesIn as MonitorBlockProperties_Number).DataSource;
                if (dataSource == null) return;
                if (dataSource.Length == 0) return;

                // set the device alias and the field name comboboxes
                SetPlot1DeviceAndFieldFromDataSource(dataSource);
                // did we set it properly? 
                if(ctlDeviceAndFieldPicker1.DeviceAlias == KnownDeviceEnum.None.ToString())
                {
                    // no, user may be using a complex expression. We still might be able to plot
                    // this if they are using a user defined data reference

                    // Do we have a reference name for this and is it a reserved user data one
                    string refName = (propertiesIn as MonitorBlockProperties_Number).UserReference;
                    if ((refName != null) && (refName.StartsWith(PortStatus.USERDB_STORE_NUMBER_PREFIX) == true))
                    {
                        // doesn't hurt to try
                        ctlDeviceAndFieldPicker1.DeviceAlias = KnownDeviceEnum.USERDATA.ToString();
                        ctlDeviceAndFieldPicker1.FieldName = refName.ToString();
                    }
                }

                // set the interval, just assume 5 Min, number MB's do not have this field-
                comboBoxDataInterval.SelectedItem = DataIntervalEnum.FIVE_MIN;

                // set the dates

                // the end date when coming off a monitor block is always today
                EndDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                // the start date is the start of the day
                int numDays = 1;
                if (numDays <= 0) numDays = 1;
                // zero base it for the math
                numDays = numDays - 1;
                // set it to the start of the day numDays ago
                StartDate = EndDate.Date.AddDays(numDays * -1).AddHours(0).AddMinutes(0).AddSeconds(0);

                // we should be all set. Do the plot
                BuildAndSetPlot1AccordingToScreen();
                RebuildTitleBasedOnPlots();

            }
            else
            {
                // don't know what to do
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Sets the deviceAlias and field name from a datasource string
        /// </summary>
        private void SetPlot1DeviceAndFieldFromDataSource(string dataSource)
        {
            if (dataSource == null) return;
            if (dataSource.Length == 0) return;

            string deviceAlias = "";
            string fieldName = "";
            // get the device alias and the field name
            int retInt = OutbackSystem.GetDeviceNameAndFieldFromDataSource(dataSource, out deviceAlias, out fieldName);
            if (retInt != 0) return;
            if (deviceAlias == null) return;
            if (deviceAlias.Length == 0) return;
            if (fieldName == null) return;
            if (fieldName.Length == 0) return;

            // set our pickers
            ctlDeviceAndFieldPicker1.DeviceAlias = deviceAlias; // must be done before the field name
            ctlDeviceAndFieldPicker1.FieldName = fieldName;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The DB Manager is stored down in the logger singleton instance at startup.
        /// There is only ever one of these. There is no set here.
        /// </summary>
        public DBManager DBManager
        {
            get
            {
                return (DBManager)g_Logger.g_dbManager;
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
        /// The plot title
        /// </summary>
        public string PlotTitle
        {
            set
            {
                Plot.Title(value);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets ScatterPlot1. Can be null
        /// </summary>
        public ScatterPlot ScatterPlot1 { get => scatterPlot1; set => scatterPlot1 = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets ScatterPlot2. Can be null
        /// </summary>
        public ScatterPlot ScatterPlot2 { get => scatterPlot2; set => scatterPlot2 = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets ScatterPlot2. Can be null
        /// </summary>
        public ScatterPlot ScatterPlot3 { get => scatterPlot3; set => scatterPlot3 = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Configures the datafrequency enum
        /// </summary>
        private void SetupDataFrequencyComboBox()
        {
            comboBoxDataInterval.DataSource = Enum.GetValues(typeof(DataIntervalEnum));
            comboBoxDataInterval.SelectedItem = DataIntervalEnum.FIVE_MIN;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the ScatterPlot1Title. Will never get/set null
        /// </summary>
        public string ScatterPlot1Title
        {
            get
            {
                if (scatterPlot1Title == null) scatterPlot1Title = "";
                return scatterPlot1Title;
            }
            set
            {
                scatterPlot1Title = value;
                if (scatterPlot1Title == null) scatterPlot1Title = "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the ScatterPlot2Title. Will never get/set null
        /// </summary>
        public string ScatterPlot2Title
        {
            get
            {
                if (scatterPlot2Title == null) scatterPlot2Title = "";
                return scatterPlot2Title;
            }
            set
            {
                scatterPlot2Title = value;
                if (scatterPlot2Title == null) scatterPlot2Title = "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the ScatterPlot3Title. Will never get/set null
        /// </summary>
        public string ScatterPlot3Title
        {
            get
            {
                if (scatterPlot3Title == null) scatterPlot3Title = "";
                return scatterPlot3Title;
            }
            set
            {
                scatterPlot3Title = value;
                if (scatterPlot3Title == null) scatterPlot3Title = "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the interval marker string for a specified interval enum
        /// </summary>
        /// <param name="valIn">the value we are getting the marker for</param>
        /// <returns>the marker string or FIVE_MINUTE_MARKER_AS_STRING for default</returns>
        private string GetInterValMarkerStringForIntervalEnumVal(DataIntervalEnum valIn)
        {
            switch (valIn)
            {
                case DataIntervalEnum.FIVE_MIN:
                    return FIVE_MINUTE_MARKER_AS_STRING;
                case DataIntervalEnum.TEN_MIN:
                    return TEN_MINUTE_MARKER_AS_STRING;
                case DataIntervalEnum.FIFTEEN_MIN:
                    return FIFTEEN_MINUTE_MARKER_AS_STRING;
                case DataIntervalEnum.THIRTY_MIN:
                    return THIRTY_MINUTE_MARKER_AS_STRING;
                case DataIntervalEnum.ONE_HOUR:
                    return ONE_HOUR_MARKER_AS_STRING;
                case DataIntervalEnum.ONE_DAY:
                    return ONE_DAY_MARKER_AS_STRING;
                default:
                    return FIVE_MINUTE_MARKER_AS_STRING;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the close button
        /// </summary>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            LogMessage("frmPlotReport buttonClose_Click Called");
            this.Close();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Reset the plot display
        /// </summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            // just calling the axis auto seems to be enough to get
            // everything back in frame
            Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Builds the plot1 data according to the screen controls. Will remove the 
        /// old plot1.
        /// </summary>
        private void BuildAndSetPlot1AccordingToScreen()
        {
            try
            {
                // we remove the existing plot, if we have one
                if (ScatterPlot1 != null)
                {
                    Plot.Remove(ScatterPlot1);
                    ScatterPlot1Title = "";
                    ScatterPlot1 = null;
                }

                // get the device alias
                string deviceAlias = ctlDeviceAndFieldPicker1.DeviceAlias;
                if (deviceAlias == null) return;
                if (deviceAlias.Length == 0) return;
                if (deviceAlias == KnownDeviceEnum.None.ToString()) return;

                // get the field name
                string fieldName = ctlDeviceAndFieldPicker1.FieldName;
                if (fieldName == null) return;
                if (fieldName.Length == 0) return;
                if (fieldName == "None") return;

                ScatterPlot workingPlot = FetchPlotOfData(deviceAlias, fieldName);
                if (workingPlot == null) return;
                // set the plot color from the pallette
                workingPlot.Color = Plot.Palette.GetColor(0);
                // Just use the field name for the title
                ScatterPlot1Title = fieldName;
                ScatterPlot1 = workingPlot;

                Plot.Add(scatterPlot1);
            }
            finally
            {
                Plot.AxisAuto();
                formsPlot1.Refresh();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Builds the plot2 data according to the screen controls. Will remove the 
        /// old plot1.
        /// </summary>
        private void BuildAndSetPlot2AccordingToScreen()
        {
            try
            {
                // we remove the existing plot, if we have one
                if (ScatterPlot2 != null)
                {
                    Plot.Remove(ScatterPlot2);
                    ScatterPlot2Title = "";
                    ScatterPlot2 = null;
                }

                // get the device alias
                string deviceAlias = ctlDeviceAndFieldPicker2.DeviceAlias;
                if (deviceAlias == null) return;
                if (deviceAlias.Length == 0) return;
                if (deviceAlias == KnownDeviceEnum.None.ToString()) return;

                // get the field name
                string fieldName = ctlDeviceAndFieldPicker2.FieldName;
                if (fieldName == null) return;
                if (fieldName.Length == 0) return;
                if (fieldName == "None") return;

                ScatterPlot workingPlot = FetchPlotOfData(deviceAlias, fieldName);
                if (workingPlot == null) return;
                // set the plot color from the pallette
                workingPlot.Color = Plot.Palette.GetColor(1);
                // Just use the field name for the title
                ScatterPlot2Title = fieldName;
                ScatterPlot2 = workingPlot;

                Plot.Add(scatterPlot2);
            }
            finally
            {
                Plot.AxisAuto();
                formsPlot1.Refresh();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Builds the plot3 data according to the screen controls. Will remove the 
        /// old plot1.
        /// </summary>
        private void BuildAndSetPlot3AccordingToScreen()
        {
            try
            {
                // we remove the existing plot, if we have one
                if (ScatterPlot3 != null)
                {
                    Plot.Remove(ScatterPlot3);
                    ScatterPlot3Title = "";
                    ScatterPlot3 = null;
                }

                // get the device alias
                string deviceAlias = ctlDeviceAndFieldPicker3.DeviceAlias;
                if (deviceAlias == null) return;
                if (deviceAlias.Length == 0) return;
                if (deviceAlias == KnownDeviceEnum.None.ToString()) return;

                // get the field name
                string fieldName = ctlDeviceAndFieldPicker3.FieldName;
                if (fieldName == null) return;
                if (fieldName.Length == 0) return;
                if (fieldName == "None") return;

                ScatterPlot workingPlot = FetchPlotOfData(deviceAlias, fieldName);
                if (workingPlot == null) return;
                // set the plot color from the pallette
                workingPlot.Color = Plot.Palette.GetColor(2);
                // Just use the field name for the title
                ScatterPlot3Title = fieldName;
                ScatterPlot3 = workingPlot;

                Plot.Add(scatterPlot3);
            }
            finally
            {
                Plot.AxisAuto();
                formsPlot1.Refresh();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Refresh all known plots
        /// </summary>
        private void RebuildAllKnownPlots()
        {
            // check each one and simulate a button press
            if (ScatterPlot1 != null) buttonPlot1Refresh_Click(this, new EventArgs());
            if (ScatterPlot2 != null) buttonPlot2Refresh_Click(this, new EventArgs());
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Refresh plot 1
        /// </summary>
        private void buttonPlot1Refresh_Click(object sender, EventArgs e)
        {
            // build it and set it
            BuildAndSetPlot1AccordingToScreen();
            RebuildTitleBasedOnPlots();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Refresh plot 2
        /// </summary>
        private void buttonPlot2Refresh_Click(object sender, EventArgs e)
        {
            // build it and set it
            BuildAndSetPlot2AccordingToScreen();
            RebuildTitleBasedOnPlots();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Refresh plot 3
        /// </summary>
        private void buttonPlot3Refresh_Click(object sender, EventArgs e)
        {
            // build it and set it
            BuildAndSetPlot3AccordingToScreen();
            RebuildTitleBasedOnPlots();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clears scatter plot 1 from the display
        /// </summary>
        private void buttonPlot1Clear_Click(object sender, EventArgs e)
        {
            // we remove the existing plot, if we have one
            if (ScatterPlot1 != null)
            {
                Plot.Remove(ScatterPlot1);
                ScatterPlot1Title = "";
            }
            RebuildTitleBasedOnPlots();
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clears scatter plot 2 from the display
        /// </summary>
        private void buttonPlot2Clear_Click(object sender, EventArgs e)
        {
            // we remove the existing plot, if we have one
            if (ScatterPlot2 != null)
            {
                Plot.Remove(ScatterPlot2);
                ScatterPlot2Title = "";
            }
            RebuildTitleBasedOnPlots();
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clears scatter plot 3 from the display
        /// </summary>
        private void buttonPlot3Clear_Click(object sender, EventArgs e)
        {
            // we remove the existing plot, if we have one
            if (ScatterPlot3 != null)
            {
                Plot.Remove(ScatterPlot3);
                ScatterPlot3Title = "";
            }
            RebuildTitleBasedOnPlots();
            formsPlot1.Refresh();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle date duration change. Date on all plots must be the same
        /// </summary>
        private void dateTimePickerStartDate_ValueChanged(object sender, EventArgs e)
        {
            RebuildAllKnownPlots();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle date duration change. Date on all plots must be the same
        /// </summary>
        private void dateTimePickerEndDate_ValueChanged(object sender, EventArgs e)
        {
            RebuildAllKnownPlots();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle data interval change. Data interval on all plots must be the same
        /// </summary>
        private void comboBoxDataInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            RebuildAllKnownPlots();
        }

     }
}

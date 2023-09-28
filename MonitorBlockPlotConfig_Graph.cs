using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    // Some Notes: The graphing tool used is called ScottPlot - is very quick and quite
    // versatile. There are many types of graph - the one we use is the type called Scatter
    // Plot. It is not the fastest of the available types but it does the autoscaling and 
    // formatting very well.
    //
    // Every time we need to refresh the plot we just rebuild it by re-quering the DB


    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// Contains a configuration for a Monitor Block graph
    /// </summary>
    public class MonitorBlockPlotConfig_Graph : OISObjBase
    {
        private MonitorBlockProperties_Graph mbProperties = new MonitorBlockProperties_Graph();

        private const int DEFAULT_LINE_THICKNESS = 1;
        private const int DEFAULT_MARKER_THICKNESS = 1;

        public const int MAX_DAYLINES_ON_PLOT = 7;

        private const string FIVE_MINUTE_MARKER_AS_STRING = "FiveMinuteMarker";
        private const string TEN_MINUTE_MARKER_AS_STRING = "TenMinuteMarker";
        private const string FIFTEEN_MINUTE_MARKER_AS_STRING = "FifteenMinuteMarker";
        private const string THIRTY_MINUTE_MARKER_AS_STRING = "ThirtyMinuteMarker";
        private const string ONE_HOUR_MARKER_AS_STRING = "HourMarker";
        private const string ONE_DAY_MARKER_AS_STRING = "DayMarker";

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

        private const DataIntervalEnum DEFAULT_DATA_INTERVAL = DataIntervalEnum.FIVE_MIN;
        private DataIntervalEnum dataInterval = DEFAULT_DATA_INTERVAL;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockPlotConfig_Graph()
        {
            RightHandMarginDateTime = DateTime.MinValue;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Initializes the plot config.  This is what sets the Plot properties
        /// 
        /// </summary>
        /// <param name="mbPropertiesIn">the MB properties behind the graph</param>
        public void InitThePlotConfiguration()
        {
            // sanity check, days to display can never be < zero
            if (MBProperties.DaysToDisplay <= 0) MBProperties.DaysToDisplay = 1;

            // make sure we know what datetime the right hand margin references
            SetMarginDateTimesAccordingToProperties();
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
            string markerString = GetInterValMarkerStringForIntervalEnumVal(DataInterval);
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
            workingSQL = workingSQL.Replace(START_DATETIME_REPSTR, LeftHandMarginDateTime.Date.AddHours(0).AddMinutes(0).AddSeconds(0).ToString("yyyy-MM-dd HH:mm:ss"));
            workingSQL = workingSQL.Replace(END_DATETIME_REPSTR, RightHandMarginDateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss"));

            //LogMessage("SqlQuery=" + workingSQL);
            return workingSQL;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Sets the left and right margin dates according to the properties. Assumes
        /// the properties have been set.
        /// 
        /// </summary>
        public void SetMarginDateTimesAccordingToProperties()
        {
            // right hand margin is always the end of today
            rightHandMarginDateTime = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            // the left hand property is always the EOD today minus the days to display. Note that
            // since the time on the righthandmargin is 23:59:59 subtracting 1 day sets the day down
            // leaves the day the same but sets the time to 00:00:00. Subtracting 2 days drops 1 day
            // and sets the time to 00:00:00
            leftHandMarginDateTime = rightHandMarginDateTime.Date.AddDays((MBProperties.DaysToDisplay-1) * -1);
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
                LogMessage("FetchData query was>" + sqlToRun + "<");
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

            if (doubleList.Count == 0)
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
        /// Gets/Sets the monitor block properties behind this graph. Never gets/sets
        /// null
        /// </summary>
        public MonitorBlockProperties_Graph MBProperties
        {
            get
            {
                if (mbProperties == null) mbProperties = new MonitorBlockProperties_Graph();
                return mbProperties;
            }
            set
            {
                mbProperties = value;
                if (mbProperties == null) mbProperties = new MonitorBlockProperties_Graph();
            }
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

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The last outback is stored down in the logger singleton instance.
        /// There is only ever one of these. There is no set here.
        /// </summary>
        public OutbackSystem OutbackSystemReport
        {
            get
            {
                return (OutbackSystem)g_Logger.g_DataContainer;
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
        /// Gets/Sets the data interval information. This is a function of the number of days we are 
        /// viewing
        /// </summary>
        public DataIntervalEnum DataInterval { get => dataInterval; set => dataInterval = value; }

        public DateTime RightHandMarginDateTime { get => rightHandMarginDateTime; set => rightHandMarginDateTime = value; }
        public DateTime LeftHandMarginDateTime { get => leftHandMarginDateTime; set => leftHandMarginDateTime = value; }

    }
}

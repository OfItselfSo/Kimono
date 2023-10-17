using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

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
    /// <summary>
    /// A class to provide properties for the ctlMonitorBlock_MinMax class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_MinMax))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [KnownType(typeof(DoubleRecord))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_MinMax : MonitorBlockProperties_Base
    {

        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX + " MinMax";

        // non browsable attributes

        // the list of max values we have seen. Will be one for each day
        List<DoubleRecord> recordListMax = new List<DoubleRecord>();
        // the list of min values we have seen. Will be one for each day
        List<DoubleRecord> recordListMin = new List<DoubleRecord>();

        // this is the current days Max record, can be null if never set
        DoubleRecord todaysRecordMax = null;
        // this is the current days Min record, can be null if never set
        DoubleRecord todaysRecordMin = null;

        // browsable attributes
        private string titleText = "Title Goes Here";
        private string dataSource = "Device#Field";
        private const int DEFAULT_DAYS_TO_DISPLAY = 1;
        private int daysToDisplay = DEFAULT_DAYS_TO_DISPLAY;
        private string suffix = "";
        private string prefix = "";
        private int decimalPlaces = 2;


        private bool resetAtMidnite = true;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_MinMax()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;
            SummaryString = "MinMax Monitor Block";

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the maximum value we have seen
        /// </summary>
        /// <returns>the First DoubleRecord with a MaxValue it finds or null for fail</returns>
        public DoubleRecord GetMaxValue()
        {
            DoubleRecord currentRec = null;
            foreach (DoubleRecord dRec in RecordListMax)
            {
                if(currentRec==null) currentRec = dRec;
                else if (dRec.DoubleValue > currentRec.DoubleValue) currentRec = dRec;
            }
            // return what we got or null
            return currentRec;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the minimum value we have seen
        /// </summary>
        /// <returns>the First DoubleRecord with a MinValue it finds or null for fail</returns>
        public DoubleRecord GetMinValue()
        {
            DoubleRecord currentRec = null;
            foreach (DoubleRecord dRec in RecordListMin)
            {
                if (currentRec == null) currentRec = dRec;
                else if (dRec.DoubleValue < currentRec.DoubleValue) currentRec = dRec;
            }
            // return what we got or null
            return currentRec;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clears older records from the max list
        /// </summary>
        /// <param name="numDaysToKeep">remove all records older than this number of days</param>
        public void ClearOldRecordsMax(int numDaysToKeep)
        {
            // note: we go in reverse so the index do not change under us
            for (int i = RecordListMax.Count - 1; i >= 0; i--)
            {
                if ((DateTime.Now.Date - RecordListMax[i].DateTimeValue.Date).Days < numDaysToKeep) continue;
                // too old, remove it
                RecordListMax.RemoveAt(i);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Clears older records from the min list
        /// </summary>
        /// <param name="numDaysToKeep">remove all records older than this number of days</param>
        public void ClearOldRecordsMin(int numDaysToKeep)
        {
            // note: we go in reverse so the index do not change under us
            for (int i = RecordListMin.Count - 1; i >= 0; i--)
            {
                if ((DateTime.Now.Date - RecordListMin[i].DateTimeValue.Date).Days < numDaysToKeep) continue;
                // too old, remove it
                RecordListMin.RemoveAt(i);
            }
        }

        #region NONBROWSABLE

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Sets the incoming value
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public double LastValue
        {
            set
            {
                // if we do not have one then create one. 
                if (TodaysRecordMax == null)
                {
                    TodaysRecordMax = new DoubleRecord(value, DateTime.Now);
                    // add it
                    RecordListMax.Add(TodaysRecordMax);
                }

                if (TodaysRecordMin == null)
                {
                    TodaysRecordMin = new DoubleRecord(value, DateTime.Now);
                    // add it
                    RecordListMin.Add(TodaysRecordMin);
                }

                // just set the max
                if (value > TodaysRecordMax.DoubleValue)
                {
                    todaysRecordMax.DoubleValue = value;
                    todaysRecordMax.DateTimeValue = DateTime.Now;
                }
                // just set the min
                if (value < TodaysRecordMin.DoubleValue)
                {
                    todaysRecordMin.DoubleValue = value;
                    todaysRecordMin.DateTimeValue = DateTime.Now;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Manually add a value 
        /// </summary>
        /// <param name="valueIn">the value to add</param>
        /// <param name="dateTimeIn">the date time to add it</param>
        public void ManualAddValue(double valueIn, DateTime dateTimeIn)
        {
            // if we do not have one then create one. 
            if (TodaysRecordMax == null)
            {
                TodaysRecordMax = new DoubleRecord(valueIn, dateTimeIn);
                // add it
                RecordListMax.Add(TodaysRecordMax);
            }

            if (TodaysRecordMin == null)
            {
                TodaysRecordMin = new DoubleRecord(valueIn, dateTimeIn);
                // add it
                RecordListMin.Add(TodaysRecordMin);
            }

            // just set the max
            if (valueIn > TodaysRecordMax.DoubleValue)
            {
                todaysRecordMax.DoubleValue = valueIn;
                todaysRecordMax.DateTimeValue = dateTimeIn;
            }
            // just set the min
            if (valueIn < TodaysRecordMin.DoubleValue)
            {
                todaysRecordMin.DoubleValue = valueIn;
                todaysRecordMin.DateTimeValue = dateTimeIn;
            }

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The list of max value records we are maintaining. Will never get/set null
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public List<DoubleRecord> RecordListMax 
        {
            get
            {
                if (recordListMax == null) recordListMax = new List<DoubleRecord>();
                return recordListMax;
            }
            set 
            { 
                recordListMax = value;
                if (recordListMax == null) recordListMax = new List<DoubleRecord>();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The list of min value records we are maintaining. Will never get/set null
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public List<DoubleRecord> RecordListMin
        {
            get
            {
                if (recordListMin == null) recordListMin = new List<DoubleRecord>();
                return recordListMin;
            }
            set
            {
                recordListMin = value;
                if (recordListMin == null) recordListMin = new List<DoubleRecord>();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Todays max record. Changes over at midnight. Can return null
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public DoubleRecord TodaysRecordMax { get => todaysRecordMax; set => todaysRecordMax = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Todays min record. Changes over at midnight. Can return null
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public DoubleRecord TodaysRecordMin { get => todaysRecordMin; set => todaysRecordMin = value; }

        #endregion


        #region BROWSABLE

        [DataMember]
        [DefaultValueAttribute("Device#Field")]
        [CategoryAttribute("Data"), DescriptionAttribute("The device and field name which provides the source of the data. Can be a complex expression.")]
        public string DataSource { get => dataSource; set => dataSource = value; }

        [DataMember]
        [DefaultValueAttribute("Title Goes Here")]
        [CategoryAttribute("Display"), DescriptionAttribute("Title of the Monitor Block. This is what you see at the top of the Monitor Block")]
        public string TitleText { get => titleText; set => titleText = value; }

        [DataMember]
        [DefaultValueAttribute(true)]
        [CategoryAttribute("Data"), DescriptionAttribute("If true the testing resets at midnite. If false it just keeps testing.")]
        public bool ResetAtMidnite { get => resetAtMidnite; set => resetAtMidnite = value; }

        [DataMember]
        [DefaultValueAttribute(1)]
        [CategoryAttribute("Data"), DescriptionAttribute("The number of days of data to track the Max/Min value over.")]
        public int DaysToDisplay { get => daysToDisplay; set => daysToDisplay = value; }

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Display"), DescriptionAttribute("A text suffix for the screen value. If present it will be appended to the value on display.")]
        public string Suffix { get => suffix; set => suffix = value; }

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Display"), DescriptionAttribute("A text suffix for the screen value. If present it will be prepended to the value on display.")]
        public string Prefix { get => prefix; set => prefix = value; }

        [DataMember]
        [DefaultValueAttribute(2)]
        [CategoryAttribute("Display"), DescriptionAttribute("The number of decimal places to use for the value on display.")]
        public int DecimalPlaces { get => decimalPlaces; set => decimalPlaces = value; }


        #endregion

    }
}

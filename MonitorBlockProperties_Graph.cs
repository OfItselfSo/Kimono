using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

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
    /// A class to provide properties for the ctlMonitorBlock_Graph class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Graph))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Graph : MonitorBlockProperties_Base
    {
        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX + " Graph";

        // non browsable attributes
        private double graphValue = 1;
        private DateTime graphDateTime = DateTime.MinValue;

        // browsable attributes
        private string titleText = "Title Goes Here";
        private string dataSource = "Device#Field";

        private const int DEFAULT_DAYS_TO_DISPLAY = 1;
        private int daysToDisplay = DEFAULT_DAYS_TO_DISPLAY;

        private DataIntervalEnum dataInterval = DataIntervalEnum.FIVE_MIN;

        public const DataDateEndpontEnum DEFAULT_DATA_DATE_ENDPOINT = DataDateEndpontEnum.FIXED_EOD;
        private DataDateEndpontEnum dataDateEndPoint = DEFAULT_DATA_DATE_ENDPOINT;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_Graph()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;
            SummaryString = "Graphing Monitor Block";

        }

        #region NONBROWSABLE
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public double GraphValue { get => graphValue; set => graphValue = value; }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public DateTime GraphDateTime { get => graphDateTime; set => graphDateTime = value; }
        #endregion


        #region BROWSABLE

        [DataMember]
        [DefaultValueAttribute("Device#Field")]
        [CategoryAttribute("Data"), DescriptionAttribute("The device and field name which provides the source of the data. Cannot be a complex expression.")]
        public string DataSource { get => dataSource; set => dataSource = value; }

        [DataMember]
        [DefaultValueAttribute("Title Goes Here")]
        [CategoryAttribute("Display"), DescriptionAttribute("Title of the Monitor Block")]
        public string TitleText { get => titleText; set => titleText = value; }

        [DataMember]
        [DefaultValueAttribute(1)]
        [CategoryAttribute("Data"), DescriptionAttribute("The number of days of data to display. If you display more 3-4 days it is probably best to reduce the DataInterval.")]
        public int DaysToDisplay { get => daysToDisplay; set => daysToDisplay = value; }

        [DataMember]
        [DefaultValueAttribute(DataIntervalEnum.FIVE_MIN)]
        [CategoryAttribute("Data"), DescriptionAttribute("The sampling interval for the graph. Take care not to pull more data than you need - it slows things down.")]
        public DataIntervalEnum DataInterval { get => dataInterval; set => dataInterval = value; }

        [DataMember]
        [DefaultValueAttribute(DataDateEndpontEnum.FIXED_EOD)]
        [CategoryAttribute("Data"), DescriptionAttribute("The type of endpoint for the graph. ROLLING places the current date+time at the right margin, EOD places midnight at the right margin.")]
        public DataDateEndpontEnum DataDateEndPoint { get => dataDateEndPoint; set => dataDateEndPoint = value; }

        #endregion

    }
}

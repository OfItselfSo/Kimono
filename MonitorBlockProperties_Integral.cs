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
    /// A class to provide properties for the ctlMonitorBlock_Integral class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Integral))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Integral : MonitorBlockProperties_Base
    {

        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX + " Integral";

        // non browsable attributes
        private double numberValue = 1;
        // if false we had an error parsing this
        private bool numberValueIsValid = true;

        // this is the collected amounts we have found in valueIn*seconds
        private double accumulator = 0;
        private DateTime lastRecordedDateTime = DateTime.MaxValue;
        private bool resetAtMidnite = true;
        private DateTime lastResetTime = DateTime.Now;
        // we store the last display value as a double before converting it to a string for actual display
        private double lastDisplayValue = 0;


        // browsable attributes
        private int decimalPlaces = 6;
        private string titleText = "Title Goes Here";
        private string suffix = "";
        private string prefix = "";
        private string dataSource = "Device#Field";
        private string userReference = "";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_Integral()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;
            SummaryString = "Integral Monitor Block";

        }

        #region NONBROWSABLE

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public double NumberValue { get => numberValue; set => numberValue = value; }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool NumberValueIsValid { get => numberValueIsValid; set => numberValueIsValid = value; }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public double Accumulator { get => accumulator; set => accumulator = value; }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public DateTime LastRecordedDateTime { get => lastRecordedDateTime; set => lastRecordedDateTime = value; }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public DateTime LastResetTime { get => lastResetTime; set => lastResetTime = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the last display value as a double. This can be the results
        /// of calculations on the Accumulator and NumberValue. This is essentially
        /// what is on the screen without all the prefix and suffix stuff
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public double LastDisplayValue { get => lastDisplayValue; set => lastDisplayValue = value; }

        #endregion


        #region BROWSABLE

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Data"), DescriptionAttribute("A text based reference name. Should be only one word. Certain reserved reference names also store the data in the database.")]
        public string UserReference { get => userReference; set => userReference = value; }

        [DataMember]
        [DefaultValueAttribute("Device#Field")]
        [CategoryAttribute("Data"), DescriptionAttribute("The device and field name which provides the source of the data. Can be a complex expression.")]
        public string DataSource { get => dataSource; set => dataSource = value; }

        [DataMember]
        [DefaultValueAttribute(true)]
        [CategoryAttribute("Data"), DescriptionAttribute("If true the integration value will reset to zero at midnite.")]
        public int DecimalPlaces { get => decimalPlaces; set => decimalPlaces = value; }

        [DataMember]
        [DefaultValueAttribute(2)]
        [CategoryAttribute("Data"), DescriptionAttribute("The number of decimal places to use for the value on display.")]
        public bool ResetAtMidnite { get => resetAtMidnite; set => resetAtMidnite = value; }

        [DataMember]
        [DefaultValueAttribute("Title Goes Here")]
        [CategoryAttribute("Display"), DescriptionAttribute("Title of the Monitor Block. This is what you see at the top of the Monitor Block")]
        public string TitleText { get => titleText; set => titleText = value; }

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Display"), DescriptionAttribute("A text suffix for the screen value. If present it will be appended to the value on display.")]
        public string Suffix { get => suffix; set => suffix = value; }

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Display"), DescriptionAttribute("A text suffix for the screen value. If present it will be prepended to the value on display.")]
        public string Prefix { get => prefix; set => prefix = value; }

        #endregion

    }
}

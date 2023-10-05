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
    /// A class to provide properties for the ctlMonitorBlock_Latch class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Latch))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Latch : MonitorBlockProperties_Base
    {

        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX + " Latch";

        // non browsable attributes
        private object lastValue = 1;
        // if false we had an error parsing this
        private bool lastValueIsValid = true;
        private DateTime lastRecordedDateTime = DateTime.MinValue;

        // browsable attributes
        private string titleText = "Title Goes Here";
        private string dataSource = "Device#Field";
        private string comparisonValue = "27.2";
        private string userReference = "";

        private bool resetAtMidnite = true;
        private MB_TriggerDirectionEnum triggerDirection = MB_TriggerDirectionEnum.NONE;
        private MB_TriggerActionEnum triggerAction = MB_TriggerActionEnum.RECORD_FIRST;

        private string displayTextNotTriggered = "False";
        private string displayTextTriggered = "True";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_Latch()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;
            SummaryString = "Latch Monitor Block";

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detects if we have ever triggered
        /// </summary>
        /// <returns>true - we have triggered at least once, false - we have not</returns>
        public bool HasEverTriggered()
        {
            if (LastRecordedDateTime == DateTime.MinValue) return false;
            return true;
        }

        #region NONBROWSABLE

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public object LastValue { get => lastValue; set => lastValue = value; }
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool LastValueIsValid { get => lastValueIsValid; set => lastValueIsValid = value; }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public DateTime LastRecordedDateTime { get => lastRecordedDateTime; set => lastRecordedDateTime = value; }

        #endregion


        #region BROWSABLE

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Data"), DescriptionAttribute("A text based reference name. Should be only one word. Certain reserved reference names also store the data in the database.")]
        public string UserReference { get => userReference; set => userReference = value; }

        [DataMember]
        [DefaultValueAttribute("27.2")]
        [CategoryAttribute("Data"), DescriptionAttribute("The value against which we compare the incoming data presented by the DataSource.")]
        public string ComparisonValue { get => comparisonValue; set => comparisonValue = value; }

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
        [DefaultValueAttribute(MB_TriggerDirectionEnum.NONE)]
        [CategoryAttribute("Data"), DescriptionAttribute("Determines the trigger direction for the tests. None disables the monitor block.")]
        public MB_TriggerDirectionEnum TriggerDirection { get => triggerDirection; set => triggerDirection = value; }

        [DataMember]
        [DefaultValueAttribute(MB_TriggerActionEnum.RECORD_FIRST)]
        [CategoryAttribute("Data"), DescriptionAttribute("Determines the action when triggering.")]
        public MB_TriggerActionEnum TriggerAction { get => triggerAction; set => triggerAction = value; }

        [DataMember]
        [DefaultValueAttribute("False")]
        [CategoryAttribute("Display"), DescriptionAttribute("The text displayed if the trigger event has not occurred.")]
        public string DisplayTextNotTriggered { get => displayTextNotTriggered; set => displayTextNotTriggered = value; }


        [DataMember]
        [DefaultValueAttribute("True")]
        [CategoryAttribute("Display"), DescriptionAttribute("The text displayed if the trigger event has occurred.")]
        public string DisplayTextTriggered { get => displayTextTriggered; set => displayTextTriggered = value; }


        #endregion

    }
}

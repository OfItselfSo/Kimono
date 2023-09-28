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
    /// A class to provide properties for the ctlMonitorBlock_Number class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Number))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Number : MonitorBlockProperties_Base
    {

        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX + " Number";

        // non browsable attributes
        private double numberValue = 1;
        // if false we had an error parsing this
        private bool numberValueIsValid = true;

        // browsable attributes
        private string titleText = "Title Goes Here";
        private string suffix = "";
        private string prefix = "";
        private string dataSource = "Device#Field";
        private string userReference = "";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_Number()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;
            SummaryString = "Number Monitor Block";

        }

        #region NONBROWSABLE

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public double NumberValue { get => numberValue; set => numberValue = value; }
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool NumberValueIsValid { get => numberValueIsValid; set => numberValueIsValid = value; }

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

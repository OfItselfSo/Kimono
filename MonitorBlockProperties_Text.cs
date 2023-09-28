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
    /// A class to provide properties for the ctlMonitorBlock_Text class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Text))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Text : MonitorBlockProperties_Base
    {
        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX + " Text";

        // non browsable attributes
        private string textValue = "";

        // browsable attributes
        private string titleText = "Title Goes Here";
        private string suffix = "";
        private string prefix = "";
        private string dataSource = "Device#Field";

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_Text()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;
            SummaryString = "Text Monitor Block";

        }

        #region NONBROWSABLE
        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the textValue property will never get or set null. Will return 
        /// empty
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public string TextValue 
        { 
            get 
            {
                if (textValue == null) textValue = "";
                return textValue; 
            }

            set
            {
                textValue = value;
                if (textValue == null) textValue = "";
            }
        }
        #endregion


        #region BROWSABLE

        [DataMember]
        [DefaultValueAttribute("Device#Field")]
        [CategoryAttribute("Data"), DescriptionAttribute("The device and field name which provides the source of the data.")]
        public string DataSource { get => dataSource; set => dataSource = value; }

        [DataMember]
        [DefaultValueAttribute("Title Goes Here")]
        [CategoryAttribute("Display"), DescriptionAttribute("Title of the Monitor Block. This is what you see at the top of the Monitor Block.")]
        public string TitleText { get => titleText; set => titleText = value; }

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Display"), DescriptionAttribute("Suffix for the screen value")]
        public string Suffix { get => suffix; set => suffix = value; }

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Display"), DescriptionAttribute("Prefix for the screen value")]
        public string Prefix { get => prefix; set => prefix = value; }


        #endregion

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Deep clone this object, we just use a quick xml serialize/deserialize
        /// this does not happen often
        /// </summary>
        public MonitorBlockProperties_Text DeepClone(MonitorBlockProperties_Text obj)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(typeof(MonitorBlockProperties_Text));
                xs.Serialize(ms, obj);
                ms.Position = 0;

                return (MonitorBlockProperties_Text)xs.Deserialize(ms);
            }
        }
    }
}

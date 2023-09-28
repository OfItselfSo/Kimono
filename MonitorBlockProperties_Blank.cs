using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

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
    /// A class to provide properties for the ctlMonitorBlock_Blank class. An
    /// object derived from MonitorBlockProperties_Base serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Blank))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Blank : MonitorBlockProperties_Base
    {
        public const string MB_DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX+" Blank";
        
        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorBlockProperties_Blank()
        {
            // set the display name
            DisplayName = MB_DEFAULT_DISPLAY_NAME;

            SummaryString = "Blank Monitor Block";
        }

        // non browsable attributes

        // browsable attributes
        private string titleText = "Drag+Drop Monitor Block Here";

        #region NONBROWSABLE

        #endregion


        #region BROWSABLE
        [DataMember]
        [DefaultValueAttribute("Drag+Drop Monitor Block Here")]
        [CategoryAttribute("Display"), DescriptionAttribute("The text for the block. Can be anything you wish.")]
        public string TitleText { get => titleText; set => titleText = value; }

        #endregion

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Deep clone this object, we just use a quick xml serialize/deserialize
        /// this does not happen often
        /// </summary>
        public MonitorBlockProperties_Blank DeepClone(MonitorBlockProperties_Blank obj)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(typeof(MonitorBlockProperties_Blank));
                xs.Serialize(ms, obj);
                ms.Position = 0;

                return (MonitorBlockProperties_Blank)xs.Deserialize(ms);
            }
        }
    }
}

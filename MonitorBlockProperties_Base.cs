﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using OISCommon;

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
    /// A class to provide properties for the ctlMonitorBlock_Base class. An
    /// object of this class (or its children) serves as the backing store which
    /// drives the propertyGrid behind each monitor block.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Blank))]
    [KnownType(typeof(MonitorBlockProperties_Bool))]
    [KnownType(typeof(MonitorBlockProperties_Graph))]
    [KnownType(typeof(MonitorBlockProperties_Integral))]
    [KnownType(typeof(MonitorBlockProperties_MinMax))]
    [KnownType(typeof(MonitorBlockProperties_Number))]
    [KnownType(typeof(MonitorBlockProperties_Latch))]
    [KnownType(typeof(MonitorBlockProperties_Text))]
    [KnownType(typeof(MonitorBlockProperties_Base))]


    [DataContract]
    [Serializable]
    public class MonitorBlockProperties_Base 
    {

        public const string DEFAULT_USRDEF_DISPLAYNAME_PREFIX = "User Def";
        public const string DEFAULT_DISPLAYNAME_PREFIX = ".Generic MB";
        public const string DEFAULT_DISPLAYNAME_SUFFIX = "Base";
        public const string DEFAULT_DISPLAY_NAME = DEFAULT_DISPLAYNAME_PREFIX+" "+ DEFAULT_DISPLAYNAME_SUFFIX;
        private string displayName = DEFAULT_DISPLAY_NAME;
        public const string DEFAULT_SLOTID = "?x?";
        private string slotID = DEFAULT_SLOTID;

        // this helps us remove the object from the user defined MB picker list on drop
        private bool wantUsrPickerDeleteOnDrop = false;
        // this flags the control as auto generated
        private bool isAutoGenerated = false;

        #region NONBROWSABLE
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool WantUsrPickerDeleteOnDrop { get => wantUsrPickerDeleteOnDrop; set => wantUsrPickerDeleteOnDrop = value; }

        public string summaryString = "Base Monitor Block";

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool IsAutoGenerated { get => isAutoGenerated; set => isAutoGenerated = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the summary string. Used for titles etc. Never gets/sets null
        /// </summary>
        [Browsable(false)]
        public string SummaryString
        {
            get
            {
                if (summaryString == null) summaryString = "";
                return summaryString;
            }
            set
            {
                summaryString = value;
                if (summaryString == null) summaryString = "";
            }
        }

        #endregion

        #region BROWSABLE

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the displayName. This is a more user understandable name of the
        /// monitor block. Never returns null or empty will use the default.
        /// </summary>
        [DataMember]
        [DefaultValueAttribute(".Generic MB Base")]
        [CategoryAttribute("Display"), DescriptionAttribute("The display name of this device. This is what you see in the User Defined Monitor Block List.")]
        public string DisplayName
        {
            get
            {
                if ((displayName == null) || (displayName.Length == 0)) displayName = DEFAULT_DISPLAY_NAME;
                return displayName;
            }
            set
            {
                displayName = value;
                if ((displayName == null) || (displayName.Length == 0)) displayName = DEFAULT_DISPLAY_NAME;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the slotID. This is an identifier used to identify the slot on the 
        /// summary panel on which the control is positioned. Never returns null or empty
        /// </summary>
        [DataMember]
        [DefaultValueAttribute("?x?")]
        [CategoryAttribute("Display"), DescriptionAttribute("The summary screen slot of a specific Monitor Block")]
#if DEBUG
        [Browsable(true)]
#else
        [Browsable(false)]
#endif
        public string SlotID
        {
            get
            {
                // NOTE this property should be read only
                if ((slotID == null) || (slotID.Length == 0)) slotID = DEFAULT_SLOTID;
                return slotID;
            }
            set
            {
                slotID = value;
                if ((slotID == null) || (slotID.Length == 0)) slotID = DEFAULT_SLOTID;
            }
        }

        #endregion

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Overrides ToString() we return the DisplayName here
        /// </summary>
        public override string ToString()
        {
            return DisplayName;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// DeepClones this object. We did have a nice XML Serializer/Deserializer
        /// here but it proved problematic. So it is replaced with this hand cranked
        /// version. Anytime you add a field you need to add it to this clone function
        /// </summary>
        /// <returns>A deep clone of this object</returns>
        public MonitorBlockProperties_Base DeepClone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;

                return (MonitorBlockProperties_Base)formatter.Deserialize(ms);
            }
        }


    }
}

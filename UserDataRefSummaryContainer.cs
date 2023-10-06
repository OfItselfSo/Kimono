using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

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
    /// A class to contain information about the usage of UserData references
    /// </summary>
    [DataContract]
    public class UserDataRefSummaryContainer
    {
        private string udN01 = "";
        private string udN02 = "";
        private string udN03 = "";
        private string udN04 = "";
        private string udN05 = "";
        private string udN06 = "";
        private string udN07 = "";
        private string udN08 = "";
        private string udN09 = "";
        private string udN10 = "";
        private string udN11 = "";
        private string udN12 = "";
        private string udN13 = "";
        private string udN14 = "";

        //private string udD01  = "";
        //private string udD02  = "";
        //private string udD03  = "";

        //private string udT01  = "";
        //private string udT02  = "";
        //private string udT03  = "";

        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN01 { get { return udN01; } set { udN01 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN02 { get { return udN02; } set { udN02 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN03 { get { return udN03; } set { udN03 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN04 { get { return udN04; } set { udN04 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN05 { get { return udN05; } set { udN05 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN06 { get { return udN06; } set { udN06 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN07 { get { return udN07; } set { udN07 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN08 { get { return udN08; } set { udN08 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN09 { get { return udN09; } set { udN09 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN10 { get { return udN10; } set { udN10 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN11 { get { return udN11; } set { udN11 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN12 { get { return udN12; } set { udN12 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN13 { get { return udN13; } set { udN13 = value;} }
        [DataMember]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Number"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        public string UDN14 { get { return udN14; } set { udN14 = value;} }

        //[DataMember]
        //[DefaultValueAttribute("")]
        //[CategoryAttribute("Date"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        //public string UDD01 { get { return udD01; } set { udD01 = value;} }
        //[DataMember]
        //[DefaultValueAttribute("")]
        //[CategoryAttribute("Date"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        //public string UDD02 { get { return udD02; } set { udD02 = value;} }
        //[DataMember]
        //[DefaultValueAttribute("")]
        //[CategoryAttribute("Date"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        //public string UDD03 { get { return udD03; } set { udD03 = value;} }

        //[DataMember]
        //[DefaultValueAttribute("")]
        //[CategoryAttribute("Text"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        //public string UDT01 { get { return udT01; } set { udT01 = value;} }
        //[DataMember]
        //[DefaultValueAttribute("")]
        //[CategoryAttribute("Text"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        //public string UDT02 { get { return udT02; } set { udT02 = value;} }
        //[DataMember]
        //[DefaultValueAttribute("")]
        //[CategoryAttribute("Text"), DescriptionAttribute("The Title of the Monitor Block currently using this UserData reference.")]
        //public string UDT03 { get { return udT03; } set { udT03 = value;} }
    }
}

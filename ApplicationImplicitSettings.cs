using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Drawing;

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
    /// A class to persist the settings of the Kimono app.
    /// </summary>
    /// <remarks>
    /// The ApplicationSettingsBase class does not write to the registry. It 
    /// persists the configuration settings by leaving an XML file on disk in the location 
    /// below.
    /// 
    /// <document and settings folder>\<userfolder>\Local Settings\Application
    /// Data\<companyname>\<applicationname>_StrongName\<applicationversion>\user.config
    /// 
    /// This code uses the techniques discussed here
    /// http://msdn.microsoft.com/en-us/library/system.configuration.applicationsettingsbase%28VS.80%29.aspx
    /// </remarks>
    public sealed class ApplicationImplicitSettings : ApplicationSettingsBase
    {
      
        // ####################################################################
        // ##### Config Items Explicitly Set By the User
        // ####################################################################
        #region Config Items Explicitly Set By the User

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("false")]
        public bool OKWithDisclaimer
        {
            get { return (bool)this["OKWithDisclaimer"]; }
            set { this["OKWithDisclaimer"] = value; }
        }

        #endregion

        // ####################################################################
        // ##### Config Items Implicitly Set By the User
        // ####################################################################
        #region Config Items Implicitly Set By the User

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("800, 600")]
        public Size FormSize
        {
            get { return (Size)this["FormSize"]; }
            set { this["FormSize"] = value; }
        }

        #endregion

    }
}


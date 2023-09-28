using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// Handles the display of known devices and their acceptable fields
    /// </summary>
    public partial class ctlDeviceAndFieldPicker : ctlOISBase
    {
        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlDeviceAndFieldPicker()
        {
            InitializeComponent();
            // set up the known devices combobox
            SetupDataFrequencyComboBox();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Configures the known devices combobox
        /// </summary>
        private void SetupDataFrequencyComboBox()
        {
            comboBoxKnownDevices.DataSource = Enum.GetValues(typeof(KnownDeviceEnum));
            comboBoxKnownDevices.SelectedItem = KnownDeviceEnum.None;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the device alias. In practice this means accessing the combo box
        /// Will return null
        /// </summary>
        public string DeviceAlias
        {
            get
            {
                // the ToString() of an enum is just the enum value text
                if (comboBoxKnownDevices.SelectedItem == null) return null;
                return comboBoxKnownDevices.SelectedItem.ToString();
            }
            set
            {
                KnownDeviceEnum enumVal = KnownDeviceEnum.None;
                // try to get the value from the string
                try
                {
                    enumVal = (KnownDeviceEnum)Enum.Parse(typeof(KnownDeviceEnum), value);
                }
                catch
                {
                    enumVal = KnownDeviceEnum.None;
                }
                // now set it
                comboBoxKnownDevices.SelectedItem = enumVal;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the fieldname. In practice this means accessing the combo box
        /// Will return null. Note the known devices combobox must have been set first
        /// when setting.
        /// </summary>
        public string FieldName
        {
            get
            {
                // the ToString() of an enum is just the enum value text
                if (comboBoxDeviceFields.SelectedItem == null) return null;
                return comboBoxDeviceFields.SelectedItem.ToString();
            }
            set
            {
                // the known devices combobox must have been set first
                switch (comboBoxKnownDevices.SelectedItem)
                {
                    case KnownDeviceEnum.MATE3S:
                        KnownDevicePlottableFieldsMATE3SEnum enumValMATE3S = KnownDevicePlottableFieldsMATE3SEnum.None;
                        try
                        {
                            enumValMATE3S = (KnownDevicePlottableFieldsMATE3SEnum)Enum.Parse(typeof(KnownDevicePlottableFieldsMATE3SEnum), value);
                        }
                        catch { enumValMATE3S = KnownDevicePlottableFieldsMATE3SEnum.None; }
                        comboBoxDeviceFields.SelectedItem = enumValMATE3S;
                        break;
                    case KnownDeviceEnum.FNDC:
                        KnownDevicePlottableFieldsFNDCEnum enumValFNDC = KnownDevicePlottableFieldsFNDCEnum.None;
                        try
                        {
                            enumValFNDC = (KnownDevicePlottableFieldsFNDCEnum)Enum.Parse(typeof(KnownDevicePlottableFieldsFNDCEnum), value);
                        }
                        catch { enumValFNDC = KnownDevicePlottableFieldsFNDCEnum.None; }
                        comboBoxDeviceFields.SelectedItem = enumValFNDC;
                        break;
                    case KnownDeviceEnum.USERDATA:
                        KnownDevicePlottableFieldsUSERDATAEnum enumValUSERDATA = KnownDevicePlottableFieldsUSERDATAEnum.None;
                        try
                        {
                            enumValUSERDATA = (KnownDevicePlottableFieldsUSERDATAEnum)Enum.Parse(typeof(KnownDevicePlottableFieldsUSERDATAEnum), value);
                        }
                        catch { enumValUSERDATA = KnownDevicePlottableFieldsUSERDATAEnum.None; }
                        comboBoxDeviceFields.SelectedItem = enumValUSERDATA;
                        break;
                    case KnownDeviceEnum.CC1:
                    case KnownDeviceEnum.CC2:
                    case KnownDeviceEnum.CC3:
                        KnownDevicePlottableFieldsCCEnum enumValCC = KnownDevicePlottableFieldsCCEnum.None;
                        try
                        {
                            enumValCC = (KnownDevicePlottableFieldsCCEnum)Enum.Parse(typeof(KnownDevicePlottableFieldsCCEnum), value);
                        }
                        catch { enumValCC = KnownDevicePlottableFieldsCCEnum.None; }
                        comboBoxDeviceFields.SelectedItem = enumValCC;
                        break;
                    case KnownDeviceEnum.INV1:
                    case KnownDeviceEnum.INV2:
                    case KnownDeviceEnum.INV3:
                        KnownDevicePlottableFieldsINVEnum enumValINV = KnownDevicePlottableFieldsINVEnum.None;
                        try
                        {
                            enumValINV = (KnownDevicePlottableFieldsINVEnum)Enum.Parse(typeof(KnownDevicePlottableFieldsINVEnum), value);
                        }
                        catch { enumValINV = KnownDevicePlottableFieldsINVEnum.None; }
                        comboBoxDeviceFields.SelectedItem = enumValINV;
                        break;
                    case KnownDeviceEnum.None:
                    default:
                        break;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a change of selection on the comboBoxKnownDevices
        /// </summary>
        private void comboBoxKnownDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            // always clear down the fields box
            switch(comboBoxKnownDevices.SelectedItem)
            {
                case KnownDeviceEnum.MATE3S:
                    comboBoxDeviceFields.DataSource = Enum.GetValues(typeof(KnownDevicePlottableFieldsMATE3SEnum));
                    comboBoxDeviceFields.SelectedItem = KnownDevicePlottableFieldsMATE3SEnum.None;
                    break;
                case KnownDeviceEnum.FNDC:
                    comboBoxDeviceFields.DataSource = Enum.GetValues(typeof(KnownDevicePlottableFieldsFNDCEnum));
                    comboBoxDeviceFields.SelectedItem = KnownDevicePlottableFieldsFNDCEnum.None;
                    break;
                case KnownDeviceEnum.USERDATA:
                    comboBoxDeviceFields.DataSource = Enum.GetValues(typeof(KnownDevicePlottableFieldsUSERDATAEnum));
                    comboBoxDeviceFields.SelectedItem = KnownDevicePlottableFieldsUSERDATAEnum.None;
                    break;
                case KnownDeviceEnum.CC1:
                case KnownDeviceEnum.CC2:
                case KnownDeviceEnum.CC3:
                    comboBoxDeviceFields.DataSource = Enum.GetValues(typeof(KnownDevicePlottableFieldsCCEnum));
                    comboBoxDeviceFields.SelectedItem = KnownDevicePlottableFieldsCCEnum.None;
                    break;
                case KnownDeviceEnum.INV1:
                case KnownDeviceEnum.INV2:
                case KnownDeviceEnum.INV3:
                    comboBoxDeviceFields.DataSource = Enum.GetValues(typeof(KnownDevicePlottableFieldsINVEnum));
                    comboBoxDeviceFields.SelectedItem = KnownDevicePlottableFieldsINVEnum.None;
                    break;
                case KnownDeviceEnum.None:
                default:
                    comboBoxDeviceFields.DataSource = null;
                    break;
            }
        }
    }
}

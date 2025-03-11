using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// A class which defines every Mate3s port status as reported by the Outback system
    /// 
    //  This object is populated by the JSON deserializer. The properties are filled by name
    /// using reflection and the case really MATTERS! Any element in the JSON not 
    /// appearing here as a property will just be ignored
    ///
    /// NOTE this class contains all possible properties for all possible port types.
    /// This makes the JSON deserialize much easier. The ones that are unused are just
    /// left at defaults. You just have to know which fields to access based on the 
    /// type of port the data in this object represents. 
    /// 
    /// </summary>
    public class PortStatus
    {
        // these are the identifiers in the JSON used to indicate what type
        // of data record this object represents
        public const string DEVICE_IDENTIFIER_FNDC = "FNDC";
        public const string DEVICE_IDENTIFIER_CC = "CC";
        public const string DEVICE_IDENTIFIER_FXR = "FXR";
        public const string DEVICE_IDENTIFIER_GS = "GS";

        private const int DEFAULT_PORT_NUMBER = -1;
        private bool userDataHasChanged = false;

        // these are the field names for the user db store
        public const string USERDB_STORE_NUMBER_PREFIX = "UDN";
        public const string USERDB_STORE_TEXT_PREFIX = "UDT";
        public const string USERDB_STORE_DATE_PREFIX = "UDD";

        public const string USERDB_STORE_UDN01 = USERDB_STORE_NUMBER_PREFIX + "01";
        public const string USERDB_STORE_UDN02 = USERDB_STORE_NUMBER_PREFIX + "02";
        public const string USERDB_STORE_UDN03 = USERDB_STORE_NUMBER_PREFIX + "03";
        public const string USERDB_STORE_UDN04 = USERDB_STORE_NUMBER_PREFIX + "04";
        public const string USERDB_STORE_UDN05 = USERDB_STORE_NUMBER_PREFIX + "05";
        public const string USERDB_STORE_UDN06 = USERDB_STORE_NUMBER_PREFIX + "06";
        public const string USERDB_STORE_UDN07 = USERDB_STORE_NUMBER_PREFIX + "07";
        public const string USERDB_STORE_UDN08 = USERDB_STORE_NUMBER_PREFIX + "08";
        public const string USERDB_STORE_UDN09 = USERDB_STORE_NUMBER_PREFIX + "09";
        public const string USERDB_STORE_UDN10 = USERDB_STORE_NUMBER_PREFIX + "10";
        public const string USERDB_STORE_UDN11 = USERDB_STORE_NUMBER_PREFIX + "11";
        public const string USERDB_STORE_UDN12 = USERDB_STORE_NUMBER_PREFIX + "12";
        public const string USERDB_STORE_UDN13 = USERDB_STORE_NUMBER_PREFIX + "13";
        public const string USERDB_STORE_UDN14 = USERDB_STORE_NUMBER_PREFIX + "14";
        public const string USERDB_STORE_UDD01 = USERDB_STORE_DATE_PREFIX + "01";
        public const string USERDB_STORE_UDD02 = USERDB_STORE_DATE_PREFIX + "02";
        public const string USERDB_STORE_UDD03 = USERDB_STORE_DATE_PREFIX + "03";
        public const string USERDB_STORE_UDT01 = USERDB_STORE_TEXT_PREFIX + "01";
        public const string USERDB_STORE_UDT02 = USERDB_STORE_TEXT_PREFIX + "02";
        public const string USERDB_STORE_UDT03 = USERDB_STORE_TEXT_PREFIX + "03";

        // this is the datetime on the PC not the datetime in the JSON
        private DateTime recordDate;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public PortStatus()
        {
            Port = DEFAULT_PORT_NUMBER;
        }

        // this is the Kimono softwares name for this port and type
        public string DeviceAlias { get; set; }

        // these properties represent all possible JSON parameters in the incoming 
        // JSON string. Not all port objects will use all of these. Some types will
        // use the ones with the same name. They are noted in alphabetical order for
        // ease of maintenance

        public string AC_Input { get; set; }
        public string AC_mode { get; set; }
        public string AUX { get; set; }
        public string Aux_mode { get; set; }
        public string Batt_temp { get; set; }
        public double Batt_V { get; set; }
        public double Buy_I_L2 { get; set; }
        public string CC_mode { get; set; }
        public double Chg_I_L2 { get; set; }
        public bool CHG_parms_met { get; set; }
        public double Days_since_full { get; set; }
        public string Dev { get; set; }
        public IList<string> Enabled { get; set; }
        public IList<string> Error { get; set; }
        public string Gateway_Type { get; set; }
        public double In_AH_today { get; set; }
        public double In_kWh_today { get; set; }
        public double In_I { get; set; }
        public double In_V { get; set; }
        public double Inv_I_L2 { get; set; }
        public string INV_mode { get; set; }
        public double Min_SOC { get; set; }
        public double Net_CFC_AH { get; set; }
        public double Net_CFC_kWh { get; set; }
        public DateTime OutbackDate { get; set; }
        public double Out_AH { get; set; }
        public double Out_AH_today { get; set; }
        public double Out_I { get; set; }
        public double Out_kWh { get; set; }
        public double Out_kWh_today { get; set; }
        public int Port { get; set; }
        public double Sell_I_L2 { get; set; }
        public double Shunt_A_I { get; set; }
        public double Shunt_A_AH { get; set; }
        public double Shunt_A_kWh { get; set; }
        public double Shunt_B_I { get; set; }
        public double Shunt_B_AH { get; set; }
        public double Shunt_B_kWh { get; set; }
        public double Shunt_C_I { get; set; }
        public double Shunt_C_AH { get; set; }
        public double Shunt_C_kWh { get; set; }
        public double Sys_Batt_V { get; set; }
        public double SOC { get; set; }
        public string Type { get; set; }
        public IList<string> Warn { get; set; }
        public double VAC1_in_L2 { get; set; }
        public double VAC2_in_L2 { get; set; }
        public double VAC_out_L2 { get; set; }

        // for Radian - Kiomono V01.04
        public double Inv_I_L1 { get; set; }
        public double Chg_I_L1 { get; set; }
        public double Buy_I_L1 { get; set; }
        public double Sell_I_L1 { get; set; }
        public double VAC1_in_L1 { get; set; }
        public double VAC2_in_L1 { get; set; }
        public double VAC_out_L1 { get; set; }
        public string RELAY { get; set; }

        private double udN01 = 0;
        private double udN02 = 0;
        private double udN03 = 0;
        private double udN04 = 0;
        private double udN05 = 0;
        private double udN06 = 0;
        private double udN07 = 0;
        private double udN08 = 0;
        private double udN09 = 0;
        private double udN10 = 0;
        private double udN11 = 0;
        private double udN12 = 0;
        private double udN13 = 0;
        private double udN14 = 0;

        private DateTime udD01 = DateTime.MinValue;
        private DateTime udD02 = DateTime.MinValue;
        private DateTime udD03 = DateTime.MinValue;

        private string udT01 = null;
        private string udT02 = null;
        private string udT03 = null;

        public double UDN01 { get { return udN01; } set { udN01 = value; UserDataHasChanged = true; } }
        public double UDN02 { get { return udN02; } set { udN02 = value; UserDataHasChanged = true; } }
        public double UDN03 { get { return udN03; } set { udN03 = value; UserDataHasChanged = true; } }
        public double UDN04 { get { return udN04; } set { udN04 = value; UserDataHasChanged = true; } }
        public double UDN05 { get { return udN05; } set { udN05 = value; UserDataHasChanged = true; } }
        public double UDN06 { get { return udN06; } set { udN06 = value; UserDataHasChanged = true; } }
        public double UDN07 { get { return udN07; } set { udN07 = value; UserDataHasChanged = true; } }
        public double UDN08 { get { return udN08; } set { udN08 = value; UserDataHasChanged = true; } }
        public double UDN09 { get { return udN09; } set { udN09 = value; UserDataHasChanged = true; } }
        public double UDN10 { get { return udN10; } set { udN10 = value; UserDataHasChanged = true; } }
        public double UDN11 { get { return udN11; } set { udN11 = value; UserDataHasChanged = true; } }
        public double UDN12 { get { return udN12; } set { udN12 = value; UserDataHasChanged = true; } }
        public double UDN13 { get { return udN13; } set { udN13 = value; UserDataHasChanged = true; } }
        public double UDN14 { get { return udN14; } set { udN14 = value; UserDataHasChanged = true; } }

        public DateTime UDD01 { get { return udD01; } set { udD01 = value; UserDataHasChanged = true; } }
        public DateTime UDD02 { get { return udD02; } set { udD02 = value; UserDataHasChanged = true; } }
        public DateTime UDD03 { get { return udD03; } set { udD03 = value; UserDataHasChanged = true; } }

        public string UDT01 { get { return udT01; } set { udT01 = value; UserDataHasChanged = true; } }
        public string UDT02 { get { return udT02; } set { udT02 = value; UserDataHasChanged = true; } }
        public string UDT03 { get { return udT03; } set { udT03 = value; UserDataHasChanged = true; } }
        public DateTime RecordDate { get => recordDate; set => recordDate = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detects if the user data has changed
        /// </summary>
        public bool UserDataHasChanged { get => userDataHasChanged; set => userDataHasChanged = value; }

    }
}

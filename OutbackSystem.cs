using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
    /// Defines the state of the Outback System as reported by the Mate3s. 
    /// </summary>
    public class OutbackSystem
    {

        // these are the aliases we use to access the contained classes as strings. The user specifies
        // these in the monitor block properties in order to specifiy the device (essentially converted JSON object)
        // they wish to access.
        public const string DEVALIAS_USERDATA = "USERDATA";
        public const string DEVALIAS_MATE3S = "MATE3S";
        public const string DEVALIAS_FNDC = "FNDC";
        public const string DEVALIAS_CC_PREFIX = "CC"; // charge controller alias prefix
        public const string DEVALIAS_INV_PREFIX = "INV"; // inverter alias prefix
        public const string DEVALIAS_CC1 = DEVALIAS_CC_PREFIX + "1"; // first charge controller
        public const string DEVALIAS_CC2 = DEVALIAS_CC_PREFIX + "2";
        public const string DEVALIAS_CC3 = DEVALIAS_CC_PREFIX + "3";
        public const string DEVALIAS_INV1 = DEVALIAS_INV_PREFIX + "1"; // first inverter
        public const string DEVALIAS_INV2 = DEVALIAS_INV_PREFIX + "2";
        public const string DEVALIAS_INV3 = DEVALIAS_INV_PREFIX + "3";

        public const string DEVPORTPREFIX = "PORT";
        public const string DEVALIAS_PORT0 = DEVPORTPREFIX + "0";  // always the mate3s is synonym for DEVALIAS_MATE3S
        public const string DEVALIAS_PORT1 = DEVPORTPREFIX + "1";  // device on port 1 can be a CC or an INV or FNDC
        public const string DEVALIAS_PORT2 = DEVPORTPREFIX + "2";
        public const string DEVALIAS_PORT3 = DEVPORTPREFIX + "3";
        public const string DEVALIAS_PORT4 = DEVPORTPREFIX + "4";
        public const string DEVALIAS_PORT5 = DEVPORTPREFIX + "5";
        public const string DEVALIAS_PORT6 = DEVPORTPREFIX + "6";
        public const string DEVALIAS_PORT7 = DEVPORTPREFIX + "7";
        public const string DEVALIAS_PORT8 = DEVPORTPREFIX + "8";
        public const string DEVALIAS_PORT9 = DEVPORTPREFIX + "9";
        public const string DEVALIAS_PORT10 = DEVPORTPREFIX + "10";

        public const char DATASOURCE_DELIMTER_CHAR = '#';
        public const string DATASOURCE_DELIMTER_STR = "#";
        public const string DATASOURCE_DELIMTER_ALPHANUM = "_HASH_DELIM_";

        public const char USRREF_DELIMTER_CHAR = '$';
        public const string USRREF_DELIMTER_STR = "$";
        public const string USRREF_DELIMTER_ALPHANUM = "_DOLLAR_USRREF_";
        public const string EVALUATE_DATA_PREFIX = "=";

        // a flag to signal if this data has been written. We do not want to do this twice
        private bool dataHasBeenWrittenToDB = false;

        // the JSON backing this report. Should never be null
        string lastReportJSON = "";

        // this is the main device object, there is nothing recorded in the JSON report
        // above this
        public DevStatus devstatus { get; set; }

        // the information below is derived at load time from the contents of the JSON

        // this is the Mate3s pseudo device        
        private PortStatus mate3sPort = null;
        // this is the UserData pseudo device        
        private PortStatus userDataPort = null;
        // this is the FNDC device        
        private PortStatus primaryFNDCPort = null;
        // these are the charge controllers
        private List<PortStatus> chargeControllerDevices = new List<PortStatus>();
        // these are the inverters
        private List<PortStatus> inverterDevices = new List<PortStatus>();


        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public OutbackSystem() 
        {
            // defaults
            RecordDate = DateTime.Now;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The date this record was created
        /// </summary>
        public DateTime RecordDate { get; set; }  // date on the PC when this record was written

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Perform post processing steps on the report once it has been converted
        /// from JSON
        /// </summary>
        public void PostProcess()
        {
            // the post processing date of this object is the date in the record
            RecordDate = DateTime.Now;

            // do we have a nested devstatus object
            if (devstatus == null) return;

            // create a pseudo Mate3s port
            Mate3sPort = new PortStatus();
            // configure it with the data in the devstatus object
            Mate3sPort.OutbackDate = Utils.FromEpochTime(devstatus.Sys_Time);
            Mate3sPort.Port = 0;  // always assumed to be this
            Mate3sPort.RecordDate = this.RecordDate;
            Mate3sPort.Sys_Batt_V = devstatus.Sys_Batt_V;
            Mate3sPort.Gateway_Type = devstatus.Gateway_Type;
            Mate3sPort.DeviceAlias = DEVALIAS_MATE3S;
            // add it to the port list
            devstatus.ports.Add(mate3sPort);

            // create the pseudo UserData port
            UserDataPort = new PortStatus();
            UserDataPort.Port = -1;  // always assumed to be this
            // it gets no data by default
            UserDataPort.RecordDate = this.RecordDate;
            UserDataPort.DeviceAlias = DEVALIAS_USERDATA;
            // add it to the port list
            devstatus.ports.Add(UserDataPort);

            // counters
            int ccNumber = 1;
            int invNumber = 1;
            // we loop through the ports. 
            foreach (PortStatus portObject in devstatus.ports)
            {
                // always give it this
                portObject.RecordDate = this.RecordDate;

                // now group the various port types

                // now the FNDC. The first FNDC we find is the considered to 
                // be the primary
                if (portObject.Dev == PortStatus.DEVICE_IDENTIFIER_FNDC)
                {
                    // set this now
                    PrimaryFNDCPort = portObject;
                    PrimaryFNDCPort.DeviceAlias = DEVALIAS_FNDC;
                }

                // now the Charge Controllers. We collect them all in order
                if (portObject.Dev == PortStatus.DEVICE_IDENTIFIER_CC)
                {
                    // set this now
                    ChargeControllerDevices.Add(portObject);
                    portObject.DeviceAlias = DEVALIAS_CC_PREFIX + ccNumber.ToString();
                    ccNumber++;
                }

                // now the Inverters. We collect them all in order
                if (portObject.Dev == PortStatus.DEVICE_IDENTIFIER_FXR)
                {
                    // set this now
                    InverterDevices.Add(portObject);
                    portObject.DeviceAlias = DEVALIAS_INV_PREFIX + invNumber.ToString();
                    invNumber++;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Sets the time markers on the record.
        /// </summary>
        /// <param name="markerRecorder">the current marker recorder</param>
        public void SetDatabaseTimeMarkers(IntervalRecorder markerRecorder)
        {
            // set up our markers
            if (markerRecorder != null)
            {
                markerRecorder.UpdateMarkerFlags(RecordDate);
                FiveMinuteMarker = markerRecorder.FiveMinuteMarkerFlag;
                TenMinuteMarker = markerRecorder.TenMinuteMarkerFlag;
                FifteenMinuteMarker = markerRecorder.FifteenMinuteMarkerFlag;
                ThirtyMinuteMarker = markerRecorder.ThirtyMinuteMarkerFlag;
                HourMarker = markerRecorder.HourMarkerFlag;
                DayMarker = markerRecorder.DayMarkerFlag;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets a double data item from the outback system report.
        /// 
        /// Note: Uses reflection!
        /// 
        /// </summary>
        /// <param name="dataSource">the dotted name of the device and field to access</param>
        /// <param name="outVal">the data item is returned here, "" on fail</param>
        /// <returns>true - success, false - fail</returns>
        public bool GetDataFromOutbackSystemReportByDeviceAndFieldName_double(string dataSource, out double outVal)
        {
            outVal = 0;

            // sanity checks
            if (dataSource == null) return false;
            if (dataSource.Length == 0) return false;

            // recover the device and field name from the dataSource string
            string deviceName = null;
            string fieldName = null;
            int retInt = GetDeviceNameAndFieldFromDataSource(dataSource, out deviceName, out fieldName);
            if (retInt != 0) return false;
            if (deviceName == null) return false;
            if (deviceName.Length == 0) return false;
            if (fieldName == null) return false;
            if (fieldName.Length == 0) return false;

            // get the device
            PortStatus dataObj = GetOutbackDeviceObjectByDeviceName(deviceName);
            if (dataObj == null) return false;
            // get the data from the Mate3s report sub-object via reflection
            PropertyInfo propertyInfo = dataObj.GetType().GetProperty(fieldName);
            // do we have that property here? All kinds of nonsense can come in on this field since it is user editable
            if (propertyInfo == null) return false;
            // we have the property, get the value
            var outData = propertyInfo.GetValue(dataObj, null);
            // we expect a double here, this procedure only gets doubles
            if ((outData is double) == false) return false;
            // set up for the return
            outVal = (double)outData;
            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets a string data item from the outback system report.
        /// 
        /// Note: Uses reflection!
        /// 
        /// </summary>
        /// <param name="dataSource">the dotted name of the device and field to access</param>
        /// <param name="outVal">the data item is returned here, "" on fail</param>
        /// <returns>true - success, false - fail</returns>
        public bool GetDataFromOutbackSystemReportByDeviceAndFieldName_string(string dataSource, out string outVal)
        {
            outVal = "";

            // sanity checks
            if (dataSource == null) return false;
            if (dataSource.Length == 0) return false;

            // recover the device and field name from the dataSource string
            string deviceName = null;
            string fieldName = null;
            int retInt = OutbackSystem.GetDeviceNameAndFieldFromDataSource(dataSource, out deviceName, out fieldName);
            if (retInt != 0) return false;
            if (deviceName == null) return false;
            if (deviceName.Length == 0) return false;
            if (fieldName == null) return false;
            if (fieldName.Length == 0) return false;

            // get the device
            PortStatus dataObj = GetOutbackDeviceObjectByDeviceName(deviceName);
            if (dataObj == null) return false;
            // get the data from the Mate3s report sub-object via reflection
            PropertyInfo propertyInfo = dataObj.GetType().GetProperty(fieldName);
            // do we have that property here? All kinds of nonsense can come in on this field since it is user editable
            if (propertyInfo == null) return false;
            // we have the property, get the value
            var outData = propertyInfo.GetValue(dataObj, null);
            // we expect a double here, this procedure only gets strings
            if ((outData is string) == false) return false;
            // set up for the return
            outVal = (string)outData;
            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the device object from the outback report by device name. The device
        /// name can be an alias.
        /// 
        /// </summary>
        /// <param name="deviceName">the raw name or alias of the device in the outback data</param>
        /// <returns>the device object or null for fail</returns>
        public PortStatus GetOutbackDeviceObjectByDeviceName(string deviceName)
        {
            if (deviceName == null) return null;
            if (deviceName.Length == 0) return null;

            if ( (deviceName == OutbackSystem.DEVALIAS_MATE3S) || (deviceName == OutbackSystem.DEVALIAS_PORT0))
            {
                return this.Mate3sPort;
            }
            else if (deviceName == OutbackSystem.DEVALIAS_USERDATA)
            {
                return this.UserDataPort;
            }
            else if (deviceName == OutbackSystem.DEVALIAS_FNDC)
            {
                return this.PrimaryFNDCPort;
            }
            else if (deviceName == OutbackSystem.DEVALIAS_CC1)
            {
                // looking for the first charge controller device
                if (this.ChargeControllerDevices.Count < 1) return null;
                return this.ChargeControllerDevices[0];
            }
            else if (deviceName == OutbackSystem.DEVALIAS_CC2)
            {
                // looking for the first charge controller device
                if (this.ChargeControllerDevices.Count < 2) return null;
                return this.ChargeControllerDevices[1];
            }
            else if (deviceName == OutbackSystem.DEVALIAS_CC3)
            {
                // looking for the first charge controller device
                if (this.ChargeControllerDevices.Count < 3) return null;
                return this.ChargeControllerDevices[2];
            }
            else if (deviceName == OutbackSystem.DEVALIAS_INV1)
            {
                // looking for the first inverter device
                if (this.InverterDevices.Count < 1) return null;
                return this.InverterDevices[0];
            }
            else if (deviceName == OutbackSystem.DEVALIAS_INV2)
            {
                // looking for the first inverter device
                if (this.InverterDevices.Count < 2) return null;
                return this.InverterDevices[1];
            }
            else if (deviceName == OutbackSystem.DEVALIAS_INV3)
            {
                // looking for the first inverter device
                if (this.InverterDevices.Count < 3) return null;
                return this.InverterDevices[2];
            }
            else if (deviceName.StartsWith(OutbackSystem.DEVPORTPREFIX) == true)
            {
                if (this.devstatus == null) return null;
                if (this.devstatus.ports == null) return null;
                // port number are apparently 1 based. Port 0 is the Mate3s itself
                int portNumber = -1;
                try
                {
                    // get the port number by converting the alias
                    portNumber = Convert.ToInt32(deviceName.Replace(OutbackSystem.DEVPORTPREFIX, ""));
                }
                catch { return null; }
                // return what we got
                return this.devstatus.GetPortByNumber(portNumber);
            }
            return null;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the device and field names from a dataSource string. These always 
        /// are assumed to be in deviceName#fieldname format. 
        /// 
        /// </summary>
        /// <param name="dataSource">the dataSource string to look at</param>
        /// <param name="deviceName">the alias of the device in the dataSource</param>
        /// <param name="fieldName">the field name in the dataSource</param>
        /// <returns>z success, nz fail</returns>
        public static int GetDeviceNameAndFieldFromDataSource(string dataSource, out string deviceName, out string fieldName)
        {

            // init
            deviceName = null;
            fieldName = null;
            char[] sep = { OutbackSystem.DATASOURCE_DELIMTER_CHAR };

            if (dataSource == null) return 1;
            // there can be no spaces or other delimiters here
            string tmpDataSource = dataSource.Replace(" ", "");

            // more checks
            if (tmpDataSource.Length == 0) return 2;

            // separate the cleaned dataSource
            string[] strlist = tmpDataSource.Split(sep);
            // we expect only two
            if (strlist.Length != 2) return 2;
            // the first one is always the device name
            deviceName = strlist[0];
            // the field name is always the second
            fieldName = strlist[1];

            return 0;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the port number as an int from a dataSource string. DataSources  
        /// are assumed to be in deviceName#fieldname format. 
        /// 
        /// NOTE: if the dataSource references the DevStatus object we return
        /// a pseudo portNumber of 0;
        /// 
        /// </summary>
        /// <param name="dataSource">the dataSource string to look at</param>
        /// <returns>port number or -ve for fail</returns>
        public int GetPortNumberForDataSource(string dataSource )
        {
            string deviceName = "";
            string fieldName = "";

            if (dataSource == null) return -1;
            if (dataSource.Length == 0) return -1;

            // split the dataSource and get the device and field
            int retInt = GetDeviceNameAndFieldFromDataSource(dataSource, out deviceName, out fieldName);
            if (retInt != 0) return -1;

            // we are interested only in the device name
            PortStatus devObj = GetOutbackDeviceObjectByDeviceName(deviceName);
            if (devObj == null) return -1;
            // return the port number
            return (devObj as PortStatus).Port;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the first PortStatus object in the report of any Type
        /// </summary>
        public PortStatus GetFirstPortObjectOfAnyType()
        {
            if (devstatus == null) return null;
            if (devstatus.ports == null) return null;
            foreach (PortStatus portObj in devstatus.ports)
            {
                return portObj;
            }
            return null;
        }

        public PortStatus PrimaryFNDCPort { get => primaryFNDCPort; set => primaryFNDCPort = value; }
        public List<PortStatus> ChargeControllerDevices { get => chargeControllerDevices; set => chargeControllerDevices = value; }
        public List<PortStatus> InverterDevices { get => inverterDevices; set => inverterDevices = value; }
        public bool DataHasBeenWrittenToDB { get => dataHasBeenWrittenToDB; set => dataHasBeenWrittenToDB = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the lastReportJSON. Will never get or set null
        /// </summary>
        public string LastReportJSON
        {
            get
            {
                if (lastReportJSON == null) lastReportJSON = "";
                return lastReportJSON;
            }
            set
            {
                lastReportJSON = value;
                if (lastReportJSON == null) lastReportJSON = "";
            }
        }

        // these markers are used to enable the sql query to pick up historical records 
        // in a fast and efficient way
        public bool FiveMinuteMarker { get; set; }
        public bool TenMinuteMarker { get; set; }
        public bool FifteenMinuteMarker { get; set; }
        public bool ThirtyMinuteMarker { get; set; }
        public bool HourMarker { get; set; }
        public bool DayMarker { get; set; }

        public int KimonoMajorVer { get; set; }
        public int KimonoMinorVer { get; set; }
        public PortStatus Mate3sPort { get => mate3sPort; set => mate3sPort = value; }
        public PortStatus UserDataPort { get => userDataPort; set => userDataPort = value; }
    }

}

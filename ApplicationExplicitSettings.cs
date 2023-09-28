using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;


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
    /// A class to persist the explicitly set settings of the Kimono app.
    /// </summary>
    [KnownType(typeof(MonitorBlockProperties_Text))]
    [KnownType(typeof(MonitorBlockProperties_Base))]
    [DataContract]
    public sealed class ApplicationExplicitSettings 
    {

        [DataMember]
        private BindingList<MonitorBlockProperties_Base> summaryTabBlockList = null;
        [DataMember]
        private BindingList<MonitorBlockProperties_Base> userDefBlockList = null;
        [DataMember]
        private int heartBeatUpdateInterval_Sec = frmMain.DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC;
        [DataMember]
        private int databaseWriteTimeInterval_Sec = frmMain.DEFAULT_DATABASE_WRITE_INTERVAL_SEC;
        [DataMember]
        private string ipAddress = frmMain.DEFAULT_IP_ADDRESS;
        [DataMember]
        private bool disableMonitorBlockRefresh = frmMain.DEFAULT_DISABLE_MONITOR_BLOCK_REFRESH;
        [DataMember]
        private bool disableDBWrites = frmMain.DEFAULT_DISABLE_DB_WRITES;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/sets the list of summary tab monitor blocks. Will never get/set a null
        /// </summary>
        public string IPAddress
        {
            get
            {
                if ((ipAddress == null) || (ipAddress.Length==0)) ipAddress = frmMain.DEFAULT_IP_ADDRESS;
                return ipAddress;
            }
            set
            {
                ipAddress = value;
                if ((ipAddress == null) || (ipAddress.Length == 0)) ipAddress = frmMain.DEFAULT_IP_ADDRESS;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/sets the list of summary tab monitor blocks. Will never get/set a null
        /// </summary>
        public BindingList<MonitorBlockProperties_Base> SummaryTabBlockList
        {
            get
            {
                if (summaryTabBlockList == null) summaryTabBlockList = new BindingList<MonitorBlockProperties_Base>();
                return summaryTabBlockList;
            }
            set
            {
                summaryTabBlockList = value;
                if (summaryTabBlockList == null) summaryTabBlockList = new BindingList<MonitorBlockProperties_Base>();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/sets the list of user defined monitor blocks. Will never get/set a null
        /// </summary>
        public BindingList<MonitorBlockProperties_Base> UserDefBlockList
        {
            get
            {
                if (userDefBlockList == null) userDefBlockList = new BindingList<MonitorBlockProperties_Base>();
                return userDefBlockList;
            }
            set
            {
                userDefBlockList = value;
                if (userDefBlockList == null) userDefBlockList = new BindingList<MonitorBlockProperties_Base>();
            }
        }

        public int HeartBeatUpdateInterval_Sec { get => heartBeatUpdateInterval_Sec; set => heartBeatUpdateInterval_Sec = value; }

        public int DatabaseWriteTimeInterval_Sec { get => databaseWriteTimeInterval_Sec; set => databaseWriteTimeInterval_Sec = value; }
        public bool DisableMonitorBlockRefresh { get => disableMonitorBlockRefresh; set => disableMonitorBlockRefresh = value; }
        public bool DisableDBWrites { get => disableDBWrites; set => disableDBWrites = value; }
    }
}


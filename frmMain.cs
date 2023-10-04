using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Xml;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml.Serialization;
using OISCommon;
using org.matheval;

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
    /// The main form for the Kimono application
    /// </summary>
    public partial class frmMain : frmOISBase
    {

        private const string DEFAULT_CONFIG_FILENAME = "KimonoSettings.xml";

        // app constants
#if DEBUG
        // we give it a different directory in DEBUG for logging and settings
        public const string APPLICATION_NAME = "KimonoX";
#else
        public const string APPLICATION_NAME = "Kimono";
#endif
        public const int APPLICATIION_MAJOR_VERSION = 0;
        public const int APPLICATIION_MINOR_VERSION = 90;
        public const string APPLICATION_VERSION = "00.91 beta";
        private const string APPLICATION_HOME = @"http://www.OfItselfSo.com/Kimono/Kimono.php";

        private const string WARN01 = "The Kimono software is released under the MIT License. There";
        private const string WARN02 = "is no warranty or guarantee that the softwre or the information";
        private const string WARN03 = "it displays is without error. You use it, entirely at your own";
        private const string WARN04 = "risk. In particular: ";
        private const string WARN05 = "";
        private const string WARN06 = "THE KIMONO SOFTWARE, AND THE INFORMATION IT DISPLAYS, ARE ";
        private const string WARN07 = "PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR ";
        private const string WARN08 = "IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF ";
        private const string WARN09 = "MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND ";
        private const string WARN10 = "NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT ";
        private const string WARN11 = "HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, ";
        private const string WARN12 = "WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING ";
        private const string WARN13 = "FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR ITS OUTPUT";
        private const string WARN14 = "OR FROM THE USE OR OTHER DEALINGS IN THE SOFTWARE. ";

        private const int MB_BLOCK_WIDTH = 200;
        private const int MB_BLOCK_HEIGHT = 100;
        private const int MB_BLOCK_SEPARATION = 10;

#if DEBUG
        // the full path of the source code repo during development. We pick and place the 
        // preset monitor blocks from here.
        public const string LOCAL_SOURCE_CODE_REPO = @"C:\Projects\Kimono\Kimono_GitRepo";
#endif
        // this is the default directory into which we write all output such as setting, logfile, db etc
        string outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), APPLICATION_NAME);

        public const string DEFAULT_IP_ADDRESS = "192.168.0.64";
        private const string OUTBACK_STATISTICS_URLPATH = "/Dev_status.cgi?&Port=0";

        // we don't want to spam the log file so we have these flags
        private bool canSendDataFetchError = true;
        private bool canSendJsonConvertError = true;
        private bool canSendDBWriteError = true;

        public const bool DEFAULT_DISABLE_MONITOR_BLOCK_REFRESH = false;
        public const bool DEFAULT_DISABLE_DB_WRITES = false;

        // this is the thread that fetches the data from the Mate3s
        public const int DEFAULT_DATABASE_WRITE_INTERVAL_SEC = 60;
        private Thread databaseThread = null;
        private IntervalRecorder markerRecorder = new IntervalRecorder();

        // this is the thread that updates the screen
        public const int DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC = 2;
        private Thread heartbeatThread = null;

        // this is the client that GETs the data from the Mate3s
        private HttpClient httpClient = null;

        // these are settings the user does not explicitly configure such as form size
        // or some boolean screen control states
        private ApplicationImplicitSettings implictUserSettings = null;
        // these are settings the user configures such as file managers
        private ApplicationExplicitSettings explictUserSettings = null;

        // a flag that stops the update of the summary screen
        private bool pauseSummaryScreenUpdates = false;
        private bool summaryScreenLayoutChanged = false;

        // makes the heartbeat and dbupdate threads not do anything
        private bool pauseAllNonFormThreads = false;

        private const int DEFAULT_NUM_MB_ROWS = 5;
        private const int DEFAULT_NUM_MB_COLS = 3;
        private int numberOfMBRows = DEFAULT_NUM_MB_ROWS;
        private int numberOfMBCols = DEFAULT_NUM_MB_COLS;

        // this is the diretory where our preset monitor blocks can be found
        public const string DEFAULT_PRESETS_SUBDIRECTORY = @"Presets";
        string presetDirPath = @".\" + DEFAULT_PRESETS_SUBDIRECTORY;

        // this allows us to be notified the subscriber that this ctl should be changed out for 
        // a new one - which can be of a different type
        public delegate void ReplaceMonitorBlockDelegate(ctlMonitorBlock_Base targetMBCtl, MonitorBlockProperties_Base newMBObj);
        // this allows us to delete a monitor block based on an incoming mb property object
        public delegate void DeleteMonitorBlockDelegate(MonitorBlockProperties_Base newMBObj);

        // if exceptions happen in the Data Worker threads these will go non-null. 
        // we stop working and present the exception to the user
        private Exception databaseWorkerThreadRethrowException = null;
        private bool databaseWorkerThreadExceptionHasBeenRethrown = false;
        private Exception heartbeatWorkerThreadRethrowException = null;
        private bool heartbeatWorkerThreadExceptionHasBeenRethrown = false;

        /// Note that the panels on the main form are actually just a normal tabcontrol with
        /// the tab header and buttons hidden. We bring the tabpages to the front with 
        /// regular buttons on the side. This is intended to reduce visual clutter.
        /// Credit: https://stackoverflow.com/questions/10316567/how-do-i-create-a-tab-control-with-no-tab-header-in-windows-form

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public frmMain() 
        {
            bool retBOOL = false;

            // set the current directory equal to the exe directory. We do this because
            // people can start from a link and if the start-in directory is not right
            // it can put the log file in strange places
            Directory.SetCurrentDirectory(Application.StartupPath);

            // set up the Singleton g_Logger instance.
            if (g_Logger == null)
            {
                // did not work, nothing will start say so now in a generic way
                OISMessageBox("Logger Class Failed to Initialize. Nothing will work well.");
                return;
            }
            // record this in the logger for everybodys use
            g_Logger.ApplicationMainForm = this;
            g_Logger.DefaultDialogBoxTitle = APPLICATION_NAME;
            try
            {
                // set the icon for this form and for all subsequent forms
                g_Logger.AppIcon = new Icon(GetType(), "kimono_icon.ico");
                this.Icon = new Icon(GetType(), "kimono_icon.ico");
            }
            catch (Exception)
            {
            }

            // Register the global error handler as soon as we can in Main
            // to make sure that we catch as many exceptions as possible
            // this is a last resort. All execeptions should really be trapped
            // and handled by the code.
            OISGlobalExceptions eh = new OISGlobalExceptions();
            Application.ThreadException += new ThreadExceptionEventHandler(eh.OnThreadException);

            // set the culture so our numbers convert consistently
            System.Threading.Thread.CurrentThread.CurrentCulture = g_Logger.GetDefaultCulture();

            InitializeComponent();

            // we need to make sure the output dir exists, if possible
            EnsureOutputDirExists();
            // set it as the saved settings dir now
            textBoxSavedSettingsDir.Text = OutputDir;

            // set up logging
            retBOOL = g_Logger.InitLogging(OutputDir, APPLICATION_NAME, false);

            if (retBOOL == false)
            {
                // did not work, nothing will start say so now in a generic way
                OISMessageBox("The log file failed to create. No log file will be recorded.");
            }
            // pump out the header
            g_Logger.EmitStandardLogfileheader(APPLICATION_NAME);
            LogMessage("");
            LogMessage("Version: " + APPLICATION_VERSION);
            LogMessage("");
            // tell the user the logfile dir
            textBoxLogFileDir.Text = Path.Combine(g_Logger.LogFileDirectory, g_Logger.LogFileName);

            // default a few settings
            DisableMonitorBlockRefresh = DEFAULT_DISABLE_MONITOR_BLOCK_REFRESH;
            DisableDBWrites = DEFAULT_DISABLE_DB_WRITES;

            // set the default update timings before we getthe settings 
            HeartBeatUpdateInterval_Sec = DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC;
            DatabaseWriteTimeInterval_Sec = DEFAULT_DATABASE_WRITE_INTERVAL_SEC;

            // create the initial monitor blocks. These are the ones the user sees
            // if they do not have any saved settings.
            CreateInitialMonitorBlocksOnSummaryTabPage();

            // at this point we are still dealing with the default summary screen
            // because we have not loaded the settings yet. Make sure each Monitor
            // block has a slot ID by default
            SetSlotIDOnAllBlocksOnSummaryTabPage();

            // populate the MBPicker UserDefined List Box
            PopulateMBPickerPresetsWithGenerics();

            // Set the preset monitor block directory now. This may get overridden
            // by the load of the settings. in Release mode this is always just a
            // subdirectory where the .exe file is located
            string exePath = Directory.GetCurrentDirectory();
            if ((exePath == null) || (exePath.Length == 0)) return;
            presetDirPath = Path.Combine(exePath, DEFAULT_PRESETS_SUBDIRECTORY);
#if DEBUG
            // in debug mode, during development, this is always hard coded to a 
            // subdirectory of the source code repository
            presetDirPath = Path.Combine(LOCAL_SOURCE_CODE_REPO, DEFAULT_PRESETS_SUBDIRECTORY);
#endif
            // set the default IP address
            IPAddress = DEFAULT_IP_ADDRESS;

            // do not do anything until reset
            pauseAllNonFormThreads = true;

            // set the default implicit user settings
            implictUserSettings = new ApplicationImplicitSettings();
            // for testing
            //implictUserSettings.OKWithDisclaimer = false;

            // now recover the last configuration settings - if saved, we only do this if 
            // the control key is not pressed. This allows the user to start with the
            // Control key pressed and reset to defaults
#if DEBUG
            // use shift in debug mode, interferes with the VS run keypress
            if ((Control.ModifierKeys & Keys.Shift) == 0)
#else
            if ((Control.ModifierKeys & Keys.Control) == 0)
#endif
            {
                try
                {
                    try
                    {
                        // we do not want to trigger user activated events when setting things
                        // up on startup
                        //suppressUserActivatedEvents = true;
                        // if we got here the above lines did not fail
                        MoveImplicitUserSettingsToScreen();
                        ReadExplictUserSettings(true);
                        MoveExplicitUserSettingsToScreen();
                    }
                    finally
                    {
                        //suppressUserActivatedEvents = false;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Error recovering previous application settings. Msg=" + ex.Message);
                }
            }

            // make sure every Monitor Block knows how to notify about changes
            SetReplaceMonitorBlockDelegateOnActiveBlocks();
            // make sure the MonitorBlock Picker knows how to do it as well
            ctlMonitorBlockPicker1.ReplaceMonitorBlock = new ReplaceMonitorBlockDelegate(HandleReplaceMonitorBlock);
            ctlMonitorBlockPicker1.DeleteMonitorBlock = new DeleteMonitorBlockDelegate(HandleDeleteMonitorBlock);

            // load the presets
            LoadAllPresets(presetDirPath);

            // create the DB Manager, at this point it is using default paths and names
            // and nothing is open. Note the db manager is stored down in the singleton
            // g_logger instance. This means anything that inherits from the various OISCommon
            // base classes has access to it
            g_Logger.g_dbManager = new DBManager(OutputDir);
            textBoxDBFilePathAndName.Text = DBManager.DBFilePathAndName;
            bool retBool = PrepareDatabase(DBManager);
            // this can go null, if the db could not open properly
            if (retBool == false)
            {
                g_Logger.g_dbManager = null; // set it explicitly in the logger base class
                textBoxStatusBar2.ForeColor = Color.Red;
                textBoxStatusBar2.Text = "An error happened. Please see the logfile!";
            }

            // create the http client, this is used to fetch data
            httpClient = new HttpClient();

            // start the outback data thread, this fetches the data from the Mate3s
            StartDatabaseThread();

            // start the heartbeat, this updates the screen
            StartHeartbeatThread();
            
            // reset this, it changes when the saved configuration is loaded
            summaryScreenLayoutChanged = false;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the IP address. This is stored in the IPAddressControl
        /// </summary>
        private string IPAddress
        {
            get
            {
                return ipAddressControl1.IPAddress;
            }
            set
            {
                ipAddressControl1.IPAddress = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The MarkerRecorder is used to figure out marker for the hour, day etc
        /// intervals on the incoming data. Never returns null
        /// </summary>
        public IntervalRecorder MarkerRecorder
        {
            get
            {
                if(markerRecorder==null) markerRecorder = new IntervalRecorder();
                return markerRecorder;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The DB Manager is stored down in the logger singleton instance at startup.
        /// There is only ever one of these. There is no set here.
        /// </summary>
        public DBManager DBManager
        {
            get
            {
                return (DBManager)g_Logger.g_dbManager;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the OutbackSystemReport container. This can be null if we 
        /// don't have one. The code must cope with this
        /// </summary>
        public OutbackSystem OutbackSystemReport
        {
            get
            {
                return (OutbackSystem)g_Logger.g_DataContainer;
            }
            set
            {
                g_Logger.g_DataContainer = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Check the output dir exists
        /// </summary>
        private void EnsureOutputDirExists()
        {
            // test to see if it exists
            if(Directory.Exists(OutputDir)==false)
            {
                // create it.
                Directory.CreateDirectory(OutputDir);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the output dir, will never return null or an invalid string
        /// Does not check for existence.
        /// </summary>
        public string OutputDir
        {
            get
            {
                // if not valid, set it to a valid default
                if ((outputDir == null) || (outputDir.Length < 6) || (Path.IsPathRooted(outputDir)==false)) 
                {
                    outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), APPLICATION_NAME);
                }
                return outputDir;
            }
            set
            {
                outputDir = value;
                if ((outputDir == null) || (outputDir.Length < 6) || (Path.IsPathRooted(outputDir) == false))
                {
                    outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), APPLICATION_NAME);
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Populates the MB Picker preset List box with the generics. 
        /// This is hard coded since we 
        /// know the Monitor Blocks we have. We put one of each type in.
        /// </summary>
        private void PopulateMBPickerPresetsWithGenerics()
        {
            LogMessage("Populating the UserDefined Monitor Blocks");
            List<MonitorBlockProperties_Base> mbList = new List<MonitorBlockProperties_Base>();
            // just run down what we know we have
            mbList.Add(new MonitorBlockProperties_Blank());
            mbList.Add(new MonitorBlockProperties_Graph());
            mbList.Add(new MonitorBlockProperties_Integral());
            mbList.Add(new MonitorBlockProperties_Number());
            mbList.Add(new MonitorBlockProperties_Text());
            this.ctlMonitorBlockPicker1.MBList_Preset = mbList;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Does everything necessary to read the explicit configuration settings
        /// </summary>
        /// <param name="bSilent">if true we do not tell the user on error</param>
        /// <returns>z success, nz fail</returns>
        private int ReadExplictUserSettings(bool bSilent)
        {
            LogMessage("ReadExplictUserSettings");

            string appPath = "";
            string errStr;
            XmlReader reader = null;

            // get the preset output directory
            appPath = OutputDir;

            try
            {
                string filePathAndName = Path.Combine(appPath, DEFAULT_CONFIG_FILENAME);
                // now read the configuration file
                reader = XmlReader.Create(filePathAndName);
                DataContractSerializer serializerIn = new DataContractSerializer(typeof(ApplicationExplicitSettings));
                ExplicitUserSettings = (ApplicationExplicitSettings)serializerIn.ReadObject(reader);
            }
            catch (Exception ex)
            {
                errStr = "Error reading configuration: " + ex.ToString();
                LogMessage(errStr);
                if (bSilent == false) OISMessageBox(errStr);
                return 100;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
            }
            return 0;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Does everything necessary to write the explict configuration settings
        /// </summary>
        /// <param name="bSilent">if true we do not tell the user on error</param>
        /// <returns>z success, nz fail</returns>
        private int WriteExplicitUserSettings(bool bSilent)
        {
            string appPath = "";
            string errStr;
            XmlWriter writer = null;

            // get the preset output directory
            appPath = OutputDir;
            try
            {
                string filePathAndName = Path.Combine(appPath, DEFAULT_CONFIG_FILENAME);
                // now write the configuration file
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                writer = XmlWriter.Create(filePathAndName, settings);
                DataContractSerializer serializer = new DataContractSerializer(typeof(ApplicationExplicitSettings));
                serializer.WriteObject(writer, ExplicitUserSettings);
            }
            catch (Exception ex)
            {
                errStr = "Error saving configuration: " + ex.ToString();
                LogMessage(errStr);
                if (bSilent == false) OISMessageBox(errStr);
                return 100;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
            }
            // reset this
            MarkAllExplictSettingsAsSaved();
            return 0;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Save the explicit configuration
        /// </summary>
        private void SetExplicitUserSettings()
        {
            // clear down the Explict User Settings field
            ExplicitUserSettings.SummaryTabBlockList.Clear();
            // get all of the summary screen monitor blocks.
            List<ctlMonitorBlock_Base> allMonitorBlocks = FindAllMonitorBlocksOnSummaryTabPage();
            // move the current settings in
            foreach (ctlMonitorBlock_Base blockObj in allMonitorBlocks)
            {
                ExplicitUserSettings.SummaryTabBlockList.Add(blockObj.Properties);
            }

            // clear down the Explict User Settings field
            ExplicitUserSettings.UserDefBlockList.Clear();
            // get all of the user defined monitor blocks.
            List<MonitorBlockProperties_Base> usrDefMonitorBlocks = ctlMonitorBlockPicker1.FindAllUserDefinedMonitorBlocks();
            // move the current settings in
            foreach (MonitorBlockProperties_Base mbObj in usrDefMonitorBlocks)
            {
                ExplicitUserSettings.UserDefBlockList.Add(mbObj);
            }

            // other settings
            ExplicitUserSettings.HeartBeatUpdateInterval_Sec = HeartBeatUpdateInterval_Sec;
            ExplicitUserSettings.DatabaseWriteTimeInterval_Sec = DatabaseWriteTimeInterval_Sec;
            ExplicitUserSettings.IPAddress = IPAddress;
            ExplicitUserSettings.DisableMonitorBlockRefresh = DisableMonitorBlockRefresh;
            ExplicitUserSettings.DisableDBWrites = DisableDBWrites;

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Mark all explict settings as saved so we don't keep reminding the user
        /// </summary>
        private void MarkAllExplictSettingsAsSaved()
        {
            LogMessage("MarkAllExplictSettingsAsSaved");

            // get all of the summary screen monitor blocks.
            List<ctlMonitorBlock_Base> allMonitorBlocks = FindAllMonitorBlocksOnSummaryTabPage();
            // reset each one
            foreach (ctlMonitorBlock_Base blockObj in allMonitorBlocks)
            {
                blockObj.PropertiesHaveChanged = false;
            }

            // reset the monitor block picker
            ctlMonitorBlockPicker1.ChangesHaveBeenMade = false;

            // reset this
            summaryScreenLayoutChanged = false;
            // set this to reality
            ExplicitUserSettings.HeartBeatUpdateInterval_Sec = HeartBeatUpdateInterval_Sec;
            ExplicitUserSettings.DatabaseWriteTimeInterval_Sec = DatabaseWriteTimeInterval_Sec;
            ExplicitUserSettings.IPAddress = IPAddress;
            ExplicitUserSettings.DisableMonitorBlockRefresh = DisableMonitorBlockRefresh;
            ExplicitUserSettings.DisableDBWrites = DisableDBWrites;

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Moves the explicit configuration settings from settings file to the screen
        /// </summary>
        private void MoveExplicitUserSettingsToScreen()
        {
            LogMessage("MoveExplicitUserSettingsToScreen");

            // Create the summary screen Monitor Blocks as configured in the explicit settings
            foreach (var mbObj in ExplicitUserSettings.SummaryTabBlockList)
            {
                // look in the properties and get the slotID
                ctlMonitorBlock_Base targetMBCtl = FindMonitorBlockOnSummaryTabPageBySlotID(mbObj.SlotID);
                if (targetMBCtl == null) continue;
                // this does the change
                HandleReplaceMonitorBlock(targetMBCtl, mbObj);
            }

            // Move the UsrDef Monitor Blocks as configured in the explicit settings
            foreach (var mbObj in ExplicitUserSettings.UserDefBlockList)
            {
                // just add it to the control
                ctlMonitorBlockPicker1.MBList_UserDefined.Add(mbObj);
            }
            ctlMonitorBlockPicker1.RefreshUserDefinedMBList();

            // other settings
            HeartBeatUpdateInterval_Sec = ExplicitUserSettings.HeartBeatUpdateInterval_Sec;
            DatabaseWriteTimeInterval_Sec = ExplicitUserSettings.DatabaseWriteTimeInterval_Sec;
            DisableMonitorBlockRefresh = ExplicitUserSettings.DisableMonitorBlockRefresh;
            DisableDBWrites = ExplicitUserSettings.DisableDBWrites;
            IPAddress = ExplicitUserSettings.IPAddress;

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detects if the configuration settings the user explicitly specifies have
        /// changed. The contents here need to be synced with the actions in
        /// SetUserImplicitUserSettings()
        /// </summary>
        private bool DoWeNeedToSaveExplicitUserSettings()
        {
            // Check the summary tab monitor blocks
            List<ctlMonitorBlock_Base> mbList = FindAllMonitorBlocksOnSummaryTabPage();
            foreach(ctlMonitorBlock_Base mbObj in mbList)
            {
                // we only need one
                if (mbObj.PropertiesHaveChanged == true) return true;
            }
            if (ExplicitUserSettings.HeartBeatUpdateInterval_Sec != HeartBeatUpdateInterval_Sec) return true;
            if (ExplicitUserSettings.DatabaseWriteTimeInterval_Sec != DatabaseWriteTimeInterval_Sec) return true;
            if (ExplicitUserSettings.IPAddress != IPAddress) return true;

            // other settings
            if (summaryScreenLayoutChanged == true) return true;
            if (ctlMonitorBlockPicker1.ChangesHaveBeenMade == true) return true;
            if (ExplicitUserSettings.DisableMonitorBlockRefresh != DisableMonitorBlockRefresh) return true;
            if (ExplicitUserSettings.DisableDBWrites != DisableDBWrites) return true;

            return false;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Returns the explicit user config settings object. Will never get or set null
        /// </summary>
        public ApplicationExplicitSettings ExplicitUserSettings
        {
            get
            {
                if (explictUserSettings == null) explictUserSettings = new ApplicationExplicitSettings();
                return explictUserSettings;
            }
            set
            {
                explictUserSettings = value;
                if (explictUserSettings == null) explictUserSettings = new ApplicationExplicitSettings();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Moves the implicit configuration settings from settings file to the screen
        /// </summary>
        private void MoveImplicitUserSettingsToScreen()
        {
            LogMessage("MoveImplicitUserSettingsToScreen");

            // implicit settings
            this.Size = ImplicitUserSettings.FormSize;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Sets the form settings which the user does not really specify. These
        /// are things like form size etc.
        /// </summary>
        private void SetImplicitUserSettings()
        {
            ImplicitUserSettings.FormSize = this.Size;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Returns the implicit user config settings object. Will never get or set null
        /// </summary>
        public ApplicationImplicitSettings ImplicitUserSettings
        {
            get
            {
                if (implictUserSettings == null) implictUserSettings = new ApplicationImplicitSettings();
                return implictUserSettings;
            }
            set
            {
                implictUserSettings = value;
                if (implictUserSettings == null) implictUserSettings = new ApplicationImplicitSettings();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Update the textBoxStatusBar2 in a threadsafe way
        /// 
        /// </summary>
        public void UpdateStatusBar1ThreadSafe(string updateStr)
        {
            // we must be on the form thread
            if (this.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateStatusBar1ThreadSafe(updateStr); };
                this.Invoke(safeWrite);
            }
            else
            {
                if (updateStr == null) updateStr = "";
                textBoxStatusBar1.Text = updateStr;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Update the textBoxStatusBar2 in a threadsafe way
        /// 
        /// </summary>
        public void UpdateStatusBar2ThreadSafe(string updateStr)
        {
            // we must be on the form thread
            if (this.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateStatusBar2ThreadSafe(updateStr); };
                this.Invoke(safeWrite);
            }
            else
            {
                if (updateStr == null) updateStr = "";
                textBoxStatusBar2.Text = updateStr;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Update the database with the latest outback data we have
        /// </summary>
        private void UpdateDatabaseWithLatestOutbackData()
        {
            // note we do not do error handling in here. This is done by the caller

            // get this we do not want it to go null in the middle of this 
            OutbackSystem workingReport = OutbackSystemReport;

            if (workingReport == null) return;
            // has it already been written. This can happen if we write quicker than we fetch new
            if (workingReport.DataHasBeenWrittenToDB == true) return;
            // this can be null if we did not open correctly
            if (DBManager == null) return;
            if (DBManager.ConnectionIsOpen() == false) return;

            // We are going to write this record, set the time markers now
            workingReport.SetDatabaseTimeMarkers(MarkerRecorder);

            if (workingReport.devstatus != null)
            {
                // set our marker flags now
                foreach (PortStatus portObj in workingReport.devstatus.ports)
                {
                    DBManager.WritePortStatusToDB(workingReport, portObj);
                    UpdateStatusBar2ThreadSafe("Last db update:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            workingReport.DataHasBeenWrittenToDB = true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Update the screen with the latest outback data we have. 
        /// 
        /// NOTE: we cannot update the screen from anything else than the main 
        /// form thread. Hence the Invoke checks at the start.
        /// </summary>
        public void UpdateScreenWithLatestOutbackData()
        {
            // do we want to do this 
            if (pauseSummaryScreenUpdates == true) return;

            // we must be on the form thread
            if (this.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateScreenWithLatestOutbackData(); };
                this.Invoke(safeWrite);
            }
            else
            {
                if (OutbackSystemReport != null)
                {

                    // this is the stuff we get from the devstatus nested object
                    if(OutbackSystemReport.devstatus != null)
                    {
                        // set the last report time
                        DateTime reportTime = Utils.FromEpochTime(OutbackSystemReport.devstatus.Sys_Time);
                        textBoxStatusBar1.ForeColor = Color.Black;
                        textBoxStatusBar1.Text = "Last screen update:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // find all the monitor blocks
                        List<ctlMonitorBlock_Base> blockList = FindAllMonitorBlocksOnSummaryTabPage();
                        // update all monitor blocks
                        UpdateAllMonitorBlocksOnSummaryTabPage(blockList);

                    }
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Updates the all monitor block controls on the summary tab. 
        /// 
        /// NOTE: it is assumed that we are on the form thread here. It is assumed
        /// that we have a relevant OutbackSystemReport and that this will not change
        /// while we are in here.
        /// </summary>
        /// <param name="blockList">a list of all monitor block controls</param>
        private void UpdateAllMonitorBlocksOnSummaryTabPage(List<ctlMonitorBlock_Base> blockList)
        {
            bool retBool;
            if (blockList == null) return;
            OutbackSystem workingOutBackData = OutbackSystemReport;
            if (workingOutBackData == null) return;

            foreach (ctlMonitorBlock_Base blockObj in blockList)
            {
                // get the properties from the object, This will tell us 
                // what to do with it. 
                MonitorBlockProperties_Base blockProperties = blockObj.Properties;

                // there can be various types of block object. We find out what we are 
                // dealing with.
                if ((blockProperties is MonitorBlockProperties_Graph) == true)
                {
                    // we are dealing with a display mb, get what we need.
                    string dataSource = (blockProperties as MonitorBlockProperties_Graph).DataSource;
                    if (dataSource == null) continue;
                    // strip off leading and trailing spaces
                    string workingExpression = dataSource.Trim();
                    //do we have a prefix which indicates we are to evaluate this dataname as an expression
                    double outVal = 0;
                    if (workingExpression.StartsWith(OutbackSystem.EVALUATE_DATA_PREFIX) == true)
                    {
                        // yes we do, evaluate the dataSource
                        retBool = EvaluateDataSourceAsExpression(workingOutBackData, dataSource, out outVal);
                        if (retBool == false) continue; // failed
                    }
                    else
                    { 
                        // get the data value from the object as a constant
                        retBool = workingOutBackData.GetDataFromOutbackSystemReportByDeviceAndFieldName_double(dataSource, out outVal);
                        // did we succeed?
                        if (retBool == false) continue;
                    }
                    // we have the value. Update the control
                    (blockProperties as MonitorBlockProperties_Graph).GraphValue = outVal;
                    (blockProperties as MonitorBlockProperties_Graph).GraphDateTime = DateTime.Now;
                    // sync the screen to the properties
                    blockObj.SyncData();
                }
                else if ((blockProperties is MonitorBlockProperties_Integral) == true)
                {
                    // we are dealing with a display mb, get what we need.
                    string dataSource = (blockProperties as MonitorBlockProperties_Integral).DataSource;
                    if (dataSource == null) continue;
                    // strip off leading and trailing spaces
                    string workingExpression = dataSource.Trim();
                    //do we have a prefix which indicates we are to evaluate this dataname as an expression
                    double outVal = 0;
                    if (workingExpression.StartsWith(OutbackSystem.EVALUATE_DATA_PREFIX) == true)
                    {
                        // yes we do, evaluate the dataSource
                        retBool = EvaluateDataSourceAsExpression(workingOutBackData, dataSource, out outVal);
                        if (retBool == false)
                        {
                            // flag this
                            (blockProperties as MonitorBlockProperties_Integral).NumberValueIsValid = false;
                            // sync the screen to the properties
                            blockObj.SyncData();
                            continue; // failed
                        }
                    }
                    else
                    {
                        // get the data value from the object as a constant
                        retBool = workingOutBackData.GetDataFromOutbackSystemReportByDeviceAndFieldName_double(dataSource, out outVal);
                        // did we succeed?
                        if (retBool == false)
                        {
                            // flag this
                            (blockProperties as MonitorBlockProperties_Integral).NumberValueIsValid = false;
                            // sync the screen to the properties
                            blockObj.SyncData();
                            continue; // failed
                        }
                    }

                   // we have the value. Update the control
                   (blockProperties as MonitorBlockProperties_Integral).NumberValue = outVal;
                    // flag this
                    (blockProperties as MonitorBlockProperties_Integral).NumberValueIsValid = true;
                    // sync the screen to the properties
                    blockObj.SyncData();
                }
                else if ((blockProperties is MonitorBlockProperties_Number)==true)
                {
                    // we are dealing with a display mb, get what we need.
                    string dataSource = (blockProperties as MonitorBlockProperties_Number).DataSource;
                    if (dataSource == null) continue;
                    // strip off leading and trailing spaces
                    string workingExpression = dataSource.Trim();
                    //do we have a prefix which indicates we are to evaluate this dataname as an expression
                    double outVal = 0;
                    if (workingExpression.StartsWith(OutbackSystem.EVALUATE_DATA_PREFIX) == true)
                    {
                        // yes we do, evaluate the dataSource
                        retBool = EvaluateDataSourceAsExpression(workingOutBackData, dataSource, out outVal);
                        if (retBool == false)
                        {
                            // flag this
                            (blockProperties as MonitorBlockProperties_Number).NumberValueIsValid = false;
                            // sync the screen to the properties
                            blockObj.SyncData();
                            continue; // failed
                        }
                    }
                    else
                    {
                        // get the data value from the object as a constant
                        retBool = workingOutBackData.GetDataFromOutbackSystemReportByDeviceAndFieldName_double(dataSource, out outVal);
                        // did we succeed?
                        if (retBool == false)
                        {
                            // flag this
                            (blockProperties as MonitorBlockProperties_Number).NumberValueIsValid = false;
                            // sync the screen to the properties
                            blockObj.SyncData();
                            continue; // failed
                        }
                    }

                    // we have the value. Update the control
                    (blockProperties as MonitorBlockProperties_Number).NumberValue = outVal;
                    // flag this
                    (blockProperties as MonitorBlockProperties_Number).NumberValueIsValid = true;
                    // sync the screen to the properties
                    blockObj.SyncData();
                }
                else if ((blockProperties is MonitorBlockProperties_Text) == true)
                {
                    // we are dealing with a display mb, get what we need.
                    string dataSource = (blockProperties as MonitorBlockProperties_Text).DataSource;
                    if (dataSource == null) continue;

                    // get the data value from the object
                    string outVal = "?";
                    retBool = workingOutBackData.GetDataFromOutbackSystemReportByDeviceAndFieldName_string(dataSource, out outVal);
                    // did we succeed?
                    if (retBool == false) continue;
                    // yes, we did. Update the control
                    (blockProperties as MonitorBlockProperties_Text).TextValue = outVal;
                    // sync the screen to the properties
                    blockObj.SyncData();
                }
                else
                {
                    // unknown, do nothing
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Evaluates a math expression involving variables.
        /// 
        /// Note: Uses the matheval library
        /// 
        /// </summary>
        /// <param name="workingOutBackData">the outbackdata to look in</param>
        /// <param name="dataSource">the hashed name of the device and field to access</param>
        /// <param name="retDouble">the data item is returned here, "" on fail</param>
        private bool EvaluateDataSourceAsExpression(OutbackSystem workingOutBackData, string dataSource, out double retDouble)
        {
            retDouble = 0;
            bool retBool=false;

            try
            {
                // strip off leading and trailing spaces
                string workingExpression = dataSource.Trim();
                // clean off the prefix which indicates we are to evaluate this dataname
                if (workingExpression.StartsWith(OutbackSystem.EVALUATE_DATA_PREFIX) == true) workingExpression = workingExpression.Remove(workingExpression.IndexOf(OutbackSystem.EVALUATE_DATA_PREFIX), OutbackSystem.EVALUATE_DATA_PREFIX.Length);
                // we use a DATASOURCE_DELIMTER of '#' in the dataSource to separate the device and the field. This (and pretty much all non-alphanum chars)
                // make the expression evaluator puke. We replace that character with a temporary alphanum one for the purposes of evaluation
                // we then can substitute it appropriately
                workingExpression = workingExpression.Replace(OutbackSystem.DATASOURCE_DELIMTER_STR, OutbackSystem.DATASOURCE_DELIMTER_ALPHANUM);
                // we use a USRREF_DELIMTER of '$' in the dataSource to indicate a UserReference . This (and pretty much all non-alphanum chars)
                // make the expression evaluator puke. We replace that character with a temporary alphanum one for the purposes of evaluation
                // we then can substitute it appropriately
                workingExpression = workingExpression.Replace(OutbackSystem.USRREF_DELIMTER_STR, OutbackSystem.USRREF_DELIMTER_ALPHANUM);

                // create an expression evaluator
                Expression expression = new Expression(workingExpression);
                // get the variables
                List<string> expVariables = expression.getVariables();
                if (expVariables == null) return false;
                foreach (string varStr in expVariables)
                {
                    // put the real delimiters back
                    string tmpVarStr = varStr.Replace(OutbackSystem.DATASOURCE_DELIMTER_ALPHANUM, OutbackSystem.DATASOURCE_DELIMTER_STR);
                    tmpVarStr = tmpVarStr.Replace(OutbackSystem.USRREF_DELIMTER_ALPHANUM, OutbackSystem.USRREF_DELIMTER_STR);
                    double outVal = 0;
                    if(tmpVarStr.Contains(OutbackSystem.USRREF_DELIMTER_STR)==true)
                    {
                        // the user is referencing the output of another Monitor Block
                        retBool = GetDataFromMonitorBlocksByUserRef_double(tmpVarStr, out outVal);
                    }
                    else
                    {
                        // assume this, get the value of the variable from the report
                        retBool = workingOutBackData.GetDataFromOutbackSystemReportByDeviceAndFieldName_double(tmpVarStr, out outVal);
                    }
                    // cannot resolve it, only takes one to render the expression unviable
                    if (retBool != true) return false;
                    // bind it
                    expression.Bind(varStr, outVal);
                }
                // evaluate the expression
                Object expValue = expression.Eval();
                // this had better be convertible or we get an exception
                retDouble = Convert.ToDouble(expValue);
                // all is well
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the data from the Monitor Blocks if the user is using one of the 
        /// UD... reserved reference names.
        /// 
        /// </summary>
        /// <param name="userDataPort">the port object we populate</param>
        private void PopulateUserDataPortWithData(PortStatus userDataPort)
        {
            PropertyInfo propertyInfo = null;

            if (userDataPort == null) return;
            // Check the summary tab monitor blocks, we just get the values from each
            // first matching one we find. If the user put in duplicates they just 
            // get overwritten
            List<ctlMonitorBlock_Base> mbList = FindAllMonitorBlocksOnSummaryTabPage();
            foreach (ctlMonitorBlock_Base mbObj in mbList)
            {
                // only certain types have userRefs
                if ((mbObj.Properties is MonitorBlockProperties_Integral) == true)
                {
                    string userRef = (mbObj.Properties as MonitorBlockProperties_Integral).UserReference;
                    // do we even have a valid output value?
                    if ((mbObj.Properties as MonitorBlockProperties_Integral).NumberValueIsValid == false) continue;
                    if (userRef == null) continue;
                    // we only accept userReferences that begin with a specific value
                    if (userRef.StartsWith(PortStatus.USERDB_STORE_NUMBER_PREFIX) == false) continue;
                    // ok we have a candidate, trye to set the data via reflection
                    propertyInfo = userDataPort.GetType().GetProperty(userRef);
                    // do we have that property in the userDataPort? This can be anything and the users
                    // can set it to a value for their own use.
                    if (propertyInfo == null) continue;
                    // we have that property, set the value now, note we pull the last display value here. Not the Number Value
                    propertyInfo.SetValue(userDataPort, (mbObj.Properties as MonitorBlockProperties_Integral).LastDisplayValue);
                }
                else if ((mbObj.Properties is MonitorBlockProperties_Number) == true)
                {
                    string userRef = (mbObj.Properties as MonitorBlockProperties_Number).UserReference;
                    // do we even have a valid output value?
                    if ((mbObj.Properties as MonitorBlockProperties_Number).NumberValueIsValid == false) continue;
                    if (userRef == null) continue;
                    // we only accept userReferences that begin with a specific value
                    if (userRef.StartsWith(PortStatus.USERDB_STORE_NUMBER_PREFIX) == false) continue;
                    // ok we have a candidate, trye to set the data via reflection
                    propertyInfo = userDataPort.GetType().GetProperty(userRef);
                    // do we have that property in the userDataPort? This can be anything and the users
                    // can set it to a value for their own use.
                    if (propertyInfo == null) continue;
                    // we have that property, set the value now
                    propertyInfo.SetValue(userDataPort, (mbObj.Properties as MonitorBlockProperties_Number).NumberValue);
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets a double data item from the an existing monitor block.
        /// 
        /// </summary>
        /// <param name="userRef">the user reference of the control access</param>
        /// <param name="outVal">the data item is returned here, "" on fail</param>
        /// <returns>true - success, false - fail</returns>
        public bool GetDataFromMonitorBlocksByUserRef_double(string userRef, out double outVal)
        {
            outVal = 0;

            // sanity checks
            if (userRef == null) return false;
            // remove the USRREF_DELIMTER_STR
            userRef = userRef.Replace(OutbackSystem.USRREF_DELIMTER_STR, "");
            if (userRef.Length == 0) return false;

            // Check the summary tab monitor blocks, we just get the value from the 
            // first matching one we find
            List<ctlMonitorBlock_Base> mbList = FindAllMonitorBlocksOnSummaryTabPage();
            foreach (ctlMonitorBlock_Base mbObj in mbList)
            {
                // only certain types have userRefs which return doubles
                if ((mbObj.Properties is MonitorBlockProperties_Integral) == true)
                {
                    // check this one
                    if ((mbObj.Properties as MonitorBlockProperties_Integral).UserReference == userRef)
                    {
                        outVal = (mbObj.Properties as MonitorBlockProperties_Integral).LastDisplayValue;
                        return true;
                    }
                }
                else if ((mbObj.Properties is MonitorBlockProperties_Number) == true)
                {
                    // check this one
                    if((mbObj.Properties as MonitorBlockProperties_Number).UserReference == userRef)
                    {
                        outVal = (mbObj.Properties as MonitorBlockProperties_Number).NumberValue;
                        return true;
                    }
                }
            }
            // not found
            return false;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Returns a list of all monitorBlock controls located on the Summary TabPage
        /// </summary>
        /// <returns>a list of all monitorBlock controls</returns>
        private List<ctlMonitorBlock_Base> FindAllMonitorBlocksOnSummaryTabPage()
        {
            List<ctlMonitorBlock_Base> outList = new List<ctlMonitorBlock_Base>();

            // just loop through them. We only update on this page so we don't
            // have to recursively check other forms and controls
            foreach (var controlObj in tabPageMonitorBlocks.Controls)
            {
                if ((controlObj is ctlMonitorBlock_Base) == true) outList.Add((ctlMonitorBlock_Base)controlObj);
            }
            return outList;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Finds the monitor block control by matching it to a slotID. There should 
        /// only be one match and we return the first one we find
        /// </summary>
        /// <param name="slotIDIn">the slotID string</param>
        /// <returns>the matching control or null</returns>
        private ctlMonitorBlock_Base FindMonitorBlockOnSummaryTabPageBySlotID(string slotIDIn)
        {
            if (slotIDIn == null) return null;
            if (slotIDIn.Length == 0) return null;
            foreach (var controlObj in tabPageMonitorBlocks.Controls)
            {
                // we only care about these kind of controls
                if ((controlObj is ctlMonitorBlock_Base) == false) continue;
                // compare the slotIDs, if they match return what we got
                if ((controlObj as ctlMonitorBlock_Base).Properties.SlotID == slotIDIn) return (ctlMonitorBlock_Base)controlObj;
            }
            return null;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Finds the monitor block control by matching its property object with a 
        /// reference equals. Not just the same content but the exact same object.
        /// </summary>
        /// <param name="mbObj">the property object we are looking for</param>
        /// <returns>the matching control or null</returns>
        private ctlMonitorBlock_Base FindMonitorBlockOnSummaryTabPageByProperty(MonitorBlockProperties_Base mbObj)
        {
            if (mbObj == null) return null;
            foreach (var controlObj in tabPageMonitorBlocks.Controls)
            {
                // we only care about these kind of controls
                if ((controlObj is ctlMonitorBlock_Base) == false) continue;
                // compare the properties, if they match return what we got
                if (Object.ReferenceEquals(mbObj, (controlObj as ctlMonitorBlock_Base).Properties) == true) return (ctlMonitorBlock_Base)controlObj;
            }
            return null;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Start up the heartbeat thread
        /// </summary>
        private void StartHeartbeatThread()
        {
            LogMessage("Starting Heartbeat Worker thread");
            if (heartbeatThread != null) StopHeartbeatThread();

            heartbeatThread = new Thread(HeartBeatWorker);
            heartbeatThread.Start();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Stop the heartbeat thread
        /// </summary>
        private void StopHeartbeatThread()
        {
            LogMessage("Stopping Heartbeat Worker thread");
            if (heartbeatThread == null) return;
            try
            {
                heartbeatThread.Abort();
            }
            catch { }
            heartbeatThread = null;            
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the heartbeat worker update time
        /// </summary>
        private int HeartBeatUpdateInterval_Sec
        {
            get
            {
                try
                {
                    // has to be an integer
                    int val = Convert.ToInt32(textBoxHeartbeatUpdateTimeSec.Text);
                    if (val <= 0)
                    {
                        // we do not let it be less than zero
                        textBoxHeartbeatUpdateTimeSec.Text = DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC.ToString();
                        return DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC;
                    }
                    else return val;
                }
                catch
                {
                    // an error happened, return default and reset
                    textBoxHeartbeatUpdateTimeSec.Text = DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC.ToString();
                    return DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC;
                }
            }
            set
            {
                if (value <= 0) value = DEFAULT_HEARTBEAT_UPDATE_INTERVAL_SEC;
                // convert what we got to a string
                textBoxHeartbeatUpdateTimeSec.Text = value.ToString();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the database update time interval
        /// </summary>
        private int DatabaseWriteTimeInterval_Sec
        {
            get
            {
                try
                {
                    // has to be an integer
                    int val = Convert.ToInt32(textBoxDatabaseWriteTimeInterval_Sec.Text);
                    if (val <= 0)
                    {
                        // we do not let it be less than zero
                        textBoxDatabaseWriteTimeInterval_Sec.Text = DEFAULT_DATABASE_WRITE_INTERVAL_SEC.ToString();
                        return DEFAULT_DATABASE_WRITE_INTERVAL_SEC;
                    }
                    else return val;
                }
                catch
                {
                    // an error happened, return default and reset
                    textBoxDatabaseWriteTimeInterval_Sec.Text = DEFAULT_DATABASE_WRITE_INTERVAL_SEC.ToString();
                    return DEFAULT_DATABASE_WRITE_INTERVAL_SEC;
                }
            }
            set
            {
                if (value <= 0) value = DEFAULT_DATABASE_WRITE_INTERVAL_SEC;
                // convert what we got to a string
                textBoxDatabaseWriteTimeInterval_Sec.Text = value.ToString();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The heartbeat worker thread, handles periodic things
        /// </summary>
        public void HeartBeatWorker()
        {
            string workingJSONReport = "";

            LogMessage("Heartbeat Worker Starts");

            while (true)
            {
                Thread.Sleep(HeartBeatUpdateInterval_Sec * 1000);

                if (DisableMonitorBlockRefresh == true) continue;
                if (pauseAllNonFormThreads == true) continue;

                if (heartbeatWorkerThreadRethrowException != null)
                {
                    // we only do this once
                    if (heartbeatWorkerThreadExceptionHasBeenRethrown == false)
                    {
                        LogMessage("Exception detected, attempting to rethrow");
                        RethrowExceptionOnMainThread(heartbeatWorkerThreadRethrowException);
                        // we do not re-set, any unhandled error in here means stop working
                        // if we stop working in this way the user has a chance to save their work
                        //heartbeatWorkerThreadRethrowException = null;
                        heartbeatWorkerThreadExceptionHasBeenRethrown = true;
                    }
                    continue;
                }

                // we do not do ANYTHING if the Disclaimer has not been agreed to
                if (ImplicitUserSettings.OKWithDisclaimer == false) continue;

                try
                {
                    // clear it down
                    OutbackSystemReport = null;
                    // fetch the outback data as JSON, traps its own errors
                    workingJSONReport = GetOutBackData();
                    if ((workingJSONReport != null) && (workingJSONReport.Length > 0))
                    {
                        // convert into nested objects, but do not make it globally available yet
                        OutbackSystem tmpReport = JsonSerializer.Deserialize<OutbackSystem>(workingJSONReport);
                        // set a few things
                        tmpReport.KimonoMajorVer = APPLICATIION_MAJOR_VERSION;
                        tmpReport.KimonoMinorVer = APPLICATIION_MINOR_VERSION;
                        // record the JSON
                        tmpReport.LastReportJSON = workingJSONReport;
                        // post process the report
                        tmpReport.PostProcess();
                        // after post processing the report now has a UserDataPort object, we populate it
                        PopulateUserDataPortWithData(tmpReport.UserDataPort);
                        // make it available
                        OutbackSystemReport = tmpReport;
                        canSendJsonConvertError = true;
                    }
                    else
                    {
                        // we do not want to spam the logfile so we only emit an error if we 
                        // have had a successful convert
                        if (canSendJsonConvertError == true)
                        {
                            canSendJsonConvertError = false;
                            LogMessage("Could not process data from Mate3s");
                            UpdateStatusBar1ThreadSafe("Error accessing Mate3s. Please see the log file.");
                        }
                    }

                    // update the screen
                    UpdateScreenWithLatestOutbackData();
                }
                catch (JsonException jex)
                {
                    // log it
                    LogMessage("JSON exception happened" + jex.Message);
                    if(workingJSONReport!=null)
                    {
                        LogMessage("JSON is " + workingJSONReport);
                    }
                    UpdateStatusBar1ThreadSafe("Error reading data from Mate3s");
                    // note we do NOT rethrow here
                }
                catch (Exception ex)
                {
                    // log it
                    LogMessage("Heartbeat exception happened" + ex.Message);
                    heartbeatWorkerThreadRethrowException = ex;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Start the database worker thread
        /// </summary>
        private void StartDatabaseThread()
        {
            LogMessage("Starting the DatabaseWorker thread");
            if (databaseThread != null) StopDatabaseThread();

            databaseThread = new Thread(DatabaseWorker);
            databaseThread.Start();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Stop the database worker thread
        /// </summary>
        private void StopDatabaseThread()
        {
            LogMessage("Stopping DatabaseWorker thread");
            if (databaseThread == null) return;
            try
            {
                databaseThread.Abort();
            }
            catch { }
            databaseThread = null;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// The database worker thread
        /// </summary>
        public void DatabaseWorker()
        {
            LogMessage("Database Worker Starts");

            while (true)
            {
                Thread.Sleep(DatabaseWriteTimeInterval_Sec * 1000);

                if (DisableDBWrites == true) continue;
                if (pauseAllNonFormThreads == true) continue;

                if (databaseWorkerThreadRethrowException != null)
                {
                    // we only do this once
                    if (databaseWorkerThreadExceptionHasBeenRethrown == false)
                    {
                        LogMessage("Exception detected, attempting to rethrow");
                        RethrowExceptionOnMainThread(databaseWorkerThreadRethrowException);
                        // we do not re-set, any unhandled error in here means stop working
                        // if we stop working in this way the user has a chance to save their work
                        //databaseWorkerThreadRethrowException = null;
                        databaseWorkerThreadExceptionHasBeenRethrown = true;
                    }
                    continue;
                }

                try
                {
                    // update the database
                    UpdateDatabaseWithLatestOutbackData();
                    // we had a successful write
                    canSendDBWriteError = true;

                }
                catch (Exception ex)
                {
                    // we do not want to spam the log so we only do this if we have 
                    // had a successful write
                    if (canSendDBWriteError == true)
                    {
                        canSendDBWriteError = false;
                        // log it
                        LogMessage("DatabaseWorker exception happened" + ex.Message);
                        // typically the inner exceptions contain more information
                        if (ex.InnerException != null)
                        {
                            LogMessage("Error: " + ex.InnerException.Message);
                        }
                        databaseWorkerThreadRethrowException = ex;
                    }
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Rethrows an unhandled exception occuring in a thread in a handler
        /// on the main form
        /// </summary>
        private void RethrowExceptionOnMainThread(Exception ex)
        {
            // we must be in the form thread. 
            if (this.InvokeRequired)
            {
                Action safeWrite = delegate { RethrowExceptionOnMainThread(ex); };
                this.Invoke(safeWrite);
            }
            else
            {
                // log it
                LogMessage(" RethrowExceptionOnMainThread called");
                // throw it on main form thread
                OISGlobalExceptions eh = new OISGlobalExceptions();
                eh.HandleThreadException(ex);
                textBoxStatusBar2.Text = "An error happened. Please see the logfile!";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Get the JSON data from the Outback Mate3s
        /// </summary>
        public string GetOutBackData()
        {

            string urlString = "";
            try
            {
                // compose the string
                urlString = "http://" + IPAddress + OUTBACK_STATISTICS_URLPATH;
                Task<string> t = httpClient.GetStringAsync(urlString);
                string checkResult = t.Result;
                // we were successful. It is now possible to resend these
                canSendDataFetchError = true;
                return checkResult;
            }
            catch (Exception ex)
            {
                if (canSendDataFetchError)
                {
                    // do not send any more repeated messages until we get a successful connect
                    canSendDataFetchError = false;
                    LogMessage("Exception:" + ex.Message);
                    LogMessage("Attempted to fetch data from URL>" + urlString + "<");
                    // typically the inner exceptions contain more information
                    if (ex.InnerException != null)
                    {
                        LogMessage("Error: " + ex.InnerException.Message);
                    }
                }
                return "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Shows the disclaimer if it has not already been shown
        /// </summary>
        private void ShowDisclaimer()
        {
            // we show a message once regarding the license
            if (implictUserSettings != null)
            {
                if (implictUserSettings.OKWithDisclaimer == false)
                {
                    frmDisclaimer disFrm = new frmDisclaimer(APPLICATION_VERSION);
                    disFrm.ShowDialog();
                    implictUserSettings.OKWithDisclaimer = disFrm.OKWithDisclaimer;
                    if (implictUserSettings.OKWithDisclaimer == false)
                    {
                        OISMessageBox("This application will now close");
                        this.Close();
                    }
                    else
                    {
                        // they are ok with the disclaimer ask about the IP address
                        frmGetIPAddress frmIPAddr = new frmGetIPAddress();
                        frmIPAddr.IPAddress = this.IPAddress;
                        frmIPAddr.ShowDialog();
                        if(frmIPAddr.DialogResult == DialogResult.OK)
                        {
                            // set the IP address
                            IPAddress = frmIPAddr.IPAddress;
                        }
                        // let the heartbeat and update db worker start
                        pauseAllNonFormThreads = false;
                    }
                }
                else
                {
                    // let the heartbeat and update db worker start
                    pauseAllNonFormThreads = false;
                }
            }
            else
            {
                frmDisclaimer disFrm = new frmDisclaimer(APPLICATION_VERSION);
                disFrm.ShowDialog();
                if (disFrm.OKWithDisclaimer == false)
                {
                    OISMessageBox("This application will now close");
                    this.Close();
                }
                else
                {
                    // they are ok with the disclaimer ask about the IP address
                    frmGetIPAddress frmIPAddr = new frmGetIPAddress();
                    frmIPAddr.IPAddress = this.IPAddress;
                    frmIPAddr.ShowDialog();
                    if (frmIPAddr.DialogResult == DialogResult.OK)
                    {
                        // set the IP address
                        IPAddress = frmIPAddr.IPAddress;
                    }
                    // let the heartbeat and update db worker start
                    pauseAllNonFormThreads = false;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Trigger when the form is shown
        /// </summary>
        private void frmMain_Shown(object sender, EventArgs e)
        {
            ShowDisclaimer();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Trigger on the form closing
        /// </summary>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlgResult = OISMessageBox_YesNo("Do you really want to close Kimono?");
            if(dlgResult != DialogResult.Yes)
            {
                // user cancelled
                e.Cancel = true;
                return;
            }


            // this does it all
            Shutdown();

            // put the non user specified configuration settings in place now
            SetImplicitUserSettings();

            // we always save implicit settings on close, unless the Control key is pressed
#if DEBUG
            // use shift in debug mode, interferes with the VS run keypress
            if ((Control.ModifierKeys & Keys.Shift) == 0)
#else
            if ((Control.ModifierKeys & Keys.Control) == 0)
#endif
            {
                LogMessage("frmMain1_FormClosing close ImplicitUserSettings.Save called");
                ImplicitUserSettings.Save();
            }
            if (DoWeNeedToSaveExplicitUserSettings() == true)
            {
                DialogResult dlgRes = OISMessageBox_YesNo("The configuration options have changed.\n\nDo you wish to save them?");
                if (dlgRes == DialogResult.Yes)
                {
                    LogMessage("frmMain1_FormClosing close SetUserImplicitUserSettings() called");
                    SetExplicitUserSettings();
                    WriteExplicitUserSettings(false);
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Shutdown gracefully
        /// </summary>
        private void Shutdown()
        {
            LogMessage("Shutting down");

            try
            {
                // Stop the heartbeat thread
                StopHeartbeatThread();
                // Stop the outback data thread
                StopDatabaseThread();

                // Dispose the http client
                if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }

                // close the database
                if(DBManager != null) DBManager.CloseConnection();
            }
            catch (Exception ex)
            {
                LogMessage("Exception when closing down");
                LogMessage(ex.Message);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the lastOutbackSystemReportJSON. Will never get or set null
        /// </summary>
        private string LastOutbackSystemReportJSON
        {
            get
            {
                if (OutbackSystemReport == null) return "";
                if (OutbackSystemReport.LastReportJSON == null) OutbackSystemReport.LastReportJSON = "";
                return OutbackSystemReport.LastReportJSON;
            }
            set
            {
                if (OutbackSystemReport == null) return;
                OutbackSystemReport.LastReportJSON = value;
                if (OutbackSystemReport.LastReportJSON == null) OutbackSystemReport.LastReportJSON = "";
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the number of MB rows on the summary screen
        /// </summary>
        public int NumberOfMBRows { get => numberOfMBRows; set => numberOfMBRows = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the number of MB cols on the summary screen
        /// </summary>
        public int NumberOfMBCols { get => numberOfMBCols; set => numberOfMBCols = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a click on the about button
        /// </summary>
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            frmAbout aboutForm = new frmAbout(APPLICATION_VERSION);
            aboutForm.ShowDialog();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a click on the show raw JSON button
        /// </summary>
        private void buttonShowRawJSON_Click(object sender, EventArgs e)
        {
            frmRawJSON jsonForm = new frmRawJSON(LastOutbackSystemReportJSON);
            jsonForm.ShowDialog();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a click on the Summary tab activation button
        /// </summary>
        private void buttonMonitorBlocksTab_Click(object sender, EventArgs e)
        {
            // Bring the Summary Tab to the front
            tabControlMainForm.SelectedTab = tabControlMainForm.TabPages["tabPageMonitorBlocks"];
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a click on the Setup tab activation button
        /// </summary>
        private void buttonSetupTab_Click(object sender, EventArgs e)
        {
            /// Bring the DPlot1 to the front
            tabControlMainForm.SelectedTab = tabControlMainForm.TabPages["tabPageSystemConfiguration"];
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Prepare and open the database for use
        /// </summary>
        /// <param name="DBManagerIn">the db manager we use to prepare and open</param>
        /// <returns>true - all is well, false - error happened</returns>
        private bool PrepareDatabase(DBManager DBManagerIn)
        {
            LogMessage("PrepareDatabase");
            bool retBool;

            // sanity check, should never happen
            if (DBManagerIn == null)
            {
                LogMessage("No DB Manger, cannot proceed");
                return false;
            }

            // make sure the dir exists
            retBool = DBManagerIn.EnsureDBDirExists();
            if (retBool == false)
            {
                // the connection did not open, remove the DBManager
                LogMessage("DB dir invalid and could not be fixed, closing the db manager");
                return false;
            }

            // open the db file, this also creates it if it does not exist
            retBool = DBManagerIn.OpenConnection();
            if(retBool == false)
            {
                // the connection did not open, remove the DBManager
                LogMessage("DB coonnection could not open, closing the db manager");
                return false;
            }

            // check if the tables exist and create them if they do not
            retBool = DBManagerIn.CheckTablesExistAndCreateIfNot();
            if (retBool == false)
            {
                // the tables did not create, remove the DBManager
                LogMessage("DB tables do not exist and would not create, closing the db manager");
                return false;
            }
            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the save configuration button
        /// </summary>
        private void buttonSaveSetup_Click(object sender, EventArgs e)
        {
            SetExplicitUserSettings();
            // write out the settings
            WriteExplicitUserSettings(false);
            // read the settings 
            ReadExplictUserSettings(false);
        }


        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Loads all presets
        /// </summary>
        /// <param name="presetDirPath">the dir path to the presets</param>
        private void LoadAllPresets(string presetDirPath)
        {
            LogMessage("LoadAllPresets");
            List<MonitorBlockProperties_Base> presetList = new List<MonitorBlockProperties_Base>();

            // sanity checks
            if ((presetDirPath == null) || (presetDirPath.Length == 0))
            {
                LogMessage("Null or empty preset directory");
                return;
            }
            LogMessage("LoadAllPresets preset dir is >"+ presetDirPath+ "<");
            // the dir has to exist
            if (Directory.Exists(presetDirPath) == false)
            {
                LogMessage("Invalid preset dir>" + presetDirPath+"<");
                return;
            }
            // now read all files
            FileInfo[] files = null;
            try
            {
                DirectoryInfo d = new DirectoryInfo(presetDirPath); 
                files = d.GetFiles("*.xml"); 
            }
            catch (Exception ex)
            {
                LogMessage("Error reading preset directory" + ex.Message);
                return;
            }
            try
            {
                // now loop through the list
                foreach (FileInfo fileInfo in files)
                {
                    // now loop through the list
                    string xmlText = File.ReadAllText(fileInfo.FullName);
                    // cannot figure out how to make xml deserialize by specifing the base class
                    // so we have a series of if..else statements here. Mildly annoying
                    XmlSerializer ser = null;
                    if (xmlText.Contains("MonitorBlockProperties_Blank"))
                    {
                        ser = new XmlSerializer(typeof(MonitorBlockProperties_Blank));
                    }
                    else if (xmlText.Contains("MonitorBlockProperties_Graph"))
                    {
                        ser = new XmlSerializer(typeof(MonitorBlockProperties_Graph));
                    }
                    else if (xmlText.Contains("MonitorBlockProperties_Integral"))
                    {
                        ser = new XmlSerializer(typeof(MonitorBlockProperties_Integral));
                    }
                    else if (xmlText.Contains("MonitorBlockProperties_Number"))
                    {
                        ser = new XmlSerializer(typeof(MonitorBlockProperties_Number));
                    }
                    else if (xmlText.Contains("MonitorBlockProperties_Text"))
                    {
                        ser = new XmlSerializer(typeof(MonitorBlockProperties_Text));
                    }
                    else
                    {
                        // don't know what to do so we do nothing
                        continue;
                    }

                    // deserialize and create the monitor block object
                    using (FileStream reader = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        MonitorBlockProperties_Base mbObj = (MonitorBlockProperties_Base)ser.Deserialize(reader);
                        // now add it to the preset list
                        presetList.Add(mbObj);
                    }
                 }
            }
            catch (Exception ex)
            {
                LogMessage("Error reading preset file");
                LogMessage(ex.Message);
            }
            // send the list of presets over to the picker object
            ctlMonitorBlockPicker1.MBList_Preset.AddRange(presetList);
            // ensure it displays
            ctlMonitorBlockPicker1.RefreshPresetMBList();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Composes a slotID from the on screen coordinates
        /// </summary>
        /// <param name="x">the screen x coord</param>
        /// <param name="y">the screen y coord</param>
        private string GetSlotIDFromScreenCoords(int x, int y)
        {
            if (x < 0) return MonitorBlockProperties_Base.DEFAULT_SLOTID;
            if (y < 0) return MonitorBlockProperties_Base.DEFAULT_SLOTID;
            // it is a very simple algorythm
            return (y / 110).ToString() + "x" + (x / 210).ToString();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle the change of a monitor block on the Summary panel to another 
        /// type. Typically this gets called when a new monitor block is dropped on
        /// an existing one. Also called when restoring the summary screen from saved
        /// settings
        /// </summary>
        /// <param name="targetMBCtl">the old control. this should be on the summary panel</param>
        /// <param name="newMBObj">the properties of the new monitor block</param>
        public void HandleReplaceMonitorBlock(ctlMonitorBlock_Base targetMBCtl, MonitorBlockProperties_Base newMBObj)
        {
            LogMessage("HandleReplaceMonitorBlock called");
            ctlMonitorBlock_Base newMBCtl = null;


            // sanity checks
            if (targetMBCtl == null) return;
            if (newMBObj == null) return;

            // we must be in the form thread. 
            if (this.InvokeRequired)
            {
                Action safeWrite = delegate { HandleReplaceMonitorBlock(targetMBCtl, newMBObj); };
                this.Invoke(safeWrite);
            }
            else
            {

                try
                {
                    // we must stop the update of the screen
                    pauseSummaryScreenUpdates = true;

                    // check that the targetMBCtl is actually on the summary panel
                    if (tabPageMonitorBlocks.Controls.Contains(targetMBCtl) == false) return;
                    // are we dropping on ourself, if so ignore
                    if (Object.ReferenceEquals(targetMBCtl.Properties, newMBObj)) return;
                    // does the incoming newMBObj actually represent an MB already on the summary tab?
                    // if so then we are swapping, not creating new and replacing. 
                    // find the original control (if it exists)
                    ctlMonitorBlock_Base sourceMBCtl = FindMonitorBlockOnSummaryTabPageByProperty(newMBObj);
                    // I was going to make pressing the control key duplicate the source onto the target, not
                    //  swap but that is too dangerous. We just always swap
                    //if ((sourceMBCtl != null) && ((Control.ModifierKeys & Keys.Control) == 0))
                    if (sourceMBCtl != null)
                    {
                        // we are swapping, not replacing
                        Point oldLocation = targetMBCtl.Location;
                        Size oldSize = targetMBCtl.Size;
                        string oldSlotID = targetMBCtl.Properties.SlotID;

                        // now the first replace
                        targetMBCtl.Location = sourceMBCtl.Location;
                        targetMBCtl.Size = sourceMBCtl.Size;
                        targetMBCtl.Properties.SlotID = sourceMBCtl.Properties.SlotID;
                        // now the swap
                        sourceMBCtl.Location = oldLocation;
                        sourceMBCtl.Size = oldSize;
                        sourceMBCtl.Properties.SlotID = oldSlotID;

                        // reset this always
                        sourceMBCtl.Properties.WantUsrPickerDeleteOnDrop = false;
                        targetMBCtl.Properties.WantUsrPickerDeleteOnDrop = false;
                        // flag this
                        summaryScreenLayoutChanged = true;
                        return;
                    }


                    // create the new control. no need for reflection here. Note we
                    // clone the properties of the incoming newMBObj otherwise
                    // we could have two controls working off the same object
                    if ((newMBObj is MonitorBlockProperties_Blank) == true)
                    {
                        newMBCtl = new ctlMonitorBlock_Blank();
                    }
                    else if ((newMBObj is MonitorBlockProperties_Graph) == true)
                    {
                        newMBCtl = new ctlMonitorBlock_Graph();
                    }
                    else if ((newMBObj is MonitorBlockProperties_Number) == true)
                    {
                        newMBCtl = new ctlMonitorBlock_Number();
                    }
                    else if ((newMBObj is MonitorBlockProperties_Integral) == true)
                    {
                        newMBCtl = new ctlMonitorBlock_Integral();
                    }
                    else if ((newMBObj is MonitorBlockProperties_Text) == true)
                    {
                        newMBCtl = new ctlMonitorBlock_Text();
                    }
                    else
                    {
                        // don't know what to do so do nothing
                        return;
                    }

                    // deep clone the properties over to the new object
                    newMBCtl.Properties = newMBObj.DeepClone();

                    // transfer some screen properties to the newMBCtrl
                    newMBCtl.Location = targetMBCtl.Location;
                    newMBCtl.Size = targetMBCtl.Size;
                    // give it the delegate so it too can accept drag and drop
                    newMBCtl.ReplaceMonitorBlock = targetMBCtl.ReplaceMonitorBlock;
                    // it also needs to know its slotID
                    newMBCtl.Properties.SlotID = targetMBCtl.Properties.SlotID;

                    // make sure the properties are updated
                    newMBCtl.SyncDisplayToProperties();

                    // remove the old control
                    tabPageMonitorBlocks.Controls.Remove(targetMBCtl);
                    // add the new control
                    tabPageMonitorBlocks.Controls.Add(newMBCtl);

                    // occasionally when we replace we want to remove the original from a source
                    // usually this is when it comes in via a dragdrop
                    if(newMBObj.WantUsrPickerDeleteOnDrop == true)
                    {
                        // the User MB Picker wants a delete
                        ctlMonitorBlockPicker1.RemoveFromUsrPickerListByObject(newMBObj);
                        // reset this
                        newMBCtl.Properties.WantUsrPickerDeleteOnDrop = false;
                    }

                    // for safetys sake if we are dropping on a non blank MB we move the
                    // target to the user defined list. That way the user can get it back
                    // if they need to
                    if (((targetMBCtl is ctlMonitorBlock_Blank)==false) && (targetMBCtl.Properties.IsAutoGenerated==false))
                    {
                        // add the mb
                        ctlMonitorBlockPicker1.MBList_UserDefined.Add(targetMBCtl.Properties);
                        ctlMonitorBlockPicker1.RefreshUserDefinedMBList();
                    }

                    // flag this
                    summaryScreenLayoutChanged = true;
                }
                finally
                {
                    // reset this
                    pauseSummaryScreenUpdates = false;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle the change of a monitor block on the Summary panel to another 
        /// type. Typically this gets called when a new monitor block is dropped on
        /// an existing one. Also called when restoring the summary screen from saved
        /// settings
        /// </summary>
        /// <param name="mbObjectToDelete">the properties of the monitor block we delete</param>
        public void HandleDeleteMonitorBlock(MonitorBlockProperties_Base mbObjectToDelete)
        {
            LogMessage("HandleDeleteMonitorBlock called");
            ctlMonitorBlock_Base newMBCtl = null;


            // sanity checks
            if (mbObjectToDelete == null) return;

            // we must be in the form thread. 
            if (this.InvokeRequired)
            {
                Action safeWrite = delegate { HandleDeleteMonitorBlock(mbObjectToDelete); };
                this.Invoke(safeWrite);
            }
            else
            {
                try
                {
                    // we must stop the update of the screen
                    pauseSummaryScreenUpdates = true;

                    // find the new control
                    ctlMonitorBlock_Base targetMBCtl = FindMonitorBlockOnSummaryTabPageBySlotID(mbObjectToDelete.SlotID);
                    if (targetMBCtl == null) return;

                    // create the new control. no need for reflection here
                    newMBCtl = new ctlMonitorBlock_Blank();
                    newMBCtl.Properties = new MonitorBlockProperties_Blank();

                    // transfer some screen properties to the newMBCtrl
                    newMBCtl.Location = targetMBCtl.Location;
                    newMBCtl.Size = targetMBCtl.Size;
                    // give it the delegate so it too can accept drag and drop
                    newMBCtl.ReplaceMonitorBlock = targetMBCtl.ReplaceMonitorBlock;
                    // it also needs to know its slotID
                    newMBCtl.Properties.SlotID = targetMBCtl.Properties.SlotID;
                    // reset this
                    newMBCtl.Properties.WantUsrPickerDeleteOnDrop = false;

                    // remove the old control
                    tabPageMonitorBlocks.Controls.Remove(targetMBCtl);
                    // add the new control
                    tabPageMonitorBlocks.Controls.Add(newMBCtl);
                    // flag this
                    summaryScreenLayoutChanged = true;
                }
                finally
                {
                    // reset this
                    pauseSummaryScreenUpdates = false;
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Set the Change Monitor Delegate on all Active Blocks
        /// </summary>
        private void SetReplaceMonitorBlockDelegateOnActiveBlocks()
        {
            LogMessage("SetReplaceMonitorBlockDelegateOnActiveBlocks");
            List<ctlMonitorBlock_Base> mbList = FindAllMonitorBlocksOnSummaryTabPage();
            foreach(ctlMonitorBlock_Base mbObj in mbList)
            {
                mbObj.ReplaceMonitorBlock = new ReplaceMonitorBlockDelegate(HandleReplaceMonitorBlock);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Set the SlotID on all monitor blocks on the summary page
        /// </summary>
        private void SetSlotIDOnAllBlocksOnSummaryTabPage()
        {
            LogMessage("SetSlotIDOnAllBlocksOnSummaryTabPage");
            List<ctlMonitorBlock_Base> mbList = FindAllMonitorBlocksOnSummaryTabPage();
            foreach (ctlMonitorBlock_Base mbObj in mbList)
            {
                mbObj.Properties.SlotID = GetSlotIDFromScreenCoords(mbObj.Location.X, mbObj.Location.Y);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Creates the initial monitor blocks on the summary page
        /// </summary>
        private void CreateInitialMonitorBlocksOnSummaryTabPage()
        {
            LogMessage("CreateBlankMonitorBlocksOnSummaryTabPage");
            for(int row=0; row<NumberOfMBRows; row++)
            {
                ctlMonitorBlock_Base baseMBObj = null;
                for (int col = 0; col < NumberOfMBRows; col++)
                {
                    // there are certain special cases where we display a hard coded MB
                    // this is so the user has something to see on the screen even at defaults
                    if ((row == 0) && (col == 0)) baseMBObj = CreateHardCoded_Sys_Batt_V_MonitorBlock();
                    else baseMBObj = new ctlMonitorBlock_Blank();

                    int rowLoc = (row * MB_BLOCK_WIDTH) + (row * MB_BLOCK_SEPARATION);
                    int colLoc = (col * MB_BLOCK_HEIGHT) + (col * MB_BLOCK_SEPARATION);

                    // set the position
                    baseMBObj.Location = new Point(rowLoc, colLoc);
                    tabPageMonitorBlocks.Controls.Add(baseMBObj);
                    // init it
                    baseMBObj.SyncDisplayToProperties();
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Hand builds a monitor block displaying the Sys_Batt_V
        /// </summary>
        private ctlMonitorBlock_Number CreateHardCoded_Sys_Batt_V_MonitorBlock()
        {
            ctlMonitorBlock_Number outCtl = new ctlMonitorBlock_Number();
            outCtl.Properties.DisplayName = "MATE3S#Sys_Batt_V";
            outCtl.Properties.IsAutoGenerated = true;
            (outCtl.Properties as MonitorBlockProperties_Number).DataSource = "MATE3S#Sys_Batt_V";
            (outCtl.Properties as MonitorBlockProperties_Number).Suffix = "v";
            (outCtl.Properties as MonitorBlockProperties_Number).TitleText = "Mate3s System Battery";
            return outCtl;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the view logfile dir button
        /// </summary>
        private void buttonOpenLogFileDir_Click(object sender, EventArgs e)
        {
            // opens the folder in explorer
            System.Diagnostics.Process.Start("explorer.exe", g_Logger.LogFileDirectory);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the view db dir button
        /// </summary>
        private void buttonOpenDBFileDir_Click(object sender, EventArgs e)
        {
            // opens the folder in explorer
            System.Diagnostics.Process.Start("explorer.exe", OutputDir);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the view saved settings dir button
        /// </summary>
        private void buttonOpenSavedSettingsDir_Click(object sender, EventArgs e)
        {
            // opens the folder in explorer
            System.Diagnostics.Process.Start("explorer.exe", OutputDir);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a key press on the textBoxHeartbeatUpdateTimeSec
        /// </summary>
        private void textBoxHeartbeatUpdateTimeSec_KeyPress(object sender, KeyPressEventArgs e)
        {
            // we only permit numbers
            // credit: https://stackoverflow.com/questions/37805179/how-can-i-make-a-textbox-accepts-only-numeric-values
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a key press on the textBoxDatabaseWriteTimeInterval_Sec
        /// </summary>
        private void textBoxDatabaseWriteTimeInterval_Sec_KeyPress(object sender, KeyPressEventArgs e)
        {
            // we only permit numbers
            // credit: https://stackoverflow.com/questions/37805179/how-can-i-make-a-textbox-accepts-only-numeric-values
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// User settable option to inhibit the refresh of the monitor blocks
        /// </summary>
        private bool DisableMonitorBlockRefresh
        {
            get
            {
                return checkBoxDisableMonitorBlockRefresh.Checked;
            }
            set
            {
                checkBoxDisableMonitorBlockRefresh.Checked = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// User settable option to inhibit the refresh of the monitor blocks
        /// </summary>
        private bool DisableDBWrites
        {
            get
            {
                return checkBoxDisableDBWrites.Checked;
            }
            set
            {
                checkBoxDisableDBWrites.Checked = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a press on the close button
        /// </summary>
        private void buttonClose_Click(object sender, EventArgs e)
        {
#if DEBUG
            //// auto generate number presets
            //// CC1
            //GeneratePresetsFromEnum("n_DEVALIAS#FIELDNAME.xml", "Kimono.MB_NumberXMLTemplate.txt", "CC1", typeof(KnownDevicePlottableFieldsCCEnum));
            //// INV1
            //GeneratePresetsFromEnum("n_DEVALIAS#FIELDNAME.xml", "Kimono.MB_NumberXMLTemplate.txt", "INV1", typeof(KnownDevicePlottableFieldsINVEnum));
            //// FNDC
            //GeneratePresetsFromEnum("n_DEVALIAS#FIELDNAME.xml", "Kimono.MB_NumberXMLTemplate.txt", "FNDC", typeof(KnownDevicePlottableFieldsFNDCEnum));
            //// MATE3S
            //GeneratePresetsFromEnum("n_DEVALIAS#FIELDNAME.xml", "Kimono.MB_NumberXMLTemplate.txt", "MATE3S", typeof(KnownDevicePlottableFieldsMATE3SEnum));

            //// auto generate graph presets
            //// CC1
            //GeneratePresetsFromEnum("g_DEVALIAS#FIELDNAME.xml", "Kimono.MB_GraphXMLTemplate.txt", "CC1", typeof(KnownDevicePlottableFieldsCCEnum));
            //// INV1
            //GeneratePresetsFromEnum("g_DEVALIAS#FIELDNAME.xml", "Kimono.MB_GraphXMLTemplate.txt", "INV1", typeof(KnownDevicePlottableFieldsINVEnum));
            //// FNDC
            //GeneratePresetsFromEnum("g_DEVALIAS#FIELDNAME.xml", "Kimono.MB_GraphXMLTemplate.txt", "FNDC", typeof(KnownDevicePlottableFieldsFNDCEnum));
            //// MATE3S
            //GeneratePresetsFromEnum("g_DEVALIAS#FIELDNAME.xml", "Kimono.MB_GraphXMLTemplate.txt", "MATE3S", typeof(KnownDevicePlottableFieldsMATE3SEnum));

            //// auto generate text presets
            //// CC1
            //GeneratePresetsFromEnum("t_DEVALIAS#FIELDNAME.xml", "Kimono.MB_TextXMLTemplate.txt", "CC1", typeof(KnownDeviceTextFieldsCCEnum));
            //// INV1
            //GeneratePresetsFromEnum("t_DEVALIAS#FIELDNAME.xml", "Kimono.MB_TextXMLTemplate.txt", "INV1", typeof(KnownDeviceTextFieldsINVEnum));
            //// FNDC
            //GeneratePresetsFromEnum("t_DEVALIAS#FIELDNAME.xml", "Kimono.MB_TextXMLTemplate.txt", "FNDC", typeof(KnownDeviceTextFieldsFNDCEnum));
            //// MATE3S
            //GeneratePresetsFromEnum("t_DEVALIAS#FIELDNAME.xml", "Kimono.MB_TextXMLTemplate.txt", "MATE3S", typeof(KnownDeviceTextFieldsMATE3SEnum));


#else
            this.Close();
#endif
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Generates Basic PreSets from a template using an enum for the substitution values 
        /// 
        /// Only used in DEBUG mode 
        /// 
        /// </summary>
        /// <param name="deviceAlias">the deviceAlias</param>
        /// <param name="fileNameTemplateIn">the output filename template</param>
        /// <param name="nameOfFileTemplate">the name of the file template as a resource</param>
        /// <param name="typeOfEnumToEmit">the type of the enum containing the fieldnames</param>
        private void GeneratePresetsFromEnum(string fileNameTemplateIn, string nameOfFileTemplate, string deviceAlias, Type typeOfEnumToEmit)
        {
            string templateXMLStr = null;
            string filePath = @"C:\Projects\Kimono\Kimono_GitRepo\Presets";
            string fileNameTemplate = ";";

            if ((fileNameTemplateIn == null) || (fileNameTemplateIn.Length == 0)) return;
            if ((nameOfFileTemplate == null) || (nameOfFileTemplate.Length == 0)) return;
            if ((deviceAlias == null) || (deviceAlias.Length == 0)) return;

            // build the filename and path
            fileNameTemplate = Path.Combine(filePath, fileNameTemplateIn);
            if ((fileNameTemplate == null) || (fileNameTemplate.Length == 0)) return;

            // the xml code is an .text file flagged as an Embedded resource. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // get the content
            using (Stream stream = assembly.GetManifestResourceStream(nameOfFileTemplate))
            using (StreamReader reader = new StreamReader(stream))
            {
                templateXMLStr = reader.ReadToEnd();
            }
            if ((templateXMLStr == null) || (templateXMLStr.Length == 0)) return;

            // loop through each value
            foreach (string fieldName in Enum.GetNames(typeOfEnumToEmit))
            {
                if (fieldName == "None") continue;
                string workingTemplate = String.Copy(templateXMLStr);
                string workingFileName = String.Copy(fileNameTemplate);
                workingTemplate = workingTemplate.Replace("DEVALIAS", deviceAlias).Replace("FIELDNAME", fieldName);
                workingFileName = workingFileName.Replace("DEVALIAS", deviceAlias).Replace("FIELDNAME", fieldName);
                // write it out as a text file
                File.WriteAllText(workingFileName, workingTemplate);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a drag/drop onto the trash 
        /// </summary>
        private void buttonTrashCan_DragDrop(object sender, DragEventArgs e)
        {
            // get the data. This is always a MonitorBlockDragDropContainer
            MonitorBlockDragDropContainer ddObj = (MonitorBlockDragDropContainer)e.Data.GetData(typeof(MonitorBlockDragDropContainer));
            if (ddObj == null) return;
            if (ddObj.MBObj == null) return;
            if (ddObj.Sender == null) return;

            MonitorBlockProperties_Base dropItemMBOriginal = ddObj.MBObj;
            // who sent us this, this determines our actions
            if((ddObj.Sender is ctlMonitorBlock_Base)==true)
            {
                // it was a control on the summary screen
                this.HandleDeleteMonitorBlock(dropItemMBOriginal);
            }
            else if ((ddObj.Sender is ctlMonitorBlockPicker)== true)
            {
                // it was a ctlMonitorBlockPicker. This is an MB obj
                // being removed from the User Def List
                ctlMonitorBlockPicker1.RemoveFromUsrPickerListByObject(ddObj.MBObj);
            }
            else
            {
                // don't know what to do so we do nothing
            }

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a drag over on the trash 
        /// </summary>
        private void buttonTrashCan_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handles a press on the Plot report button
        /// </summary>
        private void buttonPlotReport_Click(object sender, EventArgs e)
        {
            StartPlotReport();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Starts up the plot report dialog box
        /// </summary>
        private void StartPlotReport()
        {
            frmPlotReport frmPlot = new frmPlotReport();
            frmPlot.Show();
        }
    }
}

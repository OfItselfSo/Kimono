using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
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
    /// A class to handle database access. This class can handle only one connection
    /// at a time. The open connection is retained until closed and is used for 
    /// all subsequent calls.
    /// </summary>
    public class DBManager : OISObjBase
    {
        private SQLiteConnection currentConnection = null;

        private const int MIN_DB_DIR_LEN = 6;
        private const int MIN_DB_NAME_LEN = 6;
        private const string DEFAULT_DB_NAME = "kimono.db";
        private const string DEBUG_DB_DIRECTORY = @"C:\Dump\Project Logs";
        private string db_directory = null;
        private string db_name = DEFAULT_DB_NAME;

        // this is the number of PortData_* tables that we expect to see
        private int EXPECTED_NUMBER_OF_KIMONO_PORTDATA_TABLES = 5;
        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="dbDirectoryToUse">the db directory</param>
        public DBManager(string dbDirectoryToUse)
        {
            // calling this also sets the defaults if the path is really wrong
            DB_Directory = dbDirectoryToUse;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the db dir, will never return null or an invalid string
        /// Does not check for existence.
        /// </summary>
        public string DB_Directory
        {
            get
            {
                // if not valid, set it to a valid default
                if ((db_directory == null) || (db_directory.Length < MIN_DB_DIR_LEN) || (Path.IsPathRooted(db_directory) == false))
                {
                    db_directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmMain.APPLICATION_NAME);
                }
                return db_directory;
            }
            set
            {
                db_directory = value;
                if ((db_directory == null) || (db_directory.Length < MIN_DB_DIR_LEN) || (Path.IsPathRooted(db_directory) == false))
                {
                    db_directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmMain.APPLICATION_NAME);
                }
            }            
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Check the db dir exists. Will create it if it can, might recursively 
        /// create on some windows versions
        /// </summary>
        /// <returns>true - directory accessed ok, false - an error happened</returns>
        public bool EnsureDBDirExists()
        {
            try
            {
                // test to see if it exists
                if (Directory.Exists(DB_Directory) == false)
                {
                    // create it.
                    Directory.CreateDirectory(DB_Directory);
                    LogMessage("Created db directory >" + DB_Directory + "<");
                }
                else
                {
                    LogMessage("DB Directory exists >" + DB_Directory + "<");
                }
                return true;
            }
            catch ( Exception ex)
            {
                LogMessage("Error accessing db directory: " + ex.Message);
                return false;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Creates a new database. Does NOT check to see if it already exists 
        /// and will overwrite if it does exist. Uses the pre-set file path and
        /// name. Will exception on fail. The path must exist.
        /// </summary>
        /// <returns>z success, nz fail</returns>
        public int CreateNewDB()
        {
            string dbFilePathAndName = DBFilePathAndName;
            SQLiteConnection.CreateFile(dbFilePathAndName);
            return 0;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detects if the current connection is open
        /// </summary>
        /// <returns>true is open, false is not open</returns>
        public bool ConnectionIsOpen()
        {
            if (CurrentConnection == null) return false;
            if (CurrentConnection.State != System.Data.ConnectionState.Open) return false;
            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the current connection. There is no set accessor, this is done by 
        /// calling OpenConnection(). Will return null if not open.
        /// </summary>
        /// <returns>the db connection</returns>
        public SQLiteConnection CurrentConnection { get => currentConnection; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Open connection, records the open connection
        /// </summary>
        /// <returns>true- the db opened, false - it did not open</returns>
        public bool OpenConnection()
        {
            LogMessage("OpenConnection to db");
            try
            {
                // if already open close it up
                if (ConnectionIsOpen() == true) CloseConnection();
                // create the full file path and name, this is assumed to be already set
                string dbFilePathAndName = DBFilePathAndName;
                LogMessage("DBFilePathAndName is>" + DBFilePathAndName + "<");

                // open the connection
                currentConnection = new SQLiteConnection("Data Source=" + dbFilePathAndName);
                currentConnection.Open();
                if (ConnectionIsOpen() == false)
                {
                    LogMessage("The database did not open. Unknown reason.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogMessage("The database failed to open: " + ex.Message);
                return false;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Closes the current connection
        /// </summary>
        /// <returns>true is open, false is not open</returns>
        public void CloseConnection()
        {
            LogMessage("CloseConnection to db");
            if (ConnectionIsOpen() == false)
            {
                currentConnection = null;
                return;
            }
            CurrentConnection.Close();
            currentConnection = null;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the DB File Path and name. Expects the components to have already
        /// been set. Will return defaults othewise
        /// </summary>
        /// <returns>the dbfile full file path</returns>
        public string DBFilePathAndName
        {
            get 
            { 
                // create the full file path and name, this is assumed to be already set
                return Path.Combine(this.DB_Directory, DB_Name);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets the current DB Name, will return a default if null or 
        /// too small. There is no Set, we always use the default.
        /// </summary>
        /// <returns>the db name</returns>
        public string DB_Name
        {
            get
            {
                if ((db_name == null) || (db_name.Length < MIN_DB_NAME_LEN)) db_name = DEFAULT_DB_NAME;
                return db_name;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Drops the Kimono tables in the database. Does not check to see if this is 
        /// a good idea. It just does it
        /// </summary>
        private void DropKimonoTables()
        {
            if (ConnectionIsOpen() == false)
            {
                throw new Exception("Database connection is not open");
            }
            string sql = "drop table IF EXISTS PortData";
            SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
            command.ExecuteNonQuery();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Checks the tabels exist and creates them if necessary
        /// </summary>
        /// <returns>true - tables exist, false - error happened</returns>
        public bool CheckTablesExistAndCreateIfNot()
        {
            try
            {
                LogMessage("Checking if database tables exist");
                if (ConnectionIsOpen() == false)
                {
                    throw new Exception("Database connection is not open");
                }

                string sql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name like 'PortData_%'";
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                // the existence of some returning data is enough
                if (reader.HasRows)
                {
                    reader.Read();
                    int numTables = reader.GetInt32(0);
                    if(numTables == EXPECTED_NUMBER_OF_KIMONO_PORTDATA_TABLES)
                    { 
                        LogMessage("The database tables exist");
                        return true;
                    }
                    else
                    {
                        // not correct, we create the tables now
                        CreateKimonoTables();
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Error accessing database: " + ex.Message);
                return false;
            }
            return true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Creates the tables in the DB used by Kimono. Throws exceptions on errors
        /// </summary>
        private void CreateKimonoTables()
        {
            LogMessage("Creating Kimono database tables");

            if (ConnectionIsOpen() == false)
            {
                throw new Exception("Database connection is not open");
            }
            // the sql code is an .sql file flagged as an Embedded resource. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // the PortData_CC Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_TableCreateCC.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_FNDC Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_TableCreateFNDC.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_INV Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_TableCreateINV.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_MATE3S Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_TableCreateMATE3S.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_USERDATA Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_TableCreateUSERDATA.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // now the IDXPortData1 index on the PortData_* tables, we run it for each table

            // the PortData_CC Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_IndexCreate1.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                sql = sql.Replace("%TABLE_ID%","CC");
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_FNDC Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_IndexCreate1.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                sql = sql.Replace("%TABLE_ID%", "FNDC");
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_INV Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_IndexCreate1.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                sql = sql.Replace("%TABLE_ID%", "INV");
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_MATE3S Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_IndexCreate1.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                sql = sql.Replace("%TABLE_ID%", "MATE3S");
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }

            // the PortData_USERDATA Table
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_IndexCreate1.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                sql = sql.Replace("%TABLE_ID%", "USERDATA");
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                command.ExecuteNonQuery();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Writes the data in a generic portstatus object out to the database
        /// </summary>
        /// <param name="outbackReport">the outback report object</param>
        /// <param name="portObj">the port status object</param>
        public void WritePortStatusToDB(OutbackSystem outbackReport, PortStatus portObj)
        {
            if (outbackReport == null) return;
            if (portObj == null) return;
            if(portObj.DeviceAlias==null) return;

            if (portObj.DeviceAlias == OutbackSystem.DEVALIAS_MATE3S)
            {
                // we are dealing with a Mate3s Device. Call the approriate data writer
                WritePortStatusToDB_MATE3S(outbackReport, portObj);
            }
            else if (portObj.DeviceAlias == OutbackSystem.DEVALIAS_FNDC)
            {
                // we are dealing with a FNDC Device. Call the approriate data writer
                WritePortStatusToDB_FNDC(outbackReport, portObj);
            }
            else if (portObj.DeviceAlias == OutbackSystem.DEVALIAS_USERDATA)
            {
                // we are dealing with a USERDATA Device. Is it empty?
                if (portObj.UserDataHasChanged == false) return;
                // It has changed, call the approriate data writer
                WritePortStatusToDB_USERDATA(outbackReport, portObj);
            }
            else if (portObj.DeviceAlias.StartsWith(OutbackSystem.DEVALIAS_CC_PREFIX) == true)
            {
                // we are dealing with a CC Device. Call the approriate data writer
                WritePortStatusToDB_CC(outbackReport, portObj);
            }
            else if (portObj.DeviceAlias.StartsWith(OutbackSystem.DEVALIAS_INV_PREFIX) == true)
            {
                // we are dealing with a INV Device. Call the approriate data writer
                WritePortStatusToDB_INV(outbackReport, portObj);
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Writes the data in a FNDC portstatus object out to the database. We 
        /// do not check the alias. It is assumed the caller knows what it is doing.
        /// </summary>
        /// <param name="outbackReport">the outback report object</param>
        /// <param name="portObj">the port status object</param>
        public void WritePortStatusToDB_FNDC(OutbackSystem outbackReport, PortStatus portObj)
        {
            if (outbackReport == null) return;
            if (portObj == null) return;

            // the sql code is an .sql file flagged as an Embedded resourse. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // fetch the insert statement PortDataTable
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_InsertFNDC.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                //                LogMessage(">" + sql + "<");
                SQLiteCommand insertCmd = new SQLiteCommand(sql, CurrentConnection);
                insertCmd.Parameters.AddWithValue("RecordDate", portObj.RecordDate);
                insertCmd.Parameters.AddWithValue("DeviceAlias", portObj.DeviceAlias);
                insertCmd.Parameters.AddWithValue("KimonoMajorVer", outbackReport.KimonoMajorVer);
                insertCmd.Parameters.AddWithValue("KimonoMinorVer", outbackReport.KimonoMinorVer);

                insertCmd.Parameters.AddWithValue("FiveMinuteMarker", outbackReport.FiveMinuteMarker);
                insertCmd.Parameters.AddWithValue("TenMinuteMarker", outbackReport.TenMinuteMarker);
                insertCmd.Parameters.AddWithValue("FifteenMinuteMarker", outbackReport.FifteenMinuteMarker);
                insertCmd.Parameters.AddWithValue("ThirtyMinuteMarker", outbackReport.ThirtyMinuteMarker);
                insertCmd.Parameters.AddWithValue("HourMarker", outbackReport.HourMarker);
                insertCmd.Parameters.AddWithValue("DayMarker", outbackReport.DayMarker);

                insertCmd.Parameters.AddWithValue("Port", portObj.Port);
                insertCmd.Parameters.AddWithValue("Dev", portObj.Dev);

                // properties specific to FNDC Dev ports
                insertCmd.Parameters.AddWithValue("Shunt_A_I", portObj.Shunt_A_I);
                insertCmd.Parameters.AddWithValue("Shunt_A_AH", portObj.Shunt_A_AH);
                insertCmd.Parameters.AddWithValue("Shunt_A_kWh", portObj.Shunt_A_kWh);
                insertCmd.Parameters.AddWithValue("Shunt_B_I", portObj.Shunt_B_I);
                insertCmd.Parameters.AddWithValue("Shunt_B_AH", portObj.Shunt_B_AH);
                insertCmd.Parameters.AddWithValue("Shunt_B_kWh", portObj.Shunt_B_kWh);
                insertCmd.Parameters.AddWithValue("Shunt_C_I", portObj.Shunt_B_I);
                insertCmd.Parameters.AddWithValue("Shunt_C_AH", portObj.Shunt_B_AH);
                insertCmd.Parameters.AddWithValue("Shunt_C_kWh", portObj.Shunt_B_kWh);
                insertCmd.Parameters.AddWithValue("SOC", portObj.SOC);
                insertCmd.Parameters.AddWithValue("Min_SOC", portObj.Min_SOC);
                insertCmd.Parameters.AddWithValue("Days_since_full", portObj.Days_since_full);
                insertCmd.Parameters.AddWithValue("CHG_parms_met", portObj.CHG_parms_met);
                insertCmd.Parameters.AddWithValue("In_AH_today", portObj.In_AH_today);
                insertCmd.Parameters.AddWithValue("Out_AH_today", portObj.Out_AH_today);
                insertCmd.Parameters.AddWithValue("In_kWh_today", portObj.In_kWh_today);
                insertCmd.Parameters.AddWithValue("Out_kWh_today", portObj.Out_kWh_today);
                insertCmd.Parameters.AddWithValue("Net_CFC_AH", portObj.Net_CFC_AH);
                insertCmd.Parameters.AddWithValue("Net_CFC_kWh", portObj.Net_CFC_kWh);
                insertCmd.Parameters.AddWithValue("Batt_temp", portObj.Batt_temp);
                insertCmd.Parameters.AddWithValue("Batt_V", portObj.Batt_V);
                insertCmd.Parameters.AddWithValue("Aux_mode", portObj.Aux_mode);
                insertCmd.Parameters.AddWithValue("AUX", portObj.AUX);

                // run it
                insertCmd.ExecuteNonQuery();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Writes the data in a MATE3S portstatus object out to the database. We 
        /// do not check the alias. It is assumed the caller knows what it is doing.
        /// </summary>
        /// <param name="outbackReport">the outback report object</param>
        /// <param name="portObj">the port status object</param>
        public void WritePortStatusToDB_MATE3S(OutbackSystem outbackReport, PortStatus portObj)
        {
            if (outbackReport == null) return;
            if (portObj == null) return;

            // the sql code is an .sql file flagged as an Embedded resourse. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // fetch the insert statement PortDataTable
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_InsertMATE3S.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
//                LogMessage(">" + sql + "<");
                SQLiteCommand insertCmd = new SQLiteCommand(sql, CurrentConnection);
                insertCmd.Parameters.AddWithValue("RecordDate", portObj.RecordDate);
                insertCmd.Parameters.AddWithValue("DeviceAlias", portObj.DeviceAlias);
                insertCmd.Parameters.AddWithValue("KimonoMajorVer", outbackReport.KimonoMajorVer);
                insertCmd.Parameters.AddWithValue("KimonoMinorVer", outbackReport.KimonoMinorVer);

                insertCmd.Parameters.AddWithValue("FiveMinuteMarker", outbackReport.FiveMinuteMarker);
                insertCmd.Parameters.AddWithValue("TenMinuteMarker", outbackReport.TenMinuteMarker);
                insertCmd.Parameters.AddWithValue("FifteenMinuteMarker", outbackReport.FifteenMinuteMarker);
                insertCmd.Parameters.AddWithValue("ThirtyMinuteMarker", outbackReport.ThirtyMinuteMarker);
                insertCmd.Parameters.AddWithValue("HourMarker", outbackReport.HourMarker);
                insertCmd.Parameters.AddWithValue("DayMarker", outbackReport.DayMarker);

                insertCmd.Parameters.AddWithValue("OutbackDate", portObj.OutbackDate);
                insertCmd.Parameters.AddWithValue("Sys_Batt_V", portObj.Sys_Batt_V);
                insertCmd.Parameters.AddWithValue("Gateway_Type", portObj.Gateway_Type);

                // run it
                insertCmd.ExecuteNonQuery();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Writes the data in a CC portstatus object out to the database. We 
        /// do not check the alias. It is assumed the caller knows what it is doing.
        /// </summary>
        /// <param name="outbackReport">the outback report object</param>
        /// <param name="portObj">the port status object</param>
        public void WritePortStatusToDB_CC(OutbackSystem outbackReport, PortStatus portObj)
        {
            if (outbackReport == null) return;
            if (portObj == null) return;

            // the sql code is an .sql file flagged as an Embedded resourse. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // fetch the insert statement PortDataTable
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_InsertCC.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                //                LogMessage(">" + sql + "<");
                SQLiteCommand insertCmd = new SQLiteCommand(sql, CurrentConnection);
                insertCmd.Parameters.AddWithValue("RecordDate", portObj.RecordDate);
                insertCmd.Parameters.AddWithValue("DeviceAlias", portObj.DeviceAlias);
                insertCmd.Parameters.AddWithValue("KimonoMajorVer", outbackReport.KimonoMajorVer);
                insertCmd.Parameters.AddWithValue("KimonoMinorVer", outbackReport.KimonoMinorVer);

                insertCmd.Parameters.AddWithValue("FiveMinuteMarker", outbackReport.FiveMinuteMarker);
                insertCmd.Parameters.AddWithValue("TenMinuteMarker", outbackReport.TenMinuteMarker);
                insertCmd.Parameters.AddWithValue("FifteenMinuteMarker", outbackReport.FifteenMinuteMarker);
                insertCmd.Parameters.AddWithValue("ThirtyMinuteMarker", outbackReport.ThirtyMinuteMarker);
                insertCmd.Parameters.AddWithValue("HourMarker", outbackReport.HourMarker);
                insertCmd.Parameters.AddWithValue("DayMarker", outbackReport.DayMarker);

                insertCmd.Parameters.AddWithValue("Port", portObj.Port);
                insertCmd.Parameters.AddWithValue("Dev", portObj.Dev);
                insertCmd.Parameters.AddWithValue("Type", portObj.Type);

                // properties specific to CC Dev ports
                insertCmd.Parameters.AddWithValue("Out_I", portObj.Out_I);
                insertCmd.Parameters.AddWithValue("In_I", portObj.In_I);
                insertCmd.Parameters.AddWithValue("Batt_V", portObj.Batt_V);
                insertCmd.Parameters.AddWithValue("In_V", portObj.In_V);
                insertCmd.Parameters.AddWithValue("Out_kWh", portObj.Out_kWh);
                insertCmd.Parameters.AddWithValue("Out_AH", portObj.Out_AH);
                insertCmd.Parameters.AddWithValue("CC_mode", portObj.CC_mode);
                insertCmd.Parameters.AddWithValue("Aux_mode", portObj.Aux_mode);
                insertCmd.Parameters.AddWithValue("AUX", portObj.AUX);

                // run it
                insertCmd.ExecuteNonQuery();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Writes the data in a INV portstatus object out to the database. We 
        /// do not check the alias. It is assumed the caller knows what it is doing.
        /// </summary>
        /// <param name="outbackReport">the outback report object</param>
        /// <param name="portObj">the port status object</param>
        public void WritePortStatusToDB_INV(OutbackSystem outbackReport, PortStatus portObj)
        {
            if (outbackReport == null) return;
            if (portObj == null) return;

            // the sql code is an .sql file flagged as an Embedded resourse. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // fetch the insert statement PortDataTable
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_InsertINV.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                //                LogMessage(">" + sql + "<");
                SQLiteCommand insertCmd = new SQLiteCommand(sql, CurrentConnection);
                insertCmd.Parameters.AddWithValue("RecordDate", portObj.RecordDate);
                insertCmd.Parameters.AddWithValue("DeviceAlias", portObj.DeviceAlias);
                insertCmd.Parameters.AddWithValue("KimonoMajorVer", outbackReport.KimonoMajorVer);
                insertCmd.Parameters.AddWithValue("KimonoMinorVer", outbackReport.KimonoMinorVer);

                insertCmd.Parameters.AddWithValue("FiveMinuteMarker", outbackReport.FiveMinuteMarker);
                insertCmd.Parameters.AddWithValue("TenMinuteMarker", outbackReport.TenMinuteMarker);
                insertCmd.Parameters.AddWithValue("FifteenMinuteMarker", outbackReport.FifteenMinuteMarker);
                insertCmd.Parameters.AddWithValue("ThirtyMinuteMarker", outbackReport.ThirtyMinuteMarker);
                insertCmd.Parameters.AddWithValue("HourMarker", outbackReport.HourMarker);
                insertCmd.Parameters.AddWithValue("DayMarker", outbackReport.DayMarker);

                insertCmd.Parameters.AddWithValue("Port", portObj.Port);
                insertCmd.Parameters.AddWithValue("Dev", portObj.Dev);
                insertCmd.Parameters.AddWithValue("Type", portObj.Type);

                // properties specific to INV Dev ports
                insertCmd.Parameters.AddWithValue("Inv_I_L2", portObj.Inv_I_L2);
                insertCmd.Parameters.AddWithValue("Chg_I_L2", portObj.Chg_I_L2);
                insertCmd.Parameters.AddWithValue("Buy_I_L2", portObj.Buy_I_L2);
                insertCmd.Parameters.AddWithValue("Sell_I_L2", portObj.Sell_I_L2);
                insertCmd.Parameters.AddWithValue("VAC1_in_L2", portObj.VAC1_in_L2);
                insertCmd.Parameters.AddWithValue("VAC2_in_L2", portObj.VAC2_in_L2);
                insertCmd.Parameters.AddWithValue("VAC_out_L2", portObj.VAC_out_L2);
                insertCmd.Parameters.AddWithValue("AC_Input", portObj.AC_Input);
                insertCmd.Parameters.AddWithValue("Batt_V", portObj.Batt_V);
                insertCmd.Parameters.AddWithValue("AC_mode", portObj.AC_mode);
                insertCmd.Parameters.AddWithValue("INV_mode", portObj.INV_mode);
                insertCmd.Parameters.AddWithValue("AUX", portObj.AUX);

                // run it
                insertCmd.ExecuteNonQuery();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Writes the data in a USERDATA portstatus object out to the database. We 
        /// do not check the alias. It is assumed the caller knows what it is doing.
        /// </summary>
        /// <param name="outbackReport">the outback report object</param>
        /// <param name="portObj">the port status object</param>
        public void WritePortStatusToDB_USERDATA(OutbackSystem outbackReport, PortStatus portObj)
        {
            if (outbackReport == null) return;
            if (portObj == null) return;

            // the sql code is an .sql file flagged as an Embedded resourse. We access it like this
            var assembly = Assembly.GetExecutingAssembly();

            // fetch the insert statement PortDataTable
            using (Stream stream = assembly.GetManifestResourceStream("Kimono.SQL_PortData_InsertUSERDATA.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();
                //                LogMessage(">" + sql + "<");
                SQLiteCommand insertCmd = new SQLiteCommand(sql, CurrentConnection);
                insertCmd.Parameters.AddWithValue("RecordDate", portObj.RecordDate);
                insertCmd.Parameters.AddWithValue("DeviceAlias", portObj.DeviceAlias);
                insertCmd.Parameters.AddWithValue("KimonoMajorVer", outbackReport.KimonoMajorVer);
                insertCmd.Parameters.AddWithValue("KimonoMinorVer", outbackReport.KimonoMinorVer);

                insertCmd.Parameters.AddWithValue("FiveMinuteMarker", outbackReport.FiveMinuteMarker);
                insertCmd.Parameters.AddWithValue("TenMinuteMarker", outbackReport.TenMinuteMarker);
                insertCmd.Parameters.AddWithValue("FifteenMinuteMarker", outbackReport.FifteenMinuteMarker);
                insertCmd.Parameters.AddWithValue("ThirtyMinuteMarker", outbackReport.ThirtyMinuteMarker);
                insertCmd.Parameters.AddWithValue("HourMarker", outbackReport.HourMarker);
                insertCmd.Parameters.AddWithValue("DayMarker", outbackReport.DayMarker);

                // properties specific to USERDATA Dev ports
                insertCmd.Parameters.AddWithValue("UDN01", portObj.UDN01);
                insertCmd.Parameters.AddWithValue("UDN02", portObj.UDN02);
                insertCmd.Parameters.AddWithValue("UDN03", portObj.UDN03);
                insertCmd.Parameters.AddWithValue("UDN04", portObj.UDN04);
                insertCmd.Parameters.AddWithValue("UDN05", portObj.UDN05);
                insertCmd.Parameters.AddWithValue("UDN06", portObj.UDN06);
                insertCmd.Parameters.AddWithValue("UDN07", portObj.UDN07);
                insertCmd.Parameters.AddWithValue("UDN08", portObj.UDN08);
                insertCmd.Parameters.AddWithValue("UDN09", portObj.UDN09);
                insertCmd.Parameters.AddWithValue("UDN10", portObj.UDN10);
                insertCmd.Parameters.AddWithValue("UDN11", portObj.UDN11);
                insertCmd.Parameters.AddWithValue("UDN12", portObj.UDN12);
                insertCmd.Parameters.AddWithValue("UDN13", portObj.UDN13);
                insertCmd.Parameters.AddWithValue("UDN14", portObj.UDN14);

                insertCmd.Parameters.AddWithValue("UDD01", portObj.UDD01);
                insertCmd.Parameters.AddWithValue("UDD02", portObj.UDD02);
                insertCmd.Parameters.AddWithValue("UDD03", portObj.UDD03);

                insertCmd.Parameters.AddWithValue("UDT01", portObj.UDT01);
                insertCmd.Parameters.AddWithValue("UDT02", portObj.UDT02);
                insertCmd.Parameters.AddWithValue("UDT03", portObj.UDT03);
                // run it
                insertCmd.ExecuteNonQuery();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Reads two columns from the database. The first must be a date and the 
        /// second is assumed to be a double. The arrays cannot be null and we only
        /// populate the number we have space for. If the returned data is longer
        /// than the array it will be ignored.
        /// 
        /// Note the arrays must be built and must be the same size, we do not clean them
        /// 
        /// </summary>
        /// <param name="datetimeArr">the array for the datetimes</param>
        /// <param name="doubleArr">the array for the doubles</param>
        /// <param name="sqlQuery">the fully built sql query to run</param>
        public void ReadData2Col_DateAndDouble(ref DateTime[] datetimeArr, ref double[] doubleArr, string sqlQuery)
        {
            if (datetimeArr == null) return;
            if (doubleArr == null) return;
            if (datetimeArr.Length == 0) return;
            if (doubleArr.Length == 0) return;
            if (datetimeArr.Length != doubleArr.Length) return;

            // get the command
            using (var cmd = new SQLiteCommand(sqlQuery, CurrentConnection))
            {
                // run the query and get the reader
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    int i = 0;
                    while (rdr.Read())
                    {
                        if (i >= datetimeArr.Length) break;
                        datetimeArr[i] = rdr.GetDateTime(0);
                        doubleArr[i] = rdr.GetDouble(1);
                        i++;
                    }
                }
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Reads two columns from the database. The first must be a date and the 
        /// second is assumed to be a double. We return the data in lists
        /// 
        /// </summary>
        /// <param name="datetimeList">the list for the datetimes</param>
        /// <param name="doubleList">the list for the doubles</param>
        /// <param name="sqlQuery">the fully built sql query to run</param>
        public void ReadData2Col_DateAndDouble(ref List<DateTime>datetimeList, ref List<double> doubleList, string sqlQuery)
        {
            datetimeList = new List<DateTime>();
            doubleList = new List<double>();

            // get the command
            using (var cmd = new SQLiteCommand(sqlQuery, CurrentConnection))
            {
                // run the query and get the reader
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    int i = 0;
                    while (rdr.Read())
                    {
                        datetimeList.Add(rdr.GetDateTime(0));
                        doubleList.Add(rdr.GetDouble(1));
                        i++;
                    }
                }
            }
        }

    }
}

namespace Kimono
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.buttonMonitorBlocksTab = new System.Windows.Forms.Button();
            this.textBoxStatusBar1 = new System.Windows.Forms.TextBox();
            this.tabControlMainForm = new System.Windows.Forms.TabControl();
            this.tabPageMonitorBlocks = new System.Windows.Forms.TabPage();
            this.tabPageSystemConfiguration = new System.Windows.Forms.TabPage();
            this.buttonUserDataSummary = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxDisableDBWrites = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableMonitorBlockRefresh = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonPlotReport = new System.Windows.Forms.Button();
            this.buttonShowRawJSON = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDatabaseWriteTimeInterval_Sec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxHeartbeatUpdateTimeSec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOpenSavedSettingsDir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSavedSettingsDir = new System.Windows.Forms.TextBox();
            this.buttonOpenDBFileDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDBFilePathAndName = new System.Windows.Forms.TextBox();
            this.buttonOpenLogFileDir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLogFileDir = new System.Windows.Forms.TextBox();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonSaveSetup = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxStatusBar2 = new System.Windows.Forms.TextBox();
            this.buttonSetupTab = new System.Windows.Forms.Button();
            this.buttonTrashCan = new System.Windows.Forms.Button();
            this.ctlMonitorBlockPicker1 = new Kimono.ctlMonitorBlockPicker();
            this.ipAddressControl1 = new Kimono.IPAddressControl();
            this.tabControlMainForm.SuspendLayout();
            this.tabPageSystemConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMonitorBlocksTab
            // 
            this.buttonMonitorBlocksTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMonitorBlocksTab.Location = new System.Drawing.Point(840, 90);
            this.buttonMonitorBlocksTab.Name = "buttonMonitorBlocksTab";
            this.buttonMonitorBlocksTab.Size = new System.Drawing.Size(140, 23);
            this.buttonMonitorBlocksTab.TabIndex = 1;
            this.buttonMonitorBlocksTab.Text = "&Monitor Blocks";
            this.buttonMonitorBlocksTab.UseVisualStyleBackColor = true;
            this.buttonMonitorBlocksTab.Click += new System.EventHandler(this.buttonMonitorBlocksTab_Click);
            // 
            // textBoxStatusBar1
            // 
            this.textBoxStatusBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxStatusBar1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxStatusBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatusBar1.Location = new System.Drawing.Point(8, 550);
            this.textBoxStatusBar1.Multiline = true;
            this.textBoxStatusBar1.Name = "textBoxStatusBar1";
            this.textBoxStatusBar1.ReadOnly = true;
            this.textBoxStatusBar1.Size = new System.Drawing.Size(294, 14);
            this.textBoxStatusBar1.TabIndex = 3;
            // 
            // tabControlMainForm
            // 
            this.tabControlMainForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMainForm.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControlMainForm.Controls.Add(this.tabPageMonitorBlocks);
            this.tabControlMainForm.Controls.Add(this.tabPageSystemConfiguration);
            this.tabControlMainForm.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControlMainForm.Location = new System.Drawing.Point(0, 0);
            this.tabControlMainForm.Name = "tabControlMainForm";
            this.tabControlMainForm.SelectedIndex = 0;
            this.tabControlMainForm.Size = new System.Drawing.Size(836, 548);
            this.tabControlMainForm.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlMainForm.TabIndex = 5;
            // 
            // tabPageMonitorBlocks
            // 
            this.tabPageMonitorBlocks.Location = new System.Drawing.Point(4, 5);
            this.tabPageMonitorBlocks.Name = "tabPageMonitorBlocks";
            this.tabPageMonitorBlocks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonitorBlocks.Size = new System.Drawing.Size(828, 539);
            this.tabPageMonitorBlocks.TabIndex = 0;
            this.tabPageMonitorBlocks.Text = "tabPage1";
            this.tabPageMonitorBlocks.UseVisualStyleBackColor = true;
            // 
            // tabPageSystemConfiguration
            // 
            this.tabPageSystemConfiguration.Controls.Add(this.buttonUserDataSummary);
            this.tabPageSystemConfiguration.Controls.Add(this.label11);
            this.tabPageSystemConfiguration.Controls.Add(this.label10);
            this.tabPageSystemConfiguration.Controls.Add(this.checkBoxDisableDBWrites);
            this.tabPageSystemConfiguration.Controls.Add(this.checkBoxDisableMonitorBlockRefresh);
            this.tabPageSystemConfiguration.Controls.Add(this.textBox1);
            this.tabPageSystemConfiguration.Controls.Add(this.label9);
            this.tabPageSystemConfiguration.Controls.Add(this.label8);
            this.tabPageSystemConfiguration.Controls.Add(this.buttonPlotReport);
            this.tabPageSystemConfiguration.Controls.Add(this.buttonShowRawJSON);
            this.tabPageSystemConfiguration.Controls.Add(this.label6);
            this.tabPageSystemConfiguration.Controls.Add(this.textBoxDatabaseWriteTimeInterval_Sec);
            this.tabPageSystemConfiguration.Controls.Add(this.label7);
            this.tabPageSystemConfiguration.Controls.Add(this.label5);
            this.tabPageSystemConfiguration.Controls.Add(this.textBoxHeartbeatUpdateTimeSec);
            this.tabPageSystemConfiguration.Controls.Add(this.label4);
            this.tabPageSystemConfiguration.Controls.Add(this.buttonOpenSavedSettingsDir);
            this.tabPageSystemConfiguration.Controls.Add(this.label3);
            this.tabPageSystemConfiguration.Controls.Add(this.textBoxSavedSettingsDir);
            this.tabPageSystemConfiguration.Controls.Add(this.buttonOpenDBFileDir);
            this.tabPageSystemConfiguration.Controls.Add(this.label2);
            this.tabPageSystemConfiguration.Controls.Add(this.textBoxDBFilePathAndName);
            this.tabPageSystemConfiguration.Controls.Add(this.buttonOpenLogFileDir);
            this.tabPageSystemConfiguration.Controls.Add(this.label1);
            this.tabPageSystemConfiguration.Controls.Add(this.textBoxLogFileDir);
            this.tabPageSystemConfiguration.Controls.Add(this.ipAddressControl1);
            this.tabPageSystemConfiguration.Location = new System.Drawing.Point(4, 5);
            this.tabPageSystemConfiguration.Name = "tabPageSystemConfiguration";
            this.tabPageSystemConfiguration.Size = new System.Drawing.Size(828, 539);
            this.tabPageSystemConfiguration.TabIndex = 3;
            this.tabPageSystemConfiguration.Text = "Config";
            this.tabPageSystemConfiguration.UseVisualStyleBackColor = true;
            // 
            // buttonUserDataSummary
            // 
            this.buttonUserDataSummary.Location = new System.Drawing.Point(21, 351);
            this.buttonUserDataSummary.Name = "buttonUserDataSummary";
            this.buttonUserDataSummary.Size = new System.Drawing.Size(155, 23);
            this.buttonUserDataSummary.TabIndex = 24;
            this.buttonUserDataSummary.Text = "&UserData Summary...";
            this.buttonUserDataSummary.UseVisualStyleBackColor = true;
            this.buttonUserDataSummary.Click += new System.EventHandler(this.buttonUserDataSummary_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 436);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(259, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "This option stops all recording of data in the database";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 400);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(264, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "This option prevents contact with the Outback Mate3s";
            // 
            // checkBoxDisableDBWrites
            // 
            this.checkBoxDisableDBWrites.AutoSize = true;
            this.checkBoxDisableDBWrites.Location = new System.Drawing.Point(33, 453);
            this.checkBoxDisableDBWrites.Name = "checkBoxDisableDBWrites";
            this.checkBoxDisableDBWrites.Size = new System.Drawing.Size(112, 17);
            this.checkBoxDisableDBWrites.TabIndex = 21;
            this.checkBoxDisableDBWrites.Text = "Disable DB Writes";
            this.checkBoxDisableDBWrites.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisableMonitorBlockRefresh
            // 
            this.checkBoxDisableMonitorBlockRefresh.AutoSize = true;
            this.checkBoxDisableMonitorBlockRefresh.Location = new System.Drawing.Point(33, 416);
            this.checkBoxDisableMonitorBlockRefresh.Name = "checkBoxDisableMonitorBlockRefresh";
            this.checkBoxDisableMonitorBlockRefresh.Size = new System.Drawing.Size(169, 17);
            this.checkBoxDisableMonitorBlockRefresh.TabIndex = 20;
            this.checkBoxDisableMonitorBlockRefresh.Text = "Disable Monitor Block Refresh";
            this.checkBoxDisableMonitorBlockRefresh.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(286, 244);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(244, 58);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = "Note: If you change the IP Address it is best to  save the settings and and resta" +
    "rt the application. The background connection takes a while to timeout.";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(308, 244);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 13);
            this.label9.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(308, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "IP Address of Mate3s";
            // 
            // buttonPlotReport
            // 
            this.buttonPlotReport.Location = new System.Drawing.Point(21, 322);
            this.buttonPlotReport.Name = "buttonPlotReport";
            this.buttonPlotReport.Size = new System.Drawing.Size(155, 23);
            this.buttonPlotReport.TabIndex = 15;
            this.buttonPlotReport.Text = "Open &Plot Report...";
            this.buttonPlotReport.UseVisualStyleBackColor = true;
            this.buttonPlotReport.Click += new System.EventHandler(this.buttonPlotReport_Click);
            // 
            // buttonShowRawJSON
            // 
            this.buttonShowRawJSON.Location = new System.Drawing.Point(21, 293);
            this.buttonShowRawJSON.Name = "buttonShowRawJSON";
            this.buttonShowRawJSON.Size = new System.Drawing.Size(155, 23);
            this.buttonShowRawJSON.TabIndex = 14;
            this.buttonShowRawJSON.Text = "View Mate3s &JSON Data...";
            this.buttonShowRawJSON.UseVisualStyleBackColor = true;
            this.buttonShowRawJSON.Click += new System.EventHandler(this.buttonShowRawJSON_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(179, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "seconds";
            // 
            // textBoxDatabaseWriteTimeInterval_Sec
            // 
            this.textBoxDatabaseWriteTimeInterval_Sec.Location = new System.Drawing.Point(144, 224);
            this.textBoxDatabaseWriteTimeInterval_Sec.Name = "textBoxDatabaseWriteTimeInterval_Sec";
            this.textBoxDatabaseWriteTimeInterval_Sec.Size = new System.Drawing.Size(32, 20);
            this.textBoxDatabaseWriteTimeInterval_Sec.TabIndex = 11;
            this.textBoxDatabaseWriteTimeInterval_Sec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDatabaseWriteTimeInterval_Sec_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Write to Database every ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "seconds";
            // 
            // textBoxHeartbeatUpdateTimeSec
            // 
            this.textBoxHeartbeatUpdateTimeSec.Location = new System.Drawing.Point(144, 199);
            this.textBoxHeartbeatUpdateTimeSec.Name = "textBoxHeartbeatUpdateTimeSec";
            this.textBoxHeartbeatUpdateTimeSec.Size = new System.Drawing.Size(32, 20);
            this.textBoxHeartbeatUpdateTimeSec.TabIndex = 8;
            this.textBoxHeartbeatUpdateTimeSec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxHeartbeatUpdateTimeSec_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Update the screen every ";
            // 
            // buttonOpenSavedSettingsDir
            // 
            this.buttonOpenSavedSettingsDir.Location = new System.Drawing.Point(477, 58);
            this.buttonOpenSavedSettingsDir.Name = "buttonOpenSavedSettingsDir";
            this.buttonOpenSavedSettingsDir.Size = new System.Drawing.Size(102, 20);
            this.buttonOpenSavedSettingsDir.TabIndex = 1;
            this.buttonOpenSavedSettingsDir.Text = "&Open Folder...";
            this.buttonOpenSavedSettingsDir.UseVisualStyleBackColor = true;
            this.buttonOpenSavedSettingsDir.Click += new System.EventHandler(this.buttonOpenSavedSettingsDir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Saved Settings Directory";
            // 
            // textBoxSavedSettingsDir
            // 
            this.textBoxSavedSettingsDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSavedSettingsDir.Location = new System.Drawing.Point(19, 58);
            this.textBoxSavedSettingsDir.Name = "textBoxSavedSettingsDir";
            this.textBoxSavedSettingsDir.ReadOnly = true;
            this.textBoxSavedSettingsDir.Size = new System.Drawing.Size(452, 20);
            this.textBoxSavedSettingsDir.TabIndex = 6;
            this.textBoxSavedSettingsDir.TabStop = false;
            // 
            // buttonOpenDBFileDir
            // 
            this.buttonOpenDBFileDir.Location = new System.Drawing.Point(477, 151);
            this.buttonOpenDBFileDir.Name = "buttonOpenDBFileDir";
            this.buttonOpenDBFileDir.Size = new System.Drawing.Size(102, 20);
            this.buttonOpenDBFileDir.TabIndex = 3;
            this.buttonOpenDBFileDir.Text = "O&pen Folder...";
            this.buttonOpenDBFileDir.UseVisualStyleBackColor = true;
            this.buttonOpenDBFileDir.Click += new System.EventHandler(this.buttonOpenDBFileDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Database File Path and Name";
            // 
            // textBoxDBFilePathAndName
            // 
            this.textBoxDBFilePathAndName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDBFilePathAndName.Location = new System.Drawing.Point(19, 151);
            this.textBoxDBFilePathAndName.Name = "textBoxDBFilePathAndName";
            this.textBoxDBFilePathAndName.ReadOnly = true;
            this.textBoxDBFilePathAndName.Size = new System.Drawing.Size(452, 20);
            this.textBoxDBFilePathAndName.TabIndex = 3;
            this.textBoxDBFilePathAndName.TabStop = false;
            // 
            // buttonOpenLogFileDir
            // 
            this.buttonOpenLogFileDir.Location = new System.Drawing.Point(477, 103);
            this.buttonOpenLogFileDir.Name = "buttonOpenLogFileDir";
            this.buttonOpenLogFileDir.Size = new System.Drawing.Size(102, 20);
            this.buttonOpenLogFileDir.TabIndex = 2;
            this.buttonOpenLogFileDir.Text = "&Open Folder...";
            this.buttonOpenLogFileDir.UseVisualStyleBackColor = true;
            this.buttonOpenLogFileDir.Click += new System.EventHandler(this.buttonOpenLogFileDir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log File Path and Name";
            // 
            // textBoxLogFileDir
            // 
            this.textBoxLogFileDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLogFileDir.Location = new System.Drawing.Point(19, 103);
            this.textBoxLogFileDir.Name = "textBoxLogFileDir";
            this.textBoxLogFileDir.ReadOnly = true;
            this.textBoxLogFileDir.Size = new System.Drawing.Size(452, 20);
            this.textBoxLogFileDir.TabIndex = 0;
            this.textBoxLogFileDir.TabStop = false;
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.Image = global::Kimono.Properties.Resources.kimono_icon_s;
            this.buttonAbout.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonAbout.Location = new System.Drawing.Point(840, 7);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(140, 77);
            this.buttonAbout.TabIndex = 6;
            this.buttonAbout.Text = "About Kimono";
            this.buttonAbout.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // buttonSaveSetup
            // 
            this.buttonSaveSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSetup.Location = new System.Drawing.Point(840, 504);
            this.buttonSaveSetup.Name = "buttonSaveSetup";
            this.buttonSaveSetup.Size = new System.Drawing.Size(140, 25);
            this.buttonSaveSetup.TabIndex = 9;
            this.buttonSaveSetup.Text = "Save Setup";
            this.buttonSaveSetup.UseVisualStyleBackColor = true;
            this.buttonSaveSetup.Click += new System.EventHandler(this.buttonSaveSetup_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(840, 532);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(139, 25);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxStatusBar2
            // 
            this.textBoxStatusBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatusBar2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxStatusBar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatusBar2.Location = new System.Drawing.Point(317, 550);
            this.textBoxStatusBar2.Multiline = true;
            this.textBoxStatusBar2.Name = "textBoxStatusBar2";
            this.textBoxStatusBar2.ReadOnly = true;
            this.textBoxStatusBar2.Size = new System.Drawing.Size(514, 14);
            this.textBoxStatusBar2.TabIndex = 12;
            // 
            // buttonSetupTab
            // 
            this.buttonSetupTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetupTab.Location = new System.Drawing.Point(840, 118);
            this.buttonSetupTab.Name = "buttonSetupTab";
            this.buttonSetupTab.Size = new System.Drawing.Size(140, 25);
            this.buttonSetupTab.TabIndex = 13;
            this.buttonSetupTab.Text = "Setup";
            this.buttonSetupTab.UseVisualStyleBackColor = true;
            this.buttonSetupTab.Click += new System.EventHandler(this.buttonSetupTab_Click);
            // 
            // buttonTrashCan
            // 
            this.buttonTrashCan.AllowDrop = true;
            this.buttonTrashCan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTrashCan.Image = global::Kimono.Properties.Resources.trashcan;
            this.buttonTrashCan.Location = new System.Drawing.Point(873, 443);
            this.buttonTrashCan.Name = "buttonTrashCan";
            this.buttonTrashCan.Size = new System.Drawing.Size(75, 41);
            this.buttonTrashCan.TabIndex = 14;
            this.buttonTrashCan.UseVisualStyleBackColor = true;
            this.buttonTrashCan.DragDrop += new System.Windows.Forms.DragEventHandler(this.buttonTrashCan_DragDrop);
            this.buttonTrashCan.DragOver += new System.Windows.Forms.DragEventHandler(this.buttonTrashCan_DragOver);
            // 
            // ctlMonitorBlockPicker1
            // 
            this.ctlMonitorBlockPicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlMonitorBlockPicker1.ChangesHaveBeenMade = false;
            this.ctlMonitorBlockPicker1.Location = new System.Drawing.Point(840, 152);
            this.ctlMonitorBlockPicker1.MinimumSize = new System.Drawing.Size(140, 150);
            this.ctlMonitorBlockPicker1.Name = "ctlMonitorBlockPicker1";
            this.ctlMonitorBlockPicker1.Size = new System.Drawing.Size(143, 295);
            this.ctlMonitorBlockPicker1.TabIndex = 11;
            // 
            // ipAddressControl1
            // 
            this.ipAddressControl1.IPAddress = "0.0.0.0";
            this.ipAddressControl1.Location = new System.Drawing.Point(311, 215);
            this.ipAddressControl1.Name = "ipAddressControl1";
            this.ipAddressControl1.Size = new System.Drawing.Size(198, 26);
            this.ipAddressControl1.TabIndex = 16;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 566);
            this.Controls.Add(this.buttonTrashCan);
            this.Controls.Add(this.buttonSetupTab);
            this.Controls.Add(this.ctlMonitorBlockPicker1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSaveSetup);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.tabControlMainForm);
            this.Controls.Add(this.buttonMonitorBlocksTab);
            this.Controls.Add(this.textBoxStatusBar1);
            this.Controls.Add(this.textBoxStatusBar2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximumSize = new System.Drawing.Size(1015, 615);
            this.MinimumSize = new System.Drawing.Size(1005, 605);
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Kimono: Monitoring for Outback Solar Systems";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.tabControlMainForm.ResumeLayout(false);
            this.tabPageSystemConfiguration.ResumeLayout(false);
            this.tabPageSystemConfiguration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonMonitorBlocksTab;
        private System.Windows.Forms.TextBox textBoxStatusBar1;
        private System.Windows.Forms.TabControl tabControlMainForm;
        private System.Windows.Forms.TabPage tabPageMonitorBlocks;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonSaveSetup;
        private System.Windows.Forms.Button buttonClose;
        private ctlMonitorBlockPicker ctlMonitorBlockPicker1;
        private System.Windows.Forms.TextBox textBoxStatusBar2;
        private System.Windows.Forms.Button buttonSetupTab;
        private System.Windows.Forms.TabPage tabPageSystemConfiguration;
        private System.Windows.Forms.TextBox textBoxLogFileDir;
        private System.Windows.Forms.Button buttonOpenLogFileDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenDBFileDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDBFilePathAndName;
        private System.Windows.Forms.Button buttonOpenSavedSettingsDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSavedSettingsDir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxHeartbeatUpdateTimeSec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxDatabaseWriteTimeInterval_Sec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonShowRawJSON;
        private System.Windows.Forms.Button buttonTrashCan;
        private System.Windows.Forms.Button buttonPlotReport;
        private IPAddressControl ipAddressControl1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBoxDisableMonitorBlockRefresh;
        private System.Windows.Forms.CheckBox checkBoxDisableDBWrites;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonUserDataSummary;
    }
}


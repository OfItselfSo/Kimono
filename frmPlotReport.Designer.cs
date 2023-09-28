
namespace Kimono
{
    partial class frmPlotReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPlotReport));
            this.buttonClose = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.buttonReset = new System.Windows.Forms.Button();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDataInterval = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonPlot1Refresh = new System.Windows.Forms.Button();
            this.buttonPlot1Clear = new System.Windows.Forms.Button();
            this.ctlDeviceAndFieldPicker1 = new Kimono.ctlDeviceAndFieldPicker();
            this.buttonPlot2Clear = new System.Windows.Forms.Button();
            this.buttonPlot2Refresh = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ctlDeviceAndFieldPicker2 = new Kimono.ctlDeviceAndFieldPicker();
            this.buttonPlot3Clear = new System.Windows.Forms.Button();
            this.buttonPlot3Refresh = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ctlDeviceAndFieldPicker3 = new Kimono.ctlDeviceAndFieldPicker();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(673, 449);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(117, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(0, 1);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(664, 479);
            this.formsPlot1.TabIndex = 1;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(673, 420);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(117, 23);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Reset View";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(673, 30);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(117, 20);
            this.dateTimePickerStartDate.TabIndex = 3;
            this.dateTimePickerStartDate.ValueChanged += new System.EventHandler(this.dateTimePickerStartDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(670, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start Date";
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(673, 69);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(117, 20);
            this.dateTimePickerEndDate.TabIndex = 5;
            this.dateTimePickerEndDate.ValueChanged += new System.EventHandler(this.dateTimePickerEndDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(670, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "End Date";
            // 
            // comboBoxDataInterval
            // 
            this.comboBoxDataInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDataInterval.FormattingEnabled = true;
            this.comboBoxDataInterval.Location = new System.Drawing.Point(673, 111);
            this.comboBoxDataInterval.Name = "comboBoxDataInterval";
            this.comboBoxDataInterval.Size = new System.Drawing.Size(117, 21);
            this.comboBoxDataInterval.TabIndex = 8;
            this.comboBoxDataInterval.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataInterval_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(670, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Data Interval";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(670, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Plot1 is...";
            // 
            // buttonPlot1Refresh
            // 
            this.buttonPlot1Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlot1Refresh.Location = new System.Drawing.Point(748, 204);
            this.buttonPlot1Refresh.Name = "buttonPlot1Refresh";
            this.buttonPlot1Refresh.Size = new System.Drawing.Size(42, 19);
            this.buttonPlot1Refresh.TabIndex = 12;
            this.buttonPlot1Refresh.Text = "Plot It";
            this.buttonPlot1Refresh.UseVisualStyleBackColor = true;
            this.buttonPlot1Refresh.Click += new System.EventHandler(this.buttonPlot1Refresh_Click);
            // 
            // buttonPlot1Clear
            // 
            this.buttonPlot1Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlot1Clear.Location = new System.Drawing.Point(700, 204);
            this.buttonPlot1Clear.Name = "buttonPlot1Clear";
            this.buttonPlot1Clear.Size = new System.Drawing.Size(42, 19);
            this.buttonPlot1Clear.TabIndex = 13;
            this.buttonPlot1Clear.Text = "Clear";
            this.buttonPlot1Clear.UseVisualStyleBackColor = true;
            this.buttonPlot1Clear.Click += new System.EventHandler(this.buttonPlot1Clear_Click);
            // 
            // ctlDeviceAndFieldPicker1
            // 
            this.ctlDeviceAndFieldPicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlDeviceAndFieldPicker1.DeviceAlias = "None";
            this.ctlDeviceAndFieldPicker1.FieldName = null;
            this.ctlDeviceAndFieldPicker1.Location = new System.Drawing.Point(673, 157);
            this.ctlDeviceAndFieldPicker1.Name = "ctlDeviceAndFieldPicker1";
            this.ctlDeviceAndFieldPicker1.Size = new System.Drawing.Size(117, 64);
            this.ctlDeviceAndFieldPicker1.TabIndex = 10;
            // 
            // buttonPlot2Clear
            // 
            this.buttonPlot2Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlot2Clear.Location = new System.Drawing.Point(700, 293);
            this.buttonPlot2Clear.Name = "buttonPlot2Clear";
            this.buttonPlot2Clear.Size = new System.Drawing.Size(42, 19);
            this.buttonPlot2Clear.TabIndex = 17;
            this.buttonPlot2Clear.Text = "Clear";
            this.buttonPlot2Clear.UseVisualStyleBackColor = true;
            this.buttonPlot2Clear.Click += new System.EventHandler(this.buttonPlot2Clear_Click);
            // 
            // buttonPlot2Refresh
            // 
            this.buttonPlot2Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlot2Refresh.Location = new System.Drawing.Point(748, 293);
            this.buttonPlot2Refresh.Name = "buttonPlot2Refresh";
            this.buttonPlot2Refresh.Size = new System.Drawing.Size(42, 19);
            this.buttonPlot2Refresh.TabIndex = 16;
            this.buttonPlot2Refresh.Text = "Plot It";
            this.buttonPlot2Refresh.UseVisualStyleBackColor = true;
            this.buttonPlot2Refresh.Click += new System.EventHandler(this.buttonPlot2Refresh_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(670, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Plot 2 is...";
            // 
            // ctlDeviceAndFieldPicker2
            // 
            this.ctlDeviceAndFieldPicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlDeviceAndFieldPicker2.DeviceAlias = "None";
            this.ctlDeviceAndFieldPicker2.FieldName = null;
            this.ctlDeviceAndFieldPicker2.Location = new System.Drawing.Point(673, 246);
            this.ctlDeviceAndFieldPicker2.Name = "ctlDeviceAndFieldPicker2";
            this.ctlDeviceAndFieldPicker2.Size = new System.Drawing.Size(117, 64);
            this.ctlDeviceAndFieldPicker2.TabIndex = 14;
            // 
            // buttonPlot3Clear
            // 
            this.buttonPlot3Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlot3Clear.Location = new System.Drawing.Point(700, 387);
            this.buttonPlot3Clear.Name = "buttonPlot3Clear";
            this.buttonPlot3Clear.Size = new System.Drawing.Size(42, 19);
            this.buttonPlot3Clear.TabIndex = 21;
            this.buttonPlot3Clear.Text = "Clear";
            this.buttonPlot3Clear.UseVisualStyleBackColor = true;
            this.buttonPlot3Clear.Click += new System.EventHandler(this.buttonPlot3Clear_Click);
            // 
            // buttonPlot3Refresh
            // 
            this.buttonPlot3Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlot3Refresh.Location = new System.Drawing.Point(748, 387);
            this.buttonPlot3Refresh.Name = "buttonPlot3Refresh";
            this.buttonPlot3Refresh.Size = new System.Drawing.Size(42, 19);
            this.buttonPlot3Refresh.TabIndex = 20;
            this.buttonPlot3Refresh.Text = "Plot It";
            this.buttonPlot3Refresh.UseVisualStyleBackColor = true;
            this.buttonPlot3Refresh.Click += new System.EventHandler(this.buttonPlot3Refresh_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(670, 324);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Plot 3 is...";
            // 
            // ctlDeviceAndFieldPicker3
            // 
            this.ctlDeviceAndFieldPicker3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlDeviceAndFieldPicker3.DeviceAlias = "None";
            this.ctlDeviceAndFieldPicker3.FieldName = null;
            this.ctlDeviceAndFieldPicker3.Location = new System.Drawing.Point(673, 340);
            this.ctlDeviceAndFieldPicker3.Name = "ctlDeviceAndFieldPicker3";
            this.ctlDeviceAndFieldPicker3.Size = new System.Drawing.Size(117, 64);
            this.ctlDeviceAndFieldPicker3.TabIndex = 18;
            // 
            // frmPlotReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 483);
            this.Controls.Add(this.buttonPlot3Clear);
            this.Controls.Add(this.buttonPlot3Refresh);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ctlDeviceAndFieldPicker3);
            this.Controls.Add(this.buttonPlot2Clear);
            this.Controls.Add(this.buttonPlot2Refresh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ctlDeviceAndFieldPicker2);
            this.Controls.Add(this.buttonPlot1Clear);
            this.Controls.Add(this.buttonPlot1Refresh);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ctlDeviceAndFieldPicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxDataInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerEndDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerStartDate);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.buttonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "frmPlotReport";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Plot Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxDataInterval;
        private System.Windows.Forms.Label label2;
        private ctlDeviceAndFieldPicker ctlDeviceAndFieldPicker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonPlot1Refresh;
        private System.Windows.Forms.Button buttonPlot1Clear;
        private System.Windows.Forms.Button buttonPlot2Clear;
        private System.Windows.Forms.Button buttonPlot2Refresh;
        private System.Windows.Forms.Label label5;
        private ctlDeviceAndFieldPicker ctlDeviceAndFieldPicker2;
        private System.Windows.Forms.Button buttonPlot3Clear;
        private System.Windows.Forms.Button buttonPlot3Refresh;
        private System.Windows.Forms.Label label6;
        private ctlDeviceAndFieldPicker ctlDeviceAndFieldPicker3;
    }
}
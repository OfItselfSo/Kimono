
namespace Kimono
{
    partial class ctlDeviceAndFieldPicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxKnownDevices = new System.Windows.Forms.ComboBox();
            this.comboBoxDeviceFields = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxKnownDevices
            // 
            this.comboBoxKnownDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxKnownDevices.FormattingEnabled = true;
            this.comboBoxKnownDevices.Location = new System.Drawing.Point(0, 0);
            this.comboBoxKnownDevices.Name = "comboBoxKnownDevices";
            this.comboBoxKnownDevices.Size = new System.Drawing.Size(117, 21);
            this.comboBoxKnownDevices.TabIndex = 0;
            this.comboBoxKnownDevices.SelectedIndexChanged += new System.EventHandler(this.comboBoxKnownDevices_SelectedIndexChanged);
            // 
            // comboBoxDeviceFields
            // 
            this.comboBoxDeviceFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDeviceFields.FormattingEnabled = true;
            this.comboBoxDeviceFields.Location = new System.Drawing.Point(24, 24);
            this.comboBoxDeviceFields.Name = "comboBoxDeviceFields";
            this.comboBoxDeviceFields.Size = new System.Drawing.Size(93, 21);
            this.comboBoxDeviceFields.TabIndex = 1;
            // 
            // ctlDeviceAndFieldPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxDeviceFields);
            this.Controls.Add(this.comboBoxKnownDevices);
            this.Name = "ctlDeviceAndFieldPicker";
            this.Size = new System.Drawing.Size(117, 45);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxKnownDevices;
        private System.Windows.Forms.ComboBox comboBoxDeviceFields;
    }
}

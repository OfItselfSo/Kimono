namespace Kimono
{
    partial class ctlMonitorBlock_Integral
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
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxIntegerValue = new System.Windows.Forms.TextBox();
            this.textBoxTimeDuration = new System.Windows.Forms.TextBox();
            this.panelBlockDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBlockDisplay
            // 
            this.panelBlockDisplay.Controls.Add(this.textBoxTimeDuration);
            this.panelBlockDisplay.Controls.Add(this.textBoxTitle);
            this.panelBlockDisplay.Controls.Add(this.textBoxIntegerValue);
            this.panelBlockDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelBlockDisplay_MouseDown);
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTitle.Enabled = false;
            this.textBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTitle.Location = new System.Drawing.Point(9, 6);
            this.textBoxTitle.Multiline = true;
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.ReadOnly = true;
            this.textBoxTitle.Size = new System.Drawing.Size(177, 28);
            this.textBoxTitle.TabIndex = 4;
            this.textBoxTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxIntegerValue
            // 
            this.textBoxIntegerValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIntegerValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxIntegerValue.Enabled = false;
            this.textBoxIntegerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIntegerValue.Location = new System.Drawing.Point(9, 28);
            this.textBoxIntegerValue.Name = "textBoxIntegerValue";
            this.textBoxIntegerValue.ReadOnly = true;
            this.textBoxIntegerValue.Size = new System.Drawing.Size(177, 55);
            this.textBoxIntegerValue.TabIndex = 5;
            this.textBoxIntegerValue.Text = "n/a";
            this.textBoxIntegerValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTimeDuration
            // 
            this.textBoxTimeDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTimeDuration.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTimeDuration.Location = new System.Drawing.Point(26, 82);
            this.textBoxTimeDuration.Name = "textBoxTimeDuration";
            this.textBoxTimeDuration.ReadOnly = true;
            this.textBoxTimeDuration.Size = new System.Drawing.Size(142, 13);
            this.textBoxTimeDuration.TabIndex = 6;
            this.textBoxTimeDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ctlMonitorBlock_Integral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ctlMonitorBlock_Integral";
            this.panelBlockDisplay.ResumeLayout(false);
            this.panelBlockDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxIntegerValue;
        private System.Windows.Forms.TextBox textBoxTimeDuration;
    }
}

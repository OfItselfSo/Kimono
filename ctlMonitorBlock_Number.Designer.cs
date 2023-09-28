namespace Kimono
{
    partial class ctlMonitorBlock_Number
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
            this.panelBlockDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBlockDisplay
            // 
            this.panelBlockDisplay.Controls.Add(this.textBoxIntegerValue);
            this.panelBlockDisplay.Controls.Add(this.textBoxTitle);
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
            this.textBoxIntegerValue.Location = new System.Drawing.Point(9, 31);
            this.textBoxIntegerValue.Name = "textBoxIntegerValue";
            this.textBoxIntegerValue.ReadOnly = true;
            this.textBoxIntegerValue.Size = new System.Drawing.Size(177, 55);
            this.textBoxIntegerValue.TabIndex = 5;
            this.textBoxIntegerValue.Text = "n/a";
            this.textBoxIntegerValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ctlMonitorBlock_Number
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ctlMonitorBlock_Number";
            this.panelBlockDisplay.ResumeLayout(false);
            this.panelBlockDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxIntegerValue;
    }
}

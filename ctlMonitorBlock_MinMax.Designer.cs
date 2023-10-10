namespace Kimono
{
    partial class ctlMonitorBlock_MinMax
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
            this.textBoxValueMax = new System.Windows.Forms.TextBox();
            this.textBoxValueMin = new System.Windows.Forms.TextBox();
            this.panelBlockDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBlockDisplay
            // 
            this.panelBlockDisplay.Controls.Add(this.textBoxValueMin);
            this.panelBlockDisplay.Controls.Add(this.textBoxValueMax);
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
            this.textBoxTitle.Size = new System.Drawing.Size(177, 19);
            this.textBoxTitle.TabIndex = 4;
            this.textBoxTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxValueMax
            // 
            this.textBoxValueMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValueMax.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxValueMax.Enabled = false;
            this.textBoxValueMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxValueMax.Location = new System.Drawing.Point(12, 36);
            this.textBoxValueMax.Name = "textBoxValueMax";
            this.textBoxValueMax.ReadOnly = true;
            this.textBoxValueMax.Size = new System.Drawing.Size(177, 24);
            this.textBoxValueMax.TabIndex = 5;
            this.textBoxValueMax.Text = "n/a";
            this.textBoxValueMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxValueMin
            // 
            this.textBoxValueMin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValueMin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxValueMin.Enabled = false;
            this.textBoxValueMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxValueMin.Location = new System.Drawing.Point(12, 66);
            this.textBoxValueMin.Name = "textBoxValueMin";
            this.textBoxValueMin.ReadOnly = true;
            this.textBoxValueMin.Size = new System.Drawing.Size(177, 24);
            this.textBoxValueMin.TabIndex = 6;
            this.textBoxValueMin.Text = "n/a";
            this.textBoxValueMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ctlMonitorBlock_MinMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ctlMonitorBlock_MinMax";
            this.panelBlockDisplay.ResumeLayout(false);
            this.panelBlockDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxValueMax;
        private System.Windows.Forms.TextBox textBoxValueMin;
    }
}

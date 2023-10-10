namespace Kimono
{
    partial class ctlMonitorBlock_Bool
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
            this.textBoxTriggerText = new System.Windows.Forms.TextBox();
            this.panelBlockDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBlockDisplay
            // 
            this.panelBlockDisplay.Controls.Add(this.textBoxTriggerText);
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
            // textBoxTriggerText
            // 
            this.textBoxTriggerText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTriggerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTriggerText.Enabled = false;
            this.textBoxTriggerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTriggerText.Location = new System.Drawing.Point(12, 36);
            this.textBoxTriggerText.Name = "textBoxTriggerText";
            this.textBoxTriggerText.ReadOnly = true;
            this.textBoxTriggerText.Size = new System.Drawing.Size(177, 37);
            this.textBoxTriggerText.TabIndex = 5;
            this.textBoxTriggerText.Text = "n/a";
            this.textBoxTriggerText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ctlMonitorBlock_Bool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ctlMonitorBlock_Bool";
            this.panelBlockDisplay.ResumeLayout(false);
            this.panelBlockDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxTriggerText;
    }
}

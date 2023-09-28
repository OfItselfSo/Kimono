namespace Kimono
{
    partial class ctlMonitorBlockPicker
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
            this.tabControlMBPicker = new System.Windows.Forms.TabControl();
            this.tabPageMBPickerPresets = new System.Windows.Forms.TabPage();
            this.listBoxMBPickerPresets = new System.Windows.Forms.ListBox();
            this.tabPageMBPickerUserDefined = new System.Windows.Forms.TabPage();
            this.listBoxMBPickerUserDefined = new System.Windows.Forms.ListBox();
            this.tabControlMBPicker.SuspendLayout();
            this.tabPageMBPickerPresets.SuspendLayout();
            this.tabPageMBPickerUserDefined.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMBPicker
            // 
            this.tabControlMBPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMBPicker.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControlMBPicker.Controls.Add(this.tabPageMBPickerPresets);
            this.tabControlMBPicker.Controls.Add(this.tabPageMBPickerUserDefined);
            this.tabControlMBPicker.Location = new System.Drawing.Point(0, 0);
            this.tabControlMBPicker.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlMBPicker.Name = "tabControlMBPicker";
            this.tabControlMBPicker.SelectedIndex = 0;
            this.tabControlMBPicker.ShowToolTips = true;
            this.tabControlMBPicker.Size = new System.Drawing.Size(140, 306);
            this.tabControlMBPicker.TabIndex = 0;
            // 
            // tabPageMBPickerPresets
            // 
            this.tabPageMBPickerPresets.Controls.Add(this.listBoxMBPickerPresets);
            this.tabPageMBPickerPresets.Location = new System.Drawing.Point(4, 25);
            this.tabPageMBPickerPresets.Name = "tabPageMBPickerPresets";
            this.tabPageMBPickerPresets.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMBPickerPresets.Size = new System.Drawing.Size(132, 277);
            this.tabPageMBPickerPresets.TabIndex = 0;
            this.tabPageMBPickerPresets.Text = "PreSets";
            this.tabPageMBPickerPresets.ToolTipText = "Pre-defined Monitor Blocks for you to use.";
            this.tabPageMBPickerPresets.UseVisualStyleBackColor = true;
            // 
            // listBoxMBPickerPresets
            // 
            this.listBoxMBPickerPresets.AllowDrop = true;
            this.listBoxMBPickerPresets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMBPickerPresets.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxMBPickerPresets.FormattingEnabled = true;
            this.listBoxMBPickerPresets.Location = new System.Drawing.Point(0, 0);
            this.listBoxMBPickerPresets.Name = "listBoxMBPickerPresets";
            this.listBoxMBPickerPresets.Size = new System.Drawing.Size(132, 277);
            this.listBoxMBPickerPresets.TabIndex = 1;
            this.listBoxMBPickerPresets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxMBPickerPresets_MouseDown);
            // 
            // tabPageMBPickerUserDefined
            // 
            this.tabPageMBPickerUserDefined.Controls.Add(this.listBoxMBPickerUserDefined);
            this.tabPageMBPickerUserDefined.Location = new System.Drawing.Point(4, 25);
            this.tabPageMBPickerUserDefined.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMBPickerUserDefined.Name = "tabPageMBPickerUserDefined";
            this.tabPageMBPickerUserDefined.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMBPickerUserDefined.Size = new System.Drawing.Size(132, 277);
            this.tabPageMBPickerUserDefined.TabIndex = 1;
            this.tabPageMBPickerUserDefined.Text = "User Defined";
            this.tabPageMBPickerUserDefined.ToolTipText = "User Configured Monitor Blocks";
            this.tabPageMBPickerUserDefined.UseVisualStyleBackColor = true;
            // 
            // listBoxMBPickerUserDefined
            // 
            this.listBoxMBPickerUserDefined.AllowDrop = true;
            this.listBoxMBPickerUserDefined.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMBPickerUserDefined.FormattingEnabled = true;
            this.listBoxMBPickerUserDefined.Location = new System.Drawing.Point(0, 0);
            this.listBoxMBPickerUserDefined.Name = "listBoxMBPickerUserDefined";
            this.listBoxMBPickerUserDefined.Size = new System.Drawing.Size(132, 277);
            this.listBoxMBPickerUserDefined.TabIndex = 0;
            this.listBoxMBPickerUserDefined.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxMBPickerUserDefined_DragDrop);
            this.listBoxMBPickerUserDefined.DragOver += new System.Windows.Forms.DragEventHandler(this.listBoxMBPickerUserDefined_DragOver);
            this.listBoxMBPickerUserDefined.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxMBPickerUserDefined_MouseDown);
            // 
            // ctlMonitorBlockPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMBPicker);
            this.MinimumSize = new System.Drawing.Size(140, 150);
            this.Name = "ctlMonitorBlockPicker";
            this.Size = new System.Drawing.Size(140, 306);
            this.tabControlMBPicker.ResumeLayout(false);
            this.tabPageMBPickerPresets.ResumeLayout(false);
            this.tabPageMBPickerUserDefined.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMBPicker;
        private System.Windows.Forms.TabPage tabPageMBPickerPresets;
        private System.Windows.Forms.TabPage tabPageMBPickerUserDefined;
        private System.Windows.Forms.ListBox listBoxMBPickerUserDefined;
        private System.Windows.Forms.ListBox listBoxMBPickerPresets;
    }
}

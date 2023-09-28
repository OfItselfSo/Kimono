namespace Kimono
{
    partial class ctlMonitorBlock_Base
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
            this.panelBlockDisplay = new System.Windows.Forms.Panel();
            this.propertyGridBase = new System.Windows.Forms.PropertyGrid();
            this.buttonTogglePanelAndProperties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelBlockDisplay
            // 
            this.panelBlockDisplay.AllowDrop = true;
            this.panelBlockDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBlockDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBlockDisplay.Location = new System.Drawing.Point(0, 0);
            this.panelBlockDisplay.MaximumSize = new System.Drawing.Size(200, 100);
            this.panelBlockDisplay.MinimumSize = new System.Drawing.Size(200, 100);
            this.panelBlockDisplay.Name = "panelBlockDisplay";
            this.panelBlockDisplay.Size = new System.Drawing.Size(200, 100);
            this.panelBlockDisplay.TabIndex = 0;
            this.panelBlockDisplay.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelBlockDisplay_DragDrop);
            this.panelBlockDisplay.DragOver += new System.Windows.Forms.DragEventHandler(this.panelBlockDisplay_DragOver);
            // 
            // propertyGridBase
            // 
            this.propertyGridBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridBase.HelpVisible = false;
            this.propertyGridBase.Location = new System.Drawing.Point(-20, 0);
            this.propertyGridBase.MaximumSize = new System.Drawing.Size(220, 100);
            this.propertyGridBase.MinimumSize = new System.Drawing.Size(220, 100);
            this.propertyGridBase.Name = "propertyGridBase";
            this.propertyGridBase.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGridBase.Size = new System.Drawing.Size(220, 100);
            this.propertyGridBase.TabIndex = 7;
            this.propertyGridBase.ToolbarVisible = false;
            this.propertyGridBase.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridBase_PropertyValueChanged);
            // 
            // buttonTogglePanelAndProperties
            // 
            this.buttonTogglePanelAndProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTogglePanelAndProperties.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonTogglePanelAndProperties.FlatAppearance.BorderSize = 0;
            this.buttonTogglePanelAndProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTogglePanelAndProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTogglePanelAndProperties.Image = global::Kimono.Properties.Resources.kimono_icon_s;
            this.buttonTogglePanelAndProperties.Location = new System.Drawing.Point(189, 2);
            this.buttonTogglePanelAndProperties.Name = "buttonTogglePanelAndProperties";
            this.buttonTogglePanelAndProperties.Size = new System.Drawing.Size(9, 10);
            this.buttonTogglePanelAndProperties.TabIndex = 1;
            this.buttonTogglePanelAndProperties.TabStop = false;
            this.buttonTogglePanelAndProperties.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonTogglePanelAndProperties.UseVisualStyleBackColor = false;
            this.buttonTogglePanelAndProperties.Click += new System.EventHandler(this.buttonTogglePanelAndProperties_Click);
            // 
            // ctlMonitorBlock_Base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonTogglePanelAndProperties);
            this.Controls.Add(this.panelBlockDisplay);
            this.Controls.Add(this.propertyGridBase);
            this.MaximumSize = new System.Drawing.Size(200, 100);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "ctlMonitorBlock_Base";
            this.Size = new System.Drawing.Size(200, 100);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelBlockDisplay;
        private System.Windows.Forms.PropertyGrid propertyGridBase;
        private System.Windows.Forms.Button buttonTogglePanelAndProperties;
    }
}

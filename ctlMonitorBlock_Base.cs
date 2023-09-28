using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    /// A base class for the monitor blocks, note we inherit from ctlOISBase here
    /// not directly from UserControl. This gives us logging and other things.
    /// </summary>
    public partial class ctlMonitorBlock_Base: ctlOISBase
    {
        // the properties used to configure this MonitorBlock. Will almost always be an object
        // inherited from MonitorBlockProperties_Base
        MonitorBlockProperties_Base monitorBlockProperties = new MonitorBlockProperties_Base();

        // this allows us to notify the subscriber that this ctl should be changed out for 
        // a new one - which can be of a different type
        public frmMain.ReplaceMonitorBlockDelegate ReplaceMonitorBlock = null;

        private bool propertiesHaveChanged = false;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlock_Base()
        {
            InitializeComponent();
            this.panelBlockDisplay.Visible = true;
            this.propertyGridBase.Visible = false;
            this.buttonTogglePanelAndProperties.BringToFront();

            Properties = monitorBlockProperties;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Toggle the visual back and forward between the property panel and the 
        /// display panel.
        /// </summary>
        private void buttonTogglePanelAndProperties_Click(object sender, EventArgs e)
        {
            if(propertyGridBase.Visible == true)
            {
                // bring the display panel to the front
                panelBlockDisplay.Visible = true;
                propertyGridBase.Visible = false;
                // sync the display to the new properties
                SyncDisplayToProperties();
            }
            else
            {
                // have we got a control key pressed?
                if ((Control.ModifierKeys & Keys.Control) == 0)
                {
                    // no, just bring the properties panel to the front
                    panelBlockDisplay.Visible = false;
                    propertyGridBase.Visible = true;
                }
                else
                {
                    // yes, display the properties form
                    frmMonitorBlockProperties frmProp = new frmMonitorBlockProperties();
                    // clone our properties
                    MonitorBlockProperties_Base tmpProp = Properties.DeepClone();
                    // give it to the form
                    frmProp.Properties = tmpProp;
                    // display it modally
                    frmProp.ShowDialog();
                    if (frmProp.DialogResult == DialogResult.OK)
                    {
                        // user pressed save, record the properties
                        Properties = frmProp.Properties;
                    }
                }
            }
            buttonTogglePanelAndProperties.BringToFront();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/sets the properties of the panel. These are actually stored in the
        /// property grid
        /// </summary>
        public MonitorBlockProperties_Base Properties
        {
            get
            {
                if (propertyGridBase.SelectedObject == null) propertyGridBase.SelectedObject = new MonitorBlockProperties_Base();
                return (MonitorBlockProperties_Base)propertyGridBase.SelectedObject;
            }
            set
            {
                propertyGridBase.SelectedObject = value;
                if (propertyGridBase.SelectedObject == null) propertyGridBase.SelectedObject = new MonitorBlockProperties_Base();
                // sync the display to the new properties
                SyncDisplayToProperties();
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the display to the current properties. This only does user
        /// settable things like titles not the data that changes constantly
        /// </summary>
        public virtual void SyncDisplayToProperties()
        {
            // sanity check
            if ((propertyGridBase.SelectedObject is MonitorBlockProperties_Base) == false) return;
            // nothing to do here, override this in the inheritied classes
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Syncs the data on the display. This only
        /// does the changeable data not user settable things like titles.
        /// </summary>
        public virtual void SyncData()
        {
            // sanity check
            if ((propertyGridBase.SelectedObject is MonitorBlockProperties_Base) == false) return;
            // nothing to do here, override this in the inheritied classes
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Drag over event handler
        /// </summary>
        private void panelBlockDisplay_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Drag drop event handler
        /// </summary>
        private void panelBlockDisplay_DragDrop(object sender, DragEventArgs e)
        {
            // if we don't have this delegate we cannot notify the main form to 
            // change the display so there is no point in carrying on.
            if (ReplaceMonitorBlock == null) return;

            // get the data. This is always a MonitorBlockDragDropContainer
            MonitorBlockDragDropContainer ddObj = (MonitorBlockDragDropContainer)e.Data.GetData(typeof(MonitorBlockDragDropContainer));
            if (ddObj == null) return;
            if (ddObj.MBObj == null) return;
            if (ddObj.Sender == null) return;

            MonitorBlockProperties_Base dropItemMBOriginal = ddObj.MBObj;
            ReplaceMonitorBlock(this, dropItemMBOriginal);

        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets called when if the properties have changed
        /// </summary>
        private void propertyGridBase_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // just do this
            PropertiesHaveChanged = true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the properties changed flag
        /// </summary>
        public bool PropertiesHaveChanged
        {
            get
            {
                return propertiesHaveChanged;
            }
            set
            {
                propertiesHaveChanged = value;
            }
        }
    }
}

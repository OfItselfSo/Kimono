using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
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
    /// A class to enable the user to pick the MonitorBlocks they wish to use.
    /// </summary>
    public partial class ctlMonitorBlockPicker : ctlOISBase
    {
        // this allows us to notify the subscriber that a MB should be changed out for 
        // a new one - which can be of a different type
        public frmMain.ReplaceMonitorBlockDelegate ReplaceMonitorBlock = null;
        public frmMain.DeleteMonitorBlockDelegate DeleteMonitorBlock = null;

        private bool changesHaveBeenMade = false;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>
        public ctlMonitorBlockPicker()
        {
            InitializeComponent();
            // set to empty default so the type is correct
            listBoxMBPickerUserDefined.DataSource = new List<MonitorBlockProperties_Base>();
            listBoxMBPickerPresets.DataSource = new List<MonitorBlockProperties_Base>();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the generic MB List
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<MonitorBlockProperties_Base> MBList_UserDefined
        {
            get
            {
                if ((listBoxMBPickerUserDefined.DataSource is List<MonitorBlockProperties_Base>) == false) listBoxMBPickerUserDefined.DataSource = new List<MonitorBlockProperties_Base>();
                return (listBoxMBPickerUserDefined.DataSource as List<MonitorBlockProperties_Base>);
            }
            set
            {
                listBoxMBPickerUserDefined.DataSource = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets/Sets the preset MB List
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<MonitorBlockProperties_Base> MBList_Preset
        {
            get
            {
                if ((listBoxMBPickerPresets.DataSource is List<MonitorBlockProperties_Base>) == false) listBoxMBPickerPresets.DataSource = new List<MonitorBlockProperties_Base>();
                return (listBoxMBPickerPresets.DataSource as List<MonitorBlockProperties_Base>);
            }
            set
            {
                listBoxMBPickerPresets.DataSource = value;
            }
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// flags if changes have been made
        /// </summary>
        public bool ChangesHaveBeenMade { get => changesHaveBeenMade; set => changesHaveBeenMade = value; }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a MouseDown event on the generics/usr listbox
        /// </summary>
        private void listBoxMBPickerUserDefined_MouseDown(object sender, MouseEventArgs e)
        {
            // the only thing we care about here is setting up for a drag out of the listbox
            if (listBoxMBPickerUserDefined.Items.Count == 0)
            {
                return;
            }
            // get the index into the list from the click point
            int index = listBoxMBPickerUserDefined.IndexFromPoint(e.X, e.Y);
            // did we find it
            if ((index < 0) || (index >= listBoxMBPickerUserDefined.Items.Count)) return;
            // get the object backing the data
            MonitorBlockProperties_Base mbObj = MBList_UserDefined[index];
            if (mbObj == null) return;
            if ((Control.ModifierKeys & Keys.Control) == 0)
            {
                // we do not delete the generics
                if (mbObj.DisplayName.StartsWith(MonitorBlockProperties_Base.DEFAULT_DISPLAYNAME_PREFIX) == false)
                {
                    // set, so the caller can remove this
                    mbObj.WantUsrPickerDeleteOnDrop = true;
                }
                else
                {
                    // reset
                    mbObj.WantUsrPickerDeleteOnDrop = false;
                }
            }
            // set the drag drop effects
            MonitorBlockDragDropContainer ddObj = new MonitorBlockDragDropContainer(this, mbObj);
            DragDropEffects dde1 = DoDragDrop(ddObj, DragDropEffects.Copy);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Drag over event handler
        /// </summary>
        private void listBoxMBPickerUserDefined_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Drag drop event handler
        /// </summary>
        private void listBoxMBPickerUserDefined_DragDrop(object sender, DragEventArgs e)
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
            MonitorBlockProperties_Base dropItemMBCopy = dropItemMBOriginal.DeepClone();

            // quick test, if the original is already in the list we are dropping onto ourselves
            if (IsMonitorBlockInUsrDefList(dropItemMBOriginal) == true)
            {
                // reset this 
                dropItemMBOriginal.WantUsrPickerDeleteOnDrop = false;
#if DEBUG
                // in debug mode call this 
               // SavePresetToDisk(dropItemMBOriginal);
#endif 
                return;
            }

            // reset the slotID
            dropItemMBCopy.SlotID = MonitorBlockProperties_Base.DEFAULT_SLOTID;
            // reset this 
            dropItemMBCopy.WantUsrPickerDeleteOnDrop = false;
            // make sure the display name does not contain the text of the generic
            dropItemMBCopy.DisplayName = dropItemMBCopy.DisplayName.Replace(MonitorBlockProperties_Base.DEFAULT_DISPLAYNAME_PREFIX, MonitorBlockProperties_Base.DEFAULT_USRDEF_DISPLAYNAME_PREFIX);
            // add the item to the list
            List<MonitorBlockProperties_Base> mbList = MBList_UserDefined;
            mbList.Add(dropItemMBCopy);
            // we have to do it this way to get it to refresh
            MBList_UserDefined = null;
            MBList_UserDefined = mbList;

            // now replace the one which called us with a blank, unless the control key is held down. 
            // Then we just leave the original in place
            if ((Control.ModifierKeys & Keys.Control) == 0)
            {
                // just delete the existing control on drop
                if (DeleteMonitorBlock != null) DeleteMonitorBlock(dropItemMBOriginal);
            }

            // when dragging one monitor block on top of another we need to perform a swap operation
            ChangesHaveBeenMade = true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Removes an object from the generic/usr list
        /// </summary>
        public void RemoveFromUsrPickerListByObject(MonitorBlockProperties_Base mbObj)
        {
            if (mbObj == null) return;
            // we do not delete the generics
            if (mbObj.DisplayName.StartsWith(MonitorBlockProperties_Base.DEFAULT_DISPLAYNAME_PREFIX) == true) return;

            List<MonitorBlockProperties_Base> mbList = MBList_UserDefined;
            mbList.Remove(mbObj);
            // we have to do it this way to get it to refresh
            MBList_UserDefined = null;
            MBList_UserDefined = mbList;
            ChangesHaveBeenMade = true;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Gets a list of all user defined monitor blocks. Will never return null
        /// </summary>
        public List<MonitorBlockProperties_Base> FindAllUserDefinedMonitorBlocks()
        {
            List<MonitorBlockProperties_Base> outList = new List<MonitorBlockProperties_Base>();
            foreach (MonitorBlockProperties_Base mbObj in MBList_UserDefined)
            {
                // the generics always start with this, UsrDef blocks cannot
                if (mbObj.DisplayName.StartsWith(MonitorBlockProperties_Base.DEFAULT_DISPLAYNAME_PREFIX) == true) continue;
                outList.Add(mbObj);
            }
            return outList;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Detects if a Monitor Block is already in the User defined list
        /// </summary>
        /// <param name="testObj">the Monitor block objec to test</param>
        /// <returns>true - object is in the list, false - it is not</returns>
        public bool IsMonitorBlockInUsrDefList(MonitorBlockProperties_Base testObj)
        {
            if (testObj is null) return false;
            foreach (MonitorBlockProperties_Base mbObj in MBList_UserDefined)
            {
                // conduct the test
                if (Object.ReferenceEquals(testObj, mbObj) == true) return true;
            }
            return false;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// We have to do it this way because the list uses a datasource
        /// </summary>
        public void RefreshUserDefinedMBList()
        {
            List<MonitorBlockProperties_Base> mbList = MBList_UserDefined;
            // we have to do it this way to get it to refresh
            MBList_UserDefined = null;
            MBList_UserDefined = mbList;
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// We have to do it this way because the list uses a datasource
        /// </summary>
        public void RefreshPresetMBList()
        {
            List<MonitorBlockProperties_Base> mbList = MBList_Preset;
            // we have to do it this way to get it to refresh
            MBList_Preset = null;
            MBList_Preset = mbList;
        }


        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Save the presets to disk. This is just a quick way of saving a user def
        /// Monitor Block as a preset. Only available in development mode.
        /// </summary>
        private void SavePresetToDisk(MonitorBlockProperties_Base mbObj)
        {
#if DEBUG
            //// write the User Defs out as a preset - debug mode only
            //if (mbObj == null) return;

            //// figure out the presets directory. In Debug mode when we are writing 
            //// presets this is always a hard coded sub directory of the source code location
            //string presetsPath = Path.Combine(frmMain.LOCAL_SOURCE_CODE_REPO, frmMain.DEFAULT_PRESETS_SUBDIRECTORY);

            //// compose the file name
            //string outFilePathAndName = Path.Combine(presetsPath, mbObj.DisplayName.Replace(" ", "_") + ".xml");

            //// create the serializer
            //XmlSerializer xs = new XmlSerializer(mbObj.GetType());
            //// now emit the xml file using the serializer
            //TextWriter txtWriter = new StreamWriter(outFilePathAndName);
            //xs.Serialize(txtWriter, mbObj);
            //txtWriter.Close();
#endif
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Handle a MouseDown event on the presets listbox
        /// </summary>
        private void listBoxMBPickerPresets_MouseDown(object sender, MouseEventArgs e)
        {
            // the only thing we care about here is setting up for a drag out of the listbox
            if (listBoxMBPickerPresets.Items.Count == 0)
            {
                return;
            }
            // get the index into the list from the click point
            int index = listBoxMBPickerPresets.IndexFromPoint(e.X, e.Y);
            // did we find it
            if ((index < 0) || (index >= listBoxMBPickerPresets.Items.Count)) return;
            // get the object backing the data
            MonitorBlockProperties_Base mbObj = MBList_Preset[index];
            if (mbObj == null) return;
            mbObj.WantUsrPickerDeleteOnDrop = false;
            // set the drag drop effects
            MonitorBlockDragDropContainer ddObj = new MonitorBlockDragDropContainer(this, mbObj);
            DragDropEffects dde1 = DoDragDrop(ddObj, DragDropEffects.Copy);

        }
    }
}

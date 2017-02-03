#region --- Revision History ---
/*
 * 
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.  
 *  Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    Watch
 * 
 *  File name:  FormConfigureChartRecorder.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  04/27/11    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  04/27/11    1.1     K.McD           1.  Modified the SetChartModeRadioButtonsAndMenuOptions() method to use menu keys rather than strings to define the menu options 
 *                                          that are to be modified.
 *                                          
 *  05/23/11    1.2     K.McD           1.  Significant changes to allow the user to define the chart recorder scaling values.
 *                                      2.  Added support for the IChartScale interface.
 *                                      3.  Applied the 'Organize Usings/Remove and Sort' context menu option.
 *                                      4.  Replaced the OK/Apply buttons with the Download tool strip button.
 *                                      5.  Modified the 'Apply' button event handler to download the chart scaling values defined by ListBox controls.
 *                                      
 *  05/24/11    1.3     K.McD           1.  Corrected the chart recorder scaling assignments in the ConvertToWorkset() method.
 *  
 *  05/26/11    1.4     K.McD           1.  Modified the constructor such that the check as to whether the chart recorder configuration downloaded from the VCU matches 
 *                                          an existing workset includes a check on the chart recorder scaling values. As these will always be set to 'not defined' in 
 *                                          the workset downloaded from the VCU this will ensure that a match is never made.
 *                                          
 *                                      2.  Modified the AddSupplementalFields() method such that, if the watch variable does not exist, the string representing 
 *                                          'not defined' is displayed in the ListBox controls associated with the upper and lower Y axis limits rather than 'NaN'.
 *                                          
 *                                      3.  Modified the WatchItemsAddRange() method such that, if the value for the upper and lower Y axis limits stored in the 
 *                                          workset is 'double.NaN', the string representing 'not defined' is displayed in the ListBox controls rather than 'NaN'.
 *                                          
 *                                      4.  Bug fix. Added an override to the ClearListBoxColumnItems() method  to ensure that all ListBox controls associated with 
 *                                          the workset are cleared.
 *                                          
 *                                      5.  Modified the ConvertToWorkset() method to ensure that the upper and lower limits of the Y axis values are converted back 
 *                                          to 'NaN' if the value displayed in the corresponding ListBox control is the string representation of 'not defined'.
 *                                          
 *  06/21/11    1.5     K.McD           1.  SNCR001.141. Removed the feature that allows the user to modify the chart mode, this can now only be carried out from the 
 *                                          main menu.
 *                                          
 *                                              a.  Removed the SetChartModeRadioButtonsAndMenuOptions() method.
 *                                              b.  Removed the SetMenuItemChecked() method.
 *                                              c.  Removed the 'CheckedChanged' event handlers associated with the chart mode radio buttons.
 *                                              d.  Removed the m_ChartModeFromVCU and m_ChartMode member variables.
 *                                              
 *                                      2.  Auto-modified as a result of a name change to an inherited member variable.
 *                                      
 *                                      3.  SNCR001.142. Modified the design such that the default chart recorder workset is presented for download to the VCU when 
 *                                          the form is first shown.
 *                                          
 *  06/30/11    1.6     K.McD           1.  Added support for hexadecimal Y axis limits. The ListBox controls that are used to display the upper and lower limits now 
 *                                          store the limits as text rather than double values.
 *                                          
 *                                              (a) Modified the AddSupplementalFields() method to check whether the Y axis limits are to be displayed in hexadecimal or 
 *                                                  decimal format and to add the default chart scale values, as text strings, to the Items property of the ListBox 
 *                                                  controls associated with the upper and lower Y axis limits.
 *                                                  
 *                                              (b) Modified the WatchItemAddRange() method to check whether the Y axis limits are to be displayed in hexadecimal or 
 *                                                  decimal format and to add the chart scale values, as text string, to the Items property of the ListBox controls 
 *                                                  associated with the upper and lower Y axis limits.
 *                                                  
 *                                              (c) Modified the ConvertToWorkset() method to include support for hexadecimal values and to take into account the fact 
 *                                                  that the ListBox controls associated with the upper and lower Y axis limits now store the chart scale values as 
 *                                                  text strings.
 *                                                  
 *                                              (d) Removed the superfluous code that determined the old identifier associated with the selected watch variable from the 
 *                                                  m_MenuItemChangeChartScaleFactor_Click() method.
 *                                                  
 *  07/20/11    1.7     K.McD           1.  Modified the signature of the constructor to use the ICommunicationParent interface.
 *  
 *  08/05/11    1.8     K.McD           1.  Added an override to the FormConfigure.CompareWorkset() method to include a check on the chart scaling information when 
 *                                          comparing the worksets.
 *                                          
 *                                      2.  No longer de-registers the DataUpdate event in the Cleanup() method as this is not required.
 *                                      
 *  08/09/11    1.9     K.McD           1.  Modified the ConvertToWorkset() method so that it uses the ConvertToWorkset() method of the base class. 
 *  
 *  08/10/11    1.10    Sean.D          1.  Added support for offline mode. Modified the constructor to conditionally choose CommunicationWatch or
 *                                          CommunicationWatchOffline.
 *  
 *  12/01/11    1.11    K.McD           1.  Set the CountMax property of the workset to Parameter.WatchSizeChartRecorder in the ConvertToWorkset() method.
 *  
 *  03/26/15    1.12    K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *  
 *                                          1.  Changes to address the items outlined in the minutes of the meeting between BTPC, 
 *                                              Kawasaki Rail Car and NYTC on 12th April 2013 - MOC-0171:
 *                                               
 *                                              1.  MOC-0171-26. Kawasaki pointed out that Chart Recorder tabs are identified as ‘COLUMNS’ and requested that this
 *                                                  be changed.
 *                                                  
 *                                              2.  MOC-0171-28. A check will be included as part of the ‘Save’ procedure to ensure that an empty workset cannot be saved.
 *                                              
 *                                      2.  SNCR - R188 PTU [20 Mar 2015] Item 6. Bug associated with saving chart recorder worksets that contain watch variables
 *                                          that have different types of FormatString values e.g. 'General Number' and 'Hexadecimal'.
 *                                                  
 *                                      Modifications
 *                                      1.  Ref.: 1.1.1.
 *                                          1.  Modified the constructor so that the TabPage header associated with column 1 is not updated with the workset name.
 *                                          
 *                                      2.  Ref.: 1.1.2.
 *                                          1.  Modified the 'Move Up' and 'Move Down' Button event handlers to call the EnableApplyAndOKButtons() method instead of just
 *                                              setting the 'Enabled' property of the 'Apply' Button to true. This ensures that a full check is made on the state of the
 *                                              workset whenever the Button controls are pressed.
 *                                              
 *                                          2.  Modified the constructor to delete the statement which clears the ' m_NoDefinedWorksed' flag. This flag been
 *                                              replaced by the m_UseTextBoxAsNameSource flag and there is now no need to clear this flag in this module.
 *                                              
 *                                      3.  Ref.: 2.
 *                                          1.  Modified the ConvertToWorkset() and the WatchItemsAddRange() methods to set the local hexFormat variable to
 *                                              either true or false, depending upon the value of the 'FormatString' field. 
 *                                          
 *                                              Previously, it had not been updated on every pass of the 'for' loop so if the workset contained a watch variable
 *                                              that was formatted as 'Hexadecimal' followed by a watch variable that was formatted as 'General Number', the
 *                                              second watch variables would have been processed as a hexadecimal number.
 *                                              
 *  07/11/2016  1.13    K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 14. Chart Recorder Configuration - Change the word 'Download'
 *                                          to 'Upload'.
 *                                          
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 26. Do not include the individual TabPage 1 Count value for the DataStream
 *                                          Configuration screen, just show the Total Count value.
 *                                      
 *                                      Modifications
 *                                      1.  Auto-modified as a result of changing the name of the ToolStripButton control from  m_TSBDownload th m_TSBUpload. - Ref.: 1.
 *                                      2.  Modified the UpdateCount() method to update the TabPage1 'Count' label and the overall 'Total Count:' label. - Ref.: 2.
 *                                      
 *  07/27/2016  1.14    K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 18. Use a double-click to change the chart scale factor on the
 *                                          'Chart Recorder Configuration' dialog box.
 *                                      
 *                                      Modifications
 *                                      1.  De-Registered the m_MenuItemChangeChartScaleFactor_Click() event handler from the 'Column 1' ListBox control 'DoubleClick'
 *                                          event.
 *                                      2.  Modified the m_MenuItemChangeChartScaleFactor_Click() event handler to return immediately if the ModifyEnabled property is
 *                                          not asserted. This is necessary in case the event handler is called as a result of the 'Column 1' ListBox control
 *                                          'DoubleClick' event.
 *                                          
 *  08/01/2016  1.15    K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Items 16, 24, 61.  For the Chart Recorder, Data Stream and Watch Window configuration
 *                                          dialog boxes, change the word 'Workset' to 'Chart Recorder', 'Data Stream' and 'Watch Window' respectively and change the 
 *                                          title of each dialogbox to be 'Configure - Chart Recorder', 'Configure - Data Stream' and 'Configure - Watch Window'.
 *  
 *                                      Modifications
 *                                      1.  Modified the constructor to add appropriate ToolTipText information.
 *                                      
 *  08/10/2016  1.16    K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 17. Add the options for Chart Mode from the original Configure drop-down menu as
 *                                          buttons on the dialogbox used to configure the chart recorder.
 *                                          
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 15, 22, 23, 23, 25, 47, 48. Add 'Delete', 'Set As Default' and 'Override Security'
 *                                          ToolStripButton controls to the Chart Recorder, Data Stream and Watch Window configuration dialogbox forms. On selecting the
 *                                          'Delete' ToolStripButton, a pop-up asking 'Are you sure you want to delete the ...?' should appear with the option to
 *                                          answer 'Yes' or 'Cancel'.
 *                                          
 *                                      3.  PTE Changes - List 5-17-2016.xlsx Item 19, 20, 28. Make the Chart Recorder, Data Stream and Watch Window configuration
 *                                          available from the 'Configure' drop-down menu and remove the 'Manage' window completely.
 *  
 *                                      Modifications
 *                                      1.  Added the m_MenuInterfaceWatch and m_ChartMode member variables. - Ref.: 1.
 *                                      2.  Modified the constructor to: instantiate the m_MenuInterfaceWatch member variable, call the GetChartMode() method to
 *                                          initialize the m_ChartMode member variable, and call the SetChartModeToolStripButtons() method to set up the 'Checked'
 *                                          property of the 'Chart Mode' ToolStripButton controls provided a valid communication ineterfce is defined. - Ref.: 1.
 *                                      3.  Added the event handlers for the 'Click' events associated with the 'Chart Mode' ToolStripButton controls. - Ref.: 1.
 *                                      4.  Added the SetCheckedChartModeToolStripButtons() method. - Ref.: 1.
 *                                      5.  Added the GetChartMode() method. - Ref. 1.
 *                                      6.  Auto-Update - SetEnabledEditNewCopyRename()  renamed to SetEnabledToolStripButtons() in FormConfigure class. - Ref.: 2.
 *                                      7.  Modified the constructor to update the 'Is Default' image associated with the FormConfigure class which identifies whether
 *                                          the selected workset is the default workset. - Ref. 2.
 *                                      8.  Modified the contructor to update the ToolTipText associated with the 'Delete', 'Set As Default' and 'Override Sceurity'
 *                                          ToolStripButton controls. - Ref.: 2.
 *                                      9.  Updated the SetModifyState() method to toggle the ToolTipText depending upon the current state of the ToolStripButton
 *                                          controls. - Ref.: 2.
 *                                      10. Modified the constructor to support a null communicationInterface parameter. If the communicationInterface parameter is
 *                                          null, the form is still shown, however, the 'Upload' ToosStripButton control is disabled. - Ref.: 3.
 *                                      11. Set all Windows Form Designer variables to null and detach all event handlers as part of the clean-up operation. Internal
 *                                          Audit of Code.
 *                                      12. Added the standard check for IsDisposed on all methods that return void. - Internal Audit of Code.
 */
#endregion --- Revision History ---

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Common;
using Common.Communication;
using Common.Configuration;
using Common.Forms;
using Watch.Communication;
using Watch.Properties;

namespace Watch.Forms
{
    /// <summary>
    /// Form to allow the user to configure the chart recorder.
    /// </summary>
    public partial class FormConfigureChartRecorder : FormConfigure, ICommunicationInterface<ICommunicationWatch>, IChartScale
    {
        #region --- Member Variables ---
        /// <summary>
        /// Reference to the selected communication interface.
        /// </summary>
        private ICommunicationWatch m_CommunicationInterface;

        /// <summary>
        /// Reference to the class which calls the menu options associated with the subsystem which shows and records the watch variables - Watch.dll.
        /// </summary>
        private MenuInterfaceWatch m_MenuInterfaceWatch;

        /// <summary>
        /// The current <c>ChartMode</c> of the chart recorder.
        /// </summary>
        private ChartMode m_ChartMode = ChartMode.Undefined;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes an new instance of the form. Zero parameter constructor.
        /// </summary>
        public FormConfigureChartRecorder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes an new instance of the form. Retrieve the chart recorder configuration from the VCU.
        /// </summary>
        /// <remarks>The reference to the main application window is also derived from the CalledFrom parameter, however, this is not obtained until after the form 
        /// has been shown. As a number of multiple document interface child forms (mdi child) may be polling the VCU when this form is instantiated, the 
        /// call to the PauseCommunication() method must be made before the chart configuration data can be retrieved from the VCU from within the constructor code. 
        /// A requirement of the PauseCommunication() method is that the reference to the main application window must be defined.
        /// </remarks>
        /// <param name="communicationInterface">Reference to the communication interface that is to be used to communicate with the VCU.</param>
        /// <param name="mainWindow">Reference to the main application window, this is required for the call to the PauseCommunication() method in the constructor 
        /// code.</param>
        /// <param name="worksetCollection">The workset collection associated with the chart recorder.</param>
        public FormConfigureChartRecorder(ICommunicationParent communicationInterface, IMainWindow mainWindow, WorksetCollection worksetCollection)
            : base(worksetCollection)
        {
            InitializeComponent();

            // Only one column is required for this workset so delete the tab pages associated with columns 2 and 3.
            m_TabControlColumn.TabPages.Remove(m_TabPageColumn2);
            m_TabControlColumn.TabPages.Remove(m_TabPageColumn3);

            Debug.Assert(mainWindow != null, "FormConfigureChartRecorder.Ctor() - [mainWindow != null]");
            m_MainWindow = mainWindow;

            // Move the position of the Cancel buttons.
            m_ButtonCancel.Location = m_ButtonApply.Location;

            // Check the mode of the PTU.
            if (communicationInterface == null)
            {
                CommunicationInterface = null;

                // The PTU is in offline / configuration mode, disable the [Upload] button.
                m_TSBUpload.Enabled = false;
            }
            else if (communicationInterface is CommunicationParent)
            {
                // The PTU is in online mode.
                CommunicationInterface = new CommunicationWatch(communicationInterface);
                PauseCommunication<ICommunicationWatch>(CommunicationInterface, true);
            }
            else
            {
                // The PTU is in simulation mode (originally referred to as offline mode).
                CommunicationInterface = new CommunicationWatchOffline(communicationInterface);
                PauseCommunication<ICommunicationWatch>(CommunicationInterface, true);
            }

            #region - [ToolTipText] -
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonUpload].ToolTipText = Resources.FunctionKeyToolTipChartRecorderUpload;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonSave].ToolTipText = Resources.FunctionKeyToolTipChartRecorderSave;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonEdit].ToolTipText = Resources.FunctionKeyToolTipChartRecorderConfigure;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonNew].ToolTipText = Resources.FunctionKeyToolTipChartRecorderCreate;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonCopy].ToolTipText = Resources.FunctionKeyToolTipChartRecorderCopy;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonRename].ToolTipText = Resources.FunctionKeyToolTipChartRecorderRename;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonDelete].ToolTipText = Resources.FunctionKeyToolTipChartRecorderDelete;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonSetAsDefault].ToolTipText = Resources.FunctionKeyToolTipChartRecorderSetAsDefault;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonOverrideSecurity].ToolTipText = Resources.FunctionKeyToolTipChartRecorderOverrideSecurity;
            #endregion - [ToolTipText] -

            #region - [Chart Mode] -
            // Can only process Chart Mode if the CommunicationsInterface is defined.
            if (CommunicationInterface != null)
            {
                // Allow this class to call the: 'ConfigureChartModeData(), 'ConfigureChartModeRamp', 'ConfigureChartModeFullScale', and 'ConfigureChartModeZeroOutput'
                // methods of the 'MenuInterfaceWatch class. 
                m_MenuInterfaceWatch = new MenuInterfaceWatch(m_MainWindow);

                m_ChartMode = GetChartMode();
            }
            else
            {
                // Don't enable the Chart Mode ToolStripButton controls.
                m_GroupBoxChartMode.Enabled = false;
            }
            SetCheckedChartModeToolStripButtons(m_ChartMode);
            #endregion - [Chart Mode] -

            // Show the context menu that allows the user to configure the chart scaling, however, disable it until the form is in edit mode.
            m_MenuItemChangeChartScaleFactor.Visible = true;
            m_MenuItemChangeChartScaleFactor.Enabled = false;

            // Don't allow the user to edit the workset until the security level of the workset has been established.
            ModifyEnabled = false;

            m_CreateMode = false;

            // Set the structure containing the workset that was downloaded from the VCU to be an empty workset.
            m_WorksetFromVCU = new Workset_t();

            // Get the default chart recorder workset.
            Workset_t workset = worksetCollection.Worksets[worksetCollection.DefaultIndex];

            // Keep a record of the selected workset. This must be set up before the call to SetEnabledToolStripButtons().
            m_SelectedWorkset = workset;

            SetEnabledToolStripButtons(true);

            // Update the 'Default' Image that identifies whether the selected workset is the default workset or not.
            m_PictureBoxDefault.Visible = (m_SelectedWorkset.Name.Equals(m_WorksetCollection.DefaultName)) ? true : false;

            // Display the name of the default workset on the ComboBox control.
            // Ensure that the SelectionChanged event is not triggered as a result of specifying the Text property of the ComboBox control.
            m_ComboBoxWorkset.SelectedIndexChanged -= new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);
            m_ComboBoxWorkset.Text = workset.Name;
            m_ComboBoxWorkset.SelectedIndexChanged += new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);

            LoadWorkset(m_SelectedWorkset);

            // Allow the user to upload the workset if a valid communication interface is defined; otherwise, disable the upload button.
            if (CommunicationInterface != null)
            {
                m_TSBUpload.Enabled = true;
            }
        }
        #endregion --- Constructors ---

        #region --- Cleanup ---
        /// <summary>
        /// Clean up the resources used by the form.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Cleanup(bool disposing)
        {
            try
            {
                // Resume polling of the VCU.
                PauseCommunication<ICommunicationWatch>(CommunicationInterface, false);

                if (disposing)
                {
                    // Cleanup managed objects by calling their Dispose() methods.
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }

                // Whether called by consumer code or the garbage collector free all unmanaged resources and set the value of managed data members to null.
                m_CommunicationInterface = null;

                #region --- Windows Form Designer Variables ---
                // Detach the event handler delegates.
                this.m_ListBox1.DoubleClick -= new System.EventHandler(this.m_MenuItemChangeChartScaleFactor_Click);
                this.m_TSBDataMode.Click -= new System.EventHandler(this.m_TSBDataMode_Click);
                this.m_TSBRampMode.Click -= new System.EventHandler(this.m_TSBRampMode_Click);
                this.m_TSBZeroOutput.Click -= new System.EventHandler(this.m_TSBZeroOutput_Click);
                this.m_TSBFullScale.Click -= new System.EventHandler(this.m_TSBFullScale_Click);

                // Set the Windows Form Designer Variables to null.
                m_ListBoxUnits = null;
                m_ListBoxChartScaleLowerLimit = null;
                m_ListBoxChartScaleUpperLimit = null;
                m_LabelListBoxUnitsColumnHeader = null;
                m_LabelListBoxChartScaleLowerLimitColumnHeader = null;
                m_LabelListBoxChartScaleUpperLimitColumnHeader = null;
                m_LegendListBoxTitle = null;
                m_GroupBoxChartMode = null;
                m_ToolStripChartMode = null;
                m_TSBDataMode = null;
                m_TSSeparatorDataMode = null;
                m_TSSeparatorRampMode = null;
                m_TSBZeroOutput = null;
                m_TSSeparatorZeroOutput = null;
                m_TSBFullScale = null;
                m_TSSeparatorFullScale = null;
                m_TSBRampMode = null;
                #endregion --- Windows Form Designer Variables ---
            }
            catch (Exception)
            {
                // Don't do anything, just ensure that an exception isn't thrown.
            }
            finally
            {
                base.Cleanup(disposing);
            }
        }
        #endregion --- Cleanup ---

        #region --- Delegated Methods ---
        /// <summary>
        /// Event handler for the 'Upload' <c>ToolStripButton</c> <c>Click</c> event. Upload the selected chart recorder workset to the VCU.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected override void m_TSBUpload_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            m_TSBUpload.Checked = true;

            Workset_t workset;
            workset = ConvertToWorkset(m_ComboBoxWorkset.Text);

            // ---------------------------------------------------------------------
            // Check whether the user has modified the chart recorder configuration.
            // ---------------------------------------------------------------------
            if (CompareWorkset(workset, m_WorksetFromVCU) == false)
            {
                // Yes - Ask for confirmation and then download the chart recorder configuration to the VCU.
                DialogResult dialogResult = MessageBox.Show(Resources.MBTQueryModifyChartRecorderChannels, Resources.MBCaptionInformation, MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Information);
                Update();
                if (dialogResult == DialogResult.Yes)
                {
                    bool updateSuccess = UploadWorkset(workset);
                    if (updateSuccess == false)
                    {
                        MessageBox.Show(Resources.MBTModifyChannelsFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        m_TSBUpload.Checked = false;
                        Cursor = Cursors.Default;
                        return;
                    }

                    // Update the record of the chart recorder channel configuration that was retrieved from the VCU.
                    m_WorksetFromVCU = workset;

                    MessageBox.Show(Resources.MBTModifyChartRecorderConfigurationSuccess, Resources.MBCaptionInformation, MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    m_TSBUpload.Enabled = false;
                }
            }

            m_TSBUpload.Checked = false;
            Cursor = Cursors.Default;
            return;
        }

        #region - [Buttons] -
        /// <summary>
        /// Event handler associated with the 'Move Up' button <c>Click</c> event. Move the selected item up the list.
        /// </summary>
        /// <remarks>Only one item may be selected at a time. If more than one item is selected the action will be ignored.</remarks>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected override void m_ButtonMoveUp_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            ListBox.SelectedIndexCollection selectedIndices = m_ListBoxSelected.SelectedIndices;
            int index;
            object previousItem;
            for (int selection = 0; selection < selectedIndices.Count; ++selection)
            {
                index = selectedIndices[selection];

                // Bounds checking.
                if (index >= 1)
                {
                    previousItem = m_ListBoxSelected.Items[index - 1];
                    m_ListBoxSelected.Items.Remove(previousItem);
                    m_ListBoxSelected.Items.Insert(index, previousItem);
                    m_ListBoxSelected.SetSelected(index - 1, true);

                    previousItem = m_ListBoxChartScaleUpperLimit.Items[index - 1];
                    m_ListBoxChartScaleUpperLimit.Items.RemoveAt(index - 1);
                    m_ListBoxChartScaleUpperLimit.Items.Insert(index, previousItem);

                    previousItem = m_ListBoxChartScaleLowerLimit.Items[index - 1];
                    m_ListBoxChartScaleLowerLimit.Items.RemoveAt(index - 1);
                    m_ListBoxChartScaleLowerLimit.Items.Insert(index, previousItem);

                    previousItem = m_ListBoxUnits.Items[index - 1];
                    m_ListBoxUnits.Items.RemoveAt(index - 1);
                    m_ListBoxUnits.Items.Insert(index, previousItem);
                }
                else
                {
                    break;
                }
            }

            EnableApplyAndOKButtons();
            OnDataUpdate(this, new EventArgs());
        }

        /// <summary>
        /// Event handler associated with the 'Move Down' button <c>Click</c> event. Move the selected item down the list.
        /// </summary>
        /// <remarks>Only one item may be selected at a time. If more than one item is selected the action will be ignored.</remarks>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected override void m_ButtonMoveDown_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            ListBox.SelectedIndexCollection selectedIndices = m_ListBoxSelected.SelectedIndices;
            int index;
            object moveItem;
            for (int selection = selectedIndices.Count - 1; selection >= 0; --selection)
            {
                index = selectedIndices[selection];

                // Bounds checking.
                if (index < m_ListBoxSelected.Items.Count - 1)
                {
                    moveItem = m_ListBoxSelected.Items[index];
                    m_ListBoxSelected.Items.Remove(moveItem);
                    m_ListBoxSelected.Items.Insert(index + 1, moveItem);
                    m_ListBoxSelected.SelectedItem = moveItem;

                    moveItem = m_ListBoxChartScaleUpperLimit.Items[index];
                    m_ListBoxChartScaleUpperLimit.Items.RemoveAt(index);
                    m_ListBoxChartScaleUpperLimit.Items.Insert(index + 1, moveItem);

                    moveItem = m_ListBoxChartScaleLowerLimit.Items[index];
                    m_ListBoxChartScaleLowerLimit.Items.RemoveAt(index);
                    m_ListBoxChartScaleLowerLimit.Items.Insert(index + 1, moveItem);

                    moveItem = m_ListBoxUnits.Items[index];
                    m_ListBoxUnits.Items.RemoveAt(index);
                    m_ListBoxUnits.Items.Insert(index + 1, moveItem);
                }
                else
                {
                    break;
                }
            }

            EnableApplyAndOKButtons();
            OnDataUpdate(this, new EventArgs());
        }
        #endregion - [Buttons] -

        /// <summary>
        /// Event handler for the <c>Click</c> event associated with the 'Change Chart Scale' context menu option attached to the <c>ListBox</c> containing the watch 
        /// variables that have been added to the workset.
        /// </summary>
        /// <remarks>This menu option is only relevant to the form used to configure the chart recorder.</remarks>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected override void m_MenuItemChangeChartScaleFactor_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Skip, if the user is not allowed to modify the workset.
            if (ModifyEnabled == false)
            {
                return;
            }

            try
            {
                FormChangeChartScale formChangeChartScale = new FormChangeChartScale(m_ListBox1);
                formChangeChartScale.CalledFrom = this;
                formChangeChartScale.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region - [Chart Mode] -
        /// <summary>
        /// Event handler for the <c>Click</c> event associated with the 'Data Mode' ToolStripButton.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_TSBDataMode_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            try
            {
                m_MenuInterfaceWatch.ConfigureChartModeData();
                m_ChartMode = ChartMode.DataMode;
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.MBTModifyChartRecorderModeFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_ChartMode = ChartMode.Undefined;
            }

            SetCheckedChartModeToolStripButtons(m_ChartMode);
        }

        /// <summary>
        /// Event handler for the <c>Click</c> event associated with the 'Ramp Mode' ToolStripButton.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_TSBRampMode_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            try
            {
                m_MenuInterfaceWatch.ConfigureChartModeRamp();
                m_ChartMode = ChartMode.RampMode;
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.MBTModifyChartRecorderModeFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_ChartMode = ChartMode.Undefined;
            }

            SetCheckedChartModeToolStripButtons(m_ChartMode);
        }

        /// <summary>
        /// Event handler for the <c>Click</c> event associated with the 'Zero Output' ToolStripButton.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_TSBZeroOutput_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            try
            {
                m_MenuInterfaceWatch.ConfigureChartModeZeroOutput();
                m_ChartMode = ChartMode.ZeroOutputMode;
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.MBTModifyChartRecorderModeFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_ChartMode = ChartMode.Undefined;
            }

            SetCheckedChartModeToolStripButtons(m_ChartMode);
        }

        /// <summary>
        /// Event handler for the <c>Click</c> event associated with the 'Full Scale' ToolStripButton.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_TSBFullScale_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            try
            {
                m_MenuInterfaceWatch.ConfigureChartModeFullScale();
                m_ChartMode = ChartMode.FullScaleMode;
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.MBTModifyChartRecorderModeFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_ChartMode = ChartMode.Undefined;
            }

            SetCheckedChartModeToolStripButtons(m_ChartMode);
        }
        #endregion - [Chart Mode] -
        #endregion --- Delegated Methods ---

        #region --- Methods ---
        #region - [FormWorksetDefine] -
        /// <summary>
        /// Add any supplemental workset fields specific to the child class. Add the chart recorder scaling information.
        /// </summary>
        /// <param name="oldIdentifier">The old identifier of the watch variable that is to be added.</param>
        protected override void AddSupplementalFields(int oldIdentifier)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            WatchVariable watchVariable;
            try
            {
                watchVariable = Lookup.WatchVariableTableByOldIdentifier.RecordList[oldIdentifier];
                if (watchVariable == null)
                {
                    throw new Exception();
                }

                if (watchVariable.FormatString.ToLower().Equals(CommonConstants.DDFormatStringHex))
                {
                    m_ListBoxChartScaleLowerLimit.Items.Add(CommonConstants.HexValueIdentifier + ((uint)watchVariable.MinChartScale).ToString(CommonConstants.FormatStringHex));
                    m_ListBoxChartScaleUpperLimit.Items.Add(CommonConstants.HexValueIdentifier + ((uint)watchVariable.MaxChartScale).ToString(CommonConstants.FormatStringHex));
                }
                else
                {
                    m_ListBoxChartScaleLowerLimit.Items.Add(watchVariable.MinChartScale.ToString(CommonConstants.FormatStringNumeric));
                    m_ListBoxChartScaleUpperLimit.Items.Add(watchVariable.MaxChartScale.ToString(CommonConstants.FormatStringNumeric));
                }

                m_ListBoxUnits.Items.Add(watchVariable.Units);
            }
            catch (Exception)
            {
                m_ListBoxChartScaleLowerLimit.Items.Add(CommonConstants.ChartScaleValueNotDefinedString);
                m_ListBoxChartScaleUpperLimit.Items.Add(CommonConstants.ChartScaleValueNotDefinedString);
                m_ListBoxUnits.Items.Add(CommonConstants.ChartScaleUnitsNotDefinedString);
            }

            return;
        }

        /// <summary>
        /// Remove any supplemental workset fields specific to the child class. Remove the chart recorder scaling information.
        /// </summary>
        /// <param name="removeAtIndex">The index of the ListBox row that is to be removed.</param>
        protected override void RemoveSupplementalFields(int removeAtIndex)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ListBoxChartScaleUpperLimit.Items.RemoveAt(removeAtIndex);
            m_ListBoxChartScaleLowerLimit.Items.RemoveAt(removeAtIndex);
            m_ListBoxUnits.Items.RemoveAt(removeAtIndex);
        }

        /// <summary>
        /// Update the count label that shows the number of watch variables that have been added to the chart configuration workset.
        /// </summary>
        protected override void UpdateCount()
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_LabelAvailableCount.Text = Resources.LegendAvailable + CommonConstants.Colon + m_ListBoxAvailable.Items.Count.ToString();

            int countColumn1 = m_ListBox1.Items.Count;
            m_LabelCount1.Text = Resources.LegendCount + CommonConstants.Colon + countColumn1.ToString();

            m_LabelCountTotal.Text = Resources.LegendCount + CommonConstants.Colon + m_ListItemCount.ToString() +
                                     CommonConstants.Space + Resources.LegendOf + CommonConstants.Space + m_WorksetCollection.EntryCountMax.ToString();
        }

        /// <summary>
        /// Add the watch variables defined in the specified workset column to the <c>ListBox</c> controls that display the description and the chart recorder scaling 
        /// information for each chart recorder channel.
        /// </summary>
        /// <param name="listBox">The <c>ListBox</c> to which the items are to be added.</param>
        /// <param name="worksetColumn">The column of the workset that is to be added to the <c>ListBox</c> control.</param>
        protected override void WatchItemAddRange(ListBox listBox, Column_t worksetColumn)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            listBox.Items.Clear();
            m_ListBoxChartScaleUpperLimit.Items.Clear();
            m_ListBoxChartScaleLowerLimit.Items.Clear();
            m_ListBoxUnits.Items.Clear();

            listBox.SuspendLayout();
            m_ListBoxChartScaleLowerLimit.SuspendLayout();
            m_ListBoxChartScaleUpperLimit.SuspendLayout();
            m_ListBoxUnits.SuspendLayout();

            bool hexFormat = false;
            WatchItem_t watchItem;
            short oldIdentifier;
            WatchVariable watchVariable;
            ChartScale_t chartScale;
            for (int index = 0; index < worksetColumn.OldIdentifierList.Count; index++)
            {
                watchItem = new WatchItem_t();
                oldIdentifier = worksetColumn.OldIdentifierList[index];
                watchItem.OldIdentifier = oldIdentifier;
                watchItem.Added = true;
                m_ListBox1.Items.Add(watchItem);

                // Check whether the watch variable exists.
                try
                {
                    watchVariable = Lookup.WatchVariableTableByOldIdentifier.RecordList[oldIdentifier];
                    if (watchVariable == null)
                    {
                        throw new Exception();
                    }

                    // Determine the format of the watch variable.
                    hexFormat = (watchVariable.FormatString.ToLower().Equals(CommonConstants.DDFormatStringHex)) ? true : false;

                    // Check whether the chart scaling for the current watch variable has been defined.
                    try
                    {
                        chartScale = worksetColumn.ChartScaleList[index];
                    }
                    catch (Exception)
                    {
                        // No - Set up the default chart scaling for the watch variable based upon the data dictionary.
                        chartScale = new ChartScale_t();
                        chartScale.ChartScaleLowerLimit = watchVariable.MinChartScale;
                        chartScale.ChartScaleUpperLimit = watchVariable.MaxChartScale;
                        chartScale.Units = watchVariable.Units;
                    }
                }
                catch (Exception)
                {
                    // Watch variable does not exist.
                    chartScale.ChartScaleLowerLimit = double.NaN;
                    chartScale.ChartScaleUpperLimit = double.NaN;
                    chartScale.Units = CommonConstants.ChartScaleUnitsNotDefinedString;
                }

                // Rather tha displaying 'NaN' if the chart scale values are undefined, display the default string used to represent a chart scale value that is not defined.
                if (chartScale.ChartScaleLowerLimit.Equals(double.NaN))
                {
                    m_ListBoxChartScaleLowerLimit.Items.Add(CommonConstants.ChartScaleValueNotDefinedString);
                }
                else
                {
                    if (hexFormat == true)
                    {
                        m_ListBoxChartScaleLowerLimit.Items.Add(CommonConstants.HexValueIdentifier + ((uint)chartScale.ChartScaleLowerLimit).ToString(CommonConstants.FormatStringHex));
                    }
                    else
                    {
                        m_ListBoxChartScaleLowerLimit.Items.Add(chartScale.ChartScaleLowerLimit.ToString(CommonConstants.FormatStringNumeric));
                    }
                }

                if (chartScale.ChartScaleUpperLimit.Equals(double.NaN))
                {
                    m_ListBoxChartScaleUpperLimit.Items.Add(CommonConstants.ChartScaleValueNotDefinedString);
                }
                else
                {
                    if (hexFormat == true)
                    {
                        m_ListBoxChartScaleUpperLimit.Items.Add(CommonConstants.HexValueIdentifier + ((uint)chartScale.ChartScaleUpperLimit).ToString(CommonConstants.FormatStringHex));
                    }
                    else
                    {
                        m_ListBoxChartScaleUpperLimit.Items.Add(chartScale.ChartScaleUpperLimit.ToString(CommonConstants.FormatStringNumeric));
                    }
                }

                m_ListBoxUnits.Items.Add(chartScale.Units);
            }

            listBox.PerformLayout();
            m_ListBoxChartScaleLowerLimit.PerformLayout();
            m_ListBoxChartScaleUpperLimit.PerformLayout();
            m_ListBoxUnits.PerformLayout();

            Cursor = Cursors.Default;
        }
        #endregion - [FormWorksetDefine] -

        #region - [FormConfigure] -
        /// <summary>
        /// Upload the specified workset to the VCU.
        /// </summary>
        /// <param name="workset">The workset that is to be downloaded to the VCU.</param>
        /// <returns>A flag that indicates whether the workset was successfully downloaded to the VCU. True, if the VCU update was successful; otherwise, false.</returns>
        protected override bool UploadWorkset(Workset_t workset)
        {
            // A flag that indicates whether the chart recorder update was successful.
            bool updateSuccess;

            try
            {
                CommunicationInterface.DownloadChartRecorderWorkset(workset);
                updateSuccess = true;
            }
            catch (Exception)
            {
                updateSuccess = false;
            }

            return updateSuccess;
        }

        /// <summary>
        /// Set the modify state to the specified state.
        /// </summary>
        /// <param name="modifyState">The required modify state.</param>
        protected override void SetModifyState(ModifyState modifyState)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            base.SetModifyState(modifyState);

            if (m_ModifyState == ModifyState.Configure)
            {
                // Restore the ToolTipText properties of the ToolStripButtons.
                UpdateFunctionKey(m_TSBEdit, string.Empty, Resources.FunctionKeyToolTipChartRecorderConfigure, null);
                UpdateFunctionKey(m_TSBNew, string.Empty, Resources.FunctionKeyToolTipChartRecorderCreate, null);
                UpdateFunctionKey(m_TSBCopy, string.Empty, Resources.FunctionKeyToolTipChartRecorderCopy, null);
                UpdateFunctionKey(m_TSBRename, string.Empty, Resources.FunctionKeyToolTipChartRecorderRename, null);
                UpdateFunctionKey(m_TSBDelete, string.Empty, Resources.FunctionKeyToolTipChartRecorderDelete, null);
                UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, Resources.FunctionKeyToolTipChartRecorderSetAsDefault, null);
                UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, Resources.FunctionKeyToolTipChartRecorderOverrideSecurity, null);

                // Only enable the Chart Mode ToolStripButton controls if a valid CommunicationInterface is defined.
                if (CommunicationInterface != null)
                {
                    m_GroupBoxChartMode.Enabled = true;
                }

                m_TSBUpload.Enabled = ((ConfigurationModified() == true) && (CommunicationInterface != null)) ? true : false;
            }
            else
            {
                switch (m_ModifyState)
                {
                    case ModifyState.Configure:
                        
                        break;
                    case ModifyState.Edit:
                        UpdateFunctionKey(m_TSBEdit, string.Empty, Resources.FunctionKeyToolTipChartRecorderConfigureUndo, null);
                        break;
                    case ModifyState.Create:
                        UpdateFunctionKey(m_TSBNew, string.Empty, Resources.FunctionKeyToolTipChartRecorderCreateUndo, null);
                        break;
                    case ModifyState.Copy:
                        UpdateFunctionKey(m_TSBCopy, string.Empty, Resources.FunctionKeyToolTipChartRecorderCopyUndo, null);
                        break;
                    case ModifyState.Rename:
                        UpdateFunctionKey(m_TSBRename, string.Empty, Resources.FunctionKeyToolTipChartRecorderRenameUndo, null);
                        break;
                    case ModifyState.Delete:
                        UpdateFunctionKey(m_TSBDelete, string.Empty, Resources.FunctionKeyToolTipChartRecorderDeleteUndo, null);
                        break;
                    case ModifyState.SetAsDefault:
                        UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, Resources.FunctionKeyToolTipChartRecorderSetAsDefaultUndo, null);
                        break;
                    case ModifyState.OverrideSecurity:
                        UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, Resources.FunctionKeyToolTipChartRecorderOverrideSecurityUndo, null);
                        break;
                    default:
                        break;
                }

                m_GroupBoxChartMode.Enabled = false;

                // Disable the Upload button.
                m_TSBUpload.Enabled = false;
            }
        }

        /// <summary>
        /// Convert the current user setting to a workset.
        /// </summary>
        /// <remarks>
        /// The conversion includes the chart scaling parameters.
        /// </remarks>
        /// <param name="worksetName">The name of the workset.</param>
        /// <returns>The user settings converted to a workset.</returns>
        protected override Workset_t ConvertToWorkset(string worksetName)
        {
            Workset_t workset = base.ConvertToWorkset(worksetName);
            workset.CountMax = Parameter.WatchSizeChartRecorder;

            #region - [ChartScaleList] -
            workset.Column[0].ChartScaleList = new List<ChartScale_t>();

            // The old identifer associated with the current watch variable.
            short oldIdentifier;

            // The current watch variable.
            WatchVariable watchVariable;

            // A flag to indicate whether the current watch variable is to be displayed in hexadecimal format. True, if the variable is to be displayed in hex format.
            bool hexFormat = false;

            // The upper and lower chart scale values as a text string.
            string chartScaleLowerLimitText, chartScaleUpperLimitText;
            ChartScale_t chartScale = new ChartScale_t();

            // The current culture information. Used to parse hexadecimal values.
            CultureInfo provider = new CultureInfo(CommonConstants.CultureInfoString);

            // A flag to indicate whether the parse operationwas successful. True, if the parse was successful.
            bool successfulParse;
            for (int index = 0; index < m_ListBox1.Items.Count; index++)
            {
                // Get the watch variable associated with the current item.
                oldIdentifier = ((WatchItem_t)m_ListBox1.Items[index]).OldIdentifier;
                try
                {
                    watchVariable = Lookup.WatchVariableTableByOldIdentifier.Items[oldIdentifier];
                    if (watchVariable == null)
                    {
                        throw new Exception();
                    }

                    // Determine the format of the watch variable.
                    hexFormat = (watchVariable.FormatString.ToLower().Equals(CommonConstants.DDFormatStringHex)) ? true : false;

                    chartScaleLowerLimitText = (string)m_ListBoxChartScaleLowerLimit.Items[index];
                    chartScaleUpperLimitText = (string)m_ListBoxChartScaleUpperLimit.Items[index];

                    if (m_ListBoxChartScaleLowerLimit.Items[index].Equals(CommonConstants.ChartScaleValueNotDefinedString))
                    {
                        chartScale.ChartScaleLowerLimit = double.NaN;
                    }
                    else
                    {
                        if (hexFormat == true)
                        {
                            uint lowerLimitAsUInt32;
                            string strippedChartScaleLowerLimitText;

                            // Strip out the leading HexValueIdentifier.
                            Debug.Assert(chartScaleLowerLimitText.Contains(CommonConstants.HexValueIdentifier), "FormConfigureChartRecorder.ConvertToWorkset() - [chartScaleLowerLimitText.Contains(HexValueIdentifier)]");
                            strippedChartScaleLowerLimitText = chartScaleLowerLimitText.Remove(0, CommonConstants.HexValueIdentifier.Length);

                            // Check that the value entered is a valid 32 bit hexadecimal value.
                            successfulParse = UInt32.TryParse(strippedChartScaleLowerLimitText, NumberStyles.HexNumber, provider, out lowerLimitAsUInt32);
                            if (successfulParse == false)
                            {
                                chartScale.ChartScaleLowerLimit = double.NaN;
                            }
                            else
                            {
                                chartScale.ChartScaleLowerLimit = lowerLimitAsUInt32;
                            }
                        }
                        else
                        {
                            double lowerLimitAsDouble;

                            // Check that the value entered is a valid 32 bit decimal value.
                            successfulParse = double.TryParse(chartScaleLowerLimitText, out lowerLimitAsDouble);
                            if (successfulParse == false)
                            {
                                chartScale.ChartScaleLowerLimit = double.NaN;
                            }
                            else
                            {
                                chartScale.ChartScaleLowerLimit = lowerLimitAsDouble;
                            }
                        }
                    }

                    if (m_ListBoxChartScaleUpperLimit.Items[index].Equals(CommonConstants.ChartScaleValueNotDefinedString))
                    {
                        chartScale.ChartScaleUpperLimit = double.NaN;
                    }
                    else
                    {
                        if (hexFormat == true)
                        {
                            uint upperLimitAsUInt32;
                            string strippedChartScaleUpperLimitText;

                            // Strip out the leading HexValueIdentifier.
                            Debug.Assert(chartScaleUpperLimitText.Contains(CommonConstants.HexValueIdentifier), "FormConfigureChartRecorder.ConvertToWorkset() - [chartScaleUpperLimitText.Contains(HexValueIdentifier)]");
                            strippedChartScaleUpperLimitText = chartScaleUpperLimitText.Remove(0, CommonConstants.HexValueIdentifier.Length);

                            // Check that the value entered is a valid 32 bit hexadecimal value.
                            successfulParse = UInt32.TryParse(strippedChartScaleUpperLimitText, NumberStyles.HexNumber, provider, out upperLimitAsUInt32);
                            if (successfulParse == false)
                            {
                                chartScale.ChartScaleUpperLimit = double.NaN;
                            }
                            else
                            {
                                chartScale.ChartScaleUpperLimit = upperLimitAsUInt32;
                            }
                        }
                        else
                        {
                            double upperLimitAsDouble;

                            // Check that the value entered is a valid 32 bit decimal value.
                            successfulParse = double.TryParse(chartScaleUpperLimitText, out upperLimitAsDouble);
                            if (successfulParse == false)
                            {
                                chartScale.ChartScaleUpperLimit = double.NaN;
                            }
                            else
                            {
                                chartScale.ChartScaleUpperLimit = upperLimitAsDouble;
                            }
                        }
                    }

                    chartScale.Units = (string)m_ListBoxUnits.Items[index];
                    workset.Column[0].ChartScaleList.Add(chartScale);
                }
                catch (Exception)
                {
                    // The watch variable does not exist, add an empty chart scale value.
                    chartScale.ChartScaleLowerLimit = double.NaN;
                    chartScale.ChartScaleUpperLimit = double.NaN;
                    chartScale.Units = CommonConstants.ChartScaleUnitsNotDefinedString;
                    workset.Column[0].ChartScaleList.Add(chartScale);
                }
            }
            #endregion - [ChartScaleList] -

            return workset;
        }

        /// <summary>
        /// Clear the items in the 'Column' ListBox controls.
        /// </summary>
        protected override void ClearListBoxColumnItems()
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ListBox1.Items.Clear();
            m_ListBoxChartScaleLowerLimit.Items.Clear();
            m_ListBoxChartScaleUpperLimit.Items.Clear();
            m_ListBoxUnits.Items.Clear();
        }

        /// <summary>
        /// Predicate function called by the <c>List.Find()</c> method to return a workset that matches the specified workset, ignoring the Name, SecurityLevel and 
        /// HeaderText fields of each workset. 
        /// </summary>
        /// <param name="workset">The list item that is to be processed.</param>
        /// <returns>True, if the specified item meets the logic requirements given in the function; otherwise false.</returns>
        protected override bool CompareWorkset(Workset_t workset)
        {
            Workset_t worksetToCompare = m_WorksetToCompare;

            if (base.CompareWorkset(workset) == false)
            {
                return false;
            }
            else
            {
                #region - [ChartScaleList] -
                for (int columnIndex = 0; columnIndex < workset.Column.Length; columnIndex++)
                {
                    for (int index = 0; index < workset.Column[columnIndex].ChartScaleList.Count; index++)
                    {
                        if ((workset.Column[columnIndex].ChartScaleList[index].ChartScaleLowerLimit != 
                             worksetToCompare.Column[columnIndex].ChartScaleList[index].ChartScaleLowerLimit) ||
                            (workset.Column[columnIndex].ChartScaleList[index].ChartScaleUpperLimit != 
                             worksetToCompare.Column[columnIndex].ChartScaleList[index].ChartScaleUpperLimit) ||
                            (workset.Column[columnIndex].ChartScaleList[index].Units != worksetToCompare.Column[columnIndex].ChartScaleList[index].Units))
                        {
                            return false;
                        }
                    }
                }
                #endregion - [ChartScaleList] -
            }

            return true;
        }
        #endregion - [FormConfigure] -

        /// <summary>
        /// Set the <c>Checked</c> properties of the 'Chart Mode' ToolStripButton controls according the the specified chart mode.
        /// </summary>
        /// <param name="chartMode">The current mode of the Chart Recorder.</param>
        private void SetCheckedChartModeToolStripButtons(ChartMode chartMode)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_TSBDataMode.Checked = false;
            m_TSBRampMode.Checked = false;
            m_TSBZeroOutput.Checked = false;
            m_TSBFullScale.Checked = false;

            switch (chartMode)
            {
                case ChartMode.DataMode:
                    m_TSBDataMode.Checked = true;
                    break;
                case ChartMode.RampMode:
                    m_TSBRampMode.Checked = true;
                    break;
                case ChartMode.ZeroOutputMode:
                    m_TSBZeroOutput.Checked = true;
                    break;
                case ChartMode.FullScaleMode:
                    m_TSBFullScale.Checked = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get the mode of the chart recorder e.g. Data, Ramp, Full Scale or Zero. 
        /// </summary>
        /// <returns>The current <c>ChartMode</c> of the Chart Recorder; otherwise, if unsuccessful, ChartMode.Undefined.</returns>
        private ChartMode GetChartMode()
        {
            ChartMode chartMode = ChartMode.Undefined;

            try
            {
                // Get the current chart mode.
                chartMode = CommunicationInterface.GetChartMode();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                chartMode = ChartMode.Undefined;
            }

            return chartMode;
        }
        #endregion --- Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets or sets the communication interface that is to be used to communicate with the target.
        /// </summary>
        /// <remarks>This property is set by the child class, if appropriate.</remarks>
        public ICommunicationWatch CommunicationInterface
        {
            get { return m_CommunicationInterface; }
            set { m_CommunicationInterface = value; }
        }

        /// <summary>
        /// Gets the reference to the <c>ListBox</c> control that contains the lower limits of the Y axis for each chart recorder channel.
        /// </summary>
        public ListBox ListBoxChartScaleLowerLimit
        {
            get  { return m_ListBoxChartScaleLowerLimit; }
        }

        /// <summary>
        /// Gets the reference to the <c>ListBox</c> control that contains the upper limits of the Y axis for each chart recorder channel.
        /// </summary>
        public ListBox ListBoxChartScaleUpperLimit
        {
            get { return m_ListBoxChartScaleUpperLimit; }
        }

        /// <summary>
        /// Gets the refernce to the 'Save' <c>ToolStripButton</c> control.
        /// </summary>
        public ToolStripButton TSBSave
        {
            get { return m_TSBSave; }
        }
        #endregion --- Properties ---
    }
}
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
 *  Project:    Event
 * 
 *  File name:  FormConfigureFaultLogParametersNew.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  11/16/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  01/06/11    1.1     K.McD           1.  Added the m_WorksetFromVCU member variable to store a copy of the stream parameters retrieved from the VCU in workset format.
 *                                      2.  Modified the event handler for the Apply button Click event to include a check to determine if the data stream 
 *                                          parameters have been changed. If the parameters have not been changed no action is taken; otherwise: (a) the event log is 
 *                                          saved to disk and then cleared and (b) the updated data stream parameters are downloaded to the VCU.
 *                                      3.  Included an additional signature for the CompareWorkset() method.
 * 
 *  01/31/11    1.2     K.McD           1.  Modified the Apply button event handler to use the ClearCurrentEventLog() method of the FormViewEventLog class to clear 
 *                                          the current event log.
 * 
 *  02/21/11    1.3     K.McD           1.  Modified the event handler for the Apply button such that it no longer clears the current log before downloading the new 
 *                                          parameters.
 *                                      2.  Auto-modified as a result of the method name change to UpdateSampleMultiple().
 * 
 *  02/28/11    1.4     K.McD           1.  Modified the constructor such that the name of the current workset is shown on the combo box control.
 *                                      2.  Modified to accommodate the signature change associated with the ConvertToWorkset() method.
 *                                      3.  Modified the event handler for the Save function key to ask for confirmation before updating a saved workset.
 * 
 *  03/17/11    1.5     K.McD           1.  Auto-modified as a result of property name changes associated with the Common.Security class.
 * 
 *  03/28/11    1.6     K.McD           1.  Auto-modified as a result of a number of name changes to the properties and methods of external classes.
 * 
 *  04/27/11    1.7     K.McD           1.  Renamed to FormConfigureFaultLogParameters from FormSetupStream.
 *                                      2.  Auto-modified as a result of a name change to a member variable inherited from the parent class.
 *                                      3.  Added a check as to whether the class has been disposed of to a number of methods.
 *                                      4.  Auto-modified as a result of name changes to a number of resources.
 *                                      5.  Modified the tool tip text depending upon the state of the form.
 *                                      6.  Modified the DropDownStyle property of the ComboBox depending upon whether the workset exists or not.
 *                                      
 *  05/23/11    1.8     K.McD           1.  Modified the LoadWorkset() method to accommodate the signature change to the FormWorksetDefine.WatchItemAddRange() method.
 *  
 *  06/21/11    1.9     K.McD           1.  Auto-modified as a result of a name change to an inherited member variable.
 *  
 *  07/20/11    1.10    K.McD           1.  Modified the signature of the constructor to use the ICommunicationParent interface.
 *  
 *  08/05/11    2.0     K.McD           1.  Major changes, now inherits from the FormConfigure class.
 *  
 *  08/10/11    2.1     Sean.D          1.  Included support for offline mode. Modified the constructor to conditionally choose CommunicationEvent or
 *                                          CommunicationEventOffline.
 *  
 *  12/01/11    2.2     K.McD           1.  Set the CountMax property of the workset to Parameter.WatchSizeFaultLogMax in the ConvertToWorkset() method.
 *                                      2.  Modified the m_NumericUpDownSampleMultiple_ValueChanged() method such that the DataUpdate event is only triggered if 
 *                                          the ModifyEnabled property is asserted.
 *                                          
 *  07/24/13    2.3     K.McD           1.  Included update of the 'Total Count' label in the UpdateCount() method.
 *                                      2.  Automatic update when all references to the Parameter.WatchSizeFaultLogMax constant were replaced by references to the
 *                                          Parameter.WatchSizeFaultLog property.
 *                                      3.  Modified the event handler for the Download key to check whether the number of watch variables associated with the current
 *                                          workset exceeds the number supported by the current event log.
 *                                      4.  Modified the signature of the constructor to include a reference to the current event log and used this to update the
 *                                          EntryCountMax property of the class.
 *                                          
 *  03/26/15    2.4     K.McD           1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *  
 *                                          1.  Changes to address the items outlined in the minutes of the meeting between BTPC, 
 *                                              Kawasaki Rail Car and NYTC on 12th April 2013 - MOC-0171:
 *                                              
 *                                              1. MOC-0171-26. Kawasaki pointed out that Chart Recorder tabs are identified as ‘COLUMNS’ and requested that this
 *                                                 be changed.
 *                                                 
 *                                              2.  MOC-0171-28. A check will be included as part of the ‘Save’ procedure to ensure that an empty workset cannot be saved.
 *                                              
 *                                      2.  SNCR - R188 PTU [20 Mar 2015] Item 7. While attempting to configure a data stream, the set of parameters that were downloaded
 *                                          from the VCU were not defined in an existing workset, consequently FormConfigureFaultLogParameters entered 'Create' mode.
 *                                          While in this mode, the PTU displayed the workset parameters that were downloaded from the VCU and gave the user the
 *                                          opportunity to name the workset but not the opportunity to Save the workset. Modify the code to ensure that the new workset
 *                                          can be saved.
 *                                                  
 *                                      Modifications
 *                                      1.  Ref.: 1.1.1.
 *                                          1.  Modified the constructor so that the TabPage header associated with column 1 is not updated with the workset name.
 *                                      
 *                                          2.  Modified the constructor to check whether the project supports multiple data stream types or whether
 *                                              the number of parameters supported by the data stream exceeds the number that can be displayed on the TabPage without
 *                                              requiring scroll bars and call AddRowHeader() or NoRowHeader() as appropriate.
 *                                          
 *                                          3.  Included the constant WatchSizeFaultLogMax which defines the number of parameters that can be displayed on the TabPage
 *                                              without requiring scroll bars.
 *                                              
 *                                          4.  Modified the constructor to disable all ToolStrip buttons, except the Save button, if the workset downloaded from the
 *                                              Vehicle Control Unit does not already exist. Also removed any references to the  m_NoDefinedWorkset flag as this is
 *                                              no longer used. It has been replaced by the m_UseTextBoxAsNameSource flag.
 *                                              
 *                                      2.  Ref.: 1.1.2, 2.
 *                                          1.  Modified the m_NumericUpDownSampleMultiple_ValueChanged() method to call the EnableApplyAndOKButtons() method. This
 *                                              ensures that a full check is made on the state of the workset whenever the 'Sample Multiple' value is changed.
 *                                          2.  Wherever the Text property of the 'm_TextBoxName' TextBox was changed the code was modified to ensure that this didn't
 *                                              trigger the TextChanged() event handler by de-registering the event handler prior to setting the property and then
 *                                              re-registering it again.
 *                                          3.  Modified the constructor to set the state of the m_NoDefinedWorkset flag to true if the workset downloaded from the
 *                                              VCU did not match an existing workset. It had previously, incorrectly been set to false.
 *                                          4.  Modified the constructor to enable or disable the Save ToolStrip button depending upon whether the workset
 *                                              downloaded from the VCU matched an existing workset or not. Enabled if the downloaded workset did not exist; otherwise,
 *                                              false.
 *                                              
 *  07/11/2016  2.5     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 14. Chart Recorder Configuration - Change the word 'Download'
 *                                          to 'Upload'.
 *                                          
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 26. Do not include the individual TabPage 1 Count value for the DataStream
 *                                          Configuration screen, just show the Total Count value. 
 *                                      
 *                                      Modifications
 *                                      1.  Auto-modified as a result of changing the name of the ToolStripButton control from  m_TSBDownload th m_TSBUpload. - Ref.: 1.
 *                                      2.  Modified the UpdateCount() method to display 'Count' rather than 'Total Count'.
 *                                      3.  Modified the UpdateCount() method to update the TapPage1 count for consistency with FormWorksetDefineFaultLog.cs.
 *                                      
 *  08/01/2016  2.6     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Items 16, 24, 61.  For the Chart Recorder, Data Stream and Watch Window configuration
 *                                          dialog boxes, change the word 'Workset' to 'Chart Recorder', 'Data Stream' and 'Watch Window' respectively and change the 
 *                                          title of each dialogbox to be 'Configure - Chart Recorder', 'Configure - Data Stream' and 'Configure - Watch Window'.
 *  
 *                                      Modifications
 *                                      1.  Modified the constructor to add appropriate ToolTipText information.
 */

/*
 *  09/06/2016  2.7     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 15, 22, 23, 23, 25, 47, 48. Add 'Delete', 'Set As Default' and 'Override Security'
 *                                          ToolStripButton controls to the Chart Recorder, Data Stream and Watch Window configuration dialogbox forms. On selecting the
 *                                          'Delete' ToolStripButton, a pop-up asking 'Are you sure you want to delete the ...?' should appear with the option to
 *                                          answer 'Yes' or 'Cancel'.
 *                                          
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 19, 20, 28. Make the Chart Recorder, Data Stream and Watch Window configuration
 *                                          available from the 'Configure' drop-down menu and remove the 'Manage' window completely.
 *  
 *                                      Modifications
 *                                      1.  Auto-Update - SetEnabledEditNewCopyRename()  renamed to SetEnabledToolStripButtons() in FormConfigure class. - Ref.: 1.
 *                                      2.  Modified the constructor to update the 'Is Default' image associated with the FormConfigure class which identifies whether
 *                                          the selected workset is the default workset. - Ref. 2.
 *                                      3.  Modified the contructor to update the ToolTipText associated with the 'Delete', 'Set As Default' and 'Override Sceurity'
 *                                          ToolStripButton controls. - Ref.: 1.
 *                                      4.  Modified the constructor to support a null communicationInterface parameter. If the communicationInterface parameter is
 *                                          null, the form is still shown, however, the 'Upload' ToosStripButton control is disabled and the default workset is
 *                                          loaded up on start-up. - Ref.: 2.
 *                                      5.  Modified the constructor to check whether the log parameter has been specified and, if so, to update the m_EntryCountMax
 *                                          member variable to reflect the maximum number of watch variables that the current event log supports rather than to the
 *                                          maximum number that is supported by the workset collection.
 *                                      6.  Updated the SetModifyState() method to toggle the ToolTipText depending upon the current state of the ToolStripButton
 *                                          controls. Also toggles the state of the Enabled properties of the 'Upload' ToolStripButton control and the NumericUpDown
 *                                          control. - Ref.: 1.
 *                                      7.  Auto-Update -  DownloadWorkset() renamed to UploadWorkset().
 *                                      8.  Set all Windows Form Designer variables to null and detach all event handlers as part of the clean-up operation. Internal
 *                                          Audit of Code.
 *                                      9.  Added the standard check for IsDisposed on all methods that return void. - Internal Audit of Code.
 *                                      10. Added a form 'Shown' event handler to hide the 'Upload' ToolStripButton control if this form was called from the 
 *                                          'Configure/Data Stream' menu option. Each event log supports one fault log data stream and these can only be configured
 *                                          if the an event log is selected, therefore the 'Upload' button should only be made available when this form is called from
 *                                          the 'F8-Stream' button on the 'Diagnostices/Event Log' form.
 */
#endregion --- Revision History ---

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Common;
using Common.Forms;
using Common.Communication;
using Common.Configuration;
using Event.Communication;
using Event.Properties;

namespace Event.Forms
{
    /// <summary>
    /// Form to allow the user to define: (a) the watch variables that are associated with a fault log data stream (b) the order in which they are to be displayed and 
    /// (c) the multiple of the recording interval at which the data is to be recorded.
    /// </summary>
    public partial class FormConfigureFaultLogParameters : FormConfigure, ICommunicationInterface<ICommunicationEvent>
    {
        #region --- Constants ---
        /// <summary>
        /// The maximum FaultLog/DataStream WatchSize that can be displayed using the 'Row Header' <c>ListBox</c>. Value: 16.  
        /// </summary>
        private const int WatchSizeFaultLogMax = 16;
        #endregion --- Constants ---

        #region --- Member Variables ---
        /// <summary>
        /// Reference to the selected communication interface.
        /// </summary>
        private ICommunicationEvent m_CommunicationInterface;

        /// <summary>
        /// The multiple of the recording interval at which the data is to be recorded.
        /// </summary>
        protected short m_SampleMultiple;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes an new instance of the form. Zero parameter constructor.
        /// </summary>
        public FormConfigureFaultLogParameters()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes an new instance of the form. Defines the communication interface and then downloads the current default data stream parameters and displays these 
        /// on the form.
        /// </summary>
        /// <param name="communicationInterface">Reference to the communication interface that is to be used to communicate with the VCU.</param>
        /// <param name="worksetCollection">The workset collection that is to be managed.</param>
        /// <param name="log">The selected event log.</param>
        public FormConfigureFaultLogParameters(ICommunicationParent communicationInterface, WorksetCollection worksetCollection, Log log)
            : base(worksetCollection)
        {
            InitializeComponent();

            // Only one column is required for this workset so delete the tab pages associated with columns 2 and 3.
            m_TabControlColumn.TabPages.Remove(m_TabPageColumn2);
            m_TabControlColumn.TabPages.Remove(m_TabPageColumn3);

            // Move the position of the Cancel buttons.
            m_ButtonCancel.Location = m_ButtonApply.Location;

            // Check the mode of the PTU.
            if (communicationInterface == null)
            {
                CommunicationInterface = null;
            }
            else if (communicationInterface is CommunicationParent)
            {
                // The PTU is in online mode.
                CommunicationInterface = new CommunicationEvent(communicationInterface);
            }
            else
            {
                // The PTU is in simulation mode (originally referred to as offline mode).
                CommunicationInterface = new CommunicationEventOffline(communicationInterface);
            }

            // Don't allow the user to edit the workset until the security level of the workset has been established.
            ModifyEnabled = false;
            m_NumericUpDownSampleMultiple.Enabled = ModifyEnabled;

            #region - [ToolTipText] -
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonUpload].ToolTipText = Resources.FunctionKeyToolTipDataStreamUpload;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonSave].ToolTipText = Resources.FunctionKeyToolTipDataStreamSave;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonEdit].ToolTipText = Resources.FunctionKeyToolTipDataStreamConfigure;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonNew].ToolTipText = Resources.FunctionKeyToolTipDataStreamCreate;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonCopy].ToolTipText = Resources.FunctionKeyToolTipDataStreamCopy;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonRename].ToolTipText = Resources.FunctionKeyToolTipDataStreamRename;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonDelete].ToolTipText = Resources.FunctionKeyToolTipDataStreamDelete;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonSetAsDefault].ToolTipText = Resources.FunctionKeyToolTipDataStreamSetAsDefault;
            m_ToolStrip.Items[CommonConstants.KeyToolStripButtonOverrideSecurity].ToolTipText = Resources.FunctionKeyToolTipDataStreamOverrideSecurity;
            #endregion - [ToolTipText] -

            #region - [Get the Current Data Stream from the VCU] -
            // This only applies if the PTU is online.
            Workset_t workset;
            if (CommunicationInterface != null)
            {
                short variableCount, pointCount, sampleMultiple;
                short[] watchIdentifiers, dataTypes;
                try
                {
                    CommunicationInterface.GetDefaultStreamInformation(out variableCount, out pointCount, out sampleMultiple, out watchIdentifiers, out dataTypes);
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.MBTGetDefaultStreamParametersFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convert the parameters to a workset.
                workset = CommunicationInterface.ConvertToWorkset("GetDefaultStreamInformation()", watchIdentifiers, sampleMultiple);

                // Search the list of worksets for a match.
                m_WorksetFromVCU = workset;
                m_WorksetToCompare = workset;
                Workset_t matchedWorkset = Workset.FaultLog.Worksets.Find(CompareWorkset);

                // Check whether a match was found.
                if (matchedWorkset.WatchItems == null)
                {
                    // No - Create a new workset.
                    workset.Name = DefaultNewWorksetName;

                    // Set the flag to indicate that the workset does not exist and that any changes will be saved as a new workset.
                    m_CreateMode = true;
                    m_ComboBoxWorkset.DropDownStyle = ComboBoxStyle.DropDown;

                    // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
                    m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                    m_TextBoxName.Text = workset.Name;
                    m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

                    m_TextBoxName.Enabled = true;
                }
                else
                {
                    // Yes - Update the name and security level of the workset.
                    workset = matchedWorkset;

                    // Set the flag to indicate that the form already exists and any changes will result in the existing workset being modified.
                    m_CreateMode = false;
                    m_ComboBoxWorkset.DropDownStyle = ComboBoxStyle.DropDownList;

                    // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
                    m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                    m_TextBoxName.Text = workset.Name;
                    m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

                    m_TextBoxName.Enabled = false;
                }
            }
            else
            {
                // Load the default workset.
                workset = worksetCollection.Worksets[worksetCollection.DefaultIndex];

                // Set the flag to indicate that the form already exists and any changes will result in the existing workset being modified.
                m_CreateMode = false;
                m_ComboBoxWorkset.DropDownStyle = ComboBoxStyle.DropDownList;

                // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
                m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                m_TextBoxName.Text = workset.Name;
                m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

                m_TextBoxName.Enabled = false;


            }
            #endregion - [Get the Current Data Stream from the VCU] -

            // Keep a record of the selected workset. This must be set up before the call to SetEnabledToolStripButtons().
            m_SelectedWorkset = workset;

            SetEnabledToolStripButtons(true);

            // Update the 'Default' Image that identifies whether the selected workset is the default workset or not.
            m_PictureBoxDefault.Visible = (m_SelectedWorkset.Name.Equals(m_WorksetCollection.DefaultName)) ? true : false;

            // Display the name of the workset on the ComboBox control.
            // Ensure that the SelectionChanged event is not triggered as a result of specifying the Text property of the ComboBox control.
            m_ComboBoxWorkset.SelectedIndexChanged -= new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);
            m_ComboBoxWorkset.Text = workset.Name;
            m_ComboBoxWorkset.SelectedIndexChanged += new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);

            LoadWorkset(m_SelectedWorkset);

            // If an event log was specified, update the EntryCountMax property to reflect the actual number of watch variables associated with the current event log.
            if (log != null)
            {
                m_EntryCountMax = log.DataStreamTypeParameters.WatchVariablesMax;
            }
            UpdateCount();

            // Check whether the 'Row Header' ListBox can be used to display the channel numbers. This is only possible if
            // the project doesn't support multiple data stream types and the number of parameters supported by the data
            // stream can be displayed on the TabPage without the need for scroll bars i.e. <= WatchSizeFaultLogMax.
            if ((Parameter.SupportsMultipleDataStreamTypes == false) && (Parameter.WatchSizeFaultLog <= WatchSizeFaultLogMax))
            {
                AddRowHeader();
            }
            else
            {
                NoRowHeader();
            }

            // Disable all options other than save if the downloaded workset doesn't exist.
            if (m_CreateMode == true)
            {
                SetEnabledToolStripButtons(false);
            }

            // Allow the user to save the workset if it doesn't already exist.
            m_TSBSave.Enabled = (m_CreateMode == true) ? true : false;
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
                this.m_NumericUpDownSampleMultiple.ValueChanged += new System.EventHandler(this.m_NumericUpDownSampleMultiple_ValueChanged);

                // Set the Windows Form Designer Variables to null.
                m_LegendSampleMultiple = null;
                m_NumericUpDownSampleMultiple = null;
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
        #region - [Form] -
        /// <summary>
        /// Event handler for the form <c>Shown</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected override void FormPTUDialog_Shown(object sender, EventArgs e)
        {
            base.FormPTUDialog_Shown(sender, e);

            // If this Form is called from the 'Configure/Data Stream' menu option then the 'Visible' property of the 'Upload' ToolStripButton should be cleared.
            FormViewEventLog calledFrom = this.CalledFrom as FormViewEventLog;
            if (calledFrom == null)
            {
                // This Form was not called from the FormViewWatch Form i.e. it was called from the 'Configure/Watch Window' menu option.
                m_TSBUpload.Visible = false;
            }
        }
        #endregion - [Form] -

        /// <summary>
        /// Event handler for the 'Download' <c>ToolStripButton</c> <c>Click</c> event. Download the selected chart recorder workset to the VCU.
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

            // Check whether the number of watch variables defined in the workset exceeds the number that are supported by the current event log.
            if (workset.Count > EntryCountMax)
            {
                MessageBox.Show(Resources.MBTFaultLogWorksetWatchVariablesMaxExceeded + CommonConstants.NewPara + Resources.MBTFaultLogWorksetEditRequest,
                                Resources.MBCaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // ---------------------------------------------------------------------
                // Check whether the user has modified the fault log parameter configuration.
                // ---------------------------------------------------------------------
                if (CompareWorkset(workset, m_WorksetFromVCU) == false)
                {
                    // Yes - Ask for confirmation and then download the chart recorder configuration to the VCU.
                    DialogResult dialogResult = MessageBox.Show(Resources.MBTQueryModifyFaultLogParameters, Resources.MBCaptionInformation, MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Information);
                    Update();
                    if (dialogResult == DialogResult.Yes)
                    {
                        bool updateSuccess = UploadWorkset(workset);
                        if (updateSuccess == false)
                        {
                            MessageBox.Show(Resources.MBTModifyFaultLogParametersFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            m_TSBUpload.Checked = false;
                            Cursor = Cursors.Default;
                            return;
                        }

                        // Update the record of the chart recorder channel configuration that was retrieved from the VCU.
                        m_WorksetFromVCU = workset;

                        MessageBox.Show(Resources.MBTModifyFaultLogParametersSuccess, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_TSBUpload.Enabled = false;
                    }
                }
            }

            m_TSBUpload.Checked = false;
            Cursor = Cursors.Default;
            return;
        }

        /// <summary>
        /// Event handler for the <c>ValueChanged</c> event associated with the <c>NumericUpDown</c> control. Update the member variable that records the sample multiple 
        /// and raise a 'DataUpdate' event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_NumericUpDownSampleMultiple_ValueChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_SampleMultiple = (short)m_NumericUpDownSampleMultiple.Value;

            // Only generate an update event if the ModifyEnabled property is asserted.
            if (ModifyEnabled == true)
            {
                EnableApplyAndOKButtons();
                OnDataUpdate(this, new EventArgs());
            }
        }
        #endregion --- Delegated Methods ---

        #region --- Methods ---
        #region - [FormWorksetDefine] -
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

            // Total number of watch variables in the workset.
            m_LabelCountTotal.Text = Resources.LegendCount + CommonConstants.Colon + countColumn1.ToString() +
                                     CommonConstants.Space + Resources.LegendOf + CommonConstants.Space + EntryCountMax.ToString();
        }
        #endregion - [FormWorksetDefine] -

        #region - [FormConfigure] -
        /// <summary>
        /// Upload the specified fault log workset to the VCU.
        /// </summary>
        /// <param name="workset">The workset that is to be downloaded to the VCU.</param>
        /// <returns>A flag that indicates whether the workset was successfully downloaded to the VCU. True, if the VCU update was successful; otherwise, false.</returns>
        protected override bool UploadWorkset(Workset_t workset)
        {
            // A flag that indicates whether the chart recorder update was successful.
            bool updateSuccess;

            try
            {
                CommunicationInterface.SetDefaultStreamInformation(workset.SampleMultiple, workset.Column[0].OldIdentifierList);
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
                UpdateFunctionKey(m_TSBEdit, string.Empty, Resources.FunctionKeyToolTipDataStreamConfigure, null);
                UpdateFunctionKey(m_TSBNew, string.Empty, Resources.FunctionKeyToolTipDataStreamCreate, null);
                UpdateFunctionKey(m_TSBCopy, string.Empty, Resources.FunctionKeyToolTipDataStreamCopy, null);
                UpdateFunctionKey(m_TSBRename, string.Empty, Resources.FunctionKeyToolTipDataStreamRename, null);
                UpdateFunctionKey(m_TSBDelete, string.Empty, Resources.FunctionKeyToolTipDataStreamDelete, null);
                UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, Resources.FunctionKeyToolTipDataStreamSetAsDefault, null);
                UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, Resources.FunctionKeyToolTipDataStreamOverrideSecurity, null);

                m_NumericUpDownSampleMultiple.Enabled = ModifyEnabled;
                m_TSBUpload.Enabled = ((ConfigurationModified() == true) && (CommunicationInterface != null)) ? true : false;
            }
            else
            {
                switch (m_ModifyState)
                {
                    case ModifyState.Configure:

                        break;
                    case ModifyState.Edit:
                        UpdateFunctionKey(m_TSBEdit, string.Empty, Resources.FunctionKeyToolTipDataStreamConfigureUndo, null);
                        break;
                    case ModifyState.Create:
                        UpdateFunctionKey(m_TSBNew, string.Empty, Resources.FunctionKeyToolTipDataStreamCreateUndo, null);
                        break;
                    case ModifyState.Copy:
                        UpdateFunctionKey(m_TSBCopy, string.Empty, Resources.FunctionKeyToolTipDataStreamCopyUndo, null);
                        break;
                    case ModifyState.Rename:
                        UpdateFunctionKey(m_TSBRename, string.Empty, Resources.FunctionKeyToolTipDataStreamRenameUndo, null);
                        break;
                    case ModifyState.Delete:
                        UpdateFunctionKey(m_TSBDelete, string.Empty, Resources.FunctionKeyToolTipDataStreamDeleteUndo, null);
                        break;
                    case ModifyState.SetAsDefault:
                        UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, Resources.FunctionKeyToolTipDataStreamSetAsDefaultUndo, null);
                        break;
                    case ModifyState.OverrideSecurity:
                        UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, Resources.FunctionKeyToolTipDataStreamOverrideSecurityUndo, null);
                        break;
                    default:
                        break;
                }

                m_NumericUpDownSampleMultiple.Enabled = ModifyEnabled;

                // Disable the Upload button.
                m_TSBUpload.Enabled = false;
            }
        }

        /// <summary>
        /// Convert the current user settings to a workset.
        /// </summary>
        /// <param name="worksetName">The name of the workset.</param>
        /// <returns>The user settings converted to a workset.</returns>
        protected override Workset_t ConvertToWorkset(string worksetName)
        {
            Workset_t workset = base.ConvertToWorkset(worksetName);

            #region - [SampleMultiple] -
            workset.SampleMultiple = m_SampleMultiple;
            workset.CountMax = Parameter.WatchSizeFaultLog;
            #endregion - [SampleMultiple] -

            return workset;
        }

        /// <summary>
        /// Load the specified workset.
        /// </summary>
        /// <param name="workset">The workset that is to be processed.</param>
        protected override void LoadWorkset(Workset_t workset)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            base.LoadWorkset(workset);

            UpdateSampleMultiple(workset.SampleMultiple);
        }

        /// <summary>
        /// Update the sample multiple <c>NumericUpDown</c> control and member variable with the specified sample multiple value.
        /// </summary>
        /// <param name="sampleMultiple">The sample multiple.</param>
        protected void UpdateSampleMultiple(short sampleMultiple)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_SampleMultiple = sampleMultiple;

            // Include a try/catch block in case the sample multiple is invalid.
            try
            {
                m_NumericUpDownSampleMultiple.Value = m_SampleMultiple;
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.EMSampleMultipleInvalid, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_SampleMultiple = 1;
                m_NumericUpDownSampleMultiple.Value = m_SampleMultiple;
            }
        }
        #endregion - [FormConfigure] -
        #endregion --- Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets or sets the communication interface that is to be used to communicate with the target.
        /// </summary>
        /// <remarks>This property is set by the child class, if appropriate.</remarks>
        public ICommunicationEvent CommunicationInterface
        {
            get { return m_CommunicationInterface; }
            set { m_CommunicationInterface = value; }
        }
        #endregion --- Properties ---
    }
}
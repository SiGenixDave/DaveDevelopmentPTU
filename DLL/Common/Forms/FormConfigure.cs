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
 *  File name:  FormConfigure.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  04/15/11    1.0     K.McD           1.  First entry into TortoiseSVN.
 *  
 *  05/23/11    1.1     K.McD           1.  Corrected a number of calls to the Debug.Assert() method.
 *                                      2.  Corrected a number of XML tags.
 *                                      3.  Replaced the ToolStripComboBox control with a standard ComboBox control.
 *                                      4.  Renamed the CompareWorkset() method to CompareWorksetNoChartScale() to emphasise that the comparison does not 
 *                                          include chart recorder scaling information.
 *                                      5.  Created a new CompareWorkset() method that includes a comparison of the chart recorder scaling information.
 *                                      6.  Modified the LoadWorkset() method to accommodate the signature change to the FormWorksetDefine.WatchItemAddRange() method.
 *                                      7.  Auto-modified as a result of a name change to an inherited TabControl.
 *                                      8.  Modified the contructor to sort out the display and positioning of the OK/Cancel/Apply buttons.
 *                                      9.  Replaced the OK/Apply buttons with the 'Download' tool strip button.
 *                                      10. Added a virtual event handler for the 'Download' button.
 *                                      
 *  05/24/11    1.2     K.McD           1.  Added the DownloadWorkset() virtual method.
 *  
 *  05/26/11    1.3     K.McD           1.  Bug fix. Added the ClearListBoxColumns() virtual method to allow child classes to clear multiple ListBox controls, if 
 *                                          required.
 *                                      2.  Modified the CreateNewWorkset() method to call the ClearListBoxColumns() virtual method rather than clearing the 
 *                                          ListBox items directly.
 *                                      3.  Corrected the CompareWorkset() method.
 *                                      
 *  06/21/11    1.4     K.McD           1.  Corrected a number of comments and XML tags.
 *                                      2.  Auto-modified as a result of a name change to an inherited member variable.
 *                                      3.  Modified the 'SelectedIndexChanged' event handler for the workset selection 'ComboBox' control to display the name of 
 *                                          the selected workset in the header of the 'TabPage'. 
 *  07/11/11    1.5     K.McD           1.  Removed the command to force Focus when the selected index changes on the ComboBoxWorkset control.
 *  
 *  08/05/11    1.6     K.McD           1.  Modified to support the form which allows the user to configure the fault log parameters.
 *                                              (a) Changed the modifiers associated with the ConfigurationModified() and CompareName() methods to private.
 *                                              (b) Made the LoadWorkset() method virtual.
 *                                              (c) Removed the CompareWorksetNoChartScale() methods.
 *                                              (d) Modified the CompareWorkset() method such that the chart scaling information is now excluded from the workset
 *                                                  comparison. 
 *                                              
 *  10/02/11    1.7     K.McD           1.  Added a check as to whether a call to the Dispose() method has been called to the DataUpdate event handler.
 *  
 *	10/05/11	1.8		Sean.D			1.	Fixed a small bug in CompareWorkset() where an exception was raised when the OldIdentifier list in the given column of
 *	                                        worksetToCompare had fewer items than workset. Added a check to indicate the worksets are different if the counts differ.
 *	                                        
 *  04/02/15    1.9     K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *  
 *                                          1.  Changes to address the items outlined in the minutes of the meeting between BTPC, 
 *                                              Kawasaki Rail Car and NYTC on 12th April 2013 - MOC-0171:
 *                                                  
 *                                              1.  MOC-0171-28. A check will be included as part of the ‘Save’ procedure to ensure that an empty workset cannot be saved.
 *                                          
 *                                                  As a result of a review of the software, it is proposed that it should only be possible to save a workset if the
 *                                                  following criteria are met:
 *                                              
 *                                                      1.  The workset must contain at least one watch variable.
 *                                                      2.  The workset must have a valid name that is not currently in use.
 *                                          
 *                                      Modifications
 *                                      1.  Included a call to the ClearStatusMessage() method in the constructor.
 *                                      2.  Modified the  m_ComboBoxWorkset_SelectedIndexChanged() method so that the TabPage header is not updated with  the
 *                                          workset name when a new workset is selected.
 *                                      3.  Modified the  m_TSBEdit_Click(() event handler so that it disables the Save button until after the user modifies the
 *                                          empty workset.
 *                                      4.  Modified the  m_TSBNew, m_TSBCopy_Click() and m_TSBRename() event handlers to to check whether the workset name
 *                                          is in use and to disable the Save button until after the user modifies the workset.
 *                                      5.  Modified the  SetModifyState() method to call to the ClearStatusMessage() method immediately before the return statement.
 *                                      6.  Added the EnableApplAndOKButtons() override to set m_TSBSave rather than the Apply and OK buttons.
 *                                      7.  Removed the FormWorksetDefine_DataUpdate() method and the registration of this against the DataUpdate event.
 *                                      8.  Wherever the Text property of the 'm_TextBoxName' TextBox was changed the code was modified to ensure that this didn't
 *                                          trigger the TextChanged() event handler by de-registering the event handler prior to setting the property and then
 *                                          re-registering it again.
 *                                      9.  Replaced the m_NoDefinedWorkset flag with the m_UseTextBoxAsNameSource flag.
 *
 *  05/20/2016	1.10 	D.Smail		    References
 *	                                    1.  PTE Changes - List 5-17-2016.xlsx Item 14. Chart Recorder/DataStream Configuration Screens - Change the word "Download"
 *                                          to"Upload".
 *	                                    
 *                                      Modifications
 *	                                    1.	Auto-modified as a result of the ToolStripButton name change from m_TSBDownload to m_TSBUpload.
 *	                                    
 *  08/19/2016  1.11    K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 15, 22, 23, 23, 25, 47, 48. Add 'Delete', 'Set As Default' and 'Override Security'
 *                                          ToolStripButton controls to the Chart Recorder, Data Stream and Watch Window configuration dialogbox forms. On selecting the
 *                                          'Delete' ToolStripButton, a pop-up asking 'Are you sure you want to delete the ...?' should appear with the option to
 *                                          answer 'Yes' or 'Cancel'.
 *                                          
 *                                      Modifications
 *                                      1.  Added 'Delete', 'Set As Default' and 'Override Security' states to the ModifyState enumerator. - Ref.: 1.
 *                                      2.  Added the m_SelectedWorksetAfterDeletion,  m_OriginalDefaultWorksetName and m_SelectedWorksetOriginalSecurityLevel member
 *                                          variables. - Ref.: 1.
 *                                      3.  Modified the constructor such that 'TabPageColumn2' and 'TabPageColumn3' are no longer removed, thes are now removed
 *                                          in the child classes, if required. Also, no longer updates the 'Text' property of the 'Name' Label.
 *                                      4.  Modified the Cleanup() method to call the appropriate Undo...() method if the current state is: ModifyDelete,
 *                                          ModifySetAsDefault, or ModifyStateOverrideSecurity.- Ref.: 1.
 *                                      5.  Set all Windows Form Designer variables to null and detach all event handlers as part of the clean-up operation. Internal
 *                                          Audit of Code.
 *                                      6.  Modified the m_ComboBoxWorkset_SelectedIndexChanged() event handler so that in no longer checks whether the workset
 *                                          has been modified, this is not done in the SetModifyState(ModifyState.Configure) method of the child class.
 *                                      7.  m_TSBDownload_Click() event handler renamed to m_TSBUpload_Click().
 *                                      8.  Added the standard check for IsDisposed on all methods that return void. - Internal Audit of Code.
 *                                      9.  Modified the  m_TSBSave_Click() method to include support for the: Delete, Set As Default, and Override Seurity operations.
 *                                      10. Added the event handlers for the 'Click' event associated with the 'Delete', 'Set As Default' and 'Override Security'
 *                                          ToolStripButton controls. - Ref.: 1.
 *                                      11. Modified the ConvertToWorkset() method to set the security level of the workset to be that of the selected workset rather
 *                                          than to be the security level of the current user.
 *                                      12. Added the UndoDelete() and UndoSetAsDefault() and UndoOverrideSecurity() methods to support the undo operations associated
 *                                          with the 'Delete', 'Set As Default' and 'Override Security' ToolStripButton controls.
 *                                      13. Modified the SetModifyState() method to:
 *                                              1.  Support the 'Delete', 'Set As Default' and 'Override Security' states.
 *                                              2.  Toggle the images associated with the ToolStripButton controls between the individual function images and the
 *                                                  undo image.
 *                                              3.  Re-order the switch statement to: Edit, Create, Copy, Rename, Delete, Set As Default and Override Security.
 *                                      14. Renamed the SetEnabledEditNewCopyRename() method to SetEnabledToolStripButtons() and added support for the 'Delete'
 *                                          'Set As Default' and 'Override Security' ToolStripButton controls.
 *                                      15. Renamed the ClearCheckedEditNewCopyRename() to ClearCheckedToolStripButtons() and added the 'Delete', 'Set As Default' and
 *                                          'Override Security' buttons.
 *                                      16. Modified the ComboBoxAddWorksets() method to populate the drop-down list in a fixed order, as per the logic used in the
 *                                          Watch Window configuration selection.
 *                                      17. Added the UpdateFunctionKey() method as a convenient way to update the Text, ToolTipText and Image properties of
 *                                          individual function keys.
 *                                      18. Added the StartOperation() method to rationalize and standardise the code within the SetModifyState() method switch statement.
 *                                      19. Changed the modifier on the ConfigurationModified() method to protected.
 *                                      
 *  09/13/2016  1.12    K.McD           References
 *                                      1.  Bug Fix - The status message is not cleared when the user undoes the 'Create New' operation. 
 *  
 *                                      Modifications.
 *                                      1.  Added the call to ClearStatusMessage() in the SetModifyState(ModifyState.Configure) method.
 */
#endregion --- Revision History ---

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Common.Configuration;
using Common.Properties;

namespace Common.Forms
{
    /// <summary>
    /// Parent class for the forms that are used to configure the chart recorder and fault log data streams.
    /// </summary>
    public partial class FormConfigure : FormWorksetDefine
    {
        #region --- Enumerators ---
        /// <summary>
        /// The current modify state of the form.
        /// </summary>
        protected enum ModifyState
        {
            /// <summary>
            /// Configure.
            /// </summary>
            Configure,

            /// <summary>
            /// Edit mode.
            /// </summary>
            Edit,

            /// <summary>
            /// Create mode.
            /// </summary>
            Create,

            /// <summary>
            /// Copy mode.
            /// </summary>
            Copy,

            /// <summary>
            /// Rename mode.
            /// </summary>
            Rename,

            /// <summary>
            /// Delete mode.
            /// </summary>
            Delete,

            /// <summary>
            /// Set As Default mode.
            /// </summary>
            SetAsDefault,

            /// <summary>
            /// Override Security mode.
            /// </summary>
            OverrideSecurity,

            /// <summary>
            /// Undefined.
            /// </summary>
            Undefined
        }
        #endregion --- Enumerators ---

        #region --- Member Variables ---
        /// <summary>
        /// The workset that is to be compared with the worksets contained within the list.
        /// </summary>
        protected Workset_t m_WorksetToCompare;

        /// <summary>
        /// The workset that was initially uploaded from the VCU.
        /// </summary>
        protected Workset_t m_WorksetFromVCU;

        /// <summary>
        /// A record of the selected workset.
        /// </summary>
        protected Workset_t m_SelectedWorkset;

        /// <summary>
        /// A record of the new selected workset after the original selected workset has been deleted.
        /// </summary>
        protected Workset_t m_SelectedWorksetAfterDeletion;

        /// <summary>
        /// The current state of the form.
        /// </summary>
        protected ModifyState m_ModifyState = ModifyState.Undefined;

        /// <summary>
        /// <para>A flag that specifies whether the workset name should be derived from the TextBox or the ComboBox control when the workset is
        /// saved.</para><para>True, if the TextBox is the source of the workset name; otherwise, false, if the ComboBox is the source of the workset name.
        /// Initialized to false.</para>
        /// </summary>
        protected bool m_UseTextBoxAsNameSource = false;

        /// <summary>
        /// The name of the default workset associated with the workset collection that was passed to the contructor i.e. the name of the original default workset.
        /// This is required to undo the 'Set As Default' operation.
        /// </summary>
        protected string m_OriginalDefaultWorksetName = string.Empty;

        /// <summary>
        /// The original security level of the selected workset. This is required to undo the 'Set As Default' operation. 
        /// </summary>
        protected SecurityLevel m_SelectedWorksetOriginalSecurityLevel = SecurityLevel.Undefined;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes an new instance of the form. Zero parameter constructor.
        /// </summary>
        public FormConfigure()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes an new instance of the form.
        /// </summary>
        /// <param name="worksetCollection">The workset collection that is to be managed.</param>
        public FormConfigure(WorksetCollection worksetCollection)
            : base(worksetCollection)
        {
            InitializeComponent();

            ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);
            m_TextBoxName.Visible = false;

            // OK/Apply/Cancel Buttons.
            m_ButtonCancel.Location = m_ButtonApply.Location;
            m_ButtonApply.Visible = false;
            m_ButtonOK.Visible = false;

            ClearStatusMessage();
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
                // If the user is part way through making a modification, return to the ModifyState.Configure state to ensure that the workset collection is in a
                // known state prior to clean-up.
                if (((m_ModifyState != ModifyState.Configure) && (m_ModifyState != ModifyState.Undefined)) == true)
                {
                    // If the form is in the Delete state, ensure that the workset collection is restored.
                    switch (m_ModifyState)
                    {
                        case ModifyState.Delete:
                            UndoDelete();
                            break;
                        case ModifyState.SetAsDefault:
                            UndoSetAsDefault();
                            break;
                        case ModifyState.OverrideSecurity:
                            UndoOverrideSecurity();
                            break;
                        default:
                            break;
                    }
                }

                if (disposing)
                {
                    // Cleanup managed objects by calling their Dispose() methods.
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }

                // Whether called by consumer code or the garbage collector free all unmanaged resources and set the value of managed data members to null.

                #region --- Windows Form Designer Variables ---
                // Detach the event handler delegates.
                this.m_TSBEdit.Click -= new System.EventHandler(this.m_TSBEdit_Click);
                this.m_TSBNew.Click -= new System.EventHandler(this.m_TSBNew_Click);
                this.m_TSBCopy.Click -= new System.EventHandler(this.m_TSBCopy_Click);
                this.m_TSBRename.Click -= new System.EventHandler(this.m_TSBRename_Click);
                this.m_TSBDelete.Click -= new System.EventHandler(this.m_TSBDelete_Click);
                this.m_TSBSetAsDefault.Click -= new System.EventHandler(this.m_TSBSetAsDefault_Click);
                this.m_TSBOverrideSecurity.Click -= new System.EventHandler(this.m_TSBOverrideSecurity_Click);
                this.m_TSBUpload.Click -= new System.EventHandler(this.m_TSBUpload_Click);
                this.m_TSBSave.Click -= new System.EventHandler(this.m_TSBSave_Click);

                // Set the Windows Form Designer Variables to null.
                m_ToolTip = null;
                m_ToolStrip = null;
                m_TSBSave = null;
                m_TSBEdit = null;
                m_TSBNew = null;
                m_TSBCopy = null;
                m_TSBRename = null;
                m_TSBUpload = null;
                m_ComboBoxWorkset = null;
                m_TSBDelete = null;
                m_TSBSetAsDefault = null;
                m_TSBOverrideSecurity = null;
                m_TSSeparatorSave = null;
                m_TSSeparatorEdit = null;
                m_TSSeparatorNew = null;
                m_TSSeparatorCopy = null;
                m_TSSeparatorRename = null;
                m_TSSeparatorDownload = null;
                m_TSSeparatorDelete = null;
                m_TSSeparatorSetAsDefault = null;
                m_TSSeparatorOverrideSecurity = null;
                m_PictureBoxDefault = null;
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
        /// Event handler for <c>ComboBox</c> control <c>SelectedIndexChanged</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected void m_ComboBoxWorkset_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            m_ComboBoxWorkset.DropDownStyle = ComboBoxStyle.DropDownList;

            // Load the selected workset.
            Workset_t workset = (Workset_t)m_ComboBoxWorkset.SelectedItem;
            m_SelectedWorkset = workset;

            m_CreateMode = false;
            m_UseTextBoxAsNameSource = false;
            m_TextBoxName.Enabled = false;

            SetModifyState(ModifyState.Configure);
            Cursor = Cursors.Default;
        }

        #region - [ToolStripButtons] -
        /// <summary>
        /// Event handler for the 'Download' <c>ToolStripButton</c> <c>Click</c> event. The logic is defined in the child class.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected virtual void m_TSBUpload_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Event handler for the 'Save' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected virtual void m_TSBSave_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            m_TSBSave.Checked = true;

            Workset_t workset, matchedWorkset;
            DialogResult dialogResult;
            string userMessage = string.Empty;
            switch (m_ModifyState)
            {
                case ModifyState.Rename:
                    // Convert the current configuration to a workset format.
                    if (m_UseTextBoxAsNameSource == true)
                    {
                        workset = ConvertToWorkset(m_TextBoxName.Text);
                    }
                    else
                    {
                        workset = ConvertToWorkset(m_ComboBoxWorkset.Text);
                    }
                    m_WorksetToCompare = workset;

                    // Check whether the workset name already exists. 
                    matchedWorkset = m_WorksetCollection.Worksets.Find(CompareName);
                    if (matchedWorkset.WatchItems != null)
                    {
                        // Yes, inform the user.
                        MessageBox.Show(Resources.MBTWorksetNameExists, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_TSBSave.Checked = false;
                        Cursor = Cursors.Default;
                        return;
                    }
                    break;

                case ModifyState.Delete:
                case ModifyState.SetAsDefault:
                case ModifyState.OverrideSecurity:
                    switch (m_ModifyState)
                    {
                        case ModifyState.Delete:
                            dialogResult = MessageBox.Show(string.Format(Resources.MBTQueryDeleteWorkset, m_SelectedWorkset.Name), Resources.MBCaptionQuestion,
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.No)
                            {
                                m_TSBSave.Checked = false;
                                Cursor = Cursors.Default;
                                return;
                            }

                            userMessage = string.Format(Resources.MBTWorksetDeleteSuccess, m_SelectedWorkset.Name);
                            m_SelectedWorkset = m_SelectedWorksetAfterDeletion;
                            break;

                        case ModifyState.SetAsDefault:
                            dialogResult = MessageBox.Show(string.Format(Resources.MBTQuerySetAsDefaultWorkset, m_SelectedWorkset.Name),
                                                   Resources.MBCaptionQuestion,
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.No)
                            {
                                m_TSBSave.Checked = false;
                                Cursor = Cursors.Default;
                                return;
                            }

                            userMessage = string.Format(Resources.MBTWorksetSetAsDefaultSuccess, m_SelectedWorkset.Name);
                            break;

                        case ModifyState.OverrideSecurity:
                            dialogResult = MessageBox.Show(string.Format(Resources.MBTQueryOverrideSecurity, m_SelectedWorkset.Name),
                                                   Resources.MBCaptionQuestion,
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.No)
                            {
                                m_TSBSave.Checked = false;
                                Cursor = Cursors.Default;
                                return;
                            }

                            userMessage = string.Format(Resources.MBTWorksetOverrideSecuritySuccess, m_SelectedWorkset.Name);
                            break;
                    }

                    SetModifyState(ModifyState.Configure);
                    Save();

                    m_TSBSave.Checked = false;

                    // Inform the user of a successful update.
                    MessageBox.Show(userMessage, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;

                default:
                    // Convert the current configuration to a workset format.
                    if (m_UseTextBoxAsNameSource == true)
                    {
                        workset = ConvertToWorkset(m_TextBoxName.Text);
                    }
                    else
                    {
                        workset = ConvertToWorkset(m_ComboBoxWorkset.Text);
                    }
                    m_WorksetToCompare = workset;

                    // Check whether the current workset parameters, excluding name and security level, match those of an existing workset.
                    matchedWorkset = m_WorksetCollection.Worksets.Find(CompareWorkset);
                    if (matchedWorkset.WatchItems != null)
                    {
                        // Yes - check whether the names are identical.
                        if (workset.Name == matchedWorkset.Name)
                        {
                            MessageBox.Show(string.Format(Resources.MBTWorksetExists, workset.Name), Resources.MBCaptionInformation, MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(string.Format(Resources.MBTWorksetIdentical, matchedWorkset.Name), Resources.MBCaptionInformation, MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }

                        m_TSBSave.Checked = false;
                        Cursor = Cursors.Default;
                        return;
                    }
                    break;
            }

            // Check whether the current workset is an existing workset that was modified or a new workset.
            if (m_CreateMode == true)
            {
                // The workset is a new workset, check whether the name already exists.
                matchedWorkset = m_WorksetCollection.Worksets.Find(CompareName);
                if (matchedWorkset.WatchItems != null)
                {
                    // Yes, inform the user.
                    MessageBox.Show(Resources.MBTWorksetNameExists, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    m_TSBSave.Checked = false;
                    Cursor = Cursors.Default;
                    return;
                }
                else
                {
                    // No, add the new workset to the current workset collection.
                    m_WorksetCollection.Add(workset);

                    // Update the ComboBox control.
                    ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);

                    // Updating the Text property of the ComboBox control will trigger the SelectedIndexChanged event and will call the event handler.
                    // This will update the m_selectedWorkset member variable and call SetModifyState(ModifyState.Configure).  
                    m_ComboBoxWorkset.Text = workset.Name;
                    userMessage = string.Format(Resources.MBTWorksetCreationSuccess, workset.Name);
                }
            }
            else
            {
                if (Security.SecurityLevelCurrent < ((Workset_t)m_ComboBoxWorkset.SelectedItem).SecurityLevel)
                {
                    MessageBox.Show(Resources.MBTInsufficientPrivilegesModify, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    m_TSBSave.Checked = false;
                    Cursor = Cursors.Default;
                    return;
                }
                else
                {
                    // Ask the user for confirmation.
                    string worksetName = string.Empty;
                    switch (m_ModifyState)
                    {
                        case ModifyState.Rename:
                            worksetName = m_ComboBoxWorkset.Text;
                            dialogResult = MessageBox.Show(string.Format(Resources.MBTQueryRenameWorkset, worksetName), Resources.MBCaptionQuestion,
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            userMessage = string.Format(Resources.MBTWorksetRenameSuccess, worksetName, workset.Name);
                            break;
                        case ModifyState.Edit:
                            worksetName = workset.Name;
                            dialogResult = MessageBox.Show(string.Format(Resources.MBTConfirmWorksetModify, worksetName), Resources.MBCaptionQuestion,
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            userMessage = string.Format(Resources.MBTWorksetUpdateSuccess, workset.Name);
                            break;
                        default:
                            m_TSBSave.Checked = false;
                            Cursor = Cursors.Default;
                            return;
                    }

                    if (dialogResult == DialogResult.No)
                    {
                        m_TSBSave.Checked = false;
                        Cursor = Cursors.Default;
                        return;
                    }

                    Update();

                    // Update the workset collection.
                    m_WorksetCollection.Edit(worksetName, workset);

                    // Update the ComboBox control.
                    ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);

                    // Updating the Text property of the ComboBox control will trigger the SelectedIndexChanged event and will call the event handler.
                    // This will update the m_selectedWorkset member variable and call SetModifyState(ModifyState.Configure).  
                    m_ComboBoxWorkset.Text = workset.Name;
                }
            }

            Save();

            m_TSBSave.Checked = false;

            // Inform the user of a successful update.
            MessageBox.Show(userMessage, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'Edit' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected virtual void m_TSBEdit_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between the Edit state and the Configure state.
            if (m_ModifyState == ModifyState.Edit)
            {
                SetModifyState(ModifyState.Configure);
            }
            else
            {
                // Check whether the current workset is the baseline workset.
                if (m_SelectedWorkset.Name == Resources.NameBaselineWorkset)
                {
                    MessageBox.Show(Resources.MBTUnauthorizedEditBaselineWorkset, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }

                // Check whether the current workset is the default workset.
                if (m_SelectedWorkset.Name == m_WorksetCollection.DefaultName)
                {
                    DialogResult dialogResult = MessageBox.Show(Resources.MBTQueryEditDefaultWorkset, Resources.MBCaptionConfirm, MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question);
                    Update();
                    if (dialogResult == DialogResult.No)
                    {
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                SetModifyState(ModifyState.Edit);

                // No need to check whether the workset name is in use when entering Edit mode.
                // EnableApplyAndOKButtons();

                // Disable the Save button until the user has modified the workset. 
                m_TSBSave.Enabled = false;
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'New' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected void m_TSBNew_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between the Create state and the Configure state.
            if (m_ModifyState == ModifyState.Create)
            {
                SetModifyState(ModifyState.Configure);
            }
            else
            {
                SetModifyState(ModifyState.Create);

                // On this occasion, only used to check whether the default workset name is already in use.
                EnableApplyAndOKButtons();

                // Disable the Save button until the user has added at least one watch variable to the workset. 
                m_TSBSave.Enabled = false;
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'Copy' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected virtual void m_TSBCopy_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between Copy state and the Configure state.
            if (m_ModifyState == ModifyState.Copy)
            {
                SetModifyState(ModifyState.Configure);
            }
            else
            {
                SetModifyState(ModifyState.Copy);

                // On this occasion, only used to check whether the workset name '{original name} - Copy' is already in use.
                EnableApplyAndOKButtons();

                // Disable the Save button until the user has modified the workset. 
                m_TSBSave.Enabled = false;
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'Rename' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected void m_TSBRename_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between the Rename state and the Configure state.
            if (m_ModifyState == ModifyState.Rename)
            {
                SetModifyState(ModifyState.Configure);
            }
            else
            {
                if (m_SelectedWorkset.Name == Resources.NameBaselineWorkset)
                {
                    MessageBox.Show(Resources.MBTUnauthorizedRenameBaselineWorkset, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }

                SetModifyState(ModifyState.Rename);

                // No need to check whether the workset name is in use when entering Rename mode.
                // EnableApplyAndOKButtons();

                // Disable the Save button until the user has renamed the workset. 
                m_TSBSave.Enabled = false;
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'Delete' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected void m_TSBDelete_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between the Delete state and the Configure state.
            if (m_ModifyState == ModifyState.Delete)
            {
                UndoDelete();

                SetModifyState(ModifyState.Configure);
            }
            else
            {
                // Check whether the current workset is the baseline workset.
                if (m_SelectedWorkset.Name == Resources.NameBaselineWorkset)
                {
                    MessageBox.Show(Resources.MBTUnauthorizedDeleteBaselineWorkset, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }

                // Check whether the current workset is the default workset.
                if (m_SelectedWorkset.Name == m_WorksetCollection.DefaultName)
                {
                    DialogResult dialogResult = MessageBox.Show(Resources.MBTQueryDeleteDefaultWorkset, Resources.MBCaptionConfirm, MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.No)
                    {
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                SetModifyState(ModifyState.Delete);

                // Allow the user to save the workset collection changes.
                m_TSBSave.Enabled = true;
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'Set As Default' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected void m_TSBSetAsDefault_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between the SetAsDefault state and the Configure state.
            if (m_ModifyState == ModifyState.SetAsDefault)
            {
                UndoSetAsDefault();

                SetModifyState(ModifyState.Configure);
            }
            else
            {
                // Check whether the selected workset is the default workset.
                if (m_SelectedWorkset.Name == m_WorksetCollection.DefaultName)
                {
                    MessageBox.Show(Resources.MBTSelectedWorksetIsAlreadyDefault, Resources.MBCaptionInformation, MessageBoxButtons.OK,
                                                                MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }

                SetModifyState(ModifyState.SetAsDefault);

                // Allow the user to save the workset collection changes.
                m_TSBSave.Enabled = true;
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the 'Override Security' <c>ToolStripButton</c> <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        protected void m_TSBOverrideSecurity_Click(object sender, EventArgs e)
        {
             // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            // Toggle between the Override Security state and the configure state.
            if (m_ModifyState == ModifyState.OverrideSecurity)
            {
                UndoOverrideSecurity();

                SetModifyState(ModifyState.Configure);
            }
            else
            {
                // Check whether the current workset is the default workset.
                if (m_SelectedWorkset.Name == m_WorksetCollection.DefaultName)
                {
                    MessageBox.Show(Resources.MBTCannotOverrideDefaultWorkset, Resources.MBCaptionInformation, MessageBoxButtons.OK,
                                                                MessageBoxIcon.Information);
                    Cursor = Cursors.Default;
                    return;
                }

                SetModifyState(ModifyState.OverrideSecurity);

                // Allow the user to save the workset collection changes.
                m_TSBSave.Enabled = true;
            }

            Cursor = Cursors.Default;
        }
        #endregion - [ToolStripButtons] -
        #endregion --- Delegated Methods ---

        #region --- Methods ---
        /// <summary>
        /// Upload the specified workset to the VCU. The logic is performed in the child class.
        /// </summary>
        /// <param name="workset">The workset that is to be downloaded to the VCU.</param>
        /// <returns>A flag that indicates whether the workset was successfully downloaded to the VCU. True, if the VCU update was successful; otherwise, false.</returns>
        protected virtual bool UploadWorkset(Workset_t workset)
        {
            // A flag that indicates whether the VCU update was successful.
            bool updateSuccess = false;

            return updateSuccess;
        }

        /// <summary>
        /// Set the modify state to the specified state.
        /// </summary>
        /// <remarks>The Enabled property of the menu option that allows the user to modify the Y axis limits of the individual chart recorder channels is linked 
        /// directly to the ModifyEnabled property of the parent class.</remarks>
        /// <param name="modifyState">The required modify state.</param>
        protected virtual void SetModifyState(ModifyState modifyState)
        {
            m_ModifyState = modifyState;

            if (m_ModifyState == ModifyState.Configure)
            {
                ClearCheckedToolStripButtons();

                ModifyEnabled = false;
                SetEnabledToolStripButtons(true);
                ClearStatusMessage();

                // Update the 'Default' Image that identifies whether the selected workset is the default workset or not.
                m_PictureBoxDefault.Visible = (m_SelectedWorkset.Name.Equals(m_WorksetCollection.DefaultName)) ? true : false;

                // Restore the Image properties of the ToolStripButtons. The ToolTipText properties are restored in the child classes.
                UpdateFunctionKey(m_TSBEdit, string.Empty, string.Empty, Resources.Modify);
                UpdateFunctionKey(m_TSBNew, string.Empty, string.Empty, Resources.CreateNew);
                UpdateFunctionKey(m_TSBCopy, string.Empty, string.Empty, Resources.Copy);
                UpdateFunctionKey(m_TSBRename, string.Empty, string.Empty, Resources.Rename);
                UpdateFunctionKey(m_TSBDelete, string.Empty, string.Empty, Resources.Remove_From_List);
                UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, string.Empty, Resources.Favourite);
                UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, string.Empty, Resources.Security);

                // Load the previously selected workset.
                LoadWorkset(m_SelectedWorkset);

                // Display the name of the previously selected workset.
                // Ensure that the TextChanged event handler is not called as a result of specifying the Text property of the TextBox control.
                m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                m_TextBoxName.Text = m_SelectedWorkset.Name;
                m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

                // Ensure that the workset name is displayed on the ComboBox control and that the SelectedIndexChanged event handler is not called. 
                m_ComboBoxWorkset.SelectedIndexChanged -= new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);
                m_ComboBoxWorkset.Text = m_TextBoxName.Text;
                m_ComboBoxWorkset.SelectedIndexChanged += new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);

                // Don't allow the user to change the name of the workset.
                m_TextBoxName.Visible = false;
                m_TextBoxName.Enabled = false;
                m_ComboBoxWorkset.Visible = true;

                // Use the ComboBox as the source of the workset name if it is saved.
                m_UseTextBoxAsNameSource = false;

                // Show the security level of the selected workset.
                m_TextBoxSecurityLevel.Text = Security.GetSecurityDescription(m_SelectedWorkset.SecurityLevel);
            }
            else
            {
                // Disable the ToolStrip buttons.
                SetEnabledToolStripButtons(false);

                // Use the TextBox as the source of the workset name.
                m_UseTextBoxAsNameSource = true;

                // Show the TextBox.
                m_TextBoxName.Visible = true;
                m_ComboBoxWorkset.Visible = false;

                // Keep a record of the original security level of the selected workset in case the operation needs to be undone and to ensure that the security level
                // of an edited, copied, renamed workset is not updated to the current security level.
                m_SelectedWorksetOriginalSecurityLevel = m_SelectedWorkset.SecurityLevel;

                switch (modifyState)
                {
                    case ModifyState.Edit:
                        StartOperation(m_TSBEdit, false, true, false);
                        break;
                    case ModifyState.Create:
                        StartOperation(m_TSBNew, true, true, true);

                        #region - [Create Operation Specific Code] -
                        // Display the default name for a new workset.
                        // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
                        m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                        m_TextBoxName.Text = Resources.NameNewWorksetDefault;
                        m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

                        // Show the security level of the current user as this will be the assigned security level of the new workset.
                        m_TextBoxSecurityLevel.Text = Security.Description;

                        // Hide the 'Is Default' image as this new workset cannot yet be defined as the default workset.
                        m_PictureBoxDefault.Visible = false;

                        CreateNewWorkset();
                        #endregion - [Create Operation Specific Code] -
                        break;
                    case ModifyState.Copy:
                        StartOperation(m_TSBCopy, true, true, true);

                        #region - [Copy Operation Specific Code] -
                        // Display the default name of the copied workset.
                        // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
                        m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                        m_TextBoxName.Text = string.Format(Resources.NameWorksetCopy, m_SelectedWorkset.Name);
                        m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

                        // Show the security level of the current user as this will be the assigned security level of the copied workset.
                        m_TextBoxSecurityLevel.Text = Security.Description;

                        // Hide the 'Is Default' image as this new workset cannot yet be defined as the default workset.
                        m_PictureBoxDefault.Visible = false;
                        #endregion - [Copy Operation Specific Code] -
                        break;
                    case ModifyState.Rename:
                        StartOperation(m_TSBRename, false, false, true);

                        #region - [Rename Operation Specific Code] -
                        #endregion - [Rename Operation Specific Code] -
                        break;
                    case ModifyState.Delete:
                        StartOperation(m_TSBDelete, false, false, false);

                        #region - [Delete Operation Specific Code] -
                        #region - [Determine which workset to display next.] -
                        // Get the index of the selected workset.
                        m_WorksetToCompare = m_SelectedWorkset;
                        int index = m_WorksetCollection.Worksets.FindIndex(CompareName);
                        Debug.Assert(index > 0, "FormConfigure.SetModifyState() - [index > 0]");

                        // Get the number of worksets in the collection.
                        int count = m_WorksetCollection.Worksets.Count;
                        Debug.Assert(count > 1, "FormConfigure.SetModifyState() - [count > 1]");

                        // Display the previous workset in the list, if possible; otherwise display the next workset. As a last resort display the
                        // baseline workset.
                        if (count > 2)
                        {
                            if (index == 1)
                            {
                                m_SelectedWorksetAfterDeletion = m_WorksetCollection.Worksets[index + 1];
                            }
                            else
                            {
                                m_SelectedWorksetAfterDeletion = m_WorksetCollection.Worksets[index - 1];
                            }
                        }
                        else if (count == 2)
                        {
                            if (index == 1)
                            {
                                m_SelectedWorksetAfterDeletion = m_WorksetCollection.Worksets[0];
                            }
                        }
                        #endregion - [Determine which workset to display next.] -

                        #region - [Remove the selected workset, update the ComboBox and display the next workset] -
                        m_WorksetCollection.Remove(m_SelectedWorkset.Name);

                        // Update the ComboBox control.
                        ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);

                        // Load the next workset. 
                        LoadWorkset(m_SelectedWorksetAfterDeletion);

                        // Display the name of the next workset in the list.
                        // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
                        m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
                        m_TextBoxName.Text = m_SelectedWorksetAfterDeletion.Name;
                        m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);
                        #endregion - [Remove the selected workset, update the ComboBox and display the next workset] -

                        // Update the 'Default' Image that identifies whether the selected workset is the default workset or not.
                        m_PictureBoxDefault.Visible = (m_SelectedWorksetAfterDeletion.Name.Equals(m_WorksetCollection.DefaultName)) ? true : false;
                        #endregion - [Delete Operation Specific Code] -
                        break;
                    case ModifyState.SetAsDefault:
                        StartOperation(m_TSBSetAsDefault, false, false, false);

                        #region - [Set As Default Operation Specific Code] -
                        // Keep a record of the original default workset name in case the operation needs to be undone.
                        m_OriginalDefaultWorksetName = m_WorksetCollection.DefaultName;

                        // Ensure that the security level of the new default workset is set to the appropriate level.
                        if (m_SelectedWorkset.SecurityLevel < Security.SecurityLevelHighest)
                        {
                            m_SelectedWorkset.SecurityLevel = Security.SecurityLevelHighest;
                            m_WorksetCollection.Edit(m_SelectedWorkset.Name, m_SelectedWorkset);
                        }

                        // Update the default workset of the workset collection to be the selected workset.
                        m_WorksetCollection.SetDefaultWorkset(m_SelectedWorkset.Name);

                        // Update the ComboBox control.
                        ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);

                        // Update the Security TextBox to show the SecurityLevel that is appropriate to the default workset.
                        m_TextBoxSecurityLevel.Text = Security.GetSecurityDescription(Security.SecurityLevelHighest);

                        // Update the 'Default' Image to show the selected workset as the default workset.
                        m_PictureBoxDefault.Visible = true;
                        #endregion - [Set As Default Operation Specific Code] -
                        break;
                    case ModifyState.OverrideSecurity:
                        StartOperation(m_TSBOverrideSecurity, false, false, false);

                        #region - [Override Security Operation Specific Code] -
                        // Show the dialogbox that allows the user to select the new security level.
                        FormSetSecurityLevel formSetSecurityLevel = new FormSetSecurityLevel(m_SelectedWorkset.Name, m_SelectedWorkset.SecurityLevel);
                        formSetSecurityLevel.CalledFrom = this;
                        DialogResult dialogResult = formSetSecurityLevel.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                        {
                            // Modify the security level of the workset to the selected security level.
                            m_SelectedWorkset.SecurityLevel = formSetSecurityLevel.SecurityLevel;

                            // Replace the existing workset with the workset with the modified security level.
                            m_WorksetCollection.Edit(m_SelectedWorkset.Name, m_SelectedWorkset);

                            // Update the ComboBox control.
                            ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);

                            // Update the Security TextBox to show the SecurityLevel that is appropriate to the default workset.
                            m_TextBoxSecurityLevel.Text = Security.GetSecurityDescription(m_SelectedWorkset.SecurityLevel);
                        }
                        #endregion - [Override Security Operation Specific Code] -
                        break;
                    default:
                        break;
                }

                // The Enabled property of the menu option that allows the user to modify the Y axis limits of the individual chart recorder channels is linked 
                // directly to the Enabled property of the ModifyEnabled property of the parent class.
                m_MenuItemChangeChartScaleFactor.Enabled = ModifyEnabled;

                ClearStatusMessage();
            }
        }

        /// <summary>
        /// <para>1.  Sets the specified <c>ToolStripButton</c> <c>Image</c> property to the <c>Resources.Edit_Undo</c> image.</para>
        /// <para>2.  Asserts the specified <c>ToolStripButton</c> <c>Checked</c> and <c>Enabled</c> properties.</para>
        /// <para>3.  Sets the <c>m_CreateMode</c> member flag to the state defined by the <c>createMode</c> parameter.</para>
        /// <para>4.  Sets the <c>ModifyEnabled</c> property to the state defined by the <c>allowModify</c> parameter.</para>
        /// <para>5.  Sets the <c>Enabled</c> property of the 'Workset' <c>TextBox</c> to the state defined by the <c>allowNameEdit</c> parameter.</para>
        /// </summary>
        /// <param name="toolStripButton">The <c>ToolStripButton</c> control corresponding to the current operation.</param>
        /// <param name="createMode">The required state of the <c>m_CreateMode</c> member variable.</param>
        /// <param name="allowModify">The required state of the <c>ModifyEnabled</c> property.</param>
        /// <param name="allowNameEdit">The required state of the <c>Enabled</c> property associated with the 'Workset' <c>TextBox</c>.</param>
        private void StartOperation(ToolStripButton toolStripButton, bool createMode, bool allowModify, bool allowNameEdit)
        {
            UpdateFunctionKey(toolStripButton, string.Empty, string.Empty, Resources.Edit_Undo);
            toolStripButton.Checked = true;
            toolStripButton.Enabled = true;
            m_CreateMode = createMode;
            ModifyEnabled = allowModify;
            m_TextBoxName.Enabled = allowNameEdit;
        }

        /// <summary>
        /// Convert the current user settings to a workset.
        /// </summary>
        /// <param name="worksetName">The name of the workset.</param>
        /// <returns>The user settings converted to a workset.</returns>
        protected virtual Workset_t ConvertToWorkset(string worksetName)
        {
            Workset_t workset = new Workset_t();

            workset.Name = worksetName;
            workset.SampleMultiple = Workset_t.DefaultSampleMultiple;
            workset.CountMax = m_WorksetCollection.EntryCountMax;

            // The security level of the workset should only be set to the current securily level if a new workset is created, for all other operations the security
            // level of workset should remain unchanged.
            workset.SecurityLevel = (m_ModifyState == ModifyState.Create) ? Security.SecurityLevelCurrent : m_SelectedWorksetOriginalSecurityLevel;

            #region - [Column] -
            workset.Column = new Column_t[1];
            workset.Column[0].HeaderText = m_TextBoxHeader1.Text;
            workset.Column[0].OldIdentifierList = new List<short>();

            #region - [OldIdentifierList] -
            for (int index = 0; index < m_ListBox1.Items.Count; index++)
            {
                workset.Column[0].OldIdentifierList.Add(((WatchItem_t)m_ListBox1.Items[index]).OldIdentifier);
            }
            #endregion - [OldIdentifierList] -
            #endregion - [Column] -

            #region - [WatchItems] -
            workset.WatchItems = new WatchItem_t[m_WatchItems.Length];
            Array.Copy(m_WatchItems, workset.WatchItems, m_WatchItems.Length);
            #endregion - [WatchItems] -

            #region - [WatchElementList] -
            workset.WatchElementList = new List<short>();
            short oldIdentifier;
            WatchVariable watchVariable;

            for (int rowIndex = 0; rowIndex < workset.Column[0].OldIdentifierList.Count; rowIndex++)
            {
                oldIdentifier = workset.Column[0].OldIdentifierList[rowIndex];
                try
                {
                    watchVariable = Lookup.WatchVariableTableByOldIdentifier.Items[oldIdentifier];
                    if (watchVariable == null)
                    {
                        workset.WatchElementList.Add(CommonConstants.WatchIdentifierNotDefined);
                    }
                    else
                    {
                        workset.WatchElementList.Add(watchVariable.Identifier);
                    }
                }
                catch (Exception)
                {
                    workset.WatchElementList.Add(CommonConstants.WatchIdentifierNotDefined);
                }
            }
            workset.WatchElementList.Sort();
            #endregion - [WatchElementList] -

            #region - [Count] -
            workset.Count = workset.WatchElementList.Count;

            if (workset.Count != m_ListItemCount)
            {
                throw new ArgumentException(Resources.EMWorksetIntegrityCheckFailed, "FormWorksetDefineFaultLog.ConvertToWorkset() - [workset.WatchElements.Count]");
            }
            #endregion - [Count] -

            return workset;
        }

        /// <summary>
        /// Clear the items in the 'Column' ListBox controls.
        /// </summary>
        protected virtual void ClearListBoxColumnItems()
        {
            m_ListBox1.Items.Clear();
        }

        /// <summary>
        /// Predicate function called by the <c>List.Find()</c> method to return a workset that matches the specified workset, ignoring the Name, SecurityLevel, 
        /// HeaderText and TabPagePlots fields of each workset. 
        /// </summary>
        /// <param name="workset">The list item that is to be processed.</param>
        /// <returns>True, if the specified item meets the logic requirements given in the function; otherwise false.</returns>
        protected virtual bool CompareWorkset(Workset_t workset)
        {
            Workset_t worksetToCompare = m_WorksetToCompare;

            // ----------------------------------------------------------------------
            // Ignore the name and security level as these are not stored on the VCU.
            // ----------------------------------------------------------------------
            if ((workset.SampleMultiple != worksetToCompare.SampleMultiple) ||
                (workset.CountMax != worksetToCompare.CountMax))
            {
                return false;
            }

            #region - [Column] -
            if (workset.Column.Length != worksetToCompare.Column.Length)
            {
                return false;
            }

            #region - [OldIdentifierList] -
            for (int columnIndex = 0; columnIndex < workset.Column.Length; columnIndex++)
            {
                // --------------------------------------------------------
                // Ignore the header text as this is not stored on the VCU.
                // --------------------------------------------------------

				// Check to be sure both lists have the same number of Old Identifiers.
				// At one point, this could lead to an exception if the user tried to create a Chart Recorder with no items.
				if (workset.Column[columnIndex].OldIdentifierList.Count != worksetToCompare.Column[columnIndex].OldIdentifierList.Count)
				{
					return false;
				}

                for (int index = 0; index < workset.Column[columnIndex].OldIdentifierList.Count; index++)
                {
                    if (workset.Column[columnIndex].OldIdentifierList[index] != worksetToCompare.Column[columnIndex].OldIdentifierList[index])
                    {
                        return false;
                    }
                }
            }
            #endregion - [OldIdentifierList] -
            #endregion - [Column] -

            #region - [WatchItems] -
            if (workset.WatchItems.Length != worksetToCompare.WatchItems.Length)
            {
                return false;
            }

            for (int index = 0; index < workset.WatchItems.Length; index++)
            {
                if (workset.WatchItems[index].Added != worksetToCompare.WatchItems[index].Added)
                {
                    return false;
                }
            }
            #endregion - [WatchItems] -

            #region - [WatchElementList] -
            if (workset.WatchElementList.Count != worksetToCompare.WatchElementList.Count)
            {
                return false;
            }

            for (int index = 0; index < workset.WatchElementList.Count; index++)
            {
                if (workset.WatchElementList[index] != worksetToCompare.WatchElementList[index])
                {
                    return false;
                }
            }
            #endregion - [WatchElementList] -

            #region - [Count] -
            if (workset.Count != worksetToCompare.Count)
            {
                return false;
            }
            #endregion - [Count] -

            return true;
        }

        /// <summary>
        /// Load the specified workset.
        /// </summary>
        /// <param name="workset">The workset that is to be processed.</param>
        protected virtual void LoadWorkset(Workset_t workset)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
            m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
            m_TextBoxName.Text = workset.Name;
            m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

            m_TextBoxHeader1.Text = workset.Column[0].HeaderText;

            m_ListItemCount = workset.Count;

            m_WatchItems = new WatchItem_t[workset.WatchItems.Length];
            Array.Copy(workset.WatchItems, m_WatchItems, workset.WatchItems.Length);

            UpdateListBoxAvailable(m_WatchItems);

            // ------------------------------------
            // Update the 'Column' ListBox control.
            // -------------------------------------
            WatchItemAddRange(m_ListBox1, workset.Column[0]);
            UpdateCount();

            m_TextBoxSecurityLevel.Text = Security.GetSecurityDescription(workset.SecurityLevel);
        }

        /// <summary>
        /// Enable the 'Save' button and clear the status message provided at least one watch variable has been selected and the workset name has been
        /// defined; otherwise, disable the 'Save' button and write a status message.
        /// </summary>
        protected override void EnableApplyAndOKButtons()
        {
            // Check whether the watch variable count is within the permitted limits and that the 'm_ModifyState' variable is not undefined. This ensures
            // that the following code is only processed after the form has been instantiated.
            if ((m_ListItemCount <= EntryCountMax) && m_ModifyState != ModifyState.Undefined)
            {
                // Only enable the OK and Apply buttons if the workset has at least one watch variable and a valid workset name has been defined.    
                if (m_ListItemCount > 0)
                {
                    // Yes - The workset contains at least one watch variable.

                    // Check whether the workset name has been defined.
                    if (m_TextBoxName.Text != string.Empty)
                    {
                        // Don't enable the 'Save' Button if in Configuration mode.
                        if (m_ModifyState != ModifyState.Configure)
                        {
                            // Enable the Save button and clear the status message.
                            m_TSBSave.Enabled = true;

                            ClearStatusMessage();
                        }
                    }
                    else
                    {
                        // No - The workset contains at least one watch variable but the workset name has not been defined. Disable the Apply and OK buttons.
                        m_TSBSave.Enabled = false;
                        WriteStatusMessage(Resources.SMWorksetNameNotDefined);
                    }
                }
                else
                {
                    // No - The workset doesn't contain any watch variables.

                    // check whether the workset name has been defined.
                    string message = string.Empty;
                    if (m_TextBoxName.Text != string.Empty)
                    {
                        message = Resources.SMWorksetWatchCountTooSmall;
                    }
                    else
                    {
                        // The workset must be given a name and contain at least one watch variable. Unfortunately, this status message is too long
                        // to be displayed within the control so it has been changed to Resources.SMWorksetNameNotDefined.
                        message = Resources.SMWorksetNameNotDefined;
                    }

                    m_TSBSave.Enabled = false;
                    WriteStatusMessage(message);
                }

                if (m_WorksetCollection != null)
                {
                    // Check whether the current workset name is already in use provide the user is not in configuration mode.
                    if ((m_ModifyState != ModifyState.Edit) &&
                        (m_ModifyState != ModifyState.Configure) &&
                        (m_WorksetCollection.Contains(m_TextBoxName.Text.Trim()) == true))
                    {
                        m_TSBSave.Enabled = false;
                        WriteStatusMessage(string.Format(Resources.SMWorksetNameExists, m_TextBoxName.Text));
                    }
                }
            }
        }

        /// <summary>
        /// Set the Enabled property of the: Edit, New, Copy, Rename, Delete, Set As Default, and Override Security buttons to the specified state and disable the
        /// 'Save' button. If the specified state is true, then, for the: Edit, Rename, Delete and Override Security the user must have sufficient privileges in order
        /// to enable the buttons. For the 'Set As Default' button, the user must be logged into, at least, the Security.SecurityLevelHighest security level and the
        /// currently selected workset must not be the default workset.
        /// </summary>
        /// <remarks>The member variables <c>m_SelectedWorkset</c> and <c>m_WorksetCollection</c>must be initialized before calling this method.</remarks>
        /// <param name="enabled">The required state of the Enabled property.</param>
        protected void SetEnabledToolStripButtons(bool enabled)
        {
            m_TSBSave.Enabled = false;

            m_TSBNew.Enabled = enabled;
            m_TSBCopy.Enabled = enabled;

            if ((enabled == true) && (Security.SecurityLevelCurrent >= m_SelectedWorkset.SecurityLevel))
            {
                m_TSBEdit.Enabled = true;
                m_TSBRename.Enabled = true;
                m_TSBDelete.Enabled = true;

                // For the 'Set As Default' ToolStripButton control, the user must be logged into, at least, the Security.SecurityLevelHighest security level and
                // the currently selected workset must not be the default workset.
                if (Security.SecurityLevelCurrent >= Security.SecurityLevelHighest)
                {
                    m_TSBSetAsDefault.Enabled = (m_SelectedWorkset.Name != m_WorksetCollection.DefaultName) ? true : false;
                }

                // For the 'Override Security' ToolStripButton control, the user cannot override the security level of the workset if the workset is defined as the
                // default workset.
                m_TSBOverrideSecurity.Enabled = (m_SelectedWorkset.Name != m_WorksetCollection.DefaultName) ? true : false; ;
            }
            else
            {
                m_TSBEdit.Enabled = false;
                m_TSBRename.Enabled = false;
                m_TSBDelete.Enabled = false;
                m_TSBSetAsDefault.Enabled = false;
                m_TSBOverrideSecurity.Enabled = false;
            }

            return;
        }

        /// <summary>
        /// Clear the Checked property of the: Edit, New, Copy, Rename, Delete, Set As Default, and Override Security buttons.
        /// </summary>
        private void ClearCheckedToolStripButtons()
        {
            m_TSBEdit.Checked = false;
            m_TSBNew.Checked = false;
            m_TSBCopy.Checked = false;
            m_TSBRename.Checked = false;
            m_TSBDelete.Checked = false;
            m_TSBSetAsDefault.Checked = false;
            m_TSBOverrideSecurity.Checked = false;
        }

        /// <summary>
        /// Compare the two worksets, ignoring the Name, SecurityLevel, HeaderText and TabPagePlots fields of each workset.
        /// </summary>
        /// <param name="worksetA">The first workset that is to be compared.</param>
        /// <param name="worksetB">The second workset that is to be compared.</param>
        /// <returns>True, if the worksets are identical; otherwise, false.</returns>
        protected bool CompareWorkset(Workset_t worksetA, Workset_t worksetB)
        {
            m_WorksetToCompare = worksetB;
            return CompareWorkset(worksetA);
        }

        /// <summary>
        /// Compare the current chart recorder channel settings with the workset that was downloaded from the VCU and return a flag that indicates whether the current 
        /// settings are different from the downloaded workset.
        /// </summary>
        /// <returns></returns>
        protected bool ConfigurationModified()
        {
            bool configurationModified = true;

            // Convert the current user settings to a workset.
            Workset_t workset = ConvertToWorkset(string.Empty);

            // Check whether the workset associated with the current settings matches the workset that was initially downloaded from the VCU.
            if (CompareWorkset(workset, m_WorksetFromVCU) == true)
            {
                configurationModified = false;
            }

            return configurationModified;
        }

        /// <summary>
        /// Predicate function called by the <c>List.Find()</c> method to return a workset that matches the Name variable of the m_WorksetToCompare workset.
        /// </summary>
        /// <param name="workset">The workset list item that is to be processed.</param>
        /// <returns>True if the specified item meets the logic requirements given in the function; otherwise false.</returns>
        private bool CompareName(Workset_t workset)
        {
            bool match = false;
            Workset_t worksetToCompare = m_WorksetToCompare;

            if (worksetToCompare.Name == workset.Name)
            {
                match = true;
            }

            return match;
        }

        /// <summary>
        /// Create a new, empty chart recorder/fault log workset.
        /// </summary>
        private void CreateNewWorkset()
        {
            // WatchItem - populate the array defining which watch variables have been added to the workset.
            m_WatchItems = new WatchItem_t[Lookup.WatchVariableTableByOldIdentifier.RecordList.Count];
            WatchItem_t watchItem;
            WatchVariable watchVariable;
            for (short oldIdentifier = 0; oldIdentifier < Lookup.WatchVariableTableByOldIdentifier.RecordList.Count; oldIdentifier++)
            {
                watchItem = new WatchItem_t();
                watchItem.OldIdentifier = oldIdentifier;
                watchItem.Added = false;

                try
                {
                    watchVariable = Lookup.WatchVariableTableByOldIdentifier.Items[oldIdentifier];

                    if (watchVariable == null)
                    {
                        watchItem.Exists = false;
                    }
                    else
                    {
                        watchItem.Exists = true;
                    }
                }
                catch (Exception)
                {
                    watchItem.Exists = false;
                }

                m_WatchItems[oldIdentifier] = watchItem;
            }

            UpdateListBoxAvailable(m_WatchItems);
            ClearListBoxColumnItems();
            m_ListItemCount = 0;
            UpdateCount();
        }

        /// <summary>
        /// Add the worksets contained within the specified workset collection to the <c>Items</c> property of the specified <c>ComboBox</c> control.
        /// </summary>
        /// <param name="comboBox">The <c>ComboBox</c> control that it to be processed.</param>
        /// <param name="worksetCollection">The workset collection containing the worksets that are to be added.</param>
        private void ComboBoxAddWorksets(ComboBox comboBox, WorksetCollection worksetCollection)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Debug.Assert(worksetCollection != null, "FormConfigure.ComboBoxAddWorksets() - [worksetCollection != null]");
            Debug.Assert(comboBox != null, "FormConfigure.ComboBoxAddWorksets() - [comboBox != null]");

            comboBox.Items.Clear();

            // Only include the Baseline workset if it is defined as the default workset.
            if (worksetCollection.DefaultIndex == 0)
            {
                comboBox.Items.Add(worksetCollection.Worksets[0]);
            }

            for (int worksetIndex = 1; worksetIndex < worksetCollection.Worksets.Count; worksetIndex++)
            {
                // Only add those worksets that can be displayed i.e. exclude any worksets where the number of watch variables exceeds the current watch
                // size.
                if (worksetCollection.Worksets[worksetIndex].WatchElementList.Count <= Parameter.WatchSize)
                {
                    comboBox.Items.Add(worksetCollection.Worksets[worksetIndex]);
                }
            }
        }

        /// <summary>
        /// Update the Text, ToolTipText and Image of the specified function key.
        /// </summary>
        /// <param name="functionKey">The function key to be displayed.</param>
        /// <param name="text">The text that is to appear on the function key.</param>
        /// <param name="toolTipText">The tool-tip text associated with the function key.</param>
        /// <param name="image">The image that is to appear on the function key.</param>
        protected void UpdateFunctionKey(ToolStripButton functionKey, string text, string toolTipText, System.Drawing.Bitmap image)
        {
            if (text != string.Empty)
            {
                functionKey.Text = text;
            }

            if (toolTipText != string.Empty)
            {
                functionKey.ToolTipText = toolTipText;
            }

            if (image != null)
            {
                functionKey.Image = image;
            }
        }

        /// <summary>
        /// Undo the Delete operation by:
        /// <para>1.  deserialising the WorksetCollection that was last saved to disk,</para>
        /// <para>2.  updating the ComboBox control with the list of worksets contained within the deserialised collection,</para>
        /// <para>3.  loading the workset that had been selected for deletion, and</para>
        /// <para>4.  updating the name on the ComboBox control to the workset that had been selected for deletion.</para>
        /// </summary>
        private void UndoDelete()
        {
            Debug.Assert(MainWindow != null, "FormConfigure.UndoDelete() - [MainWindow != null]");
            try
            {
                MainWindow.LoadWorksetCollection(m_WorksetCollection, Parameter.ProjectInformation.ProjectIdentifier);
            }
            catch (Exception exception)
            {
                MessageBox.Show(Resources.MBTWorksetCollectionLoadFailed + CommonConstants.NewLine + CommonConstants.NewLine + exception.Message,
                                Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Update the ComboBox control with the list of worksets contained within the deserialised collection.
            ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);

            // Load the workset that had been selected for deletion.
            LoadWorkset(m_SelectedWorkset);

            // Display the name of the workset that had been selected for deletion on the ComboBox control.
            // Ensure that the SelectionChanged event is not triggered as a result of specifying the Text property of the ComboBox control.
            m_ComboBoxWorkset.SelectedIndexChanged -= new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);
            m_ComboBoxWorkset.Text = m_SelectedWorkset.Name;
            m_ComboBoxWorkset.SelectedIndexChanged += new EventHandler(m_ComboBoxWorkset_SelectedIndexChanged);
        }

        /// <summary>
        /// Undo the 'Set As Default' operation by:
        /// <para>1.  Setting the initial default workset as the default workset.</para>
        /// <para>2.  Restoring the security level of the updated default workset to its initial value.</para>
        /// </summary>
        private void UndoSetAsDefault()
        {
            // Update the default workset of the workset collection to be the initial default workset.
            m_WorksetCollection.SetDefaultWorkset(m_OriginalDefaultWorksetName);

            // Restore the security level of the workset that was selected to be the new default workset to its original security level.
            m_SelectedWorkset.SecurityLevel = m_SelectedWorksetOriginalSecurityLevel;
            m_WorksetCollection.Edit(m_SelectedWorkset.Name, m_SelectedWorkset);

            // Update the ComboBox control with the list of worksets contained within the deserialised collection.
            ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);
        }

        /// <summary>
        /// Undo the 'OverrideSecurity' operation by:
        /// <para></para>
        /// </summary>
        private void UndoOverrideSecurity()
        {
            // Restore the security level of the selected workset to its original security level.
            m_SelectedWorkset.SecurityLevel = m_SelectedWorksetOriginalSecurityLevel;
            m_WorksetCollection.Edit(m_SelectedWorkset.Name, m_SelectedWorkset);

            // Update the ComboBox control with the list of worksets contained within the deserialised collection.
            ComboBoxAddWorksets(m_ComboBoxWorkset, m_WorksetCollection);
        }
        #endregion --- Methods ---
    }
}
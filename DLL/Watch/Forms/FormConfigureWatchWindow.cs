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
 *  File name:  FormConfigureWatchWindow.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  09/06/2016  1.0     K.McD           1.  First entry into TortoiseSVN.
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
    /// Form to allow the user to configure the various worksets associated with the Watch Window screen.
    /// </summary>
    public partial class FormConfigureWatchWindow : FormConfigure, ICommunicationInterface<ICommunicationWatch>
    {
        #region --- Member Variables ---
        /// <summary>
        /// Reference to the selected communication interface.
        /// </summary>
        private ICommunicationWatch m_CommunicationInterface;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes an new instance of the form. Zero parameter constructor.
        /// </summary>
        public FormConfigureWatchWindow()
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
        public FormConfigureWatchWindow(ICommunicationParent communicationInterface, IMainWindow mainWindow, WorksetCollection worksetCollection)
            : base(worksetCollection)
        {
            InitializeComponent();

            Debug.Assert(mainWindow != null, "FormConfigureWatchWindow.Ctor() - [mainWindow != null]");
            m_MainWindow = mainWindow;

            // Move the position of the Cancel buttons.
            m_ButtonCancel.Location = m_ButtonApply.Location;

            NoRowHeader();

            // Check the mode of the PTU.
            if (communicationInterface == null)
            {
                CommunicationInterface = null;
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

            // Don't allow the user to edit the workset until the security level of the workset has been established.
            ModifyEnabled = false;

            m_CreateMode = false;

            // Set the structure containing the workset that was downloaded from the VCU to be an empty workset.
            m_WorksetFromVCU = new Workset_t();
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
                

                // Set the Windows Form Designer Variables to null.
                
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

            // If this Form is called from the 'Configure/Watch Window' menu option then the 'Visible' property of the 'Upload' ToolStripButton should be cleared.
            Workset_t workset;
            Form calledFrom = this.CalledFrom as FormViewWatch;
            if (calledFrom == null)
            {
                // This Form was not called from the FormViewWatch Form i.e. it was called from the 'Configure/Watch Window' menu option.
                m_TSBUpload.Visible = false;

                // Get the default watch window workset.
                workset = m_WorksetCollection.Worksets[m_WorksetCollection.DefaultIndex];
            }
            else
            {
                // This form was called from the 'View/Watch Window' screen, load the 'Active' workset rather than the default workset.
                m_TSBUpload.Visible = false;

                // Get the Active watch window workset.
                workset = m_WorksetCollection.Worksets[m_WorksetCollection.ActiveIndex];
            }

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
        }
        #endregion - [Form] -
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

            // Watch variable count associated with each column of the workset.
            int countColumn1 = m_ListBox1.Items.Count;
            int countColumn2 = m_ListBox2.Items.Count;
            int countColumn3 = m_ListBox3.Items.Count;

            m_LabelCount1.Text = countColumn1.ToString() + CommonConstants.Space + Resources.LegendItems;
            m_LabelCount2.Text = countColumn2.ToString() + CommonConstants.Space + Resources.LegendItems;
            m_LabelCount3.Text = countColumn3.ToString() + CommonConstants.Space + Resources.LegendItems;

            // Total number of watch variables in the workset.
            m_LabelCountTotal.Text = Resources.LegendCount + CommonConstants.Colon + m_ListItemCount.ToString() +
                                     CommonConstants.Space + Resources.LegendOf + CommonConstants.Space + m_WorksetCollection.EntryCountMax.ToString();
        }
        #endregion - [FormWorksetDefine] -

        #region - [FormConfigure] -
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
                UpdateFunctionKey(m_TSBEdit, string.Empty, Resources.FunctionKeyToolTipWatchWindowConfigure, null);
                UpdateFunctionKey(m_TSBNew, string.Empty, Resources.FunctionKeyToolTipWatchWindowCreate, null);
                UpdateFunctionKey(m_TSBCopy, string.Empty, Resources.FunctionKeyToolTipWatchWindowCopy, null);
                UpdateFunctionKey(m_TSBRename, string.Empty, Resources.FunctionKeyToolTipWatchWindowRename, null);
                UpdateFunctionKey(m_TSBDelete, string.Empty, Resources.FunctionKeyToolTipWatchWindowDelete, null);
                UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, Resources.FunctionKeyToolTipWatchWindowSetAsDefault, null);
                UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, Resources.FunctionKeyToolTipWatchWindowOverrideSecurity, null);
            }
            else
            {
                switch (m_ModifyState)
                {
                    case ModifyState.Configure:
                        
                        break;
                    case ModifyState.Edit:
                        UpdateFunctionKey(m_TSBEdit, string.Empty, Resources.FunctionKeyToolTipWatchWindowConfigureUndo, null);
                        break;
                    case ModifyState.Create:
                        UpdateFunctionKey(m_TSBNew, string.Empty, Resources.FunctionKeyToolTipWatchWindowCreateUndo, null);
                        break;
                    case ModifyState.Copy:
                        UpdateFunctionKey(m_TSBCopy, string.Empty, Resources.FunctionKeyToolTipWatchWindowCopyUndo, null);
                        break;
                    case ModifyState.Rename:
                        UpdateFunctionKey(m_TSBRename, string.Empty, Resources.FunctionKeyToolTipWatchWindowRenameUndo, null);
                        break;
                    case ModifyState.Delete:
                        UpdateFunctionKey(m_TSBDelete, string.Empty, Resources.FunctionKeyToolTipWatchWindowDeleteUndo, null);
                        break;
                    case ModifyState.SetAsDefault:
                        UpdateFunctionKey(m_TSBSetAsDefault, string.Empty, Resources.FunctionKeyToolTipWatchWindowSetAsDefaultUndo, null);
                        break;
                    case ModifyState.OverrideSecurity:
                        UpdateFunctionKey(m_TSBOverrideSecurity, string.Empty, Resources.FunctionKeyToolTipWatchWindowOverrideSecurityUndo, null);
                        break;
                    default:
                        break;
                }
            }
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
            m_ListBox2.Items.Clear();
            m_ListBox3.Items.Clear();
        }
            

        /// <summary>
        /// Convert the current user setting to a workset.
        /// </summary>
        /// <param name="worksetName">The name of the workset.</param>
        /// <returns>The user settings converted to a workset.</returns>
        protected override Workset_t ConvertToWorkset(string worksetName)
        {
            // --------------------------------------------------------------------------
            // Copy the definitions to a new workset and update the WorksetManager class.
            // --------------------------------------------------------------------------
            Workset_t workset = new Workset_t();

            workset.Name = worksetName;
            workset.SampleMultiple = Workset_t.DefaultSampleMultiple;
            workset.CountMax = m_WorksetCollection.EntryCountMax;

            // The security level of the workset should only be set to the current securily level if a new workset is created, for all other operations the security
            // level of workset should remain unchanged.
            workset.SecurityLevel = (m_ModifyState == ModifyState.Create) ? Security.SecurityLevelCurrent : m_SelectedWorksetOriginalSecurityLevel;

            #region - [Column] -
            workset.Column = new Column_t[m_WorksetCollection.ColumnCountMax];
            workset.Column[0].HeaderText = m_TextBoxHeader1.Text;
            workset.Column[1].HeaderText = m_TextBoxHeader2.Text;
            workset.Column[2].HeaderText = m_TextBoxHeader3.Text;

            #region - [OldIdentifierList] -
            for (int index = 0; index < m_WorksetCollection.ColumnCountMax; index++)
            {
                workset.Column[index].OldIdentifierList = new List<short>();
            }

            for (int index = 0; index < m_ListBox1.Items.Count; index++)
            {
                workset.Column[0].OldIdentifierList.Add(((WatchItem_t)m_ListBox1.Items[index]).OldIdentifier);
            }

            for (int index = 0; index < m_ListBox2.Items.Count; index++)
            {
                workset.Column[1].OldIdentifierList.Add(((WatchItem_t)m_ListBox2.Items[index]).OldIdentifier);
            }

            for (int index = 0; index < m_ListBox3.Items.Count; index++)
            {
                workset.Column[2].OldIdentifierList.Add(((WatchItem_t)m_ListBox3.Items[index]).OldIdentifier);
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
            for (int columnIndex = 0; columnIndex < workset.Column.Length; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < workset.Column[columnIndex].OldIdentifierList.Count; rowIndex++)
                {
                    oldIdentifier = workset.Column[columnIndex].OldIdentifierList[rowIndex];
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
        /// Predicate function called by the <c>List.Find()</c> method to return a workset that matches the specified workset, ignoring the Name and SecurityLevel fields
        /// of each workset. 
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
                    if (workset.Column[columnIndex].HeaderText != worksetToCompare.Column[columnIndex].HeaderText)
                    {
                        return false;
                    }
                }
                #endregion - [ChartScaleList] -
            }

            return true;
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

            // Ensure that the TextChanged event is not triggered as a result of specifying the Text property of the TextBox control.
            m_TextBoxName.TextChanged -= new EventHandler(m_TextBoxName_TextChanged);
            m_TextBoxName.Text = workset.Name;
            m_TextBoxName.TextChanged += new EventHandler(m_TextBoxName_TextChanged);

            m_TextBoxHeader1.Text = workset.Column[0].HeaderText;
            m_TextBoxHeader2.Text = workset.Column[1].HeaderText;
            m_TextBoxHeader3.Text = workset.Column[2].HeaderText;

            m_ListItemCount = workset.Count;

            m_WatchItems = new WatchItem_t[workset.WatchItems.Length];
            Array.Copy(workset.WatchItems, m_WatchItems, workset.WatchItems.Length);

            UpdateListBoxAvailable(m_WatchItems);

            // ------------------------------------
            // Update the 'Column' ListBox control.
            // -------------------------------------
            WatchItemAddRange(m_ListBox1, workset.Column[0]);
            WatchItemAddRange(m_ListBox2, workset.Column[1]);
            WatchItemAddRange(m_ListBox3, workset.Column[2]);
            UpdateCount();

            m_TextBoxSecurityLevel.Text = Security.GetSecurityDescription(workset.SecurityLevel);
        }
        #endregion - [FormConfigure] -
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
        #endregion --- Properties ---
    }
}
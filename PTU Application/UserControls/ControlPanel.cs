#region --- Revision History ---
/*
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly
 *  prohibited. Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    Common
 * 
 *  File name:  ControlPanel.cs
 * 
 *  Revision History
 *  ----------------
 */

#region - [1.0 - 1.4] -
/*
 *  Date        Version Author      Comments
 *  06/19/15    1.0     K.McD       1.  First entry into TortoiseSVN.
 */

/*
 *  07/27/15    1.1     K.McD       References
 *                                  1.  An informal review of version 6.11 of the PTU concluded that, where possible - i.e. if the PTU is started from a shortcut
 *                                      that passes the project identifier as a shortcut parameter, the project specific PTU initialization should be carried out
 *                                      in the MDI Form contructor that has the parameter string array as its signature rather than by the LoadDictionary() method.
 *                                      This streamlines the display construction of the Control Panel associated with the R188 project. In the 6.11 implementation
 *                                      the CTA layout is momentarily displayed before the Control Panel is drawn, however by initializing the project specific
 *                                      features in the constructor the Control Panel associated with the R188 project is drawn immediately and the CTA layout
 *                                      is not shown at all.
 *                                      
 *                                  2.  Bug Fix - SNCR - R188 PTU [20-Mar-2015] Item 22. The toggling of the ToolTipText for the Online and Offline buttons on the
 *                                      R188 project doesn’t work correctly.
 *                                      
 *                                  3.  Bug Fix - SNCR - R188 PTU [20-Mar-2015] Item 25. The Font of the ‘NYCT R188’, ‘PTU STATUS’, ‘MAINTENANCE’ and
 *                                      ‘ADMINISTRATION’  labels is not automatically updated when the user changes the Font via the ‘Tools/Options’ menu. 
 *  
 *                                  Modifications
 *                                  1.  As a result of the changes associated with reference 1, it was necessary to modify the InitializeControlPanel() method to
 *                                      always hide the WibuBox StatusLabel and show the local WibuBox Label and Legend.
 *                                      
 *                                      The original design of the Control Panel was generic and supported projects that required a WibuBox as well as those that
 *                                      did not. By default, the local WibuBox Label and Legend were hidden and code in the InitializeControlPanel() method would
 *                                      make these visible and hide the WibuBox StatusLabel if the WibuBox StatusLabel had previously been made visible, indicating
 *                                      that the project required a WibuBox.
 *                                      
 *                                      As not all projects use a WibuBox, the WibuBox StatusLabel is initially hidden then, just before the Control Panel is
 *                                      instantiated, a check is made to see whether the project supports a WibuBox and, if so, the WibuBox StatusLabel is made
 *                                      visible. Unfortunately, the duration between the section of code running in the MDI constructor that makes the WibuBox StatusLabel
 *                                      visible and the code in the InitializeControlPanel() method of the Control Panel that checks whether the WibuBox StatusLabel
 *                                      is visible is such that the WibuBox StatusLabel is always seen as being hidden by the code in the
 *                                      InitializeControlPanel() as the setting of the WibuBox StatusLabel Visible property does not have time to ripple through
 *                                      the system before it is checked. - Ref.: 1.
 *                                      
 *                                  2.  Standardized the 'TextChanged' event handlers so that a check is now made as to whether the StatusLabel text includes legend
 *                                      information as standard e.g. 'Log: Saved', as opposed to simply 'Saved'. If the text does not include legend information, the
 *                                      StatusLabel text is copied directly to the corresponding local Control Panel Label; otherwise, the legend information is
 *                                      firstly removed from the StatusLabel text. - REf.: N/A.
 *                                      
 *                                  3.  Corrected the ToolTip assignment in the Offline_CheckedChanged() event handler. - Ref.: 2.
 *                                  
 *                                  4.  Now includes the Mode and SecurityLevel StatusLabels in the list of 'PTU STATUS' labels. These are no longer displayed in
 *                                      the PanelStatus control of the MDI Form. - Ref.: N/A.
 *                                      
 *                                  5.  Created a new ChangeFont() event handler and attached the MainWindow FontChanged event to this handler. - Ref.: 3.
 */

/*
 *  08/11/15    1.2     K.McD       References
 *                                  1.  Changes resulting from documents: 'PTU MOC Findings - .docx' and 'PTU Installation on 64-bit Machine_v1-08022015.docx' sent
 *                                      from Atul Chaudhari on 4th Aug 2015 and the follow up email sent on 5th Aug 2015.
 *                                      
 *                                      1.  MOC-0171-005. KRC have requested that the 'Log' button is to be disabled in simulation mode to avoid confusion if the
 *                                          simulated event data were ever saved to disk.
 *                                          
 *                                  Modifications
 *                                  1.  Modified the DiagnosticsEventLog_EnabledChanged() method to disable the 'Log' button if the
 *                                      PTE is in simulation mode.
 */

/*
 *  09/30/15    1.3    K.McD       References
 *                                  1.  Bug Fix - SNCR - R188 [20-Mar215] Item 32. The R188 project requires that the PTE can be used on a 1024 x 768 laptop.
 *                                      When release 6.14 is displayed at a resolution of 1024 x 768: (a) the ‘Help’ and ‘Exit’ buttons at the bottom of the control panel
 *                                      are partly cut off (b) the ‘View/Event Log’, ‘View/Test Results’ and ‘Data Stream Replay’ screens are only fully visible
 *                                      (without resorting to the horizontal scroll bar) if the R188 control panel is removed; (c) the ‘Open/Saved Event Log’ screen
 *                                      is not fully visible because of the additional ‘real-estate’ occupied by the ‘[Log]’ column of the DataGridView
 *                                      control; and (d) the watch worksets can only use 2 of the 3 available columns if all watch variables in the workset are to be
 *                                      visible without resorting to the horizontal scroll bar.
 *                                      
 *                                  Modifications
 *                                  1.  Modified the MdiChildActivate() method to also hide the 'R188 Control Panel' in the case where forms: FormViewEventLog;
 *                                      FormViewTestResults; and FormDataStreamReplay are to be displayed. - Ref.: 1.
 *                                      
 *                                  2.  Modified the MenuUpdated() method such that the 'Exit' menu options is still displayed despite being replicated on
 *                                      the R188 Control panel.
 */

/*
 *  11/16/15    1.4     K.McD       References
 *                                  1.  Added support for R179.
 *
 *                                  Modifications
 *                                  1.  Updated the summary XML tag associated with the class description.
 *                                  2.  Added the ProjectTitle property and associated member variable.
 *                                  3.  Included code to update the m_LabelProjectTitle label when the ProjectTitle property is updated.
 */
#endregion - [1.0 - 1.4] -

#region - [1.5] -
/*
 * 04/19/2016   1.5     K.McD       References
 *                                  1.  Conference Call 11th April 2016 - Add Control Panel to the BART and Toronto Rocket projects.
 * 
 *                                  Modifications
 *                                  1.  Added the 'ConnectionDirect' and 'ConnectionRemote' constants and removed the 'Subsystem', 'LocationCode' and 'Connection'
 *                                      constants.
 *                                  2.  Added the: ProjectIdentifier and WibuRequired properties and associated member variables.
 *                                  3.  Renamed the ProjectTitle property to ControlPanelTitle and renamed the associated member variable. 
 *                                  4.  Added the Connection, LocationCode and SubsystemCode properties and linked these with the Text property of the
 *                                      corresponding 'PTE Status' Label control. Also introduced code to hide the label and associated legend if the value is
 *                                      set to string.Empty.
 *                                  5.  Modified the InitializeControlPanel() method to:
 *                                          1.  Update the Font.
 *                                          2.  Initialise the WibuBox controls only if the WibuBoxIsRequired property is asserted.
 *                                          3.  Hide the 'PTE Status' WibuBox legend and label if the WibuBoxRequired property is clear.
 *                                  6.  Modified the DiagnosticsEventLog_EnabledChanged() method such that the 'Event Log' button is disabled in
 *                                      Simulation mode for the R188 project.
 *                                  7.  Modified the Cleanup() method to also restore the Visible property of the Mode and SecurityLevel status labels.
 *                                  8.  Modified the Cleanup() method to only restore the Visible property of the WibuBox StatusLabel if the WibuBoxRequired property is
 *                                      asserted.
 */
#endregion - [1.5] -

#region - [1.6] -
/*
 * 05/18/2016   1.6     DAS         References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Items: 36, 37, 38. When the: 'System SW Version', 'Log Erasure' or 'Password Protection'
 *                                      buttons are used to select the associated dialog boxes, a blue outline appears around the selected button. This outline remains
 *                                      when the dialog box is closed, whereas, it should be cleared once the dialog box is closed.
 *                                      
 *                                  Modifications
 *                                  1.  Created the m_RemoveBlueHighlightFromButton() method to set the ActiveControl property to null.
 *                                  2.  Called the m_RemoveBlueHighlightFromButton() method from within the: m_ButtonViewSystemInformation_Click(), 
 *                                      m_ButtonDiagnosticsInitializeEventLogs_Click() and  m_ButtonAdministrationPasswordProtection_Click() methods.
 *                                      
 */

/*
 *  07/202016   1.6.1   K.McD       References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 12. Under the 'Help' drop-down menu, add a 'Show User Manual menu option above the
 *                                      'About'menu option with a link to the 'Software User Manual'.
 *  
 *                                  Modifications.
 *                                  1.  Renamed m_ToolStripMenuItemHelpPTUHelp to m_ToolStripMenuItemHelpShowUserManual.
 *                                  2.  Renamed m_ToolStripSeparatorHelpPTUHelp to m_ToolStripSeparatorHelpShowUserManual.
 *                                  3.  Corrected the m_ButtonAdministrationPasswordProtection_Click() event handler to check the state of the Visible property of
 *                                      the m_ToolStripMenuItemConfigurePasswordProtection menu option instead of the m_ToolStripMenuItemHelpShowUserManual menu option.
 *                                  4.  Modified the m_ButtonHelp_Click() event handler to just call the m_ToolStripMenuItemHelpShowUserManual.PerformClick() method
 *                                      rather than checking on the state of the Visible property of the m_ToolStripMenuItemHelpShowUserManual menu option.
 *                                  5.  Modified the MenuUpdated() event handler to ensure that the Visible properties of the m_ToolStripMenuItemHelpShowUserManual
 *                                      menu option and the m_ToolStripSeparatorHelpShowUserManual separator are not set to false.
 */
#endregion - [1.6] -
#endregion --- Revision History ---

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bombardier.PTU.Properties;
using Common;
using Common.Configuration;
using Common.Forms;

namespace Bombardier.PTU
{
    /// <summary>
    /// <para>
    /// This UserControl allows the user to access the key PTE maintenance and administrative operations via a set of Button controls on the
    /// 'Control Panel' and gives feedback, via the 'PTE STATUS' panel, on its current operational status.</para>
    /// <para>The UserControl monitors: TextChanged; EnabledChanged; CheckedChanged; MdiChildActive; MenuUpdated; and ChangeFont PTE events and uses
    /// the PerformClick() method associated with the ToolStripMeneuItem control to modify the standard PTE User Interface to that specified in the NYCT
    /// specification.</para>
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        #region --- Constants ---
        /// <summary>
        /// The connection text string that is used to specify that the PTU is connected to the PTU connector on the Supplier Subsystem.
        /// </summary>
        public const string ConnectionDirect = "Direct";

        /// <summary>
        /// The connection text string that is used to specify that the PTU is connected via a MDS PTU connector.
        /// </summary>
        public const string ConnectionRemote = "Remote";

        #endregion --- Constants ---

        #region --- Member Variables ---
        /// <summary>
        /// Flag to indicate whether the class has been disposed of. True, indicates that the class has already been disposed of; otherwise, false.
        /// </summary>
        protected bool m_IsDisposed;

        /// <summary>
        /// Reference to the main application window.
        /// </summary>
        private IMainWindow m_MainWindow;

        /// <summary>
        /// Reference to the 'Online' ToolStripButton.
        /// </summary>
        private ToolStripButton m_ToolStripButtonOnline;

        /// <summary>
        /// Reference to the 'Offline' ToolStripButton.
        /// </summary>
        private ToolStripButton m_ToolStripButtonOffline;

        /// <summary>
        /// Reference to the 'File' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemFile;

        /// <summary>
        /// Reference to the 'File/Select Data Dictionary' ToolStripSeparator.
        /// </summary>
        private ToolStripSeparator m_ToolStripSeparatorFileOpenDataDictionary;

        /// <summary>
        /// Reference to the 'File/Exit' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemFileExit;

        /// <summary>
        /// Reference to the 'View' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemView;

        /// <summary>
        /// Reference to the 'View/Watch Window' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemViewWatchWindow;

        /// <summary>
        /// Reference to the 'View/System Information' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemViewSystemInformation;

        /// <summary>
        /// Reference to the 'Diagnostics' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemDiagnostics;

        /// <summary>
        /// Reference to the 'Diagnostics/Event Log' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemDiagnosticsEventLog;

        /// <summary>
        /// Reference to the 'Diagnostics/Self Tests' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemDiagnosticsSelfTests;

        /// <summary>
        /// Reference to the 'Diagnostics/Initialize Event Logs' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemDiagnosticsInitializeEventLogs;

        /// <summary>
        /// Reference to the 'Configure' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemConfigure;

        /// <summary>
        /// Reference to the 'Configure/Password Protection' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemConfigurePasswordProtection;

        /// <summary>
        /// Reference to the 'Configure/Password Protection' ToolStripSeparator.
        /// </summary>
        private ToolStripSeparator m_ToolStripSeparatorConfigurePasswordProtection;
  
        /// <summary>
        /// Reference to the 'Help' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemHelp;

        /// <summary>
        /// Reference to the 'Help/Show User Manual' ToolStripMenuItem.
        /// </summary>
        private ToolStripMenuItem m_ToolStripMenuItemHelpShowUserManual;

        /// <summary>
        /// Reference to the 'Help/Show User Manual' ToolStripSeparator.
        /// </summary>
        private ToolStripSeparator m_ToolStripSeparatorHelpShowUserManual;

        /// <summary>
        /// Reference to the PTU Multiple Document Interface.
        /// </summary>
        private MdiPTU m_MdiPTU;

        /// <summary>
        /// Reference to the LogStatus ToolStripStatusLabel.
        /// </summary>
        private ToolStripStatusLabel m_ToolStripStatusLabelLogStatus;

        /// <summary>
        /// Reference to the WibuBoxStatus ToolStripStatusLabel.
        /// </summary>
        private ToolStripStatusLabel m_ToolStripStatusLabelWibuBoxStatus;

        /// <summary>
        /// Reference to the CarNumber ToolStripStatusLabel.
        /// </summary>
        private ToolStripStatusLabel m_ToolStripStatusLabelCarNumber;

        /// <summary>
        /// Reference to the Mode ToolStripStatusLabel.
        /// </summary>
        private ToolStripStatusLabel m_ToolStripStatusLabelMode;

        /// <summary>
        /// Reference to the SecurityLevel ToolStripStatusLabel.
        /// </summary>
        private ToolStripStatusLabel m_ToolStripStatusLabelSecurityLevel;

        /// <summary>
        /// A flag that that controls whether the control panel is to support the WibuBox security system. True, if support for the WibuBox security system is required;
        /// otherwise, false.
        /// </summary>
        private bool m_WibuBoxSupportRequired = false;

        /// <summary>
        /// The project identifier associated with the control panel.
        /// </summary>
        private string m_ProjectIdentifier = string.Empty;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Zero Parameter Constructor.
        /// </summary>
        public ControlPanel()
        {
            InitializeComponent();
        }
        #endregion --- Constructors ---

        #region --- Cleanup ---
        /// <summary>
        /// Clean up the resources used by the form.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Cleanup(bool disposing)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Restore the ToolStripButtons
            #region - [Online] -
            m_ToolStripButtonOnline.Text = Resources.FunctionKeyTextOnline;
            m_ToolStripButtonOnline.ToolTipText = Resources.FunctionKeyToolTipTextOnline;
            m_ToolStripButtonOnline.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            m_ToolStripButtonOnline.TextAlign = ContentAlignment.BottomCenter;
            m_ToolStripButtonOnline.Size = new Size(70,50);
            m_ToolStripButtonOnline.Margin = new Padding(0, 1, 0, 2);
            ToolStripSeparator toolStripSeparatorOnlineLHS = (ToolStripSeparator)MainWindow.ToolStripFunctionKeys.Items[CommonConstants.KeyToolStripSeparatorOnlineLHS];
            Debug.Assert(toolStripSeparatorOnlineLHS != null, "ControlPanel.InitializeControlPanel() - toolStripSeparatorOnlineLHS != null");
            toolStripSeparatorOnlineLHS.Visible = false;
            toolStripSeparatorOnlineLHS.Margin = new Padding(0, 0, 0, 0);
            #endregion - [Online] -

            #region - [Offline] -
            m_ToolStripButtonOffline.Text = Resources.FunctionKeyTextOffline;
            m_ToolStripButtonOffline.ToolTipText = Resources.FunctionKeyToolTipTextOffline;
            m_ToolStripButtonOffline.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            m_ToolStripButtonOffline.TextAlign = ContentAlignment.BottomCenter;
            m_ToolStripButtonOffline.Size = new Size(70, 50);
            m_ToolStripButtonOffline.Margin = new Padding(0, 1, 0, 2);
            ToolStripSeparator toolStripSeparatorOfflineLHS = (ToolStripSeparator)MainWindow.ToolStripFunctionKeys.Items[CommonConstants.KeyToolStripSeparatorOfflineLHS];
            Debug.Assert(toolStripSeparatorOfflineLHS != null, "ControlPanel.InitializeControlPanel() - toolStripSeparatorOfflineLHS != null");
            toolStripSeparatorOfflineLHS.Visible = false;
            toolStripSeparatorOnlineLHS.Margin = new Padding(0, 0, 0, 0);
            #endregion - [Offline] -

            MainWindow.ToolStripFunctionKeys.AutoSize = true;

            // Restore the Visible property of those StatusLabels that were modified, to their default state.
            m_ToolStripStatusLabelLogStatus.Visible = true;
            m_ToolStripStatusLabelCarNumber.Visible = true;
            m_ToolStripStatusLabelMode.Visible = true;
            m_ToolStripStatusLabelSecurityLevel.Visible = true;

            // Only restore the Visible property of the WibuBox StatusLabel if the WibuBoxRequired flag is asserted.
            if (this.WibuBoxRequired == true)
            {
                m_ToolStripStatusLabelWibuBoxStatus.Visible = true;
            }

            // Restore the Visible property of the ToolStripFunctionKeys ToolStrip.
            if (MainWindow != null)
            {
                MainWindow.ToolStripFunctionKeys.Visible = true;
            }

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
                #region - [Detach the event handler methods.] -
                m_ToolStripButtonOnline.CheckedChanged -= new EventHandler(Online_CheckedChanged);
                m_ToolStripButtonOffline.CheckedChanged -= new EventHandler(Offline_CheckedChanged);

                m_ToolStripStatusLabelLogStatus.TextChanged -= new EventHandler(LogStatus_TextChanged);
                m_ToolStripStatusLabelWibuBoxStatus.TextChanged -= new EventHandler(WibuBoxStatus_TextChanged);
                m_ToolStripStatusLabelMode.TextChanged -= new EventHandler(Mode_TextChanged);
                m_ToolStripStatusLabelSecurityLevel.TextChanged -= new EventHandler(SecurityLevel_TextChanged);

                // WibuBox
                if (this.WibuBoxRequired == true)
                {
                    // Detach the TextChanged event from the event handler.
                    m_ToolStripStatusLabelWibuBoxStatus.TextChanged -= new EventHandler(WibuBoxStatus_TextChanged);
                }

                m_ToolStripMenuItemView.EnabledChanged -= new EventHandler(ViewWatchWindow_EnabledChanged);
                m_ToolStripMenuItemView.EnabledChanged -= new EventHandler(ViewSystemInformation_EnabledChanged);

                m_ToolStripMenuItemViewWatchWindow.EnabledChanged -= new EventHandler(ViewWatchWindow_EnabledChanged);
                m_ToolStripMenuItemViewSystemInformation.EnabledChanged -= new EventHandler(ViewSystemInformation_EnabledChanged);

                m_ToolStripMenuItemDiagnostics.EnabledChanged -= new EventHandler(DiagnosticsEventLog_EnabledChanged);
                m_ToolStripMenuItemDiagnosticsEventLog.EnabledChanged -= new EventHandler(DiagnosticsEventLog_EnabledChanged);

                m_ToolStripMenuItemDiagnostics.EnabledChanged -= new EventHandler(DiagnosticsSelfTests_EnabledChanged);
                m_ToolStripMenuItemDiagnosticsSelfTests.EnabledChanged -= new EventHandler(DiagnosticsSelfTests_EnabledChanged);

                m_ToolStripMenuItemDiagnostics.EnabledChanged -= new EventHandler(DiagnosticsSelfTests_EnabledChanged);
                m_ToolStripMenuItemDiagnosticsInitializeEventLogs.EnabledChanged -= new EventHandler(DiagnosticsInitializeEventLogs_EnabledChanged);

                m_MdiPTU.MdiChildActivate -= new EventHandler(MdiChildActivate);
                m_MdiPTU.MenuUpdated -= new EventHandler(MenuUpdated);

                if (MainWindow != null)
                {
                    MainWindow.FontChanged -= new System.EventHandler(ChangeFont);
                }
                #endregion - [Detach the event handler methods.] -

                m_MdiPTU = null;
                m_MainWindow = null;
                m_ToolStripButtonOnline = null;
                m_ToolStripButtonOffline = null;
                m_ToolStripMenuItemView = null;
                m_ToolStripMenuItemViewWatchWindow = null;
                m_ToolStripMenuItemViewSystemInformation = null;
                m_ToolStripMenuItemDiagnostics = null;
                m_ToolStripMenuItemDiagnosticsEventLog = null;
                m_ToolStripMenuItemDiagnosticsSelfTests = null;
                m_ToolStripMenuItemDiagnosticsInitializeEventLogs = null;
                m_ToolStripStatusLabelLogStatus = null;
                m_ToolStripStatusLabelCarNumber = null;
                m_ToolStripStatusLabelMode = null;
                m_ToolStripStatusLabelSecurityLevel = null;

                // WibuBox.
                m_ToolStripStatusLabelWibuBoxStatus = null;

                #region - [Component Designer Variables] -
                #endregion - [Component Designer Variables] -
            }
            catch (Exception)
            {
                // Don't do anything, just ensure that an exception isn't thrown.
            }
        }
        #endregion --- Cleanup ---

        #region --- Delegated Methods ---
        #region - [Buttons] -
        #region - [Maintenance] -
        /// <summary>
        /// Event handler for the 'Maintenance/System Information' Button Click event. Simulates the user selecting the 'View/System Information' menu option. The
        /// simulation only works if the menu option is enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonViewSystemInformation_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ToolStripMenuItemViewSystemInformation.PerformClick();

            // Added to remove "blue outline" from button after returning from other form
            m_RemoveBlueHighlightFromButton();
        }

        /// <summary>
        /// Event handler for the 'Watch Window' Button Click event. Simulates the user selecting the 'View/Watch Window' menu option. The
        /// simulation only works if the menu option is enabled and visible.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonViewWatchWindow_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ToolStripMenuItemViewWatchWindow.PerformClick();
        }

        /// <summary>
        /// Event handler for the 'Maintenance/Event Log' Button menu Click event. Simulates the user selecting the 'Diagnostics/Event Log' menu option. The
        /// simulation only works if the menu option is enabled and visible.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonDiagnosticsEventLog_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ToolStripMenuItemDiagnosticsEventLog.PerformClick();
        }

        /// <summary>
        /// Event handler for the 'Maintenance/Self-Tests' Button menu Click event. Simulates the user selecting the 'Diagnostics/Self-Tests' menu option. The
        /// simulation only works if the menu option is enabled and visible.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonDiagnosticsSelfTest_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ToolStripMenuItemDiagnosticsSelfTests.PerformClick();
        }

        /// <summary>
        /// Event handler for the 'Maintenance/Spare 1' Button menu Click event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonMaintenanceSpare1_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }
        }

        /// <summary>
        /// Event handler for the 'Maintenance/Spare 2' Button menu Click event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonMaintenanceSpare2_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }
        }
        #endregion - [Maintenance] -

        #region - [Administration] -
        /// <summary>
        /// Event handler for the 'Administration/Initialize Event Logs' Button menu Click event. Simulates the user selecting the 'Diagnostics/Initialize Event Logs'
        /// menu option. The simulation only works if the menu option is enabled and visible.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonDiagnosticsInitializeEventLogs_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ToolStripMenuItemDiagnosticsInitializeEventLogs.PerformClick();

            // Added to remove "blue outline" from button after returning from other form
            m_RemoveBlueHighlightFromButton();
        }

        /// <summary>
        /// Event handler for the 'Administration/Password Protection' Button menu Click event. Simulates the user selecting the 'Configure/Password Protection'
        /// menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonAdministrationPasswordProtection_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether the 'Configure/Password Protection' menu option is hidden and, if so, briefly make it visible before calling the PerformClick() method.
            if (m_ToolStripMenuItemConfigurePasswordProtection.Visible == false)
            {
                m_ToolStripMenuItemConfigurePasswordProtection.Visible = true;
                m_ToolStripMenuItemConfigurePasswordProtection.PerformClick();
                m_ToolStripMenuItemConfigurePasswordProtection.Visible = false;
            }
            else
            {
                m_ToolStripMenuItemConfigurePasswordProtection.PerformClick();
            }

            m_RemoveBlueHighlightFromButton();
        }

        /// <summary>
        /// Removes the "blue outline" or control focus from the button after returning from another form invocation
        /// </summary>
        private void m_RemoveBlueHighlightFromButton()
        {
            this.ActiveControl = null;
        }

        /// <summary>
        ///  Event handler for the 'Administration/Spare 2' Button menu Click event
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_AdministrationSpare2_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }
        }
        #endregion - [Administration] -

        #region - [Control] -
        /// <summary>
        /// Event handler for the 'Help' Button menu Click event. Simulates the user selecting the 'Help/Show User Manual' menu option.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_ButtonHelp_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            m_ToolStripMenuItemHelpShowUserManual.PerformClick();
        }

        /// <summary>
        /// Event handler for the 'Exit' Button menu Click event. Simulates the user selecting the 'File/Exit' menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonExit_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether the 'File/Exit' menu option is hidden and, if so, briefly make it visible before calling the PerformClick() method.
            if (m_ToolStripMenuItemFileExit.Visible == false)
            {
                m_ToolStripMenuItemFileExit.Visible = true;
                m_ToolStripMenuItemFileExit.PerformClick();
                m_ToolStripMenuItemFileExit.Visible = false;
            }
            else
            {
                m_ToolStripMenuItemFileExit.PerformClick();
            }
        }
        #endregion - [Control] -
        #endregion - [Buttons] -

        #region - [TextChanged Events] -
        /// <summary>
        /// Event handler for the 'Log Status' StatusLabel TextChanged event. Writes the contents of the 'Log Status' StatusLabel Text property to the Text property
        /// of the local 'Log Status' Label.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void LogStatus_TextChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether this StatusLabel text normally includes legend information i.e. does it display, for example, 'Log: Saved' or just 'Saved'.
            if (Resources.LegendLogStatus.Equals(string.Empty))
            {
                // The StatusLabel text does not normally include legend information, simply copy the StatusLabel text to the local Label.
                m_LabelLogStatus.Text = m_ToolStripStatusLabelLogStatus.Text;
            }
            else
            {
                // The StatusLabel text normally includes legend information, check whether the current StatusLabel text string includes this information.
                if (m_ToolStripStatusLabelLogStatus.Text.Contains(Resources.LegendLogStatus))
                {
                    // Yes, remove the legend information and display the status.
                    m_LabelLogStatus.Text = m_ToolStripStatusLabelLogStatus.Text.Remove(0, (Resources.LegendLogStatus + CommonConstants.Space).Length);
                }
                else
                {
                    // No, the text string is inconsistent, display string.Empty.
                    m_LabelLogStatus.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Event handler for the 'WibuBox' StatusLabel TextChanged event. Writes the contents of the 'WibuBox' StatusLabel Text property to the Text property
        /// of the local 'WibuBox' Label without any legend text.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void WibuBoxStatus_TextChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether this StatusLabel text normally includes legend information i.e. does it display, for example, 'Log: Saved' or just 'Saved'.
            if (Resources.LegendWibuBoxStatus.Equals(string.Empty))
            {
                // The StatusLabel text does not normally include legend information, simply copy the StatusLabel text to the local Label.
                m_LabelWibuBoxStatus.Text = m_ToolStripStatusLabelWibuBoxStatus.Text;
            }
            else
            {
                // The StatusLabel text normally includes legend information, check whether the current StatusLabel text string includes this information.
                if (m_ToolStripStatusLabelWibuBoxStatus.Text.Contains(Resources.LegendWibuBoxStatus))
                {
                    // Yes, remove the legend information and display the status.
                    m_LabelWibuBoxStatus.Text = m_ToolStripStatusLabelWibuBoxStatus.Text.Remove(0, (Resources.LegendWibuBoxStatus + CommonConstants.Space).Length);
                }
                else
                {
                    // No, the text string is inconsistent, display string.Empty.
                    m_LabelWibuBoxStatus.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Event handler for the 'Car Number' StatusLabel TextChanged event. Writes the contents of the 'Car Number' StatusLabel Text property to the Text property
        /// of the local 'Car Number' Label.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void CarNumber_TextChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether this StatusLabel text normally includes legend information i.e. does it display, for example, 'Log: Saved' or just 'Saved'.
            if (Resources.LegendCarNumber.Equals(string.Empty))
            {
                // The StatusLabel text does not normally include legend information, simply copy the StatusLabel text to the local Label.
                m_LabelCarNumber.Text = m_ToolStripStatusLabelCarNumber.Text;
            }
            else
            {
                // The StatusLabel text normally includes legend information, check whether the current StatusLabel text string includes this information.
                if (m_ToolStripStatusLabelCarNumber.Text.Contains(Resources.LegendCarNumber))
                {
                    // Yes, remove the legend information and display the status.
                    m_LabelCarNumber.Text = m_ToolStripStatusLabelCarNumber.Text.Remove(0, (Resources.LegendCarNumber + CommonConstants.Space).Length);
                }
                else
                {
                    // No, the text string is inconsistent, display string.Empty.
                    m_LabelCarNumber.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Event handler for the 'Mode' StatusLabel TextChanged event. Writes the contents of the 'Mode' StatusLabel Text property to the Text property
        /// of the local 'Mode' Label.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void Mode_TextChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether this StatusLabel text normally includes legend information i.e. does it display, for example, 'Log: Saved' or just 'Saved'.
            if (Resources.LegendMode.Equals(string.Empty))
            {
                // The StatusLabel text does not normally include legend information, simply copy the StatusLabel text to the local Label.
                m_LabelMode.Text = m_ToolStripStatusLabelMode.Text;
            }
            else
            {
                // The StatusLabel text normally includes legend information, check whether the current StatusLabel text string includes this information.
                if (m_ToolStripStatusLabelMode.Text.Contains(Resources.LegendMode))
                {
                    // Yes, remove the legend information and display the status.
                    m_LabelMode.Text = m_ToolStripStatusLabelMode.Text.Remove(0, (Resources.LegendMode + CommonConstants.Space).Length);
                }
                else
                {
                    // No, the text string is inconsistent, display string.Empty.
                    m_LabelMode.Text = string.Empty;
                }
            }

            // Map "Offline" --> "VCU Simulation"
            if (m_LabelMode.Text.Equals(Mode.Offline.ToString()))
            {
                m_LabelMode.Text = Resources.LabelModeVCUSimulation;
            }
        }

        /// <summary>
        /// Event handler for the 'SecurityLevel' StatusLabel TextChanged event. Writes the contents of the 'SecurityLevel' StatusLabel Text property to the Text property
        /// of the local 'SecurityLevel' Label.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void SecurityLevel_TextChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Check whether this StatusLabel text normally includes legend information i.e. does it display, for example, 'Log: Saved' or just 'Saved'.
            if (Resources.LegendSecurityLevel.Equals(string.Empty))
            {
                // The StatusLabel text does not normally include legend information, simply copy the StatusLabel text to the local Label.
                m_LabelSecurityLevel.Text = m_ToolStripStatusLabelSecurityLevel.Text;
            }
            else
            {
                // The StatusLabel text normally includes legend information, check whether the current StatusLabel text string includes this information.
                if (m_ToolStripStatusLabelSecurityLevel.Text.Contains(Resources.LegendSecurityLevel))
                {
                    // Yes, remove the legend information and display the status.
                    m_LabelSecurityLevel.Text = m_ToolStripStatusLabelSecurityLevel.Text.Remove(0, (Resources.LegendSecurityLevel + CommonConstants.Space).Length);
                }
                else
                {
                    // No, the text string is inconsistent, display string.Empty.
                    m_LabelSecurityLevel.Text = string.Empty;
                }
            }

            m_LabelSecurityLevel.Text = m_ToolStripStatusLabelSecurityLevel.Text;
        }
        #endregion - [TextChanged Events] -

        #region - [EnabledChanged Events] -
        #region - [View] -
        /// <summary>
        /// Event handler for the 'View/Watch Window' menu option EnabledChanged event. Updates the Enabled property of the 'Maintenance/Watch Window' Button to reflect
        /// whether the 'View/Watch Window' menu option is currently enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void ViewWatchWindow_EnabledChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // If both the View menu item and the ViewWatchWindow menu item are enabled the ViewWatchWindow button should be enabled; otherwise, it should be
            // disabled.
            m_ButtonViewWatchWindow.Enabled = ((m_ToolStripMenuItemView.Enabled == true) &&
                                               (m_ToolStripMenuItemViewWatchWindow.Enabled == true)) ? true : false;
        }

        /// <summary>
        /// Event handler for the 'View/System Information'  menu option EnabledChanged event. Updates the Enabled property of the 'Maintenance/System Information'
        /// Button to reflect whether the 'View/System Information' menu option is currently enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void ViewSystemInformation_EnabledChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // If both the View menu item and the ViewSystemInformation menu item are enabled the ViewSystemInformation button should be enabled; otherwise, it should be
            // disabled.
            m_ButtonViewSystemInformation.Enabled = ((m_ToolStripMenuItemView.Enabled == true) &&
                                                     (m_ToolStripMenuItemViewSystemInformation.Enabled == true)) ? true : false;
        }
        #endregion - [View] -

        #region - [Diagnostics] -
        /// <summary>
        /// Event handler for the 'Diagnostics/Event Log'  menu option EnabledChanged event. Updates the Enabled property of the 'Maintenance/Event Log' Button to reflect
        /// whether the 'Diagnostics/Event Log' menu option is currently enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void DiagnosticsEventLog_EnabledChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // If both the Diagnostics menu item and the DiagnosticsEventLog menu item are enabled the DiagnosticsEventLog button should be enabled; otherwise, it
            // should be disabled.
            m_ButtonDiagnosticsEventLog.Enabled = ((m_ToolStripMenuItemDiagnostics.Enabled == true) &&
                                                   (m_ToolStripMenuItemDiagnosticsEventLog.Enabled == true)) ? true : false;

            // For the R188 project, disable the 'Event Log' button in Simulation mode.
            switch (this.ProjectIdentifier)
            {
                case CommonConstants.ProjectIdNYCT:
                    if (MainWindow.Mode.Equals(Mode.Offline))
                    {
                        m_ButtonDiagnosticsEventLog.Enabled = false;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler for the 'Diagnostics/Self-Test'  menu option EnabledChanged event. Updates the Enabled property of the 'Maintenance/Self-Tests' Button
        /// to reflect whether the 'Diagnostics/Self-Test' menu option is currently enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void DiagnosticsSelfTests_EnabledChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // If both the Diagnostics menu item and the DiagnosticsSelfTests menu item are enabled the DiagnosticsSelfTests button should be enabled; otherwise, it
            // should be disabled.
            m_ButtonDiagnosticsSelfTest.Enabled = ((m_ToolStripMenuItemDiagnostics.Enabled == true) &&
                                                   (m_ToolStripMenuItemDiagnosticsSelfTests.Enabled == true)) ? true : false;
        }

        /// <summary>
        /// Event handler for the 'Diagnostics/Initialize Event Logs'  menu option EnabledChanged event. Updates the Enabled property of the
        /// 'Administration/Initialize Event Logs' Button to reflect whether the 'Diagnostics/Initialize Event Logs' menu option is currently enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void DiagnosticsInitializeEventLogs_EnabledChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // If both the Diagnostics menu item and the DiagnosticsInitializeEventLogs menu item are enabled the DiagnosticsInitializeEventLogs button should be enabled;
            // otherwise, it should be disabled.
            m_ButtonDiagnosticsInitializeEventLogs.Enabled = ((m_ToolStripMenuItemDiagnostics.Enabled == true) &&
                                                              (m_ToolStripMenuItemDiagnosticsInitializeEventLogs.Enabled == true)) ? true : false;
        }
        #endregion - [Diagnostics] -

        #region - [Configure] -
        /// <summary>
        /// Event handler for the 'Configure/Password Protection'  menu option EnabledChanged event. Updates the Enabled property of the 
        /// 'Administration/Password Protection' Button to reflect whether the 'Configure/Password Protection' menu option is currently enabled.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void ConfigurePasswordProtection_EnabledChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // If both the 'Configure' menu item and the 'Configure/Password Protection' menu item are enabled the 'Password Protection' button should be enabled;
            // otherwise, it should be disabled.
            m_ButtonPasswordProtection.Enabled = ((m_ToolStripMenuItemConfigure.Enabled == true) &&
                                                  (m_ToolStripMenuItemConfigurePasswordProtection.Enabled == true)) ? true : false;
           
        }
        #endregion - [Configure] -
        #endregion - [EnabledChanged Events] -

        #region - [CheckedChanged Events] -
        /// <summary>
        /// Event handler for the 'Online' ToolStripButton CheckedChanged event. Toggles the ToolStripButton Text and ToolTipText properties depending upon
        /// the state of the Checked property.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void Online_CheckedChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            if (m_ToolStripButtonOnline.Checked == true)
            {

                m_ToolStripButtonOnline.Text = Resources.FunctionKeyTextDisconnectLogic;
                m_ToolStripButtonOnline.ToolTipText = Resources.FunctionKeyToolTipTextDisconnectLogic;
            }
            else
            {
                m_ToolStripButtonOnline.Text = Resources.FunctionKeyTextSelectLogic;
                m_ToolStripButtonOnline.ToolTipText = Resources.FunctionKeyToolTipTextSelectLogic;
            }
        }

        /// <summary>
        /// Event handler for the 'Offline' ToolStripButton CheckedChanged event. Toggles the ToolStripButton Text and ToolTipText properties depending upon
        /// the state of the Checked property.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void Offline_CheckedChanged(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            if (m_ToolStripButtonOffline.Checked == true)
            {
                m_ToolStripButtonOffline.Text = Resources.FunctionKeyTextSimulationExit;
                m_ToolStripButtonOffline.ToolTipText = Resources.FunctionKeyToolTipTextSimulationExit;
            }
            else
            {
                m_ToolStripButtonOffline.Text = Resources.FunctionKeyTextSimulationStart;
                m_ToolStripButtonOffline.ToolTipText = Resources.FunctionKeyToolTipTextSimulationStart;
            }
        }
        #endregion - [CheckedChanged Events] -

        /// <summary>
        /// Event handler for the MdiChildActivate event. This event is raised whenever a child Form is displayed in the MDI. The handler checks the Name
        /// of the child Form that has been activated and determines whether this ControlPanel is to be visible or not while the child Form is on display.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MdiChildActivate(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Default is to show the Control Panel.
            this.Visible = true;

            FormPTU formPTU = m_MdiPTU.ActiveMdiChild as FormPTU;
            if (formPTU != null)
            {
                if ((formPTU.Name.Equals(CommonConstants.KeyFormViewWatch)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormViewEventLog)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormViewFaultLog)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormViewTestResults)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormDataStreamReplay)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormOpenWatch)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormOpenSimulatedFaultLog)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormOpenFaultLog)) ||
                    (formPTU.Name.Equals(CommonConstants.KeyFormOpenEventLog)))
                    
                {
                    this.Visible = false;
                }
            }
            else
            {
                this.Visible = true;
            }
        }

        /// <summary>
        /// Event handler for the MenuUpdated event. This event is raised whenever the main menu of the MDI has been updated as a result of a Mode or SecurityLevel
        /// change. The handler hides those menu options and separators that have been replaced by Button controls on the control panel.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MenuUpdated(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // With the exception of the 'File/Exit' and the 'Help/Show User Manual' menu options, hide the menu options that have been replaced by buttons.
            //m_ToolStripMenuItemFileExit.Visible = false;
            m_ToolStripMenuItemView.Visible = false;
            m_ToolStripMenuItemDiagnostics.Visible = false;
            m_ToolStripMenuItemConfigurePasswordProtection.Visible = false;


            // Hide the 'Configure/Password Protection' Separator control.
            m_ToolStripSeparatorConfigurePasswordProtection.Visible = false;
        }

        /// <summary>
        /// Event handler for the FontChanged event. Changes the font of this form and all controls associated with the form to the same font as that of the main window.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        public void ChangeFont(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Change the font associated with various controls associated with this form.
            Font = MainWindow.Font;

            // The Fonts for the following controls need to be set up manually as they are a non-standard size.
            m_LabelControlPanelTitle.Font = new Font(this.Font.FontFamily, (float)15.75, FontStyle.Bold);
            m_LegendPTUStatus.Font = new Font(this.Font.FontFamily, (float)10.0, FontStyle.Bold);
            m_LegendMaintenance.Font = new Font(this.Font.FontFamily, (float)10.0, FontStyle.Bold);
            m_LegendAdministration.Font = new Font(this.Font.FontFamily, (float)10.0, FontStyle.Bold);
        }
        #endregion --- Delegated Methods ---

        #region --- Methods ---
        /// <summary>
        /// Initializes the ControlPanel UserControl.
        /// </summary>
        /// <param name="mainWindow">Reference to the IMAinWindow interface.</param>
        public void InitializeControlPanel(IMainWindow mainWindow)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            MainWindow = mainWindow;
            if (MainWindow == null)
            {
                return;
            }

            // Update the Font.
            ChangeFont(this, new EventArgs());

            // Attach the FontChaged event in the PTU main application window to the ChangeFont event handler.
            MainWindow.FontChanged += new System.EventHandler(ChangeFont);

            #region - [ToolStripButtons] -
            // ---------------------------------------------------------------------------------------------------------------------------
            // Updated the layout of the 'Online' and 'Offline' ToolStripButton controls to match the requirements of the current project.
            // ---------------------------------------------------------------------------------------------------------------------------

            // Get the references to the ToolStripButton controls on the ToolStripFunctionKeys ToolStrip.
            m_ToolStripButtonOnline = (ToolStripButton)MainWindow.ToolStripFunctionKeys.Items[CommonConstants.KeyToolStripButtonOnline];
            m_ToolStripButtonOffline = (ToolStripButton)MainWindow.ToolStripFunctionKeys.Items[CommonConstants.KeyToolStripButtonOffline];

            Debug.Assert(m_ToolStripButtonOnline != null, "ControlPanel.InitializeControlPanel() - m_ToolStripButtonOnline != null");
            Debug.Assert(m_ToolStripButtonOffline != null, "ControlPanel.InitializeControlPanel() - m_ToolStripButtonOffline != null");

            MainWindow.ToolStripFunctionKeys.AutoSize = false;

            #region - [Online] -
            m_ToolStripButtonOnline.DisplayStyle = ToolStripItemDisplayStyle.Text;
            m_ToolStripButtonOnline.TextAlign = ContentAlignment.MiddleCenter;
            m_ToolStripButtonOnline.Size = new Size(136, 32);
            m_ToolStripButtonOnline.Margin = new Padding(13, 0, 13, 0);
            ToolStripSeparator toolStripSeparatorOnlineLHS = (ToolStripSeparator)MainWindow.ToolStripFunctionKeys.Items[CommonConstants.KeyToolStripSeparatorOnlineLHS];
            Debug.Assert(toolStripSeparatorOnlineLHS != null, "ControlPanel.InitializeControlPanel() - toolStripSeparatorOnlineLHS != null");
            toolStripSeparatorOnlineLHS.Visible = true;
            toolStripSeparatorOnlineLHS.Margin = new Padding(8, 0, 0, 0);
            #endregion - [Online] -

            #region - [Offline] -
            m_ToolStripButtonOffline.DisplayStyle = ToolStripItemDisplayStyle.Text;
            m_ToolStripButtonOffline.TextAlign = ContentAlignment.MiddleCenter;
            m_ToolStripButtonOffline.Size = new Size(136, 32);
            m_ToolStripButtonOffline.Margin = new Padding(13, 0, 13, 0);
            ToolStripSeparator toolStripSeparatorOfflineLHS = (ToolStripSeparator)MainWindow.ToolStripFunctionKeys.Items[CommonConstants.KeyToolStripSeparatorOfflineLHS];
            Debug.Assert(toolStripSeparatorOfflineLHS != null, "ControlPanel.InitializeControlPanel() - toolStripSeparatorOfflineLHS != null");
            toolStripSeparatorOfflineLHS.Visible = true;
            toolStripSeparatorOfflineLHS.Margin = new Padding(16, 0, 0, 0);
            #endregion - [Offline] -

            // Attach the CheckedChanged events to the event handlers.
            m_ToolStripButtonOnline.CheckedChanged += new EventHandler(Online_CheckedChanged);
            m_ToolStripButtonOffline.CheckedChanged += new EventHandler(Offline_CheckedChanged);

            // Call each of the CheckedChanged event handlers to ensure that the Text property of the ToolStripButton controls matches the current Checked state.
            Online_CheckedChanged(this, new EventArgs());
            Offline_CheckedChanged(this, new EventArgs());
            #endregion - [ToolStripButtons] -

            #region - [StatusLabels] -
            // Get the references to the ToolStripStatusLabel controls.
            m_ToolStripStatusLabelLogStatus = (ToolStripStatusLabel)MainWindow.StatusStrip.Items[CommonConstants.KeyToolStripStatusLabelLogStatus];
            m_ToolStripStatusLabelCarNumber = (ToolStripStatusLabel)MainWindow.StatusStrip.Items[CommonConstants.KeyToolStripStatusLabelCarNumber];
            m_ToolStripStatusLabelMode = (ToolStripStatusLabel)MainWindow.StatusStrip.Items[CommonConstants.KeyToolStripStatusLabelMode];
            m_ToolStripStatusLabelSecurityLevel = (ToolStripStatusLabel)MainWindow.StatusStrip.Items[CommonConstants.KeyToolStripStatusLabelSecurityLevel];

            Debug.Assert(m_ToolStripStatusLabelLogStatus != null, "ControlPanel.InitializeControlPanel() - m_ToolStripStatusLabelLogStatus != null");
            Debug.Assert(m_ToolStripStatusLabelCarNumber != null, "ControlPanel.InitializeControlPanel() - m_ToolStripStatusLabelCarNumber != null");
            Debug.Assert(m_ToolStripStatusLabelMode != null, "ControlPanel.InitializeControlPanel() - m_ToolStripStatusLabelMode != null");
            Debug.Assert(m_ToolStripStatusLabelSecurityLevel != null, "ControlPanel.InitializeControlPanel() - m_ToolStripStatusLabelSecurityLevel != null");

            // Attach the TextChanged events to the event handlers.
            m_ToolStripStatusLabelLogStatus.TextChanged += new EventHandler(LogStatus_TextChanged);
            m_ToolStripStatusLabelCarNumber.TextChanged +=new EventHandler(CarNumber_TextChanged);
            m_ToolStripStatusLabelMode.TextChanged += new EventHandler(Mode_TextChanged);
            m_ToolStripStatusLabelSecurityLevel.TextChanged += new EventHandler(SecurityLevel_TextChanged);

            // Call each of the event handlers to ensure that the Text property of the Labels accurately reflect the Text property of its associated
            // ToolStripStatusLabel.
            LogStatus_TextChanged(this, new EventArgs());
            CarNumber_TextChanged(this, new EventArgs());
            Mode_TextChanged(this, new EventArgs());
            SecurityLevel_TextChanged(this, new EventArgs());

            // Hide the ToolStripStatusLabel controls that are displayed on the Control Panel.
            m_ToolStripStatusLabelLogStatus.Visible = false;
            m_ToolStripStatusLabelCarNumber.Visible = false;
            m_ToolStripStatusLabelMode.Visible = false;
            m_ToolStripStatusLabelSecurityLevel.Visible = false;
            #endregion - [StatusLabels] -

            #region - [WibuBox] -
            if (this.WibuBoxRequired == true)
            {
                // Get the reference to the ToolStripStatusLabel controls.
                m_ToolStripStatusLabelWibuBoxStatus = (ToolStripStatusLabel)MainWindow.StatusStrip.Items[CommonConstants.KeyToolStripStatusLabelWibuBoxStatus];
                Debug.Assert(m_ToolStripStatusLabelWibuBoxStatus != null, "ControlPanel.InitializeControlPanel() - m_ToolStripStatusLabelWibuBoxStatus != null");


                // Attach the TextChanged event to the event handler.
                m_ToolStripStatusLabelWibuBoxStatus.TextChanged += new EventHandler(WibuBoxStatus_TextChanged);

                // Call the WibuBox event handler to ensure that the Text property of the label accurately reflect the Text property of its associated
                // ToolStripStatusLabel.
                WibuBoxStatus_TextChanged(this, new EventArgs());

                // Hide the ToolStripStatusLabel control that is displayed on the Control Panel.
                m_ToolStripStatusLabelWibuBoxStatus.Visible = false;

                m_LabelWibuBoxStatus.Visible = true;
                m_LegendWibuBox.Visible = true;
            }
            else
            {
                m_LabelWibuBoxStatus.Visible = false;
                m_LegendWibuBox.Visible = false;
            }
            #endregion - [WibuBox] -

            #region - [Menu Items] -
            #region - [File] -
            m_ToolStripMenuItemFile = ((ToolStripMenuItem)MainWindow.MenuStrip.Items[CommonConstants.KeyMenuItemFile]);
            Debug.Assert(m_ToolStripMenuItemFile != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemFile != null");
            m_ToolStripMenuItemFileExit = ((ToolStripMenuItem)m_ToolStripMenuItemFile.DropDownItems[CommonConstants.KeyMenuItemFileExit]);
            m_ToolStripSeparatorFileOpenDataDictionary =
                ((ToolStripSeparator)m_ToolStripMenuItemFile.DropDownItems[CommonConstants.KeyToolStripSeparatorFileOpenDataDictionary]);
            Debug.Assert(m_ToolStripMenuItemFileExit != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemFileExit != null");
            Debug.Assert(m_ToolStripSeparatorFileOpenDataDictionary != null, 
                "ControlPanel.InitializeControlPanel() - m_ToolStripSeparatorFileOpenDataDictionary != null");
            #endregion - [File] -

            #region - [View] -
            m_ToolStripMenuItemView = ((ToolStripMenuItem)MainWindow.MenuStrip.Items[CommonConstants.KeyMenuItemView]);
            Debug.Assert(m_ToolStripMenuItemView != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemView != null");
            m_ToolStripMenuItemViewWatchWindow = ((ToolStripMenuItem)m_ToolStripMenuItemView.DropDownItems[CommonConstants.KeyMenuItemViewWatchWindow]);
            m_ToolStripMenuItemViewSystemInformation = ((ToolStripMenuItem)m_ToolStripMenuItemView.DropDownItems[CommonConstants.KeyMenuItemViewSystemInformation]);
            Debug.Assert(m_ToolStripMenuItemViewWatchWindow != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemViewWatchWindow != null");
            Debug.Assert(m_ToolStripMenuItemViewSystemInformation != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemViewSystemInformation != null");

            // Attach the EnabledChanged events to the event handlers.
            m_ToolStripMenuItemView.EnabledChanged += new EventHandler(ViewWatchWindow_EnabledChanged);
            m_ToolStripMenuItemViewWatchWindow.EnabledChanged += new EventHandler(ViewWatchWindow_EnabledChanged);

            m_ToolStripMenuItemView.EnabledChanged += new EventHandler(ViewSystemInformation_EnabledChanged);
            m_ToolStripMenuItemViewSystemInformation.EnabledChanged += new EventHandler(ViewSystemInformation_EnabledChanged);

            // Call each 'View' event handler in turn to ensure that the Enabled property of each button matches that of the corresponding ToolStripMenuItem.
            ViewWatchWindow_EnabledChanged(this, new EventArgs());
            ViewSystemInformation_EnabledChanged(this, new EventArgs());
            #endregion - [View] -

            #region - [Diagnostics] -
            m_ToolStripMenuItemDiagnostics = ((ToolStripMenuItem)MainWindow.MenuStrip.Items[CommonConstants.KeyMenuItemDiagnostics]);
            Debug.Assert(m_ToolStripMenuItemDiagnostics != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemDiagnostics != null");
            m_ToolStripMenuItemDiagnosticsEventLog = ((ToolStripMenuItem)m_ToolStripMenuItemDiagnostics.DropDownItems[CommonConstants.KeyMenuItemDiagnosticsEventLog]);
            m_ToolStripMenuItemDiagnosticsSelfTests = ((ToolStripMenuItem)m_ToolStripMenuItemDiagnostics.DropDownItems[CommonConstants.KeyMenuItemDiagnosticsSelfTests]);
            m_ToolStripMenuItemDiagnosticsInitializeEventLogs =
                ((ToolStripMenuItem)m_ToolStripMenuItemDiagnostics.DropDownItems[CommonConstants.KeyMenuItemDiagnosticsInitializeEventLogs]);
            Debug.Assert(m_ToolStripMenuItemDiagnosticsEventLog != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemDiagnosticsEventLog != null");
            Debug.Assert(m_ToolStripMenuItemDiagnosticsSelfTests != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemDiagnosticsSelfTests != null");
            Debug.Assert(m_ToolStripMenuItemDiagnosticsInitializeEventLogs != null,
                         "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemDiagnosticsInitializeEventLogs != null");

            // Attach the EnabledChanged events to the event handlers.
            m_ToolStripMenuItemDiagnostics.EnabledChanged += new EventHandler(DiagnosticsEventLog_EnabledChanged);
            m_ToolStripMenuItemDiagnosticsEventLog.EnabledChanged += new EventHandler(DiagnosticsEventLog_EnabledChanged);

            m_ToolStripMenuItemDiagnostics.EnabledChanged += new EventHandler(DiagnosticsSelfTests_EnabledChanged);
            m_ToolStripMenuItemDiagnosticsSelfTests.EnabledChanged += new EventHandler(DiagnosticsSelfTests_EnabledChanged);

            m_ToolStripMenuItemDiagnostics.EnabledChanged += new EventHandler(DiagnosticsInitializeEventLogs_EnabledChanged);
            m_ToolStripMenuItemDiagnosticsInitializeEventLogs.EnabledChanged += new EventHandler(DiagnosticsInitializeEventLogs_EnabledChanged);

            // Call each 'Diagnostics'event handler in turn to ensure that the Enabled property of each button matches that of the corresponding ToolStripMenuItem.
            DiagnosticsEventLog_EnabledChanged(this, new EventArgs());
            DiagnosticsSelfTests_EnabledChanged(this, new EventArgs());
            DiagnosticsInitializeEventLogs_EnabledChanged(this, new EventArgs());
            #endregion - [Diagnostics] -

            #region - [Configure] -
            m_ToolStripMenuItemConfigure = ((ToolStripMenuItem)MainWindow.MenuStrip.Items[CommonConstants.KeyMenuItemConfigure]);
            Debug.Assert(m_ToolStripMenuItemConfigure != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemConfigure != null");
            m_ToolStripMenuItemConfigurePasswordProtection =
                    ((ToolStripMenuItem)m_ToolStripMenuItemConfigure.DropDownItems[CommonConstants.KeyMenuItemConfigurePasswordProtection]);
            m_ToolStripSeparatorConfigurePasswordProtection =
                ((ToolStripSeparator)m_ToolStripMenuItemConfigure.DropDownItems[CommonConstants.KeyToolStripSeparatorConfigurePasswordProtection]);
            Debug.Assert(m_ToolStripMenuItemConfigurePasswordProtection != null,
                         "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemConfigurePasswordProtection != null");

            // Attach the EnabledChanged events to the event handlers.
            m_ToolStripMenuItemConfigure.EnabledChanged +=new EventHandler(ConfigurePasswordProtection_EnabledChanged);
            m_ToolStripMenuItemConfigurePasswordProtection.EnabledChanged += new EventHandler(ConfigurePasswordProtection_EnabledChanged);

            // Call each 'Diagnostics'event handler in turn to ensure that the Enabled property of each button matches that of the corresponding ToolStripMenuItem.
            ConfigurePasswordProtection_EnabledChanged(this, new EventArgs());
            #endregion - [Configure] -

            #region - [Help] -
            m_ToolStripMenuItemHelp = ((ToolStripMenuItem)MainWindow.MenuStrip.Items[CommonConstants.KeyMenuItemHelp]);
            Debug.Assert(m_ToolStripMenuItemHelp != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemHelp != null");
            m_ToolStripMenuItemHelpShowUserManual = ((ToolStripMenuItem)m_ToolStripMenuItemHelp.DropDownItems[CommonConstants.KeyMenuItemHelpShowUserManual]);
            m_ToolStripSeparatorHelpShowUserManual = ((ToolStripSeparator)m_ToolStripMenuItemHelp.DropDownItems[CommonConstants.KeyToolStripSeparatorHelpShowUserManual]);
            Debug.Assert(m_ToolStripMenuItemHelpShowUserManual != null, "ControlPanel.InitializeControlPanel() - m_ToolStripMenuItemHelpShowUserManual != null");
            Debug.Assert(m_ToolStripSeparatorHelpShowUserManual != null, "ControlPanel.InitializeControlPanel() - m_ToolStripSeparatorHelpShowUserManual != null");
            #endregion - [Help] -
            #endregion - [Menu Items] -

            m_MdiPTU = this.Parent as MdiPTU;
            m_MdiPTU.MdiChildActivate += new EventHandler(MdiChildActivate);

            // Call the MdiChildActivate event handler to ensure that the Visible properties of this UserControl and the ToolStripFunctionKeys ToolStrip reflect the
            // current status.
            MdiChildActivate(this, new EventArgs());

            // Hide the menu options that have been replaced by Control Panel buttons whenever the menu is updated as a result of a security level or mode change.
            m_MdiPTU.MenuUpdated +=new EventHandler(MenuUpdated);

            // Call the MenuUpdated event handler to ensure that the menu options that have been replaced by Control Panel buttons are hidden.
            MenuUpdated(this, new EventArgs());
        }
        #endregion --- Methods ---

        #region --- Properties ---
        /// <summary>
        /// Get or set the reference to the IMainWindow interface.
        /// </summary>
        public IMainWindow MainWindow
        {
            get { return m_MainWindow; }
            set { m_MainWindow = value; }
        }

        /// <summary>
        /// Get or set the title of the control panel.
        /// </summary>
        public string ControlPanelTitle
        {
            get { return m_LabelControlPanelTitle.Text; }
            set
            {
                m_LabelControlPanelTitle.Text = value;
            }
        }

        /// <summary>
        /// Get or set the sub-system code text.
        /// </summary>
        public string SubsystemCode
        {
            get { return m_LabelSubsystemCode.Text; }
            set
            {
                m_LabelSubsystemCode.Text = value;

                // If the sub-system code is an empty string, hide the label and corresponding legend.
                if (m_LabelSubsystemCode.Text.Equals(string.Empty))
                {
                    m_LabelSubsystemCode.Visible = false;
                    m_LegendSubsystem.Visible = false;
                }
                else
                {
                    m_LabelSubsystemCode.Visible = true;
                    m_LegendSubsystem.Visible = false;
                }
            }
        }

        /// <summary>
        /// Get or set the location code text. The location code is used to specify which connector the PTU is connected to in to the car. This field is blank where 
        /// the location is not ambiguous i.e. only one PTU connector exists on a car.
        /// </summary>
        public string LocationCode
        {
            get { return m_LabelLocationCode.Text; }
            set
            {
                m_LabelLocationCode.Text = value;

                // If the location code is an empty string, hide the label and corresponding legend.
                if (m_LabelLocationCode.Text.Equals(string.Empty))
                {
                    m_LabelLocationCode.Visible = false;
                    m_LegendLocation.Visible = false;
                }
                else
                {
                    m_LabelLocationCode.Visible = true;
                    m_LegendLocation.Visible = true;
                }
            }
        }

        /// <summary>
        /// Get or set the connection text. This is normally either "Direct" or "Remote". Direct means that the PTU is connected to the PTU connector on the 
        /// Supplier Subsystem. Remote means that the PTU is connected via a MDS PTU connector.
        /// </summary>
        public string Connection
        {
            get { return m_LabelConnection.Text; }
            set
            {
                m_LabelConnection.Text = value;

                // If the connection type is an empty string, hide the label and corresponding legend.
                if (m_LabelConnection.Text.Equals(string.Empty))
                {
                    m_LabelConnection.Visible = false;
                    m_LegendConnection.Visible = false;
                }
                else
                {
                    m_LabelConnection.Visible = true;
                    m_LegendConnection.Visible = true;
                }
            }
        }


        /// <summary>
        /// Get or set the flag that controls whether the control panel is to support the WibuBox security system. True, if support for the WibuBox security system is
        /// required; otherwise, false.
        /// </summary>
        public bool WibuBoxRequired
        {
            get { return m_WibuBoxSupportRequired; }
            set
            {
                m_WibuBoxSupportRequired = value;
            }
        }

        /// <summary>
        /// Get or set the project identifier associated with the control panel.
        /// </summary>
        public string ProjectIdentifier
        {
            get { return m_ProjectIdentifier; }
            set
            {
                m_ProjectIdentifier = value;
            }
        }
        #endregion --- Properties ---
    }
}


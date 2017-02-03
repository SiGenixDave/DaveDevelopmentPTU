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
 *  Project:    Common
 * 
 *  File name:  FormSetSecurityLevel.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  11/23/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  02/18/11    1.1     K.McD           1.  Modified the constructor such that only the security levels available to the client are added to the Items property of the 
 *                                          ComboBox control.
 *                                          
 *  09/09/2016  1.2     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 15, 22, 23, 23, 25, 47, 48. Add 'Delete', 'Set As Default' and 'Override Security'
 *                                          ToolStripButton controls to the Chart Recorder, Data Stream and Watch Window configuration dialogbox forms. On selecting the
 *                                          'Delete' ToolStripButton, a pop-up asking 'Are you sure you want to delete the ...?' should appear with the option to
 *                                          answer 'Yes' or 'Cancel'.
 *  
 *                                      Modifications
 *                                      1.  Set all Windows Form Designer variables to null and detached all event handlers. - Internal Audit of Code.
 *                                      2.  Modified the 'OK' button event handler to include support for the 'Factory' security level.
 *                                      3.  Renamed the parameter name from securityLevelCurrent to securityLevelWorkset as this makes more logical sense. No
 *                                          functional change to code.
 *                                      4.  Modified the constructor such that the ComboBox now displays all security levels up to and including the current security
 *                                          level rather than just up to the 'Engineering' security level.
 *
 */
#endregion --- Revision History ---

using System;
using System.Windows.Forms;

using Common.Configuration;
using Common.Properties;

namespace Common.Forms
{
    /// <summary>
    /// Form to allow the user to modify the security level associated with a workset.
    /// </summary>
    public partial class FormSetSecurityLevel : FormPTUDialog
    {
        #region --- Member Variables ---
        /// <summary>
        /// The selected security level of the workset.
        /// </summary>
        private SecurityLevel m_SecurityLevel;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes a new instance of the class. Zero parameter constructor.
        /// </summary>
        public FormSetSecurityLevel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Loads the <c>ComboBox</c> control with the security level descriptions associated with the project and sets the 
        /// <c>Text</c> property of the <c>ComboBox</c> control to the description corresponding to the specified security level.
        /// </summary>
        /// <param name="name">The name of the workset.</param>
        /// <param name="securityLevelWorkset">The current security level of the workset.</param>
        public FormSetSecurityLevel(string name, SecurityLevel securityLevelWorkset)
        {
            InitializeComponent();

            m_LabelWorkset.Text = name;

            // Populate the ComboBox control with the available security levels. This is linked to the security level of the current user in that the available
            // security levels are from the base security level up to and including the security level of the current user.
            for (short securityLevel = (short)Security.SecurityLevelBase; securityLevel <= (short)Security.SecurityLevelCurrent; securityLevel++ )
            {
                string description = Security.GetSecurityDescription((SecurityLevel)securityLevel);
                m_ComboBoxSecurityLevel.Items.Add(description);
            }

            // Display the security level of the selected workset.
            switch (securityLevelWorkset)
            {
                case SecurityLevel.Level0:
                    m_ComboBoxSecurityLevel.Text = Security.DescriptionLevel0;
                    break;
                case SecurityLevel.Level1:
                    m_ComboBoxSecurityLevel.Text = Security.DescriptionLevel1;
                    break;
                case SecurityLevel.Level2:
                    m_ComboBoxSecurityLevel.Text = Security.DescriptionLevel2;
                    break;
                case SecurityLevel.Level3:
                    m_ComboBoxSecurityLevel.Text = Security.DescriptionLevel3;
                    break;
                default:
                    m_ComboBoxSecurityLevel.Text = SecurityLevel.Undefined.ToString();
                    break;
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
                this.m_ButtonCancel.Click -= new System.EventHandler(this.m_ButtonCancel_Click);
                this.m_ButtonOK.Click -= new System.EventHandler(this.m_ButtonOK_Click);

                // Set the Windows Form Designer Variables to null.
                m_ComboBoxSecurityLevel = null;
                m_LegendSecurityLevel = null;
                m_PanelOuter = null;
                m_ButtonOK = null;
                m_ButtonCancel = null;
                m_LegendWorkset = null;
                m_LabelWorkset = null;
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
        /// Event handler for the Canel button <c>Click</c> event. Close the form.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Event handler for the OK button <c>Click</c> event. Set the <c>SecurityLevel</c> property to the selected security level.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_ButtonOK_Click(object sender, EventArgs e)
        {
            if (m_ComboBoxSecurityLevel.Text.Equals(Security.DescriptionLevel0))
            {
                m_SecurityLevel = SecurityLevel.Level0;
            }
            else if (m_ComboBoxSecurityLevel.Text.Equals(Security.DescriptionLevel1))
            {
                m_SecurityLevel = SecurityLevel.Level1;
            }
            else if (m_ComboBoxSecurityLevel.Text.Equals(Security.DescriptionLevel2))
            {
                m_SecurityLevel = SecurityLevel.Level2;
            }
            else if (m_ComboBoxSecurityLevel.Text.Equals(Security.DescriptionLevel3))
            {
                m_SecurityLevel = SecurityLevel.Level3;
            }
            else
            {
                m_SecurityLevel = SecurityLevel.Undefined;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion --- Delegated Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets the selected security level of the workset.
        /// </summary>
        public SecurityLevel SecurityLevel
        {
            get { return m_SecurityLevel; }
        }
        #endregion --- Properties ---

    }
}
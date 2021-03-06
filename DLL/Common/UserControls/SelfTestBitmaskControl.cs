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
 *  File name:  SelfTestBitmaskControl.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  07/21/11    1.0     K.McD           1.  First entry into TortoiseSVN.
 *  
 *  07/29/11    1.1     K.McD           1.  Modified the m_MenuItemShowFlags_DoubleClick() method to call the FormShowFlagsSelfTest form. 
 */

/*
 *  11/16/15    1.2     K.McD       References
 *                                  1.  Bug Fix - SNCR-R188 PTU [20th Mar 2015] Item 35. The �Show Flags� context menu associated with viewing the event variables
 *                                      does not work correctly, however, double-clicking on the 'Units' section of the Event Variable User Control does display the *
 *                                      'FormShowFlags' form.
 *                                  
 *                                  Modifications
 *                                  1.  Renamed the m_MenuItemShowFlags_DoubleClick() event handler to m_MenuItemShowFlags_Click().
 *                                  2.  Removed the m_LabelNameField_DoubleClick() event handler. This event is re-directed in the SelfTestControl.Designer.cs file. 
 *
 *  07/22/2016  1.3     K.McD       References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 63. Change the '(Bit Mask)' component of the Watch Variable UserControl to say 'Detail'
 *                                      and make it look like a clickable button.
 *                                          
 *                                  Modifications
 *                                  1.  Added an override to the WidthUnitsField property.
 *                                  2.  Modified Cleanup() to detach the Button control Click event handler.
 */
#endregion --- Revision History ---

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;

using Common;
using Common.Configuration;
using Common.Forms;
using Common.Properties;

namespace Common.UserControls
{
    /// <summary>
    /// The bitmask self test variable user control. Displays the asserted flags corresponding to the specified <c>Value</c> property for the bit mask self test 
    /// variable specified by the <c>Identifier</c> property.
    /// </summary>
    public partial class SelfTestBitmaskControl : SelfTestControl
    {
        #region --- Member Variables ---
        /// <summary>
        /// The value cast to a uint.
        /// </summary>
        uint m_ValueUINT;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes anew instance of the user control.
        /// </summary>
        public SelfTestBitmaskControl()
        {
            InitializeComponent();
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

                #region - [Detach the event handler methods.] -
                this.m_LabelNameField.DoubleClick -= new System.EventHandler(this.m_MenuItemShowDefinition_Click);
                this.m_LabelUnitsField.DoubleClick -= new System.EventHandler(this.m_MenuItemShowFlags_Click);
                this.m_MenuItemShowDefinition.Click -= new System.EventHandler(this.m_MenuItemShowDefinition_Click);
                this.m_MenuItemShowFlags.DoubleClick -= new System.EventHandler(this.m_MenuItemShowFlags_Click);
                this.m_ButtonDetails.Click -= new System.EventHandler(this.m_MenuItemShowFlags_Click);
                #endregion - [Detach the event handler methods.] -
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
        #region - [Context Menu] -
        /// <summary>
        /// Event handler for the 'Show Definition' context menu option <c>Click</c> event. Call the ShowHelpPopup() method.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemShowDefinition_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            ShowHelpPopup();
        }

        /// <summary>
        /// Event handler for the 'Show Flags' context menu option <c>Click</c> event. 
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemShowFlags_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Skip, if the ClientForm propery is null.
            if (ClientForm != null)
            {
                FormShowFlagsSelfTest formShowFlagsSelfTest = new FormShowFlagsSelfTest(this);
                formShowFlagsSelfTest.CalledFrom = m_ClientForm;
                formShowFlagsSelfTest.Show();
            }
        }
        #endregion - [Context Menu] -
        #endregion --- Delegated Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets or sets the width of the units field, in pixels.
        /// </summary>
        [
        Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("Appearance"),
        Description("The width of the units field, in pixels.")
        ]
        public override int WidthUnitsField
        {
            get { return m_LabelUnitsField.Width; }
            set
            {
                m_LabelUnitsField.Width = value;
                Width = m_LabelNameField.Width + m_LabelValueField.Width + m_LabelUnitsField.Width;
                m_ButtonDetails.Location = m_LabelUnitsField.Location;
                m_ButtonDetails.Width = m_LabelUnitsField.Width - WidthVerticalScrollBar;
            }
        }

        /// <summary>
        /// Gets or sets the current value of the bit mask self test variable.
        /// </summary>
        [
        Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("false"),
        Description("The current value of the bitmask watch variable.")
        ]
        public override double Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                m_ValueUINT = (uint)m_Value;

                if (InvalidValue == true)
                {
                    m_LabelValueField.Text = string.Empty;
                }
                else
                {
                    // Check whether the watch variable is defined.
                    if (m_LabelNameField.Text != CommonConstants.VariableNotDefinedString)
                    {
                        // The text that is to be displayed in the value field.
                        string valueText;
                        valueText = HexValueIdentifier + m_ValueUINT.ToString(FormatStringHex);

                        m_LabelValueField.Text = valueText;
                    }
                    else
                    {
                        m_Value = double.NaN;
                        m_ValueUINT = 0;
                        m_LabelValueField.Text = string.Empty;
                        return;
                    }
                }
            }
        }
        #endregion --- Properties ---
    }
}

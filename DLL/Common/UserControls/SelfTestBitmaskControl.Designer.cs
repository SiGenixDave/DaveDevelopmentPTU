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
 *  10/01/11    1.1     K.McD           1.  Corrected the name of the control.  
 */

/*
 *  11/16/15    1.2     K.McD       References
 *                                  1.  Bug Fix - SNCR-R188 PTU [20th Mar 2015] Item 35. The ‘Show Flags’ context menu associated with viewing the event variables
 *                                      does not work correctly, however, double-clicking on the 'Units' section of the Event Variable User Control does display the 
 *                                      'FormShowFlags' form.
 *                                  
 *                                  Modifications
 *                                  1.  Auto-modified as a result of the name change to the m_MenuItemShowFlags_Click() event handler.
 *                                  2.  Modified such that the m_MenuItemShowFlags() event handler is linked to the m_MenuItemShowFlags.Click event, not the DoubleClick
 *                                      event.
 *                                      
 *  07/22/2016  1.3     K.McD       References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 63. Change the '(Bit Mask)' component of the Watch Variable UserControl to say 'Detail'
 *                                      and make it look like a clickable button.
 *                                          
 *                                  Modifications
 *                                  1.  Added the 'Details' Button control and attached the 'Click' event to the m_MenuItemShowFlags_Click() event handler.
 */
#endregion --- Revision History ---

namespace Common.UserControls
{
    partial class SelfTestBitmaskControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_ContextMenuBitmask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_MenuItemShowDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuItemShowFlags = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ButtonDetails = new System.Windows.Forms.Button();
            this.m_ContextMenuBitmask.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_LabelNameField
            // 
            this.m_LabelNameField.DoubleClick += new System.EventHandler(this.m_MenuItemShowDefinition_Click);
            // 
            // m_LabelUnitsField
            // 
            this.m_LabelUnitsField.DoubleClick += new System.EventHandler(this.m_MenuItemShowFlags_Click);
            // 
            // m_ContextMenuBitmask
            // 
            this.m_ContextMenuBitmask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_MenuItemShowDefinition,
            this.m_MenuItemShowFlags});
            this.m_ContextMenuBitmask.Name = "m_ContextMenuBitmask";
            this.m_ContextMenuBitmask.Size = new System.Drawing.Size(159, 48);
            // 
            // m_MenuItemShowDefinition
            // 
            this.m_MenuItemShowDefinition.Image = global::Common.Properties.Resources.Help;
            this.m_MenuItemShowDefinition.Name = "m_MenuItemShowDefinition";
            this.m_MenuItemShowDefinition.Size = new System.Drawing.Size(158, 22);
            this.m_MenuItemShowDefinition.Text = "Show &Definition";
            this.m_MenuItemShowDefinition.Click += new System.EventHandler(this.m_MenuItemShowDefinition_Click);
            // 
            // m_MenuItemShowFlags
            // 
            this.m_MenuItemShowFlags.Name = "m_MenuItemShowFlags";
            this.m_MenuItemShowFlags.Size = new System.Drawing.Size(158, 22);
            this.m_MenuItemShowFlags.Text = "Show &Flags";
            this.m_MenuItemShowFlags.Click += new System.EventHandler(this.m_MenuItemShowFlags_Click);
            // 
            // m_ButtonDetails
            // 
            this.m_ButtonDetails.Location = new System.Drawing.Point(300, 0);
            this.m_ButtonDetails.Name = "m_ButtonDetails";
            this.m_ButtonDetails.Size = new System.Drawing.Size(100, 23);
            this.m_ButtonDetails.TabIndex = 0;
            this.m_ButtonDetails.TabStop = false;
            this.m_ButtonDetails.Text = "Details";
            this.m_ButtonDetails.UseVisualStyleBackColor = true;
            this.m_ButtonDetails.Click += new System.EventHandler(this.m_MenuItemShowFlags_Click);
            // 
            // SelfTestBitmaskControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ContextMenuStrip = this.m_ContextMenuBitmask;
            this.Controls.Add(this.m_ButtonDetails);
            this.Name = "SelfTestBitmaskControl";
            this.Controls.SetChildIndex(this.m_LabelNameField, 0);
            this.Controls.SetChildIndex(this.m_LabelValueField, 0);
            this.Controls.SetChildIndex(this.m_LabelUnitsField, 0);
            this.Controls.SetChildIndex(this.m_ButtonDetails, 0);
            this.m_ContextMenuBitmask.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip m_ContextMenuBitmask;
        private System.Windows.Forms.ToolStripMenuItem m_MenuItemShowDefinition;
        private System.Windows.Forms.ToolStripMenuItem m_MenuItemShowFlags;
        private System.Windows.Forms.Button m_ButtonDetails;
    }
}

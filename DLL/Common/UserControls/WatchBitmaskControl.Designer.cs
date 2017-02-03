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
 *  File name:  WatchBitmaskControl.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  11/02/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  07/22/2016  1.1     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 63. Change the '(Bit Mask)' component of the Watch Variable UserControl to say 'Detail'
 *                                          and make it look like a clickable button.
 *                                          
 *                                      Modifications
 *                                      1.  Added the 'Details' Button control and attached the 'Click' event to the m_MenuItemShowFlags_Click() event handler.
 */
#endregion --- Revision History ---

using System;
using System.Windows.Forms;

namespace Common.UserControls
{
    partial class WatchBitmaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatchBitmaskControl));
            this.m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_MenuItemShowDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuItemChangeValue = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuItemShowFlags = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ButtonDetails = new System.Windows.Forms.Button();
            this.m_ContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_LabelNameField
            // 
            this.m_LabelNameField.DoubleClick += new System.EventHandler(this.m_MenuItemShowDefinition_Click);
            // 
            // m_LabelValueField
            // 
            this.m_LabelValueField.DoubleClick += new System.EventHandler(this.m_LabelValueField_DoubleClick);
            // 
            // m_LabelUnitsField
            // 
            this.m_LabelUnitsField.DoubleClick += new System.EventHandler(this.m_MenuItemShowFlags_Click);
            // 
            // m_ContextMenu
            // 
            this.m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_MenuItemShowDefinition,
            this.m_MenuItemChangeValue,
            this.m_MenuItemShowFlags});
            this.m_ContextMenu.Name = "m_ContextMenu";
            this.m_ContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.m_ContextMenu.Size = new System.Drawing.Size(159, 70);
            // 
            // m_MenuItemShowDefinition
            // 
            this.m_MenuItemShowDefinition.Image = ((System.Drawing.Image)(resources.GetObject("m_MenuItemShowDefinition.Image")));
            this.m_MenuItemShowDefinition.Name = "m_MenuItemShowDefinition";
            this.m_MenuItemShowDefinition.Size = new System.Drawing.Size(158, 22);
            this.m_MenuItemShowDefinition.Text = "Show &Definition";
            this.m_MenuItemShowDefinition.Click += new System.EventHandler(this.m_MenuItemShowDefinition_Click);
            // 
            // m_MenuItemChangeValue
            // 
            this.m_MenuItemChangeValue.Name = "m_MenuItemChangeValue";
            this.m_MenuItemChangeValue.Size = new System.Drawing.Size(158, 22);
            this.m_MenuItemChangeValue.Text = "Change &Value";
            this.m_MenuItemChangeValue.Click += new System.EventHandler(this.m_MenuItemChangeValue_Click);
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
            // WatchBitmaskControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ContextMenuStrip = this.m_ContextMenu;
            this.Controls.Add(this.m_ButtonDetails);
            this.Name = "WatchBitmaskControl";
            this.DoubleClick += new System.EventHandler(this.m_MenuItemShowFlags_Click);
            this.Controls.SetChildIndex(this.m_LabelNameField, 0);
            this.Controls.SetChildIndex(this.m_LabelValueField, 0);
            this.Controls.SetChildIndex(this.m_LabelUnitsField, 0);
            this.Controls.SetChildIndex(this.m_ButtonDetails, 0);
            this.m_ContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Reference to the context menu associated with this user control.
        /// </summary>
        protected System.Windows.Forms.ContextMenuStrip m_ContextMenu;

        /// <summary>
        /// Reference to the 'Show Definition' menu option of the context menu.
        /// </summary>
        protected System.Windows.Forms.ToolStripMenuItem m_MenuItemShowDefinition;

        /// <summary>
        /// Reference to the 'Show Flags' menu option of the context menu.
        /// </summary>
        protected System.Windows.Forms.ToolStripMenuItem m_MenuItemShowFlags;
        private ToolStripMenuItem m_MenuItemChangeValue;
        private Button m_ButtonDetails;
    }
}

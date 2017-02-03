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
 *  File name:  FormWorksetManagerFaultLog.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  10/13/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  02/28/11    1.1     K.McD           1.  Modified the title text.
 *                                      2.  Modified the AutoScaleMode property to Inherit.
 * 
 *  04/27/11    1.2     K.McD           1.  Modified the title of the form.
 *  
 *  03/13/15    1.3     K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order TBD.
 *                                          1.  Changes to address the items outlined in the minutes of the meeting between BTPC,  Kawasaki Rail
 *                                              Car and NYTC on 12th April 2013 - MOC-0171:
 *                                               
 *                                              1.  MOC-0171-06. All references to fault logs, including menu options and directory names to be
 *                                                  replaced by 'data streams' for all projects.
 *                                                   
 *                                      Modifications
 *                                      1.  Changed the form title to "Manage - Data Stream Worksets".
 *                                      
 *  08/02/2016  1.4     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Items 16, 24, 61.  For the Chart Recorder, Data Stream and Watch Window configuration
 *                                          dialog boxes, change the word 'Workset' to 'Chart Recorder Configuration', 'Data Stream Configuration' and 'Watch Window
 *                                          Configuration' respectively and change the title of each dialogbox to be 'Configure - Chart Recorder', 'Configure - Data
 *                                          Stream' and 'Configure - Watch Window'.
 *                                  
 *                                      Modifications
 *                                      1.  Changed the form title to 'Manage - Data Stream Configuration List'.
 *
 */
#endregion --- Revision History ---

namespace Event.Forms
{
    partial class FormWorksetManagerFaultLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWorksetManagerFaultLog));
            this.SuspendLayout();
            // 
            // m_TextBoxNotes
            // 
            this.m_TextBoxNotes.Text = resources.GetString("m_TextBoxNotes.Text");
            // 
            // FormWorksetManagerFaultLog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Name = "FormWorksetManagerFaultLog";
            this.Text = "Manage - Data Stream Configuration List";
            this.ResumeLayout(false);
        }

        #endregion
    }
}
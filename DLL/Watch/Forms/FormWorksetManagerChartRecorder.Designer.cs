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
 *  File name:  FormWorsetManagerChartRecorder.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  04/11/11    1.0     K.McD           1.  First entry into TortoiseSVN.
 *  
 *  08/02/2016  1.1     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Items 16, 24, 61.  For the Chart Recorder, Data Stream and Watch Window configuration
 *                                          dialog boxes, change the word 'Workset' to 'Chart Recorder Configuration', 'Data Stream Configuration' and 'Watch Window
 *                                          Configuration' respectively and change the title of each dialogbox to be 'Configure - Chart Recorder', 'Configure - Data
 *                                          Stream' and 'Configure - Watch Window'
 *                                          
 *                                      2.  Bug Fix - The contents of the 'Notes' GroupBox control on ALL dialogboxes derived from the 'Workset Manager' form cannot be
 *                                          fully seen as the size of the dialogboxes is too small.
 *                                  
 *                                      Modifications
 *                                      1.  Changed the form title to 'Manage - Chart Recorder Configuration List'. - Ref.: 1.
 *                                      2.  Changed the ClentSize property of the form. - Ref.: 2.
 */
#endregion --- Revision History ---

namespace Watch.Forms
{
    partial class FormWorksetManagerChartRecorder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWorksetManagerChartRecorder));
            this.SuspendLayout();
            // 
            // m_TextBoxNotes
            // 
            this.m_TextBoxNotes.Text = resources.GetString("m_TextBoxNotes.Text");
            // 
            // m_ImageList
            // 
            this.m_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImageList.ImageStream")));
            this.m_ImageList.Images.SetKeyName(0, "Blank.png");
            this.m_ImageList.Images.SetKeyName(1, "TickHS.png");
            this.m_ImageList.Images.SetKeyName(2, "Book_openHS.png");
            // 
            // FormWorksetManagerChartRecorder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(470, 562);
            this.Name = "FormWorksetManagerChartRecorder";
            this.Text = "Manage - Chart Recorder Configuration List";
            this.ResumeLayout(false);

        }
        #endregion
    }
}
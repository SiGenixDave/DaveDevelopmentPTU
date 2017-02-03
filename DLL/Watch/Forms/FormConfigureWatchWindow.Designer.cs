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
 *  File name:  FormConfigureWatchWindow.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  09/06/2016  1.0     K.McD           1.  First entry into TortoiseSVN.
 */
#endregion --- Revision History ---

namespace Watch.Forms
{
    partial class FormConfigureWatchWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Resizes and repositions the <c>ListBox</c> controls for when the 'Row Header' <c>ListBox</c> is not used i.e. 
        /// if the project supports multiple data stream types or the number of parameters supported by the workset exceeds the number that can be 
        /// displayed on the <c>TabPage</c> without the use of scroll bars.
        /// </summary>
        /// <remarks>The 'Total Count' <c>Label</c> i.e. 'Total Count: nn of 20', can't be displayed if the project supports multiple data stream types
        /// as the upper limit is the maximum number of watch variables and may not apply to the current workset, which could be confusing to the 
        /// operator.</remarks>
        private void NoRowHeader()
        {
            // Resize and reposition the ListBox controls.
            this.m_ListBox1.Location = new System.Drawing.Point(8, 61);
            this.m_ListBox1.Size = new System.Drawing.Size(274, 264);

            this.m_ListBox2.Location = new System.Drawing.Point(8, 61);
            this.m_ListBox2.Size = new System.Drawing.Size(274, 264);

            this.m_ListBox3.Location = new System.Drawing.Point(8, 61);
            this.m_ListBox3.Size = new System.Drawing.Size(274, 264);

            // Reposition the 'Column Header' Label.
            this.m_LabelListBox1ColumnHeader.Location = new System.Drawing.Point(8, 44);
            this.m_LabelListBox2ColumnHeader.Location = new System.Drawing.Point(8, 44);
            this.m_LabelListBox3ColumnHeader.Location = new System.Drawing.Point(8, 44);

            this.m_LabelCount1.Location = new System.Drawing.Point(8, 328);
            this.m_LabelCount2.Location = new System.Drawing.Point(8, 328);
            this.m_LabelCount3.Location = new System.Drawing.Point(8, 328);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxDefault)).BeginInit();
            this.m_PanelOuter.SuspendLayout();
            this.m_GroupBoxWorkset.SuspendLayout();
            this.m_TabControlColumn.SuspendLayout();
            this.m_TabPageColumn1.SuspendLayout();
            this.m_TabPageColumn2.SuspendLayout();
            this.m_TabPageColumn3.SuspendLayout();
            this.m_GroupBoxAvailable.SuspendLayout();
            this.m_TabControlAvailable.SuspendLayout();
            this.m_TabPageAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ComboBoxWorkset
            // 
            this.m_ComboBoxWorkset.Location = new System.Drawing.Point(110, 17);
            // 
            // m_PanelOuter
            // 
            this.m_PanelOuter.Size = new System.Drawing.Size(703, 500);
            // 
            // m_TextBoxName
            // 
            this.m_TextBoxName.Location = new System.Drawing.Point(110, 17);
            this.m_TextBoxName.Visible = false;
            // 
            // m_LegendHeader1
            // 
            this.m_LegendHeader1.Visible = true;
            // 
            // m_TextBoxHeader1
            // 
            this.m_TextBoxHeader1.Visible = true;
            // 
            // m_ButtonOK
            // 
            this.m_ButtonOK.Location = new System.Drawing.Point(462, 564);
            // 
            // m_ButtonCancel
            // 
            this.m_ButtonCancel.Location = new System.Drawing.Point(541, 564);
            // 
            // m_ButtonApply
            // 
            this.m_ButtonApply.Location = new System.Drawing.Point(620, 564);
            // 
            // m_ListBox1RowHeader
            // 
            this.m_ListBox1RowHeader.Visible = false;
            // 
            // m_PanelStatusMessage
            // 
            this.m_PanelStatusMessage.Location = new System.Drawing.Point(10, 561);
            // 
            // FormConfigureWatchWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(703, 600);
            this.Name = "FormConfigureWatchWindow";
            this.Text = "Configure - Watch Window";
            this.Controls.SetChildIndex(this.m_PanelStatusMessage, 0);
            this.Controls.SetChildIndex(this.m_ButtonOK, 0);
            this.Controls.SetChildIndex(this.m_ButtonCancel, 0);
            this.Controls.SetChildIndex(this.m_ButtonApply, 0);
            this.Controls.SetChildIndex(this.m_PanelOuter, 0);
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxDefault)).EndInit();
            this.m_PanelOuter.ResumeLayout(false);
            this.m_PanelOuter.PerformLayout();
            this.m_GroupBoxWorkset.ResumeLayout(false);
            this.m_GroupBoxWorkset.PerformLayout();
            this.m_TabControlColumn.ResumeLayout(false);
            this.m_TabPageColumn1.ResumeLayout(false);
            this.m_TabPageColumn1.PerformLayout();
            this.m_TabPageColumn2.ResumeLayout(false);
            this.m_TabPageColumn2.PerformLayout();
            this.m_TabPageColumn3.ResumeLayout(false);
            this.m_TabPageColumn3.PerformLayout();
            this.m_GroupBoxAvailable.ResumeLayout(false);
            this.m_TabControlAvailable.ResumeLayout(false);
            this.m_TabPageAll.ResumeLayout(false);
            this.m_TabPageAll.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
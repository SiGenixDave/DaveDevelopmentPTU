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
 *  File name:  FormConfigureChartRecorder.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  04/15/11    1.0     K.McD           1.  First entry into TortoiseSVN.
 *  
 *  10/26/11    1.1     K.McD           1.  Moved the location of the Panel control associated with the status message.
 *  
 *  03/26/15    1.2     K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *  
 *                                          1.  Changes to address the items outlined in the minutes of the meeting between BTPC, 
 *                                              Kawasaki Rail Car and NYTC on 12th April 2013 - MOC-0171:
 *                                               
 *                                              1.  MOC-0171-26. Kawasaki pointed out that Chart Recorder tabs are identified as ‘COLUMNS’ and requested that this
 *                                                  be changed.
 *                                                  
 *                                      Modifications
 *                                      1.  Removed the Text property of the 'Workset Layout' GroupBox control. Now displays 'Workset Layout'.
 *                                      2.  Changed the text associated with the TabPage header to 'Chart Recorder'.
 *                                      3.  Changed the text associated with the list box title label to 'Channel List'.
 *                                      
 *  07/11/2016  1.3     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 26. Do not include the individual TabPage 1 Count value for the DataStream
 *                                          Configuration screen, just show the Total Count value.
 *                                      
 *                                      Modifications
 *                                      1.  Set the Visible property of the m_LabelCount1 control to false. - Ref.: 1.
 *                                      
 *  07/27/2016  1.4     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 18. Use a double-click to change the chart scale factor on the
 *                                          'Chart Recorder Configuration' dialog box.
 *                                      
 *                                      Modifications
 *                                      1.  Registered the m_MenuItemChangeChartScaleFactor_Click() event handler with the 'Column 1' ListBox control 'DoubleClick'
 *                                          event.
 *                                          
 *  08/01/2016  1.5     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Items 16, 24, 61.  For the Chart Recorder, Data Stream and Watch Window configuration
 *                                          dialog boxes, change the word 'Workset' to 'Chart Recorder', 'Data Stream' and 'Watch Window' respectively and change the 
 *                                          title of each dialogbox to be 'Configure - Chart Recorder', 'Configure - Data Stream' and 'Configure - Watch Window'.
 *  
 *                                      Modifications
 *                                      1.  Changed the Location and Size properties of the m_ComboBoxWorkset ComboBox and m_TextBoxName TextBox controls.
 *                                      2.  Renamed m_LegendName to m_LegendName1 and added m_LegendName2. - Ref.: 2.
 *                                      3.  Changed the Text property of the m_GroupBoxWorkset GroupBox to 'Chart Recorder Channel Layout'.
 *                                      4.  Changed the Text property of the m_TabPageColumn1 TabPage to 'Channel List'.
 *                                      5.  Set the Visible property of the m_LabelListBoxTitle Label to false.
 *                                      
 *  08/10/2016  1.6     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 17. Add the options for Chart Mode from the original Configure drop-down menu as
 *                                          buttons on the dialogbox used to configure the chart recorder.
 *                                          
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 15, 22, 23, 23, 25, 47, 48. Add 'Delete', 'Set As Default' and 'Override Security'
 *                                          ToolStripButton controls to the Chart Recorder, Data Stream and Watch Window configuration dialogbox forms. On selecting the
 *                                          'Delete' ToolStripButton, a pop-up asking 'Are you sure you want to delete the ...?' should appear with the option to
 *                                          answer 'Yes' or 'Cancel'.
 *  
 *                                      Modifications
 *                                      1.  Added the 'Chart Mode' GroupBox and ToolStrip controls. - Ref.: 1.
 *                                      2.  Added and configured the 'Chart Mode' ToolStripButton and ToolStripSeparator controls associated with the 'Chart Mode'
 *                                          ToolStrip. - Ref.: 1.
 *                                      3.  Linked the 'Click' events associated with the 'Chart Mode' ToolStripButton controls to the appropriate event handlers.
 *                                          - Ref.: 1.
 *                                      4.  Changed to location of the 'Is Default' image associated with FormConfigure. - Ref.: 2.
 */
#endregion --- Revision History ---

namespace Watch.Forms
{
    partial class FormConfigureChartRecorder
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
            this.m_ListBoxUnits = new System.Windows.Forms.ListBox();
            this.m_ListBoxChartScaleLowerLimit = new System.Windows.Forms.ListBox();
            this.m_ListBoxChartScaleUpperLimit = new System.Windows.Forms.ListBox();
            this.m_LabelListBoxUnitsColumnHeader = new System.Windows.Forms.Label();
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader = new System.Windows.Forms.Label();
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader = new System.Windows.Forms.Label();
            this.m_LegendListBoxTitle = new System.Windows.Forms.Label();
            this.m_GroupBoxChartMode = new System.Windows.Forms.GroupBox();
            this.m_ToolStripChartMode = new System.Windows.Forms.ToolStrip();
            this.m_TSBDataMode = new System.Windows.Forms.ToolStripButton();
            this.m_TSSeparatorDataMode = new System.Windows.Forms.ToolStripSeparator();
            this.m_TSBRampMode = new System.Windows.Forms.ToolStripButton();
            this.m_TSSeparatorRampMode = new System.Windows.Forms.ToolStripSeparator();
            this.m_TSBZeroOutput = new System.Windows.Forms.ToolStripButton();
            this.m_TSSeparatorZeroOutput = new System.Windows.Forms.ToolStripSeparator();
            this.m_TSBFullScale = new System.Windows.Forms.ToolStripButton();
            this.m_TSSeparatorFullScale = new System.Windows.Forms.ToolStripSeparator();
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
            this.m_GroupBoxChartMode.SuspendLayout();
            this.m_ToolStripChartMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_PictureBoxDefault
            // 
            this.m_PictureBoxDefault.Location = new System.Drawing.Point(499, 17);
            // 
            // m_ComboBoxWorkset
            // 
            this.m_ComboBoxWorkset.Location = new System.Drawing.Point(120, 17);
            this.m_ComboBoxWorkset.Size = new System.Drawing.Size(365, 21);
            // 
            // m_PanelOuter
            // 
            this.m_PanelOuter.Controls.Add(this.m_GroupBoxChartMode);
            this.m_PanelOuter.Size = new System.Drawing.Size(971, 500);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_PictureBoxDefault, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_LegendName2, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_TextBoxName, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_LegendName1, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_GroupBoxAvailable, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_GroupBoxWorkset, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_TextBoxSecurityLevel, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_LegendSecurity, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_ComboBoxWorkset, 0);
            this.m_PanelOuter.Controls.SetChildIndex(this.m_GroupBoxChartMode, 0);
            // 
            // m_LegendName1
            // 
            this.m_LegendName1.Size = new System.Drawing.Size(85, 13);
            this.m_LegendName1.Text = "Chart Recorder :";
            // 
            // m_TextBoxName
            // 
            this.m_TextBoxName.Location = new System.Drawing.Point(120, 17);
            this.m_TextBoxName.Size = new System.Drawing.Size(365, 20);
            // 
            // m_LegendSecurity
            // 
            this.m_LegendSecurity.Location = new System.Drawing.Point(565, 19);
            // 
            // m_TextBoxSecurityLevel
            // 
            this.m_TextBoxSecurityLevel.Location = new System.Drawing.Point(627, 17);
            // 
            // m_GroupBoxWorkset
            // 
            this.m_GroupBoxWorkset.Size = new System.Drawing.Size(541, 313);
            this.m_GroupBoxWorkset.Text = "Chart Recorder Channel Layout";
            // 
            // m_TabControlColumn
            // 
            this.m_TabControlColumn.Size = new System.Drawing.Size(467, 251);
            // 
            // m_TabPageColumn1
            // 
            this.m_TabPageColumn1.Controls.Add(this.m_LabelListBoxUnitsColumnHeader);
            this.m_TabPageColumn1.Controls.Add(this.m_LabelListBoxChartScaleUpperLimitColumnHeader);
            this.m_TabPageColumn1.Controls.Add(this.m_LabelListBoxChartScaleLowerLimitColumnHeader);
            this.m_TabPageColumn1.Controls.Add(this.m_ListBoxUnits);
            this.m_TabPageColumn1.Controls.Add(this.m_ListBoxChartScaleUpperLimit);
            this.m_TabPageColumn1.Controls.Add(this.m_ListBoxChartScaleLowerLimit);
            this.m_TabPageColumn1.Controls.Add(this.m_LegendListBoxTitle);
            this.m_TabPageColumn1.Size = new System.Drawing.Size(459, 225);
            this.m_TabPageColumn1.Text = "Channel List";
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LabelCount1, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LegendHeader1, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_ListBox1RowHeader, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_TextBoxHeader1, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LegendListBoxTitle, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_ListBox1, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_ListBoxChartScaleLowerLimit, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_ListBoxChartScaleUpperLimit, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_ListBoxUnits, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LabelListBox1ColumnHeader, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LabelListBoxChartScaleLowerLimitColumnHeader, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LabelListBoxChartScaleUpperLimitColumnHeader, 0);
            this.m_TabPageColumn1.Controls.SetChildIndex(this.m_LabelListBoxUnitsColumnHeader, 0);
            // 
            // m_TabPageColumn2
            // 
            this.m_TabPageColumn2.Size = new System.Drawing.Size(459, 225);
            // 
            // m_TabPageColumn3
            // 
            this.m_TabPageColumn3.Size = new System.Drawing.Size(459, 225);
            // 
            // m_ListBox1
            // 
            this.m_ListBox1.Size = new System.Drawing.Size(252, 134);
            this.m_ListBox1.DoubleClick += new System.EventHandler(this.m_MenuItemChangeChartScaleFactor_Click);
            // 
            // m_LabelCount1
            // 
            this.m_LabelCount1.Location = new System.Drawing.Point(27, 198);
            this.m_LabelCount1.Visible = false;
            // 
            // m_LabelCountTotal
            // 
            this.m_LabelCountTotal.Location = new System.Drawing.Point(9, 274);
            // 
            // m_TextBoxHeader1
            // 
            this.m_TextBoxHeader1.TabIndex = 0;
            this.m_TextBoxHeader1.TabStop = false;
            // 
            // m_GroupBoxAvailable
            // 
            this.m_GroupBoxAvailable.Location = new System.Drawing.Point(559, 54);
            // 
            // m_ButtonRemove
            // 
            this.m_ButtonRemove.Location = new System.Drawing.Point(399, 277);
            // 
            // m_ButtonMoveUp
            // 
            this.m_ButtonMoveUp.Location = new System.Drawing.Point(483, 137);
            // 
            // m_ButtonMoveDown
            // 
            this.m_ButtonMoveDown.Location = new System.Drawing.Point(483, 184);
            // 
            // m_ButtonOK
            // 
            this.m_ButtonOK.Location = new System.Drawing.Point(730, 564);
            // 
            // m_ButtonCancel
            // 
            this.m_ButtonCancel.Location = new System.Drawing.Point(809, 564);
            // 
            // m_ButtonApply
            // 
            this.m_ButtonApply.Location = new System.Drawing.Point(888, 564);
            // 
            // m_ListBox1RowHeader
            // 
            this.m_ListBox1RowHeader.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.m_ListBox1RowHeader.Size = new System.Drawing.Size(30, 130);
            // 
            // m_LabelListBox1ColumnHeader
            // 
            this.m_LabelListBox1ColumnHeader.AutoSize = false;
            this.m_LabelListBox1ColumnHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_LabelListBox1ColumnHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_LabelListBox1ColumnHeader.Location = new System.Drawing.Point(30, 39);
            this.m_LabelListBox1ColumnHeader.Size = new System.Drawing.Size(253, 23);
            // 
            // m_PanelStatusMessage
            // 
            this.m_PanelStatusMessage.Location = new System.Drawing.Point(10, 561);
            // 
            // m_ListBoxUnits
            // 
            this.m_ListBoxUnits.BackColor = System.Drawing.SystemColors.Window;
            this.m_ListBoxUnits.FormattingEnabled = true;
            this.m_ListBoxUnits.Location = new System.Drawing.Point(389, 61);
            this.m_ListBoxUnits.Name = "m_ListBoxUnits";
            this.m_ListBoxUnits.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.m_ListBoxUnits.Size = new System.Drawing.Size(60, 134);
            this.m_ListBoxUnits.TabIndex = 0;
            this.m_ListBoxUnits.TabStop = false;
            // 
            // m_ListBoxChartScaleLowerLimit
            // 
            this.m_ListBoxChartScaleLowerLimit.BackColor = System.Drawing.SystemColors.Window;
            this.m_ListBoxChartScaleLowerLimit.FormatString = "###,###,##0.####";
            this.m_ListBoxChartScaleLowerLimit.FormattingEnabled = true;
            this.m_ListBoxChartScaleLowerLimit.Location = new System.Drawing.Point(281, 61);
            this.m_ListBoxChartScaleLowerLimit.Name = "m_ListBoxChartScaleLowerLimit";
            this.m_ListBoxChartScaleLowerLimit.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.m_ListBoxChartScaleLowerLimit.Size = new System.Drawing.Size(55, 134);
            this.m_ListBoxChartScaleLowerLimit.TabIndex = 0;
            this.m_ListBoxChartScaleLowerLimit.TabStop = false;
            // 
            // m_ListBoxChartScaleUpperLimit
            // 
            this.m_ListBoxChartScaleUpperLimit.BackColor = System.Drawing.SystemColors.Window;
            this.m_ListBoxChartScaleUpperLimit.FormatString = "###,###,##0.####";
            this.m_ListBoxChartScaleUpperLimit.FormattingEnabled = true;
            this.m_ListBoxChartScaleUpperLimit.Location = new System.Drawing.Point(335, 61);
            this.m_ListBoxChartScaleUpperLimit.Name = "m_ListBoxChartScaleUpperLimit";
            this.m_ListBoxChartScaleUpperLimit.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.m_ListBoxChartScaleUpperLimit.Size = new System.Drawing.Size(55, 134);
            this.m_ListBoxChartScaleUpperLimit.TabIndex = 0;
            this.m_ListBoxChartScaleUpperLimit.TabStop = false;
            // 
            // m_LabelListBoxUnitsColumnHeader
            // 
            this.m_LabelListBoxUnitsColumnHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_LabelListBoxUnitsColumnHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_LabelListBoxUnitsColumnHeader.Location = new System.Drawing.Point(389, 39);
            this.m_LabelListBoxUnitsColumnHeader.Name = "m_LabelListBoxUnitsColumnHeader";
            this.m_LabelListBoxUnitsColumnHeader.Size = new System.Drawing.Size(60, 23);
            this.m_LabelListBoxUnitsColumnHeader.TabIndex = 0;
            this.m_LabelListBoxUnitsColumnHeader.Text = "Units";
            this.m_LabelListBoxUnitsColumnHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_LabelListBoxChartScaleLowerLimitColumnHeader
            // 
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.Location = new System.Drawing.Point(281, 39);
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.Name = "m_LabelListBoxChartScaleLowerLimitColumnHeader";
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.Size = new System.Drawing.Size(55, 23);
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.TabIndex = 0;
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.Text = "Lower";
            this.m_LabelListBoxChartScaleLowerLimitColumnHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_LabelListBoxChartScaleUpperLimitColumnHeader
            // 
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.Location = new System.Drawing.Point(335, 39);
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.Name = "m_LabelListBoxChartScaleUpperLimitColumnHeader";
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.Size = new System.Drawing.Size(55, 23);
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.TabIndex = 0;
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.Text = "Upper";
            this.m_LabelListBoxChartScaleUpperLimitColumnHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_LegendListBoxTitle
            // 
            this.m_LegendListBoxTitle.AutoSize = true;
            this.m_LegendListBoxTitle.BackColor = System.Drawing.Color.Transparent;
            this.m_LegendListBoxTitle.Location = new System.Drawing.Point(27, 22);
            this.m_LegendListBoxTitle.Name = "m_LegendListBoxTitle";
            this.m_LegendListBoxTitle.Size = new System.Drawing.Size(65, 13);
            this.m_LegendListBoxTitle.TabIndex = 9;
            this.m_LegendListBoxTitle.Text = "Channel List";
            this.m_LegendListBoxTitle.Visible = false;
            // 
            // m_GroupBoxChartMode
            // 
            this.m_GroupBoxChartMode.Controls.Add(this.m_ToolStripChartMode);
            this.m_GroupBoxChartMode.Location = new System.Drawing.Point(877, 16);
            this.m_GroupBoxChartMode.Name = "m_GroupBoxChartMode";
            this.m_GroupBoxChartMode.Size = new System.Drawing.Size(87, 472);
            this.m_GroupBoxChartMode.TabIndex = 4;
            this.m_GroupBoxChartMode.TabStop = false;
            this.m_GroupBoxChartMode.Text = "Chart Mode";
            // 
            // m_ToolStripChartMode
            // 
            this.m_ToolStripChartMode.AutoSize = false;
            this.m_ToolStripChartMode.BackColor = System.Drawing.SystemColors.Control;
            this.m_ToolStripChartMode.Dock = System.Windows.Forms.DockStyle.None;
            this.m_ToolStripChartMode.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.m_ToolStripChartMode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_TSBDataMode,
            this.m_TSSeparatorDataMode,
            this.m_TSBRampMode,
            this.m_TSSeparatorRampMode,
            this.m_TSBZeroOutput,
            this.m_TSSeparatorZeroOutput,
            this.m_TSBFullScale,
            this.m_TSSeparatorFullScale});
            this.m_ToolStripChartMode.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.m_ToolStripChartMode.Location = new System.Drawing.Point(5, 16);
            this.m_ToolStripChartMode.Name = "m_ToolStripChartMode";
            this.m_ToolStripChartMode.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.m_ToolStripChartMode.ShowItemToolTips = false;
            this.m_ToolStripChartMode.Size = new System.Drawing.Size(77, 450);
            this.m_ToolStripChartMode.TabIndex = 0;
            // 
            // m_TSBDataMode
            // 
            this.m_TSBDataMode.AutoSize = false;
            this.m_TSBDataMode.Image = global::Watch.Properties.Resources.Data;
            this.m_TSBDataMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_TSBDataMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.m_TSBDataMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_TSBDataMode.Name = "m_TSBDataMode";
            this.m_TSBDataMode.Size = new System.Drawing.Size(75, 50);
            this.m_TSBDataMode.Text = "Da&ta Mode";
            this.m_TSBDataMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_TSBDataMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_TSBDataMode.Click += new System.EventHandler(this.m_TSBDataMode_Click);
            // 
            // m_TSSeparatorDataMode
            // 
            this.m_TSSeparatorDataMode.Name = "m_TSSeparatorDataMode";
            this.m_TSSeparatorDataMode.Size = new System.Drawing.Size(75, 6);
            // 
            // m_TSBRampMode
            // 
            this.m_TSBRampMode.AutoSize = false;
            this.m_TSBRampMode.Image = global::Watch.Properties.Resources.Ramp;
            this.m_TSBRampMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_TSBRampMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.m_TSBRampMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_TSBRampMode.Name = "m_TSBRampMode";
            this.m_TSBRampMode.Size = new System.Drawing.Size(75, 50);
            this.m_TSBRampMode.Text = "R&amp Mode";
            this.m_TSBRampMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_TSBRampMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_TSBRampMode.Click += new System.EventHandler(this.m_TSBRampMode_Click);
            // 
            // m_TSSeparatorRampMode
            // 
            this.m_TSSeparatorRampMode.Name = "m_TSSeparatorRampMode";
            this.m_TSSeparatorRampMode.Size = new System.Drawing.Size(75, 6);
            // 
            // m_TSBZeroOutput
            // 
            this.m_TSBZeroOutput.AutoSize = false;
            this.m_TSBZeroOutput.Image = global::Watch.Properties.Resources.Zero_Output;
            this.m_TSBZeroOutput.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_TSBZeroOutput.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.m_TSBZeroOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_TSBZeroOutput.Name = "m_TSBZeroOutput";
            this.m_TSBZeroOutput.Size = new System.Drawing.Size(77, 50);
            this.m_TSBZeroOutput.Text = "&Zero Output";
            this.m_TSBZeroOutput.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_TSBZeroOutput.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_TSBZeroOutput.ToolTipText = "t";
            this.m_TSBZeroOutput.Click += new System.EventHandler(this.m_TSBZeroOutput_Click);
            // 
            // m_TSSeparatorZeroOutput
            // 
            this.m_TSSeparatorZeroOutput.Name = "m_TSSeparatorZeroOutput";
            this.m_TSSeparatorZeroOutput.Size = new System.Drawing.Size(75, 6);
            // 
            // m_TSBFullScale
            // 
            this.m_TSBFullScale.AutoSize = false;
            this.m_TSBFullScale.Image = global::Watch.Properties.Resources.Full_Scale;
            this.m_TSBFullScale.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_TSBFullScale.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.m_TSBFullScale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_TSBFullScale.Name = "m_TSBFullScale";
            this.m_TSBFullScale.Size = new System.Drawing.Size(75, 50);
            this.m_TSBFullScale.Text = "Fu&ll Scale";
            this.m_TSBFullScale.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_TSBFullScale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_TSBFullScale.Click += new System.EventHandler(this.m_TSBFullScale_Click);
            // 
            // m_TSSeparatorFullScale
            // 
            this.m_TSSeparatorFullScale.Name = "m_TSSeparatorFullScale";
            this.m_TSSeparatorFullScale.Size = new System.Drawing.Size(75, 6);
            // 
            // FormConfigureChartRecorder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(971, 600);
            this.Name = "FormConfigureChartRecorder";
            this.Text = "Configure - Chart Recorder";
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
            this.m_GroupBoxChartMode.ResumeLayout(false);
            this.m_ToolStripChartMode.ResumeLayout(false);
            this.m_ToolStripChartMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Reference to the 'Units' <c>ListBox</c>.
        /// </summary>
        private System.Windows.Forms.ListBox m_ListBoxUnits;

        /// <summary>
        /// Reference to the 'Chart Scale/Lower Limit' <c>ListBox</c>.
        /// </summary>
        private System.Windows.Forms.ListBox m_ListBoxChartScaleLowerLimit;

        /// <summary>
        /// Reference to the 'Chart Scale/Upper Limit' <c>ListBox</c>.
        /// </summary>
        private System.Windows.Forms.ListBox m_ListBoxChartScaleUpperLimit;

        /// <summary>
        /// Reference to the 'Units' Header <c>Label</c>.
        /// </summary>
        private System.Windows.Forms.Label m_LabelListBoxUnitsColumnHeader;

        /// <summary>
        /// Reference to the 'Chart Scale/Lower Limit' Header <c>Label</c>.
        /// </summary>
        private System.Windows.Forms.Label m_LabelListBoxChartScaleLowerLimitColumnHeader;

        /// <summary>
        /// Reference to the 'Chart Scale/Upper Limit' Header <c>Label</c>.
        /// </summary>
        private System.Windows.Forms.Label m_LabelListBoxChartScaleUpperLimitColumnHeader;

        /// <summary>
        /// Reference to the 'Title' Legend.
        /// </summary>
        private System.Windows.Forms.Label m_LegendListBoxTitle;

        /// <summary>
        /// Reference to the 'Chart Mode' GroupBox control.
        /// </summary>
        private System.Windows.Forms.GroupBox m_GroupBoxChartMode;

        /// <summary>
        /// Reference to the 'Chart Mode' ToolStrip control.
        /// </summary>
        private System.Windows.Forms.ToolStrip m_ToolStripChartMode;

        /// <summary>
        /// Reference to the 'Data Mode' ToolStripButton control.
        /// </summary>
        private System.Windows.Forms.ToolStripButton m_TSBDataMode;

        /// <summary>
        /// Reference to the 'Data Mode' ToolStripSeparator control.
        /// </summary>
        private System.Windows.Forms.ToolStripSeparator m_TSSeparatorDataMode;

        /// <summary>
        /// Reference to the 'Ramp Mode' ToolStripSeparator control.
        /// </summary>
        private System.Windows.Forms.ToolStripSeparator m_TSSeparatorRampMode;

        /// <summary>
        /// Reference to the 'Zero Output Mode' ToolStripButton control.
        /// </summary>
        private System.Windows.Forms.ToolStripButton m_TSBZeroOutput;

        /// <summary>
        /// Reference to the 'Zero Output Mode' ToolStripSeparator control.
        /// </summary>
        private System.Windows.Forms.ToolStripSeparator m_TSSeparatorZeroOutput;

        /// <summary>
        /// Reference to the 'Full Scale Mode' ToolStripButton control.
        /// </summary>
        private System.Windows.Forms.ToolStripButton m_TSBFullScale;

        /// <summary>
        /// Reference to the 'Full Scale Mode' ToolStripSeparator control.
        /// </summary>
        private System.Windows.Forms.ToolStripSeparator m_TSSeparatorFullScale;

        /// <summary>
        /// Reference to the 'Ramp Mode' ToolStripButton control.
        /// </summary>
        private System.Windows.Forms.ToolStripButton m_TSBRampMode;
    }
}
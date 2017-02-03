namespace FTPDownloadRTDM
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonSelectVCU = new System.Windows.Forms.Button();
            this.buttonStartDownload = new System.Windows.Forms.Button();
            this.buttonCancelDownload = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.numUpDownFTPTimeout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cBoxClearRTDMData = new System.Windows.Forms.CheckBox();
            this.cBoxRTDMDownload = new System.Windows.Forms.CheckBox();
            this.cBoxIELFDownload = new System.Windows.Forms.CheckBox();
            this.cBoxClearIELFData = new System.Windows.Forms.CheckBox();
            this.groupBoxRTDM = new System.Windows.Forms.GroupBox();
            this.groupBoxIELF = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownFTPTimeout)).BeginInit();
            this.groupBoxRTDM.SuspendLayout();
            this.groupBoxIELF.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSelectVCU
            // 
            this.buttonSelectVCU.Location = new System.Drawing.Point(265, 40);
            this.buttonSelectVCU.Name = "buttonSelectVCU";
            this.buttonSelectVCU.Size = new System.Drawing.Size(87, 29);
            this.buttonSelectVCU.TabIndex = 4;
            this.buttonSelectVCU.Text = "Select VCU(s)";
            this.buttonSelectVCU.UseVisualStyleBackColor = true;
            this.buttonSelectVCU.Click += new System.EventHandler(this.buttonSelectVCU_Click);
            this.buttonSelectVCU.MouseLeave += new System.EventHandler(this.buttonSelectVCU_MouseLeave);
            this.buttonSelectVCU.MouseHover += new System.EventHandler(this.buttonSelectVCU_MouseHover);
            // 
            // buttonStartDownload
            // 
            this.buttonStartDownload.Enabled = false;
            this.buttonStartDownload.Location = new System.Drawing.Point(15, 19);
            this.buttonStartDownload.Name = "buttonStartDownload";
            this.buttonStartDownload.Size = new System.Drawing.Size(66, 28);
            this.buttonStartDownload.TabIndex = 5;
            this.buttonStartDownload.Text = "Start";
            this.buttonStartDownload.UseVisualStyleBackColor = true;
            this.buttonStartDownload.Click += new System.EventHandler(this.buttonStartDownload_Click);
            this.buttonStartDownload.MouseLeave += new System.EventHandler(this.buttonStartDownload_MouseLeave);
            this.buttonStartDownload.MouseHover += new System.EventHandler(this.buttonStartDownload_MouseHover);
            // 
            // buttonCancelDownload
            // 
            this.buttonCancelDownload.Enabled = false;
            this.buttonCancelDownload.Location = new System.Drawing.Point(87, 19);
            this.buttonCancelDownload.Name = "buttonCancelDownload";
            this.buttonCancelDownload.Size = new System.Drawing.Size(70, 28);
            this.buttonCancelDownload.TabIndex = 6;
            this.buttonCancelDownload.Text = "Cancel";
            this.buttonCancelDownload.UseVisualStyleBackColor = true;
            this.buttonCancelDownload.Click += new System.EventHandler(this.buttonCancelDownload_Click);
            this.buttonCancelDownload.MouseLeave += new System.EventHandler(this.buttonCancelDownload_MouseLeave);
            this.buttonCancelDownload.MouseHover += new System.EventHandler(this.buttonCancelDownload_MouseHover);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(546, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.Location = new System.Drawing.Point(12, 75);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(340, 258);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Selected VCUs";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 358);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(546, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Status: ";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 31);
            this.label2.TabIndex = 12;
            this.label2.Text = "Communication Timeout";
            // 
            // numUpDownFTPTimeout
            // 
            this.numUpDownFTPTimeout.Location = new System.Drawing.Point(15, 84);
            this.numUpDownFTPTimeout.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numUpDownFTPTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownFTPTimeout.Name = "numUpDownFTPTimeout";
            this.numUpDownFTPTimeout.Size = new System.Drawing.Size(49, 20);
            this.numUpDownFTPTimeout.TabIndex = 13;
            this.numUpDownFTPTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDownFTPTimeout.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "seconds";
            // 
            // cBoxClearRTDMData
            // 
            this.cBoxClearRTDMData.AutoSize = true;
            this.cBoxClearRTDMData.Location = new System.Drawing.Point(5, 42);
            this.cBoxClearRTDMData.Margin = new System.Windows.Forms.Padding(2);
            this.cBoxClearRTDMData.Name = "cBoxClearRTDMData";
            this.cBoxClearRTDMData.Size = new System.Drawing.Size(152, 17);
            this.cBoxClearRTDMData.TabIndex = 15;
            this.cBoxClearRTDMData.Text = "Clear Data After Download";
            this.cBoxClearRTDMData.UseVisualStyleBackColor = true;
            this.cBoxClearRTDMData.CheckedChanged += new System.EventHandler(this.cBoxClearVCUFiles_CheckedChanged);
            this.cBoxClearRTDMData.MouseLeave += new System.EventHandler(this.cBoxClearRTDM_MouseLeave);
            this.cBoxClearRTDMData.MouseHover += new System.EventHandler(this.cBoxClearRTDM_MouseHover);
            // 
            // cBoxRTDMDownload
            // 
            this.cBoxRTDMDownload.AutoSize = true;
            this.cBoxRTDMDownload.Checked = true;
            this.cBoxRTDMDownload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBoxRTDMDownload.Location = new System.Drawing.Point(5, 19);
            this.cBoxRTDMDownload.Name = "cBoxRTDMDownload";
            this.cBoxRTDMDownload.Size = new System.Drawing.Size(100, 17);
            this.cBoxRTDMDownload.TabIndex = 16;
            this.cBoxRTDMDownload.Text = "Download Data";
            this.cBoxRTDMDownload.UseVisualStyleBackColor = true;
            this.cBoxRTDMDownload.CheckedChanged += new System.EventHandler(this.cBoxRTDMDownload_CheckedChanged);
            this.cBoxRTDMDownload.MouseLeave += new System.EventHandler(this.cBoxRTDMDownload_MouseLeave);
            this.cBoxRTDMDownload.MouseHover += new System.EventHandler(this.cBoxRTDMDownload_MouseHover);
            // 
            // cBoxIELFDownload
            // 
            this.cBoxIELFDownload.AutoSize = true;
            this.cBoxIELFDownload.Checked = true;
            this.cBoxIELFDownload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBoxIELFDownload.Location = new System.Drawing.Point(6, 19);
            this.cBoxIELFDownload.Name = "cBoxIELFDownload";
            this.cBoxIELFDownload.Size = new System.Drawing.Size(100, 17);
            this.cBoxIELFDownload.TabIndex = 17;
            this.cBoxIELFDownload.Text = "Download Data";
            this.cBoxIELFDownload.UseVisualStyleBackColor = true;
            this.cBoxIELFDownload.CheckedChanged += new System.EventHandler(this.cBoxDownloadIELF_CheckedChanged);
            this.cBoxIELFDownload.MouseLeave += new System.EventHandler(this.cBoxIELFDownload_MouseLeave);
            this.cBoxIELFDownload.MouseHover += new System.EventHandler(this.cBoxIELFDownload_MouseHover);
            // 
            // cBoxClearIELFData
            // 
            this.cBoxClearIELFData.AutoSize = true;
            this.cBoxClearIELFData.Location = new System.Drawing.Point(6, 41);
            this.cBoxClearIELFData.Margin = new System.Windows.Forms.Padding(2);
            this.cBoxClearIELFData.Name = "cBoxClearIELFData";
            this.cBoxClearIELFData.Size = new System.Drawing.Size(152, 17);
            this.cBoxClearIELFData.TabIndex = 18;
            this.cBoxClearIELFData.Text = "Clear Data After Download";
            this.cBoxClearIELFData.UseVisualStyleBackColor = true;
            this.cBoxClearIELFData.CheckedChanged += new System.EventHandler(this.cBoxClearIELFData_CheckedChanged);
            this.cBoxClearIELFData.MouseLeave += new System.EventHandler(this.cBoxClearIELFData_MouseLeave);
            this.cBoxClearIELFData.MouseHover += new System.EventHandler(this.cBoxClearIELFData_MouseHover);
            // 
            // groupBoxRTDM
            // 
            this.groupBoxRTDM.Controls.Add(this.cBoxClearRTDMData);
            this.groupBoxRTDM.Controls.Add(this.cBoxRTDMDownload);
            this.groupBoxRTDM.Enabled = false;
            this.groupBoxRTDM.Location = new System.Drawing.Point(370, 56);
            this.groupBoxRTDM.Name = "groupBoxRTDM";
            this.groupBoxRTDM.Size = new System.Drawing.Size(166, 68);
            this.groupBoxRTDM.TabIndex = 19;
            this.groupBoxRTDM.TabStop = false;
            this.groupBoxRTDM.Text = "RTDM";
            // 
            // groupBoxIELF
            // 
            this.groupBoxIELF.Controls.Add(this.cBoxIELFDownload);
            this.groupBoxIELF.Controls.Add(this.cBoxClearIELFData);
            this.groupBoxIELF.Enabled = false;
            this.groupBoxIELF.Location = new System.Drawing.Point(370, 130);
            this.groupBoxIELF.Name = "groupBoxIELF";
            this.groupBoxIELF.Size = new System.Drawing.Size(166, 64);
            this.groupBoxIELF.TabIndex = 20;
            this.groupBoxIELF.TabStop = false;
            this.groupBoxIELF.Text = "IELF";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonStartDownload);
            this.groupBox1.Controls.Add(this.buttonCancelDownload);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numUpDownFTPTimeout);
            this.groupBox1.Location = new System.Drawing.Point(370, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 133);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Download Commands";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 380);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxIELF);
            this.Controls.Add(this.groupBoxRTDM);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonSelectVCU);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bombardier RTDM/IELF Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownFTPTimeout)).EndInit();
            this.groupBoxRTDM.ResumeLayout(false);
            this.groupBoxRTDM.PerformLayout();
            this.groupBoxIELF.ResumeLayout(false);
            this.groupBoxIELF.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectVCU;
        private System.Windows.Forms.Button buttonStartDownload;
        private System.Windows.Forms.Button buttonCancelDownload;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fIleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUpDownFTPTimeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cBoxClearRTDMData;
        private System.Windows.Forms.CheckBox cBoxRTDMDownload;
        private System.Windows.Forms.CheckBox cBoxIELFDownload;
        private System.Windows.Forms.CheckBox cBoxClearIELFData;
        private System.Windows.Forms.GroupBox groupBoxRTDM;
        private System.Windows.Forms.GroupBox groupBoxIELF;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}


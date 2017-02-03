namespace FTPDownloadRTDM
{
    partial class HelpAboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpAboutForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBoxProductInfo = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBoxCopyright = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxProductInfo.SuspendLayout();
            this.groupBoxCopyright.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::FTPDownloadRTDM.Properties.Resources.BombardierLogo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 167);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBoxProductInfo
            // 
            this.groupBoxProductInfo.Controls.Add(this.label3);
            this.groupBoxProductInfo.Controls.Add(this.label2);
            this.groupBoxProductInfo.Controls.Add(this.label1);
            this.groupBoxProductInfo.Location = new System.Drawing.Point(12, 194);
            this.groupBoxProductInfo.Name = "groupBoxProductInfo";
            this.groupBoxProductInfo.Size = new System.Drawing.Size(291, 94);
            this.groupBoxProductInfo.TabIndex = 3;
            this.groupBoxProductInfo.TabStop = false;
            this.groupBoxProductInfo.Text = "Product Information";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Operating System: Win7/Win XP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Build No: 1.3.0.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP Download RTDM";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(309, 345);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBoxCopyright
            // 
            this.groupBoxCopyright.Controls.Add(this.label5);
            this.groupBoxCopyright.Controls.Add(this.label4);
            this.groupBoxCopyright.Location = new System.Drawing.Point(12, 294);
            this.groupBoxCopyright.Name = "groupBoxCopyright";
            this.groupBoxCopyright.Size = new System.Drawing.Size(291, 74);
            this.groupBoxCopyright.TabIndex = 4;
            this.groupBoxCopyright.TabStop = false;
            this.groupBoxCopyright.Text = "Copyright";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "All rights reserved";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(273, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "(C) 2016 Bombardier Transportation  (Holdings) USA Inc.";
            // 
            // HelpAboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 373);
            this.Controls.Add(this.groupBoxCopyright);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBoxProductInfo);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpAboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About the FTP Download RTDM ";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxProductInfo.ResumeLayout(false);
            this.groupBoxProductInfo.PerformLayout();
            this.groupBoxCopyright.ResumeLayout(false);
            this.groupBoxCopyright.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBoxProductInfo;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBoxCopyright;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;

    }
}
namespace FTPDownloadRTDM
{
    partial class HandleErrorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandleErrorForm));
            this.buttonContinueWithNext = new System.Windows.Forms.Button();
            this.buttonCancelAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelErrorMessage = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonContinueWithNext
            // 
            this.buttonContinueWithNext.Location = new System.Drawing.Point(40, 165);
            this.buttonContinueWithNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonContinueWithNext.Name = "buttonContinueWithNext";
            this.buttonContinueWithNext.Size = new System.Drawing.Size(125, 50);
            this.buttonContinueWithNext.TabIndex = 0;
            this.buttonContinueWithNext.Text = "Continue WIth Next URL";
            this.buttonContinueWithNext.UseVisualStyleBackColor = true;
            this.buttonContinueWithNext.Click += new System.EventHandler(this.buttonContinueWithNext_Click);
            // 
            // buttonCancelAll
            // 
            this.buttonCancelAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelAll.Location = new System.Drawing.Point(344, 165);
            this.buttonCancelAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCancelAll.Name = "buttonCancelAll";
            this.buttonCancelAll.Size = new System.Drawing.Size(125, 50);
            this.buttonCancelAll.TabIndex = 1;
            this.buttonCancelAll.Text = "Cancel All Remaining";
            this.buttonCancelAll.UseVisualStyleBackColor = true;
            this.buttonCancelAll.Click += new System.EventHandler(this.buttonCancelAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelErrorMessage);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(40, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(429, 129);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Error Message";
            // 
            // labelErrorMessage
            // 
            this.labelErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorMessage.Location = new System.Drawing.Point(23, 31);
            this.labelErrorMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelErrorMessage.Name = "labelErrorMessage";
            this.labelErrorMessage.Size = new System.Drawing.Size(381, 82);
            this.labelErrorMessage.TabIndex = 0;
            this.labelErrorMessage.Text = "Error Message";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(193, 165);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(125, 50);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Visible = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // HandleErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelAll;
            this.ClientSize = new System.Drawing.Size(509, 233);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancelAll);
            this.Controls.Add(this.buttonContinueWithNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HandleErrorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Communication Error Detected";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonContinueWithNext;
        private System.Windows.Forms.Button buttonCancelAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelErrorMessage;
        private System.Windows.Forms.Button buttonClose;
    }
}
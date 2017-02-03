#region --- Revision History ---
/*
 * 
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.  
 *  Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   PTU
 * 
 *  Project:    PTU Application
 * 
 *  File name:  FormSecurity.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  04/27/10    1.0     K.McDonald      First Release.
 * 
 *  09/29/10    1.1     K.McD           1. Added new Bombardier logo.
 *  
 *	05/18/16	1.2	    D.Smail         References
 *	                                    1.  PTE Changes - List 5-17-2016.xlsx Item 1.   Login Screen - Make the Bombardier logo smaller.
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 2.   Login Screen - Add 'Portable Test Application'.
 *                                      3.  PTE Changes - List 5-17-2016.xlsx Item 3.   Login Screen - Add OK and Cancel buttons.
 *                                      4.  PTE Changes - List 5-17-2016.xlsx Item 69.  Login Screen - Add 'Enter factory or engineering password.' above the Password:
 *                                          text entry box.
 *	
 *                                      Modifications
 *                                      1.	Reduced the size of the Bombardier logo.
 *	                                    2.  Modified the Form Title to 'Portable Test Application Login'.
 *	                                    3.  Added 'OK' and 'Cancel' button to supplement the 'Enter' and 'Escape' key.
 *	                                    4.  Added the .m_LegendEnterPassword Label.
 *	                                    5.  Set the TabIndex values of a number of controls to 0.
 *	                                    
 *  08/03/2016  1.3     K.McD           References
 *                                      1.  PTE Changes - List 5-17-2016.xlsx Item 2. Add "Portable Test Application" with the version of the PTU Software to the Login
 *                                          screen.
 *                                      
 *                                      2.  PTE Changes - List 5-17-2016.xlsx Item 69. Add 'Enter Factory or Engineering Password' above the 'Password' text entry box.
 *                                          This should be project specific i.e. for the CTA and Toronto projects the text should read 'Enter Factory, Engineering or
 *                                          Maintenance Password'.  
 *                                      
 *                                      Modifications
 *                                      1.  Set the Text property of the m_LegendEnterPassword Label to '<Legend>'.
 *                                      2.  No longer sets the this.Text property to '"Portable Test Application Login', this is now carried out in the constructor
 *                                          and the Application.ProductVersion string is appended to this message e.g. '"Portable Test Application
 *                                          (Build: 6.18.1.0) Login'.
 *
 */
#endregion --- Revision History ---

namespace Bombardier.PTU.Forms
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.m_LegendPassword = new System.Windows.Forms.Label();
            this.m_TextBoxPassword = new System.Windows.Forms.TextBox();
            this.m_PanelLogo = new System.Windows.Forms.Panel();
            this.m_PictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.m_BtnOK = new System.Windows.Forms.Button();
            this.m_BtnCancel = new System.Windows.Forms.Button();
            this.m_LegendEnterPassword = new System.Windows.Forms.Label();
            this.m_PanelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // m_LegendPassword
            // 
            this.m_LegendPassword.AutoSize = true;
            this.m_LegendPassword.BackColor = System.Drawing.Color.Transparent;
            this.m_LegendPassword.ForeColor = System.Drawing.SystemColors.WindowText;
            this.m_LegendPassword.Location = new System.Drawing.Point(11, 140);
            this.m_LegendPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_LegendPassword.Name = "m_LegendPassword";
            this.m_LegendPassword.Size = new System.Drawing.Size(56, 13);
            this.m_LegendPassword.TabIndex = 0;
            this.m_LegendPassword.Text = "Password:";
            // 
            // m_TextBoxPassword
            // 
            this.m_TextBoxPassword.BackColor = System.Drawing.SystemColors.Window;
            this.m_TextBoxPassword.Location = new System.Drawing.Point(88, 137);
            this.m_TextBoxPassword.Margin = new System.Windows.Forms.Padding(2);
            this.m_TextBoxPassword.Name = "m_TextBoxPassword";
            this.m_TextBoxPassword.Size = new System.Drawing.Size(233, 20);
            this.m_TextBoxPassword.TabIndex = 1;
            this.m_TextBoxPassword.UseSystemPasswordChar = true;
            this.m_TextBoxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_TxtPassword_KeyPress);
            // 
            // m_PanelLogo
            // 
            this.m_PanelLogo.BackColor = System.Drawing.SystemColors.Window;
            this.m_PanelLogo.Controls.Add(this.m_PictureBoxLogo);
            this.m_PanelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_PanelLogo.Location = new System.Drawing.Point(0, 0);
            this.m_PanelLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_PanelLogo.Name = "m_PanelLogo";
            this.m_PanelLogo.Size = new System.Drawing.Size(429, 104);
            this.m_PanelLogo.TabIndex = 0;
            // 
            // m_PictureBoxLogo
            // 
            this.m_PictureBoxLogo.Image = global::Bombardier.PTU.Properties.Resources.BombardierLogo;
            this.m_PictureBoxLogo.Location = new System.Drawing.Point(14, 3);
            this.m_PictureBoxLogo.Name = "m_PictureBoxLogo";
            this.m_PictureBoxLogo.Size = new System.Drawing.Size(245, 95);
            this.m_PictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_PictureBoxLogo.TabIndex = 0;
            this.m_PictureBoxLogo.TabStop = false;
            // 
            // m_BtnOK
            // 
            this.m_BtnOK.Location = new System.Drawing.Point(349, 128);
            this.m_BtnOK.Name = "m_BtnOK";
            this.m_BtnOK.Size = new System.Drawing.Size(73, 25);
            this.m_BtnOK.TabIndex = 2;
            this.m_BtnOK.Text = "OK";
            this.m_BtnOK.UseVisualStyleBackColor = true;
            this.m_BtnOK.Click += new System.EventHandler(this.m_BtnOK_Click);
            // 
            // m_BtnCancel
            // 
            this.m_BtnCancel.Location = new System.Drawing.Point(349, 159);
            this.m_BtnCancel.Name = "m_BtnCancel";
            this.m_BtnCancel.Size = new System.Drawing.Size(73, 25);
            this.m_BtnCancel.TabIndex = 0;
            this.m_BtnCancel.TabStop = false;
            this.m_BtnCancel.Text = "Cancel";
            this.m_BtnCancel.UseVisualStyleBackColor = true;
            this.m_BtnCancel.Click += new System.EventHandler(this.m_BtnCancel_Click);
            // 
            // m_LegendEnterPassword
            // 
            this.m_LegendEnterPassword.AutoSize = true;
            this.m_LegendEnterPassword.Location = new System.Drawing.Point(11, 115);
            this.m_LegendEnterPassword.Name = "m_LegendEnterPassword";
            this.m_LegendEnterPassword.Size = new System.Drawing.Size(55, 13);
            this.m_LegendEnterPassword.TabIndex = 0;
            this.m_LegendEnterPassword.Text = "<Legend>";
            // 
            // FormLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(429, 194);
            this.Controls.Add(this.m_LegendEnterPassword);
            this.Controls.Add(this.m_BtnCancel);
            this.Controls.Add(this.m_BtnOK);
            this.Controls.Add(this.m_TextBoxPassword);
            this.Controls.Add(this.m_LegendPassword);
            this.Controls.Add(this.m_PanelLogo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "FormLogin";
            this.Shown += new System.EventHandler(this.FormLogin_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormAccount_KeyPress);
            this.m_PanelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_LegendPassword;
        private System.Windows.Forms.TextBox m_TextBoxPassword;
        private System.Windows.Forms.Panel m_PanelLogo;
        private System.Windows.Forms.Button m_BtnOK;
        private System.Windows.Forms.Button m_BtnCancel;
        private System.Windows.Forms.Label m_LegendEnterPassword;
        private System.Windows.Forms.PictureBox m_PictureBoxLogo;
    }
}
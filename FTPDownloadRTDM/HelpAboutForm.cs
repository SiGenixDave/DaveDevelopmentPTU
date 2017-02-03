#region --- Revision History ---

/*
 *
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.
 *  Offenders will be held liable for the payment of damages.
 *
 *  (C) 2016    Bombardier Inc. or its subsidiaries. All rights reserved.
 *
 *  Solution:   RTDM FTP Download
 *
 *  Project:    FTPDownloadRTDM
 *
 *  File name:  HelpAboutForm.cs
 *
 *  Revision History
 *  ----------------
 *
 *  Date        Version Author      Comments
 *  06/30/2016  1.0     D.Smail     First Release.
 *
 *
 */

#endregion --- Revision History ---

using System;
using System.Windows.Forms;

namespace FTPDownloadRTDM
{
    /// <summary>
    /// Help... About form that displays software version and copyright information
    /// </summary>
    public partial class HelpAboutForm : Form
    {
        #region --- Constructors ---

        /// <summary>
        /// Default windows form constructor
        /// </summary>
        public HelpAboutForm()
        {
            InitializeComponent();
        }

        #endregion --- Constructors ---

        #region --- Methods ---

        /// <summary>
        /// Clicking button closes the About Form
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button click event info</param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion --- Methods ---
    }
}
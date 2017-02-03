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
 *  File name:  MainFormMouseHelp.cs
 *
 *  Revision History
 *  ----------------
 *
 *  Date        Version Author      Comments
 *  12/13/2016  1.0     D.Smail     First Release.
 *
 *
 */

#endregion --- Revision History ---

using System.Windows.Forms;
using System;

namespace FTPDownloadRTDM
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Handles the mouse hover event for the Select VCU button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonSelectVCU_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Instantiates another Windows form that detects the presence of VCUs that have an " + System.Environment.NewLine +
                           "active FTP server and allows the user to select which VCU(s) to download RTDM/IELF data files", buttonSelectVCU, 10, -50);
        }

        /// <summary>
        /// Handles the mouse hover event for the Start Download button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonStartDownload_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Starts or restarts the FTP download of all VCU(s) listed in the above window.", (Button)sender, 10, -20);
        }

        /// <summary>
        /// Handles the mouse hover event for the Cancel Download button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonCancelDownload_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Cancels the FTP download that is in progress.", (Button)sender, 10, -20);
        }

        /// <summary>
        /// Handles the mouse leave event for the Cancel Download button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonCancelDownload_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((Button)sender);
        }

        /// <summary>
        /// Handles the mouse leave event for the Start Download button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonStartDownload_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((Button)sender);
        }

        /// <summary>
        /// Handles the mouse leave event for the Select VCU button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonSelectVCU_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((Button)sender);
        }

        /// <summary>
        /// Displays a help message when the mouse hovers over the control.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxRTDMDownload_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("If selected, all files required to build a viewable DAN file will be downloaded from " + System.Environment.NewLine +
                           "selected VCU", cBoxRTDMDownload, 10, -50);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxRTDMDownload_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((CheckBox)sender);
        }

        /// <summary>
        /// Displays a help message when the mouse hovers over the control.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxIELFDownload_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("If selected, all files required to create the IELF file and viewable CSV file will be downloaded from " + System.Environment.NewLine +
                           "selected VCU", cBoxIELFDownload, 10, -50);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxIELFDownload_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((CheckBox)sender);
        }

        /// <summary>
        /// Displays a help message when the mouse hovers over the control.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxClearRTDM_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("If selected, after successfully downloading all data from a VCU, the RTDM  " + System.Environment.NewLine +
                           "data files will be cleared on the VCU (VCUs must be in RUN mode in order for data " + System.Environment.NewLine +
                           "clearing to complete)", cBoxClearRTDMData, 10, -50);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxClearRTDM_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((CheckBox)sender);
        }

        /// <summary>
        /// Displays a help message when the mouse hovers over the control.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxClearIELFData_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("If selected, after successfully downloading all data from a VCU, the IELF  " + System.Environment.NewLine +
                           "data files will be cleared on the VCU (VCUs must be in RUN mode in order for data " + System.Environment.NewLine +
                           "clearing to complete)", cBoxClearRTDMData, 10, -50);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxClearIELFData_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((CheckBox)sender);
        }


    }
}
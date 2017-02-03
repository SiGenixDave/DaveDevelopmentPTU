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
 *  File name:  SelectVCUForm.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace FTPDownloadRTDM
{
    public partial class SelectVCUForm : Form
    {
        #region --- Member Variables ---

        /// <summary>
        /// A list of all of the VCUs to search for... populated from the BART PTU list at the
        /// bottom of BART.Configuation.xml
        /// </summary>
        private List<string> m_VcuToScanUriList;

        /// <summary>
        /// Set to true when the scan has stopped
        /// </summary>
        private Boolean m_ScanStopped = false;

        /// <summary>
        /// These are the user selected list of VCUs from which an RTDM file download (via FTP) will be attempted
        /// </summary>
        public List<string> VcuURI
        {
            get;
            private set;
        }

        #endregion --- Member Variables ---

        #region --- Constructors ---

        /// <summary>
        /// Default windows form constructor; set to private to ensure that the
        /// constructor below is used
        /// </summary>
        private SelectVCUForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Only constructor allowed used to instantiate this form
        /// </summary>
        /// <param name="uriList">VCU URI list retrieved from the BART.Configuration.xml file</param>
        public SelectVCUForm(List<string> uriList)
            : this()
        {
            // populate the list of VCUs to scan
            m_VcuToScanUriList = uriList;

            // immediately begin the search for connected VCUs
            backgroundWorker1.RunWorkerAsync();
        }

        #endregion --- Constructors ---

        #region --- Methods ---

        /// <summary>
        /// Required to disable the "x" at the top right corner but yet display the icon in the top left
        /// when using a ShowDialog
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_NOCLOSE = 0x200;
                CreateParams mdiCp = base.CreateParams;
                mdiCp.ClassStyle = mdiCp.ClassStyle | CS_NOCLOSE;
                return mdiCp;
            }
        }

        /// <summary>
        /// Populates all of the selected items in the list box into the VCU URI
        /// list that will be used by the parent form.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse click event info</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Inform the background worker to cease
            m_ScanStopped = true;

            // Populate the URI list from the user selection
            VcuURI = new List<string>();
            foreach (string s in listBox1.SelectedItems)
            {
                VcuURI.Add(s);
            }

            // Cancel the background worker and close the form
            DialogResult = DialogResult.OK;
            backgroundWorker1.CancelAsync();
            Close();
        }

        /// <summary>
        /// When the user selects "Cancel", the VCU URI list is not populated with anything, no matter
        /// if the list box has items selected or not
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse click event info</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Cancel the background worker and close the form
            m_ScanStopped = true;
            DialogResult = DialogResult.Cancel;
            backgroundWorker1.CancelAsync();
            Close();
        }

        /// <summary>
        /// Stops the current scan of VCUs
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse click event info</param>
        private void buttonStopScan_Click(object sender, EventArgs e)
        {
            m_ScanStopped = true;
            backgroundWorker1.CancelAsync();
            buttonStopScan.Enabled = false;
            toolStripStatusLabel2.Text = "VCU scanning has been stopped";
            buttonRestartScan.Enabled = true;
            listBox1.Enabled = true;
        }

        /// <summary>
        /// Asynchronous task that scans the VCUs to determine if their FTP server
        /// is executing. This asynchronous task allows the user to interface to the UI
        /// while the code is determining if the VCUs are connected.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">background worker event info</param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Set the tool strip progress bar max count
            toolStripProgressBar1.Maximum = this.m_VcuToScanUriList.Count;

            int i;
            for (i = 0; i < m_VcuToScanUriList.Count; i++)
            {
#if !LOCALHOST
                string uri = m_VcuToScanUriList[i];
#else
                string uri = "localhost";
#endif
                Thread.Sleep(250);

                // Update the progress bar with current scan count and the URI
                backgroundWorker1.ReportProgress(i + 1, uri);

                string ftpUri = @"ftp://" + uri;
                // Test the FTP connection and add the current URL to the available VCU list
                if (TestFTPConnection(ftpUri))
                {
                    AddAvailableVCU(uri);
                }

                // Kill the loop if an asynchronous cancel was invoked
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Invoked whenever the background worker ReportProgress is called. Allows the background
        /// worked to update the UI
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">progress changed event info</param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!m_ScanStopped)
            {
                toolStripProgressBar1.Value = e.ProgressPercentage;
                // UserState is an object inherent in C#, so typecast back to a string
                toolStripStatusLabel2.Text = e.UserState as string;
            }
        }

        /// <summary>
        /// This function determines if an FTP connection is available at the specified host.
        /// Note: It doesn't download any files but just checks if the FTP connection
        /// is available by determining if a directory listing is available.
        /// NOTE: The directory listing is not used for anything except a connection check.
        /// </summary>
        /// <param name="ftpHost">The URL of the VCU</param>
        /// <returns>true if the FTP connection is available; false otherwise</returns>
        private Boolean TestFTPConnection(string ftpHost)
        {
            Boolean ftpConnectionState;
            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(ftpHost);
            try
            {
                requestDir.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                // Allow 5000 msecs for an FTP response. This timeout is valid when a connection
                // suddenly goes down. Typically, if the connection never existed, the code immediately
                // falls thru to catch()
                requestDir.Timeout = 5000;
                requestDir.GetResponse();
                ftpConnectionState = true;
            }
            catch
            {
                ftpConnectionState = false;
            }
            finally
            {
                requestDir.Abort();
            }
            return ftpConnectionState;
        }

        /// <summary>
        /// Required to update the list box from the background worker thread (See InvokeRequired)
        /// </summary>
        /// <param name="aVCU"></param>
        private delegate void AddAvailableVCUCallback(string aVCU);

        /// <summary>
        /// Required to update the list box from the background worker thread
        /// </summary>
        /// <param name="aVCU"></param>
        private void AddAvailableVCU(string aVCU)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listBox1.InvokeRequired)
            {
                AddAvailableVCUCallback l = new AddAvailableVCUCallback(AddAvailableVCU);
                listBox1.Invoke(l, new object[] { aVCU });
            }
            else
            {
                listBox1.Items.Add(aVCU);
            }
        }

        /// <summary>
        /// Invoked automatically when the background worker has completed the VCU scan
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">background worker complete event info</param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Inform the user that no VCUs were detected
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("No VCUs were detected during scan. Are you sure that you are connected to vehicle network and the systems are powered on?",
                                "",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            else if (listBox1.Items.Count > 0)
            {
                // Enable and make visible the Select All VCUs button
                buttonSelectAllVCUs.Enabled = true;
                buttonSelectAllVCUs.Visible = true;
            }

            // The user hasn't stopped the scan (scan completed on its own)
            if (!m_ScanStopped)
            {
                toolStripStatusLabel2.Text = "VCU scanning is complete";
            }

            // Disable the Stop Scan and enable the Restart Scan button
            m_ScanStopped = true;
            buttonStopScan.Enabled = false;
            buttonRestartScan.Enabled = true;
            listBox1.Enabled = true;
        }

        /// <summary>
        /// Selects all of the VCUs currently listed in the list box and closes
        /// the form.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button click event info</param>
        private void buttonSelectAllVCUs_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }

            // Simulate the OK button click which closes the form
            buttonOK_Click(null, null);
        }

        /// <summary>
        /// Invoked when an item in the list box is clicked. If at least 1 item
        /// is selected, the OK button will be enabled.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">list box changed event info</param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                buttonOK.Enabled = false;
            }
            else
            {
                buttonOK.Enabled = true;
            }
        }

        /// <summary>
        /// Enables the hover text on the list box
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void listBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Select 1 or more VCUs. Ctrl + Left Mouse click to select additional VCUs", listBox1, 10, -40);
        }

        /// <summary>
        /// Enables the hover text on the Select All VCUs button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonSelectAllVCUs_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Selects all available VCUs and exits this window.", buttonSelectAllVCUs, 10, -20);
        }

        /// <summary>
        /// Enables the hover text on the Stop Scan button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse hover event info</param>
        private void buttonStopScan_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Stops scanning for VCU FTP connections.", buttonStopScan, 10, -20);
        }

        /// <summary>
        /// Disables the hover text on the Select All VCUs button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse leave event info</param>
        private void buttonSelectAllVCUs_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((Button)sender);
        }

        /// <summary>
        /// Disables the hover text on the list box
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse leave event info</param>
        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((ListBox)sender);
        }

        /// <summary>
        /// Disables the hover text on the Stop Scan button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">mouse leave event info</param>
        private void buttonStopScan_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide((Button)sender);
        }

        /// <summary>
        /// Starts or restarts the search for VCUs that have their FTP server available.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button click event info</param>
        private void buttonRestartScan_Click(object sender, EventArgs e)
        {
            // Clear all of the VCU URIs from the list box
            listBox1.Items.Clear();

            listBox1.Enabled = false;

            // wait for the background to be free
            m_ScanStopped = false;
            while (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(100);
            }
            // Start the background worker thread and set the buttons to the appropriate "Enabled" state
            backgroundWorker1.RunWorkerAsync();
            buttonRestartScan.Enabled = false;
            buttonStopScan.Enabled = true;
            buttonSelectAllVCUs.Enabled = false;
        }

        #endregion --- Methods ---
    }
}
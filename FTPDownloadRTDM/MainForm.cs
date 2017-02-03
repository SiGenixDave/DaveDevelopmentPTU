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
 *  File name:  MainForm.cs
 *
 *  Revision History
 *  ----------------
 *
 *  Date        Version Author      Comments
 *  06/30/2016  1.0     D.Smail     First Release.
 *  12/20/2016  1.1     D.Smail     - Fix issue with download % greater than 100%
 *                                  - To reflect VCU, there are now 2 download directories
 *                                    which consist of "rtdm" and "ielf". Prior to this change, there
 *                                    was only the "rtdmielf" directory
 *
 *
 */

#endregion --- Revision History ---

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace FTPDownloadRTDM
{
    public partial class MainForm : Form
    {
        #region --- Enumerations ---

        /// <summary>
        /// Used for state machine control when downloading files from the VCU
        /// </summary>
        private enum FTPDownloadState
        {
            /// <summary>
            /// No download in progress or download canceled
            /// </summary>
            IDLE,

            /// <summary>
            /// FTP download is in progress
            /// </summary>
            IN_PROGRESS,

            /// <summary>
            /// Initiates a new FTP download from a new URL
            /// </summary>
            INITIATE_DOWNLOAD,

            /// <summary>
            /// FTP download is in progress
            /// </summary>
            HANDLE_ERROR,

            /// <summary>
            /// Indicates that a download is complete
            /// </summary>
            DOWNLOAD_COMPLETE,

            /// <summary>
            /// Checks for more VCUs to download from; will go here if an error occurred or a download was canceled by the user
            /// </summary>
            CHECK_FOR_MORE,
        }

        /// <summary>
        /// Used to aid in updating the datagrid with the appropriate comment... either "Canceled" or "Error"
        /// </summary>
        private enum FTPStopped
        {
            /// <summary>
            /// Indicates the user stopped the transfer (via the Cancel button)
            /// </summary>
            BY_USER,

            /// <summary>
            /// Indicates the FTP transfer was halted because of an error
            /// </summary>
            BY_ERROR,
        }

        #endregion --- Enumerations ---

        #region --- Member Variables ---

        /// <summary>
        /// Standard 4 character project id passed in on the command line.
        /// </summary>
        private string m_ProjectId = null;

        /// <summary>
        /// Becomes true when an entire file has been downloaded from the VCU or the file
        /// download has been canceled.
        /// </summary>
        private bool m_FTPDownloadEnded;

        /// <summary>
        /// The size of the file that is currently being downloaded. Used to calculate and
        /// display the percentage downloaded.
        /// </summary>
        private Double m_NumBytesToDownload;

        /// <summary>
        /// Used as index into the data grid to display download status.
        /// </summary>
        private int m_VCUIndex;

        /// <summary>
        /// A copy of all of the selected VCU URIs from the SelectedVCUForm.
        /// </summary>
        private List<string> m_VCUList;

        /// <summary>
        /// FTP client object that handles all of the details of the FTP download.
        /// </summary>
        private WebClient m_WebClient;

        /// <summary>
        /// Maintains the state of the FTP download. See FTPDownloadState enumeration for details.
        /// </summary>
        private FTPDownloadState m_FTPDownloadState;

        /// <summary>
        /// Maintains the amount of time (msecs) that have elapsed since the last progress update event
        /// was fired during the file download
        /// </summary>
        private uint m_FTPTimeoutCount;

        /// <summary>
        /// Maintains the amount of time (msecs) allowed before a communication timeout occurs
        /// </summary>
        private int m_FTPTimeout;

        /// <summary>
        /// List of files to be downloaded from target VCU. This list is dynamically populated just prior to downloading
        /// from a target VCU.
        /// </summary>
        private List<string> m_FilesToDownload = new List<string>();

        /// <summary>
        /// Increments after every file is downloaded from a given VCU. Used for informational purposes only.
        /// </summary>
        private int m_FileCount;

        /// <summary>
        /// Used for informational purposes only and set prior to download. The number of files to be downloaded from a target VCU
        /// </summary>
        private int m_FilesToDownloadCount;

        /// <summary>
        /// Flag set to true to indicate to the state machine to begin downloading
        /// </summary>
        private Boolean m_InitDownload;

        /// <summary>
        /// Flag set to true to indicate that an error was detected during the download
        /// </summary>
        private Boolean m_HandleDownloadError;

        // NOTE: LOCALHOST is only used for unit testing on the PC. It is defined for the "Debug" configuration
        // and not the "Release" configuration 
#if LOCALHOST

        /// <summary>
        /// Drive/Directory on host (PC) where RTDM data is stored
        /// </summary>
        private const String RTDM_DRIVE_DIR = "rtdm";

        /// <summary>
        /// Drive/Directory on host (PC) where IELF data is stored
        /// </summary>
        private const String IELF_DRIVE_DIR = "ielf";

#else

        /// <summary>
        /// Drive/Directory on host (VCU) where RTDM data is stored
        /// </summary>
        private const String RTDM_DRIVE_DIR = "/usb0/rtdm";

        /// <summary>
        /// Drive/Directory on host (VCU) where IELF data is stored
        /// </summary>
        private const String IELF_DRIVE_DIR = "/usb0/ielf";

#endif

        #endregion --- Member Variables ---

        #region --- Constructors ---

        /// <summary>
        /// Default initializer for the Form class (no command line arguments)
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            InitializeDataGrid();

            toolStripStatusLabel2.Text = "Awaiting VCU Selection";

            m_FTPTimeout = (int)numUpDownFTPTimeout.Value * 1000;
        }

        /// <summary>
        /// Constructor for the main form that accepts a command line argument. It
        /// displays the project ID between () in the form title bar. It also invokes
        /// the default form constructor
        /// </summary>
        /// <param name="args">arg[0] is the project id</param>
        public MainForm(string[] args)
            : this()
        {
            m_ProjectId = args[0];
            this.Text = this.Text + " (" + this.m_ProjectId + ")";
        }

        #endregion --- Constructors ---

        #region --- Methods ---

        /// <summary>
        /// Sets the column count and initializes the column m_Header info
        /// </summary>
        private void InitializeDataGrid()
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "VCU";
            dataGridView1.Columns[1].Name = "Download Status";
            dataGridView1.Columns[2].Name = "Progress";
        }

        /// <summary>
        /// Deletes all rows from the data grid and refreshes the form
        /// </summary>
        private void DeleteAllRows()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        /// <summary>
        /// Populates the datagrid with the user list of VCU URLs
        /// </summary>
        private void AddRows()
        {
            foreach (string vcuUrl in m_VCUList)
            {
                ArrayList row = new ArrayList();
                row.Add(vcuUrl);
                row.Add("Not Started");
                row.Add("0 %");
                dataGridView1.Rows.Add(row.ToArray());
            }
        }

        /// <summary>
        /// Invoked by the O.S. when either the entire file has been downloaded, an error has occurred or
        /// the user has canceled the download.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">asynchronous event info</param>
        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Handle error conditions
            // In come instances, the error event is fired twice (e.g. when a connection is down), so the
            // m_FTPDownloadEnded is used to prevent more than 1 message box from popping up per
            // an FTP download attempt on a URL
            if ((e.Error != null) && (!e.Cancelled) && (!m_FTPDownloadEnded))
            {
                InvokeErrorMessageBox(e.Error.Message, FTPStopped.BY_ERROR);
            }
            else if (e.Cancelled)
            {
                dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Canceled";
                dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.OrangeRed;
            }
            else
            {
                dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Successful";
                dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
            }
            // Use this flag as an indication that the download is complete, an error has occurred or the user canceled
            // the download
            m_FTPDownloadEnded = true;
        }

        /// <summary>
        /// Invoked when the download progress has changed. O.S. dependent but is used to update the % downloaded.
        /// This method allows updates of UI controls.,
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">asynchronous event info</param>
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesReceived = (double)e.BytesReceived;
            double totalBytesToReceive = (double)e.TotalBytesToReceive;

            double displayValue = bytesReceived / m_NumBytesToDownload * 100.0;
            // For some unknown reason, the "%" download is occasionally greater than 100, so when
            // it is, clamp it.
            if (displayValue > 100.0)
            {
                displayValue = 100.0;
            }

            // Update the percentage downloaded
            dataGridView1.Rows[m_VCUIndex].Cells[2].Value = displayValue.ToString("F2") + " %";

            // Since progress is happening, reset this timeout counter.
            m_FTPTimeoutCount = 0;
        }

        /// <summary>
        /// Opens the XML file where the VCU URLs are stored. Then parses the file to get the URLs and then
        /// opens up a new form so the user can select from those VCUs that respond to an FTP
        /// inquiry. After the form is closed, the datagrid is populated with the user selection.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button click info</param>
        private void buttonSelectVCU_Click(object sender, EventArgs e)
        {
            // Open the XmlDocument.
            XmlDocument doc = new XmlDocument();

            string[] filepath =
            {
                // New PTU
                @"C:\Users\Public\Documents\Bombardier\Portable Test Application\Configuration Files\",
                // Old PTU (file path to support backward compatibility)
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                @"\Bombardier\Portable Test Application\Configuration Files\"
            };

            // Create a fully qualified filename
            string filename = m_ProjectId + ".Configuration.xml";
            bool failedToOpenConfigFile = true;
            List<string> uriList = new List<string>();

            for (uint i = 0; i < filepath.Length; i++)
            {
                try
                {
                    doc.Load(filepath[i] + filename);
                }
                catch (Exception)
                {
                    continue;
                }

                failedToOpenConfigFile = false;
                // Get all of the URIs from the XML file and populate uriList
                XmlNodeList elemList = doc.GetElementsByTagName("URI");
                for (int el = 1; el < elemList.Count; el += 2)
                {
                    uriList.Add(elemList[el].InnerXml);
                }

                break;
            }

            // Inform user that a valid XML configuration file wasn't found
            if (failedToOpenConfigFile)
            {
                MessageBox.Show("Could not find Configuration.XML file that lists valid URIs");
                return;
            }

            // No valid URIs found in configuration file
            if (uriList.Count == 0)
            {
                MessageBox.Show("Could not find any valid URIs in the XML configuration file");
                return;
            }

            // The "using" construct below keeps the form "alive" while all of the selected
            // URLs are retrieved
            using (SelectVCUForm selectVCUForm = new SelectVCUForm(uriList))
            {
                DialogResult result = selectVCUForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    toolStripStatusLabel2.Text = "Click \"Start FTP Download\" button to download RTDM and/or IELF Files";
                    groupBoxIELF.Enabled = true;
                    groupBoxRTDM.Enabled = true;
                    DeleteAllRows();
                    // values preserved after close because of "using"
                    m_VCUList = selectVCUForm.VcuURI;
                    AddRows();
                    buttonStartDownload.Enabled = true;
                    cBoxClearRTDMData.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Invoked when Start Download button clicked
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button click info</param>
        private void buttonStartDownload_Click(object sender, EventArgs e)
        {
            // Perform a refresh of the data grid to remove any past download information
            DeleteAllRows();
            AddRows();

            // Place the buttons in the appropriate state
            buttonStartDownload.Enabled = false;
            buttonCancelDownload.Enabled = true;
            buttonSelectVCU.Enabled = false;

            // Disable any changes to what files are to be downloaded/uploaded
            groupBoxRTDM.Enabled = false;
            groupBoxIELF.Enabled = false;

            m_VCUIndex = 0;
            m_InitDownload = true;

            QueueUpFileDownloadList();
            toolStripStatusLabel2.Text = "File " + m_FileCount.ToString() + " of " +
                                         m_FilesToDownloadCount.ToString() + " FTP download in progress from the selected VCU.";
        }

        /// <summary>
        /// Invoked when Cancel Download button clicked
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button click info</param>
        private void buttonCancelDownload_Click(object sender, EventArgs e)
        {
            // Cancel the current ongoing FTP download
            m_WebClient.CancelAsync();
            InvokeErrorMessageBox("FTP Download canceled by user", FTPStopped.BY_USER);
        }

        /// <summary>
        /// Invoked when the user clicks File.. Exit
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">tool strip click info</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Kill the app including any background or asynchronous tasks
            Application.Exit();
        }

        /// <summary>
        /// Needed to remove cell highlighting, used for clean UI only
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">data grid selection changed info</param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // remove cell highlighting
            dataGridView1.ClearSelection();
        }

        /// <summary>
        /// Places in the file list all necessary files that need to be downloaded from a target VCU
        /// </summary>
        private void QueueUpFileDownloadList()
        {
            // Both of these are required to create the viewable DAN file (built in DLL)
            string infoFileName = RTDM_DRIVE_DIR + "/CarConDev.dat";
            string xmlConfigFileName = RTDM_DRIVE_DIR + "/RTDMConfiguration_PCU.xml";
            string ielfFileName = IELF_DRIVE_DIR + "/ielf.flt";

            string uri = dataGridView1.Rows[m_VCUIndex].Cells[0].Value.ToString();

            // Clear the files to download list
            m_FilesToDownload.Clear();

            // Add files required for RTDM download
            if (cBoxRTDMDownload.Checked)
            {
                // Add the XML configuration file
                m_FilesToDownload.Add(xmlConfigFileName);
                // Get all of the #.stream files
                List<string> danFiles = GetDirectoryList(uri, RTDM_DRIVE_DIR);
                foreach (String file in danFiles)
                {
                    m_FilesToDownload.Add(file);
                }
            }

            // Add files required for IELF download
            if (cBoxIELFDownload.Checked)
            {
                m_FilesToDownload.Add(ielfFileName);
            }

            // Required to get car, device and consist (needed to create the filename)
            m_FilesToDownload.Add(infoFileName);

            // Total number of files to download (info only on status bar)
            m_FilesToDownloadCount = m_FilesToDownload.Count;

            // File download tracking count (info only on status bar)
            m_FileCount = 1;
        }

        /// <summary>
        /// Responsible for setting up the web client to begin downloading a file from a given host as specified
        /// in the datagrid. It also specifies the where the downloaded file will be saved.
        /// </summary>
        /// <param name="fileName"></param>
        private void FtpDownloadFile(String fileName)
        {
            // Create the new WebClient and add the event handlers
            m_WebClient = new WebClient();
            m_WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            m_WebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            // Create the full URL including file name
            string ftphost = @"ftp://" + dataGridView1.Rows[m_VCUIndex].Cells[0].Value.ToString();
            string ftpfullpath = ftphost + "/" + fileName;

            // Used when the amount of VCUs selected creates a scroll bar
            dataGridView1.Rows[m_VCUIndex].Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = m_VCUIndex;

            // reset all state parameters
            m_FTPTimeoutCount = 0;
            m_FTPDownloadEnded = false;
            m_FTPDownloadState = FTPDownloadState.IN_PROGRESS;

            // Change the cell to yellow indicating a download is in progress
            dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "In Progress...";

            // Create the file name that will be stored
            string fileSavePath = @"C:\Ptu\Pcu\Data\Temp\";

            // Verify the directory exists; it should as long as the PTU was installed on this machine
            if (!Directory.Exists(fileSavePath))
            {
                Directory.CreateDirectory(fileSavePath);
            }

            // Strip directory from fileName... look for the last '/' in the path/filename
            String fileNameWithoutRemoteDir = fileName.Substring(fileName.LastIndexOf('/') + 1);

            // This is the path and name of the file that is downloaded and saved
            string fullyQualifiedFileName = fileSavePath + fileNameWithoutRemoteDir;

            // Start the download
            m_WebClient.DownloadFileAsync(new Uri(ftpfullpath), fullyQualifiedFileName);
        }

        /// <summary>
        /// This is used to get the file size prior to downloading. Due to the nature of FTP, some of the
        /// info that comes back with the progress changed event is not accurate
        /// </summary>
        /// <param name="host">FTP host</param>
        /// <param name="dir">directory where file resides on FTP host</param>
        /// <param name="filename">filename</param>
        /// <returns></returns>
        private long GetFileSize(string host, string filename)
        {
            string ftpPath = "ftp://" + host + "/" + filename;
            var req = (FtpWebRequest)WebRequest.Create(ftpPath);
            req.Proxy = null;
            req.Credentials = new NetworkCredential("anonymous", "");

            req.Method = WebRequestMethods.Ftp.GetFileSize;

            long fileSize = 0;
            // Required in case file doesn't exist on FTP host
            try
            {
                using (WebResponse resp = req.GetResponse())
                    fileSize = resp.ContentLength;
            }
            catch (Exception e)
            {
                // Let download catch it InvokeErrorMessageBox("Expected file " + filename + " not found on FTP host", FTPStopped.BY_ERROR);
            }
            return fileSize;
        }

        /// <summary>
        /// Used to get the directory listing of the FTP host and populate the list of files
        /// (#.stream) that need to be downloaded.
        /// </summary>
        /// <param name="host">FTP host</param>
        /// <param name="dir">directory where file resides on FTP host</param>
        /// <returns></returns>
        private List<string> GetDirectoryList(string host, string dir)
        {
            List<string> streamList = new List<string>();

            string ftphost = "ftp://" + host + "/" + dir;
            var req = (FtpWebRequest)WebRequest.Create(ftphost);
            req.Proxy = null;
#if LOCALHOST
            req.Credentials = new NetworkCredential("anonymous", "");
#endif
            req.Method = WebRequestMethods.Ftp.ListDirectory;

            try
            {
                using (WebResponse resp = req.GetResponse())
                using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                {
                    string list = reader.ReadToEnd();
                    string[] ftpListDir = Regex.Split(list, "\r\n");

                    // Search for #.stream file and populate list if found
                    for (uint index = 0; index < 1024; index++)
                    {
#if LOCALHOST
                        String searchForStream = dir + "/" + index.ToString() + ".stream";
#else
                        String searchForStream = index.ToString() + ".stream";
#endif
                        foreach (string file in ftpListDir)
                        {
                            if (file.Equals(searchForStream))
                            {
#if LOCALHOST
                                streamList.Add(searchForStream);
#else
                                streamList.Add(dir + "/" + searchForStream);
#endif
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Let download catch it InvokeErrorMessageBox("Expected file " + filename + " not found on FTP host", FTPStopped.BY_ERROR);
            }

            return streamList;
        }

        /// <summary>
        /// Invoked when timer 1 expires. Used to process the FTP Download state machine.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">timer changed info</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessFTPDownloads();
        }

        // IMPORTANT: CallingConvention is ABSOLUTELY required to avoid PInvoke errors. C# uses __stdcall
        // as its interface and the default calling convention for DLL is __cdecl
        // TODO BuildViewableDanFile() should be converted to C#
        [DllImport("BuildDanFileLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 BuildViewableDanFile(StringBuilder fileName, StringBuilder errorString);

        // TODO BuildDanFileName() should be converted to C#
        [DllImport("BuildDanFileLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 BuildDanFileName(StringBuilder fileName, StringBuilder errorString);

        /// <summary>
        /// TODO - handling this the old fashioned way and needs to be changed to
        /// an event driven state machine.
        /// </summary>
        private void ProcessFTPDownloads()
        {
            // Handle the case where there is a download error or user cancels the current download
            if (m_HandleDownloadError)
            {
                m_FTPDownloadState = FTPDownloadState.HANDLE_ERROR;
                m_HandleDownloadError = false;
            }

            switch (m_FTPDownloadState)
            {
                case FTPDownloadState.IDLE:
                case FTPDownloadState.HANDLE_ERROR:
                default:
                    // wait here for the message box to be closed
                    if (m_InitDownload)
                    {
                        m_FTPDownloadState = FTPDownloadState.INITIATE_DOWNLOAD;
                        m_InitDownload = false;
                    }
                    break;

                case FTPDownloadState.INITIATE_DOWNLOAD:
                    if (m_FilesToDownload.Count > 0)
                    {
                        string uri = dataGridView1.Rows[m_VCUIndex].Cells[0].Value.ToString();
                        m_NumBytesToDownload = GetFileSize(uri, m_FilesToDownload[0]);
                        FtpDownloadFile(m_FilesToDownload[0]);
                        // Remove the file from the queue
                        m_FilesToDownload.RemoveAt(0);
                        m_FTPDownloadState = FTPDownloadState.IN_PROGRESS;
                    }

                    break;

                case FTPDownloadState.IN_PROGRESS:
                    MonitorDownloadProgress();
                    if ((m_FTPDownloadEnded) && (m_FilesToDownload.Count > 0))
                    {
                        m_FTPDownloadState = FTPDownloadState.INITIATE_DOWNLOAD;
                        m_FileCount++;
                        toolStripStatusLabel2.Text = "\"" + m_FilesToDownload[0] + "\"" + " being downloaded from the selected VCU: " +
                                                      m_FileCount.ToString() + " of " +
                                                      m_FilesToDownloadCount.ToString();
                    }
                    else if (m_FTPDownloadEnded)
                    {
                        m_FTPDownloadState = FTPDownloadState.DOWNLOAD_COMPLETE;
                    }
                    break;

                case FTPDownloadState.DOWNLOAD_COMPLETE:
                    StringBuilder fileName = new StringBuilder(256);
                    StringBuilder errorString = new StringBuilder(256);
                    String toolStripText = "Downloading Complete... ";

                    // Compile all of the RTDM data
                    if (cBoxRTDMDownload.Checked)
                    {
                        Int32 errorCode = BuildViewableDanFile(fileName, errorString);
                        if (errorCode == 0)
                        {
                            toolStripText += "RTDM OK... ";
                            // Move the file to 1 directory above
                            String origFileNameRtdm = fileName.ToString();
                            String newFileNameRtdm = origFileNameRtdm.Replace("Temp\\", "");

                            File.Move(origFileNameRtdm, newFileNameRtdm);
                            // Clear all RTDM data if checked (upload clear file and VCU clears the files
                            // when this file is discovered. VCU will then delete the file
                            if (cBoxClearRTDMData.Checked)
                            {
                                UploadClearFile(RTDM_DRIVE_DIR + "/Clear.rtdm", "CLEAR RTDM");
                            }
                        }
                        else
                        {
                            toolStripText += "RTDM FAILED... ";
                            toolStripStatusLabel2.Text = "File Downloading Complete... ERROR PRODUCING VIEWABLE RTDM FILE!!!";
                            timer1.Enabled = false;
                            MessageBox.Show(errorString.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            timer1.Enabled = true;
                        }
                    }

                    // Create the IELF file and corresponding CSV file
                    if (cBoxIELFDownload.Checked)
                    {
                        Int32 errorCode = BuildDanFileName(fileName, errorString);

                        String danFileName = fileName.ToString();
                        String newDanFileName = danFileName.Replace("Temp\\", "");

                        // Create new event file with the proper naming convention
                        String origFileNameIelf = Path.GetDirectoryName(danFileName) + "\\ielf.flt";
                        String newFileNameIelf = Path.GetDirectoryName(newDanFileName) + "\\" +
                                                    Path.GetFileNameWithoutExtension(newDanFileName) + ".flt";

                        newFileNameIelf = newFileNameIelf.Replace("rtdm", "iev_");

                        if (File.Exists(newFileNameIelf))
                        {
                            File.Delete(newFileNameIelf);
                        }
                        File.Move(origFileNameIelf, newFileNameIelf);

                        // Create a human readable CSV file from the binary file
                        CreateIelfCsv c = new CreateIelfCsv();
                        string errorMsg = c.CreateCSVFile(newFileNameIelf);
                        if (errorMsg != null)
                        {
                            toolStripText += "IELF FAILED";
                            MessageBox.Show(errorMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            toolStripText += "IELF OK";
                        }
                        // Clear all IELF data if checked (upload clear file and VCU clears the files
                        // when this file is discovered. VCU will then delete the file
                        if (cBoxClearIELFData.Checked)
                        {
                            UploadClearFile(IELF_DRIVE_DIR + "/Clear.ielf", "CLEAR IELF");
                        }
                    }

                    toolStripStatusLabel2.Text = toolStripText;

                    // Delete all other files and "Temp" directory
                    Directory.Delete(@"C:\Ptu\Pcu\Data\Temp\", true);

                    // Poll and wait for directory deletion; this is only done to avoid conflicts
                    // with a subsequent download from another VCU
                    while (Directory.Exists(@"C:\Ptu\Pcu\Data\Temp\"))
                    {
                        System.Threading.Thread.Sleep(100);
                    }

                    m_FTPDownloadState = FTPDownloadState.CHECK_FOR_MORE;
                    break;

                case FTPDownloadState.CHECK_FOR_MORE:
                    // Are there more target VCUs to download from ?
                    m_VCUIndex++;
                    if (m_VCUIndex < m_VCUList.Count)
                    {
                        QueueUpFileDownloadList();
                        m_FTPDownloadState = FTPDownloadState.INITIATE_DOWNLOAD;
                    }
                    else
                    {
                        buttonCancelDownload.Enabled = false;
                        buttonStartDownload.Enabled = true;
                        buttonSelectVCU.Enabled = true;
                        groupBoxRTDM.Enabled = true;
                        groupBoxIELF.Enabled = true;
                        m_FTPDownloadState = FTPDownloadState.IDLE;
                    }
                    break;
            }
        }

        /// <summary>
        /// The purpose of this function is to upload a "clear" file to a VCU. This occurs when the user wishes to clear
        /// the RTDM/IELF data present on the VCU. When the VCU detects the presence of one of these files, it clears the
        /// appropriate data.
        /// </summary>
        /// <param name="fileName">The name of the file to upload</param>
        /// <param name="fileContents">The contents of the file (currently unused)</param>
        private void UploadClearFile(string fileName, string fileContents)
        {
            string ftpPath = @"ftp://" + dataGridView1.Rows[m_VCUIndex].Cells[0].Value.ToString() + "/" + fileName;

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "");

            // Copy the contents of the file to the request stream.
            byte[] fileBytes = Encoding.ASCII.GetBytes(fileContents);
            request.ContentLength = fileBytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileBytes, 0, fileBytes.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }

        /// <summary>
        /// Handles the form closing event
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">form closing event info</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Required when the "x" is clicked and an FTP download is in progress
            Environment.Exit(0);
        }

        /// <summary>
        /// Responsible for monitoring the "dead" time during an FTP download. If the amount
        /// of dead time exceeds m_FTPTimeout, the user is informed that an error occurred.
        /// </summary>
        private void MonitorDownloadProgress()
        {
            // m_FTPTimeoutCount is reset when the FTP progress changed event occurs
            m_FTPTimeoutCount += (uint)timer1.Interval;
            // Check if the dead time was exceeded
            if (m_FTPTimeoutCount >= m_FTPTimeout)
            {
                m_WebClient.CancelAsync();
                InvokeErrorMessageBox("The FTP download has timed out. Please retry when the connection is available.", FTPStopped.BY_ERROR);
            }
        }

        /// <summary>
        /// Displays the Help... About form
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">menu bar click event info</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formHelpAbout = new HelpAboutForm();
            formHelpAbout.ShowDialog();
        }

        /// <summary>
        /// Displays a custom message box informing the user of the error that was detected
        /// and presents the user with options based on when and where the error
        /// </summary>
        /// <param name="errorMessage">error message displayed to the user</param>
        /// <param name="userOrError">user canceled or error stopped the download</param>
        private void InvokeErrorMessageBox(string errorMessage, FTPStopped userOrError)
        {
            m_HandleDownloadError = true;
            m_FTPDownloadState = FTPDownloadState.HANDLE_ERROR;

            bool lastUrl = (m_VCUIndex + 1 >= m_VCUList.Count) ? true : false;

            // "using" required so that form is kept from being garbage collected until
            // the UserChoice is read
            using (HandleErrorForm errorForm = new HandleErrorForm(errorMessage, lastUrl))
            {
                errorForm.StartPosition = FormStartPosition.CenterScreen;
                errorForm.ShowDialog(this);
                switch (errorForm.UserChoice)
                {
                    case HandleErrorForm.UserChoiceEnum.CONTINUE_WITH_NEXT_URL:
                        // display the proper info in the data grid on why the VCU download was stopped
                        if (userOrError == FTPStopped.BY_USER)
                        {
                            dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Canceled";
                        }
                        else
                        {
                            dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Error";
                        }
                        dataGridView1.Rows[m_VCUIndex].Cells[2].Value = "N/A";
                        dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.OrangeRed;
                        m_FTPDownloadState = FTPDownloadState.CHECK_FOR_MORE;
                        break;

                    case HandleErrorForm.UserChoiceEnum.CANCEL_ALL_REMAINING:
                        if (userOrError == FTPStopped.BY_USER)
                        {
                            CancelRemaining("Canceled");
                        }
                        else
                        {
                            CancelRemaining("Error");
                        }
                        m_FTPDownloadState = FTPDownloadState.IDLE;
                        break;

                    case HandleErrorForm.UserChoiceEnum.LAST_URL:
                        // Handle the last URL in the list
                        if (userOrError == FTPStopped.BY_USER)
                        {
                            dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Canceled";
                        }
                        else
                        {
                            dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Error";
                        }
                        dataGridView1.Rows[m_VCUIndex].Cells[2].Value = "N/A";
                        dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.OrangeRed;
                        m_FTPDownloadState = FTPDownloadState.IDLE;
                        buttonCancelDownload.Enabled = false;
                        buttonStartDownload.Enabled = true;
                        buttonSelectVCU.Enabled = true;
                        groupBoxRTDM.Enabled = true;
                        groupBoxIELF.Enabled = true;
                        toolStripStatusLabel2.Text = "Click \"Start FTP Download\" button to restart the FTP downloading.";
                        break;
                }
            }
        }

        /// <summary>
        /// Cancels all remaining downloads from any additional selected VCUs
        /// </summary>
        /// <param name="errorOrCancel">Informs user whether error or user canceled.</param>
        private void CancelRemaining(string errorOrCancel)
        {
            dataGridView1.Rows[m_VCUIndex].Cells[1].Value = errorOrCancel;
            dataGridView1.Rows[m_VCUIndex].Cells[2].Value = "N/A";
            dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.OrangeRed;

            m_VCUIndex++;
            while (m_VCUIndex < m_VCUList.Count)
            {
                dataGridView1.Rows[m_VCUIndex].Cells[1].Value = "Canceled";
                dataGridView1.Rows[m_VCUIndex].Cells[2].Value = "N/A";
                dataGridView1.Rows[m_VCUIndex].DefaultCellStyle.BackColor = Color.OrangeRed;
                m_VCUIndex++;
            }

            m_FTPDownloadState = FTPDownloadState.IDLE;
            buttonCancelDownload.Enabled = false;
            buttonStartDownload.Enabled = true;
            buttonSelectVCU.Enabled = true;
            groupBoxRTDM.Enabled = true;
            groupBoxIELF.Enabled = true;
            toolStripStatusLabel2.Text = "Click \"Start FTP Download\" button to restart the FTP downloading.";
        }

        /// <summary>
        /// Allow the user to change the FTP timeout (the max amount of consecutive time when no data
        /// is received from the FTP Server).
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">numeric up down control value changed info</param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            m_FTPTimeout = (int)numUpDownFTPTimeout.Value * 1000;
        }

        /// <summary>
        /// This control becomes enabled after at least one VCU is selected. It allows the user to clear all
        /// RTDM data on the selected VCU(s). If checked, a file will be FTP'ed to the VCU after all RTDM data is downloaded to indicate to the
        /// VCU to clear all RTDM related data.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxClearVCUFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxClearRTDMData.Checked)
            {
                DialogResult result = MessageBox.Show("After downloading the RTDM data from the VCU(s), all RTDM data will be cleared. Are you sure you want to do this?", "Clear VCU RTDM files",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    cBoxClearRTDMData.CheckState = CheckState.Unchecked;
                }
            }
        }

        /// <summary>
        /// This control becomes enabled after at least one VCU is selected. It allows the user to clear all
        /// IELF data on the selected VCU(s). If checked, a file will be FTP'ed to the VCU after all RTDM data is downloaded to indicate to the
        /// VCU to clear all IELF related data.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxClearIELFData_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxClearIELFData.Checked)
            {
                DialogResult result = MessageBox.Show("After downloading the IELF data from the VCU(s), all IELF data will be cleared. Are you sure you want to do this?", "Clear VCU IELF files",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    cBoxClearIELFData.CheckState = CheckState.Unchecked;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxRTDMDownload_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxRTDMDownload.Checked)
            {
                cBoxClearRTDMData.Enabled = true;
            }
            else
            {
                cBoxClearRTDMData.Enabled = false;
            }
            DetermineStartDownloadButtonState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">check box control changed arguments</param>
        private void cBoxDownloadIELF_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxIELFDownload.Checked)
            {
                cBoxClearIELFData.Enabled = true;
            }
            else
            {
                cBoxClearIELFData.Enabled = false;
            }
            DetermineStartDownloadButtonState();
        }

        /// <summary>
        ///
        /// </summary>
        private void DetermineStartDownloadButtonState()
        {
            if (cBoxIELFDownload.Checked || cBoxRTDMDownload.Checked)
            {
                buttonStartDownload.Enabled = true;
            }
            else
            {
                buttonStartDownload.Enabled = false;
            }
        }

        #endregion --- Methods ---
    }
}
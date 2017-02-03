using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FTPDownloadRTDM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            System.Threading.Mutex m = new System.Threading.Mutex(true, "FTPDownloadRTDM", out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Another instance of the FTP Download RTDM Application is already running. " +
                                "Only 1 instance of the FTP Download RTDM application can execute simultaneously.",
                                "FTP Download RTDM Application Startup Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Check if any parameters have been passed to the PTU.
            if (args.Length > 0)
            {
                Application.Run(new MainForm(args));
            }
            else
            {
                Application.Run(new MainForm());
            }

            GC.KeepAlive(m); 

        }
    }
}

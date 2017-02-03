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
 *  File name:  HandleErrorForm.cs
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
    /// Displays detected errors and requests user response on how to proceed after an error is detected
    /// </summary>
    public partial class HandleErrorForm : Form
    {
        #region --- Enumerations ---

        /// <summary>
        /// Public enumeration that indicates the user selection when presented with options
        /// on what to do next after the error has been displayed
        /// </summary>
        public enum UserChoiceEnum
        {
            /// <summary>
            /// Continue onto next URL in the list
            /// </summary>
            CONTINUE_WITH_NEXT_URL,

            /// <summary>
            /// Cancel all remaining FTP operations
            /// </summary>
            CANCEL_ALL_REMAINING,

            /// <summary>
            /// The error was detected on the last URL
            /// </summary>
            LAST_URL,
        }

        #endregion --- Enumerations ---

        #region --- Member Variables ---

        /// <summary>
        /// Allow the parent form to access the User choice but only allow this form to modify it
        /// </summary>
        public UserChoiceEnum UserChoice { get; private set; }

        #endregion --- Member Variables ---

        #region --- Constructors ---

        /// <summary>
        /// Windows default constructor forcing the parent form to use the constructor below by making
        /// access private
        /// </summary>
        private HandleErrorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Force the parent form to use this constructor
        /// </summary>
        /// <param name="errorMessage">error message to be displayed to the user</param>
        /// <param name="closeOnly">set true if the error occurred on the last URI</param>
        public HandleErrorForm(string errorMessage, bool closeOnly)
            : this()
        {
            // display the error message
            labelErrorMessage.Text = errorMessage;
            // change the default buttons that are visible if the error occurred on the last URI in the list
            if (closeOnly)
            {
                buttonContinueWithNext.Visible = false;
                buttonCancelAll.Visible = false;
                buttonClose.Visible = true;
            }
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
        /// Clicking this button indicates the user wishes to continue with the next URL. Button
        /// is made invisible when the error occurred on the last URL
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button event info</param>
        private void buttonContinueWithNext_Click(object sender, EventArgs e)
        {
            UserChoice = UserChoiceEnum.CONTINUE_WITH_NEXT_URL;
            Close();
        }

        /// <summary>
        /// Clicking this button indicates the user wishes to cancel all remaining FTP transfers. Button
        /// is made invisible when the error occurred on the last URL.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button event info</param>
        private void buttonCancelAll_Click(object sender, EventArgs e)
        {
            UserChoice = UserChoiceEnum.CANCEL_ALL_REMAINING;
            Close();
        }

        /// <summary>
        /// Clicking this button closes the form. Button is made visible only
        /// when the error occurred on the last URL.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">button event info</param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            UserChoice = UserChoiceEnum.LAST_URL;
            Close();
        }

        #endregion --- Methods ---
    }
}
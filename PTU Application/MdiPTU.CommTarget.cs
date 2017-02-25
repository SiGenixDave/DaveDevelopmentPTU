#region --- Revision History ---

/*
 *
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.
 *  Offenders will be held liable for the payment of damages.
 *
 *  (C) 2017    Bombardier Inc. or its subsidiaries. All rights reserved.
 *
 *  Solution:   Portable Test Unit
 *
 *  Project:    PTU Application
 *
 *  File name:  MdiPTU.CommTarget.cs
 *
 *  Revision History
 *  ----------------
 */

#region - [1.0] -
/*
 *  Date        Version Author          Comments
 *  02/25/17    1.0     D.Smail         1.  Created to support cycling "pinging" of hardware target on MainForm.
 * 
 */
#endregion - [1.0] -

#endregion --- Revision History ---

using System;
using System.Drawing;
using System.Threading;
using VcuComm;
using Bombardier.PTU.Properties;

namespace Bombardier.PTU
{
    /// <summary>
    /// Methods to support the MdiPTU class.
    /// </summary>
    public partial class MdiPTU
    {
        #region - [Watchdog] -
        /// <summary>
        /// A record of the watchdog count. Used to determine if the thread on which the polling is carried out has locked.
        /// </summary>
        private int m_Watchdog;

        /// <summary>
        /// The countdown to the watchdog trip.
        /// </summary>
        private int m_WatchdogTripCountdown;

        /// <summary>
        /// A record of the packet count. Used to determine if polling is active so that the packet received icon can be blinked in a thread-safe way.
        /// </summary>
        private long m_ResponseCount;

        /// <summary>
        /// A flag to control the display update. True, stops the display update i.e pauses the display; false, re-starts the display update.
        /// </summary>
        private bool m_Pause = false;
        #endregion - [Watchdog] -

        /// <summary>
        /// Windows timer used to verify target hardware is responding with a valid response when connected 
        /// </summary>
        private System.Windows.Forms.Timer m_CommTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Background thread used to communicate with target hardware and detect any issues with the communication 
        /// </summary>
        private ThreadCommTarget m_ThreadCommTarget;

        /// <summary>
        /// The countdown value associated the watchdog trip. Value: 4.
        /// </summary>
        private int WatchdogTripCountdown = 4;

        /// <summary>
        /// Gets or sets the reference to the class responsible for polling the target hardware.
        /// </summary>
        private ThreadCommTarget ThreadCommTarget
        {
            get { return m_ThreadCommTarget; }
            set { m_ThreadCommTarget = value; }
        }

        /// <summary>
        /// Invoked at instantiation to start the windows UI timer and the background thread that
        /// will "ping" the target hardware when connected. The thread will be "paused" when no target
        /// hardware is connected or communication is lost. Unlike the other comm threads in the watch,
        /// self test, and/or event windows, this background is not destroyed until the application
        /// closes
        /// </summary>
        private void InitCommTimer()
        {
            // Create the thread and immediately pause it
            ThreadCommTarget = new ThreadCommTarget(this);
            ThreadCommTarget.Start();
            PauseCommThread();

            // set the interval, event handler when the windows timer expires and start the timer
            m_CommTimer.Interval = 1000;
            m_CommTimer.Tick += new EventHandler(m_CommTimer_Tick);
            m_CommTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PauseCommThread()
        {
            // Request the comm thread to pause and then wait for feedback from it that it is paused
            ThreadCommTarget.Pause = true;
            do
            {
                Thread.Sleep(50);
            }
            while (!ThreadCommTarget.PauseFeedback);
            // This flag triggers the state machine to pause the check of the thread communication 
            m_Pause = true;
        }

        /// <summary>
        /// Request the Windows timer to start polling the background thread to determine if there
        /// is a valid connection with the selected target hardware
        /// </summary>
        public void ResumePollingTargetHardware()
        {
            // This flag triggers the state machine to resume the check of the thread communication 
            m_Pause = false;
            // Initialize the watchdog trip countdown.
            m_WatchdogTripCountdown = WatchdogTripCountdown;
        }

        /// <summary>
        /// State machine states of the communication status with the target hardware
        /// </summary>
        private enum CommunicationState
        {
            INIT,
            POLL,
            PAUSE,
        }

        private CommunicationState commState = CommunicationState.INIT;

        /// <summary>
        /// Invoked periodically to check the state of the communication interface as well as to
        /// pause and resume polling of the target hardware based on system conditions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_CommTimer_Tick(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            switch (commState)
            {
                case CommunicationState.INIT:
                default:
                    // CommDevice is non-null when connected to any target (both actual and simulated)
                    if (m_CommunicationInterface != null)
                    {
                        // "CommDevice" checked because it remains null if a Simulated Target (F3) connected but is 
                        // non-null if connected to a real hardware target. Do not want to poll if connected to a 
                        // simulated target
                        if (m_CommunicationInterface.CommDevice != null)
                        {
                            commState = CommunicationState.POLL;
                            ThreadCommTarget.Pause = false;
                            ResumePollingTargetHardware();
                        }
                    }
                    break;

                case CommunicationState.POLL:
                    // Handle case where user disconnects by clicking F2 after a connection to a hardware target has been made
                    if (m_CommunicationInterface == null)
                    {
                        ThreadCommTarget.Pause = true;
                        commState = CommunicationState.INIT;
                    }
                    else
                    {
                        // Check if user moved to new form (sys info, watch, events or self test). "m_Pause" will
                        // be made true if thats the case
                        if (m_Pause)
                        {
                            commState = CommunicationState.PAUSE;
                        }
                        else
                        {
                            // will return INIT or POLL based on the state of the hardware target (connected returns POLL,
                            // disconnected returns INIT)
                            commState = PollTarget();
                        }
                    }
                    break;

                case CommunicationState.PAUSE:
                    // Becomes false when returning from a child form
                    if (!m_Pause)
                    {
                        // Check if comm still OK. Child form will make null if error in comm occurs
                        if (m_CommunicationInterface != null)
                        {
                            // All is well, resume polling
                            ThreadCommTarget.Pause = false;
                            commState = CommunicationState.POLL;
                        }
                        else
                        {
                            // Comm was lost on child form, wait for new connection
                            ThreadCommTarget.Pause = true;
                            commState = CommunicationState.INIT;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Code is used to determine if background thread is successfully communicating with target hardware. This 
        /// code is similar to code used in other forms.
        /// </summary>
        /// <returns>INIT if target hardware communication is determined to be down. POLL if all is good</returns>
        private CommunicationState PollTarget()
        {
            // Update the local variables with the appropriate property values of the thread that is responsible for VCU communications.
            int watchdog;
            bool communicationFault;
            long validCommWatchdogResponseCount;

            watchdog = ThreadCommTarget.Watchdog;
            communicationFault = ThreadCommTarget.CommunicationFault;
            validCommWatchdogResponseCount = ThreadCommTarget.ResponseCount;

            // ------------------------------------------
            // Check if the communication port is locked.
            // ------------------------------------------
            bool watchdogTrip = false;
            if (watchdog == m_Watchdog)
            {
                // Don't assert the watchdog trip flag until the countdown has elapsed. 
                if (m_WatchdogTripCountdown <= 0)
                {
                    watchdogTrip = true;
                }
                else
                {
                    m_WatchdogTripCountdown--;
                }
            }
            else
            {
                m_WatchdogTripCountdown = WatchdogTripCountdown;
                m_Watchdog = watchdog;
            }

            // Only update on transitions of the flag.
            if (watchdogTrip || communicationFault)
            {
                // Update status message
                WriteStatusMessage(Resources.EMLossOfComm, Color.Red, Color.Black);
                SetMode(Common.Mode.Configuration);

                // In order to avoid the thread getting locked in infinite loop around the m_watchdog
                // (visit ThreadCommTarget Run() method), acknowledge the communication fault. This avoids
                // the TargetCommThread being left hanging if the user closes this application after a comm
                // error occurs. 
                ThreadCommTarget.CommunicationFault = false;
                PauseCommThread();
                m_CommunicationInterface = null;
                return CommunicationState.INIT;
            }


            // Blink the icon to show that watch data is being updated.
            if (validCommWatchdogResponseCount != m_ResponseCount)
            {
                BlinkUpdateIcon();
                m_ResponseCount = validCommWatchdogResponseCount;
            }

            return CommunicationState.POLL;

        }

   }
}
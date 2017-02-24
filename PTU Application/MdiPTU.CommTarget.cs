#region --- Revision History ---

/*
 *
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.
 *  Offenders will be held liable for the payment of damages.
 *
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
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

#endregion --- Revision History ---

using System;
using System.Drawing;
using System.Threading;
using VcuComm;

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
        /// TODO
        /// </summary>
        private System.Windows.Forms.Timer m_CommTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// TODO
        /// </summary>
        private ThreadCommTarget m_ThreadCommTarget;

        /// <summary>
        /// The countdown value associated the watchdog trip. Value: 4.
        /// </summary>
        private int WatchdogTripCountdown = 4;

        /// <summary>
        /// Gets or sets the reference to the class responsible for polling the target hardware and recording the watch values.
        /// </summary>
        private ThreadCommTarget ThreadCommTarget
        {
            get { return m_ThreadCommTarget; }
            set { m_ThreadCommTarget = value; }
        }


        private void InitCommTimer()
        {

            ThreadCommTarget = new ThreadCommTarget(this);
            ThreadCommTarget.Start();
            PauseCommThread();

            //TODO
            m_CommTimer.Interval = 1000;
            m_CommTimer.Tick += new EventHandler(m_CommTimer_Tick);
            m_CommTimer.Start();

        }

        private void PauseCommThread()
        {
            ThreadCommTarget.Pause = true;
            do
            {
                Thread.Sleep(50);
            }
            while (!ThreadCommTarget.PauseFeedback);
            m_Pause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumePollingTargetHardware()
        {
            m_Pause = false;
            // Initialize the watchdog trip countdown.
            m_WatchdogTripCountdown = WatchdogTripCountdown;
        }


        private enum CommunicationState
        {
            INIT,
            POLL,
            PAUSE,
        }

        private CommunicationState commState = CommunicationState.INIT;

        //TODO
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
                    // CommDevice checked because it remains null
                    if (m_CommunicationInterface != null)
                    {
                        // "CommDevice" checked because it remains null if a Simulated Target (F3) connected
                        if (m_CommunicationInterface.CommDevice != null)
                        {
                            commState = CommunicationState.POLL;
                            ThreadCommTarget.Pause = false;
                            ResumePollingTargetHardware();
                        }
                    }
                    break;

                case CommunicationState.POLL:
                    // Handle case where user disconnects by clicking F2 
                    if (m_CommunicationInterface == null)
                    {
                        ThreadCommTarget.Pause = true;
                        commState = CommunicationState.INIT;
                    }
                    else
                    {
                        // Check if user moved to new form (sys info, watch, events or self test)
                        if (m_Pause)
                        {
                            commState = CommunicationState.PAUSE;
                        }
                        else
                        {
                            commState = PollTarget();
                        }
                    }
                    break;

                case CommunicationState.PAUSE:
                    if (!m_Pause)
                    {
                        if (m_CommunicationInterface != null)
                        {
                            ThreadCommTarget.Pause = false;
                            commState = CommunicationState.POLL;
                        }
                        else
                        {
                            ThreadCommTarget.Pause = true;
                            commState = CommunicationState.INIT;
                        }
                    }
                    break;
            }
        }

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
                WriteStatusMessage("Loss Of Communications",
                                    Color.Red, Color.Black);
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
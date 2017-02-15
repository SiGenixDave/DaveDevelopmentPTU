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
 *  Project:    SelfTest
 * 
 *  File name:  ThreadCommWatchdog.cs
 * 
 *  Revision History
 *  ----------------
 */

/* 
 *  Date        Version Author          Comments
 *                                      
 *  01/31/2017  1.0.1   D.Smail         References
 *                                      1.  Copy and paste from ThreadPollEvent.cs. Used to perform periodic communication watchdog checks
 *                                          while in self test (no checks exist prior to this change)
 *                                          
 *  01/31/2017  1.0.2   D.Smail         Modifications
 *                                      1.  Added code that supports a response from the target hardware which indicates whether or not
 *                                          the target hardware is in self test. Depending on system conditions, the target hardware can
 *                                          exit self test autonomously. 
 * 
 * 
 */
#endregion --- Revision History ---

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Common;
using Common.Communication;
using Common.Configuration;
using SelfTest.Forms;
using VcuComm;

namespace SelfTest
{
    /// <summary>
    /// Polls the target hardware while in self test to determine if a valid communication link still exists. The link could be
    /// dropped due to a pulled cable, a target hardware reset or power cycle, etc.
    /// </summary>
    class ThreadCommWatchdog : WorkerThread
    {
        #region --- Constants ---
        /// <summary>
        /// The interval, in ms, between communication watchdog checks. Value: 1,000 ms.
        /// </summary>
        private const int IntervalMsEventUpdate = 1000;

        /// <summary>
        /// The countdown value associated the read timeout. Value: 3.
        /// </summary>
        private int ReadTimeoutCountdown = 3;

        /// <summary>
        /// The thread sleep interval, in ms, between checking the state of the Pause property. Value: 200 ms.
        /// </summary>
        private int SleepMsCheckPause = 200;

        /// <summary>
        /// The thread sleep interval, in ms, between watchdog refreshes once a communication fault has been detected.  Value: 50 ms. 
        /// </summary>
        private int SleepMsRefreshWatchdog = 50;
        #endregion --- Constants ---

        #region - [Member Variables] -
        #region - [Properties] -
        /// <summary>
        /// Reference to the selected communication interface.
        /// </summary>
        private ICommunicationSelfTest m_CommunicationInterface;

        /// <summary>
        /// Flag to control polling of the target hardware. True, inhibits polling of the target hardware; otherwise, false, resumes polling.
        /// </summary>
        private bool m_Pause;

        /// <summary>
        /// Feedback flag to indicate that the polling of the target hardware has been inhibited.
        /// </summary>
        private bool m_PauseFeedback;

        /// <summary>
        /// Flag used to indicate that there is a communication fault. True, indicates that a fault exists; otherwise, false, indicates that communication is OK.
        /// </summary>
        private bool m_CommunicationFault;

        /// <summary>
        /// Flag used to indicate whether the target hardware is in self test mode. True, indicates that the target hardware is in self test mode; otherwise, false, indicates that communication is OK.
        /// </summary>
        private bool m_InSelfTestWatchdogResponse;

        /// <summary>
        /// The number of response that have been received since the class was instantiated;
        /// </summary>
        /// <remarks> 
        /// Used as a thread-safe way of blinking the packet received icon on the main window. The property corresponding to this value is read by the display 
        /// update method on the main thread and provided it has incremented since the previous display update that method will blink the icon. As the method
        /// is on the same thread on which the icon was created it is inherently safe.
        /// </remarks>
        private long m_ResponseCount;
        #endregion - [Properties] -

        #region - [Mutex] -
        /// <summary>
        /// Mutex to control read/write access to the <c>Pause</c> property.
        /// </summary>
        private Mutex m_MutexPause;

        /// <summary>
        /// Mutex to control read/write access to the <c>PauseFeedback</c> property.
        /// </summary>
        private Mutex m_MutexPauseFeedback;

        /// <summary>
        /// Mutex to control read/write access to the <c>CommunicationFault</c> property.
        /// </summary>
        private Mutex m_MutexCommunicationFault;
        #endregion - [Mutex] -

        /// <summary>
        /// Reference to the calling form.
        /// </summary>
        private FormViewTestResults m_FormViewTestResults;

        /// <summary>
        /// Reference to the class that schedules polling for new events.
        /// </summary>
        private PollScheduler m_PollScheduler;

        /// <summary>
        /// The countdown to the read timeout.
        /// </summary>
        private int m_ReadTimeoutCountdown;
        #endregion - [Member Variables] -

        #region - [Constructors] -
        /// <summary>
        /// Initializes a new instance of the class. Initializes the communication interface and read/write locks.
        /// </summary>
        /// <param name="communicationInterface">Reference to the communication interface used to communicate with the target hardware.</param>
        /// <param name="formViewTestResults">Reference to the form that called this form.</param>
        public ThreadCommWatchdog(ICommunicationSelfTest communicationInterface, FormViewTestResults formViewTestResults)
        {
            CommunicationInterface = communicationInterface;
            Debug.Assert(communicationInterface != null, "ThreadCommWatchdog.Ctor() - [communicationSelfTest = null]");
            
            m_FormViewTestResults = formViewTestResults;
            Debug.Assert(m_FormViewTestResults != null, "ThreadCommWatchdog.Ctor() - [m_FormViewTestResults != null]");

            m_MutexPause = new Mutex();
            m_MutexPauseFeedback = new Mutex();
            m_MutexCommunicationFault = new Mutex();

            m_ResponseCount = 0;
            m_PollScheduler = new PollScheduler();
            m_ReadTimeoutCountdown = ReadTimeoutCountdown;

            m_InSelfTestWatchdogResponse = true;

        }
        #endregion - [Constructors] -

        #region - [Methods] -
        /// <summary>
        /// Poll the target hardware for new events.
        /// </summary>
        /// <remarks>Runs on a separate thread.</remarks>
        public override void Run()
        {
            try
            {
                m_CommunicationFault = false;
                while (StopThread == false)
                {
                    if (Pause == false)
                    {
                        PauseFeedback = false;
                        m_PollScheduler.Wait(IntervalMsEventUpdate);
                        if (Pause == true)
                        {
                            m_Watchdog++;
                            continue;
                        }

                        // See if a connection still exists with the embedded system.
                        try
                        {
                            m_Watchdog++;
                            m_CommunicationInterface.CommunicationWatchdog(ref m_InSelfTestWatchdogResponse);
                            m_ReadTimeoutCountdown = ReadTimeoutCountdown;
                        }
                        catch(CommunicationException)
                        {
                            // Don't assert the communication fault flag until the countdown has elapsed. 
                            if (m_ReadTimeoutCountdown <= 0)
                            {
                                // Assert the CommunicationFault property.
                                m_CommunicationFault = true;

                                // Close the communication Port.
                                m_CommunicationInterface.CloseCommunication(m_CommunicationInterface.CommunicationSetting.Protocol);

                                // Keep the watchdog ticking over so that the client can determine whether the port has locked. 
                                do
                                {
                                    m_Watchdog++;
                                    Thread.Sleep(SleepMsRefreshWatchdog);
                                }
                                while (m_CommunicationFault == true);
                            }
                            else
                            {
                                m_ReadTimeoutCountdown--;
                                continue;
                            }
                        }
                        m_CommunicationFault = false;

                        // Keep a count of the number of responses received. Used as a thread-safe way of blinking the packet received icon on the main window. This value
                        // is read by the display update method on the main thread and provided it has incremented since the timeout last expired it will blink the icon.
                        m_ResponseCount++;

                        // Skip if the Pause property has been asserted.
                        if (Pause == true)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        PauseFeedback = true;
                        m_Watchdog++;
                        Thread.Sleep(SleepMsCheckPause);
                    }
                }
            }
            finally
            {
                base.Run();
            }
        }
        #endregion - [Methods] -

        #region - [Properties] -
        /// <summary>
        /// Gets or sets the communication interface that is to be used to communicate with the VCU.
        /// </summary>
        public ICommunicationSelfTest CommunicationInterface
        {
            get { return m_CommunicationInterface; }
            set { m_CommunicationInterface = value; }
        }

        /// <summary>
        /// Gets the Self Test state of the target hardware. true if in self test; false otherwise
        /// </summary>
        public Boolean InSelfTest
        {
            get { return m_InSelfTestWatchdogResponse; }
        }

        /// <summary>
        /// Gets or sets the flag that controls the polling of the target hardware. True, inhibits polling of the target hardware; otherwise, false, resumes polling.
        /// </summary>
        public bool Pause
        {
            get
            {
                bool result = false;
                m_MutexPause.WaitOne(DefaultMutexWaitDurationMs, false);
                result = m_Pause;
                m_MutexPause.ReleaseMutex();
                return result;
            }

            set
            {
                m_MutexPause.WaitOne(DefaultMutexWaitDurationMs, false);
                m_Pause = value;
                if (m_PollScheduler != null)
                {
                    // Terminate the scheduler if the Pause property is asserted.
                    m_PollScheduler.TerminateFlag = m_Pause;
                }
                m_MutexPause.ReleaseMutex();
            }
        }

        /// <summary>
        /// Gets the feedback flag that indicates whether polling of the target hardware has been suspended.  
        /// </summary>
        /// <remarks>This flag is asserted when the polling has entered the pause state, i.e. the current communication request is complete and no further requests will 
        /// be issued until the pause property has been cleared.</remarks>
        public bool PauseFeedback
        {
            get
            {
                bool result = false;
                m_MutexPauseFeedback.WaitOne(DefaultMutexWaitDurationMs, false);
                result = m_PauseFeedback;
                m_MutexPauseFeedback.ReleaseMutex();
                return result;
            }

            set
            {
                m_MutexPauseFeedback.WaitOne(DefaultMutexWaitDurationMs, false);
                m_PauseFeedback = value;
                m_MutexPauseFeedback.ReleaseMutex();
            }
        }

        /// <summary>
        /// Gets the flag used to indicate that there is a communication fault. True, indicates that a fault exists; otherwise, false, indicates that communication is OK.
        /// </summary>
        public bool CommunicationFault
        {
            get { return m_CommunicationFault; }
        }

        /// <summary>
        /// Gets the number of packets received since the class was instantiated.
        /// </summary>
        public long ResponseCount
        {
            get { return m_ResponseCount; }
        }
        #endregion - [Properties] -
    }
}

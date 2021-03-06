#region --- Revision History ---
/*
 *
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.
 *  Offenders will be held liable for the payment of damages.
 *
 *  (C) 2016    Bombardier Inc. or its subsidiaries. All rights reserved.
 *
 *  Solution:   PTU
 *
 *  Project:    Common
 *
 *  File name:  EventStreamMarshal.cs
 *
 *  Revision History
 *  ----------------
 */
#region - [1.0 to 1.1] -
/*
 *  Date        Version Author      Comments
 *  03/01/2015  1.0     D.Smail     First Release.
 *
 *  03/22/2016  1.1     D.Smail     References
 *                                  1.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 10. While trying out version 6.16.3 of the PTU, there was a problem if the user tries
 *                                      to download an, albeit invalid, data stream. Polling for new events was initially OK, however once the download was complete,
 *                                      the PTU reported �Unable to retrieve the data stream associated with the MVB Communication Failure event� and polling for new
 *                                      events was never resumed. After a 10 second period, Windows reported that the �Portable Test Application has stopped working�
 *                                      without ever throwing an exception.
 *                                      
 *                                  2.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 14. This is linked to SNCR-PTU [01 Mar 2016] - Item 10. While trying to save a CTA 
 *                                      event log containing invalid data streams, in order to demonstrate the generation of a CSV file, once the invalid data stream
 *                                      had been unsuccessfully downloaded and the PTU resumed polling for new events an ApplicationException was thrown by the call
 *                                      to m_MutexCommuncationInterface.ReleaseMutex() in the finally block of the try/catch block in the
 *                                      CommunicationEvent.CheckFaultLogger() method. Details of the exception are as follows:
 *                                      
 *                                          System.ApplicationException was unhandled HResult=-2146232832
 *                                          Message=Object synchronization method was called from an unsynchronized block of code.
 *                                          Source=mscorlib
 *                                          StackTrace:
 *                                          at System.Threading.Mutex.ReleaseMutex()
 *                                          at Event.Communication.CommunicationEvent.CheckFaultLogger(Int16& eventCount, UInt32& newIndex, UInt32& newEventsLogged) in
 *                                          \\psf\Home\Documents\Contracts\Bombardier PTU\Working\Visual Studio Solutions\Portable Test Unit\DLL\Event\Communication
 *                                          \CommunicationEvent.cs:line 908
 *                                          at Event.ThreadPollEvent.Run() in \\psf\Home\Documents\Contracts\Bombardier PTU\Working\Visual Studio Solutions\Portable
 *                                          Test Unit\DLL\Event\ThreadPollEvent.cs:line 235
 *                                          .
 *                                          .
 *                                          .
 *                                          at System.Threading.ThreadHelper.ThreadStart()
 *                                          
 *                                  3.  Bug Fix - SNCR-PTU [01 Mar 16] Item 18. While testing the changes associated with items 10, 11, 14 and 15 a number of valid
 *                                      fault datastreams were generated for the CTA project using the following procedure:
 *                                      
 *                                          (1) synchronised the VCU time with the PC time; 
 *                                          (2) ensured that the event flags were configured such that a �Battery Voltage Out of Range� event generates a data stream; 
 *                                          (3) initialised the event log system; 
 *                                          (4) ensured that the data stream workset loaded into the VCU was set to the �Factory Settings� workset; and; 
 *                                          (5) created a number of �Battery Voltage Out of Range� faults using the procedure outlined by John Paul in the document
 *                                              �[J.P.] - Method to Generate Event Variables.pdf�.
 *                                          
 *                                      Once the fault log datastreams had been created, they were randomly downloaded, using the �Show DataStream� context menu, and 
 *                                      in each case, instead of displaying the expected data values, the vertical bar that represents the event trip time is displayed
 *                                      way over to the left, the duration is incorrect and the data values bear little correlation to the actual data values.
 *
 *                                  Modifications
 *                                  1.  Increased the size of the receive buffer size, MAX_FAULT_BUFFER_SIZE, from 4096 to 65,500 to account for larger payloads,
 *                                      especially when downloading data streams. - Ref.: 1,2.
 *                                      
 *                                  2.  Corrected the GetStream() method to ensure that the data value bytes contained within the m_RxMessaged member variable are
 *                                      byte swapped correctly depending upon whether the target uses Big Endian or Little Endian coding.- Ref. 3.
 *
 */
#endregion - [1.0 to 1.1] -

#region - [1.2] -
/*
 *  09/12/2016  1.2     DAS            References
 *                                      1.  Bug Fix - Support 4 digit year code returned from VCU.
 *                                      
 *                                      Modifications
 *                                      1.  Modified functions GetFaultHdr & GetFaultVar to include the bool Use4DigitYearCode
 *                                          so that a 4 digit year code can be interpreted properly when an event is generated by
 *                                          the VCU. For 2 digit year codes (1 byte), the TimeStamp is 6 bytes and the year is 
 *                                          in byte[5]. For 4 digit year codes (2 bytes), the TimeStamp is 8 bytes and the year is 
 *                                          in byte[5] & byte[6] where byte[5] contains the Most Significant Byte and byte[6] contains
 *                                          the Least Significant Byte. Byte[7] is a pad/spare byte in a 4 digit year code timestamp. 
 *                                          
 *                                      2. Added another VerifyDate function to support 4 digit year codes and the valid range check.
 */
#endregion - [1.2] -
#endregion --- Revision History ---

using System;
using VcuComm;
using System.Collections.Generic;


namespace Common.Communication
{
    /// <summary>
    /// This class contains methods used to generate commands and data requests to the embedded target
    /// and process the responses. All methods are related to handling fault and event log information
    /// as well as downloading stream data
    /// </summary>
    public class EventStreamMarshal
    {
        #region --- Constants ---

        /// <summary>
        /// Indicates an empty fault buffer. Value: UInt32.MaxValue.
        /// </summary>
        private const UInt32 EMPTY_FAULT_BUFFER = UInt32.MaxValue;

        /// <summary>
        /// The maximum amount of variables in any given data stream. Value: 256.
        /// </summary>
        private const Int16 MAX_DL_VARIABLES = 256;

        /// <summary>
        /// The maximum number of events per task. Value: 100.
        /// </summary>
        private const Int16 MAX_EVENTS_PER_TASK = 100;

        /// <summary>
        /// The maximum amount of fault data that can be sent from the embedded target to this application
        /// on any given message when the events are downloaded. If the amount of fault data exceeds this
        /// size, this application requests more events to be downloaded. Value: 65,500.
        /// </summary>
        private const UInt16 MAX_FAULT_BUFFER_SIZE = 65500;

        /// <summary>
        /// The maximum size of all attached data to any given fault. Value: 256.
        /// </summary>
        private const Int16 MAX_FAULT_SIZE_BYTES = 256;

        /// <summary>
        /// The maximum number of faults that the embedded target can store and subsequently the 
        /// maximum amount of faults this application can process. VAlue: 1,000.
        /// </summary>
        private const Int16 MAX_NUM_FAULTS = 1000;

        /// <summary>
        /// The maximum number of embedded target tasks. Value: 120.
        /// </summary>
        private const Int16 MAX_TASKS = 120;
        #endregion --- Constants ---

        #region --- Member Variables ---

        /// <summary>
        /// The type of communication device used to interface with the embedded target (RS-232, TCP, etc.).
        /// </summary>
        private ICommDevice m_CommDevice;

        /// <summary>
        /// Maintains the current number of faults downloaded from the embedded target.
        /// </summary>
        private Int16 m_CurrentNumberOfFaults;

        /// <summary>
        /// Used to process fault information received from the embedded target.
        /// </summary>
        private ProtocolPTU.GetFaultDataRes m_FaultDataFromTarget;

        /// <summary>
        /// List of fault data retrieved from the embedded target. Since faults
        /// and events are usually different sizes, the size of each individual array is dynamically
        /// allocated based on the fault size. A list was used instead of jagged array because list members
        /// can be easily added but they especially can be easily removed (i.e. when the fault log has
        /// the max number of faults).
        /// </summary>
        private List<Byte[]> m_FaultStorage = new List<Byte[]>();

        /// <summary>
        /// Object used to handle the standard embedded target communication protocol.
        /// </summary>
        private PtuTargetCommunication m_PtuTargetCommunication = new PtuTargetCommunication();

        /// <summary>
        /// Buffer used to store data responses from the embedded target. Need to add the header
        /// size.
        /// </summary>
        private Byte[] m_RxMessage = new Byte[MAX_FAULT_BUFFER_SIZE + ProtocolPTU.HEADER_SIZE_BYTES];
        #endregion --- Member Variables ---

        #region --- Constructors ---

        /// <summary>
        /// Constructor that must be used to create an object of this class.
        /// </summary>
        /// <param name="device">the type of communication device (RS-232, TCP, etc.)</param>
        public EventStreamMarshal(ICommDevice device)
        {
            m_CommDevice = device;
        }

        /// <summary>
        /// The default constructor is made private to force the use of the multi-argument constructor
        /// when creating an instance of this class.
        /// </summary>
        private EventStreamMarshal()
        { }

        #endregion --- Constructors ---

        #region --- Methods ---

        #region --- Public Methods ---

        /// <summary>
        /// This method requests the embedded target to change the event log that is to be monitored or 
        /// events / streams to be downloaded from. 
        /// </summary>
        /// <param name="NewEventLogNumber">the event log id to change to</param>
        /// <param name="DataRecordingRate">the data recording rate for the event log</param>
        /// <param name="ChangeStatus">Unknown</param>
        /// <param name="MaxTasks">the maximum amount of tasks for the fault log returned by the embedded target</param>
        /// <param name="MaxEventsPerTask">the maximum amount of events per task for the fault log returned by the embedded target</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError ChangeEventLog(Int16 NewEventLogNumber, ref Int16 DataRecordingRate, ref Int16 ChangeStatus,
                                                 ref Int16 MaxTasks, ref Int16 MaxEventsPerTask)
        {
            // Create the data request
            ProtocolPTU.ChangeEventLogReq request = new ProtocolPTU.ChangeEventLogReq(NewEventLogNumber);

            // Initiate transaction with embedded target
            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, request, m_RxMessage);

            if (commError == CommunicationError.Success)
            {
                // Extract all of the information from the received data
                ChangeStatus = BitConverter.ToInt16(m_RxMessage, 8);
                DataRecordingRate = BitConverter.ToInt16(m_RxMessage, 10);
                MaxTasks = BitConverter.ToInt16(m_RxMessage, 12);
                MaxEventsPerTask = BitConverter.ToInt16(m_RxMessage, 14);

                // Perform endian conversion if necessary
                if (m_CommDevice.IsTargetBigEndian())
                {
                    ChangeStatus = Utils.ReverseByteOrder(ChangeStatus);
                    DataRecordingRate = Utils.ReverseByteOrder(DataRecordingRate);
                    MaxTasks = Utils.ReverseByteOrder(MaxTasks);
                    MaxEventsPerTask = Utils.ReverseByteOrder(MaxEventsPerTask);
                }

                // Clamp max limits
                if (MaxTasks >= MAX_TASKS)
                {
                    MaxTasks = MAX_TASKS - 1;
                }
                if (MaxEventsPerTask >= MAX_EVENTS_PER_TASK)
                {
                    MaxEventsPerTask = MAX_EVENTS_PER_TASK - 1;
                }
            
            }

            return commError;
        }

        /// <summary>
        /// This method is invoked when polling the embedded target for any new events that have occurred while displaying 
        /// event screen. 
        /// </summary>
        /// <param name="PassedNumOfFaults">Will be updated with the current number of faults if the number of faults on the
        /// embedded target has changed since the last poll</param>
        /// <param name="orig_new">The most recent fault index from the embedded target</param>
        /// <param name="newEventsLogged">The number of new events that have been added</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError CheckFaultlogger(ref Int16 PassedNumOfFaults, ref UInt32 orig_new, ref UInt32 newEventsLogged)
        {
            // Set default values
            UInt32 OldestIndex = EMPTY_FAULT_BUFFER;
            UInt32 NewestIndex = EMPTY_FAULT_BUFFER;
            CommunicationError commError;
            UInt32 FaultIndex;

            // LOOP ONCE ... EXIT IMMEDIATELY ON ERROR
            do
            {
                // Disable Fault Logging
                commError = EnableFaultLogging(false);
                if (commError != CommunicationError.Success)
                {
                    break;
                }

                // Get the most recent Fault Log Indexes from the embedded target
                commError = GetFaultIndices(out OldestIndex, out NewestIndex);
                if (commError != CommunicationError.Success)
                {
                    break;
                }

                if (orig_new == EMPTY_FAULT_BUFFER)
                {
                    // The event log was empty when this method was invoked
                    FaultIndex = OldestIndex;
                }
                else
                {
                    FaultIndex = (UInt32)(orig_new + 1);
                }

                // Check if Fault Log is Empty
                if ((NewestIndex == EMPTY_FAULT_BUFFER) && (OldestIndex == EMPTY_FAULT_BUFFER))
                {
                    break;
                }

                // Handle index wrapping from 65535 to 0
                if (NewestIndex < (UInt32)(FaultIndex - 1))
                {
                    newEventsLogged = (UInt32)(NewestIndex - (short)(FaultIndex - 1));
                }
                else
                {
                    // Compute number of Faults; there may not be any in which case newEventsLogged = 0
                    newEventsLogged = (UInt32)(NewestIndex - FaultIndex + 1);
                }


                if (newEventsLogged == 0)
                {
                    break;
                }

                // If the code reaches this point without "breaking", new events have been recorded
                // by the embedded target. However, if RemoteFaults == PassedNumOfFaults, that means
                // the embedded target buffer is full and at least 1 old fault has been flushed. 
                // If the flag is true, then the faults at the beginning of the fault list will be removed
                Int16 totalRemoteFaults = (Int16)((NewestIndex - OldestIndex) + 1);


                // Get the newest fault information
                commError = GetFaultData((UInt32)(FaultIndex % 65536), (UInt16)newEventsLogged);
                
                // Verify the transaction was successful and that at least one fault was returned
                if (commError != CommunicationError.Success)
                {
                    break;
                }
                if (m_FaultDataFromTarget.BufferSize == 0)
                {
                    break;
                }

                // Re-enable Fault Logging
                commError = EnableFaultLogging(true);
                if (commError != CommunicationError.Success)
                {
                    break;
                }

                // Loop through the fault buffer, pulling out the size and data for each fault
                Int32 Index = 0;
                while (Index < m_FaultDataFromTarget.BufferSize)
                {
                    Int16 FaultSize;
                    // Get the size of the next fault
                    FaultSize = BitConverter.ToInt16(m_FaultDataFromTarget.Buffer, Index);

                    // Allocate jagged array dynamically and store fault data there
                    if (FaultSize < MAX_FAULT_SIZE_BYTES && FaultSize > 0)
                    {
                        try
                        {
                            Byte[] tempFaultBuffer = new Byte[FaultSize + 2];
                            // Copy all data into newly created array
                            Buffer.BlockCopy(m_FaultDataFromTarget.Buffer, Index, tempFaultBuffer, 0, FaultSize + 2);

                            // Add new member with size "FaultSize" to jagged 2 dimensional array (the "FaultSize" is also part of the fault data;
                            // thus the + 2)
                            m_FaultStorage.Add(tempFaultBuffer);                        
                        }
                        catch 
                        {
                            return CommunicationError.SystemException;
                        }

                        m_CurrentNumberOfFaults++;
                    }
                    else
                    {
                        // Fault Buffer is corrupt beyond hope at this point
                        commError = CommunicationError.UnknownError;
                        break;
                    }

                    // Increment the Index to point to the size of the next fault
                    Index += (FaultSize + 2);
                }

                while (m_FaultStorage.Count > totalRemoteFaults)
                {
                    m_FaultStorage.RemoveAt(0);
                    m_CurrentNumberOfFaults--;
                }
            
            } while (false);

            // Enable Fault Logging here in case we left the while loop early
            commError = EnableFaultLogging(true);


            // Update the reference parameters if all transactions went OK and at least one new fault was reecived 
            if ((commError == CommunicationError.Success) && (newEventsLogged > 0))
            {
                orig_new = NewestIndex;
                PassedNumOfFaults = m_CurrentNumberOfFaults;
            }

            return commError;
        }

        /// <summary>
        /// Method requests the embedded target to clear the currently all fault logs
        /// </summary>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError ClearEvent()
        {
            CommunicationError commError =
                m_PtuTargetCommunication.SendCommandToEmbedded(m_CommDevice, ProtocolPTU.PacketType.CLEAR_EVENTLOG);

            m_CurrentNumberOfFaults = 0;
            m_FaultStorage.Clear();

            return commError;
        }

        /// <summary>
        /// This method gets the default stream information associated with the event log. This includes the number of variables, the 
        /// number of samples, the sample rate and the stream variables' indexes and types.
        /// </summary>
        /// <param name="NumberOfVariables">the number of variables in the stream</param>
        /// <param name="NumberOfSamples">the number of data samples in the stream</param>
        /// <param name="SampleRate">the sample rate of the stream</param>
        /// <param name="VariableIndex">variable index array that is updated in this method</param>
        /// <param name="VariableType">variable type array that is updated in this method</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetDefaultStreamInformation(out Int16 NumberOfVariables, out Int16 NumberOfSamples, out Int16 SampleRate,
                                                               Int16[] VariableIndex, Int16[] VariableType)
        {

            // TODO: required to set values based on "out" interface which was not required when API was unmanaged; may need to revisit if errors occur
            NumberOfVariables = -1;
            NumberOfSamples = -1;
            SampleRate = -1;

            // Poll the embedded target for the information and verify the transaction was successful
            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, ProtocolPTU.PacketType.GET_DEFAULT_STREAM, m_RxMessage);
            if (commError != CommunicationError.Success)
            {
                return commError;
            }

            // Update the variables and reorder the bytes if necessary
            NumberOfVariables = BitConverter.ToInt16(m_RxMessage, 8);
            NumberOfSamples = BitConverter.ToInt16(m_RxMessage, 10);
            SampleRate = BitConverter.ToInt16(m_RxMessage, 12);
            if (m_CommDevice.IsTargetBigEndian())
            {
                NumberOfVariables = Utils.ReverseByteOrder(NumberOfVariables);
                NumberOfSamples = Utils.ReverseByteOrder(NumberOfSamples);
                SampleRate = Utils.ReverseByteOrder(SampleRate);
            }

            // Clamp the maximum number of variables allowed in the stream
            if (NumberOfVariables > MAX_DL_VARIABLES)
            {
                NumberOfVariables = MAX_DL_VARIABLES;
            }

            // Update the variable index and the variable type for all stream variables
            for (Int16 Counter = 0; Counter < NumberOfVariables; Counter++)
            {
                // multiply counter by 4 to account for 4 bytes required (2 bytes for index and 2 bytes for type)
                VariableIndex[Counter] = BitConverter.ToInt16(m_RxMessage, 14 + (Counter * 4));
                VariableType[Counter] = BitConverter.ToInt16(m_RxMessage, 16 + (Counter * 4));

                if (m_CommDevice.IsTargetBigEndian())
                {
                    VariableIndex[Counter] = Utils.ReverseByteOrder(VariableIndex[Counter]);
                    VariableType[Counter] = Utils.ReverseByteOrder(VariableType[Counter]);
                }
            }

            return CommunicationError.Success;
        }

        /// <summary>
        /// NOTE: This method is currently is unused but is implemented for completeness. 
        /// </summary>
        /// <param name="CurrentEventLog"></param>
        /// <param name="NumberEventLogs"></param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetEventLog(out Int16 CurrentEventLog, out Int16 NumberEventLogs)
        {
            // TODO: required to set values based on "out" interface which was not required when API was unmanaged; may need to revisit
            CurrentEventLog = -1;
            NumberEventLogs = -1;
            CommunicationError commError = m_PtuTargetCommunication.SendCommandToEmbedded(m_CommDevice, ProtocolPTU.PacketType.GET_EVENT_LOG);

            if (commError != CommunicationError.Success)
            {
                return commError;
            }

            CurrentEventLog = BitConverter.ToInt16(m_RxMessage, 8);
            NumberEventLogs = BitConverter.ToInt16(m_RxMessage, 10);

            if (m_CommDevice.IsTargetBigEndian())
            {
                CurrentEventLog = Utils.ReverseByteOrder(CurrentEventLog);
                NumberEventLogs = Utils.ReverseByteOrder(NumberEventLogs);
            }

            return commError;
        }


        /// <summary>
        /// This method is invoked for every event that is downloaded from the embedded target. It extracts all of the information
        /// from the event header, including the faudId, taskIdm data and time and the data log id (if any) associated with the event
        /// </summary>
        /// <param name="index">the index into the jagged array of the fault</param>
        /// <param name="faultnum">updated with the fault id that is contained in the header</param>
        /// <param name="tasknum">updated with the task number that is contained in the header</param>
        /// <param name="Flttime">updated with the time that the fault was logged that is contained in the header</param>
        /// <param name="Fltdate">updated with the date that the fault was logged that is contained in the header</param>
        /// <param name="datalognum">updated with the datalog number that is contained in the header</param>
        /// <param name="Use4DigitYearCode">true if four digit year (2 byte) code is expected from the VCU</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetFaultHdr(Int16 index, ref Int16 faultnum, ref Int16 tasknum,
                                              ref String Flttime, ref String Fltdate, ref Int16 datalognum, bool Use4DigitYearCode)
        {
            // Check the Validity of the desired index
            if (index >= m_CurrentNumberOfFaults)
            {
                Flttime = "N/A";
                Fltdate = "N/A";
                datalognum = -1;
                faultnum = 0;
                tasknum = 0;
                return CommunicationError.UnknownError;
            }

            const UInt16 DATE_OFFSET_IN_FAULT_LOG = 10;

            Byte hour = m_FaultStorage[index][DATE_OFFSET_IN_FAULT_LOG];
            Byte minute = m_FaultStorage[index][DATE_OFFSET_IN_FAULT_LOG + 1];
            Byte second = m_FaultStorage[index][DATE_OFFSET_IN_FAULT_LOG + 2];
            Byte month = m_FaultStorage[index][DATE_OFFSET_IN_FAULT_LOG + 3];
            Byte day = m_FaultStorage[index][DATE_OFFSET_IN_FAULT_LOG + 4];

            UInt16 fourDigitYear = 0;
            Byte twoDigitYear = 0;

            if (Use4DigitYearCode)
            {
                fourDigitYear = BitConverter.ToUInt16(m_FaultStorage[index], DATE_OFFSET_IN_FAULT_LOG + 5);
            }
            else
            {
                twoDigitYear = m_FaultStorage[index][DATE_OFFSET_IN_FAULT_LOG + 5];
            }

            if (m_CommDevice.IsTargetBigEndian())
            {
                hour = Utils.ReverseByteOrder(hour);
                minute = Utils.ReverseByteOrder(minute);
                second = Utils.ReverseByteOrder(second);
                month = Utils.ReverseByteOrder(month);
                day = Utils.ReverseByteOrder(day);
                if (Use4DigitYearCode)
                {
                    fourDigitYear = Utils.ReverseByteOrder(fourDigitYear);
                }
                else
                {
                    twoDigitYear = Utils.ReverseByteOrder(twoDigitYear);
                }
            }

            // Check Time
            if (VerifyTime(hour, minute, second))
            {
                Flttime = hour.ToString("D2") + ":" + minute.ToString("D2") + ":" + second.ToString("D2");
            }
            else
            {
                Flttime = "N/A";
            }


            if (Use4DigitYearCode)
            {
                // Check Date
                if (VerifyDate(month, day, fourDigitYear))
                {
                    Fltdate = month.ToString("D2") + "/" + day.ToString("D2") + "/" + fourDigitYear.ToString("D4");
                }
                else
                {
                    Fltdate = "N/A";
                }
            }
            else
            {
                // Check Date
                if (VerifyDate(month, day, twoDigitYear))
                {
                    Fltdate = month.ToString("D2") + "/" + day.ToString("D2") + "/" + twoDigitYear.ToString("D2");
                }
                else
                {
                    Fltdate = "N/A";
                }
            }

            faultnum = BitConverter.ToInt16(m_FaultStorage[index], 2);
            tasknum = BitConverter.ToInt16(m_FaultStorage[index], 4);

            if (Use4DigitYearCode)
            {
                datalognum = BitConverter.ToInt16(m_FaultStorage[index], 18);
            }
            else
            {
                datalognum = BitConverter.ToInt16(m_FaultStorage[index], 16);
            }

            if (m_CommDevice.IsTargetBigEndian())
            {
                faultnum = Utils.ReverseByteOrder(faultnum);
                tasknum = Utils.ReverseByteOrder(tasknum);
                datalognum = Utils.ReverseByteOrder(datalognum);
            }

            return CommunicationError.Success;
        }

        /// <summary>
        /// Method parses through the most recent downloaded fault logs and extracts the variable and variable types for the 
        /// request event (FaultIndex) and populates the VariableType and VariableValue arrays.
        /// </summary>
        /// <param name="FaultIndex">The index of the fault to be parsed</param>
        /// <param name="NumberOfVariables">The number of variables in the fault to be parsed</param>
        /// <param name="VariableType">Array that is populated with the variable type for each variable included in the fault</param>
        /// <param name="VariableValue">Array that is populated with the variable value for each variable included in the fault</param>
        /// <param name="Use4DigitYearCode">true if four digit year (2 byte) code is expected from the VCU</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetFaultVar(Int16 FaultIndex, Int16 NumberOfVariables, Int16[] VariableType, Double[] VariableValue,
                                              bool Use4DigitYearCode)
        {
            // Check the Validity of the desired index
            if (FaultIndex >= m_CurrentNumberOfFaults)
            {
                return CommunicationError.UnknownError;
            }

            // This is the starting offset in each fault where the variable types and values are stored
            UInt16 variableOffset = 18;
            if (Use4DigitYearCode)
            {
                // Time Stamp is 8 bytes long instead of 6, so the variable offset needs to be adjusted
                variableOffset = 20;
            }

            for (Int16 var = 0; var < NumberOfVariables; var++)
            {
                switch ((ProtocolPTU.VariableType)VariableType[var])
                {
                    case ProtocolPTU.VariableType.UINT_8_TYPE:
                        Byte bVal = m_FaultStorage[FaultIndex][variableOffset];
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            bVal = Utils.ReverseByteOrder(bVal);
                        }
                        VariableValue[var] = (Double)bVal;
                        variableOffset += sizeof(Byte);
                        break;

                    case ProtocolPTU.VariableType.INT_8_TYPE:
                        SByte signedByte = (SByte)m_FaultStorage[FaultIndex][variableOffset];
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            signedByte = Utils.ReverseByteOrder(signedByte);
                        }
                        VariableValue[var] = (Double)signedByte;
                        variableOffset += sizeof(Char);
                        break;

                    case ProtocolPTU.VariableType.UINT_16_TYPE:
                        UInt16 u16 = BitConverter.ToUInt16(m_FaultStorage[FaultIndex], variableOffset);
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            u16 = Utils.ReverseByteOrder(u16);
                        }
                        VariableValue[var] = (Double)u16;
                        variableOffset += sizeof(UInt16);
                        break;

                    case ProtocolPTU.VariableType.INT_16_TYPE:
                        Int16 i16 = BitConverter.ToInt16(m_FaultStorage[FaultIndex], variableOffset);
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            i16 = Utils.ReverseByteOrder(i16);
                        }
                        VariableValue[var] = (Double)i16;
                        variableOffset += sizeof(Int16);
                        break;

                    case ProtocolPTU.VariableType.UINT_32_TYPE:
                        UInt32 u32 = BitConverter.ToUInt32(m_FaultStorage[FaultIndex], variableOffset);
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            u32 = Utils.ReverseByteOrder(u32);
                        }
                        VariableValue[var] = (Double)u32;
                        variableOffset += sizeof(UInt32);
                        break;

                    case ProtocolPTU.VariableType.INT_32_TYPE:
                        Int32 i32 = BitConverter.ToInt32(m_FaultStorage[FaultIndex], variableOffset);
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            i32 = Utils.ReverseByteOrder(i32);
                        }
                        VariableValue[var] = (Double)i32;
                        variableOffset += sizeof(Int32);
                        break;

                    default:
                        return CommunicationError.UnknownError;
                }
            }

            return CommunicationError.Success;
        }

        /// <summary>
        /// Get the status of the flags that control: (a) whether the event type is enabled and (b) whether the event type triggers the recoding of a
        /// data stream. 
        /// </summary>
        /// <param name="Valid">An array of flags that define which of the available event types are valid for the current log. The total length
        /// of the array is the maximum number of events per task multiplied by the maximum number of tasks and the array element corresponding to a
        /// particular event type is defined as: {task identifier} * {maximum number of events per task} + {event identifier}. True, indicates that
        /// the event type is valid; otherwise, false.</param>
        /// <param name="EnableFlag">An array of flags that indicate whether the event type is enabled. True, indicates that the event type is
        /// enabled; otherwise, false.</param>
        /// <param name="TriggerFlag">An array of flags that indicate whether the event type triggers the recording of a data stream.
        /// True, indicates that the event type triggers the recording of a data stream; otherwise false.</param>
        /// <param name="EntryCount">The maximum number of event types i.e. the maximum number of event types per task multiplied by the maximum
        /// number of tasks.</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetFltFlagInfo(Int16[] Valid, Int16[] EnableFlag, Int16[] TriggerFlag, Int16 EntryCount)
        {
            Byte[] message1 = new Byte[2048];

            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, ProtocolPTU.PacketType.GET_FAULT_FLAG, message1);
            if (commError != CommunicationError.Success)
            {
                return commError;
            }

            Int16 numberOfWords = BitConverter.ToInt16(message1, 8);
            if (m_CommDevice.IsTargetBigEndian())
            {
                numberOfWords = Utils.ReverseByteOrder(numberOfWords);
            }
            numberOfWords /= 2;

            Byte[] message2 = new Byte[2048];
            commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, ProtocolPTU.PacketType.GET_STREAM_FLAG, message2);
            if (commError != CommunicationError.Success)
            {
                return commError;
            }

            // Loop through all the TaskId/FaultId Combinations and set/reset a bit for each one
            UInt16 mask = 0x0001;
            Int16 counter = 0;
            for (Int16 numberOfEntries = 0; numberOfEntries < EntryCount; numberOfEntries++)
            {
                Int16 index = (Int16)(numberOfEntries / 16);
                UInt16 enableFlag = BitConverter.ToUInt16(message1, 10 + (index * 2));
                UInt16 datalogFlag = BitConverter.ToUInt16(message2, 10 + (index * 2));

                if (m_CommDevice.IsTargetBigEndian())
                {
                    enableFlag = Utils.ReverseByteOrder(enableFlag);
                    datalogFlag = Utils.ReverseByteOrder(datalogFlag);
                }

                if ((index < numberOfWords) && (Valid[numberOfEntries] != 0))
                {
                    EnableFlag[counter] = (Int16)(((enableFlag & mask) != 0) ? 1 : 0);
                    TriggerFlag[counter] = (Int16)(((datalogFlag & mask) != 0) ? 1 : 0);
                    counter++;
                }

                if (mask == 0x8000)
                {
                    mask = 0x0001;
                }
                else
                {
                    mask = (UInt16)(mask << 1);
                }
            }

            return commError;
        }

        /// <summary>
        /// Get the event history associated with the current log.
        /// </summary>
        /// <param name="Valid">An array of flags that define which of the available event types are valid for the current log. The total length
        /// of the array is the maximum number of events per task multiplied by the maximum number of tasks and the array element corresponding to a
        /// particular event type is defined as: {task identifier} * {maximum number of events per task} + {event identifier}. True, indicates that
        /// the event type is valid; otherwise, false.</param>
        /// <param name="CumulativeHistoryCounts">An array that contains the cumulative number of events of each event type, not including recent
        /// history.</param>
        /// <param name="RecentHistoryCounts">An array that contains the recent number of events of each event type.</param>
        /// <param name="MaxTasks">The maximum number of tasks that are supported by the current event log.</param>
        /// <param name="MaxEventsPerTask">The maximum number of events per task that are supported by the current event log.</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetFltHistInfo(Int16[] Valid, Int16[] CumulativeHistoryCounts, Int16[] RecentHistoryCounts,
                                                 Int16 MaxTasks, Int16 MaxEventsPerTask)
        {
            Int16 NumberOfEntries = 0;

            // Loop through all the legal TaskId/FaultId Combinations and pull the histories for each combination
            for (Int16 TaskId = 0; TaskId < MaxTasks; TaskId++)
            {
                for (Int16 EventId = 0; EventId < MaxEventsPerTask; EventId++)
                {
                    if (Valid[(TaskId * MaxEventsPerTask) + EventId] != 0)
                    {
                        ProtocolPTU.GetFaultHistoryReq request = new ProtocolPTU.GetFaultHistoryReq((UInt16)TaskId, (UInt16)EventId);

                        CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, request, m_RxMessage);

                        if (commError != CommunicationError.Success)
                        {
                            return commError;
                        }

                        CumulativeHistoryCounts[NumberOfEntries] = BitConverter.ToInt16(m_RxMessage, 8);
                        RecentHistoryCounts[NumberOfEntries] = BitConverter.ToInt16(m_RxMessage, 10);
                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            CumulativeHistoryCounts[NumberOfEntries] = Utils.ReverseByteOrder(CumulativeHistoryCounts[NumberOfEntries]);
                            RecentHistoryCounts[NumberOfEntries] = Utils.ReverseByteOrder(RecentHistoryCounts[NumberOfEntries]);
                        }
                        NumberOfEntries++;
                    }
                }
            }

            return CommunicationError.Success;
        }

        /// <summary>
        /// This method gets the stream data associated with an event.
        /// </summary>
        /// <param name="StreamNumber">the stream number to be retrieved from the embedded target</param>
        /// <param name="DatalogBuffer">used to store the variable value </param>
        /// <param name="TimeOrigin">currently unused by the application</param>
        /// <param name="NumberOfVariables">the number of variables in the stream</param>
        /// <param name="NumberOfSamples">the number of time samples of each variable in the stream</param>
        /// <param name="VariableType">Contains the variable types of each of the stream variables</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetStream(Int16 StreamNumber, Int32[] DatalogBuffer, out Int16 TimeOrigin,
                                            Int16 NumberOfVariables, Int16 NumberOfSamples, Int16[] VariableType)
        {
            TimeOrigin = -1;

            ProtocolPTU.GetDatalogBufferReq request = new ProtocolPTU.GetDatalogBufferReq((UInt16)StreamNumber);

            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, request, m_RxMessage);

            if (commError != CommunicationError.Success)
            {
                return commError;
            }

            TimeOrigin = BitConverter.ToInt16(m_RxMessage, 8);
            UInt16 sourceSize = BitConverter.ToUInt16(m_RxMessage, 10);

            if (m_CommDevice.IsTargetBigEndian())
            {
                TimeOrigin = Utils.ReverseByteOrder(TimeOrigin);
                sourceSize = Utils.ReverseByteOrder(sourceSize);
            }

            // Initialize Counters
            UInt16 byteCount = 12;
            UInt16 destCount = 0;

            // Loop through Source Buffer
            while (byteCount < sourceSize)
            {
                // Loop through the variables
                for (UInt16 Index = 0; Index < (UInt16)NumberOfVariables; Index++)
                {
                    // Make sure we don't go over destination buffer limits
                    if (destCount >= NumberOfSamples * NumberOfVariables)
                    {
                        return CommunicationError.BadResponse;
                    }

                    // Grab number of bytes depending on variable type
                    switch ((ProtocolPTU.VariableType)VariableType[Index])
                    {
                        case ProtocolPTU.VariableType.INT_8_TYPE:
                            SByte i8 = (SByte)m_RxMessage[byteCount];
                            if (m_CommDevice.IsTargetBigEndian())
                            {
                                i8 = Utils.ReverseByteOrder(i8);
                            }
                            DatalogBuffer[destCount++] = (Int32)i8;
                            byteCount++;
                            break;

                        case ProtocolPTU.VariableType.UINT_8_TYPE:
                            Byte u8 = (Byte)m_RxMessage[byteCount];
                            if (m_CommDevice.IsTargetBigEndian())
                            {
                                u8 = Utils.ReverseByteOrder(u8);
                            }
                            DatalogBuffer[destCount++] = (Int32)u8;
                            byteCount++;
                            break;

                        case ProtocolPTU.VariableType.INT_16_TYPE:
                            Int16 i16 = BitConverter.ToInt16(m_RxMessage, byteCount);
                            if (m_CommDevice.IsTargetBigEndian())
                            {
                                i16 = Utils.ReverseByteOrder(i16);
                            }
                            DatalogBuffer[destCount++] = (Int32)i16;
                            byteCount += 2;
                            break;

                        case ProtocolPTU.VariableType.UINT_16_TYPE:
                            UInt16 u16 = BitConverter.ToUInt16(m_RxMessage, byteCount);
                            if (m_CommDevice.IsTargetBigEndian())
                            {
                                u16 = Utils.ReverseByteOrder(u16);
                            }
                            DatalogBuffer[destCount++] = (Int32)u16;
                            byteCount += 2;
                            break;

                        case ProtocolPTU.VariableType.INT_32_TYPE:
                            Int32 i32 = BitConverter.ToInt32(m_RxMessage, byteCount);
                            if (m_CommDevice.IsTargetBigEndian())
                            {
                                i32 = Utils.ReverseByteOrder(i32);
                            }
                            DatalogBuffer[destCount++] = i32;
                            byteCount += 4;
                            break;

                        case ProtocolPTU.VariableType.UINT_32_TYPE:
                            UInt32 u32 = BitConverter.ToUInt32(m_RxMessage, byteCount);
                            if (m_CommDevice.IsTargetBigEndian())
                            {
                                u32 = Utils.ReverseByteOrder(u32);
                            }
                            DatalogBuffer[destCount++] = (Int32)u32;
                            byteCount += 4;
                            break;

                        default:
                            return CommunicationError.BadResponse;
                    }
                }
                // TODO DAS:Not sure why this is here and just bump the counter up by 4 for all data sizes
                // Account for left over bytes
                if ((byteCount % 4) != 0)
                {
                    byteCount += (UInt16)(4 - (byteCount % 4));
                }
            }

            return CommunicationError.Success;
        }

        /// <summary>
        /// Get the parameters associated with the specified stream.
        /// </summary>
        /// <param name="StreamNumber">The stream number.</param>
        /// <param name="NumberOfVariables">The number of watch variables included in the data stream.</param>
        /// <param name="NumberOfSamples">The number of data samples associated with the data stream.</param>
        /// <param name="SampleRate">The multiple of the base recording interval (60ms) at which the data is sampled.</param>
        /// <param name="VariableIndex">The watch identifiers of the watch variables contained within the data stream.</param>
        /// <param name="VariableType">The data type corresponding to each watch variable contained within the data stream.</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError GetStreamInformation(Int16 StreamNumber, out Int16 NumberOfVariables, out Int16 NumberOfSamples,
                                                        out Int16 SampleRate, Int16[] VariableIndex, Int16[] VariableType)
        {
            NumberOfVariables = -1;
            NumberOfSamples = -1;
            SampleRate = -1;

            ProtocolPTU.GetStreamInfoReq request = new ProtocolPTU.GetStreamInfoReq(StreamNumber);
            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, request, m_RxMessage);

            if (commError != CommunicationError.Success)
            {
                return commError;
            }

            NumberOfVariables = BitConverter.ToInt16(m_RxMessage, 8);
            NumberOfSamples = BitConverter.ToInt16(m_RxMessage, 10);
            SampleRate = BitConverter.ToInt16(m_RxMessage, 12);

            if (m_CommDevice.IsTargetBigEndian())
            {
                NumberOfVariables = Utils.ReverseByteOrder(NumberOfVariables);
                NumberOfSamples = Utils.ReverseByteOrder(NumberOfSamples);
                SampleRate = Utils.ReverseByteOrder(SampleRate);
            }

            if (NumberOfVariables > VariableIndex.Length)
            {
                NumberOfVariables = (Int16)VariableIndex.Length;
            }
            else if (NumberOfVariables > MAX_DL_VARIABLES)
            {
                NumberOfVariables = MAX_DL_VARIABLES;
            }

            for (UInt16 Counter = 0; Counter < NumberOfVariables; Counter++)
            {
                VariableIndex[Counter] = BitConverter.ToInt16(m_RxMessage, (Counter * 4) + 14);
                VariableType[Counter] = BitConverter.ToInt16(m_RxMessage, (Counter * 4) + 16);
                if (m_CommDevice.IsTargetBigEndian())
                {
                    VariableIndex[Counter] = Utils.ReverseByteOrder(VariableIndex[Counter]);
                    VariableType[Counter] = Utils.ReverseByteOrder(VariableType[Counter]);
                }
            }

            return commError;
        }

        /// <summary>
        /// Initialize the event log. Clears all event information stored on battery backed RAM for both the maintenance and engineering logs. This
        /// also clears both the cumulative history, recent history columns and all data logs. This function is typically used to establish a zero
        /// event/fault reference base when a replacement VCU is installed in a car.
        /// </summary>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError InitializeEventLog()
        {
            CommunicationError commError = m_PtuTargetCommunication.SendCommandToEmbedded(m_CommDevice, ProtocolPTU.PacketType.INITIALIZE_EVENTLOG);

            m_CurrentNumberOfFaults = 0;
            m_FaultStorage.Clear();

            return commError;
        }

        /// <summary>
        /// This method gets all of the logged faults/events from the embedded target's current event log. 
        /// </summary>
        /// <param name="NumberOfFaults">the number of events that are currently stored in the embedded target NVRAM (updated from embedded target response)</param>
        /// <param name="OldestIndex">the oldest fault index that has been logged (updated from embedded target response)</param>
        /// <param name="NewestIndex">the newest fault index that has been logged (updated from embedded target response)</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError LoadFaultLog(out Int16 NumberOfFaults, out UInt32 OldestIndex, out UInt32 NewestIndex)
        {
            // NOTE: These default values need to be set because of the interface call... may have to revisit
            NumberOfFaults = Int16.MaxValue;
            OldestIndex = EMPTY_FAULT_BUFFER;
            NewestIndex = EMPTY_FAULT_BUFFER;

            CommunicationError commError;

            // LOOP ONCE ... EXIT ON ERROR
            do
            {
                // Reset the current number of faults
                m_CurrentNumberOfFaults = 0;
                m_FaultStorage.Clear();

                // temporarily disable Fault Logging
                commError = EnableFaultLogging(false);
                if (commError != CommunicationError.Success)
                {
                    break;
                }

                // Get Fault Log Indexes from the embedded target
                commError = GetFaultIndices(out OldestIndex, out NewestIndex);
                if (commError != CommunicationError.Success)
                {
                    break;
                }

                // Check if the Fault Log is Empty (no faults have been logged)
                if ((OldestIndex == EMPTY_FAULT_BUFFER) && (NewestIndex == EMPTY_FAULT_BUFFER))
                {
                    NumberOfFaults = 0;
                    break;
                }

                UInt32 remoteFaults;
                if (NewestIndex < OldestIndex)
                {
                    remoteFaults = 0x10000 + NewestIndex - OldestIndex + 1;
                }
                else
                {
                    remoteFaults = NewestIndex - OldestIndex + 1;
                }

                if (remoteFaults == 0)
                {
                    break;
                }

                UInt32 faultCounter = 0;
                // GetFaultData() can only get a max of MAXFAULTBUFFERSIZE bytes of data. So if there are
                // more than faults in the Fault Log than this loop has to iterate several times to get all 
                // of the fault data
                do
                {
                    // Get as many faults as the response buffer size allows
                    commError = GetFaultData((OldestIndex + faultCounter) & 0xFFFF, (UInt16)(remoteFaults - faultCounter));
                    if (commError != CommunicationError.Success)
                    {
                        break;
                    }

                    if (m_FaultDataFromTarget.BufferSize == 0)
                    {
                        break;
                    }

                    // Loop through the fault buffer, pulling out the size and data for each fault
                    for (Int32 index = 0; index < m_FaultDataFromTarget.BufferSize; )
                    {
                        faultCounter++;

                        // Get the size of the next fault
                        Int16 faultSize = BitConverter.ToInt16(m_FaultDataFromTarget.Buffer, index);

                        if (m_CommDevice.IsTargetBigEndian())
                        {
                            faultSize = Utils.ReverseByteOrder(faultSize);
                        }

                        // Allocate jagged array dynamically and store fault data there
                        if (faultSize < MAX_FAULT_SIZE_BYTES && faultSize > 0)
                        {
                            Byte[] tempFaultBufffer = new Byte[faultSize + 2]; 

                            // Copy all data into newly created array
                            Buffer.BlockCopy(m_FaultDataFromTarget.Buffer, index, tempFaultBufffer, 0, faultSize + 2);

                            // Add new member with size "FaultSize" to jagged 2 dimensional array
                            m_FaultStorage.Add(tempFaultBufffer);

                            m_CurrentNumberOfFaults++;
                        }
                        else
                        {
                            // Fault Buffer is corrupt beyond hope at this point
                            commError = CommunicationError.UnknownError;
                            break;
                        }

                        // Increment the Index to point to the size of the next fault
                        index += (faultSize + 2);
                    }
                } while ((faultCounter < remoteFaults) && (commError != CommunicationError.UnknownError));

                // Force the Return Code so we can extract all valid faults
                commError = CommunicationError.Success;

                // Save the number of good faults we retrieved
                NumberOfFaults = m_CurrentNumberOfFaults;
            } while (false);

            // Enable Fault Logging here in case we left the while loop early
            EnableFaultLogging(true);

            return commError;
        }

        /// <summary>
        /// Updates the default stream information by setting what variables are to be monitored and the multiple of the
        /// embedded target sample rate.
        /// </summary>
        /// <param name="NumberOfVariables">the number of variables to update</param>
        /// <param name="SampleRate">a multiple of the embedded target sample rate </param>
        /// <param name="VariableIndex">an array of the watch variable indexes to update</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError SetDefaultStreamInformation(Int16 NumberOfVariables, Int16 SampleRate, Int16[] VariableIndex)
        {
            // Clamp he number of stream variables
            if (NumberOfVariables > MAX_DL_VARIABLES)
            {
                NumberOfVariables = MAX_DL_VARIABLES;
            }

            ProtocolPTU.SetStreamInfoReq request = new ProtocolPTU.SetStreamInfoReq(NumberOfVariables, SampleRate, VariableIndex);

            CommunicationError commError = m_PtuTargetCommunication.SendCommandToEmbedded(m_CommDevice, request);

            return commError;
        }

        /// <summary>
        /// Set the flag that controls: (a) whether the specified event type is enabled and (b) whether the event type triggers the recoding of a data
        /// stream. 
        /// </summary>
        /// <param name="TaskNumber">The task identifier associated with the event type.</param>
        /// <param name="FaultNumber">The event identifier associated with the event type.</param>
        /// <param name="EnableFlag">A flag to control whether the event type corresponding to the specified task and event identifiers is to be
        /// enabled. True, if the event type is to be enabled; otherwise, false.</param>
        /// <param name="DatalogFlag">A flag to control whether the event type corresponding to the specified task and event identifiers is to
        /// trigger the recording of a data stream. True, if the event type is to trigger the recording of a data stream; otherwise, false.</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public CommunicationError SetFaultFlags(Int16 TaskNumber, Int16 FaultNumber, Int16 EnableFlag, Int16 DatalogFlag)
        {
            ProtocolPTU.SetFaultFlagReq request = new ProtocolPTU.SetFaultFlagReq(TaskNumber, FaultNumber, EnableFlag, DatalogFlag);

            CommunicationError commError = m_PtuTargetCommunication.SendCommandToEmbedded(m_CommDevice, request);

            return commError;
        }


        #endregion --- Public Methods ---

        #region --- Private Methods ---

        /// <summary>
        /// This method attempts to get all of the logged faults and subsequent attached fault log data from the 
        /// embedded target. If successful, all of the fault data is copied from the 
        /// </summary>
        /// <param name="FaultIndex">The starting fault index to retrieve faults from the embedded target</param>
        /// <param name="NumberOfFaults">The amount of faults to retrieve.</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        private CommunicationError GetFaultData(UInt32 FaultIndex, UInt16 NumberOfFaults)
        {
            ProtocolPTU.GetFaultDataReq request = new ProtocolPTU.GetFaultDataReq(FaultIndex, NumberOfFaults);

            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, request, m_RxMessage);

            if (commError == CommunicationError.Success)
            {
                // Get the amount of data in the return buffer from the embedded target. NOTE: Due to buffer 
                // size limitations, all of the existing faults on the embedded target may be sent in more than 
                // packet
                m_FaultDataFromTarget.BufferSize = BitConverter.ToUInt16(m_RxMessage, 8);
                if (m_CommDevice.IsTargetBigEndian())
                {
                    m_FaultDataFromTarget.BufferSize = Utils.ReverseByteOrder(m_FaultDataFromTarget.BufferSize);
                }

                // Too much fault data to process
                if (m_FaultDataFromTarget.BufferSize > MAX_FAULT_BUFFER_SIZE)
                {
                    return CommunicationError.BadResponse;
                }

                // No fault data to process, but thats OK
                if (m_FaultDataFromTarget.BufferSize == 0)
                {
                    return CommunicationError.Success;
                }

                m_FaultDataFromTarget.Buffer = new Byte[m_FaultDataFromTarget.BufferSize];

                // Copy the entire response into the fault data buffer for processing by calling method. "10" is the 
                // offset of the actual data (header is 8 bytes in length and the 2 following bytes are the message size)
                //               FROM ----------> TO-----
                Buffer.BlockCopy(m_RxMessage, 10, m_FaultDataFromTarget.Buffer, 0, m_FaultDataFromTarget.BufferSize);
            }

            return commError;
        }


        /// <summary>
        /// Method responsible for getting the fault indexes of the oldest and newest faults
        /// </summary>
        /// <param name="Oldest">the index of the oldest fault logged on the embedded target</param>
        /// <param name="Newest">the index of the newest fault logged on the embedded target</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        private CommunicationError GetFaultIndices(out UInt32 Oldest, out UInt32 Newest)
        {
            // Set to empty in case of a communication fault
            Oldest = EMPTY_FAULT_BUFFER;
            Newest = EMPTY_FAULT_BUFFER;

            CommunicationError commError = m_PtuTargetCommunication.SendDataRequestToEmbedded(m_CommDevice, ProtocolPTU.PacketType.GET_FAULT_INDICES, m_RxMessage);

            // Get the oldest and newest faults and set the arguments
            if (commError == CommunicationError.Success)
            {
                Newest = BitConverter.ToUInt32(m_RxMessage, 8);
                Oldest = BitConverter.ToUInt32(m_RxMessage, 12);

                if (m_CommDevice.IsTargetBigEndian())
                {
                    Newest = Utils.ReverseByteOrder(Newest);
                    Oldest = Utils.ReverseByteOrder(Oldest);
                }
            }

            return commError;
        }

        /// <summary>
        /// Method that sets/resets the ability for the current fault log on the embedded target to log faults. Fault/event log
        /// is typically disabled for a short period of time whenever fault logs are being downloaded or whenever
        /// polling occurs to determine if any new faults have been logged.
        /// </summary>
        /// <param name="enable">true to enable fault logging; false to disable fault logging</param>
        /// <returns>CommunicationError.Success (0) if all is well; otherwise another enumeration which is less than 0</returns>
        private CommunicationError EnableFaultLogging(Boolean enable)
        {
            Byte faultLogEnable = (Byte)((enable == true) ? 1 : 0);

            ProtocolPTU.EnableFaultLoggingReq request = new ProtocolPTU.EnableFaultLoggingReq(faultLogEnable);

            CommunicationError commError = m_PtuTargetCommunication.SendCommandToEmbedded(m_CommDevice, request);

            return commError;
        }

        /// <summary>
        /// Verifies Date parameters are within expected limits.
        /// </summary>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <param name="year">Year</param>
        /// <returns>false if any of the date parameters passed into his function are not within the expected criteria</returns>
        private Boolean VerifyDate(Byte month, Byte day, Byte year)
        {
            if ((month < 1) || (month > 12))
            {
                return false;
            }

            if ((day < 1) || (day > 31))
            {
                return false;
            }

            if ((year < 0) || (year > 99))
            {
                return false;
            }

            // All is well with passed arguments
            return true;
        }


        /// <summary>
        /// Verifies Date parameters are within expected limits.
        /// </summary>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <param name="year">Year</param>
        /// <returns>false if any of the date parameters passed into his function are not within the expected criteria</returns>
        private Boolean VerifyDate(Byte month, Byte day, UInt16 year)
        {
            if ((month < 1) || (month > 12))
            {
                return false;
            }

            if ((day < 1) || (day > 31))
            {
                return false;
            }

            if ((year < 1970) || (year > 2106))
            {
                return false;
            }

            // All is well with passed arguments
            return true;
        }



        /// <summary>
        /// Verifies time parameters are within expected limits. NOTE: any checks for less than 0 are superfluous
        /// because Byte is an unsigned entity
        /// </summary>
        /// <param name="hr">Hours</param>
        /// <param name="min">Minutes</param>
        /// <param name="sec">Seconds</param>
        /// <returns>false if any of the time parameters passed into his function are not within the expected criteria</returns>
        private Boolean VerifyTime(Byte hr, Byte min, Byte sec)
        {
            if ((hr < 0) || (hr > 23))
            {
                return false;
            }

            if ((min < 0) || (min > 59))
            {
                return false;
            }

            if ((sec < 0) || (sec > 59))
            {
                return false;
            }

            // All is well with passed arguments
            return true;
        }

        #endregion --- Private Methods ---

        #endregion --- Methods ---
    }
}
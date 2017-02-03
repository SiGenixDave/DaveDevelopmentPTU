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
 *  File name:  CreateIelfCsv.cs
 *
 *  Revision History
 *  ----------------
 *
 *  Date        Version Author      Comments
 *  12/01/2016  1.0     D.Smail     First Release.
 *
 *
 */

#endregion --- Revision History ---

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

#if !FILE_READ_FROM_PC

using VcuComm;

#endif

namespace FTPDownloadRTDM
{
    /// <summary>
    /// This class is used to convert the downloaded binary IELF file to a CSV file.
    /// </summary>
    internal class CreateIelfCsv
    {
        #region --- Enumerations ---

        #endregion --- Enumerations ---

        #region --- Constructors ----

        #endregion --- Constructors ----

        #region --- Internal Classes ---

        /// <summary>
        /// This contains the data members of the header structure. It is imperative that
        /// the class attributes has "Layout.Sequential". The code that generates the CSV file
        /// uses the order to properly align the column data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class Header
        {
            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte[] Version = new byte[4];

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte SystemId;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public ushort NumberOfRecords;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public short FirstRecordIndex;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public short LastRecordIndex;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public uint TimeOfLastReset;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte ReasonForReset;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public uint Reserved;
        }

        /// <summary>
        /// This contains the data members of each event counter structure. Each member contains
        /// the amount of times and other information about how many times an event has occurred.
        /// It is imperative that  the class attributes has "Layout.Sequential". The code that \
        /// generates the CSV file uses the order to properly align the column data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class EventCounter
        {
            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte SubSystemId;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public ushort EventId;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public ushort EventCount;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte OverflowFlag;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte RateLimitFlag;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte[] Reserved = new byte[3];
        }

        /// <summary>
        /// This contains the data members of each event record structure. It is imperative that
        /// the class attributes has "Layout.Sequential". The code that generates the CSV file
        /// uses the order to properly align the column data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class EventRecord
        {
            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public uint FailureBeginning;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public uint FailureEnd;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte SubSystemId;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public ushort EventId;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte TimeInaccurate;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte DSTFlag;

            /// <summary>
            /// See description in 071-ICD-004 for details
            /// </summary>
            public byte[] Reserved = new byte[2];
        }

        #endregion --- Internal Classes ---

        #region --- Constants ---

        /// <summary>
        /// The max number of unique events that can be logged.
        /// </summary>
        private const UInt32 NUM_EVENTS = 1024;

        /// <summary>
        /// The max number of event records in the IELF file. A record occurs when an event is logged. Note:
        /// An event will not be logged when the daily event count for any event exceeds the maximum allowed
        /// for any given day. However, the event counter for that event will still be incremented.
        /// </summary>
        private const UInt32 NUM_EVENT_RECORDS = 2100;

        #endregion --- Constants ---

        #region --- Member Variables ---

        /// <summary>
        ///  Stores all the header read from the IELF binary file
        /// </summary>
        private Header m_Header = new Header();

        /// <summary>
        ///  Stores all of the event counters read from the IELF binary file
        /// </summary>
        private EventCounter[] m_EventCounters = new EventCounter[NUM_EVENTS];

        /// <summary>
        /// Stores all of the event records read from the IELF binary file
        /// </summary>
        private EventRecord[] m_EventRecords = new EventRecord[NUM_EVENT_RECORDS];

        #endregion --- Member Variables ---

        #region --- Methods ---

        /// <summary>
        /// This method opens the IELF binary file. It then reads all of the data from the file and
        /// "maps" it to the appropriate data structure. It then converts and writes this data to
        /// a CSV file.
        /// </summary>
        /// <param name="binaryFileName">The full name of the IELF binary file downloaded from the VCU</param>
        /// <returns>error message - "null" if no errors were detected while reading binary IELF and creating CSV file</returns>
        public string CreateCSVFile(string binaryFileName)
        {
            string errorMessage = null;
            using (BinaryReader binFile = new BinaryReader(File.Open(binaryFileName, FileMode.Open)))
            {
                try
                {
                    // Read the header
                    GetHeader(binFile);
                    // Read the event counters
                    GetEventCounters(binFile);
                    // Read the event records
                    GetEventRecords(binFile);
                }
                catch (Exception e)
                {
                    // File corrupt or not properly sized
                    errorMessage = e.Message;
                }
            }

            // Verify no errors exist when reading the IELF binary file
            if (errorMessage == null)
            {
                // Create a new file text (CSV) file based on the original name of the binary file. Just remove the
                // .flt extension and replace it with .csv
                string csvName = binaryFileName.Replace(".flt", ".csv");
                using (StreamWriter csvFile = new StreamWriter(csvName))
                {
                    try
                    {
                        // Write the header to the CSV file
                        WriterHeader(csvFile);
                        // Write the event counters to the CSV file
                        WriteEventCounters(csvFile);
                        // Write the event records to the CSV file
                        WriteEventRecords(csvFile);
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return errorMessage;
        }

        /// <summary>
        /// Retrieves the header information from the IELF binary file and stores it in the
        /// appropriate fields of the header object.
        /// </summary>
        /// <param name="binFile">the IELF binary file downloaded from the VCU</param>
        private void GetHeader(BinaryReader binFile)
        {
            // Populate the header object by reading the data sequentially from the IELF file
            m_Header.Version = binFile.ReadBytes(4);
            m_Header.SystemId = binFile.ReadByte();
            m_Header.NumberOfRecords = binFile.ReadUInt16();
            m_Header.FirstRecordIndex = binFile.ReadInt16();
            m_Header.LastRecordIndex = binFile.ReadInt16();
            m_Header.TimeOfLastReset = binFile.ReadUInt32();
            m_Header.ReasonForReset = binFile.ReadByte();
            m_Header.Reserved = binFile.ReadUInt32();
#if !FILE_READ_FROM_PC
            m_Header.SystemId = Utils.ReverseByteOrder(m_Header.SystemId);
            m_Header.NumberOfRecords = Utils.ReverseByteOrder(m_Header.NumberOfRecords);
            m_Header.FirstRecordIndex = Utils.ReverseByteOrder(m_Header.FirstRecordIndex);
            m_Header.LastRecordIndex = Utils.ReverseByteOrder(m_Header.LastRecordIndex);
            m_Header.TimeOfLastReset = Utils.ReverseByteOrder(m_Header.TimeOfLastReset);
            m_Header.ReasonForReset = Utils.ReverseByteOrder(m_Header.ReasonForReset);
            m_Header.Reserved = Utils.ReverseByteOrder(m_Header.Reserved);
#endif
        }

        /// <summary>
        /// Retrieves each event counter data structure from the binary IELF file and
        /// stores this information in the appropriate fields in each event counter object.
        /// </summary>
        /// <param name="binFile">the IELF binary file downloaded from the VCU</param>
        private void GetEventCounters(BinaryReader binFile)
        {
            for (uint i = 0; i < NUM_EVENTS; i++)
            {
                m_EventCounters[i] = new EventCounter();

                // Populate the each event counter object by reading the data sequentially from the IELF file
                m_EventCounters[i].SubSystemId = binFile.ReadByte();
                m_EventCounters[i].EventId = binFile.ReadUInt16();
                m_EventCounters[i].EventCount = binFile.ReadUInt16();
                m_EventCounters[i].OverflowFlag = binFile.ReadByte();
                m_EventCounters[i].RateLimitFlag = binFile.ReadByte();
                m_EventCounters[i].Reserved = binFile.ReadBytes(3);

#if !FILE_READ_FROM_PC
                m_EventCounters[i].SubSystemId = Utils.ReverseByteOrder(m_EventCounters[i].SubSystemId);
                m_EventCounters[i].EventId = Utils.ReverseByteOrder(m_EventCounters[i].EventId);
                m_EventCounters[i].EventCount = Utils.ReverseByteOrder(m_EventCounters[i].EventCount);
                m_EventCounters[i].OverflowFlag = Utils.ReverseByteOrder(m_EventCounters[i].OverflowFlag);
                m_EventCounters[i].RateLimitFlag = Utils.ReverseByteOrder(m_EventCounters[i].RateLimitFlag);
#endif
            }
        }

        /// <summary>
        /// Retrieves each event record data structure from the binary IELF file and
        /// stores this information in the appropriate fields in each event record object.
        /// </summary>
        /// <param name="binFile">the IELF binary file downloaded from the VCU</param>
        private void GetEventRecords(BinaryReader binFile)
        {
            for (uint i = 0; i < NUM_EVENT_RECORDS; i++)
            {
                m_EventRecords[i] = new EventRecord();

                // Populate the each event record object by reading the data sequentially from the IELF file
                m_EventRecords[i].FailureBeginning = binFile.ReadUInt32();
                m_EventRecords[i].FailureEnd = binFile.ReadUInt32();
                m_EventRecords[i].SubSystemId = binFile.ReadByte();
                m_EventRecords[i].EventId = binFile.ReadUInt16();
                m_EventRecords[i].TimeInaccurate = binFile.ReadByte();
                m_EventRecords[i].DSTFlag = binFile.ReadByte();
                m_EventRecords[i].Reserved = binFile.ReadBytes(2);

#if !FILE_READ_FROM_PC
                m_EventRecords[i].FailureBeginning = Utils.ReverseByteOrder(m_EventRecords[i].FailureBeginning);
                m_EventRecords[i].FailureEnd = Utils.ReverseByteOrder(m_EventRecords[i].FailureEnd);
                m_EventRecords[i].SubSystemId = Utils.ReverseByteOrder(m_EventRecords[i].SubSystemId);
                m_EventRecords[i].EventId = Utils.ReverseByteOrder(m_EventRecords[i].EventId);
                m_EventRecords[i].TimeInaccurate = Utils.ReverseByteOrder(m_EventRecords[i].TimeInaccurate);
                m_EventRecords[i].DSTFlag = Utils.ReverseByteOrder(m_EventRecords[i].DSTFlag);
#endif
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="csvFile">stream for writing to the CSV file </param>
        private void WriterHeader(StreamWriter csvFile)
        {
            StringBuilder strBuilder = new StringBuilder();
            FieldInfo[] fields = typeof(Header).GetFields();

            foreach (FieldInfo field in fields)
            {
                strBuilder.Append(field.Name);
                strBuilder.Append(",");
            }
            // Remove the last ","
            strBuilder.Remove(strBuilder.Length - 1, 1);
            csvFile.WriteLine(strBuilder.ToString());

            strBuilder.Clear();
            foreach (FieldInfo field in fields)
            {
                // Store the 4 byte version as a hex number
                if (field.Name == "Version")
                {
                    strBuilder.Append("0x");
                    strBuilder.Append(m_Header.Version[0].ToString("X2"));
                    strBuilder.Append(m_Header.Version[1].ToString("X2"));
                    strBuilder.Append(m_Header.Version[2].ToString("X2"));
                    strBuilder.Append(m_Header.Version[3].ToString("X2"));
                }
                else if (field.Name == "TimeOfLastReset")
                {
                    string dateTime;
                    dateTime = UnixTimeStampToDateTime((uint)(field.GetValue(m_Header)), false);
                    strBuilder.Append(dateTime);
                }
                else if (field.Name != "Reserved")
                {
                    strBuilder.Append(field.GetValue(m_Header).ToString());
                }
                strBuilder.Append(",");
            }
            // Remove the last ","
            strBuilder.Remove(strBuilder.Length - 1, 1);
            csvFile.WriteLine(strBuilder.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="csvFile"></param>
        private void WriteEventCounters(StreamWriter csvFile)
        {
            StringBuilder strBuilder = new StringBuilder();
            FieldInfo[] fields = typeof(EventCounter).GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.Name != "Reserved")
                {
                    strBuilder.Append(field.Name);
                    strBuilder.Append(",");
                }
            }
            // Remove the last ","
            strBuilder.Remove(strBuilder.Length - 1, 1);
            csvFile.WriteLine(strBuilder.ToString());

            for (uint i = 0; i < NUM_EVENTS; i++)
            {
                strBuilder.Clear();
                foreach (FieldInfo field in fields)
                {
                    if (field.Name != "Reserved")
                    {
                        strBuilder.Append(field.GetValue(m_EventCounters[i]).ToString());
                        strBuilder.Append(",");
                    }
                }
                // Remove the last ","
                strBuilder.Remove(strBuilder.Length - 1, 1);
                csvFile.WriteLine(strBuilder.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="csvFile"></param>
        private void WriteEventRecords(StreamWriter csvFile)
        {
            StringBuilder strBuilder = new StringBuilder();
            FieldInfo[] fields = typeof(EventRecord).GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.Name != "Reserved")
                {
                    strBuilder.Append(field.Name);
                    strBuilder.Append(",");
                }
            }
            // Remove the last ","
            strBuilder.Remove(strBuilder.Length - 1, 1);
            csvFile.WriteLine(strBuilder.ToString());

            for (uint i = 0; i < NUM_EVENT_RECORDS; i++)
            {
                strBuilder.Clear();
                foreach (FieldInfo field in fields)
                {
                    if ((field.Name == "FailureBeginning") || (field.Name == "FailureEnd"))
                    {
                        uint eventDateTime = (uint)(field.GetValue(m_EventRecords[i]));
                        string dateTime = "N/A";
                        if (eventDateTime != 0)
                        {
                            dateTime = UnixTimeStampToDateTime((uint)(field.GetValue(m_EventRecords[i])), (m_EventRecords[i].DSTFlag != 0) ? true : false);
                        }
                        strBuilder.Append(dateTime);
                        strBuilder.Append(",");
                    }
                    else if (field.Name == "DSTFlag")
                    {
                        if (m_EventRecords[i].DSTFlag != 0)
                        {
                            strBuilder.Append("PDT");
                        }
                        else
                        {
                            strBuilder.Append("PST");
                        }
                        strBuilder.Append(",");
                    }
                    else if (field.Name != "Reserved")
                    {
                        strBuilder.Append(field.GetValue(m_EventRecords[i]).ToString());
                        strBuilder.Append(",");
                    }
                }
                // Remove the last ","
                strBuilder.Remove(strBuilder.Length - 1, 1);
                csvFile.WriteLine(strBuilder.ToString());
            }
        }

        /// <summary>
        /// Converts the date & time from UTC seconds to a human readable string format
        /// </summary>
        /// <param name="utc">the amount of seconds that have expired since the Epoch (Jan 1, 1970 @ 12:00:00 AM)</param>
        /// <param name="dstFlag">true if the DST flag was set; false otherwise</param>
        /// <returns>the converted date/time</returns>
        private string UnixTimeStampToDateTime(uint utc, bool dstFlag)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

            if (dstFlag)
            {
                // Add 3600 seconds (1 hour)
                utc += 3600;
            }

            // Update the date/time and convert it to local time that exists on the PC
            dtDateTime = dtDateTime.AddSeconds(utc).ToLocalTime();

            // Example format: "Wed Jun 21 1970 12:34:56"
            return dtDateTime.ToString("ddd MMM dd yyyy HH:mm:ss");
        }

        #endregion --- Methods ---
    }
}
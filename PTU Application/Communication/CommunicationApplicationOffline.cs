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
 *  File name:  CommunicationApplicationOffline.cs
 * 
 *  Revision History
 *  ----------------
 */

/*  Date        Version Author          Comments
 *  10/08/11    1.0     Sean.D          1.  First entry into TortoiseSVN.
 */

#region - [1.1] -
/*
 *  03/15/16    1.1     K.McD           References
 *                                      1.  PTU Modifications to Support the Requirements Defined in the San Francisco Bay Area Rapid Transport (BART) PTU Generic
 *                                          Requirements and Interface Description document (071-ICD-0011).
 *                                      
 *                                          1.  [REQ 7] - The PTU software shall offer the possibility to view the fault records and to export the result as a Comma-
 *                                              Separated Value (CSV) file format, as per RFC 4180. The PTU software shall resolve the event IDs and subsystem IDs and
 *                                              store them in human readable format in additional columns within the CSV file. Event counters shall be stored in a separate
 *                                              CSV file. The columns of the CSV file shall follo the event log file format definition in 071-ICD-0004 [2], Section 3.1,
 *                                              Table 12 (Counters) and Table 13 (Event Records).
 *                                              
 *                                      Modifications
 *                                      1.  Modified the ScanPort() method to update the property values of a local TargetConfiguration_t structure then copy this to the
 *                                          output TargetConfiguration_t output structure otherwise the compiler reports: 1. Use of unassigned out parameter
 *                                          'targetConfiguration' and; 2. The out parameter 'targetConfiguration' must be assigned to before control leaves the current
 *                                          method'. This is linked to the redefinition of the TargetConfiguration_t structure to use get and set accessors.
 *                                      
 */
#endregion - [1.1] -
#endregion --- Revision History ---

using System;

using Common.Communication;

namespace Bombardier.PTU.Communication
{
    /// <summary>
    /// Class to simulate communication with the target hardare with respect to the main PTU application.
    /// </summary>
    public class CommunicationApplicationOffline : CommunicationParentOffline, ICommunicationApplication
    {
        #region --- Member Variables ---
		/// <summary>
		/// Offset from the real-time clock.
		/// </summary>
        private static TimeSpan m_Offset = TimeSpan.Zero;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public CommunicationApplicationOffline() : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class and set the properties and member variables to those values associated with the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">Reference to the communication interface containing the properties and member variables that are to be used to 
        /// initialize the class.</param>
        public CommunicationApplicationOffline(ICommunicationParent communicationInterface)
            : base(communicationInterface)
        {
        }
        #endregion --- Constructors ---

        #region --- Methods ---
        /// <summary>
        /// Get the date and time from the running computer and apply the offset.
        /// </summary>
        /// <param name="use4DigitYearCode">A flag that specifies whether the Vehicle Control Unit uses a 2 or 4 digit year code. True, if it
        /// uses a 4 digit year code; otherwise, false.</param>
        /// <param name="dateTime">The the date and time as a .NET <c>DateTime</c> object.</param>
        public void GetTimeDate(bool use4DigitYearCode, out DateTime dateTime)
        {
            dateTime = DateTime.Now + m_Offset;
        }

        /// <summary>
        /// Sets the date and time of the target hardware as an offset from the current computer time.
        /// </summary>
        /// <param name="use4DigitYearCode">A flag that specifies whether the Vehicle Control Unit uses a 2 or 4 digit year code. True, if it
        /// uses a 4 digit year code; otherwise, false.</param>
        /// <param name="dateTime">The date and time as a .NET <c>DateTime</c> object.</param>
        public void SetTimeDate(bool use4DigitYearCode, DateTime dateTime)
        {
            m_Offset = dateTime - DateTime.Now;
        }

        /// <summary>
        /// Set the car identifier.
        /// </summary>
        /// <param name="carIdentifier">The car identifier.</param>
        public void SetCarID(string carIdentifier)
        {
            m_CarID = carIdentifier;
        }

        /// <summary>
		/// Provide dummy target configuration information and write it to the output parameter <paramref name="targetConfiguration"/>.
        /// </summary>
        /// <param name="communicationSetting">The communication settings that are to be used to communicate with the target. Ignored.</param>
        /// <param name="targetConfiguration">The target configuration information returned from the target hardware if a target is found.</param>
        /// <returns>A flag to indicate whether a target was found; true, if a target was found, otherwise, false.</returns>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the PTUDLL32.InitCommunication() method is not
        /// CommunicationError.Success.</exception>
        public bool ScanPort(CommunicationSetting_t communicationSetting, out TargetConfiguration_t targetConfiguration)
        {
            TargetConfiguration_t localTargetConfiguration = new TargetConfiguration_t();
            localTargetConfiguration.CarIdentifier = m_CarID;
            localTargetConfiguration.ConversionMask = ConversionMask;
            localTargetConfiguration.ProjectIdentifier = ProjectIdentifier;
            localTargetConfiguration.SubSystemName = SubSystemName;
            localTargetConfiguration.Version = Version;
            targetConfiguration = localTargetConfiguration;

            return true;
        }
        #endregion --- Methods ---
    }
}

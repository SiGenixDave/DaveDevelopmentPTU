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
 *  Project:    Common
 * 
 *  File name:  FileHeader.cs
 * 
 *  Revision History
 *  ----------------
 */

 #region - [1.0 to 1.4] -
/*  Date        Version Author          Comments
 *  05/09/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  08/13/10    1.1     K.McD           1.  Changes required to support moving the GetUserName() method from class FormAddComments to class General.
 * 
 *  01/06/11    1.2     K.McD           1.  Included the SetToOffline() method in the Header_t structure.
 * 
 *  01/31/11    1.3     K.MCD           1.  Changed a number of XML tags and tidied the layout.
 *  
 *  07/25/11    1.4     K.McD           1.  Modified the name of the SetToOffline() method associated with the Header_t structure to be SetToDiagnostic() in accordance 
 *                                          with the June 2011 sprint review.
 */
#endregion - [1.0 to 1.4] -

#region - [1.5] -
/*
 *  03/15/16    1.5     K.McD           References
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
 *                                      1.  Modified the Header_t structure to use get and set accessors to access the private member variables.
 *                                      2.  Modified the SetToDiagnostic() method of the Header_t structure to use the Header_t properties.
 *                                      3.  Modified the static Initialize() method of the FileHeader static class to use the ProjectInformation and TargetConfiguration
 *                                          properties of the header rather than accessing the member variables directly.                                  
 */
#endregion - [1.5] -

#region - [1.6] -
/*
 *  04/02/16    1.6     K.McD           References
 *                                      1.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 19. Release 6.16.4 of the PTU can't access fault data streams that have been generated
 *                                          by older releases of the PTU.
 *                                              
 *                                      Modifications
 *                                      1.  Reverted the Header_t structure back to the one used in Rev. 1.4.
 */
#endregion - [1.6] -
#endregion --- Revision History ---

using System;
using System.Windows.Forms;

using Common.Communication;
using Common.Configuration;
using Common.Properties;

namespace Common
{
    #region --- Structures ---
    /// <summary>
    /// A structure to store the header information that is included in all data files that are saved to disk;
    /// </summary>
    [Serializable]
    public struct Header_t
    {
        /// <summary>
        /// Flag to indicate whether header information is available. True indicates that header information is available; otherwise, false.
        /// </summary>
        public bool Available;

        /// <summary>
        /// The project information associated with the downloaded data.
        /// </summary>
        public DataDictionaryInformation_t ProjectInformation;

        /// <summary>
        /// The configuration information associated with the target hardware.
        /// </summary>
        public TargetConfiguration_t TargetConfiguration;

        /// <summary>
        /// The name of the user who requested the download.
        /// </summary>
        public string UserName;

        /// <summary>
        /// Any engineer comments associated with the downloaded data.
        /// </summary>
        public string Comments;

        /// <summary>
        /// The date and time when the file was created.
        /// </summary>
        public DateTime DateTimeCreated;

        /// <summary>
        /// The <c>ProductName</c> reference of the PTU application used to collect and save the file.
        /// </summary>
        public string ProductName;

        /// <summary>
        /// The <c>ProductVersion</c> reference of the PTU application used to collect and save the file.
        /// </summary>
        public string ProductVersion;

        /// <summary>
        /// Sets the header to reflect diagnostic mode status i.e. all target configuration parameters will be cleared.
        /// </summary>
        public void SetToDiagnostic()
        {
            TargetConfiguration.CarIdentifier = string.Empty;
            TargetConfiguration.ConversionMask = 0;
            TargetConfiguration.ProjectIdentifier = string.Empty;
            TargetConfiguration.SubSystemName = string.Empty;
            TargetConfiguration.Version = string.Empty;
        }
    }
    #endregion --- Structures ---

    /// <summary>
    /// Class to manage the header information associated with each of the data files.
    /// </summary>
    public static class FileHeader
    {
        #region --- Member Variables ---
        /// <summary>
        /// Header information associated with data downloaded from the target hardware.
        /// </summary>
        private static Header_t m_HeaderCurrent;

        /// <summary>
        /// Header information associated with the last file retrieved from disk.
        /// </summary>
        private static Header_t m_HeaderLastRetrieved;

        /// <summary>
        /// Header information used by the Save All menu option.
        /// </summary>
        private static Header_t m_HeaderSaveAll;
        #endregion --- Member Variables ---

        #region --- Methods ---
        /// <summary>
        /// Initializes the specified header as unavailable.
        /// </summary>
        /// <param name="header">The header that is to be marked as unavailable.</param>
        public static void Initialize(ref Header_t header)
        {
            DataDictionaryInformation_t projectInformation;
            TargetConfiguration_t targetConfiguration;

            header.Available = true;
            header.Comments = string.Empty;
            header.DateTimeCreated = DateTime.Now;
            header.ProductName = Application.ProductName;
            header.ProductVersion = Application.ProductVersion;

            projectInformation = new DataDictionaryInformation_t();
            projectInformation.DataDictionaryBuilderVersion = Resources.TextUnavailable;
            projectInformation.DataDictionaryName = Resources.TextUnavailable;
            projectInformation.ProjectIdentifier = Resources.TextUnavailable;
            projectInformation.Version = Resources.TextUnavailable;
            projectInformation.WatchIdentifierCount = 0;
            header.ProjectInformation = projectInformation;

            targetConfiguration = new TargetConfiguration_t();
            targetConfiguration.CarIdentifier = string.Empty;
            targetConfiguration.ConversionMask = 0;
            targetConfiguration.ProjectIdentifier = Resources.TextUnavailable;
            targetConfiguration.SubSystemName = Resources.TextUnavailable;
            targetConfiguration.Version = Resources.TextUnavailable;
            header.TargetConfiguration = targetConfiguration;

            header.UserName = General.GetUsername();
        }
        #endregion --- Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets or sets the header information associated with the data downloaded from the target hardware. 
        /// </summary>
        /// <remarks>All values will be null or 0, as appropriate, until valid communications is established.</remarks>
        public static Header_t HeaderCurrent
        {
            get { return m_HeaderCurrent; }
            set { m_HeaderCurrent = value; }
        }

        /// <summary>
        /// Gets or sets the header information associated with the last file retrieved from disk.
        /// </summary>
        /// <remarks>All values will be null or 0, as appropriate, until valid communications is established. </remarks>
        public static Header_t HeaderLastRetrieved
        {
            get { return m_HeaderLastRetrieved; }
            set { m_HeaderLastRetrieved = value; }
        }

        /// <summary>
        /// Gets or sets the header information used by the Save All menu option.
        /// </summary>
        /// <remarks>All values will be null or 0, as appropriate, until valid communications is established. </remarks>
        public static Header_t HeaderSaveAll
        {
            get { return m_HeaderSaveAll; }
            set { m_HeaderSaveAll = value; }
        }
        #endregion --- Properties ---
    }
}

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
 *  File name:  SelfTestErrorMessage.cs
 * 
 *  Revision History
 *  ----------------
 *  Date        Version Author          Comments
 *  04/05/2016  1.0     K.McD           1.  First entry into TortoiseSVN.
 *
 */
#endregion --- Revision History ---

namespace Common.Configuration
{
    /// <summary>
    /// A structure to store the fields associated with an entry from the <c>SELFTESTERRMESS</c> table of the data dictionary.
    /// </summary>
    public struct SelfTestErrorMessage_t
    {
        #region --- Member Variables ---
        /// <summary>
        /// The self test error identifier associated with the record.
        /// </summary>
        private short m_ErrorIdentifier;

        /// <summary>
        /// The self test error description associated with the record.
        /// </summary>
        private string m_Description;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initialize a new instance of the structure.
        /// </summary>
        /// <param name="selfTestErrorIdentifier">The value of the <c>ERRID</c> field.</param>
        /// <param name="selfTestErrorDescription">The value of the <c>DESCRIPTION</c> field.</param>
        public SelfTestErrorMessage_t(short selfTestErrorIdentifier, string selfTestErrorDescription)
        {
            m_ErrorIdentifier = selfTestErrorIdentifier;
            m_Description = selfTestErrorDescription;
        }
        #endregion --- Constructors ---

        #region --- Properties ---
        /// <summary>
        /// Gets or sets the self test error identifier associated with the record.
        /// </summary>
        public short ErrorIdentifier
        {
            get { return m_ErrorIdentifier; }
            set { m_ErrorIdentifier = value; }
        }

        /// <summary>
        /// Gets or sets the self test error description associated with the record.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion --- Properties ---
    }
}

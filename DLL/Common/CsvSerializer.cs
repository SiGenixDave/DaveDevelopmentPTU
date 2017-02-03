#region --- Revision History ---
/*
 * 
 *  (C) The Code Project Open Source Licence Agreement
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    Common
 * 
 *  File name:  CsvSerializer.cs
 * 
 *  Revision History
 *  ----------------
 *  Date        Version Author          Comments
 */

#region - [1.0] -
/*                                    
 *  03/08/2016  1.0     K.McD           References
 *                                      1.  PTU Modifications to Support the Requirements Defined in the San Francisco Bay Area Rapid Transport (BART) PTU Generic
 *                                          Requirements and Interface Description document (071-ICD-0011).
 *                                      
 *                                          1.  [REQ 7] - The PTU software shall offer the possibility to view the fault records and to export the result as a
 *                                              Comma-Separated Value (CSV) file format, as per RFC 4180. The PTU software shall resolve the event IDs and subsystem ID's
 *                                              and store them in human readable format in additional columns within the CSV file. Event counters shall be stored in a
 *                                              separate CSV file. The columns of the CSV file shall follow the event log file format definition in 071-ICD-0004 [2],
 *                                              Section 3.1, Table 12 (Counters) and Table 13 (Event Records).
 *                                      
 *                                      Modifications
 *                                      1.  First entry into TortoiseSVN.
 */
#endregion - [1.0] -

#region - [1.1] -
/*
 *  04/04/2016  1.1     K.McD           References
 *                                      1.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 19. Release 6.16.4 of the PTU can't access fault data streams that have been generated
 *                                          by older releases of the PTU.
 *  
 *                                      Modifications
 *                                      1.  Because the modifications to the Header_t and TargetConfiguration_t structures to allow the 'type.GetProperty()'
 *                                          function to be used in GetPropertyNamesEventLogFile() had to be reverted (because of the problem outlined in the SNCR), 
 *                                          the use of 'type.GetProperty()' is no longer valid. For the time being, the properties associated with the Header_t and
 *                                          TargetConfiguration structures have been removed from the CSV file until a permanent solution is found. 
 */

/*
 *  04/14/2016  1.1.1   K.McD           References
 *                                      1.  Conference Call 4th April 2016 - Changes to the CSV content and layout resulting from the feedback from the demonstration
 *                                          to BT-AME/San Fransisco BART Team. 
 *                                          
 *                                      Modifications
 *                                      1.  Added the FieldInformation_t structure.
 *                                      2.  Added the UnixTimestampFromDateTime() method.
 *                                      3.  Extensive modifications to the existing code to: (a) accommodate the proposed changes resulting from Ref.: 1; and (b)
 *                                          take into account the problems resulting from Rev. 1.1 item 1.
 */
#endregion - [1.1] -

#region - [1.2] -
/*  
 *  10/10/2016  1.2     K.McD           References
 *                                      1.  Ref.: Conference Call 3rd Oct 2016. For both BART and R188, the fields of the CSV file that is produced whenever and new event
 *                                          log XML file is generated should should be based upon document 'NYCT R188 - Variation Order (14th Jan 2016) - XML to CSV.pdf'.
 * 
 *                                      Modifications
 *                                      1.  Added the GetFieldInformationEventVariable() method to list the CSV fields associated with the EventVariable class. These
 *                                          are now defined in the document mentioned in reference 1, above.
 *                                      
 *                                      2.  Updated the GetFieldInformationEventRecord() method to list the CSV fields associated with the EventRecord class. These
 *                                          are now defined in the document mentioned in reference 1, above.
 *                                          
 *                                      3.  Included new constants to cover the field names and property access keys associated with the CSV field names and
 *                                          corresponding property values defined in the document mentioned in reference 1, above.
 *                                          
 *                                      4.  Added the ConvertRawPropertyValueToString() method to convert a raw property value, obtained using the
 *                                          'PropertyInformation.GetValue()'call, to a string, depending upon the property type.
 *                                          
 *                                      5.  Added the GetFieldValuesEventVariable() method to generate and format the CSV field values associated with the specified
 *                                          'EventVariable'.
 *                                          
 *                                      6.  Updated the GetFieldValuesEventRecord() method to generate and format the CSV field values associated with the specified
 *                                          'EventRecord'.
 *                                          
 *                                      7.  Updated the Serialize() method to format the CSV file as per the document mentioned in reference 1, above.
 */
#endregion - [1.2] -
#endregion --- Revision History ---

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Common.Configuration;
using Common.Properties;

namespace Common
{
    /// <summary>
    /// Serializes objects into CSV documents. The Common.CsvSerializer class controls how objects are encoded into CSV.
	/// </summary>
    /// <remarks>At present, this CsvSerializer only supports serialization of the event log structure EventLogFile_t.</remarks>
    public class CsvSerializer
    {
        #region - [Structures] -
        /// <summary>
        /// The structure used to store the information associated with a particular field of a CSV record.
        /// </summary>
        public struct FieldInformation_t
        {
            #region - [Member Variables] -
            /// <summary>
            /// The name of the field.
            /// </summary>
            private string m_CSVFieldName;

            /// <summary>
            /// The field type e.g. string, UInt16, Int32, byte etc.
            /// </summary>
            private Type m_CSVFieldType;

            /// <summary>
            /// If the field is mapped to a class/structure property, the <c>PropertyInfo</c> associated with the property; otherwise, null.
            /// </summary>
            private PropertyInfo m_PropertyInformation;
            #endregion - [Member Variables] -

            #region - [Constructors] -
            /// <summary>
            /// Instantiates a new instance of the structure.
            /// </summary>
            /// <param name="fieldName">The CSV field name.</param>
            /// <param name="fieldType">The CSV field type.</param>
            /// <param name="propertyInfo">If the field is mapped directly to a class/structure property, the <c>PropertyInfo</c> associated with the property; otherwise,
            /// null.</param>
            public FieldInformation_t(string fieldName, Type fieldType, PropertyInfo propertyInfo)
            {
                m_CSVFieldName = fieldName;
                m_CSVFieldType = fieldType;
                m_PropertyInformation = propertyInfo;
            }
            #endregion - [Constructors] -

            #region - [Properties] -
            /// <summary>
            /// Get or set the name of the CSV field.
            /// </summary>
            public string CSVFieldName
            {
                get { return m_CSVFieldName; }
                set { m_CSVFieldName = value; }
            }

            /// <summary>
            /// Get or set the CSV field type e.g. string, UInt16, Int32, byte etc.
            /// </summary>
            public Type CSVFieldType
            {
                get { return m_CSVFieldType; }
                set { m_CSVFieldType = value; }
            }

            /// <summary>
            /// Get or set the <c>PropertyInfo</c> associated with the property if the field is mapped directly to a property.
            /// </summary>
            public PropertyInfo PropertyInformation
            {
                get { return m_PropertyInformation; }
                set { m_PropertyInformation = value; }
            }
            #endregion - [Properties] -
        }
        #endregion - [Structures] -

        #region --- Constants ---
        /// <summary>
        /// The .NET format string used to display the Value property of the scalar user control.
        /// </summary>
        protected const string FormatStringNumeric = "########0.####";

        /// <summary>
        /// The format string, converted to lower case, used in the data dictionary to represent a general number. Value: "general number";
        /// </summary>
        protected const string FormatStringFieldGeneralNumber = "general number";

        /// <summary>
        /// The string representation of an invalid numeric value (Not a Number). Value: "NaN".
        /// </summary>
        protected const string InvalidValueAsString = "NaN";

        #region - [CSV Record Types] -
        /// <summary>
        /// The string representation used to identify the CSV Record Type as an <c>EventRecord</c>. Value: "EventRecord". 
        /// </summary>
        protected const string CSVRecordTypeEventRecord = "EventRecord";

        /// <summary>
        /// The string representation used to identify the CSV Record Type as an <c>EventVariable</c>. Value: "EventVariable". 
        /// </summary>
        protected const string CSVRecordTypeEventVariable = "EventVariable";
        #endregion - [CSV Record Types] -

        #region - [CSV Field Names] -
        #region - [XML Layout] -
        #region - EventRecord] -
        /// <summary>
        /// The name of the field that specifies the CSV record type associated with the current line of text. Value: "CSVRecordType".
        /// </summary>
        protected const string FieldNameCSVRecordType = "CSVRecordType";

        /// <summary>
        /// The name of the field that specifies the identifier associated with the current event. Value: "Identifier".
        /// </summary>
        protected const string FieldNameIdentifier = "Identifier";

        /// <summary>
        /// The name of the field that specifies the help index associated with the current event. Value: "HelpIndex".
        /// </summary>
        protected const string FieldNameHelpIndex = "HelpIndex";

        /// <summary>
        /// The name of the field that specifies the description of the current event. Value: "Description".
        /// </summary>
        protected const string FieldNameDescription = "Description";

        /// <summary>
        /// The name of the field thst specifies the log associated with the current event. Value: "LogIdentifier".
        /// </summary>
        protected const string FieldNameLogIdentifier = "LogIdentifier";

        /// <summary>
        /// The name of the field that specifies the car identifier associated with the current event. Value: "CarIdentifier".
        /// </summary>
        protected const string FieldNameCarIdentifier = "CarIdentifier";

        /// <summary>
        /// The name of the field that specifies the date of the event. Value: "Date".
        /// </summary>
        protected const string FieldNameDate = "Date";

        /// <summary>
        /// The name of the field that specifies the time of the event. Value: "Time".
        /// </summary>
        protected const string FieldNameTime = "Time";

        /// <summary>
        /// The name of the field that specifies the date and time of the event. Value: "DateTime".
        /// </summary>
        protected const string FieldNameDateTime = "DateTime";

        /// <summary>
        /// The name of the field that specifies the day of the week associated with the event. Value: "Day".
        /// </summary>
        protected const string FieldNameDay = "Day";

        /// <summary>
        /// The name of the bool field that specifies whether a datastream was saved or not for the current event. Value: "StreamSaved". 
        /// </summary>
        protected const string FieldNameStreamSaved = "StreamSaved";

        /// <summary>
        /// The name of the field that specifies the reference number of the datastream, if one was saved for the current event. Value: "StreamNumber".
        /// </summary>
        protected const string FieldNameStreamNumber = "StreamNumber";

        /// <summary>
        /// The name of the field that specifies the number of event variables associated with the current event. Value: "EventVariableCount".
        /// </summary>
        protected const string FieldNameEventVariableCount = "EventVariableCount";
        #endregion - [EventRecord] -

        #region - [EventVariable] -
        /// <summary>
        /// The name of the field that specifies the VariableType associated with the current event variable. Value: "VariableType".
        /// </summary>
        protected const string FieldNameVariableType = "VariableType";

        /// <summary>
        /// The name of the field that specifies the Identifier associated with the current event variable. Value: "Identifier".
        /// </summary>
        protected const string FieldNameEventVariableIdentifier = "Identifier";

        /// <summary>
        /// The name of the field that specifies the Name of the current event variable. Value: "Name".
        /// </summary>
        protected const string FieldNameName = "Name";

        /// <summary>
        /// The name of the field that specifies the ValueFromTarget associated with the current event variable. Value: "ValueFromTarget".
        /// </summary>
        protected const string FieldNameValueFromTarget = "ValueFromTarget";

        /// <summary>
        /// The name of the field that specifies the Units associated with the current event variable. Value: "Units".
        /// </summary>
        protected const string FieldNameUnits = "Units";
        #endregion - [EventVariable] -
        #endregion - [XML Layout] -

        /// <summary>
        /// The name of the field that specifies the time, as a Unix Timestamp, that the event occurred. Value: "FailureBeginning". Value: "".
        /// </summary>
        protected const string FieldNameFailureBeginning = "FailureBeginning";

        /// <summary>
        /// The name of the field that specifies the time, as a Unix Timestamp, that the event was cleared. Value: "FailureEnd". Value: "".
        /// </summary>
        protected const string FieldNameFailureEnd = "FailureEnd";

        /// <summary>
        /// The name of the field that specifies the sub system. Value: "SubSystemID". Value: "".
        /// </summary>
        protected const string FieldNameSubSystemID = "SubSystemID";

        /// <summary>
        /// The name of the field that specifies the event code (1 ... 1023). Value: "EventID". Value: "".
        /// </summary>
        protected const string FieldNameEventID = "EventID";

        /// <summary>
        /// The name of the field that specifies whethet the time stamps are not using synchronized time system.  Value: "TimeInaccurate".
        /// </summary>
        protected const string FieldNameTimeInaccurate = "TimeInaccurate";

        /// <summary>
        /// The name of the field that specifies whether Daylight Saving Time is active or not: 0x00 = No Active DST, 0x01 = Set Time DST Active , 0x10 = Reset Time
        /// DST Active, 0x11 = Set and Reset DST Active. Value: "DSTFlag".
        /// </summary>
        protected const string FieldNameDSTFlag = "DSTFlag";

        /// <summary>
        /// Reserved for future use. Value: "Reserved".
        /// </summary>
        protected const string FieldNameReserved = "Reserved";

        /// <summary>
        /// The name of the field that specifies the number of event records in the CSV file. Value: "NumberOfRecords".
        /// </summary>
        protected const string FieldNameNumberOfRecords = "NumberOfRecords";

        /// <summary>
        /// The name of the field that specifies the index of the first event record in the CSV file. Value: "FirstRecordIndex".
        /// </summary>
        protected const string FieldNameFirstRecordIndex = "FirstRecordIndex";

        /// <summary>
        /// The name of the field that specifies the index of the last event record in the CSV file. Value: "LastRecordIndex".
        /// </summary>
        protected const string FieldNameLastRecordIndex = "LastRecordIndex";
        #endregion - [CSV Field Names] -

        #region - [Property Access Keys] -
        #region - [EventRecord] -
        /// <summary>
        /// Key to access the EventIndex property of an EventRecord. Value: "EventIndex".
        /// </summary>
        protected const string KeyPropertyNameEventRecordEventIndex = "EventIndex";

        /// <summary>
        /// Key to access the HelpIndex property of an EventRecord. Value: "HelpIndex".
        /// </summary>
        protected const string KeyPropertyNameEventRecordHelpIndex = "HelpIndex";

        /// <summary>
        /// Key to access the Description property of an EventRecord. Value: "Description".
        /// </summary>
        protected const string KeyPropertyNameEventRecordDescription = "Description";

        /// <summary>
        /// Key to access the StreamSaved property of an EventRecord. Value: "StreamSaved".
        /// </summary>
        protected const string KeyPropertyNameEventRecordStreamSaved = "StreamSaved";

        /// <summary>
        /// Key to access the StreamNumber property of an EventRecord. Value: "StreamNumber".
        /// </summary>
        protected const string KeyPropertyNameEventRecordStreamNumber = "StreamNumber";

        /// <summary>
        /// Key to access the CarIdentifier property of an EventRecord. Value: "CarIdentifier".
        /// </summary>
        protected const string KeyPropertyNameEventRecordCarIdentifier = "CarIdentifier";

        /// <summary>
        /// Key to access the LogIdentifier property of an EventRecord. Value: "LogIdentifier".
        /// </summary>
        protected const string KeyPropertyNameEventRecordLogIdentifier = "LogIdentifier";

        /// <summary>
        /// Key to access the Identifier property of an EventRecord. Value: "Identifier".
        /// </summary>
        protected const string KeyPropertyNameEventRecordIdentifier = "Identifier";

        /// <summary>
        /// Key to access the Date property of an EventRecord. Value: "Date".
        /// </summary>
        protected const string KeyPropertyNameEventRecordDate = "Date";

        /// <summary>
        /// Key to access the Time property of an EventRecord. Value: "Time".
        /// </summary>
        protected const string KeyPropertyNameEventRecordTime = "Time";

        /// <summary>
        /// Key to access the DateTime property of an EventRecord. Value: "DateTime".
        /// </summary>
        protected const string KeyPropertyNameEventRecordDateTime = "DateTime";

        /// <summary>
        /// Key to access the EventVariableList property of an EventRecord. Value: "EventVariableList".
        /// </summary>
        protected const string KeyPropertyNameEventRecordEventVariableList = "EventVariableList";
        #endregion - [EventRecord] -

        #region - [EventVariable] -
        /// <summary>
        /// Key to access the VariableType property of an EventVariable. Value: "VariableType".
        /// </summary>
        protected const string KeyPropertyNameEventVariableVariableType = "VariableType";

        /// <summary>
        /// Key to access the Identifier property of an EventVariable. Value: "Identifier".
        /// </summary>
        protected const string KeyPropertyNameEventVariableIdentifier = "Identifier";

        /// <summary>
        /// Key to access the Name property of an EventVariable. Value: "Name".
        /// </summary>
        protected const string KeyPropertyNameEventVariableName = "Name";

        /// <summary>
        /// Key to access the ValueFromTarget property of an EventVariable. Value: "ValueFromTarget".
        /// </summary>
        protected const string KeyPropertyNameEventVariableValueFromTarget = "ValueFromTarget";

        /// <summary>
        /// Key to access the Units property of an EventVariable. Value: "Units".
        /// </summary>
        protected const string KeyPropertyNameEventVariableUnits = "Units";
        #endregion - [EventVariable] -

        #region - [EventLogFile] -
        #region - [DataDictionaryInformation_t] -
        /// <summary>
        /// Key to access the DataDictionaryName property of a DataDictionaryInformation_t structure. Value: "DataDictionaryName".
        /// </summary>
        protected const string KeyPropertyNameDataDictionaryInformationDDName = "DataDictionaryName";

        /// <summary>
        /// Key to access the ProjectIdentifier property of a DataDictionaryInformation_t structure. Value: "ProjectIdentifier".
        /// </summary>
        protected const string KeyPropertyNameDataDictionaryInformationProjectIdentifier = "ProjectIdentifier";

        /// <summary>
        /// Key to access the Version property of a DataDictionaryInformation_t structure. Value: "Version".
        /// </summary>
        protected const string KeyPropertyNameDataDictionaryInformationDDVersion = "Version";
        #endregion - [DataDictionaryInformation_t] -
        #endregion - [EventLogFile] -
        #endregion - [Property Access Keys] -
        #endregion --- Constants ---

        #region --- Member Variables ---
        /// <summary>
        /// The separator character that is to be used between field values in the CSV file.
        /// </summary>
        private char m_Separator = ',';

        /// <summary>
        /// A flag to control whether the field values are to be enclosed within double quotes i.e. "123.456".
        /// </summary>
        private bool m_UseTextQualifier = true;

        /// <summary>
        /// The <c>List</c> of <c>FieldInformation_t</c> structures associated with an <c>EventRecord</c> class.
        /// </summary>
        List<FieldInformation_t> m_EventRecordFieldInformationList;

        /// <summary>
        /// The <c>List</c> of <c>FieldInformation_t</c> structures associated with an <c>EventVariable</c> class.
        /// </summary>
        List<FieldInformation_t> m_EventVariableFieldInformationList;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes a new instance of the Common.CsvSerializer class that can serialize objects of the specified type into CSV documents.
		/// </summary>
        /// <remarks>At present, this CsvSerializer only supports serialization of the event log structure EventLogFile_t.</remarks>
        /// <param name="type">The type of the object that this Common.CsvSerializer is to serialize.</param>
		public CsvSerializer(Type type)
		{
            if (type != typeof(EventLogFile_t))
            {
                throw new InvalidOperationException(Resources.EMCsvSerializationErrorTypeNotSupported);
            }
		}
        #endregion --- Constructors ---

        #region --- Methods ---
        /// <summary>
        /// Serializes the specified System.IO.Object and writes the CSV document to a file using the specified System.IO.Stream.
        /// </summary>
        /// <remarks>This method only supports serialization of the <c>EventLogFile_t</c> structure to a CSV file.</remarks>
        /// <param name="stream">The System.IO.Stream used to write the CSV document.</param>
        /// <param name="data">The System.IO.Object to serialize.</param>
        public void Serialize(Stream stream, object data)
		{
            // The CSV text that is written to the stream.
			StringBuilder stringBuilder = new StringBuilder();

            // A list of the field values associated with the current CSV record.
            List<string> valueList;

            // The data that is to be serialized cast to an EventLogFile_t structure.
            EventLogFile_t eventLogFile = (EventLogFile_t)data;

            // At present this method only supports the serialization of the event log structure - EventLogFile_t.
            if (data.GetType().Equals(typeof(EventLogFile_t)) && eventLogFile.EventRecordList.Count > 0)
            {
                // Generate the FieldInformation lists for the EventRecord and EventVariable classes.
                m_EventRecordFieldInformationList = GetFieldInformationEventRecord();
                m_EventVariableFieldInformationList = GetFieldInformationEventVariable();

                // Process each event record.
                foreach (EventRecord eventRecord in eventLogFile.EventRecordList)
                {
                    // Get the CSV field values for the current event record.
                    valueList = GetFieldValuesEventRecord(eventRecord, m_EventRecordFieldInformationList);

                    // Add the CSV field values for each event variable associated with the current event.
                    foreach (EventVariable eventVariable in eventRecord.EventVariableList)
                    {
                        valueList.AddRange(GetFieldValuesEventVariable(eventVariable, m_EventVariableFieldInformationList));
                    }

                    // Add the CSV field values for the current event record to the CSV file.
                    stringBuilder.AppendLine(string.Join(m_Separator.ToString(), valueList.ToArray()));
                }

                // Write the CSV file.
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(stringBuilder.ToString().Trim());
                }
            }
            else
            {
                throw new InvalidOperationException(Resources.EMCsvSerializationErrorTypeNotSupported);
            }
		}

        /// <summary>
        /// <para>Get a list of the field values, as strings, associated with the specified <c>eventRecord</c> parameter.</para>
        /// <para>The values and the order in which they are serialized are defined by the <c>fieldInformationList</c> parameter.</para>
        /// </summary>
        /// <returns>A list of the field values, as strings, associated with the specified <c>eventRecord</c> parameter.</returns>
        private List<string> GetFieldValuesEventRecord(EventRecord eventRecord, List<FieldInformation_t> fieldInformationList)
        {
            // A list of the property values associated with the specified event record. 
            List<string> valueList = new List<string>();

            // The value of a field as a string.
            string valueAsString = string.Empty;

            // The raw value returned from the call to GetValue().
            object raw = null;

            // Process each Field that is specified by the 'fieldInformationList' parameter. 
            FieldInformation_t fieldInformation;
            for (int index = 0; index < fieldInformationList.Count; index++ )
            {
                fieldInformation = fieldInformationList[index];
                try
                {
                    // If the 'PropertyInformation' value for the field is null, generate the field value depending upon the field name.
                    if (fieldInformation.PropertyInformation == null)
                    {
                        switch (fieldInformation.CSVFieldName)
                        {
                            case FieldNameLogIdentifier:
                                valueAsString = Lookup.LogTable.Items[eventRecord.LogIdentifier].Description;
                                break;
                            case FieldNameEventVariableCount:
                                valueAsString = eventRecord.EventVariableList.Count.ToString();
                                break;
                            case FieldNameDate:
                                valueAsString = eventRecord.Date;
                                break;
                            case FieldNameTime:
                                valueAsString = eventRecord.Time;
                                break;
                            case FieldNameDateTime:
                                long rawAsUnixTimestamp = UnixTimestampFromDateTime(eventRecord.DateTime);

                                // The BART Specification - 071-ICD-0004 calls for the Unix Timestamp to be displayed as a UInt32.
                                UInt32 unixTimestampAsUInt32 = Convert.ToUInt32(rawAsUnixTimestamp);

                                valueAsString = CommonConstants.HexValueIdentifier + unixTimestampAsUInt32.ToString(CommonConstants.FormatStringHex +
                                                (sizeof(UInt16) * 2).ToString());
                                break;
                            case FieldNameDay:
                                valueAsString = eventRecord.DateTime.DayOfWeek.ToString();
                                break;
                            case FieldNameStreamSaved:
                                valueAsString = (eventRecord.StreamSaved == true) ? "Yes" : "No";
                                break;
                            case FieldNameCSVRecordType:
                                valueAsString = CSVRecordTypeEventRecord;
                                break;
                            default:
                                valueAsString = string.Empty;
                                break;
                        }
                    }
                    else
                    {
                        // Get the field value using the 'PropertyInformation' reference.
                        raw = fieldInformation.PropertyInformation.GetValue(eventRecord, null);
                        valueAsString = ConvertRawPropertyValueToString(raw, ref fieldInformation);
                    }
                }
                catch (Exception)
                {
                    valueAsString = string.Empty;
                }

                valueList.Add((UseTextQualifier) ? string.Format("\"{0}\"", valueAsString) : valueAsString);
            }

            return valueList;
        }

        /// <summary>
        /// <para>Get a list of the field values, as strings, associated with the specified <c>eventVariable</c> parameter.</para>
        /// <para>The values and the order in which they are serialized are defined by the <c>fieldInformationList</c> parameter.</para>
        /// </summary>
        /// <returns>A list of the field values, as strings, associated with the specified <c>eventVariable</c> parameter.</returns>
        private List<string> GetFieldValuesEventVariable(EventVariable eventVariable, List<FieldInformation_t> fieldInformationList)
        {
            // A list of the property values associated with the specified event variable. 
            List<string> valueList = new List<string>();

            // The value of a field as a string.
            string valueAsString = string.Empty;

            // The raw value returned from the call to GetValue().
            object raw = null;

            // Process each Field that is specified by the 'fieldInformationList' parameter. 
            FieldInformation_t fieldInformation;
            for (int index = 0; index < fieldInformationList.Count; index++)
            {
                fieldInformation = fieldInformationList[index];
                try
                {
                    // If the 'PropertyInformation' value for the field is null, generate the field value depending upon the field name.
                    if (fieldInformation.PropertyInformation == null)
                    {
                        switch (fieldInformation.CSVFieldName)
                        {
                            case FieldNameValueFromTarget:
                                // Add the event variable value to the list. Scalars are output in engineering units, enumerators are output as the text string
                                // corresponding to the enumerator value and bitmask variables are displayed in the hexadecimal value of the bitmask.
                                switch (eventVariable.VariableType)
                                {
                                    case VariableType.Bitmask:
                                        uint valueUINT = (uint)eventVariable.ValueFromTarget;
                                        valueAsString = CommonConstants.HexValueIdentifier + valueUINT.ToString(CommonConstants.FormatStringHex);
                                        break;
                                    case VariableType.Enumerator:
                                        valueUINT = (uint)eventVariable.ValueFromTarget;
                                        try
                                        {
                                            valueAsString = Lookup.EventVariableTable.GetEnumeratorText(eventVariable.Identifier, valueUINT);
                                        }
                                        catch (Exception)
                                        {
                                            valueAsString = InvalidValueAsString;
                                        }
                                        break;
                                    case VariableType.Scalar:
                                        double engineeringValue = 0;
                                        double scaleFactor = 0; ;
                                        int decimalPlaces = 0;
                                        string formatString = String.Empty;
                                        try
                                        {
                                            scaleFactor = Lookup.EventVariableTable.Items[eventVariable.Identifier].ScaleFactor;
                                            General.GetDecimalPlaces(scaleFactor, out decimalPlaces);
                                            engineeringValue = eventVariable.ValueFromTarget * scaleFactor;
                                            engineeringValue = Math.Round(engineeringValue, decimalPlaces);
                                            formatString = Lookup.EventVariableTable.Items[eventVariable.Identifier].FormatString.ToLower();
                                            if (formatString == FormatStringFieldGeneralNumber)
                                            {
                                                valueAsString = engineeringValue.ToString(FormatStringNumeric);
                                            }
                                            else if (formatString == CommonConstants.DDFormatStringHex)
                                            {
                                                valueAsString = CommonConstants.HexValueIdentifier + ((long)engineeringValue).ToString(CommonConstants.FormatStringHex);
                                            }
                                            else
                                            {
                                                valueAsString = engineeringValue.ToString();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            valueAsString = InvalidValueAsString;
                                        }
                                        break;
                                    default:
                                        valueAsString = InvalidValueAsString;
                                        break;
                                }
                                break;
                            case FieldNameCSVRecordType:
                                valueAsString = CSVRecordTypeEventVariable;
                                break;
                            default:
                                valueAsString = string.Empty;
                                break;
                        }
                    }
                    else
                    {
                        // Get the field value using the 'PropertyInformation' reference.
                        raw = fieldInformation.PropertyInformation.GetValue(eventVariable, null);
                        valueAsString = ConvertRawPropertyValueToString(raw, ref fieldInformation);
                    }
                }
                catch (Exception)
                {
                    raw = null;
                }

                valueList.Add((UseTextQualifier) ? string.Format("\"{0}\"", valueAsString) : valueAsString);
            }
            return valueList;
        }

        /// <summary>
        /// Get a list of the field information to be used when generating the CSV fields associated with each event record.
        /// </summary>
        /// <returns>A <c>List</c> of <c>FieldInformation_t</c> structures that are to be used to generate the CSV fields for the EventRecord class.</returns>
        private List<FieldInformation_t> GetFieldInformationEventRecord()
        {
            // A list of the CSV fields that are to be output for each event record. 
            List<FieldInformation_t> cSVFieldInformationList = new List<FieldInformation_t>();

            Type type = typeof(EventRecord);

            FieldInformation_t fieldInformation;

            // fieldInformation = new FieldInformation_t(FieldNameCSVRecordType, typeof(string), null);
            // cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameLogIdentifier, typeof(int), null);
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameCarIdentifier, typeof(String), type.GetProperty(KeyPropertyNameEventRecordCarIdentifier));
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameDescription, typeof(string), type.GetProperty(KeyPropertyNameEventRecordDescription));
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameDate, typeof(DateTime), null);
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameTime, typeof(DateTime), null);
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameDay, typeof(string), null);
            cSVFieldInformationList.Add(fieldInformation);

            // fieldInformation = new FieldInformation_t(FieldNameIdentifier, typeof(short), type.GetProperty(KeyPropertyNameEventRecordIdentifier));
            // cSVFieldInformationList.Add(fieldInformation);

            // fieldInformation = new FieldInformation_t(FieldNameHelpIndex, typeof(int), type.GetProperty(KeyPropertyNameEventRecordHelpIndex));
            // cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameStreamSaved, typeof(string), null);
            cSVFieldInformationList.Add(fieldInformation);

            // fieldInformation = new FieldInformation_t(FieldNameStreamNumber, typeof(short), type.GetProperty(KeyPropertyNameEventRecordStreamNumber));
            // cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameEventVariableCount, typeof(int), null);
            cSVFieldInformationList.Add(fieldInformation);

            return cSVFieldInformationList;
        }

        /// <summary>
        /// Get a list of the field information to be used when generating the CSV fields associated with each event variable.
        /// </summary>
        /// <returns>A <c>List</c> of <c>FieldInformation_t</c> structures that are to be used to generate the CSV fields for the EventVariable class.</returns>
        private List<FieldInformation_t> GetFieldInformationEventVariable()
        {
            // A list of the CSV fields that are to be output for each event variable. 
            List<FieldInformation_t> cSVFieldInformationList = new List<FieldInformation_t>();

            Type type = typeof(EventVariable);

            FieldInformation_t fieldInformation;

            // fieldInformation = new FieldInformation_t(FieldNameCSVRecordType, typeof(string), null);
            // cSVFieldInformationList.Add(fieldInformation);

            // fieldInformation = new FieldInformation_t(FieldNameVariableType, typeof(VariableType), type.GetProperty(KeyPropertyNameEventVariableVariableType));
            // cSVFieldInformationList.Add(fieldInformation);

            // fieldInformation = new FieldInformation_t(FieldNameEventVariableIdentifier, typeof(short), type.GetProperty(KeyPropertyNameEventVariableIdentifier));
            // cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameName, typeof(string), type.GetProperty(KeyPropertyNameEventVariableName));
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameValueFromTarget, typeof(double), null);
            cSVFieldInformationList.Add(fieldInformation);

            fieldInformation = new FieldInformation_t(FieldNameUnits, typeof(string), type.GetProperty(KeyPropertyNameEventVariableUnits));
            cSVFieldInformationList.Add(fieldInformation);

            return cSVFieldInformationList;
        }

        /// <summary>
        /// Convert the raw property value object to a string depending upon the specified CSV field type.
        /// </summary>
        /// <param name="raw">The raw value of the property that is currently being processed.</param>
        /// <param name="fieldInformation">The <c>FieldInformation_t</c> structure that is currently being processed.</param>
        /// <returns>A string representation of the Property value.</returns>
        private static string ConvertRawPropertyValueToString(object raw, ref FieldInformation_t fieldInformation)
        {
            string valueAsString;

            if (raw == null)
            {
                valueAsString = string.Empty;
            }
            else
            {
                try
                {
                    // Convert the raw value to a string in accordance with the property type.
                    if (fieldInformation.CSVFieldType == typeof(UInt32))
                    {
                        UInt32 rawAsUInt32 = Convert.ToUInt32(raw);
                        valueAsString = CommonConstants.HexValueIdentifier + rawAsUInt32.ToString(CommonConstants.FormatStringHex + "8");
                    }
                    else if (fieldInformation.CSVFieldType == typeof(UInt16))
                    {
                        UInt16 rawAsUInt16 = Convert.ToUInt16(raw);
                        valueAsString = CommonConstants.HexValueIdentifier + rawAsUInt16.ToString(CommonConstants.FormatStringHex + (sizeof(UInt16) * 2).ToString());
                    }
                    else if (fieldInformation.CSVFieldType == typeof(byte))
                    {
                        byte rawAsByte = Convert.ToByte(raw);
                        valueAsString = CommonConstants.HexValueIdentifier + rawAsByte.ToString(CommonConstants.FormatStringHex + (sizeof(UInt16) * 2).ToString());
                    }
                    else if (fieldInformation.CSVFieldType == typeof(string))
                    {
                        valueAsString = raw.ToString();
                    }
                    else
                    {
                        valueAsString = raw.ToString();
                    }
                }
                catch (Exception)
                {
                    valueAsString = string.Empty;
                }
            }

            return valueAsString;
        }

        /// <summary>
        /// Convert from <c>DateTime</c> to Unix Timestamp where the Unix Timestamp is the number of seconds elapsed since 00:00:00 on 1st Jan 1970, i.e. the Unix
        /// Epoch, without counting leap seconds.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> object that is to be converted to a Unix Timestamp.</param>
        /// <returns>The Unix Timestamp</returns>
        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
        #endregion --- Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets or sets the separator character that is to be used between field values in the CSV file.
        /// </summary>
        public char Separator
        {
            get { return m_Separator; }
            set { m_Separator = value; }
        }

        /// <summary>
        /// Gets or set the flag that controls whether the field values are to be enclosed within double quotes i.e. "123.456".
        /// </summary>
        public bool UseTextQualifier
        {
            get { return m_UseTextQualifier; }
            set { m_UseTextQualifier = value; }
        }
        #endregion --- Properties ---
	}
}
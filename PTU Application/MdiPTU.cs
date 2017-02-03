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
 *  File name:  MdiPTU.cs
 * 
 *  Revision History
 *  ----------------
 */

#region - [1.0 to 1.19] -
/*
 *  Date        Version Author          Comments
 *  03/22/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  08/16/10    1.1     K.McD           1.  Bug fix SNCR 001.009. If the user exits the replay screen directly rather than returning back via the YT plot screen, 
 *                                          the function keys are not restored correctly. Added the ToolStripItemCollectionMainWindow property.
 * 
 *  08/18/10    1.2     K.McD           1.  Added support for the self-test menu options.
 * 
 *  09/30/10    1.3     K.McD           1.  Added support fot the 'Configuration/Enumeration' menu option.
 *                                      2.  Changed the name of a number of resource references.
 * 
 *  10/11/10    1.4     K.McD           1.  Bug fix SNCR001.27. Ensure that any menu options that are not yet implemented display a message box informing the user of
 *                                          their current status.
 * 
 *  10/15/10    1.5     K.McD           1.  Modified to use the CommunicationParent class rather than the ICommunication interface.
 * 
 *  11/02/10    1.6     K.McD           1.  Added support for the event menu interface.
 * 
 *  11/26/10    1.7     K.McD           1.  Removed the event handler for the 'Configure/Worksets' menu option.       
 *                                      2.  Added the event handler for the 'Configure/Worksets/Fault Log' menu option.
 *                                      3.  Added the event handler for the 'Configure/Worksets/Watch Window' menu option.
 * 
 *  01/06/11    1.8     K.McD           1.  Initialized the data dictionary filename to the default value - 'PTU Configuration.xml'.
 *                                      2.  Report an error if the specified configuration file canot be found.
 *                                      3.  Modified the Shown() event handler to check whether the default configuration file or a specific configuration file is to
 *                                          be loaded and then verify that the file exists.
 *                                      4.  Modified the 'File/Open/Event Log' menu option event handler to include a call to the OpenEventLog() method of the event
 *                                          menu interface.
 *                                      5.  Modifications arising from the name change of the ShowEventLog() method of the event interface to ViewEventLog().
 *                                      6.  Bug fix - SNCR001.47. Modified the m_TSBOnline_Click() event handler to ensure that a check on the version number of the 
 *                                          data dictionary is carried out when connecting to the target. If a mismatch is detected the PTU will attempt to load the 
 *                                          correct data dictionary.
 *                                      7.  Modified the m_TSBOffline_Click() event handler so that it no longer writes an empty string to the car identifier status
 *                                          label, this is now updated by the SetMode() method.
 * 
 *  02/14/11    1.9     K.McD           1.  Removed any references to the ISecurity interface and replaced it with a reference to the Security class.
 *                                      2.  Included support for the ByPassVersionCheck conditional compilation symbol.
 *                                      3.  Included support for the 'Tools/Debug Mode' menu option.
 *                                      4.  Removed the 'Configure/StartRTC' and 'Configure.StopRTC' menu options.
 *                                      5.  Removed the m_ProgressBar_VisibleChanged() event handler.
 * 
 *  02/22/11    1.10    K.McD           1.  Moved the Century constant to the CommonConstants class.
 *                                      2.  Modified a number of XML tags.
 *                                      3.  Changes to accomodate the modifications to the menu system.
 * 
 *  03/21/11    1.11    K.McD           1.  Modified the code such that the Security class is not instantiated until after the data dictionary has been loaded an a 
 *                                          call the the Security.Initialize() static method has been made.
 *                                      2.  Addde support for the Windows help engine.
 *                                      3.  Auto-modified as a result of a method name change associated with the MenuInterfaceApplication class.
 *                                      4.  Included support for the form used to configure the VCU date and time.
 * 
 *  03/28/11    1.11.1  K.McD           1.  Modified the name of a number of local variables.
 * 
 *  04/27/11    1.12    K.McD           1.  Modified the event handler for the on-line button to retrieve the mode of the chart recorder.
 *                                      2.  Added support for the menu options associated with the configuration of the chart recorder and setting the mode of the 
 *                                          chart recorder.
 *                                          
 *  06/22/11    1.12.1  K.McD           1.  Added support for the self test menu interface.
 *  
 *  07/13/11    1.12.2  K.McD           1.  Modified the function keys to take into account the redefinition of off-line mode and the addition of diagnostic mode 
 *                                          as discussed in the June sprint review.
 *                                          
 *  07/24/11    1.12.3  K.McD           1.  Added support for the KeyDown and KeyUp events.
 *                                      2.  Removed support for the diagnostic mode ToolStripButton control.
 *                                      3.  Implemented a toggle function for the on-line and off-line mode keys.
 *                                      4.  Ensured that the event handlers for the on-line and off-line ToolStripButton controls are inhibited if the ToolStripButton is 
 *                                          disabled. Note: The event handler is also called if the user presses a key rather than selecting the ToolStripButton control 
 *                                          using the mouse.
 *                                      5   Ensured that the Checked properties of the chart recorder mode menu options are initialized to false before the call to the 
 *                                          GetChartMode() method in the on-line and off-line event handlers.
 *                                          
 *  09/21/11    1.12.4  Sean.D          1.	Changed code in m_TSBOnline_Click to check for a CommunicationException when trying to close the socket so that 
 *											we don't get repeated errors trying to close a port that's already closed.
 *											
 *  10/10/11    1.12.5  K.McD           1.  Included support for the 'Help/PTU Help' menu option.
 *  
 *  10/26/11    1.12.6  K.McD           1.  Auto-modified as a result of enumerator name changes. Mode.Diagnostic renamed to Mode.Configuration.
 *  
 *  07/04/13    1.12.7  K.McD           1.  Included support for the WibuBox security device.
 *                                          1.  Added the IntervalMsWibuBoxUpdate constant.
 *                                          2.  Added the m_TimerWibuBox Timer.
 *                                          3.  Added the m_MenuInterfaceWibuKey reference to the MenuInterfaceWibuKey class.
 *                                          4.  Modified the Cleanup() method to include the WibuBox timer.
 *                                          5.  Modified the Cleanup() method to set the appropriate member variables to null.
 *                                          6.  Added the event handler for the WibuBox timer Tick event to check whether the WibuBox has been removed.
 *                                          7.  Modified the Shown event to check whether a WibuBox device is required for the current project and, if so, 
 *                                              to instantiate a MenuInterfaceWibuKey class and to initialize the WibuBox timer.
 *                                              
 *  07/26/13    1.12.8  K.McD           1.  Modified the Cleanup() method to close the communication port.
 *  
 *  07/31/13    1.12.9  K.McD           1.  Added the FilenameDataDictionary property in accordance with the modified IMainWindow interface definition.
 *                                      2.  Modified the constructor that is called if any command line arguments are passed to the PTU to update the
 *                                          FilenameDataDictionary property with the filename associated with the project XML data dictionary file if a valid project
 *                                          XML data dictionary file is found.
 *                                      3.  Modified the m_TSBOnline_Click() method to copy the source file to the file that is defined as the current XML data
 *                                          dictionary file rather than the default XML data dictionary file if a new XML data dictionary file is selected as a 
 *                                          result of a mismatch between the embedded software version reference and the current data dictionary version reference.
 *                                          
 *  08/02/13    1.12.10 K.McD           1.  Bug fix - The font associated with the user message did not change when the font was modified using the Tools/Options menu.
 *                                          Modified the  MdiPTU_FontChanged() method to update the Font property of the user message StatusStrip control.
 *                                          
 *  02/27/14    1.12.11 K.McD           1.  Corrected 'Y' Location values in MdiPTU.resx for m_StatusStripCurrentSetup (4), m_StatusStripUserMessage (4), 
 *                                          m_LegendStatusInformation (6), m_LegendRx (6) and m_DigitalControlPacketReceived (9) as these 
 *                                          values had somehow been corrupted.
 *                                          
 *                                      2.  Updated the m_MenuItemHelpAboutPTU_Click() method to use the renamed m_MenuInterfaceApplication.HelpAbout() method.
 *                                      
 *  04/15/15    1.13    K.McD           References
 *                                          
 *                                      1.  Although only relevant to Bombardier Field Service Engineers that support a number of PTU projects, the Software User Manual
 *                                          and Release Notes documents are to be made project specific by prepending the project identifier to the file name e.g.
 *                                          'R8PR.Portable Test Unit - Release Notes.pdf', 'CTA.Portable Test Unit - Release Notes.pdf' etc.
 *                                          
 *                                      Modifications
 *                                      1.  Removed the member variable m_HelpDocument.
 *                                      
 *  05/13/15    1.14    K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *                                          
 *                                          1.  NK-U-6505 Section 2.2. Mandatory MDI Global Screen Representation - Part 1. The proposal is to add additional status
 *                                              labels to the status bar at the bottom of the PTU screen to include ‘Log: [Saved | Unsaved]’ and
 *                                              ‘WibuBox: [Present | Not Present]’.
 *                                              
 *                                      2.  SNCR - R188 PTU [20-Mar-2015] Item 4. If the PTU is being used in a development environment, i.e. there is a possibility of
 *                                          switching between multiple projects, and the PTU is started up using the R188 configuration but the user agrees to switch to
 *                                          the CTA configuration when an attempt is made to connect to a CTA VCU, there is a bug which results in the PTU continuing to
 *                                          check whether a Wibu Key is present.
 *                                          
 *                                      Modifications
 *                                      1.  Added the LogSaved and WibuBoxPresent flag properties and associated member variable flags. Ref.: 1.1.
 *                                      2.  Modified the MdiPTU(string[] args) constructor to assert the Visible property of the WibuBox status label if the
 *                                          project associated with the project-identifier shortcut parameter requires a WibuBox to be present. Ref.: 1.1.
 *                                      3.  Auto-Update as result of name changes to the status label controls. Ref.: 1.1.
 *                                      4.  Modified the WibuBoxCheck() method to update the WibuBox status label if the WibuBox has been removed. Ref.: 1.1.
 *                                      5.  Changed the definition of the TaskProgressBar property to ToolStripProgressBar. Ref.: 1.1.
 *                                      6.  Moved the section of code in the 'Shown' event handler that is associated with the WibuBox to the LoadDictionary()
 *                                          method. Ref.:2.
 */

/*
 *  07/13/15    1.15    K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *                                          
 *                                          1.  NK-U-6505 Section 2.2. Mandatory MDI Global Screen Representation - Part 1. Following a conference call on 9-Jul-15,
 *                                              the current proposal is to extend the options associated with the log saved status StatusLabel to include:
 *                                              ‘[Saved | Unsaved | Unknown | Not Applicable (-)]’.
 *                                              
 *                                          2.  NK-U-6505 Section 2.2. Mandatory MDI Global Screen Representation - Part 2.  Addition of the Control Panel window.
 *                                          
 *                                      Modifications
 *                                      1.  Replaced the LogSaved property and associated variable with the LogStatus property and its associated variable. This property
 *                                          gets and sets the event log saved status associated with the current car. The LogStatus StatusLabel is also updated
 *                                          whenever the property is written to. This property was added to the IMainWindow interface. - Ref.: 1.1.
 *                                          
 *                                      2.  Added the CarNumber property and associated variable. This property gets the current car number if the PTU is connected to
 *                                          the target logic. If not connected to the car logic, the value that will be returned is short.MinVal. This property was added
 *                                          to the IMainWindow interface.  Ref.: 1.1.
 *                                          
 *                                      3.  Removed the set MenuStrip property and added the get and set StatusStrip property. This property was added to the IMainWindow
 *                                          interface. - Ref.: 1.2.
 *                                          
 *                                      4.  Modified the event handlers for the 'Online' and 'Offline' ToolStripButtons to only display the Watch Window when the unit
 *                                          goes online if the current project uses a control panel. - Ref.: 1.2.
 *                                          
 *                                      5.  Updated the 'Online' event handler to toggle the LogStatus value between 'Not Applicable ("-")' and 'Unknown' depending
 *                                          upon the Checked state. - Ref.: 1.1
 *                                          
 *                                      6.  Modified the Cleanup() method the include the ControlPanel UserControl. Also sets the cursor to the Cursors.WaitCursor
 *                                          value as soon as the Cleanup() method is called. - Ref.: 1.2.
 *                                          
 *                                      7. Removed the section of code relating to the WibuBox StatusLabel from the constructor. - Ref.: 1.2.
 */

/*
 *  07/28/15    1.16    K.McD           References
 *                                      1.  Part 1 of the upgrade to the Chicago 5000 PTU software that allows the user to download the configuration and help files for
 *                                          a particular Chicago 5000 vehicle control unit (VCU) via an ethernet connection to the FTP (File Transfer Protocol) server
 *                                          running on the VCU. The scope of Part 1 of the upgrade is defined in purchase order 4800011369-CU2 07.07.2015.
 *                                      
 *                                          The upgrade is implemented in two parts, the first part, Part 1, replaces the existing screens and logic with those outlined
 *                                          in slides 6, 7, 8 and 9 of the PowerPoint presentation '076_CTA - PTU file pullback from VCU - 20150127.pptx', but does NOT
 *                                          implement the file transfer; it merely calls an empty external batch file from within the PTU application. The second stage,
 *                                          Part 2, implements the batch file that downloads the configuration and help files from the Vehicle Control Unit (VCU) to the
 *                                          appropriate directory on the PTU computer. As described in the PowerPoint Presentation, this download is only carried out
 *                                          if the appropriate configuration file is not already present on the PTU computer.
 *                                      
 *                                      Modifications
 *                                      1.  Added the FTPError enumerator.
 *                                      2.  Added the Restart and ProjectIdentifierPassedAsParameter properties and associated member variables.
 *                                      3.  Removed the ToolStripProgressBar TaskProgressBar. The progress bar used to display the recording and playback of data
 *                                          streams now appears in the 'Information' Panel of the FormWatch Form. The progress bar was moved to allow the status
 *                                          message display to be extended to support some of the longer messages required to support the upgrade shown above.
 *                                      4.  Replaced the local projectIdentifier string with the m_ProjectIdentifierPassedAsParameter member variable and modified
 *                                          the error message in the MdiPTU(string[] args) constructor.
 *                                      5.  Refactored the m_TSBOnline_Click() event handler to create the CheckConfiguration() and UpdateChartMode() methods.
 *                                      6.  Modified the CheckConfiguration() method to implement Part 1 of reference 1.
 */

/*
 *  08/11/15    1.17    K.McD       References
 *                                  1.  Changes resulting from documents: 'PTU MOC Findings - .docx' and 'PTU Installation on 64-bit Machine_v1-08022015.docx' sent
 *                                      from Atul Chaudhari on 4th Aug 2015 and the follow up email sent on 5th Aug 2015.
 *                                          
 *                                      1.  On the R188 project only, all references to 'PTU' are to be replaced with 'PTE' and all occurrences of 'Portable Test Unit'
 *                                          are to be replaced with 'Portable Test Equipment'. Where support for multiple legends is not possible, 'Portable Test
 *                                          Application' is to be used as an alternative to 'Portable Test Unit'/'Portable Test Equipment.
 *                                      
 *                                  2.  Bug Fix - SNCR - R188 PTU [20-Mar-2015] Item 28. If the project requires a Wibu-Key and the files ‘Configuration.xml’ and
 *                                      ‘project-identifier.Configuration.xml’ do not exist, there is a problem trying to log on as a Factory user in order to
 *                                      select the required data dictionary file.
 *                                          
 *                                      As the project-identifier is now passed as a desktop shortcut parameter, the Wibu-Key timer is initialized in the MDI
 *                                      constructor, if required; as soon as the user tries to log on they are automatically logged out as the initialized timer
 *                                      calls the WibuBoxCheckIfRemoved() method which returns a value of true because the FormLogin Form is instantiated without
 *                                      first calling the WibuBoxCheckForValidEntry() method as the Parameter.ProjectInformation.ProjectIdentifier parameter used
 *                                      in the call to WibuBoxCheckIfRequires() is still set to string.Empty at that stage as no data dictionary had been selected.
 *  
 *                                  Modifications
 *                                  1.  Modified the m_MenuItemLogin_Click() event handler to pass the 'm_ProjectIdentifierPassedAsParameter' member variable in the
 *                                      call to the MenuInterfaceApplication.Login() method. - Ref.: 2.
 *                                      
 *                                  2.  Removed the reference to 'PTU' from the 'About PTU' and the 'PTU Help' menu options. - Ref.: 1.1.
 */

/*
 *  08/26/15    1.18    K.McD       References
 *                                  1.  Part 2 of the upgrade to the Chicago 5000 PTU software that allows the user to download the configuration and help files for
 *                                      a particular Chicago 5000 vehicle control unit (VCU) via an ethernet connection to the FTP (File Transfer Protocol) server
 *                                      running on the VCU. The scope of Part 2 of the upgrade is defined in purchase order  4800011369-CU2 28.07.2015.
 *                                      
 *                                      The upgrade is implemented in two parts, the first part, Part 1, replaces the existing screens and logic with those outlined
 *                                      in slides 6, 7, 8 and 9 of the PowerPoint presentation '076_CTA - PTU file pullback from VCU - 20150127.pptx', but does NOT
 *                                      implement the file transfer; it merely calls an empty external batch file from within the PTU application. The second stage,
 *                                      Part 2, implements the batch file that downloads the configuration and help files from the Vehicle Control Unit (VCU) to the
 *                                      appropriate directory on the PTU computer. As described in the PowerPoint Presentation, this download is only carried out if the
 *                                      appropriate configuration file is not already present on the PTU computer.
 *                                      
 *                                  Modifications
 *                                  1.  Renamed the Process from  'batchProcess' to 'commandProcess' in method  CheckConfiguration().
 *                                  2.  Although not used at present, updated the error codes associated with the 'FTPError' enumerator. These represent the 
 *                                      possible error codes that may be returned from the Windows Command File 'FTP Transfer.cmd'.
 *                                  3.  Modified the CheckConfiguration() method to report the error code that was returned from the Windows Command File if
 *                                      the transfer was not successful.
 */

/*
 *  09/30/15    1.19    K.McD       References
 *                                      
 *                                  1.  Part 2 of the upgrade to the Chicago 5000 PTU software that allows the user to download the configuration and help files for
 *                                      a particular Chicago 5000 vehicle control unit (VCU) via an ethernet connection to the FTP (File Transfer Protocol) server
 *                                      running on the VCU. The scope of Part 2 of the upgrade is defined in purchase order  4800011369-CU2 28.07.2015.
 *                                      
 *                                  Modifications
 *                                  1.  Renamed the FTPError enumerator to FTPErrorCode and moved this to the newly created FtpErrorProcessing class.
 *                                  2.  Auto-update of the CheckConfiguration() method associated with changing the name of the FTPError enumerator to FTPErrorCode.
 *                                  3.  Modified the CheckConfiguration() method to:
 *                                      1.  Add the instruction '3. Move the mouse to the 'virtual machine' down arrow box at the top of the screen. Click the down arrow
 *                                          box once and select 'removable devices'. Click on 'network adapter' and ensure that the text indicates 'disconnect'. If true,
 *                                          no further action is required. If it indicates 'connect', then click 'connect' and the network adapter will be enabled'.
 *                                      2.  Change this message type to 'Information'.
 *                                      3.  Report the actual error code and associated message returned from the Windows Command File in the event that the FTP download
 *                                          from the VCU is unsuccessful.
 *                                      4.  Check that the required XML configuration file and associated help file were downloaded from the VCU and, if not, to report
 *                                          the error.
 * 
 */
#endregion - [1.0 to 1.19] -

#region - [1.20] -
/*
 *  Date        Version Author      Comments
 *  03/15/2016  1.20    K.McD       References
 *                                  1.  Bug fix - SNCR - PTU [01 Mar 2016] - Item 12. There is an issue with saving fault/event logs on BART. BART has car IDs that are
 *                                      "prepended" with the letter "D" or "E" (e.g D1234, E4321). When trying to save a fault log, an exception is thrown in file
 *                                      "FormViewEventLog.cs" when executing the conditional check: 
 *                                      
 *                                          if (eventLogFile.EventRecordList[0] != Settings.Default.MostRecentDownloadedEventsSaved.DownloadedEvents
 *                                          [MainWindow.CarNumber])
 *                                          {
 *                                              ...
 *                                          }
 *                                          
 *                                      as MainWindow.CarNumber is not successfully converted to an integer because of the "D" or "E", hence the exception being thrown.
 *                                      
 *                                  Modifications
 *                                  1.  Corrected Revision History entry associated with revision 1.19.
 *                                  2.  Added the CarNumberMax UInt16 constant.
 *                                  3.  Changed the definition of the CarNumber property and associated member variable from short to UInt16.
 */
#endregion - [1.20] -

#region - [1.21] -
/*
 * 05/16/2016   1.21        K.McD   References
 *                                  1.  BART 071-ICD-0011 PTU Requirement and Interface Description document [REQ-72] and [REQ-78]. These requirements apply to 
 *                                      Windows-based PTU applications only.
 *                                      
 *                                      1.  [REQ-72] As a minimum, the Windows based PTU shall have the access level as a command line parameter to allow third party
 *                                          software to launch it directly with the required access level.
 *                                          
 *                                          The parameter to define the access level shall be -l number with number being the access level (either "2" or "3", as
 *                                          defined in [REQ-25] and [REQ-27]).
 *                                          
 *                                          Where no access level is passed, the PTU shall query Windows for the user group that the current user belongs to (refer to
 *                                          [REQ-78]) or otherwise deny access to all functions.
 *                                          
 *                                      2.  [REQ-78] Users on the PTU laptop are members of the user group PrimaryMaintenance or Engineering. If no access level is passed
 *                                          (refer to [REQ-72]) the PTU application shall query Windows for the user group that the current user belongs to. If the user
 *                                          is part of the Engineering group, access according to Level 3 shall be granted. If the user is not part of the Engineering
 *                                          group, but part of the PrimaryMaintenance group, access according to Level 2 shall be granted. If the user is not part of
 *                                          any of those two groups, access to all functions shall be denied.
 * 
 *                                  Modifications
 *                                  1.  Added the SecurityLevelPassedAsParameter property and associated member variable and initialized this to SecurityLevel.Undefined.
 *                                  2.  Added the m_HideLogin control flag. This controls whether the Login menu option is hidden or not. Tru, to hide the Login menu
 *                                      option; otherwise, false.
 *                                  3.  Extensive modifications to the 'MdiPTU(string[] args)' constructor to ensure that the project identifier command line parameter
 *                                      is passed to the application and to check whether, for the BART project, the start-up security level was passed as a command
 *                                      line parameter. Also includes changes to terminate the program if: (a) no project identifier command line parameter is passed to
 *                                      the application or, (b) for the BART project, no, or a an invalid start-up security level is passed to the application and
 *                                      the current user is not a member of the PrimaryMaintenance or Engineering Windows group.
 *                                  4.  Modified the MdiPTU_Shown() method to: (a) include an Assert statement to ensure that a valid project data dictionary has been
 *                                      defined, and (b) to close the application if the project data dictionary XML file cannot be read.
 *                                  5.  Added the UpdateFilenameDateDictionaryProperty() method.
 */
#endregion - [1.21] -

#region - [1.22] -
/*
 * 06/07/2016   1.22        DAS     References
 *                                  1.  PTU Conference call Thu 7th July 2016. Request to Change the error message text when there is a mismatch between the PTU data
 *                                      dictionary and the target software versions. The error message is to show both the database (XML) expected target software version
 *                                      and the actual target software version.
 *                                      
 *                                  Modifications
 *                                  1.  Modified the CheckConfiguration() method to change the error message that is displayed when there is a mismatch between the
 *                                      PTU data dictionary and the target software versions. The message now shows both the database (XML) expected target software
 *                                      version and the actual target software version.                                 
 */

/*
 * 07/21/2016   1.22.1      K.McD   References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 11. Move the 'Real Time Clock' menu option from the 'Configure' drop-down menu to the
 *                                      'Tools' drop-down menu.
 *                                      
 *                                  2.  PTE Changes - List 5-17-2016.xlsx Item 12. Under the 'Help' drop-down menu, add a 'Show User Manual menu option above the 'About'
 *                                      menu option with a link to the 'Software User Manual'.
 *                                      
 *                                  3.  PTE Changes - List 5-17-2016.xlsx Item 4. Monitor the Wibu-Key continuously while not connected to the target logic.
 *                                  
 *                                  4.  PTE Changes - List 5-17-2016.xlsx Item 10, 49, 58. The 'Configure/Enumeration' drop-down menu option is to be replaced by
 *                                      individual ToolStripButton controls on the 'Watch Window', 'Event Log' and 'Self Test' screens.
 *                                  
 *                                  Modifications
 *                                  1.  Renamed the m_MenuItemConfigureRealTimeClock_Click() event handler to m_MenuItemToolsRealTimeClock_Click() and moved it to the
 *                                      '[TOOLS]' region. - Ref.: 1.
 *                                  2.  Auto-Update -  m_MenuInterfaceApplication.ConfigureRealTimeClock() renamed to  m_MenuInterfaceApplication.RealTimeClock().
 *                                      - Ref.: 1.
 *                                  3.  Renamed the m_MenuItemHelpPTUHelp_Click() event handler to  m_MenuItemHelpShowUserManual_Click(). - Ref.: 2.
 *                                  4.  Modified WibuBoxCheck() to check the state of the WibuBoxPresent flag and either: (a) check whether the WibuBox has been removed,
 *                                      if a WibuBox was present on the last check; or (b) check whether a WibuBox is now present, if no WibuBox was found on the last
 *                                      check. The user is only informed that the WibuBox has been removed if they are currently logged into the Engineering account,
 *                                      or higher. - Ref.: 3.
 *                                  5.  Removed the Enumeration property and associated member variable. - Ref.: 4.                            
 */

/*
 *  09/12/2016  1.22.2      K.McD   References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 17. Add the options for Chart Mode from the original Configure drop-down menu as buttons 
 *                                      on the dialogbox used to configure the chart recorder.
 *                                      
 *                                  2.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 23. Only ask the user if they wish to terminate the application if they select
 *                                      the 'File/Exit' menu option or select the [x] button. If the close is as a result of a fatal error just close the program
 *                                      regardless.
 *                                  
 *                                  Modifications
 *                                  1.  Removed the menu options, separators and associated event handlers for: 'Chart Mode', 'Chart Mode - Ramp', 'Chart Mode - Data',
 *                                      'Chart Mode - Zero Output' and 'Chart Mode - Full Scale'. - Ref.: 1.
 *                                  2.  Set the CommunicationInterface property to null in the 'finally' statement of the 'Online' and 'Simulation/Offline' event
 *                                      handlers. - Ref.: 1.
 *                                  3.  Removed the calls to UpdateChartMode() in the 'Online' and 'Simulation/Offline' event handlers as this is now managed from
 *                                      within the FormConfigureChartRecorder dialogbox. - Ref.: 1.
 *                                  4.  Removed the UpdateChartMode() method. - Ref.: 1.
 *                                  5.  Replaced all calls to the Dispose(true) method in the constructor with calls to the Close() method. - Ref.: 2.
 *                                  6.  Added the DisplayQueryExit property and associated member variable and set it to the appropriate value immediately prior to
 *                                      making any call to the Close() method. - Ref.: 2.
 *                                  7.  Detached all event handlers and set all Windows Form Designer variables to null in the Cleanup() method. Internal Audit of Code.
 *                                  8.  Added the FormClosing() event handler. This works in conjunction with the DisplayQueryExit property. - Ref.: 2.
 */
#endregion - [1.22] -

#region - [1.23] -
/*
 * 10/07/2016   1.23        D.Smail References
 *                                  1.  BART 071-ICD-0011 PTU Requirement and Interface Description document requires an index and searchable help utilizing a 
 *                                      Windows help file (.chm)
 *                                      
 *                                  Modifications
 *                                  1.  Pass the form instance ("this") to ShowUserManual call in function m_MenuItemHelpShowUserManual_Click(). In order to
 *                                      spawn a chm file, the control/form must be known.
 */

/*
 *  10/14/2016  1.23.1      K.McD   References
 *                                  1.  Conference Call 3rd Oct 2016. If the desktop shortcut does not pass the security level as a desktop shortcut parameter and the
 *                                      current user is not a member of the PrimaryMaintenance or Engineering Windows user group. then, intead of terminating the
 *                                      application, the application should revert to the standard login procedure using the 'Login' menu option. The application should
 *                                      also revert to the standard login procedure if the security level desktop parameter is invalid.
 *                                  
 *                                  Modiications
 *                                  1.  Modified the constructor such that, if a security level desktop parameter was not specified by the desktop shortcut and the
 *                                      current user is not defined as part of the PrimaryMaintanance or Engineering Windows user group then the application
 *                                      informs the user and reverts to the standard login procedure using the 'Login' menu option. The application also reverts
 *                                      to the standard login procedure if the security level desktop parameter is invalid.
 */
#endregion - [1.23] -
#endregion --- Revision History ---

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Bombardier.PTU.Forms;
using Bombardier.PTU.Properties;
using Common;
using Common.Communication;
using Common.Configuration;
using Event;
using SelfTest;
using Watch;
using WibuKey;

namespace Bombardier.PTU
{
    /// <summary>
    /// Main user interface for the Portable Test Unit (PTU) application.
    /// </summary>
    public partial class MdiPTU : Form, IMainWindow
    {
        #region --- Constants ---
        /// <summary>
        /// Th interval, in ms, between successive checks to see whether the WibuBox is still attached to the system. Value: 5,000.
        /// </summary>
        private const int IntervalMsWibuBoxUpdate = 5000;

        /// <summary>
        /// The Maximum car number value associated with any project. This is derived from the car identifier string that is downloaded from the VCU/COMC unit. On most
        /// projects this is a string representation of a numeric value, however, on the BART project this numeric value is prepended with either 'D' or 'E' depending
        /// upon the car type. Despite being prepended with either 'D' or 'E' the numeric value is still unique, therefore the car number on BART is derived by simply
        /// stripping off the car type character. Value: 9999.
        /// </summary>
        private const UInt16 CarNumberMax = 9999;
        #endregion --- Constants ---

        #region --- Member Variables ---
        /// <summary>
        /// A static flag that controls whether the PTU does an automatic restart when the main PTU application is closed. True, if the PTU is to do an automatic
        /// restart; otherwise, false.
        /// </summary>
        public static bool m_Restart = false;

        /// <summary>
        /// Flag to indicate whether the Dispose() method has been called. True indicates that the Dispose() method has been called; otherwise, false.
        /// </summary>
        private bool m_IsDisposed;

        /// <summary>
        /// A flag that controls whether the user is asked to confirm that they wish to close the application. True, asks the user to confirm that they wish to
        /// close the application; otherwise, false to close regardless.
        /// </summary>
        private bool m_DisplayQueryExit = true;

        /// <summary>
        /// The project identifier that was passed to the application as a command line parameter. If no command line parameter is passed to the
        /// application then the value is set to string.Empty.
        /// </summary>
        private string m_ProjectIdentifierPassedAsParameter = string.Empty;

        /// <summary>
        /// The security level that was passed to the application as a command line parameter. If no command line parameter is passed to the
        /// application then the value is set to <c>SecurityLevel.Undefined</c>.
        /// </summary>
        private SecurityLevel m_SecurityLevelPassedAsParameter = SecurityLevel.Undefined;

        /// <summary>
        /// The timer that is used to check that the WibuKey security device is still attached to the system.
        /// </summary>
        private Timer m_TimerWibuBox;

        /// <summary>
        /// Reference to the KeyEventArgs object associated with the last recorded key press. 
        /// </summary>
        private KeyEventArgs m_KeyEventArgs;

        /// <summary>
        /// The <c>DataSet</c> used to store the information loaded from the XML data dictionary.
        /// </summary>
        private DataDictionary m_DataDictionary;

        /// <summary>
        /// The filename of the XML data dictionary file.
        /// </summary>
        private string m_FilenameDataDictionary = Resources.FilenameDefaultDataDictionary;

        /// <summary>
        /// The current mode of operation: setup, online, diagnostic or offline.
        /// </summary>
        private Mode m_Mode;

        /// <summary>
        /// Reference to the <c>Security</c> class associated with the PTU.
        /// </summary>
        private Security m_Security;

        /// <summary>
        /// Reference to the selected communication interface.
        /// </summary>
        private ICommunicationParent m_CommunicationInterface;

        /// <summary>
        /// Reference to the class which calls the menu options associated with the main PTU application. 
        /// </summary>
        private MenuInterfaceApplication m_MenuInterfaceApplication;

        /// <summary>
        /// Reference to the class which calls the menu options associated with the subsystem which shows and records the watch variables - Watch.dll.
        /// </summary>
        private MenuInterfaceWatch m_MenuInterfaceWatch;

        /// <summary>
        /// Reference to the class which calls the menu options associated with the events subsystem - Events.dll.
        /// </summary>
        private MenuInterfaceEvent m_MenuInterfaceEvent;

        /// <summary>
        /// Reference to the class which calls the menu options associated with the self-test subsystem - SelfTest.dll.
        /// </summary>
        private MenuInterfaceSelfTest m_MenuInterfaceSelfTest;
 
        /// <summary>
        /// Reference to the class which calls the menu options associated with the WibuKey subsystem - WibuKey.dll.
        /// </summary>
        private MenuInterfaceWibuKey m_MenuInterfaceWibuKey; 

        /// <summary>
        /// The collection of function keys associated with the form. This allows any child form that is called indirectly to restore the function keys 
        /// on exit.
        /// </summary>
        private ToolStripItemCollection m_ToolStripItemCollectionMainWindow;

        /// <summary>
        /// Flag to control whether logging of the parameter values associated with the calls to those methods within the PTUDLL32 dynamic link library is enabled. 
        /// True, to enable debug mode; otherwise, false.
        /// </summary>
        private bool m_DebugMode = false;

        /// <summary>
        /// Flag to control whether the animation showing that the PTU is busy processing data is visible. True, to show the animation; otherwise, false.
        /// </summary>
        private bool m_ShowBusyAnimation = false;

        /// <summary>
        /// The event log saved saved associated with the current car.
        /// </summary>
        private EventLogSavedStatus m_EventLogSavedStatus = EventLogSavedStatus.Undefined;

        /// <summary>
        /// Flag to indicate whether a WibuBox security device is present or not. True, if a WibuBox security device is present; otherwise, false.
        /// </summary>
        private bool m_WibuBoxPresent = false;

        /// <summary>
        /// The current car number, if the PTU is connected to the target logic; otherwise, if not connected to the car logic, the vale that is returned is
        /// short.MinVal.
        /// </summary>
        private UInt16 m_CarNumber = UInt16.MinValue;

        /// <summary>
        /// Flag to control whether the 'Login' menu option is to be hidden. True if the 'Login' menu option is to be hidden; otherwise, false.
        /// </summary>
        /// <remarks>At present this is only applicable to the BART project. The client wants the start-up security level to be specified as a command line
        /// parameter.</remarks>
        private bool m_HideLogin = false;
        #endregion --- Member Variables ---

        #region --- Constructors ---
        /// <summary>
        /// Initializes a new instance of the class. This constructor is called if one or more shortcut parameters are passed to the PTU by the desktop shortcut.
        /// </summary>
        /// <param name="args">An array of the parameter strings that were passed to the PTU by the desktop shortcut. Note: only one parameter, arg[0], should be passed
        /// and this must specify the project identifier e.g. "CTPA", "R8PR" etc</param>
        public MdiPTU(string[] args) : this()
        {

            // The format of the command line parameters are as follows. PTU.exe <project-identifier> [-l<security-level>] e.g. PTU.exe BART -l3. The first command
            // line parameter is obligatory for ALL projects and must be a valid project identifier e.g. CTPA, TOPC, R8PR etc. The second command line parameter is:
            // optional, only applies to the BART project, and defines the start-up security level. The second command line parameter, if specified, must start with 
            // '-' and have the name l or L. If whitespace is left between the l and the <security-level> no enclosing quotation marks are required. e.g. 
            // PTU.exe BART -L3, PTU.exe BART -L 3, PTU.exe BART -l3 and PTU.exe BART -l 3 are all valid. If the command line doesn't include a project identifier or 
            // the -l parameter precedes it, the command line is invalid.

            // Used to record information relating to the call to Regex.Match().
            Match match;

            // The string used to store the concatenated command line parameters.
            string commandLineString = string.Empty;

            // Concatenate the command line parameters.
            for (uint argNumber = 0; argNumber < args.Length; argNumber++)
            {
                commandLineString += args[argNumber].ToString();
            }

            // Check that the first command line parameter matches the criterion of a project identifier.
            // Specify the pattern for the project identifier, it must have exactly 4 alpha-numeric characters.
            string regexPattern = string.Empty;
            regexPattern = @"\b[A-Za-z0-9][A-Za-z0-9][A-Za-z0-9][A-Za-z0-9]\b";

            // Extract the project identifier command line parameter.
            match = Regex.Match(commandLineString, regexPattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            // Check whether a match for the project identifier is found and that the matched string starts at the first character of the command line string. 
            if ((match.Success == true) && (match.Index == 0))
            {
                if (UpdateFilenameDateDictionaryProperty(match.Value) != true)
                {
                    // Inform the user that the data dictionary for the specified project does not exist.
                    MessageBox.Show(Resources.MBTConfigDataDictionaryDoesNotExist, Resources.MBCaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DisplayQueryExit = false;
                    Close();
                    return;
                }

                // Check whether we need to check for a security level. This is only applicable for the BART project.
                switch (match.Value)
                {
                    case CommonConstants.ProjectIdBART:
                        // Yes, check whether a security level has been specified.

                        // Specify the pattern for the start up security level. This parameter is optional but if present it must take the form '-l<security-level>' e.g.
                        // -l3 or -l 3. No enclosing quotes are requires if a whitespace character is included between the l and the <security-number>.
                       regexPattern = @"\b-[lL][2-4]\b";

                        // Extract the <security-level> command line parameter.
                        match = Regex.Match(commandLineString, regexPattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                        if (match.Success == true)
                        {
                            string passedSecurityLevelAsString = match.Value.Substring(2, 1);
                            uint passedSecurityLevelAsUint;

                            try
                            {
                                passedSecurityLevelAsUint = uint.Parse(passedSecurityLevelAsString);

                                // The security levels defined by the client for the BART project are: Level 2 and Level 3, these correspond to the PTU security
                                // levels: SecurityLevel.Level1 and SecurityLevel.Level2 respectively. An additional security level, Level 4 has been added to
                                // allow for PTU security level: SecurityLevel.Level3.
                                m_SecurityLevelPassedAsParameter = (SecurityLevel)(passedSecurityLevelAsUint - 1);
                                m_HideLogin = true;
                            }
                            catch (Exception)
                            {
                                m_SecurityLevelPassedAsParameter = SecurityLevel.Undefined;
                                MessageBox.Show(Resources.MBTConfigSecurityLevelPassedAsParameterInvalid, Resources.MBCaptionError, MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // A valid security level was specified in the command line, set the security level depending upon the Windows User Group.
                            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
                            WindowsPrincipal principal = new WindowsPrincipal(currentUser);
                            if (principal.IsInRole(CommonConstants.UserGroupBARTLevel2) == true)
                            {
                                m_SecurityLevelPassedAsParameter = SecurityLevel.Level2;
                                m_HideLogin = true;
                            }
                            else if (principal.IsInRole(CommonConstants.UserGroupBARTLevel1) == true)
                            {
                                m_SecurityLevelPassedAsParameter = SecurityLevel.Level1;
                                m_HideLogin = true;
                            }
                            else
                            {
                                m_SecurityLevelPassedAsParameter = SecurityLevel.Undefined;
                                MessageBox.Show(Resources.MBTSecurityCurrentUserNotGroupMember, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // Inform the user that a valid project identifier was not passed as a command line parameter.
                MessageBox.Show(Resources.MBTConfigProjectIDPassedAsParameterNotFound, Resources.MBCaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DisplayQueryExit = false;
                Close();
            }
        }

        /// <summary>
        /// Initialises a new instance of the class. 
        /// </summary>
        public MdiPTU()
        {
            InitializeComponent();
            
            InitializePTU();
        }
        #endregion  --- Constructors ---

        #region --- Cleanup ---
        /// <summary>
        /// Clean up the resources used by the form.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Cleanup(bool disposing)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Provided that the form isn't minimized, update the WindowState setting.
                if (this.WindowState != FormWindowState.Minimized)
                {
                    Settings.Default.WindowState = this.WindowState;
                    Settings.Default.Save();
                }

                // If the WindowState property is normal, update the: Location and Size settings.
                if (this.WindowState == FormWindowState.Normal)
                {
                    Settings.Default.FormLocation = this.Location;
                    Settings.Default.FormSize = this.Size;
                    Settings.Default.Save();
                }

                CloseChildForms();
                DebugMode.Close();
                WinHlp32.Close(this.Handle.ToInt32());

                // Ensure that the communication port is closed.
                if (m_CommunicationInterface != null)
                {
                    CommunicationInterface.CloseCommunication(CommunicationInterface.CommunicationSetting.Protocol);
                }

                if (disposing)
                {
                    // Method called by consumer code. Call the Dispose method of any managed data members that implement the dispose method.
                    // Cleanup managed objects by calling their Dispose() methods.
                    if (components != null)
                    {
                        components.Dispose();
                    }

                    if (m_DataDictionary != null)
                    {
                        m_DataDictionary.Dispose();
                    }

                    if (m_ControlPanel != null)
                    {
                        m_ControlPanel.Dispose();
                    }

                    if (m_TimerWibuBox != null)
                    {
                        m_TimerWibuBox.Stop();
                        m_TimerWibuBox.Enabled = false;
                        m_TimerWibuBox.Tick -= new EventHandler(WibuBoxCheck);
                        m_TimerWibuBox.Dispose();
                    }
                }

                // Whether called by consumer code or the garbage collector free all unmanaged resources and set the value of managed data 
                // members to null.
                m_DataDictionary = null;
                m_TimerWibuBox = null;
                m_ControlPanel = null;
                m_CommunicationInterface = null;
                m_MenuInterfaceApplication = null;
                m_MenuInterfaceEvent = null;
                m_MenuInterfaceSelfTest = null;
                m_MenuInterfaceWatch = null;
                m_MenuInterfaceWibuKey = null;
                m_Security = null;

                #region - [Windows Form Designer generated code] -
                #region - [Detach the event handler methods.] -
                this.m_MenuItemFileOpenWatchFile.Click -= new System.EventHandler(this.m_MenuItemFileOpenWatchFile_Click);
                this.m_MenuItemFileOpenEventLog.Click -= new System.EventHandler(this.m_MenuItemFileOpenEventLog_Click);
                this.m_MenuItemFileOpenFaultLog.Click -= new System.EventHandler(this.m_MenuItemFileOpenFaultLog_Click);
                this.m_MenuItemFileOpenSimulatedFaultLog.Click -= new System.EventHandler(this.m_MenuItemFileOpenSimulatedFaultLog_Click);
                this.m_MenuItemFileSelectDataDictionary.Click -= new System.EventHandler(this.m_MenuItemFileOpenDataDictionary_Click);
                this.m_MenuItemFileExit.Click -= new System.EventHandler(this.m_MenuItemFileExit_Click);
                this.m_MenuItemViewWatchWindow.Click -= new System.EventHandler(this.m_MenuItemWatchViewWatchWindow_Click);
                this.m_MenuItemViewSystemInformation.Click -= new System.EventHandler(this.m_MenuItemViewSystemInformation_Click);
                this.m_MenuItemDiagnosticsSelfTests.Click -= new System.EventHandler(this.m_MenuItemDiagnosticsSelfTests_Click);
                this.m_MenuItemDiagnosticsEventLog.Click -= new System.EventHandler(this.m_MenuItemDiagnosticsEventLog_Click);
                this.m_MenuItemDiagnosticsInitializeEventLogs.Click -= new System.EventHandler(this.m_MenuItemDiagnosticsInitializeEventLogs_Click);
                this.m_MenuItemConfigurePasswordProtection.Click -= new System.EventHandler(this.m_MenuItemConfigurePasswordProtection_Click);
                this.m_MenuItemConfigureWatchWindow.Click -= new System.EventHandler(this.m_MenuItemConfigureWatchWindow_Click);
                this.m_MenuItemConfigureDataStream.Click -= new System.EventHandler(this.m_MenuItemConfigureDataStream_Click);
                this.m_MenuItemConfigureChartRecorder.Click -= new System.EventHandler(this.m_MenuItemConfigureChartRecorder_Click);
                this.m_MenuItemRealTimeClock.Click -= new System.EventHandler(this.m_MenuItemToolsRealTimeClock_Click);
                this.m_MenuItemToolsConvertEngineeringFile.Click -= new System.EventHandler(this.m_MenuItemToolsConvertEngineeringDatabase);
                this.m_MenuItemToolsDebugMode.Click -= new System.EventHandler(this.m_MenuItemToolsDebugMode_Click);
                this.m_MenuItemToolsOptions.Click -= new System.EventHandler(this.m_MenuItemToolsOptions_Click);
                this.m_MenuItemHelpShowUserManual.Click -= new System.EventHandler(this.m_MenuItemHelpShowUserManual_Click);
                this.m_MenuItemHelpAboutPTU.Click -= new System.EventHandler(this.m_MenuItemHelpAboutPTU_Click);
                this.m_MenuItemLogin.Click -= new System.EventHandler(this.m_MenuItemLogin_Click);
                this.m_TSBOnline.Click -= new System.EventHandler(this.m_TSBOnline_Click);
                this.m_TSBOffline.Click -= new System.EventHandler(this.m_TSBOffline_Click);
                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.MdiPTU_FormClosing);
                this.Shown -= new System.EventHandler(this.MdiPTU_Shown);
                this.ResizeEnd -= new System.EventHandler(this.MdiPTU_ResizeEnd);
                this.FontChanged -= new System.EventHandler(this.MdiPTU_FontChanged);
                this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.MdiPTU_KeyDown);
                this.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.MdiPTU_KeyUp);
                #endregion - [Detach the event handler methods.] -

                #region - [Set Windows Form Designer Variables to Null] -
                m_MenuStrip = null;
                m_MenuItemFile = null;
                m_MenuItemFileOpen = null;
                m_MenuItemFileOpenWatchFile = null;
                m_MenuItemFileExit = null;
                m_MenuItemView = null;
                m_MenuItemTools = null;
                m_MenuItemHelp = null;
                m_MenuItemHelpAboutPTU = null;
                m_MenuItemLogin = null;
                m_MenuItemFileOpenEventLog = null;
                m_SeparatorFileEnd = null;
                m_SeparatorHelpEnd = null;
                m_MenuItemHelpShowUserManual = null;
                m_SeparatorHelpShowUserManual = null;
                m_MenuItemFileOpenFaultLog = null;
                m_SeparatorFileOpenEnd = null;
                m_MenuItemDiagnostics = null;
                m_MenuItemDiagnosticsSelfTests = null;
                m_MenuItemDiagnosticsEventLog = null;
                m_MenuItemDiagnosticsInitializeEventLogs = null;
                m_SeparatorDiagnosticsSelfTest = null;
                m_SeparatorDiagnosticsEventLog = null;
                m_SeparatorDiagnosticsEnd = null;
                m_MenuItemConfigure = null;
                m_MenuItemConfigurePasswordProtection = null;
                m_SeparatorConfigurePasswordProtection = null;
                m_MenuItemToolsOptions = null;
                m_SeparatorToolsEnd = null;
                m_MenuItemToolsConvertEngineeringFile = null;
                m_SeparatorToolsConvertEngineeringDatabase = null;
                m_MenuItemFileSelectDataDictionary = null;
                m_SeparatorFileOpenDataDictionary = null;
                m_PanelStatus = null;
                m_DigitalControlPacketReceived = null;
                m_MenuItemFileOpenSimulatedFaultLog = null;
                m_TSBOnline = null;
                m_SeparatorOnline = null;
                m_ToolStripFunctionKeys = null;
                m_MenuItemToolsDebugMode = null;
                m_SeparatorToolsDebug = null;
                m_MenuItemViewWatchWindow = null;
                m_SeparatorViewWatchWindow = null;
                m_MenuItemViewSystemInformation = null;
                m_SeparatorViewEnd = null;
                m_MenuItemConfigureChartRecorder = null;
                m_PictureBoxBusy = null;
                m_TSBOffline = null;
                m_SeparatorOffline = null;
                m_SeparatorOnlineLHS = null;
                m_SeparatorOfflineLHS = null;
                m_StatusLabelMode = null;
                m_StatusLabelCarNumber = null;
                m_StatusLabelSecurityLevel = null;
                m_StatusLabelLogStatus = null;
                m_StatusLabelWibuBoxStatus = null;
                m_LegendStatusMessage = null;
                m_StatusLabelMessage = null;
                m_StatusStrip = null;
                m_MenuItemRealTimeClock = null;
                m_SeparatorRealTimeClock = null;
                m_MenuItemConfigureDataStream = null;
                m_MenuItemConfigureWatchWindow = null;
                m_SeparatorConfigureEnd = null;
                #endregion - [Set Windows Form Designer Variables to Null] -
                #endregion - Windows Form Designer generated code] -
            }
            catch (Exception)
            {
                // Don't do anything, just ensure that an exception is not thrown.
            }
            this.Cursor = Cursors.Default;
        }
        #endregion --- Cleanup ---

        #region --- Delegated Methods ---
        #region - [Form] -
        /// <summary>
        /// Event handler for the form 'Shown' event. Loads the PTU configuration file and initializes the communication interface.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MdiPTU_Shown(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Ensure that the FilenameDataDictionary property has been updated with a valid project data dictionary i.e. check that it no longer specifies the default
            // data dictionary.
            Debug.Assert(m_FilenameDataDictionary != Resources.FilenameDefaultDataDictionary,
                         "MdiPTU.MdiPTU_Shown() - [m_FilenameDataDictionary != Resources.FilenameDefaultDataDictionary]");

            this.Update();
            Cursor = Cursors.WaitCursor;

            // ----------------------------------
            // Load the XML data dictionary file.
            // ----------------------------------
            m_DataDictionary = new DataDictionary();
            
            // Read the XML configuration file.
            try
            {
                // If the XML file hasn't been updated to include the YearCodeSize field of the CONFIGUREPTU table, the other fields of the table are still
                // read in correctly. If an attempt is made to access 'm_DataDictionary.CONFIGUREPTU[0].YearCodeSize' an exception is thrown.
                m_DataDictionary.ReadXml(DirectoryManager.PathPTUConfigurationFiles + CommonConstants.BindingFilename + m_FilenameDataDictionary);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format(Resources.MBTConfigDataDictionaryInvalid, m_FilenameDataDictionary), 
                                              Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisplayQueryExit = false;
                Close();
                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            Cursor = Cursors.WaitCursor;
            LoadDictionary(m_DataDictionary);
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the form 'ResizeEnd' event. Only update the FormLocation and FormSize settings if the window is in the normal state.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MdiPTU_ResizeEnd(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.FormSize = this.Size;
                Settings.Default.FormLocation = this.Location;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Event handler for the FormClosing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MdiPTU_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DisplayQueryExit == true)
            {
                DialogResult dialogResult = MessageBox.Show(Resources.MBTQueryExit, Resources.MBCaptionInformation, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                // Ask the user to confirm that they wish to terminate the Portable Test Application.
                if (dialogResult.Equals(System.Windows.Forms.DialogResult.Cancel))
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion  - [Form] -

        #region - Key Events -
        /// <summary>
        /// Event handler for the <c>KeyDown</c> event. Maps the Function keys to the <c>ToolStrip</c> buttons.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MdiPTU_KeyDown(object sender, KeyEventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    break;
                case Keys.F1:
                    break;
                case Keys.F2:
                    m_TSBOnline_Click(sender, e);
                    break;
                case Keys.F3:
                    m_TSBOffline_Click(sender, e);
                    break;
                default:
                    break;
            }
            // Keep a record of the KeyEventArgs object.
            m_KeyEventArgs = e;
        }

        /// <summary>
        /// Event handler for the <c>KeyUp</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MdiPTU_KeyUp(object sender, KeyEventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Update the KeyEventArgs object.
            m_KeyEventArgs = null;
        }
        #endregion - Key Events -

        #region - [Menu Options] -
        #region - [FILE] -
        /// <summary>
        /// Event handler for the 'File/Open - Data File/Recorded Watch File' menu option. Dispatch to the appropriate menu interface method.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileOpenWatchFile_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.OpenRecordedWatchFile();
        }

        /// <summary>
        /// Event handler for the 'File/Open - Data File/Simulated Fault Log' menu option. Dispatch to the appropriate menu interface method.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileOpenSimulatedFaultLog_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.OpenSimulatedFaultLog();
        }

        /// <summary>
        /// Event handler for the 'File/Open - Data File/Fault Log' menu option <c>Click</c> event. Dispatch to the appropriate menu interface method.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileOpenFaultLog_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceEvent.OpenFaultLog();
        }

        /// <summary>
        /// Event handler for the 'File/Open - Data File/Event Log' menu option <c>Click</c> event. Dispatch to the appropriate menu interface method.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileOpenEventLog_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceEvent.OpenEventLog();
        }

        /// <summary>
        /// Event handler for the 'File/Open - Data File/Screen Capture' menu option <c>Click</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileOpenScreenCapture_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.MBTNotYetImplemented, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event handler for the 'File/Select Data Dictionary' menu option. Allows the user to select a new data dictionary.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileOpenDataDictionary_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.OpenDataDictionary();
        }

        /// <summary>
        /// Event handler for the 'File/Save/Fault Logs' menu option <c>Click</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileSaveFaultLogs_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.MBTNotYetImplemented, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event handler for the 'File/Save/Event Log' menu option <c>Click</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileSaveEventLog_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.MBTNotYetImplemented, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event handler for the 'File/Save All' menu option <c>Click</c> event.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileSaveAll_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.MBTNotYetImplemented, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event handler for the 'File/Exit' menu option. Closes the PTU application.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemFileExit_Click(object sender, EventArgs e)
        {
            DisplayQueryExit = true;
            Close();
        }
        #endregion - [FILE] -

        #region - [VIEW] -
        /// <summary>
        /// Event handler for the 'View/Watch Window' menu option. Show the child form which displays the watch variables defined by the current workset.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemWatchViewWatchWindow_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.ViewWatchWindow();
        }

        /// <summary>
        /// Event handler for the 'View/System Information' menu option. Shows the dialog box which displays the system information retrieved from the VCU.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemViewSystemInformation_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.ShowSystemInformation();
        }
        #endregion - [VIEW] -

        #region - [DIAGNOSTICS] -
        /// <summary>
        /// Event handler for the 'Diagnostics/Self-Test' menu option. Allows the user to run the self tests.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemDiagnosticsSelfTests_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceSelfTest.ConfigureSelfTests();
        }

        /// <summary>
        /// Event handler for the 'Diagnostics/Event Log' menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemDiagnosticsEventLog_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceEvent.ViewEventLog();
        }

        /// <summary>
        /// Event handler for the 'Diagnostics/Annunciators' menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemDiagnosticsAnnunciators_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.MBTNotYetImplemented, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event handler for the 'Diagnostics/Macros' menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemDiagnosticsMacros_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.MBTNotYetImplemented, Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Event handler for the 'Diagnostics/Initialize Event Logger' menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemDiagnosticsInitializeEventLogs_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceEvent.InitializeEventLogs();
        }

        #endregion - [DIAGNOSTICS] -

        #region - [CONFIGURE] -
        #region - [Worksets] -
        /// <summary>
        /// Event handler for the 'Configure/Worksets/Watch Window' menu option. Instantiate and show the FormWorksetManager dialog box. This allows the user to
        /// manage the worksets associated with viewing and recording watch variables.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigureWorksetsWatchWindow_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.ConfigureWorksetsWatchWindow();
        }

        /// <summary>
        /// Event handler for the 'Configure/Worksets/Fault Log' menu option. Instantiate and show the FormWorksetManagerFaultLog dialog box. This allows the user to
        /// manage the worksets associated with the fault log data stream.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigureWorksetsFaultLog_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceEvent.ConfigureWorksetsFaultLog();
        }

        /// <summary>
        /// Event handler for the 'Configure/Worksets/Chart Recorder' menu option. Instantiate and show the FormWorksetManagerChartRecorder dialog box. This allows 
        /// the user to manage the worksets associated with the chart recorder.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigureWorksetsChartRecorder_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.ConfigureWorksetsChartRecorder();
        }
        #endregion - [Worksets] -

        /// <summary>
        /// Event handler for the 'Configure/Password Protection' menu option. Show the form which allows user to manage password protection.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigurePasswordProtection_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.ConfigurePasswordProtection();
        }

        /// <summary>
        /// Event handler for the 'Configure/Chart Recorder' menu option. Show the form which allows user to configure the chart recorder.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigureChartRecorder_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.ConfigureChartRecorder();
        }

        /// <summary>
        /// Event handler for the 'Configure/Data Stream' menu option. Show the form which allows user to configure the data stream.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigureDataStream_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceEvent.ConfigureDataStream();
        }

        /// <summary>
        /// Event handler for the 'Configure/Watch Window' menu option. Show the form which allows user to configure the Watch Window.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemConfigureWatchWindow_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceWatch.ConfigureWatchWindow();
        }
        #endregion - [CONFIGURE] -

        #region - [TOOLS] -
        /// <summary>
        /// Event handler for the 'Tools/Real Time Clock' menu option. Call the menu interface method that is responsible for showing the dialog box which 
        /// allows the user to configure the VCU real time clock.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemToolsRealTimeClock_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.RealTimeClock();
        }

        /// <summary>
        /// Event handler for the 'Tools/Data Dictionary/Convert to XML Format' menu option. Allows the user to create an XML based data dictionary using an existing
        /// Access database data dictionary derived from the Data Dictionary Builder utility.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemToolsConvertEngineeringDatabase(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.ConvertEngineeringDatabase();
        }

        /// <summary>
        /// Event handler for the 'Tools/Debug Mode' menu option. Enable/disable debugging.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemToolsDebugMode_Click(object sender, EventArgs e)
        {
            m_DebugMode = !m_DebugMode;
            m_MenuItemToolsDebugMode.Checked = m_DebugMode;

            if (m_DebugMode)
            {
                DebugMode.Open();          
            }
            else
            {
                DebugMode.Close();
            }
        }

        /// <summary>
        /// Event handler for the 'Tools/Options' menu option. Displays the form which allows the user to set up the user specific parameters.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemToolsOptions_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.Options();
        }
        #endregion - [TOOLS] -

        #region - [HELP] -
        /// <summary>
        /// Event handler for the 'Help/PTU Help' menu option. Show the PTU user manual.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemHelpShowUserManual_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.ShowUserManual(this);
        }

        /// <summary>
        /// Event handler for the 'Help/About PTU' menu option. Display the About form.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemHelpAboutPTU_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.HelpAbout();
        }
        #endregion - [HELP] -

        #region - [LOGIN] -
        /// <summary>
        /// Event handler for the 'Login/Logout' menu option.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_MenuItemLogin_Click(object sender, EventArgs e)
        {
            m_MenuInterfaceApplication.Login(m_ProjectIdentifierPassedAsParameter);
        }
        #endregion - [LOGIN] -
        #endregion - [Menu Options] -

        #region - [ToolStrip Buttons] -
        /// <summary>
        /// Event handler for the on-line button <c>Click</c> event. Initializes the communication port specified in the <c>Communication</c> project user settings 
        /// and, if successful: (a) updates the mode setting and then (b) displays the form to show the live watch variable data.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_TSBOnline_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Skip, if the button isn't enabled.
            if (m_TSBOnline.Enabled == false)
            {
                return;
            }

            // If online mode is already selected, toggle to configuration mode.
            if (m_TSBOnline.Checked)
            {
                #region - [Return to Configuration Mode] -
                // -------------------------------------------------------
                // The PTU is already online, go to configuration mode.
                // -------------------------------------------------------
				try
				{
					this.Cursor = Cursors.WaitCursor;
					CommunicationInterface.CloseCommunication(CommunicationInterface.CommunicationSetting.Protocol);
				}
				catch (CommunicationException ex)
				{
					// Check to see if it's a failure to close, which we can potentially ignore.
					if (ex.CommunicationError != CommunicationError.SystemException)
					{
						// This is a recoverable error, so we allow the port to stay open, but display the error.
						MessageBox.Show(Resources.MBTPortCloseFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						MessageBox.Show(Resources.MBTPortCloseFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				catch (Exception)
				{
					MessageBox.Show(Resources.MBTPortCloseFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
                finally
                {
                    this.Cursor = Cursors.Default;
                    CommunicationInterface = null;
                }

                SetMode(Mode.Configuration);

                // Update the LogStatus StatusStrip.
                LogStatus = EventLogSavedStatus.NotApplicable;
                #endregion -[Return to Configuration Mode] -
                return;
            }

            // ---------------------------------------------------------
            // Show the form to allow the user to select a valid target.
            // ---------------------------------------------------------
            this.Cursor = Cursors.WaitCursor;
            FormSelectTarget formSelectTarget = new FormSelectTarget();
            ShowDialog(formSelectTarget);
            this.Cursor = Cursors.WaitCursor;

            // Skip, if no target logic was selected.
            if (formSelectTarget.TargetSelected != true)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            // -------------------------------------------------------------------------------------------------
            // A valid target was selected, check that the PTU configuration and the target configuration match.
            // -------------------------------------------------------------------------------------------------
            bool configurationMatch = CheckConfiguration(formSelectTarget.TargetConfiguration.ProjectIdentifier, formSelectTarget.TargetConfiguration.Version);
            if (configurationMatch == true)
            {
                #region - [Go Online] -
                // -------------------------------------------------------------------
                // PTU configuration and target configuration match, enter online mode.
                // -------------------------------------------------------------------
                CommunicationInterface = new CommunicationParent(formSelectTarget.CommunicationSetting);
                Debug.Assert(CommunicationInterface != null);

                try
                {
                    // Initialize the serial communications port associated with the selected target.
                    CommunicationInterface.InitCommunication(CommunicationInterface.CommunicationSetting);
                }
                catch (InvalidOperationException)
                {
                    // An error occurred trying to initialize the communication port, do not enter online mode.
                    CommunicationInterface = null;
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(Resources.MBTPortInitializationFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the header information with the target configuration.
                Header_t header = new Header_t();
                header = FileHeader.HeaderCurrent;
                header.TargetConfiguration = formSelectTarget.TargetConfiguration;
                FileHeader.HeaderCurrent = header;

                SetMode(Mode.Online);

                // Check whether the most recently downloaded event log was saved to disk and update the LogStatus StatusStrip.
                LogStatus = EventLogSavedStatus.Unknown;

                // Display the Watch Window only if the project doesn't use a Control Panel.
                if (this.Controls[CommonConstants.KeyControlPanel] == null)
                {
                    m_MenuInterfaceWatch.ViewWatchWindow();
                }
                #endregion - [Go Online] -
            }
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the off-line button <c>Click</c> event. Enter offline mode.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void m_TSBOffline_Click(object sender, EventArgs e)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            // Skip, if the button isn't enabled.
            if (m_TSBOffline.Enabled == false)
            {
                return;
            }

            // If offline mode is already selected, toggle to configuration mode.
            if (m_TSBOffline.Checked)
            {
                #region - [Return to Configuration Mode] -
                // -------------------------------------------------------
                // The PTU is already offline, go to configuration mode.
                // -------------------------------------------------------
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    CommunicationInterface.CloseCommunication(CommunicationInterface.CommunicationSetting.Protocol);
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.MBTPortCloseFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    CommunicationInterface = null;
                }

                SetMode(Mode.Configuration);

                // Update the LogStatus StatusStrip.
                LogStatus = EventLogSavedStatus.NotApplicable;
                #endregion - [Return to Configuration Mode] -
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            #region - [Go Offline] -
            // ---------------------------------------------------------------------------------------------
            // Enter offline mode. Instantiate a the communication interface which returns simulated values.
            // ---------------------------------------------------------------------------------------------
            CommunicationSetting_t communicationSetting = new CommunicationSetting_t();
            communicationSetting.Port = new Port_t();
            communicationSetting.PortIdentifier = string.Empty;
            communicationSetting.Protocol = Protocol.SIMULATOR;
            CommunicationInterface = new CommunicationParentOffline(communicationSetting);
            TargetConfiguration_t targetConfiguration;
            CommunicationInterface.GetEmbeddedInformation(out targetConfiguration);

            // Update the header information with the target configuration.
            Header_t header = new Header_t();
            header = FileHeader.HeaderCurrent;
            header.TargetConfiguration = targetConfiguration;
            FileHeader.HeaderCurrent = header;

            SetMode(Mode.Offline);

            // Check whether the most recently downloaded event log was saved to disk and update the LogStatus StatusStrip.
            LogStatus = EventLogSavedStatus.Unknown;

            // Display the Watch Window only if the project doesn't use a Control Panel.
            if (this.Controls[CommonConstants.KeyControlPanel] == null)
            {
                m_MenuInterfaceWatch.ViewWatchWindow();
            }
            #endregion - [Go Offline] -
            this.Cursor = Cursors.Default;
        }
        #endregion - [ToolStrip Buttons] -

        /// <summary>
        /// Event handler for the <c>FontChanged</c> event. Update the font associated with the controls on this form.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void MdiPTU_FontChanged(object sender, EventArgs e)
        {
            // Change the font associated with the various controls contained within the form.
            m_ToolStripFunctionKeys.Font = Font;
            m_StatusStrip.Font = Font;
            m_MenuStrip.Font = Font;
        }

        /// <summary>
        /// Event handler for the WibuBox Timer Click event. Checks whether a valid WibuBox is connected to the USB or Parallel port.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="e">Parameter passed from the object that raised the event.</param>
        private void WibuBoxCheck(object sender, EventArgs e)
        {
            if (m_TimerWibuBox != null)
            {
                m_TimerWibuBox.Stop();
            }

            // Check whether the PTU has detected that a WibuBox is present.
            if (WibuBoxPresent == true)
            {
                // Check whether the WibuBox has been removed.
                if (m_MenuInterfaceWibuKey.WibuBoxCheckIfRemoved() == true)
                {
                    // The WibuBox has been removed, update the WibuKey status label.
                    m_StatusLabelWibuBoxStatus.Text = Resources.LabelWibuBoxStatusNotPresent;

                    // Only issue the warning that the WibuBox has been removed if the user is logged into the Engineering account, or above.
                    if (m_Security.SecurityLevelCurrent > m_Security.SecurityLevelBase)
                    {
                        MessageBox.Show(Resources.MBTWibuBoxRemoved + CommonConstants.NewPara + Resources.MBTWibuBoxReInsert,
                                        Resources.MBCaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Set the current security level to the lowest security access level.
                    m_Security.SetLevel(m_Security.SecurityLevelBase);
                    ShowSecurityLevelChange(m_Security);
                    
                    WibuBoxPresent = false;
                }
            }
            else
            {
                // Check whether a WibuBox has been inserted since the last check.
                WibuBoxPresent = m_MenuInterfaceWibuKey.WibuBoxCheckForValidEntry(true);
            }

            if (m_TimerWibuBox != null)
            {
                m_TimerWibuBox.Start();
            }
        }
        #endregion  --- Delegated Methods ---

        #region --- Methods ---
        /// <summary>
        /// If the XML configuration file (data dictionary) associated with the specified project identifier exists, update the <c>FilenameDataDictionary</c>
        /// property with the name of said XML configuration file. Also update the ProjectIdentifierPassedAsParameter property with string value of the specified
        /// project identifier regardless of whether the corresponding XML configuration file exists or not.
        /// </summary>
        /// <param name="projectIdentifierPassedAsParameter">The project identifier string that was passed as a command line parameter.</param>
        /// <returns>A flag that specifies whether the project XML configuration file (data dictionary) exists or not. True, if the file exists; otherwise,
        /// false.</returns>
        private bool UpdateFilenameDateDictionaryProperty(string projectIdentifierPassedAsParameter)
        {
            // A flag to specify whether the data dictionary for the specified project exists or not. True, if the file exists; otherwise, false. 
            bool projectDataDictionaryExists = false;

            m_ProjectIdentifierPassedAsParameter = projectIdentifierPassedAsParameter;

            // Check whether a XML configuration file for the specified project identifier exists. This takes the format '<project-identifier>.Configuration.xml'.
            string fullFilename = DirectoryManager.PathPTUConfigurationFiles + CommonConstants.BindingDirectory + m_ProjectIdentifierPassedAsParameter +
                                  CommonConstants.Period + Resources.FilenameDefaultDataDictionary;
            FileInfo fileInfo = new FileInfo(fullFilename);
            if (fileInfo.Exists)
            {
                // The file exists, update the project data dictionary filename property.
                m_FilenameDataDictionary = m_ProjectIdentifierPassedAsParameter + CommonConstants.Period + Resources.FilenameDefaultDataDictionary;
                projectDataDictionaryExists = true;
                InitializePTUProjectSpecific(m_ProjectIdentifierPassedAsParameter);

            }
            else
            {
                projectDataDictionaryExists = false;
            }

            return projectDataDictionaryExists;
        }

        /// <summary>
        /// Check whether the PTU configuration and the propulsion system software match. This method checks whether the PTU configuration matches that of the
        /// propulsion system software and attempts to load the correct configuration file if they do not match.
        /// </summary>
        /// <param name="targetProjectIdentifier">The project identifier of the propulsion system software.</param>
        /// <param name="targetVersion">The version reference of the propulsion system software.</param>
        /// <returns>A flag that indicates whether the PTU configuration matches that of the propulsion system software. True, if they are matched; otherwise, false.
        /// </returns>
        private bool CheckConfiguration(string targetProjectIdentifier, string targetVersion)
        {
            // A flag that indicates whether the PTU configuration matches that of the propulsion system software. True, if they are matched; otherwise, false.
            bool configurationMatch = false;

            // The DialogResult returned from the call to the MessageBox() method.
            DialogResult dialogResult;

#if ByPassVersionCheck
            while (Parameter.ProjectInformation.ProjectIdentifier != targetProjectIdentifier)
#else
            // Repeat until the PTU configuration matches that of the propulsion system software.
            while ((Parameter.ProjectInformation.ProjectIdentifier != targetProjectIdentifier) ||
                   (Parameter.ProjectInformation.Version != targetVersion))
#endif
            {
                #region - [Configuration Mismatch] -
                this.Cursor = Cursors.WaitCursor;
                // ---------------------------------------------------------------------------------------------------------------------------------------------
                // There is a mismatch between the PTU configuration and the propulsion system software. Determine whether the mismatch is a result of a version
                // number mismatch or because of a project mismatch.
                // ---------------------------------------------------------------------------------------------------------------------------------------------

                // Check whether the project identifiers match.
                if (Parameter.ProjectInformation.ProjectIdentifier == targetProjectIdentifier)
                {
                    // The project identifiers match, therefore the mismatch is between the version numbers. Ask whether an attempt is to be made to load the
                    // data dictionary associated with the target version number.
                    String message = Resources.MBTConfigVCUMismatchVersion1 + " (" + Parameter.ProjectInformation.Version + ") " +
                                     Resources.MBTConfigVCUMismatchVersion2 + " (" + targetVersion + "). " +
                                     Resources.MBTConfigVCUMismatchVersion3;
                    dialogResult = MessageBox.Show(message, Resources.MBCaptionWarning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }
                else
                {
                    // The PTU project identifier does not match that of the propulsion system software. Ask whether an attempt is to be made to load the data dictionary
                    // associated with the project-identifier and version reference of the embedded software loaded into the VCU.
                    dialogResult = MessageBox.Show(Resources.MBTConfigVCUMismatchProjectId, Resources.MBCaptionWarning, MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Warning);
                }

                // Skip, if the user does not wish to load the appropriate data dictionary.
                if (dialogResult == DialogResult.No)
                {
                    this.Cursor = Cursors.Default;
                    configurationMatch = false;
                    return (configurationMatch);
                }

                // ------------------------------------------------------------------------------------------------------
                // The user has selected to update the PTU configuration to match that of the propulsion system software.
                // ------------------------------------------------------------------------------------------------------

                // Check whether the correct PTU configuration file associated with the propulsion system software exists in the default 'PTU Configuration Files'
                // sub-directory.
                string fullyQualifiedSourceFilename = DirectoryManager.PathPTUConfigurationFiles + CommonConstants.BindingFilename +
                                                      targetVersion + CommonConstants.ExtensionDataDictionary;
                FileInfo fileInfoSource = new FileInfo(fullyQualifiedSourceFilename);
                m_DataDictionary = new DataDictionary();

                if (fileInfoSource.Exists == false)
                {
                    #region - [Locate PTU Configuration File] -
                    switch (Parameter.ProjectInformation.ProjectIdentifier)
                    {
                        case CommonConstants.ProjectIdCTA:
                            #region - [Download From VCU] -
                            // Modified for the CTA contract. Ref.: P.O. 4800011369-CU2 07.07.2015. Attempt to download the PTU configuration files
                            // from the propulsion system software.

                            // Check that the Windows command file exists.
                            string fullyQualifiedCommandFilename = DirectoryManager.PathPTUApplicationData + CommonConstants.BindingFilename +
                                                                   Resources.FilenameWindowsCommandFile;
                            FileInfo fileInfoCommandFile = new FileInfo(fullyQualifiedCommandFilename);
                            if (fileInfoCommandFile.Exists != true)
                            {
                                MessageBox.Show(string.Format(Resources.MBTConfigVCUDownloadCommandFileNotFound, Resources.FilenameWindowsCommandFile),
                                                              Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Cursor = Cursors.Default;
                                configurationMatch = false;
                                return (configurationMatch);
                            }

                            // The error code that is returned from the batch file.
                            FTPErrorCode ftpErrorCode = FTPErrorCode.Undefined;
                            int exitCode = 0;
                            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                            string newInstruction = CommonConstants.NewLine + CommonConstants.NewLine;
                            stringBuilder.AppendFormat(Resources.MBTConfigVCUDownloadStart, newInstruction, newInstruction, newInstruction, newInstruction);
                            dialogResult = MessageBox.Show(stringBuilder.ToString(), Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            

                            try
                            {
                                Process commandProcess = new Process();
                                commandProcess.StartInfo.WorkingDirectory = DirectoryManager.PathPTUApplicationData;
                                commandProcess.StartInfo.FileName = Resources.FilenameWindowsCommandFile;
                                commandProcess.StartInfo.Arguments = (Parameter.ProjectInformation.ProjectIdentifier.Equals(CommonConstants.ProjectIdCTA)) ?
                                                                      Resources.CommandLineArgumentCTA : Resources.CommandLineArgumentBART;
                                commandProcess.Start();
                                WriteStatusMessage(Resources.MBTConfigVCUDownloadInProgress, System.Drawing.Color.White, System.Drawing.Color.Red);
                                commandProcess.WaitForExit();
                                exitCode = commandProcess.ExitCode;
                                ftpErrorCode = (FTPErrorCode)exitCode;
                            }
                            catch (Exception)
                            {
                                ftpErrorCode = FTPErrorCode.SystemException;
                            }

                            // Check the error code that was returned from the batch file.
                            if (ftpErrorCode == FTPErrorCode.Success)
                            {
                                // Check that the correct PTU configuration and help files now exists.
                                string fullyQualifiedRequiredFilenameXML = DirectoryManager.PathPTUConfigurationFiles + CommonConstants.BindingFilename +
                                                      targetVersion + CommonConstants.ExtensionDataDictionary;
                                FileInfo fileInfoRequiredFilenameXML = new FileInfo(fullyQualifiedRequiredFilenameXML);

                                string fullyQualifiedRequiredFilenameHLP = DirectoryManager.PathDiagnosticHelpFiles + CommonConstants.BindingFilename +
                                                      targetVersion + CommonConstants.ExtensionHelpFile;
                                FileInfo fileInfoRequiredFilenameHLP = new FileInfo(fullyQualifiedRequiredFilenameHLP);
                                if ((fileInfoRequiredFilenameXML.Exists == true) &&
                                    (fileInfoRequiredFilenameHLP.Exists == true))
                                {
                                    WriteStatusMessage(string.Empty);
                                    MessageBox.Show(Resources.MBTConfigVCUDownloadComplete, Resources.MBCaptionInformation, MessageBoxButtons.OK,
                                               MessageBoxIcon.Information);
                                }
                                else
                                {
                                    WriteStatusMessage(string.Empty);
                                    MessageBox.Show(Resources.MBTConfigVCUDownloadUnsuccessful, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Cursor = Cursors.Default;
                                    configurationMatch = false;
                                    return (configurationMatch);
                                }
                            }
                            else
                            {
                                WriteStatusMessage(string.Empty);
                                MessageBox.Show(string.Format(Resources.MBTConfigVCUDownloadUnsuccessfulWithErrorCode,
                                                exitCode, FtpErrorProcessing.GetErrorMessage(ftpErrorCode)),
                                                Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Cursor = Cursors.Default;
                                configurationMatch = false;
                                return (configurationMatch);
                            }
                            #endregion - [Download From VCU] -
                            break;
                        default:
                            #region - [Manual File Selection] -
                            // The PTU configuration file associated with the propulsion system software cannot be found in the default 'PTU Configuration Files'
                            // sub-directory. Ask the user whether the file is to be located manually.
                            dialogResult = MessageBox.Show(Resources.MBTConfigVCUMatchNotFoundManualSelection, Resources.MBCaptionWarning, MessageBoxButtons.YesNo,
                                                           MessageBoxIcon.Warning);

                            if (dialogResult == DialogResult.Yes)
                            {
                                // -----------------------------------------------------------------
                                // Allow the user to select an alternative data dictionary XML file.
                                // -----------------------------------------------------------------
                                fullyQualifiedSourceFilename = General.FileDialogOpenFile(Resources.FileDialogOpenTitleDataDictionary,
                                                                                          CommonConstants.ExtensionDataDictionary,
                                                                                          Resources.FileDialogOpenFilterDataDictionary,
                                                                                          DirectoryManager.PathPTUConfigurationFiles);

                                // Skip, if no alternative data dictionary XML file is selected.
                                if (fullyQualifiedSourceFilename == string.Empty)
                                {
                                    this.Cursor = Cursors.Default;
                                    configurationMatch = false;
                                    return (configurationMatch);
                                }
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                                configurationMatch = false;
                                return (configurationMatch);
                            }
                            #endregion - [Manual File Selection] -
                            break;
                    }
                    #endregion - [Locate PTU Configuration File]] -
                }

                #region - [Load the PTU Configuration File] -
                // ------------------------------------------------------------------------------------------
                // The PTU configuration file corresponding to the propulsion system software exists, load it.
                // ------------------------------------------------------------------------------------------
                try
                {
                    FileHandling.LoadDataSet<DataDictionary>(fullyQualifiedSourceFilename, ref m_DataDictionary);

                    // Copy the selected PTU configuration file to the default directory and rename it to the default filename.
                    string fullyQualifiedDestinationFilename = DirectoryManager.PathPTUConfigurationFiles + CommonConstants.BindingFilename + FilenameDataDictionary;
                    FileInfo fileInfoDestination = new FileInfo(fullyQualifiedDestinationFilename);
                    fileInfoSource = new FileInfo(fullyQualifiedSourceFilename);
                    fileInfoSource.CopyTo(fullyQualifiedDestinationFilename, true);
                    LoadDictionary(m_DataDictionary);

                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show(Resources.MBTConfigReadFailed, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    configurationMatch = false;
                    return (configurationMatch);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show(Resources.MBTConfigVCUMatchNotFoundTryAgain, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    configurationMatch = false;
                    return (configurationMatch);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
                #endregion - [Load the PTU Configuration File] -
                #endregion - [Configuration Mismatch] -
            }

            configurationMatch = true;
            return (configurationMatch);
        }
        #endregion --- Methods ---

        #region --- Properties ---
        /// <summary>
        /// Gets the state of the static flag that controls whether the PTU does an automatic restart when the main PTU application is closed. True, if the PTU is to do
        /// an automatic restart; otherwise, false.
        /// </summary>
        public static bool Restart
        {
            get { return m_Restart; }
        }

        /// <summary>
        /// Gets or sets the flag which indicates whether the Dispose() method has been called. True indicates that the Dispose() method has been called; otherwise,
        /// false.
        /// </summary>
        protected new bool IsDisposed
        {
            get
            {
                lock (this)
                {
                    return m_IsDisposed;
                }
            }

            set
            {
                lock (this)
                {
                    m_IsDisposed = value;
                }
            }
        }

        /// <summary>
        /// Get the security level that was passed to the application as a command line parameter. If no command line parameter is passed to the
        /// application then the value is set to <c>SecurityLevel.Undefined</c>.
        /// </summary>
        public SecurityLevel SecurityLevelPassedAsParameter
        {
            get { return m_SecurityLevelPassedAsParameter; }
        }

        #region - [IMainWindow] -
        /// <summary>
        /// Gets the reference to the main menu strip.
        /// </summary>
        public MenuStrip MenuStrip
        {
            get { return m_MenuStrip; }
        }

        /// <summary>
        /// Gets the reference to the main status strip.
        /// </summary>
        public StatusStrip StatusStrip
        {
            get { return m_StatusStrip; }
        }

        /// <summary>
        /// Gets or sets the reference to the <c>ToolStrip</c> control containing the function key buttons.
        /// </summary>
        public ToolStrip ToolStripFunctionKeys
        {
            get { return m_ToolStripFunctionKeys; }
            set { m_ToolStripFunctionKeys = value; }
        }

        /// <summary>
        /// Gets the mode of operation of the PTU application: setup, online, diagnostic or offline.
        /// </summary>
        public Mode Mode
        {
            get { return m_Mode; }
        }

        /// <summary>
        /// Gets or sets the communication interface that is to be used to communicate with the target.
        /// </summary>
        public ICommunicationParent CommunicationInterface
        {
            get { return m_CommunicationInterface; }
            set { m_CommunicationInterface = value; }
        }

        /// <summary>
        /// Gets the collection of function keys associated with the form. This allows any child form that is called indirectly to restore the function keys 
        /// on exit.
        /// </summary>
        public ToolStripItemCollection ToolStripItemCollectionMainWindow
        {
            get { return m_ToolStripItemCollectionMainWindow; }
        }

        /// <summary>
        /// Gets or sets the flag that controls whether the animation showing that the PTU is busy processing data is visible or not. True, to show the animation; 
        /// otherwise, false.
        /// </summary>
        public bool ShowBusyAnimation
        {
            get { return m_ShowBusyAnimation; }
            set 
            {
                m_ShowBusyAnimation = value;
                m_PictureBoxBusy.Visible = m_ShowBusyAnimation;
            }
        }

        /// <summary>
        /// Gets the filename of the XML data dictionary file.
        /// </summary>
        public string FilenameDataDictionary
        {
            get { return m_FilenameDataDictionary; }
        }

        /// <summary>
        /// Gets or sets the event log saved status associated with the current car. This property also updates the LogStatus StatusLabel whenever the property is
        /// written to.
        /// </summary>
        public EventLogSavedStatus LogStatus
        {
            get { return m_EventLogSavedStatus; }
            set
            {
                m_EventLogSavedStatus = value;
                switch (m_EventLogSavedStatus)
                {
                    case EventLogSavedStatus.Unsaved:
                    case EventLogSavedStatus.Saved:
                    case EventLogSavedStatus.Unknown:
                    case EventLogSavedStatus.Undefined:
                         m_StatusLabelLogStatus.Text = Resources.LegendLogStatus + CommonConstants.Space + m_EventLogSavedStatus.ToString();
                        break;
                    case EventLogSavedStatus.NotApplicable:
                    default:
                        m_StatusLabelLogStatus.Text = Resources.LegendLogStatus + CommonConstants.Space;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Flag that indicates whether a WibuBox security device is present or not. True, if a WibuBox security device is present; otherwise, false.
        /// </summary>
        public bool WibuBoxPresent
        {
            get { return m_WibuBoxPresent; }
            set
            {
                m_WibuBoxPresent = value;
                m_StatusLabelWibuBoxStatus.Text = (m_WibuBoxPresent == true) ? Resources.LabelWibuBoxStatusPresent : Resources.LabelWibuBoxStatusNotPresent;
            }
        }

        /// <summary>
        /// Gets the current car number, if the PTU is connected to the target logic. If not connected to the car logic, the value that will be returned is
        /// short.MinVal.
        /// </summary>
        public UInt16 CarNumber
        {
            get { return m_CarNumber; }
            set { m_CarNumber = value; }
        }

        /// <summary>
        /// Get the project identifier that was passed to the application as a shortcut parameter. If no command line parameter is passed to the
        /// application then the value is set to string.Empty.
        /// </summary>
        public string ProjectIdentifierPassedAsParameter
        {
            get { return m_ProjectIdentifierPassedAsParameter; }
        }

        /// <summary>
        /// Get or set the flag that controls whether the user is asked to confirm that they wish to close the application. True, to ask the user to confirm that they
        /// wish to close the application; otherwise, false, to close regardless.
        /// </summary>
        public bool DisplayQueryExit
        {
            get { return m_DisplayQueryExit; }
            set { m_DisplayQueryExit = value; }
        }
        #endregion - [IMainWindow] -
        #endregion --- Properties ---
    }
}

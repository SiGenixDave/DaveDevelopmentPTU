#region --- Revision History ---
/*
 * 
 *  This assembly is the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information. The reproduction,
 *  distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.
 *  Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    VcuComm
 * 
 *  File name:  AssemblyInfo.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 */
#region - [1.0] -
/*
 *  01/20/2016    1.0     DAS           References
 *                                      1.  Purchase order to replace the unmanaged communication project/dll - VcuCommunication, with the managed communication
 *                                          project/dll - VcuComm1 so that the PTU application is x32/x64 independent.
 *                                          
 *                                      Modifications
 *                                      1.  First entry into TortoiseSVN. This managed communication project replaces the unmanaged VcuCommunication
 *                                          project that included the methods and interfaces to allow the PTU user interface to send data to and receive
 *                                          data from the VCU embedded software via: the PC hardware RS232 serial port; or the the Ethernet port.      
 */
#endregion - [1.0] -

#region - [1.1] -
/*  03/21/2016      1.1     DAS     References
 *                                  1.  Bug fix - SNCR-PTU [01 Mar 2016] - Item 11. There appears to be a problem with the SetTimeDate() method in the
 *                                      CommunicationApplication class.
 *                                      
 *                                  2.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 10. While trying out version 6.16.3 of the PTU, there was a problem if the user tries
 *                                      to download an, albeit invalid, data stream. Polling for new events was initially OK, however once the download was complete,
 *                                      the PTU reported ‘Unable to retrieve the data stream associated with the MVB Communication Failure event’ and polling for new
 *                                      events was never resumed. After a 10 second period, Windows reported that the ‘Portable Test Application has stopped working’
 *                                      without ever throwing an exception.
 *                                      
 *                                  3.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 14. This is linked to SNCR-PTU [01 Mar 2016] - Item 10. While trying to save a CTA 
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
 *                                  Modifications
 *                                  1.  Modified ProtocolPTURequests.cs. Rev. 1.1. - Ref.: 1.
 *                                  2.  Modified Serial.cs. Rev. 1.1. - Ref.: 2, 3. 
 */
#endregion - [1.1] -
#endregion --- Revision History ---

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("Bombardier Transportation (Holdings) USA Inc.")]
[assembly: AssemblyProduct("Portable Test Application")]
[assembly: AssemblyCopyright("(C) 2010 - 2016 Bombardier Transportation (Holdings) USA Inc.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguageAttribute("")]

// General Information about an assembly is controlled through the following set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Portable Test Application - VCU Communication")]
[assembly: AssemblyDescription("A collection of managed classes that are used to communicate with the embedded target. " + 
                               "This project replaces the unmanaged communication dll - VcuCommunication.")]
[assembly: AssemblyConfiguration("")]

// Setting ComVisible to false makes the types in this assembly not visible to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("653ac898-2406-4d8a-ad48-8ef97d70d466")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers by using the '*'.

[assembly: AssemblyVersion("1.1.0.0")]


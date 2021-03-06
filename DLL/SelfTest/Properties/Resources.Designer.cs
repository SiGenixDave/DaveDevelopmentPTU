﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SelfTest.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SelfTest.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Abort {
            get {
                object obj = ResourceManager.GetObject("Abort", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Continue {
            get {
                object obj = ResourceManager.GetObject("Continue", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loss of Communications.
        /// </summary>
        internal static string EMCommunicationLost {
            get {
                return ResourceManager.GetString("EMCommunicationLost", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to execute the selected tests..
        /// </summary>
        internal static string EMExecuteSTTestListFailed {
            get {
                return ResourceManager.GetString("EMExecuteSTTestListFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target Hardware autonomously aborted self test.
        /// </summary>
        internal static string EMSelfTestAbortByTarget {
            get {
                return ResourceManager.GetString("EMSelfTestAbortByTarget", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot enter Self Test. An exception was thrown while trying to determine the reason why Self Test cound not be entered..
        /// </summary>
        internal static string EMSelfTestEnterExceptionThrown {
            get {
                return ResourceManager.GetString("EMSelfTestEnterExceptionThrown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot enter Self Test. The Vehicle Control Unit did not respond to the request to enter Self Test..
        /// </summary>
        internal static string EMSelfTestEnterNoResponse {
            get {
                return ResourceManager.GetString("EMSelfTestEnterNoResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot enter Self Test. The reason for this was not specified by the Vehicle Control Unit..
        /// </summary>
        internal static string EMSelfTestEnterReasonNotGiven {
            get {
                return ResourceManager.GetString("EMSelfTestEnterReasonNotGiven", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot enter Self Test. The Self Test Error Message returned from the Vehicle Control Unit was not recognised..
        /// </summary>
        internal static string EMSelfTestEnterReasonNotRecognised {
            get {
                return ResourceManager.GetString("EMSelfTestEnterReasonNotRecognised", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to update the loop counter on the Vehicle Control Unit..
        /// </summary>
        internal static string EMUpdateSTLoopCountFailed {
            get {
                return ResourceManager.GetString("EMUpdateSTLoopCountFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Vehicle Control Unit did not respond to the request to set the self test mode to engineering mode..
        /// </summary>
        internal static string EMUpdateSTModeFailed {
            get {
                return ResourceManager.GetString("EMUpdateSTModeFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to download the selected test list to the Vehicle Control Unit..
        /// </summary>
        internal static string EMUpdateSTTestListFailed {
            get {
                return ResourceManager.GetString("EMUpdateSTTestListFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Abort.
        /// </summary>
        internal static string FunctionKeyTextAbort {
            get {
                return ResourceManager.GetString("FunctionKeyTextAbort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Continue.
        /// </summary>
        internal static string FunctionKeyTextContinue {
            get {
                return ResourceManager.GetString("FunctionKeyTextContinue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Config..
        /// </summary>
        internal static string FunctionKeyTextEdit {
            get {
                return ResourceManager.GetString("FunctionKeyTextEdit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Execute.
        /// </summary>
        internal static string FunctionKeyTextExecute {
            get {
                return ResourceManager.GetString("FunctionKeyTextExecute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Abort Current Tests].
        /// </summary>
        internal static string FunctionKeyToolTipAbort {
            get {
                return ResourceManager.GetString("FunctionKeyToolTipAbort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Continue].
        /// </summary>
        internal static string FunctionKeyToolTipContinue {
            get {
                return ResourceManager.GetString("FunctionKeyToolTipContinue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Configure Test List].
        /// </summary>
        internal static string FunctionKeyToolTipEdit {
            get {
                return ResourceManager.GetString("FunctionKeyToolTipEdit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Execute Selected Tests].
        /// </summary>
        internal static string FunctionKeyToolTipExecute {
            get {
                return ResourceManager.GetString("FunctionKeyToolTipExecute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Help {
            get {
                object obj = ResourceManager.GetObject("Help", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Available Tests.
        /// </summary>
        internal static string LegendAvailableTests {
            get {
                return ResourceManager.GetString("LegendAvailableTests", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Count.
        /// </summary>
        internal static string LegendCount {
            get {
                return ResourceManager.GetString("LegendCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Item(s).
        /// </summary>
        internal static string LegendItems {
            get {
                return ResourceManager.GetString("LegendItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string MBCaptionError {
            get {
                return ResourceManager.GetString("MBCaptionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Information.
        /// </summary>
        internal static string MBCaptionInformation {
            get {
                return ResourceManager.GetString("MBCaptionInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string MBCaptionWarning {
            get {
                return ResourceManager.GetString("MBCaptionWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to abort the self test sequence..
        /// </summary>
        internal static string MBTAbortSTSequenceFailed {
            get {
                return ResourceManager.GetString("MBTAbortSTSequenceFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding the selected test(s) will exceed the maximum number that are allowed..
        /// </summary>
        internal static string MBTSelfTestsMaxExceeded {
            get {
                return ResourceManager.GetString("MBTSelfTestsMaxExceeded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to send the acknowledge command..
        /// </summary>
        internal static string MBTSendOperatorAcknowledgeFailed {
            get {
                return ResourceManager.GetString("MBTSendOperatorAcknowledgeFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to retrieve the self test results..
        /// </summary>
        internal static string MBTSTResultFailed {
            get {
                return ResourceManager.GetString("MBTSTResultFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target hardware autonomously aborted self test..
        /// </summary>
        internal static string MBTTargetHwAbortSelfTest {
            get {
                return ResourceManager.GetString("MBTTargetHwAbortSelfTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This feature is not available for multiple selections..
        /// </summary>
        internal static string MBTWorksetDefineMultipleSelection {
            get {
                return ResourceManager.GetString("MBTWorksetDefineMultipleSelection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Modify {
            get {
                object obj = ResourceManager.GetObject("Modify", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SelfTest {
            get {
                object obj = ResourceManager.GetObject("SelfTest", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Collecting Results..
        /// </summary>
        internal static string SMCollectingResults {
            get {
                return ResourceManager.GetString("SMCollectingResults", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Complete..
        /// </summary>
        internal static string SMComplete {
            get {
                return ResourceManager.GetString("SMComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Logic Abort..
        /// </summary>
        internal static string SMLogicAbort {
            get {
                return ResourceManager.GetString("SMLogicAbort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Running..
        /// </summary>
        internal static string SMRunning {
            get {
                return ResourceManager.GetString("SMRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User Abort..
        /// </summary>
        internal static string SMUserAbort {
            get {
                return ResourceManager.GetString("SMUserAbort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed.
        /// </summary>
        internal static string TextFailed {
            get {
                return ResourceManager.GetString("TextFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Passed.
        /// </summary>
        internal static string TextPassed {
            get {
                return ResourceManager.GetString("TextPassed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User Defined.
        /// </summary>
        internal static string TextUserDefined {
            get {
                return ResourceManager.GetString("TextUserDefined", resourceCulture);
            }
        }
    }
}

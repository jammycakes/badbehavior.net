﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BadBehavior.Rules {
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
    internal class Explanations {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Explanations() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BadBehavior.Rules.Explanations", typeof(Explanations).Assembly);
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
        ///   Looks up a localized string similar to An invalid request was received from your browser. This may be caused by a malfunctioning proxy server or browser privacy software..
        /// </summary>
        internal static string AcceptMissing {
            get {
                return ResourceManager.GetString("AcceptMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expectation failed. Please retry your request..
        /// </summary>
        internal static string ExpectationFailed {
            get {
                return ResourceManager.GetString("ExpectationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An invalid request was received. You claimed to be a major search engine, but you do not appear to actually be a major search engine..
        /// </summary>
        internal static string FakeSearchEngine {
            get {
                return ResourceManager.GetString("FakeSearchEngine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An invalid request was received. This may be caused by a malfunctioning proxy server. Bypass the proxy server and connect directly, or contact your proxy server administrator..
        /// </summary>
        internal static string Http11Invalid {
            get {
                return ResourceManager.GetString("Http11Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You do not have permission to access this server. If you are using the Opera browser, then Opera must appear in your user agent..
        /// </summary>
        internal static string InvalidMSIEWithTE {
            get {
                return ResourceManager.GetString("InvalidMSIEWithTE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You do not have permission to access this server..
        /// </summary>
        internal static string PermissionDenied {
            get {
                return ResourceManager.GetString("PermissionDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The automated program you are using is not permitted to access this server. Please use a different program or a standard Web browser..
        /// </summary>
        internal static string RangeHeaderZero {
            get {
                return ResourceManager.GetString("RangeHeaderZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You do not have permission to access this server. This may be caused by a malfunctioning proxy server or browser privacy software..
        /// </summary>
        internal static string UserAgentMissing {
            get {
                return ResourceManager.GetString("UserAgentMissing", resourceCulture);
            }
        }
    }
}

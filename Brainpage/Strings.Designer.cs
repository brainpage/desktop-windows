﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Brainpage {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Brainpage.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 Connected 的本地化字符串。
        /// </summary>
        internal static string connected {
            get {
                return ResourceManager.GetString("connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Connecting... 的本地化字符串。
        /// </summary>
        internal static string connecting {
            get {
                return ResourceManager.GetString("connecting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Can&apos;t connect to internet 的本地化字符串。
        /// </summary>
        internal static string disconnected {
            get {
                return ResourceManager.GetString("disconnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Break Meter: 的本地化字符串。
        /// </summary>
        internal static string energyLeft {
            get {
                return ResourceManager.GetString("energyLeft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Exit 的本地化字符串。
        /// </summary>
        internal static string exit {
            get {
                return ResourceManager.GetString("exit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 http://www.brainpage.com/rsi/ 的本地化字符串。
        /// </summary>
        internal static string rootUrl {
            get {
                return ResourceManager.GetString("rootUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Stop Break 的本地化字符串。
        /// </summary>
        internal static string stopBreak {
            get {
                return ResourceManager.GetString("stopBreak", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Go to Charts ... 的本地化字符串。
        /// </summary>
        internal static string viewChart {
            get {
                return ResourceManager.GetString("viewChart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Go to Settings ... 的本地化字符串。
        /// </summary>
        internal static string viewSetting {
            get {
                return ResourceManager.GetString("viewSetting", resourceCulture);
            }
        }
    }
}
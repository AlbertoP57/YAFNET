﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace YAF.RegisterV2 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RegisterV2Soap", Namespace="https://yetanotherforum.net/RegisterV2")]
    public partial class RegisterV2 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LatestInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback LatestVersionOperationCompleted;
        
        private System.Threading.SendOrPostCallback LatestVersionDateOperationCompleted;
        
        private System.Threading.SendOrPostCallback RegisterForumOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public RegisterV2() {
            this.Url = global::YAF.Properties.Settings.Default.YAF_RegisterV2_RegisterV2;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event LatestInfoCompletedEventHandler LatestInfoCompleted;
        
        /// <remarks/>
        public event LatestVersionCompletedEventHandler LatestVersionCompleted;
        
        /// <remarks/>
        public event LatestVersionDateCompletedEventHandler LatestVersionDateCompleted;
        
        /// <remarks/>
        public event RegisterForumCompletedEventHandler RegisterForumCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://yetanotherforum.net/RegisterV2/LatestInfo", RequestNamespace="https://yetanotherforum.net/RegisterV2", ResponseNamespace="https://yetanotherforum.net/RegisterV2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public LatestVersionInformation LatestInfo(long currentVersion, string culture) {
            object[] results = this.Invoke("LatestInfo", new object[] {
                        currentVersion,
                        culture});
            return ((LatestVersionInformation)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginLatestInfo(long currentVersion, string culture, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("LatestInfo", new object[] {
                        currentVersion,
                        culture}, callback, asyncState);
        }
        
        /// <remarks/>
        public LatestVersionInformation EndLatestInfo(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((LatestVersionInformation)(results[0]));
        }
        
        /// <remarks/>
        public void LatestInfoAsync(long currentVersion, string culture) {
            this.LatestInfoAsync(currentVersion, culture, null);
        }
        
        /// <remarks/>
        public void LatestInfoAsync(long currentVersion, string culture, object userState) {
            if ((this.LatestInfoOperationCompleted == null)) {
                this.LatestInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLatestInfoOperationCompleted);
            }
            this.InvokeAsync("LatestInfo", new object[] {
                        currentVersion,
                        culture}, this.LatestInfoOperationCompleted, userState);
        }
        
        private void OnLatestInfoOperationCompleted(object arg) {
            if ((this.LatestInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LatestInfoCompleted(this, new LatestInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://yetanotherforum.net/RegisterV2/LatestVersion", RequestNamespace="https://yetanotherforum.net/RegisterV2", ResponseNamespace="https://yetanotherforum.net/RegisterV2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public byte[] LatestVersion() {
            object[] results = this.Invoke("LatestVersion", new object[0]);
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginLatestVersion(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("LatestVersion", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public long EndLatestVersion(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void LatestVersionAsync() {
            this.LatestVersionAsync(null);
        }
        
        /// <remarks/>
        public void LatestVersionAsync(object userState) {
            if ((this.LatestVersionOperationCompleted == null)) {
                this.LatestVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLatestVersionOperationCompleted);
            }
            this.InvokeAsync("LatestVersion", new object[0], this.LatestVersionOperationCompleted, userState);
        }
        
        private void OnLatestVersionOperationCompleted(object arg) {
            if ((this.LatestVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LatestVersionCompleted(this, new LatestVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://yetanotherforum.net/RegisterV2/LatestVersionDate", RequestNamespace="https://yetanotherforum.net/RegisterV2", ResponseNamespace="https://yetanotherforum.net/RegisterV2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.DateTime LatestVersionDate() {
            object[] results = this.Invoke("LatestVersionDate", new object[0]);
            return ((System.DateTime)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginLatestVersionDate(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("LatestVersionDate", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public System.DateTime EndLatestVersionDate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.DateTime)(results[0]));
        }
        
        /// <remarks/>
        public void LatestVersionDateAsync() {
            this.LatestVersionDateAsync(null);
        }
        
        /// <remarks/>
        public void LatestVersionDateAsync(object userState) {
            if ((this.LatestVersionDateOperationCompleted == null)) {
                this.LatestVersionDateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLatestVersionDateOperationCompleted);
            }
            this.InvokeAsync("LatestVersionDate", new object[0], this.LatestVersionDateOperationCompleted, userState);
        }
        
        private void OnLatestVersionDateOperationCompleted(object arg) {
            if ((this.LatestVersionDateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LatestVersionDateCompleted(this, new LatestVersionDateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://yetanotherforum.net/RegisterV2/RegisterForum", RequestNamespace="https://yetanotherforum.net/RegisterV2", ResponseNamespace="https://yetanotherforum.net/RegisterV2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long RegisterForum(long id, string name, string address) {
            object[] results = this.Invoke("RegisterForum", new object[] {
                        id,
                        name,
                        address});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRegisterForum(long id, string name, string address, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RegisterForum", new object[] {
                        id,
                        name,
                        address}, callback, asyncState);
        }
        
        /// <remarks/>
        public long EndRegisterForum(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void RegisterForumAsync(long id, string name, string address) {
            this.RegisterForumAsync(id, name, address, null);
        }
        
        /// <remarks/>
        public void RegisterForumAsync(long id, string name, string address, object userState) {
            if ((this.RegisterForumOperationCompleted == null)) {
                this.RegisterForumOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRegisterForumOperationCompleted);
            }
            this.InvokeAsync("RegisterForum", new object[] {
                        id,
                        name,
                        address}, this.RegisterForumOperationCompleted, userState);
        }
        
        private void OnRegisterForumOperationCompleted(object arg) {
            if ((this.RegisterForumCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RegisterForumCompleted(this, new RegisterForumCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://yetanotherforum.net/RegisterV2")]
    public partial class LatestVersionInformation {
        
        private System.DateTime dateField;
        
        private string linkField;
        
        private string messageField;
        
        private long versionField;
        
        private bool isWarningField;
        
        /// <remarks/>
        public System.DateTime Date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        public string Link {
            get {
                return this.linkField;
            }
            set {
                this.linkField = value;
            }
        }
        
        /// <remarks/>
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public long Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        public bool IsWarning {
            get {
                return this.isWarningField;
            }
            set {
                this.isWarningField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void LatestInfoCompletedEventHandler(object sender, LatestInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LatestInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LatestInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public LatestVersionInformation Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LatestVersionInformation)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void LatestVersionCompletedEventHandler(object sender, LatestVersionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LatestVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LatestVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void LatestVersionDateCompletedEventHandler(object sender, LatestVersionDateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LatestVersionDateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LatestVersionDateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.DateTime Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.DateTime)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void RegisterForumCompletedEventHandler(object sender, RegisterForumCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RegisterForumCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RegisterForumCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
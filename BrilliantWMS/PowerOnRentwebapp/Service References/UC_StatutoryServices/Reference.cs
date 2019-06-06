﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BrilliantWMS.UC_StatutoryServices {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UC_StatutoryServices.iUC_StatutoryInfo")]
    public interface iUC_StatutoryInfo {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iUC_StatutoryInfo/GetStatutoryListToBind", ReplyAction="http://tempuri.org/iUC_StatutoryInfo/GetStatutoryListToBindResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SP_GetStatutoryDetails_Result[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        BrilliantWMS.UC_StatutoryServices.SP_GetStatutoryDetails_Result[] GetStatutoryListToBind(long paraReferenceID, string paraUserID, string ParaObjectName, long ParaCompanyID, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iUC_StatutoryInfo/FinalSaveToTStatutoryDetails", ReplyAction="http://tempuri.org/iUC_StatutoryInfo/FinalSaveToTStatutoryDetailsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SP_GetStatutoryDetails_Result[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        void FinalSaveToTStatutoryDetails(BrilliantWMS.UC_StatutoryServices.tStatutoryDetail[] ObjStatutory, string paraObjectName, long paraReferenceID, string paraUserID, long paraCompanyID, string[] conn);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SP_GetStatutoryDetails_Result : ComplexObject {
        
        private System.Nullable<int> idField;
        
        private string objectNameField;
        
        private System.Nullable<long> referenceIDField;
        
        private System.Nullable<long> statutoryIDField;
        
        private string nameField;
        
        private string statutoryValueField;
        
        private string activeField;
        
        private System.Nullable<long> sequenceField;
        
        private string createdByField;
        
        private System.Nullable<System.DateTime> createdDateField;
        
        private string lastEditByField;
        
        private System.Nullable<System.DateTime> lastEditDateField;
        
        private System.Nullable<long> companyIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public System.Nullable<int> ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
                this.RaisePropertyChanged("ID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ObjectName {
            get {
                return this.objectNameField;
            }
            set {
                this.objectNameField = value;
                this.RaisePropertyChanged("ObjectName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public System.Nullable<long> ReferenceID {
            get {
                return this.referenceIDField;
            }
            set {
                this.referenceIDField = value;
                this.RaisePropertyChanged("ReferenceID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public System.Nullable<long> StatutoryID {
            get {
                return this.statutoryIDField;
            }
            set {
                this.statutoryIDField = value;
                this.RaisePropertyChanged("StatutoryID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
                this.RaisePropertyChanged("Name");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string StatutoryValue {
            get {
                return this.statutoryValueField;
            }
            set {
                this.statutoryValueField = value;
                this.RaisePropertyChanged("StatutoryValue");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string Active {
            get {
                return this.activeField;
            }
            set {
                this.activeField = value;
                this.RaisePropertyChanged("Active");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=7)]
        public System.Nullable<long> Sequence {
            get {
                return this.sequenceField;
            }
            set {
                this.sequenceField = value;
                this.RaisePropertyChanged("Sequence");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string CreatedBy {
            get {
                return this.createdByField;
            }
            set {
                this.createdByField = value;
                this.RaisePropertyChanged("CreatedBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=9)]
        public System.Nullable<System.DateTime> CreatedDate {
            get {
                return this.createdDateField;
            }
            set {
                this.createdDateField = value;
                this.RaisePropertyChanged("CreatedDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string LastEditBy {
            get {
                return this.lastEditByField;
            }
            set {
                this.lastEditByField = value;
                this.RaisePropertyChanged("LastEditBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=11)]
        public System.Nullable<System.DateTime> LastEditDate {
            get {
                return this.lastEditDateField;
            }
            set {
                this.lastEditDateField = value;
                this.RaisePropertyChanged("LastEditDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=12)]
        public System.Nullable<long> CompanyID {
            get {
                return this.companyIDField;
            }
            set {
                this.companyIDField = value;
                this.RaisePropertyChanged("CompanyID");
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SP_GetStatutoryDetails_Result))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public abstract partial class ComplexObject : StructuralObject {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityObject))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(tStatutoryDetail))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComplexObject))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SP_GetStatutoryDetails_Result))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public abstract partial class StructuralObject : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityReference))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityReferenceOfmStatutory))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public abstract partial class RelatedEnd : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityReferenceOfmStatutory))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public abstract partial class EntityReference : RelatedEnd {
        
        private EntityKey entityKeyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public EntityKey EntityKey {
            get {
                return this.entityKeyField;
            }
            set {
                this.entityKeyField = value;
                this.RaisePropertyChanged("EntityKey");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class EntityKey : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string entitySetNameField;
        
        private string entityContainerNameField;
        
        private EntityKeyMember[] entityKeyValuesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string EntitySetName {
            get {
                return this.entitySetNameField;
            }
            set {
                this.entitySetNameField = value;
                this.RaisePropertyChanged("EntitySetName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string EntityContainerName {
            get {
                return this.entityContainerNameField;
            }
            set {
                this.entityContainerNameField = value;
                this.RaisePropertyChanged("EntityContainerName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=2)]
        public EntityKeyMember[] EntityKeyValues {
            get {
                return this.entityKeyValuesField;
            }
            set {
                this.entityKeyValuesField = value;
                this.RaisePropertyChanged("EntityKeyValues");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class EntityKeyMember : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string keyField;
        
        private object valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
                this.RaisePropertyChanged("Key");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public object Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class EntityReferenceOfmStatutory : EntityReference {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(tStatutoryDetail))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public abstract partial class EntityObject : StructuralObject {
        
        private EntityKey entityKeyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public EntityKey EntityKey {
            get {
                return this.entityKeyField;
            }
            set {
                this.entityKeyField = value;
                this.RaisePropertyChanged("EntityKey");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class tStatutoryDetail : EntityObject {
        
        private long idField;
        
        private string objectNameField;
        
        private long referenceIDField;
        
        private System.Nullable<long> statutoryIDField;
        
        private string statutoryValueField;
        
        private string activeField;
        
        private string createdByField;
        
        private System.DateTime createdDateField;
        
        private string lastEditByField;
        
        private System.Nullable<System.DateTime> lastEditDateField;
        
        private long companyIDField;
        
        private System.Nullable<int> sequenceField;
        
        private System.Nullable<long> approverIDField;
        
        private EntityReferenceOfmStatutory mStatutoryReferenceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public long ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
                this.RaisePropertyChanged("ID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ObjectName {
            get {
                return this.objectNameField;
            }
            set {
                this.objectNameField = value;
                this.RaisePropertyChanged("ObjectName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public long ReferenceID {
            get {
                return this.referenceIDField;
            }
            set {
                this.referenceIDField = value;
                this.RaisePropertyChanged("ReferenceID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public System.Nullable<long> StatutoryID {
            get {
                return this.statutoryIDField;
            }
            set {
                this.statutoryIDField = value;
                this.RaisePropertyChanged("StatutoryID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string StatutoryValue {
            get {
                return this.statutoryValueField;
            }
            set {
                this.statutoryValueField = value;
                this.RaisePropertyChanged("StatutoryValue");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string Active {
            get {
                return this.activeField;
            }
            set {
                this.activeField = value;
                this.RaisePropertyChanged("Active");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string CreatedBy {
            get {
                return this.createdByField;
            }
            set {
                this.createdByField = value;
                this.RaisePropertyChanged("CreatedBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public System.DateTime CreatedDate {
            get {
                return this.createdDateField;
            }
            set {
                this.createdDateField = value;
                this.RaisePropertyChanged("CreatedDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string LastEditBy {
            get {
                return this.lastEditByField;
            }
            set {
                this.lastEditByField = value;
                this.RaisePropertyChanged("LastEditBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=9)]
        public System.Nullable<System.DateTime> LastEditDate {
            get {
                return this.lastEditDateField;
            }
            set {
                this.lastEditDateField = value;
                this.RaisePropertyChanged("LastEditDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public long CompanyID {
            get {
                return this.companyIDField;
            }
            set {
                this.companyIDField = value;
                this.RaisePropertyChanged("CompanyID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=11)]
        public System.Nullable<int> Sequence {
            get {
                return this.sequenceField;
            }
            set {
                this.sequenceField = value;
                this.RaisePropertyChanged("Sequence");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=12)]
        public System.Nullable<long> ApproverID {
            get {
                return this.approverIDField;
            }
            set {
                this.approverIDField = value;
                this.RaisePropertyChanged("ApproverID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public EntityReferenceOfmStatutory mStatutoryReference {
            get {
                return this.mStatutoryReferenceField;
            }
            set {
                this.mStatutoryReferenceField = value;
                this.RaisePropertyChanged("mStatutoryReference");
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface iUC_StatutoryInfoChannel : BrilliantWMS.UC_StatutoryServices.iUC_StatutoryInfo, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class iUC_StatutoryInfoClient : System.ServiceModel.ClientBase<BrilliantWMS.UC_StatutoryServices.iUC_StatutoryInfo>, BrilliantWMS.UC_StatutoryServices.iUC_StatutoryInfo {
        
        public iUC_StatutoryInfoClient() {
        }
        
        public iUC_StatutoryInfoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public iUC_StatutoryInfoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public iUC_StatutoryInfoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public iUC_StatutoryInfoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BrilliantWMS.UC_StatutoryServices.SP_GetStatutoryDetails_Result[] GetStatutoryListToBind(long paraReferenceID, string paraUserID, string ParaObjectName, long ParaCompanyID, string[] conn) {
            return base.Channel.GetStatutoryListToBind(paraReferenceID, paraUserID, ParaObjectName, ParaCompanyID, conn);
        }
        
        public void FinalSaveToTStatutoryDetails(BrilliantWMS.UC_StatutoryServices.tStatutoryDetail[] ObjStatutory, string paraObjectName, long paraReferenceID, string paraUserID, long paraCompanyID, string[] conn) {
            base.Channel.FinalSaveToTStatutoryDetails(ObjStatutory, paraObjectName, paraReferenceID, paraUserID, paraCompanyID, conn);
        }
    }
}

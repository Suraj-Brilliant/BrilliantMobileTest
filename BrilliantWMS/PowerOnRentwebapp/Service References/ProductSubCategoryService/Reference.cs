﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BrilliantWMS.ProductSubCategoryService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProductSubCategoryService.iProductSubCategoryMaster")]
    public interface iProductSubCategoryMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetProductSubCategoryList", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetProductSubCategoryListResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        BrilliantWMS.ProductSubCategoryService.mProductSubCategory[] GetProductSubCategoryList(string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/InsertmProductSubCategory", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/InsertmProductSubCategoryResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        int InsertmProductSubCategory(BrilliantWMS.ProductSubCategoryService.mProductSubCategory prdSubCategory, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/updatemProductSubCategory", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/updatemProductSubCategoryResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        int updatemProductSubCategory(BrilliantWMS.ProductSubCategoryService.mProductSubCategory updateprdSubCategory, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetProductSubCategoryListByID", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetProductSubCategoryListByIDRespons" +
            "e")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        BrilliantWMS.ProductSubCategoryService.mProductSubCategory GetProductSubCategoryListByID(int ProductSubCategoryId, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/checkDuplicateRecord", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/checkDuplicateRecordResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        string checkDuplicateRecord(string productSubCategoryName, int prdCategoryID, long CustomerID, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/checkDuplicateRecordEdit", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/checkDuplicateRecordEditResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        string checkDuplicateRecordEdit(int productSubCategoryID, string productSubCategoryName, int productCategoryID, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetPrdSubCategoryRecordToBind", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetPrdSubCategoryRecordToBindRespons" +
            "e")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        System.Data.DataSet GetPrdSubCategoryRecordToBind(string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetProductSubCategoryByProductCatego" +
            "ryID", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetProductSubCategoryByProductCatego" +
            "ryIDResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        BrilliantWMS.ProductSubCategoryService.vGetProductSubCagetoryList[] GetProductSubCategoryByProductCategoryID(long productCategoryID, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetCategoryListByCustomer", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetCategoryListByCustomerResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        System.Data.DataSet GetCategoryListByCustomer(long CustomerID, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetSubcategorylist", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetSubcategorylistResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        System.Data.DataSet GetSubcategorylist(long ProductCategoryID, string[] conn);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/iProductSubCategoryMaster/GetChannelList", ReplyAction="http://tempuri.org/iProductSubCategoryMaster/GetChannelListResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RelatedEnd))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(StructuralObject))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(mProductSubCategory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(EntityKeyMember[]))]
        System.Data.DataSet GetChannelList(long CustomerID, string[] conn);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class mProductSubCategory : EntityObject {
        
        private long idField;
        
        private long productCategoryIDField;
        
        private string nameField;
        
        private System.Nullable<long> sequenceField;
        
        private string activeField;
        
        private string createdByField;
        
        private System.DateTime creationDateField;
        
        private string lastModifiedByField;
        
        private System.Nullable<System.DateTime> lastModifiedDateField;
        
        private long companyidField;
        
        private System.Nullable<long> customerIDField;
        
        private EntityReferenceOfmProductCategory mProductCategoryReferenceField;
        
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
        public long ProductCategoryID {
            get {
                return this.productCategoryIDField;
            }
            set {
                this.productCategoryIDField = value;
                this.RaisePropertyChanged("ProductCategoryID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public System.DateTime CreationDate {
            get {
                return this.creationDateField;
            }
            set {
                this.creationDateField = value;
                this.RaisePropertyChanged("CreationDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string LastModifiedBy {
            get {
                return this.lastModifiedByField;
            }
            set {
                this.lastModifiedByField = value;
                this.RaisePropertyChanged("LastModifiedBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=8)]
        public System.Nullable<System.DateTime> LastModifiedDate {
            get {
                return this.lastModifiedDateField;
            }
            set {
                this.lastModifiedDateField = value;
                this.RaisePropertyChanged("LastModifiedDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public long Companyid {
            get {
                return this.companyidField;
            }
            set {
                this.companyidField = value;
                this.RaisePropertyChanged("Companyid");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=10)]
        public System.Nullable<long> CustomerID {
            get {
                return this.customerIDField;
            }
            set {
                this.customerIDField = value;
                this.RaisePropertyChanged("CustomerID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public EntityReferenceOfmProductCategory mProductCategoryReference {
            get {
                return this.mProductCategoryReferenceField;
            }
            set {
                this.mProductCategoryReferenceField = value;
                this.RaisePropertyChanged("mProductCategoryReference");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class EntityReferenceOfmProductCategory : EntityReference {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityReferenceOfmProductCategory))]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityReference))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityReferenceOfmProductCategory))]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EntityObject))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(vGetProductSubCagetoryList))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(mProductSubCategory))]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(vGetProductSubCagetoryList))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(mProductSubCategory))]
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
    public partial class vGetProductSubCagetoryList : EntityObject {
        
        private System.Nullable<long> idField;
        
        private long productCategoryIDField;
        
        private string productSubCategoryField;
        
        private System.Nullable<long> sequenceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public System.Nullable<long> ID {
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
        public long ProductCategoryID {
            get {
                return this.productCategoryIDField;
            }
            set {
                this.productCategoryIDField = value;
                this.RaisePropertyChanged("ProductCategoryID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ProductSubCategory {
            get {
                return this.productSubCategoryField;
            }
            set {
                this.productSubCategoryField = value;
                this.RaisePropertyChanged("ProductSubCategory");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public System.Nullable<long> Sequence {
            get {
                return this.sequenceField;
            }
            set {
                this.sequenceField = value;
                this.RaisePropertyChanged("Sequence");
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface iProductSubCategoryMasterChannel : BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class iProductSubCategoryMasterClient : System.ServiceModel.ClientBase<BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMaster>, BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMaster {
        
        public iProductSubCategoryMasterClient() {
        }
        
        public iProductSubCategoryMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public iProductSubCategoryMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public iProductSubCategoryMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public iProductSubCategoryMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BrilliantWMS.ProductSubCategoryService.mProductSubCategory[] GetProductSubCategoryList(string[] conn) {
            return base.Channel.GetProductSubCategoryList(conn);
        }
        
        public int InsertmProductSubCategory(BrilliantWMS.ProductSubCategoryService.mProductSubCategory prdSubCategory, string[] conn) {
            return base.Channel.InsertmProductSubCategory(prdSubCategory, conn);
        }
        
        public int updatemProductSubCategory(BrilliantWMS.ProductSubCategoryService.mProductSubCategory updateprdSubCategory, string[] conn) {
            return base.Channel.updatemProductSubCategory(updateprdSubCategory, conn);
        }
        
        public BrilliantWMS.ProductSubCategoryService.mProductSubCategory GetProductSubCategoryListByID(int ProductSubCategoryId, string[] conn) {
            return base.Channel.GetProductSubCategoryListByID(ProductSubCategoryId, conn);
        }
        
        public string checkDuplicateRecord(string productSubCategoryName, int prdCategoryID, long CustomerID, string[] conn) {
            return base.Channel.checkDuplicateRecord(productSubCategoryName, prdCategoryID, CustomerID, conn);
        }
        
        public string checkDuplicateRecordEdit(int productSubCategoryID, string productSubCategoryName, int productCategoryID, string[] conn) {
            return base.Channel.checkDuplicateRecordEdit(productSubCategoryID, productSubCategoryName, productCategoryID, conn);
        }
        
        public System.Data.DataSet GetPrdSubCategoryRecordToBind(string[] conn) {
            return base.Channel.GetPrdSubCategoryRecordToBind(conn);
        }
        
        public BrilliantWMS.ProductSubCategoryService.vGetProductSubCagetoryList[] GetProductSubCategoryByProductCategoryID(long productCategoryID, string[] conn) {
            return base.Channel.GetProductSubCategoryByProductCategoryID(productCategoryID, conn);
        }
        
        public System.Data.DataSet GetCategoryListByCustomer(long CustomerID, string[] conn) {
            return base.Channel.GetCategoryListByCustomer(CustomerID, conn);
        }
        
        public System.Data.DataSet GetSubcategorylist(long ProductCategoryID, string[] conn) {
            return base.Channel.GetSubcategorylist(ProductCategoryID, conn);
        }
        
        public System.Data.DataSet GetChannelList(long CustomerID, string[] conn) {
            return base.Channel.GetChannelList(CustomerID, conn);
        }
    }
}

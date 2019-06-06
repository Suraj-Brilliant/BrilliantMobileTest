using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;
using System.Data;

using Domain.Server;

namespace Interface.Company
{

    [ServiceContract]
    public partial interface iCompanySetup
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mCompany> GetGroupCompany(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long InsertmCompany(mCompany Company, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int UpdateCompany(mCompany UpdateCompany, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecord(string CompanyName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecordEdit(string CompanyName, int CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mCompany GetCompanyById(Int64 CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int InsertmCompanyRegistration(tCompanyRegistrationDetail Company, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int UpdateCompanyRegistration(tCompanyRegistrationDetail UpdateCompany, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<vGetCompanyDetail> GetCompanyList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tCompanyRegistrationDetail GetCompanyRegisById(Int64 CompanyID, string[] conn);
                
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string ChechkCompanyServerNameDuplicate(string ServerName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string ChechkCompanyDatabaseNameDuplicate(string DataBaseName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string ChechkCompanyNameDuplicate(string CompanyName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string ChechkCompanyServer_DataBaseNameDuplicate(string ServerName, string Database, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void RestoreDatabase(string DataBaseName, string[] conn);

        # region New Customer or company code for GWC project

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCompanyName(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDepartmentListforgrid(long ParentID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void Savecustdeptinfo(long ParentID, string Territory, long Sequence, string StoreCode, long ApprovalLevel, string AutoCancel, long cancelDays, string CreatedBy, DateTime CreationDate, string Active, string ApprovalRem, long ApproRemSchedul, string AutoCancRen, long AutoRemSchedule, bool GwcDeliveries, bool ECommerce, string OrderFormat, long MaxDeliveryDays, long AddressType, bool PriceChange, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDepartmentToEdit(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateDeptInfo(long ID, string Territory, string StoreCode, long ApprovalLevel, string AutoCancel, long cancelDays, string CreatedBy, DateTime CreationDate, string Active, string ApprovalRem, long ApproRemSchedul, string AutoCancRen, long AutoRemSchedule, bool GwcDeliveries, bool ECommerce, string OrderFormat, long MaxDeliveryDays, long FinApproverID, long AddressType, bool PriceChange, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long chkDeptDuplicate(string Territory, string StoreCode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDeptListWithSLA(long ParentID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetLocationList(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveLocContactInContact(string ObjectName, long ReferenceID, long CustomerHeadID, long Sequence, string Name, string EmailID, string MobileNo, long ContactTypeID, string Active, string CreatedBy, DateTime CreationDate, long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void SaveEditLocation(long ID, long CompanyID, long ReferenceID, string LocationCode, string AddressLine1, string AddressLine2, string County, string State, string City, string zipcode, string landmark, string FaxNo, string Active, string ContactName, string ContactEmail, string LocationName, long MobileNo, string CreatedBy, DateTime CreationDate, string hdnstate, long ShippingID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateLocationDetails(long ID, long CompanyID, long ReferenceID, string LocationCode, string AddressLine1, string AddressLine2, string County, string State, string City, string zipcode, string landmark, string FaxNo, string Active, string ContactName, string ContactEmail, long MobileNo, string CreatedBy, DateTime CreationDate, string hdnstate, string LocationName, long ShippingID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string getFinApprovername(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAddressType(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long chkecommerceduplicate(long ParentID, long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertIntoDeptPayment(long DeptID, long PMethodID, int Sequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void DeleteRecordWithZeroQty(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void RemoveDeptPMethod(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateDeptPaymentMethod(long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCostCenterList(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void DeleteZeroCompanyIDCostCenter(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void SaveCostCenter(string CenterName, string Code, long ApproverID, long CompanyID, string Remark, DateTime CreationDate, long CreatedBy, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void RemoveCostCenter(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateCostCenterCmpanyID(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long Duplicatecostcenter(string CenterName, string Code, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long CheckDuplicatePMethod(long PMethodID, long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckLocationIDForAssignedUser(long Location, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetContactIdasShippingId(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateContacttableDetail(string Name, string EmailID, string MobileNo, long ID, string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tContactPersonDetail> GetContactPersonListDeptWise(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         DataSet GetContactPersonLocList(long CompanyID, string[] conn);
        #endregion

       # region new code for BrilliantWMS Project 
        
        // Get Customer List

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_GetCustomerDetails>GetCustomerList(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_GetCustomerDetails> GetSuperAdminCustomerList(string[] conn);

        // Get Channel List
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetChannelDetails> GetChannelList(string[] conn);

        // Get Vendor List
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetVendorDetails> GetVendorList(string[] conn);

        // Get Client List
         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         List<V_WMS_GetClientDetails> GetClientList(string[] conn);

        // get rateCard Details
         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         List<V_WMS_GetRateCardDetails> GetRateCardDetails(string[] conn);

        // Get Aggregator List
         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         DataSet GetAggregatorList(string[] conn);

        // Get TermnCondition List
         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         DataSet GetTermsnConditionList(string[] conn);

        // Code to insert into Company Configuration
         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         long InsertConfiguration(mConfiguration company, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         List<mDropdownValue> GetContactTypeList(string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         mConfiguration GetCompanyConfiguration(Int64 CompanyID, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         int UpdateCompanyCongig(mConfiguration UpdateCompany, string[] conn);



        #region Code for Customer Master.

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         List<mCompany> GetCompanyDropDown(string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         long InsertCustomer(mCustomer Customer, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         mCustomer GetCustomerbyID(Int64 CustomerID, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         int UpdateCustomer(mCustomer UpdateCustomer, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         List<V_WMS_GetRateCardDetails> GetCustomerRateCard(Int64 CustomerID, string Type, string[] conn);

        #endregion

        #region Code for Channel Master

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetChannelName(string Parameter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveChannelDetail(mChannel Channel, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mChannel GetChannelByChanID(long ChannelID, string[] conn);
        #endregion

        #region Code for Aggregator Master
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveAggregatorMaster(mAgreegator Aggrm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mAgreegator GetAggregatorMasterbyID(long AggregatID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveAggreegatorAPI(mAgreegatorAPI AggriAPI, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mAgreegatorAPI GetAggreegatorAPIbyID(long AggregatAPIID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mAgreegatorAPI> GetAPIListByID(long AggreID, string[] conn);

        #endregion

        #region Code for Rate card master

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetRateTypeDropdown(string Parameter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveRateCardMaster(mRateCard ratecard, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mRateCard GetRateCardByID(long RateCardID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetRateCardDetails> GetVendorRateByVendorID(long VendorID, string type, string[] conn);
        #endregion
       # endregion


    }
}

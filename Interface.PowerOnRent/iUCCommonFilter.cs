using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;

using System.Data;
using System.Data.SqlClient;


namespace Interface.PowerOnRent
{
    [ServiceContract]
    public partial interface iUCCommonFilter
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetSiteNameByUserID(long uid, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetAllSites(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<v_GetEngineDetails> GetEngineOfSite(string SId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mStatu> GetStatusListByOjbect(string ObjectName, string Remark, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<vGetUserProfileByUserID> GetUserListBySiteID(long siteID, string[] conn);

        //[OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        //List<GetProductDetail> GetProductOfEngine(string EngId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetReportData(string SiteId, string EngLst, string PrdLst, string frmdt, string todt, string objectname, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetRequisitionData(string SId, string ReqLst, string PLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetIssueData(string SiteId, string IssueLst, string PLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetReceiptData(string SiteId, string ReceiptLst, string PrdLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductofRequest(string reqid, string filter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductofIssue(string issueid, string filter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductofReceipt(string receiptID, string filter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetPenComRequestData(string SiteId, string fdt, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductOfSelectedEngine(string EngId, string filter, string frmdt, string todt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetWeeklyConsumption(string Site, string ftd, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetConsumableStock(string Category, string Site, string fdt, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllRequisitionData(string SId, string fdt, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllReportData(string SiteId, string frmdt, string todt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllIssueData(string SiteId, string fdt, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllReceiptData(string SiteId, string fdt, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<GetPrdDetail> AllProductOnSite(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductBalanceOfSite(string Site, string PrdList, string AllPrd, string excludeZero, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllRequisitionDataAllRequest(string SId, string ReqLst, string PLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllRequisitionDataAllPrd(string SId, string ReqLst, string PLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetIssueDataAllIssue(string SiteId, string IssueLst, string PLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetIssueDataAllPrd(string SiteId, string IssueLst, string PLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetReceiptDataAllReceipt(string SiteId, string ReceiptLst, string PrdLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetReceiptDataAllPrd(string SiteId, string ReceiptLst, string PrdLst, string fdt, string tdt, string objectnm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetEngineCount(string refId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetEngineCountAll(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetSiteNameByUserID_IssueID(long IssueID, long uid, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetSiteID(long IssueID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetSiteNameByUserID_Transfer(long uid, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetToSiteName_Transfer(long FrmSiteID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTransferRptData(string fromSiteID, string toSiteID, string fdt, string tdt, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAssetRptData(string toSiteID, string fromSiteID, string fdt, string tdt, string[] conn);

        #region Report_DropDown

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mCompany> GetCompanyName(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetDepartmentList(int ComapnyID, string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetAllDepartmentList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mBOMHeader> GetGroupSet(int CompanyID, int DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mBOMHeader> GetAllGroupsetList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mBOMHeader> GetGroupSetByDept(int DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mBOMHeader> GetGroupSetByCompany(int CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mStatu> GetStatus(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GetUserInformation> GetUser(int CompanyID, int DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetRequestDetails(string CompanyID, string DeptID, string status, string UserID, string FromDate, string ToDate, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GetSKUAndRequestDetails> AllSKUDetailsByRequest(long CompanyID, long DeptID, string status, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet SKUDetailsBySelectedRequestID(string selectedRequest, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderData(string FromDate, string ToDate, string CompanyID, string DeptID, string Status, string User, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDataBySelectedOrder(string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDataBySelectedSKU(string SelectedKU, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDataBySelectedSKUAndOrder(string SelectedProduct, string SelectedOrder, string[] conn);

        #endregion
        //Reports

        //[OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        //DataSet GetPaymentMethod(string[] conn);



        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllSKUData(string CompanyID, string DeptID, string GroupSetID, string Image, string WithZero, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllSKUDataSelectedRow(string SelectedProducts, string Image, string WithZero, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllUserDataSelectedRow(string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllUserData(string CompanyID, string DeptID, string Role, string Active, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSku(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrder(string[] conn);




        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSKUDetailsReprtData(string CompanyID, string DeptID, string GroupSetID, string[] conn);


        #region PartRequest_DropDown
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tContactPersonDetail> GetContactPersonListDeptWise(long Dept, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tContactPersonDetail> GetContactPersonList(long Dept, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tContactPersonDetail> GetContactPerson2List(long Dept, long Cont1, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tAddress> GetDeptAddressList(long Dept, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mCompany> GetUserCompanyName(long UID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetUserCompanyID(long UID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetDepartmentListUserWise(int UID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetSiteIdOfUser(long UID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetCompanyIDFromSiteID(long SiteID, string[] conn);
        #endregion

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GWC_SkuFilter> SkuLstRpt(string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet SkulistReport(string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string SelectedImage, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSKUDetailsSelectedRow(string SelectedProducts, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBOMDetailsReprtData(string CompanyID, string DeptID, string GroupSetID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBOMDetailsSelectedRow(string SelectedProducts, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUsrLst(string CompanyID, string DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDetails(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string SelectedStatus, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDataSelectedRow(string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GWC_GetUserInfo_Result> GetUsrLst1(string CompanyID, string DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetOrderDetailsReprtData(string SelectedCompany, string SelectedDepartment, string SelectedUser, string SelectedStatus, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDetailsDataSelectedRow(string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GWC_GetUserInfoRoleWise_Result> GetUserInformation(string CompanyID, string DeptID, string RoleID, string Active, string[] conn);



        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderLeadReprtData(string SelectedCompany, string SelectedDepartment, string SelectedStatus, string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderSelectedOrderRpt(string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditAllPrd(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditSelectedPrd(string FrmDt, string Todt, string SelectedProducts, string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditFail(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tContactPersonDetail GetContactPersonDetailsByID(long ContactID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tAddress GetAddressDetailsByID(long AdrsID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void EditContactPerson(tContactPersonDetail ConPerD, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void AddIntotContactpersonDetail(tContactPersonDetail ConPerD, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void EditAddress(tAddress Adrs, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void AddIntotAddress(tAddress Adrs, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string getContact1NameByID(long EdtCon1, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string getContact2NamesByIDs(string EdtCon2, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetAddressLineByAdrsID(long EdtAddress, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetStateList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditFailPrdLst(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditFailSelectedProduct(string FrmDt, string Todt, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string SelectedUser, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSKUTransaction(string SelectedProducts, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUserTransaction(string UserSelectedRec, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet SkulistReportSearch(string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string SelectedImage, string SearchedValue, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditAllPrdSearched(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string SearchedValue, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImageAuditFailPrdLstSearched(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string SearchedValue, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GWC_GetUserInfoRoleWise_Result> GetUserInformationSearched(string CompanyID, string DeptID, string RoleID, string Active, string Searchedvalue, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mCompany> GetUserCompanyNameNEW(long UID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTerritory> GetAddedDepartmentList(int ComapnyID, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAddedDepartmentListDS(int ComapnyID, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllDriverList(string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDelivery(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedDriver, string SelectedPaymentMode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllOrderDeliveryDataSelectedRow(string SelectedOrder, string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllSlaData(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string Status, string SelectedDriver, string SelectedDeliveryType, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllSlaDataSelectedRow(string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetToTalDeliveryVSTotalReq(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetToTalDeliveryVSTotalReqDataSelectedRow(string FrmDt, string Todt, string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetAdrsType(long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tAddress> GetDeptAddressListAdrsType(long CompanyID, long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tAddress> GetUserLocation(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet AllOrderReports(string FrmDt, string Todt, string SelectedCompany, string SelectedDepartment, string SelectedUser, string SelectedStatus, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GetUserLocation> GetLocationOfUser(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        VW_GetUserLocation GetLocationDetailsByID(long AdrsID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void EditLocation(tContactPersonDetail Con, long AdrsID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetUsers_Result> GetUsersDepartmentWise(string CompanyID, string DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetVendors(string CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetPurchaseOrder(string FrmDt, string Todt, string selectedvender, string selectedstatus, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSelectedPurchaseOrderDetails(string SelectedPurchaseOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllPurchaseOrderDetails(string FrmDt, string Todt, string selectedvender, string selectedstatus, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllPurchaseOrderReport(string FrmDt, string Todt, string selectedvender, string selectedstatus, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllPurchaseOrderSelectedRowReport(string FrmDt, string Todt, string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetReceivableOrder(string FrmDt, string Todt, string selectedvender, string[] conn);

        #region PO

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tAddress> GetWarehouseAddressList(long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tContactPersonDetail> GetContactPersonListVendorWise(long VendorID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<vGetUserProfileByUserID> GetUserListByWarehouseID(long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<vGetUserProfileByUserID> GetUserListByWarehouse(long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tContactPersonDetail> GetContactPersonListClientWise(long ClntID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetPOList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetGRNList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllGRNList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetGRNDetail(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetqcList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllqcList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetqcDetail(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetputinList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllputinList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetputinDetail(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetClients(string CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSalesOrderList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllsalesorderList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetsalesorderDetail(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetpickupList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAllpickupList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetpickupDetail(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetdispatchList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAlldispatchList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetdispatchDetail(string[] conn);
        #endregion
    }
}
